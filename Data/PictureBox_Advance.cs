namespace ImageSorter.Data
{
    public class PictureBox_Advance : PictureBox
    {
        public Image_Processing img_data;
        public Label label;
        private Color borderColor = Color.Transparent;

        public PictureBox_Advance(Image icon)
        {
            Image = icon;
            SizeMode = PictureBoxSizeMode.Zoom;
            Width = 100;
            Height = 100;
            Margin = new Padding(5);
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        /// <summary>
        /// 自定义绘制边框
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (borderColor != Color.Transparent)
            {
                using (Pen pen = new Pen(borderColor, 10)) // 设置边框颜色和宽度
                {
                    // 绘制边框
                    pe.Graphics.DrawRectangle(pen, 0, 0, Width, Height);
                }
            }
        }

        /// <summary>
        /// 销毁时使用
        /// </summary>
        public void Destroy()
        {
            Image.Dispose();
            Image = null;
        }
    }
}
