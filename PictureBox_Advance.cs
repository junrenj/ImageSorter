namespace ImageSorter
{
    public class PictureBox_Advance : PictureBox
    {
        public Image_Processing img_data;
        public Label label;
        private Color borderColor = Color.Transparent;
        
        public PictureBox_Advance(string filePath, int width, int height)
        {
            this.Image = LoadThumbnail(filePath, width, height);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.Width = width;
            this.Height = height;
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
                return img.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
        }
    }

    public class DistanceComparer : IComparer<PictureBox_Advance>
    {
        private bool isAscending;
        public DistanceComparer(bool ascending = true)
        {
            isAscending = ascending;
        }

        int IComparer<PictureBox_Advance>.Compare(PictureBox_Advance? x, PictureBox_Advance? y)
        {
            return isAscending ? x.img_data.distance.CompareTo(y.img_data.distance) : y.img_data.distance.CompareTo(x.img_data.distance);
        }
    }
}
