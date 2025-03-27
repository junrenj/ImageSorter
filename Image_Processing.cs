using System.Numerics;

namespace ImageSorter
{
    public class Image_Processing
    {
        //基础数据
        public string originalImagePath;
        public string fileName;
        public Bitmap? lowRes_Img;
        private Bitmap? gray_Img;
        private PictureBox_Advance? pictureBox;
        public E_ImgSize imgSize;
        // 通用的distance
        public double distance;
        // Hash 算法
        public ulong hash;


        public Image_Processing(string path, PictureBox_Advance p, E_ImgSize imgSize)
        {
            originalImagePath = path;
            fileName = Path.GetFileNameWithoutExtension(path);
            pictureBox = p;
            this.imgSize = imgSize;
        }


        /// <summary>
        /// 降采样(等宽高)
        /// </summary>
        /// <param name="img_size">宽高长度</param>
        public void GenerateLowResolutionMap(int img_size = GlobalConstants.size_HashDownSample, bool usePicturebox = true)
        {
            lowRes_Img?.Dispose();
            lowRes_Img = null;
            Bitmap original;
            if (usePicturebox)
                original = new Bitmap(pictureBox.Image);
            else
                original = new Bitmap(originalImagePath);
            img_size = GlobalConstants.size_HashDownSample;
            lowRes_Img = new Bitmap(img_size, img_size);

            // 使用Graphics对象进行绘制，缩放图像
            using (Graphics g = Graphics.FromImage(lowRes_Img))
            {
                // 设置图形质量
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, img_size, img_size);
            }

            original.Dispose();
        }

        /// <summary>
        /// 降采样(不等宽高)
        /// </summary>
        /// <param name="img_size_x">宽度</param>
        /// <param name="img_size_y">高度</param>
        public void GenerateLowResolutionMap(int img_size_x, int img_size_y, bool usePicturebox = true)
        {
            lowRes_Img?.Dispose();
            lowRes_Img = null;
            Bitmap original;
            if (usePicturebox)
                original = new Bitmap(pictureBox.Image);
            else
                original = new Bitmap(originalImagePath);
            lowRes_Img = new Bitmap(img_size_x, img_size_y);

            // 使用Graphics对象进行绘制，缩放图像
            using (Graphics g = Graphics.FromImage(lowRes_Img))
            {
                // 设置图形质量
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, img_size_x, img_size_y);
            }

            // 同时算好一个灰度图
            ConvertToGray(img_size_x, img_size_y);

