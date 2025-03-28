namespace ImageSorter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btn_Import = new Button();
            btn_Calculate = new Button();
            openFileDialog1 = new OpenFileDialog();
            mainPanel = new FlowLayoutPanel();
            btn_Clear = new Button();
            btn_Export = new Button();
            selectBox_Algo = new ComboBox();
            trackBar_Similar = new TrackBar();
            checkbox_similar = new CheckBox();
            selectBox_HBin = new ComboBox();
            lab_algo = new Label();
            lab_sampleFrequency = new Label();
            btn_Execute = new Button();
            btn_Compare2Img = new Button();
            btn_SetTarget = new Button();
            selectBox_ExportWay = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            selectBox_ModifyWay = new ComboBox();
            lab_high = new Label();
            lab_low = new Label();
            progressBar_Calculate = new ProgressBar();
            lab_imgSize = new Label();
            selectBox_ImgSize = new ComboBox();
            progressBar_Export = new ProgressBar();
            btn_ClearSlect = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar_Similar).BeginInit();
            SuspendLayout();
            // 
            // btn_Import
            // 
            btn_Import.Location = new Point(23, 12);
            btn_Import.Name = "btn_Import";
            btn_Import.Size = new Size(75, 23);
            btn_Import.TabIndex = 0;
            btn_Import.Text = "导入图片";
            btn_Import.UseVisualStyleBackColor = true;
            btn_Import.Click += Btn_Import_Click;
            // 
            // btn_Calculate
            // 
            btn_Calculate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_Calculate.Location = new Point(729, 149);
            btn_Calculate.Name = "btn_Calculate";
            btn_Calculate.Size = new Size(75, 23);
            btn_Calculate.TabIndex = 1;
            btn_Calculate.Text = "计算";
            btn_Calculate.UseVisualStyleBackColor = true;
            btn_Calculate.Click += Btn_Calculate_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // mainPanel
            // 
            mainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.AutoScroll = true;
            mainPanel.Location = new Point(0, 54);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(643, 458);
            mainPanel.TabIndex = 4;
            // 
            // btn_Clear
            // 
            btn_Clear.Location = new Point(116, 12);
            btn_Clear.Name = "btn_Clear";
            btn_Clear.Size = new Size(75, 23);
            btn_Clear.TabIndex = 0;
            btn_Clear.Text = "清空仓库";
            btn_Clear.UseVisualStyleBackColor = true;
            btn_Clear.Click += Btn_Clear_Click;
            // 
            // btn_Export
            // 
            btn_Export.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_Export.Location = new Point(729, 467);
            btn_Export.Name = "btn_Export";
            btn_Export.Size = new Size(75, 23);
            btn_Export.TabIndex = 6;
            btn_Export.Text = "导出";
            btn_Export.UseVisualStyleBackColor = true;
            btn_Export.Click += Btn_Export_Click;
            // 
            // selectBox_Algo
            // 
            selectBox_Algo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBox_Algo.FormattingEnabled = true;
            selectBox_Algo.Items.AddRange(new object[] { "均值哈希 AHash", "差值哈希 DHash", "直方图 H", "结构相似指数 SSIM", "直方图移动距离 EMD" });
            selectBox_Algo.Location = new Point(741, 54);
            selectBox_Algo.Name = "selectBox_Algo";
            selectBox_Algo.Size = new Size(121, 25);
            selectBox_Algo.TabIndex = 7;
            selectBox_Algo.Text = "均值哈希 AHash";
            selectBox_Algo.SelectedIndexChanged += SelectAlgoBox_OnChanged;
            // 
            // trackBar_Similar
            // 
            trackBar_Similar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBar_Similar.Location = new Point(666, 224);
            trackBar_Similar.Maximum = 100;
            trackBar_Similar.Name = "trackBar_Similar";
            trackBar_Similar.Size = new Size(196, 45);
            trackBar_Similar.TabIndex = 8;
            trackBar_Similar.TickStyle = TickStyle.None;
            trackBar_Similar.Scroll += SF_BarVaule_OnChanged;
            // 
            // checkbox_similar
            // 
            checkbox_similar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkbox_similar.AutoSize = true;
            checkbox_similar.Location = new Point(669, 197);
            checkbox_similar.Name = "checkbox_similar";
            checkbox_similar.Size = new Size(87, 21);
            checkbox_similar.TabIndex = 9;
            checkbox_similar.Text = "相似度筛选";
            checkbox_similar.UseVisualStyleBackColor = true;
            checkbox_similar.CheckedChanged += SimilarCB_OnChanged;
            // 
            // selectBox_HBin
            // 
            selectBox_HBin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBox_HBin.FormattingEnabled = true;
            selectBox_HBin.Items.AddRange(new object[] { "32", "64", "128", "256" });
            selectBox_HBin.Location = new Point(741, 85);
            selectBox_HBin.Name = "selectBox_HBin";
            selectBox_HBin.Size = new Size(121, 25);
            selectBox_HBin.TabIndex = 10;
            selectBox_HBin.Text = "32";
            selectBox_HBin.SelectedIndexChanged += SelectHBinBox_OnChanged;
            // 
            // lab_algo
            // 
            lab_algo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lab_algo.AutoSize = true;
            lab_algo.Location = new Point(666, 59);
            lab_algo.Name = "lab_algo";
            lab_algo.Size = new Size(56, 17);
            lab_algo.TabIndex = 11;
            lab_algo.Text = "使用算法";
            // 
            // lab_sampleFrequency
            // 
            lab_sampleFrequency.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lab_sampleFrequency.AutoSize = true;
            lab_sampleFrequency.Location = new Point(666, 88);
            lab_sampleFrequency.Name = "lab_sampleFrequency";
            lab_sampleFrequency.Size = new Size(56, 17);
            lab_sampleFrequency.TabIndex = 12;
            lab_sampleFrequency.Text = "采样频次";
            // 
            // btn_Execute
            // 
            btn_Execute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_Execute.Location = new Point(720, 350);
            btn_Execute.Name = "btn_Execute";
            btn_Execute.Size = new Size(96, 23);
            btn_Execute.TabIndex = 13;
            btn_Execute.Text = "执行操作";
            btn_Execute.UseVisualStyleBackColor = true;
            btn_Execute.Click += Btn_Execute_Click;
            // 
            // btn_Compare2Img
            // 
            btn_Compare2Img.Location = new Point(415, 12);
            btn_Compare2Img.Name = "btn_Compare2Img";
            btn_Compare2Img.Size = new Size(75, 23);
            btn_Compare2Img.TabIndex = 15;
            btn_Compare2Img.Text = "比对图片";
            btn_Compare2Img.UseVisualStyleBackColor = true;
            btn_Compare2Img.Click += Btn_Compare2Img_Click;
            // 
            // btn_SetTarget
            // 
            btn_SetTarget.Location = new Point(211, 12);
            btn_SetTarget.Name = "btn_SetTarget";
            btn_SetTarget.Size = new Size(75, 23);
            btn_SetTarget.TabIndex = 16;
            btn_SetTarget.Text = "设为目标";
            btn_SetTarget.UseVisualStyleBackColor = true;
            btn_SetTarget.Click += Btn_SetTarget_Click;
            // 
            // selectBox_ExportWay
            // 
            selectBox_ExportWay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBox_ExportWay.DropDownWidth = 150;
            selectBox_ExportWay.FormattingEnabled = true;
            selectBox_ExportWay.Items.AddRange(new object[] { "导出目前显示", "导出所选" });
            selectBox_ExportWay.Location = new Point(741, 414);
            selectBox_ExportWay.Name = "selectBox_ExportWay";
            selectBox_ExportWay.Size = new Size(121, 25);
            selectBox_ExportWay.TabIndex = 17;
            selectBox_ExportWay.Text = "导出目前显示所有";
            selectBox_ExportWay.SelectedIndexChanged += SelectBox_ExportWay_OnChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(666, 417);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 18;
            label1.Text = "导出选项";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(655, 309);
            label2.Name = "label2";
            label2.Size = new Size(80, 17);
            label2.TabIndex = 19;
            label2.Text = "批量修改选项";
            // 
            // selectBox_ModifyWay
            // 
            selectBox_ModifyWay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBox_ModifyWay.DropDownWidth = 200;
            selectBox_ModifyWay.FormattingEnabled = true;
            selectBox_ModifyWay.Items.AddRange(new object[] { "删除目前显示所有图片", "删除目前选择图片", "批量重命名目前所有显示图片", "批量重命名所选图片" });
            selectBox_ModifyWay.Location = new Point(741, 306);
            selectBox_ModifyWay.Name = "selectBox_ModifyWay";
            selectBox_ModifyWay.Size = new Size(121, 25);
            selectBox_ModifyWay.TabIndex = 20;
            selectBox_ModifyWay.Text = "删除目前显示所有图片";
            selectBox_ModifyWay.SelectedIndexChanged += SelectExecuteBox_OnChanged;
            // 
            // lab_high
            // 
            lab_high.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lab_high.AutoSize = true;
            lab_high.Location = new Point(670, 260);
            lab_high.Name = "lab_high";
            lab_high.Size = new Size(20, 17);
            lab_high.TabIndex = 21;
            lab_high.Text = "高";
            // 
            // lab_low
            // 
            lab_low.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lab_low.AutoSize = true;
            lab_low.Location = new Point(842, 260);
            lab_low.Name = "lab_low";
            lab_low.Size = new Size(20, 17);
            lab_low.TabIndex = 22;
            lab_low.Text = "低";
            // 
            // progressBar_Calculate
            // 
            progressBar_Calculate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar_Calculate.Location = new Point(669, 123);
            progressBar_Calculate.Name = "progressBar_Calculate";
            progressBar_Calculate.Size = new Size(193, 10);
            progressBar_Calculate.TabIndex = 23;
            // 
            // lab_imgSize
            // 
            lab_imgSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lab_imgSize.AutoSize = true;
            lab_imgSize.Location = new Point(643, 26);
            lab_imgSize.Name = "lab_imgSize";
            lab_imgSize.Size = new Size(92, 17);
            lab_imgSize.TabIndex = 24;
            lab_imgSize.Text = "图片尺寸筛选器";
            // 
            // selectBox_ImgSize
            // 
            selectBox_ImgSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectBox_ImgSize.FormattingEnabled = true;
            selectBox_ImgSize.Items.AddRange(new object[] { "512", "1024", "2048", "None" });
            selectBox_ImgSize.Location = new Point(741, 23);
            selectBox_ImgSize.Name = "selectBox_ImgSize";
            selectBox_ImgSize.Size = new Size(121, 25);
            selectBox_ImgSize.TabIndex = 25;
            selectBox_ImgSize.Text = "None";
            selectBox_ImgSize.SelectedIndexChanged += SelectImgSizeBox_OnChnaged;
            // 
            // progressBar_Export
            // 
            progressBar_Export.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBar_Export.Location = new Point(669, 451);
            progressBar_Export.Name = "progressBar_Export";
            progressBar_Export.Size = new Size(193, 10);
            progressBar_Export.TabIndex = 26;
            // 
            // btn_ClearSlect
            // 
            btn_ClearSlect.Location = new Point(314, 12);
            btn_ClearSlect.Name = "btn_ClearSlect";
            btn_ClearSlect.Size = new Size(75, 23);
            btn_ClearSlect.TabIndex = 27;
            btn_ClearSlect.Text = "清空所选";
            btn_ClearSlect.UseVisualStyleBackColor = true;
            btn_ClearSlect.Click += btn_ClearSlect_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(878, 524);
            Controls.Add(btn_ClearSlect);
            Controls.Add(progressBar_Export);
            Controls.Add(selectBox_ImgSize);
            Controls.Add(lab_imgSize);
            Controls.Add(progressBar_Calculate);
            Controls.Add(lab_low);
            Controls.Add(lab_high);
            Controls.Add(selectBox_ModifyWay);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(selectBox_ExportWay);
            Controls.Add(btn_SetTarget);
            Controls.Add(btn_Compare2Img);
            Controls.Add(btn_Execute);
            Controls.Add(lab_sampleFrequency);
            Controls.Add(lab_algo);
            Controls.Add(selectBox_HBin);
            Controls.Add(checkbox_similar);
            Controls.Add(trackBar_Similar);
            Controls.Add(selectBox_Algo);
            Controls.Add(btn_Export);
            Controls.Add(btn_Clear);
            Controls.Add(mainPanel);
            Controls.Add(btn_Calculate);
            Controls.Add(btn_Import);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Image Sorter";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar_Similar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_Import;
        private Button btn_Calculate;
        private OpenFileDialog openFileDialog1;
        public FlowLayoutPanel mainPanel;
        private Button btn_Clear;
        private Button btn_Export;
        private ComboBox selectBox_Algo;
        private TrackBar trackBar_Similar;
        private CheckBox checkbox_similar;
        private ComboBox selectBox_HBin;
        private Label lab_algo;
        private Label lab_sampleFrequency;
        private Button btn_Execute;
        private Button btn_Compare2Img;
        private Button btn_SetTarget;
        private ComboBox selectBox_ExportWay;
        private Label label1;
        private Label label2;
        private ComboBox selectBox_ModifyWay;
        private Label lab_high;
        private Label lab_low;
        private ProgressBar progressBar_Calculate;
        private Label lab_imgSize;
        private ComboBox selectBox_ImgSize;
        private ProgressBar progressBar_Export;
        private Button btn_ClearSlect;
    }
}
