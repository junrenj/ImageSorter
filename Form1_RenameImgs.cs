using System.IO;
using System.Windows.Forms;

namespace ImageSorter
{
    public class Form1_RenameImgs
    {
        private Form1 form;

        public Form1_RenameImgs(Form1 formInstance)
        {
            form = formInstance;
        }

        public void RenameMultipleImage(List<PictureBox_Advance> renameList)
        {
            string currentImagePath = renameList[0].img_data.originalImagePath;
            string directory = Path.GetDirectoryName(currentImagePath);
            string oldFileName = renameList[0].img_data.fileName;
            if (renameList == null || renameList.Count == 0)
            {
                MessageBox.Show("没有可重命名的图片！");
                return;
            }

            // 1.弹出窗口
            string baseFileName = Microsoft.VisualBasic.Interaction.InputBox
            (
                "输入第一张图片的文件名",
                "重命名图片",
                oldFileName
            );

            // 2. 检查输入是否为空
            if (string.IsNullOrWhiteSpace(baseFileName))
            {
                MessageBox.Show("文件名不能为空！");
                return;
            }
            // 2.1 检查输入是否合法
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (baseFileName.IndexOfAny(invalidChars) >= 0)
            {
                MessageBox.Show("文件名包含非法字符！");
                return;
            }
            string extension = Path.GetExtension(currentImagePath); // 获取扩展名（例如 .png）
            // 2.2 去掉空格 生成合法文件名
            baseFileName = baseFileName.Trim();

            try
            {
                for (int i = 0; i < renameList.Count; i++)
                {
                    PictureBox_Advance pb = renameList[i];
                    if (pb.Image != null)
                    {
                        string newFileName = i == 0 ? $"{baseFileName}{extension}" : $"{baseFileName}_{i}{extension}";
                        string newFilePath = Path.Combine(directory, newFileName);
                        if (!File.Exists(newFilePath))
                        {
                            // 调用重命名函数
                            RenameImage(pb, pb.img_data.originalImagePath, newFilePath, newFileName);
                        }
                                
                    }
                }
                MessageBox.Show($"成功重命名 {renameList.Count} 张图片");
            }
            catch (Exception ex)
            {
                MessageBox.Show("重命名失败：" + ex.Message);
            }
        }

        private void RenameImage(PictureBox_Advance p, string currentImagePath, string newFilePath, string newFileName)
        {
            try
            {
                // 1. 释放图片占用
                p.Image?.Dispose();
                p.Image = null;

                // 2. 执行文件重命名
                File.Move(currentImagePath, newFilePath);

                // 3. 更新路径信息
                p.img_data.originalImagePath = newFilePath;
                p.img_data.fileName = newFileName;

                // 4. 重新加载图片
                using (Image img = Image.FromFile(newFilePath))
                {
                    p.Image = img.GetThumbnailImage(256, 256, null, IntPtr.Zero);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("重命名失败：" + ex.Message);
            }
        }
    }
}
