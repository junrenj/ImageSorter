namespace ImageSorter
{
    public class Form2 : Form
    {
        public Form2(PictureBox_Advance pb_t)
        {
            InitializeComponent();
            // 保持为顶部窗口
            this.TopMost = true;
            if (pb_t != null)
                picturebox_Target.Image = Image.FromFile(pb_t.img_data.originalImagePath);
        }

        /// <summary>
        /// 更新比对图片
        /// </summary>
        /// <param name="pb_other"></param>
        public void UpdateComparePictureBox(PictureBox_Advance pb_other)
        {
            if(pictureBox_Other.Image != null)
                pictureBox_Other.Image.Dispose();
            pictureBox_Other.Image = Image.FromFile(pb_other.img_data.originalImagePath);
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            picturebox_Target = new PictureBox();
            pictureBox_Other = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picturebox_Target).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Other).BeginInit();
            SuspendLayout();
            // 
            // picturebox_target
            // 
            picturebox_Target.Location = new Point(12, 12);
            picturebox_Target.Name = "picturebox_target";
            picturebox_Target.Size = new Size(512, 512);
            picturebox_Target.TabIndex = 0;
            picturebox_Target.TabStop = false;
            picturebox_Target.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // pictureBox_Other
            // 
            pictureBox_Other.Location = new Point(542, 12);
            pictureBox_Other.Name = "pictureBox_Other";
            pictureBox_Other.Size = new Size(512, 512);
            pictureBox_Other.TabIndex = 1;
            pictureBox_Other.TabStop = false;
            pictureBox_Other.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // Form2
            // 
            ClientSize = new Size(1067, 544);
            Controls.Add(pictureBox_Other);
            Controls.Add(picturebox_Target);
            Name = "Form2";
            Text = "Image Comparer";
            ((System.ComponentModel.ISupportInitialize)picturebox_Target).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Other).EndInit();
            ResumeLayout(false);
        }

        public void Clear()
        {
            pictureBox_Other.Image.Dispose();
            picturebox_Target.Image.Dispose();
        }

        /// <summary>
        /// 目标图片
        /// </summary>
        private PictureBox picturebox_Target;
        /// <summary>
        /// 其他比对图片
        /// </summary>
        private PictureBox pictureBox_Other;
    }
}
