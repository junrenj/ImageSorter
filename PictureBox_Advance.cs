namespace ImageSorter
{
    public class PictureBox_Advance : PictureBox
    {
        public Image_Processing img_data;
        public Label label;
        private Color borderColor = Color.Transparent;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

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
