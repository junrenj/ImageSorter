using ImageSorter.Data;

namespace ImageSorter
{
    public class Form2 : Form
    {
        public Form2(PictureBox_Advance p)
        {
            InitializeComponent();
            // 保持为顶部窗口
            this.TopMost = true;
            if (p != null)
            {
                picturebox_Target.Image = Image.FromFile(p.img_data.originalImagePath);
                lab_targetImgName.Text = p.img_data.fileName;
                lab_targetImgSize.Text = $"{picturebox_Target.Image.Size.Width} x {picturebox_Target.Image.Size.Height}";
            }
        }

        /// <summary>
        /// 更新比对图片
        /// </summary>
        /// <param name="pb_other"></param>
        public void UpdateComparePictureBox(PictureBox_Advance p)
        {
            if (pictureBox_Other.Image != null)
                pictureBox_Other.Image.Dispose();
            pictureBox_Other.Image = Image.FromFile(p.img_data.originalImagePath);
            lab_compareImgName.Text = p.img_data.fileName;
            lab_compareImfSize.Text = $"{pictureBox_Other.Image.Size.Width} x {pictureBox_Other.Image.Size.Height}";
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            picturebox_Target = new PictureBox();
            pictureBox_Other = new PictureBox();
            lab_target = new Label();
            lab_compareImg = new Label();
            lab_targetImgName = new Label();
            lab_compareImgName = new Label();
            lab_targetImgSize = new Label();
            lab_compareImfSize = new Label();
            ((System.ComponentModel.ISupportInitialize)picturebox_Target).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Other).BeginInit();
            SuspendLayout();
            // 
            // picturebox_Target
            // 
            picturebox_Target.Anchor = AnchorStyles.Left;
            picturebox_Target.Location = new Point(12, 23);
            picturebox_Target.Name = "picturebox_Target";
            picturebox_Target.Size = new Size(512, 512);
            picturebox_Target.SizeMode = PictureBoxSizeMode.Zoom;
            picturebox_Target.TabIndex = 0;
            picturebox_Target.TabStop = false;
            // 
            // pictureBox_Other
            // 
            pictureBox_Other.Anchor = AnchorStyles.Left;
            pictureBox_Other.Location = new Point(542, 23);
            pictureBox_Other.Name = "pictureBox_Other";
            pictureBox_Other.Size = new Size(512, 512);
            pictureBox_Other.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_Other.TabIndex = 1;
            pictureBox_Other.TabStop = false;
            // 
            // lab_target
            // 
            lab_target.Anchor = AnchorStyles.Left;
            lab_target.AutoSize = true;
            lab_target.BackColor = Color.Transparent;
            lab_target.Location = new Point(15, 5);
            lab_target.Name = "lab_target";
            lab_target.Size = new Size(56, 17);
            lab_target.TabIndex = 2;
            lab_target.Text = "目标图片";
            // 
            // lab_compareImg
            // 
            lab_compareImg.Anchor = AnchorStyles.Left;
            lab_compareImg.AutoSize = true;
            lab_compareImg.BackColor = Color.Transparent;
            lab_compareImg.Location = new Point(543, 5);
            lab_compareImg.Name = "lab_compareImg";
            lab_compareImg.Size = new Size(56, 17);
            lab_compareImg.TabIndex = 3;
            lab_compareImg.Text = "比对图片";
            // 
            // lab_targetImgName
            // 
            lab_targetImgName.Anchor = AnchorStyles.Left;
            lab_targetImgName.AutoSize = true;
            lab_targetImgName.BackColor = Color.Transparent;
            lab_targetImgName.Location = new Point(230, 536);
            lab_targetImgName.Name = "lab_targetImgName";
            lab_targetImgName.Size = new Size(59, 17);
            lab_targetImgName.TabIndex = 4;
            lab_targetImgName.Text = "IMG.png";
            // 
            // lab_compareImgName
            // 
            lab_compareImgName.Anchor = AnchorStyles.Left;
            lab_compareImgName.AutoSize = true;
            lab_compareImgName.BackColor = Color.Transparent;
            lab_compareImgName.Location = new Point(779, 536);
            lab_compareImgName.Name = "lab_compareImgName";
            lab_compareImgName.Size = new Size(0, 17);
            lab_compareImgName.TabIndex = 5;
            // 
            // lab_targetImgSize
            // 
            lab_targetImgSize.Anchor = AnchorStyles.Left;
            lab_targetImgSize.AutoSize = true;
            lab_targetImgSize.BackColor = Color.Transparent;
            lab_targetImgSize.Location = new Point(77, 5);
            lab_targetImgSize.Name = "lab_targetImgSize";
            lab_targetImgSize.Size = new Size(64, 17);
            lab_targetImgSize.TabIndex = 6;
            lab_targetImgSize.Text = "512 x 512";
            // 
            // lab_compareImfSize
            // 
            lab_compareImfSize.Anchor = AnchorStyles.Left;
            lab_compareImfSize.AutoSize = true;
            lab_compareImfSize.BackColor = Color.Transparent;
            lab_compareImfSize.Location = new Point(605, 5);
            lab_compareImfSize.Name = "lab_compareImfSize";
            lab_compareImfSize.Size = new Size(0, 17);
            lab_compareImfSize.TabIndex = 7;
            // 
            // Form2
            // 
            ClientSize = new Size(1067, 556);
            Controls.Add(lab_compareImfSize);
            Controls.Add(lab_targetImgSize);
            Controls.Add(lab_compareImgName);
            Controls.Add(lab_targetImgName);
            Controls.Add(lab_compareImg);
            Controls.Add(lab_target);
            Controls.Add(pictureBox_Other);
            Controls.Add(picturebox_Target);
            Name = "Form2";
            Text = "Image Comparer";
            ((System.ComponentModel.ISupportInitialize)picturebox_Target).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Other).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private Label lab_target;
        private Label lab_compareImg;
        private Label lab_targetImgName;
        private Label lab_compareImgName;
        private Label lab_targetImgSize;
        private Label lab_compareImfSize;

        /// <summary>
        /// 其他比对图片
        /// </summary>
        private PictureBox pictureBox_Other;

    }
}
