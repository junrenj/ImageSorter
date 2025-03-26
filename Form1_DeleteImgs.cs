using System.Windows.Forms;

namespace ImageSorter
{
    public class Form1_DeleteImgs
    {
        private Form1 form;
        public Form1_DeleteImgs(Form1 formInstance)
        {
            form = formInstance;
        }

        public void DeleteMultipleImgs(List<PictureBox_Advance> list)
        {
            if(list.Count == 0)
                return;

            // 弹出确认窗口
            DialogResult result = MessageBox.Show(
                "确定要删除这些图片吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                foreach (PictureBox_Advance p in list)
                {
                    DeleteImg(p);
                }
                MessageBox.Show("图片删除成功！", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void DeleteImg(PictureBox_Advance p)
        {
            if (p == null || p.Image == null)
            {
                MessageBox.Show("未选中任何图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取图片路径（假设你存储了文件路径）
            string imagePath = p.img_data.originalImagePath; // 假设 PictureBox 的 Tag 存储路径
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show("找不到该图片文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 删除文件
                File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
