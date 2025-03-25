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

        public void RenameOneImage(PictureBox_Advance p)
        {
            if (p == null)
            {
                MessageBox.Show("请先选择图片！");
                return;
            }
            string currentImagePath = p.img_data.originalImagePath;
            string directory = Path.GetDirectoryName(currentImagePath);
            string oldFileName = p.img_data.fileName;

            string newFileName = Microsoft.VisualBasic.Interaction.InputBox
            (
                "请输入新的文件名（不含后缀）：",
                "重命名图片",
                oldFileName
            );
            // 2. 检查输入是否为空
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                MessageBox.Show("文件名不能为空！");
                return;
            }
            // 2.1 检查输入是否合法
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (newFileName.IndexOfAny(invalidChars) >= 0)
            {
                MessageBox.Show("文件名包含非法字符！");
                return;
            }
            // 2.2 去掉空格 生成合法文件名
            newFileName = newFileName.Trim();
            string newFilePath = Path.Combine(directory, newFileName + Path.GetExtension(currentImagePath));

            try
            {
                // 3. 释放图片占用
                p.Image?.Dispose();
                p.Image = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // 4. 执行文件重命名
                File.Move(currentImagePath, newFilePath);

                // 5. 更新路径信息
                p.img_data.originalImagePath = newFilePath;
                p.img_data.fileName = newFileName;

                // 6. 重新加载图片
                p.Image = new Bitmap(newFilePath);

                MessageBox.Show("重命名成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("重命名失败：" + ex.Message);
            }
        }
    }
}
