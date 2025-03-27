namespace ImageSorter
{
    public class PictureBox_Advance : PictureBox
    {
        public Image_Processing img_data;
        public Label label;
        private Color borderColor = Color.Transparent;
        
        public PictureBox_Advance(Image icon)
        {
            this.Image = icon;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Width = 100;
            this.Height = 100;
            this.Margin = new Padding(5);
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
                using (Pen pen = new Pen(borderColor, 3)) // 设置边框颜色和宽度
                {
                    // 绘制边框
                    pe.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
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
