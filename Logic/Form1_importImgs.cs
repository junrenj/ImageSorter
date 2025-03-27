using ImageSorter.Data;

namespace ImageSorter.Logic
{
    public class Form1_ImportImg
    {
        private Form1 form;
        private ToolTip toolTip = new ToolTip();
        private E_ImgSize imgSize;

        public Form1_ImportImg(Form1 formInstance)
        {
            form = formInstance;
        }

        public void SelectAndLoadImages()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "请选择图片文件(推荐png)";
                openFileDialog.Filter = "PNG 图片 (*.png)|*.png|JPG 图片 (*.jpg;*.jpeg)|*.jpg;*.jpeg|TIFF 图片 (*.tiff;*.tif)|*.tiff;*.tif"; ;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSelectedImages(openFileDialog.FileNames);
                }
            }
        }

        private void LoadSelectedImages(string[] filePaths)
        {
            foreach (var file in filePaths)
            {
                // 导入前检查一下是否已经导入过了
                foreach (var file_exist in form.importedImage_List)
                {
                    if (file == file_exist.originalImagePath)
                    {
                        MessageBox.Show("含有已经导入过的图片，请勿重复导入");
                        return;
                    }
                }
                // 创造缩略图
                Image icon = LoadThumbnail(file, 256, 256);
                PictureBox_Advance pictureBox = new PictureBox_Advance(icon);
                toolTip.SetToolTip(pictureBox, Path.GetFileNameWithoutExtension(file));
                // 注册点击事件
                pictureBox.Click += form.PictureBox_Click;


                // 构建Image存储结构
                Image_Processing newImage = new Image_Processing(file, pictureBox, imgSize);
                form.importedImage_List.Add(newImage);

                // 构建PictureBox列表
                pictureBox.img_data = newImage;
                form.pictureBoxes_List.Add(pictureBox);
                form.mainPanel.Controls.Add(pictureBox);
            }
        }


        /// <summary>
        /// 自动生成缩略图的方法
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Image LoadThumbnail(string filePath, int width, int height)
        {
            using (Image img = Image.FromFile(filePath))
            {
                if (img.Size.Width == 512 && img.Size.Height == 512)
                    imgSize = E_ImgSize.S_512;
                else if (img.Size.Width == 1024 && img.Size.Height == 1024)
                    imgSize = E_ImgSize.S_1024;
                else if (img.Size.Width == 2048 && img.Size.Height == 2048)
                    imgSize = E_ImgSize.S_2048;
                else
                    imgSize = E_ImgSize.Others;

                return img.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
        }
    }

}
