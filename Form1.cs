namespace ImageSorter
{
    public partial class Form1 : Form
    {
        // 0.逻辑
        private Form1_ImportImg importLogic;
        private Form1_RenameImgs renameLogic;
        private Form1_ExportImgs exportLogic;
        private Form1_DeleteImgs deleteLogic;
        // 0.数据容器相关
        public List<Image_Processing> importedImage_List;            // 导入的图片信息列表
        public List<PictureBox_Advance> pictureBoxes_List;           // 主面板上的图片UI列表
        private PictureBox_Advance? activePictureBox;                // 激活的图片(用于参照计算)
        private List<PictureBox_Advance> selectPictures_List;        // 框选的图片(用于批量导出)

        // 1.基本面板
        // 比对图片
        private bool isForm2Open;
        private Form2 form2;

        // 2.算法面板
        // 排序算法相关
        private E_SortType sortType = E_SortType.AHash;
        // 相似性控制精度
        private bool similarFilterOn;
        private double similarFilterValue = 100;  // 0-100用于映射
        private double remapValue;              // 映射成功后的值

        // 3.批量修改面板
        // 修改模式
        private E_ExecuteType executeType = E_ExecuteType.DeleteAll;

        // 4.导出面板
        // 导出模式
        private E_ExportType exportType = E_ExportType.ExportAll;


        // 构造函数
        public Form1()
        {
            InitializeComponent();
            importLogic = new Form1_ImportImg(this);
            renameLogic = new Form1_RenameImgs(this);
            exportLogic = new Form1_ExportImgs(this);
            deleteLogic = new Form1_DeleteImgs(this);
            pictureBoxes_List = new List<PictureBox_Advance>();
            importedImage_List = new List<Image_Processing>();
            selectPictures_List = new List<PictureBox_Advance>();
            CtrlHPanelVisibile(false);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region 图片处理按钮UI事件
        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Import_Click(object sender, EventArgs e)
        {
            importLogic.SelectAndLoadImages();
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            // 清空主面板
            mainPanel.Controls.Clear();
            // 清空图片信息列表
            ClearImageList();
            // 清空图片UI列表
            ClearPictureboxList();
            // 清空GC
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 设置目标图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SetTarget_Click(object sender, EventArgs e)
        {
            if (activePictureBox != null)
            {
                activePictureBox.BorderStyle = BorderStyle.None;
                activePictureBox.BorderColor = Color.Transparent;
            }
            if (selectPictures_List.Count == 0)
            {
                MessageBox.Show("请选择一张图片！");
                return;
            }
            activePictureBox = selectPictures_List[0];
            activePictureBox.BorderStyle = BorderStyle.FixedSingle;
            activePictureBox.BorderColor = Color.Red;
        }

        /// <summary>
        /// 比对图片按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Compare2Img_Click(object sender, EventArgs e)
        {
            if (activePictureBox == null)
            {
                MessageBox.Show("请先选择目标图片！");
                return;
            }
            form2 = new Form2(activePictureBox);
            form2.FormClosed += Form2ClosedEvent;
            form2.Show();
            isForm2Open = true;
            LockButtons(true);
        }

        #endregion

        #region 计算相关UI事件

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Calculate_Click(object sender, EventArgs e)
        {
            if (activePictureBox == null)
            {
                MessageBox.Show("请选择目标图片!");
                return;
            }
            ulong t = 0;
            switch (sortType)
            {
                case E_SortType.AHash:
                    t = activePictureBox.img_data.GetImageAHash();
                    foreach (var imgData in importedImage_List)
                    {
                        // 均值哈希
                        imgData.GetImageAHash();
                        imgData.HammingDistance(t);
                    }
                    break;
                case E_SortType.DHash:
                    t = activePictureBox.img_data.GetImageDHash();
                    foreach (var imgData in importedImage_List)
                    {
                        // 插值哈希
                        imgData.GetImageDHash();
                        imgData.HammingDistance(t);
                    }
                    break;
                case E_SortType.HistogramCompare:
                    activePictureBox.img_data.GenerateLowResolutionMap(GlobalConstants.h_binSize);
                    Bitmap b = new Bitmap(activePictureBox.img_data.originalImagePath);
                    foreach (var imgData in importedImage_List)
                    {
                        // 直方图
                        imgData.CompareHistogram(b);
                    }
                    b.Dispose();
                    break;
            }


            pictureBoxes_List.Sort(new DistanceComparer(true));

            RemapFilterValue();

            // 刷新面板
            if (similarFilterOn)
                RefreshFlowLayoutPanel(remapValue);
            else
                RefreshFlowLayoutPanel();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 算法选择更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAlgoBox_OnChanged(object sender, EventArgs e)
        {
            string selectedAlgorithm = selectBox_Algo.SelectedItem.ToString();
            switch (selectedAlgorithm)
            {
                case "均值哈希 AHash":
                    sortType = E_SortType.AHash;
                    CtrlHPanelVisibile(false);
                    break;
                case "差值哈希 DHash":
                    CtrlHPanelVisibile(false);
                    sortType = E_SortType.DHash;
                    break;
                case "直方图 H":
                    CtrlHPanelVisibile(true);
                    sortType = E_SortType.HistogramCompare;
                    break;
            }
        }

        /// <summary>
        /// 相似度滑动条更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SF_BarVaule_OnChanged(object sender, EventArgs e)
        {
            similarFilterValue = (double)trackBar_Similar.Value;
            // Remap Value
            if (similarFilterOn)
            {
                RemapFilterValue();
                RefreshFlowLayoutPanel(remapValue);
            }
        }

        /// <summary>
        /// 相似度选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimilarCB_OnChanged(object sender, EventArgs e)
        {
            similarFilterOn = checkbox_similar.Checked;
            if (similarFilterOn)
            {
                RemapFilterValue();
                RefreshFlowLayoutPanel(remapValue);
            }
            else
                RefreshFlowLayoutPanel();
        }

        /// <summary>
        /// 直方图精度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectHBinBox_OnChanged(object sender, EventArgs e)
        {
            string selectHBin = selectBox_HBin.SelectedItem.ToString();
            switch (selectHBin)
            {
                case "32":
                    GlobalConstants.h_binSize = 32;
                    break;
                case "64":
                    GlobalConstants.h_binSize = 64;
                    break;
                case "128":
                    GlobalConstants.h_binSize = 128;
                    break;
                case "256":
                    GlobalConstants.h_binSize = 256;
                    break;
            }
        }

        /// <summary>
        /// 面板2关上事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Form2ClosedEvent(object sender, FormClosedEventArgs args)
        {
            form2.Clear();
            isForm2Open = false;
            LockButtons(false);
        }

        #endregion

        #region 图片批量修改/导出/删除UI事件
        /// <summary>
        /// 执行图片操作(重命名、删除等等)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Execute_Click(object sender, EventArgs e)
        {
            if(mainPanel.Controls.Count == 0)
            {
                MessageBox.Show("请导入图片或降低筛选条件");
                return;
            }
            List<PictureBox_Advance> executeList = new List<PictureBox_Advance>();
            switch (executeType)
            {
                // 删除所有显示图片
                case E_ExecuteType.DeleteAll:
                    foreach (PictureBox_Advance item in mainPanel.Controls)
                        executeList.Add(item);
                    deleteLogic.DeleteMultipleImgs(executeList);
                    // 释放图片信息存储结构中的缓存
                    ClearImageList();
                    // 释放图片UI中的缓存
                    ClearPictureboxList();
                    // 刷新面板
                    RefreshFlowLayoutPanel();
                    break;
                // 删除所有所选图片
                case E_ExecuteType.DeleteSelect:
                    if(selectPictures_List.Count == 0)
                    {
                        MessageBox.Show("请选择图片");
                        return;
                    }
                    deleteLogic.DeleteMultipleImgs(selectPictures_List);
                    // 释放图片信息存储结构中的缓存
                    ClearImageListFromPictureBoxList(selectPictures_List);
                    // 释放图片UI中的缓存
                    ClearPictureboxListFromList(selectPictures_List); 
                    // 刷新面板
                    RefreshFlowLayoutPanel();
                    break;
                // 重命名所有显示图片
                case E_ExecuteType.RenameAllBaseOnTarget:
                    if (mainPanel.Controls.Count == 0)
                    {
                        MessageBox.Show("请导入图片");
                        return;
                    }
                    foreach (PictureBox_Advance item in mainPanel.Controls)
                        executeList.Add(item);
                    renameLogic.RenameMultipleImage(executeList);
                    break;
                // 重命名所选图片
                case E_ExecuteType.RenameSelectBaseOnTarget:
                    if (selectPictures_List.Count == 0)
                    {
                        MessageBox.Show("请选择图片");
                        return;
                    }
                    renameLogic.RenameMultipleImage(selectPictures_List);
                    break;
            }
        }

        /// <summary>
        /// 选择执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectExecuteBox_OnChanged(object sender, EventArgs e)
        {
            string selectExecuteWay = selectBox_ModifyWay.SelectedItem.ToString();
            switch (selectExecuteWay)
            {
                case "删除目前显示所有图片":
                    executeType = E_ExecuteType.DeleteAll;
                    break;
                case "删除目前选择图片":
                    executeType = E_ExecuteType.DeleteSelect;
                    break;
                case "批量重命名目前所有显示图片":
                    executeType = E_ExecuteType.RenameAllBaseOnTarget;
                    break;
                case "批量重命名所选图片":
                    executeType = E_ExecuteType.RenameSelectBaseOnTarget;
                    break;
            }
        }

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Export_Click(object sender, EventArgs e)
        {
            List<PictureBox_Advance> exportList = new List<PictureBox_Advance>();
            switch (exportType)
            {
                case E_ExportType.ExportAll:
                    foreach (PictureBox_Advance item in mainPanel.Controls)
                    {
                        exportList.Add(item);
                    }
                    break;
                case E_ExportType.ExportSelect:
                    exportList = selectPictures_List;
                    break;
            }
            // 传输给输出模块操作
            exportLogic.ExportImgs(exportList);
        }

        /// <summary>
        /// 选择导出方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBox_ExportWay_OnChanged(object sender, EventArgs e)
        {
            string selectExportWay = selectBox_ExportWay.SelectedItem.ToString();
            switch (selectExportWay)
            {
                case "导出目前显示":
                    exportType = E_ExportType.ExportAll;
                    break;
                case "导出所选":
                    exportType = E_ExportType.ExportSelect;
                    break;
            }
        }


        #endregion

        #region 面板方法(刷新)

        /// <summary>
        /// 控制直方图面板开关
        /// </summary>
        /// <param name="isVisible"></param>
        private void CtrlHPanelVisibile(bool isVisible)
        {
            selectBox_HBin.Visible = isVisible;
            lab_sampleFrequency.Visible = isVisible;
        }

        /// <summary>
        /// 重映射Filter值
        /// </summary>
        private double RemapFilterValue()
        {
            remapValue = Math.Abs(pictureBoxes_List[pictureBoxes_List.Count - 1].img_data.distance - pictureBoxes_List[0].img_data.distance);
            remapValue = remapValue * similarFilterValue / 100d + pictureBoxes_List[0].img_data.distance;
            return remapValue;
        }

        /// <summary>
        /// 刷新面板(没有筛选器的时候用)
        /// </summary>
        private void RefreshFlowLayoutPanel()
        {
            mainPanel.Controls.Clear();
            foreach (var pictureBox in pictureBoxes_List)
            {
                mainPanel.Controls.Add(pictureBox);
            }
        }

        /// <summary>
        /// 刷新面板(有筛选条件的时候用)
        /// </summary>
        /// <param name="filterValue"></param>
        private void RefreshFlowLayoutPanel(double filterValue)
        {
            mainPanel.Controls.Clear();
            foreach (var pictureBox in pictureBoxes_List)
            {
                if (filterValue >= pictureBox.img_data.distance)
                    mainPanel.Controls.Add(pictureBox);
            }
            // 防止因为误差导致目标图片没有出现
            if (mainPanel.Controls.Count == 0)
                mainPanel.Controls.Add(activePictureBox);
        }

        #endregion

        #region 锁定UI
        /// <summary>
        /// 锁定所有按钮
        /// </summary>
        /// <param name="isLock"></param>
        private void LockButtons(bool isLock)
        {
            btn_Calculate.Enabled = !isLock;
            btn_Clear.Enabled = !isLock;
            btn_Compare2Img.Enabled = !isLock;
            btn_Import.Enabled = !isLock;
            btn_Export.Enabled = !isLock;
            btn_Execute.Enabled = !isLock;
            btn_SetTarget.Enabled = !isLock;
        }
        #endregion

        #region 图片点击 框选 多选
        /// <summary>
        /// 图片框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox_Advance clickedPictureBox = sender as PictureBox_Advance;
            if (clickedPictureBox == null) return;

            if (ModifierKeys == Keys.Control) // Ctrl + 点击：多选
            {
                ToggleSelectionCtrl(clickedPictureBox);
            }
            else if (ModifierKeys == Keys.Shift) // Shift + 点击：范围选中
            {
                ToggleSelectionShift(clickedPictureBox);
            }
            else // 普通单击 取消选择所有图片
            {
                ToggleSelectionSingle(clickedPictureBox);
            }


        }

        /// <summary>
        /// Ctrl 多选
        /// </summary>
        /// <param name="clickedPictureBox"></param>
        private void ToggleSelectionCtrl(PictureBox_Advance clickedPictureBox)
        {
            if (selectPictures_List.Contains(clickedPictureBox))
            {
                selectPictures_List.Remove(clickedPictureBox);
                clickedPictureBox.BorderStyle = BorderStyle.None; // 取消高亮
            }
            else
            {
                selectPictures_List.Add(clickedPictureBox);
                clickedPictureBox.BorderStyle = BorderStyle.Fixed3D; // 高亮
            }
        }

        /// <summary>
        /// 普通单选
        /// </summary>
        /// <param name="clickedPictureBox"></param>
        private void ToggleSelectionSingle(PictureBox_Advance clickedPictureBox)
        {
            DeselectAll();

            if (clickedPictureBox == null)
                return;

            if (selectPictures_List.Count == 0)
                selectPictures_List.Add(clickedPictureBox);
            else
            {
                // 刷新外观
                selectPictures_List[0].BorderStyle = BorderStyle.None;
                selectPictures_List[0] = clickedPictureBox;
            }
            clickedPictureBox.BorderStyle = BorderStyle.Fixed3D;

            // 如果是比对模式 还要更新比对图片
            if (isForm2Open && form2 != null)
            {
                form2.UpdateComparePictureBox(selectPictures_List[0]);
            }

        }

        /// <summary>
        /// Shift 多选
        /// </summary>
        /// <param name="lastClicked"></param>
        private void ToggleSelectionShift(PictureBox_Advance lastClicked)
        {
            if (selectPictures_List.Count == 0)
                return;

            PictureBox_Advance firstSelected = selectPictures_List[0];
            List<PictureBox_Advance> allPictures = mainPanel.Controls.OfType<PictureBox_Advance>().ToList();

            int start = allPictures.IndexOf(firstSelected);
            int end = allPictures.IndexOf(lastClicked);

            if (start > end)
            {
                int temp = start;
                start = end;
                end = temp;

            }

            DeselectAll();
            for (int i = start; i <= end; i++)
            {
                ToggleSelectionCtrl(allPictures[i]);
            }
        }
        private void DeselectAll()
        {
            foreach (var pic in selectPictures_List)
            {
                pic.BorderStyle = BorderStyle.None;
            }
            selectPictures_List.Clear();
        }
        #endregion

        #region 数据清理

        /// <summary>
        /// 清空整个图片信息列表
        /// </summary>
        private void ClearImageList()
        {
            foreach (Image_Processing img in importedImage_List)
            {
                if (img != null)
                {
                    img.ClearBitMap();
                }
            }

            importedImage_List.Clear();
        }

        /// <summary>
        /// 清理指定图片信息
        /// </summary>
        /// <param name="clearList"></param>
        private void ClearImageListFromPictureBoxList(List<PictureBox_Advance> clearList)
        {
            foreach (var p in clearList)
            {
                p.img_data.ClearBitMap();
                importedImage_List.Remove(p.img_data);
            }
        }

        /// <summary>
        /// 清空整个图片UI列表
        /// </summary>
        private void ClearPictureboxList()
        {
            foreach (var pb in pictureBoxes_List)
            {
                if (pb != null)
                    pb.Destroy();
            }
            pictureBoxes_List.Clear();
            selectPictures_List.Clear();
            activePictureBox = null;
        }

        /// <summary>
        /// 清空图片UI指定元素
        /// </summary>
        /// <param name="clearList"></param>
        private void ClearPictureboxListFromList(List<PictureBox_Advance> clearList)
        {
            foreach(var pb in clearList)
            {
                if (pb != null)
                {
                    pb.Destroy();
                    pictureBoxes_List.Remove(pb);
                }

            }
        }
        #endregion

    }
}
