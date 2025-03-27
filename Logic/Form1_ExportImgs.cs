using ImageSorter.Data;
using System.Drawing.Imaging;

namespace ImageSorter.Logic
{
    public class Form1_ExportImgs
    {
        private Form1 form;
        public Form1_ExportImgs(Form1 formInstance)
        {
            form = formInstance;
        }
        public void ExportImgs(List<PictureBox_Advance> exportList, ProgressBar progressBar)
        {
            if (exportList == null || exportList.Count == 0)
            {
                MessageBox.Show("没有可导出的图片！");
                return;
            }

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "选择要保存图片的文件夹";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = folderDialog.SelectedPath;
                    string baseFileName = Microsoft.VisualBasic.Interaction.InputBox
                    (
                        "输入第一张图片的文件名",
                        "重命名图片",
                        "CustomImage"
                    );

                    if (string.IsNullOrWhiteSpace(baseFileName))
                    {
                        MessageBox.Show("文件名不能为空！");
                        return;
                    }

                    char[] invalidChars = Path.GetInvalidFileNameChars();
                    if (baseFileName.IndexOfAny(invalidChars) >= 0)
                    {
                        MessageBox.Show("文件名包含非法字符！");
                        return;
                    }
                    baseFileName = baseFileName.Trim();

                    try
                    {
                        int count = 0;
                        int totalImages = exportList.Count;

                        // 初始化进度条
                        progressBar.Minimum = 0;
                        progressBar.Maximum = totalImages;
                        progressBar.Value = 0;
                        progressBar.Visible = true;

                        foreach (PictureBox_Advance pb in exportList)
                        {
                            if (pb.Image != null)
                            {
                                string originalPath = pb.img_data.originalImagePath;
                                string fileExtension = Path.GetExtension(originalPath);
                                string fileName = $"{baseFileName}_{count}{fileExtension}";
                                string filePath = Path.Combine(savePath, fileName);

                                Image i = Image.FromFile(originalPath);
                                ImageFormat format = GetImageFormat(fileExtension);

                                if (format == ImageFormat.Tiff)
                                {
                                    Image original = Image.FromFile(originalPath);
                                    Bitmap normalMap = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                                    using (Graphics g = Graphics.FromImage(normalMap))
                                    {
                                        g.DrawImage(original, new Rectangle(0, 0, normalMap.Width, normalMap.Height),
                                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
                                    }
                                    SaveTiffWithRGB(normalMap, filePath);
                                }
                                else
                                {
                                    i.Save(filePath, format);
                                }

                                count++;
                                i.Dispose();

                                // 更新进度条
                                progressBar.Value = count;
                                Application.DoEvents(); // 让UI线程有时间去更新界面
                            }
                        }

                        MessageBox.Show($"成功保存 {count} 张图片到 {savePath}");

                        // 隐藏进度条
                        progressBar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存图片时出错：" + ex.Message);
                        progressBar.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("未选择目标文件夹！");
                }
            }
        }

        private ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".jpg":
                case ".jpeg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                case ".gif": return ImageFormat.Gif;
                case ".tiff": return ImageFormat.Tiff;
                default: return ImageFormat.Png; // 默认使用 PNG
            }
        }

        public void SaveTiffWithRGB(Bitmap image, string filePath)
        {
            ImageCodecInfo tiffCodec = ImageCodecInfo.GetImageEncoders()
        .FirstOrDefault(codec => codec.FormatID == ImageFormat.Tiff.Guid);

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionNone);

            image.Save(filePath, tiffCodec, encoderParams);
        }
    }
}