            original.Dispose();
        }

        #region 哈希算法
        /// <summary>
        /// 均值哈希
        /// </summary>
        public ulong GetImageAHash()
        {
            GenerateLowResolutionMap();
            ConvertToGray();
            // 计算灰度均值
            hash = 0;
            int total = 0;
            int[] pixels = new int[64];

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int gray = gray_Img.GetPixel(x, y).R;
                    pixels[y * 8 + x] = gray;
                    total += gray;
                }
            }

            int avg = total / 64;

            for (int i = 0; i < 64; i++)
            {
                if (pixels[i] > avg)
                {
                    hash |= (1UL << i);
                }
            }

            return hash;
        }

        /// <summary>
        /// 差值哈希
        /// </summary>
        /// <returns></returns>
        public ulong GetImageDHash()
        {
            // 重新计算灰度图 8x8 -> 9x8
            GenerateLowResolutionMap(9,8);

            // 接着算好一个灰度图
            ConvertToGray(9,8);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++) // 遍历 8x8 相邻列
                {
                    int leftPixel = gray_Img.GetPixel(x, y).R;   // 获取左边像素灰度值
                    int rightPixel = gray_Img.GetPixel(x + 1, y).R; // 获取右边像素灰度值

                    if (leftPixel > rightPixel) // 左 > 右 置 1，否则置 0
                        hash |= (1UL << (63 - (y * 8 + x))); // 最高位先填充
                }
            }
            return hash;
        }

        /// <summary>
        /// 计算汉明距离
        /// </summary>
        /// <param name="hash_t"></param>
        public void HammingDistance(ulong hash_t)
        {
            ulong xor = hash_t ^ hash; // 异或运算，找出不同位
            int distance = 0;
            while (xor > 0)
            {
                distance += (int)(xor & 1); // 统计不同位的个数
                xor >>= 1;
            }
            this.distance = (double)distance;
        }

        /// <summary>
        /// 转换为灰度图 等宽高
        /// </summary>
        /// <param name="size"></param>
        private void ConvertToGray(int size = GlobalConstants.size_HashDownSample)
        {
            gray_Img?.Dispose();
            gray_Img = null;
            gray_Img = new Bitmap(size, size);
            for (int y = 0; y < lowRes_Img.Height; y++)
            {
                for (int x = 0; x < lowRes_Img.Width; x++)
                {
                    Color pixel = lowRes_Img.GetPixel(x, y);
                    int grayValue = (int)(pixel.R * 0.299f + pixel.G * 0.587f + pixel.B * 0.114f);
                    gray_Img.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }
        }

        /// <summary>
        /// 转换成灰度图 不等宽高
        /// </summary>
        /// <param name="size_x"></param>
        /// <param name="size_y"></param>
        private void ConvertToGray(int size_x, int size_y)
        {
            gray_Img?.Dispose();
            gray_Img = null;
            gray_Img = new Bitmap(size_x, size_y);
            for (int y = 0; y < lowRes_Img.Height; y++)
            {
                for (int x = 0; x < lowRes_Img.Width; x++)
                {
                    Color pixel = lowRes_Img.GetPixel(x, y);
                    int grayValue = (int)(pixel.R * 0.299f + pixel.G * 0.587f + pixel.B * 0.114f);
                    gray_Img.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }
        }
        #endregion

        #region 直方图算法
        /// <summary>
        /// 计算直方插值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double CompareHistogram(int[] t)
        {
            // 释放灰度图
            gray_Img = null;
            // 重新计算一次降采样图
            GenerateLowResolutionMap(GlobalConstants.h_binSize, true);

            int[]? hist = GetHistogram(lowRes_Img);
            int[]? hist_t = t;

            // 计算归一化直方图
            double sum = hist.Sum(x => (double)x);
            double sum_t = hist_t.Sum(x => (double)x);
            distance = 0;

            for (int i = 0; i < GlobalConstants.h_binSize * 3; i++)
            {
                double norm = hist[i] / sum;
                double norm_t = hist_t[i] / sum_t;
                if (norm + norm_t > 0)
                {
                    distance += Math.Pow(norm - norm_t, 2) / (norm + norm_t);
                }
            }
            return distance; 
        }

        /// <summary>
        /// 生成直方图
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public int[] GetHistogram(Bitmap img)
        {
            int binSize = GlobalConstants.h_binSize;
            int[] histogram = new int[binSize * 3]; // R,G,B 各 n bin
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color c = img.GetPixel(x, y);
                    int rBin = c.R * binSize / 256;
                    int gBin = c.G * binSize / 256;
                    int bBin = c.B * binSize / 256;
                    histogram[rBin]++;        // Red 通道
                    histogram[binSize + gBin]++;  // Green 通道
                    histogram[binSize * 2 + bBin]++;  // Blue 通道
                }
            }
            return histogram;
        }
        #endregion

        #region SSIM算法
        /// <summary>
        /// SSIM算法
        /// </summary>
        /// <param name="img_t"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CalculateSSIM(Bitmap img_t)
        {
            Bitmap img_this = new Bitmap(pictureBox.Image);
            if (img_t.Width != pictureBox.Image.Width || img_t.Height != pictureBox.Image.Height)
            {
                throw new ArgumentException("Images must be the same size!");
            }

            int width = img_t.Width;
            int height = img_t.Height;

            double meanX = 0, meanY = 0;
            double varianceX = 0, varianceY = 0;
            double covarianceXY = 0;

            // 计算均值
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double pixelX = img_t.GetPixel(x, y).GetBrightness();
                    double pixelY = img_this.GetPixel(x, y).GetBrightness();
                    meanX += pixelX;
                    meanY += pixelY;
                }
            }
            meanX /= (width * height);
            meanY /= (width * height);

            // 计算方差和协方差
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double pixelX = img_t.GetPixel(x, y).GetBrightness();
                    double pixelY = img_this.GetPixel(x, y).GetBrightness();

                    varianceX += Math.Pow(pixelX - meanX, 2);
                    varianceY += Math.Pow(pixelY - meanY, 2);
                    covarianceXY += (pixelX - meanX) * (pixelY - meanY);
                }
            }
            varianceX /= (width * height - 1);
            varianceY /= (width * height - 1);
            covarianceXY /= (width * height - 1);

            // SSIM 计算参数
            double C1 = 0.01 * 0.01;
            double C2 = 0.03 * 0.03;

            distance = ((2 * meanX * meanY + C1) * (2 * covarianceXY + C2)) /
                          ((meanX * meanX + meanY * meanY + C1) * (varianceX + varianceY + C2));

        }
        #endregion

        /// <summary>
        /// 清空Bitmap
        /// </summary>
        public void ClearBitMap()
        {
            if(lowRes_Img != null)
                lowRes_Img.Dispose();
            if(gray_Img != null)
                gray_Img.Dispose();
        }

    }
}
