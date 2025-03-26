namespace ImageSorter
{
    public class Form1_ImportImg
    {
        private Form1 form;
        private ToolTip toolTip = new ToolTip();

        public Form1_ImportImg(Form1 formInstance)
        {
            form = formInstance;
        }

        public void SelectAndLoadImages()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "请选择图片文件(推荐png)";
                openFileDialog.Filter = "PNG 图片 (*.png)|*.png|JPG 图片 (*.jpg;*.jpeg)|*.jpg;*.jpeg";
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
                PictureBox_Advance pictureBox = new PictureBox_Advance(file, 256, 256);
                toolTip.SetToolTip(pictureBox, Path.GetFileNameWithoutExtension(file));

                // 注册点击事件
                pictureBox.Click += form.PictureBox_Click;
                // 构建Image存储结构
                Image_Processing newImage = new Image_Processing(file, pictureBox);
                form.importedImage_List.Add(newImage);
                // 构建PictureBox列表
                pictureBox.img_data = newImage;
                form.pictureBoxes_List.Add(pictureBox);
                form.mainPanel.Controls.Add(pictureBox);
            }
        }
    }

}
