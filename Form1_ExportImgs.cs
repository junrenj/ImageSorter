using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ImageSorter
{
    public class Form1_ExportImgs
    {
        private Form1 form;
        public Form1_ExportImgs(Form1 formInstance)
        {
            form = formInstance;
        }
        public void ExportImgs(List<PictureBox_Advance> exportList)
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

                    try
                    {
                        int count = 0;
                        foreach (PictureBox_Advance pb in exportList)
                        {
                            if (pb.Image != null)
                            {
                                string fileName = $"Image_{count}.png"; // 生成唯一文件名
                                string filePath = Path.Combine(savePath, fileName);

                                // 保存图片副本
                                Image i = Image.FromFile(filePath);
                                i.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                                count++;
                                i.Dispose();
                            }
                        }
                        MessageBox.Show($"成功保存 {count} 张图片到 {savePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存图片时出错：" + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("未选择目标文件夹！");
                }
            }
        }
    }
}
