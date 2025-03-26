namespace ImageSorter
{
    public partial class Form1 : Form
    {
        // 0.�߼�
        private Form1_ImportImg importLogic;
        private Form1_RenameImgs renameLogic;
        private Form1_ExportImgs exportLogic;
        private Form1_DeleteImgs deleteLogic;
        // 0.�����������
        public List<Image_Processing> importedImage_List;            // �����ͼƬ��Ϣ�б�
        public List<PictureBox_Advance> pictureBoxes_List;           // ������ϵ�ͼƬUI�б�
        private PictureBox_Advance? activePictureBox;                // �����ͼƬ(���ڲ��ռ���)
        private List<PictureBox_Advance> selectPictures_List;        // ��ѡ��ͼƬ(������������)

        // 1.�������
        // �ȶ�ͼƬ
        private bool isForm2Open;
        private Form2 form2;

        // 2.�㷨���
        // �����㷨���
        private E_SortType sortType = E_SortType.AHash;
        // �����Կ��ƾ���
        private bool similarFilterOn;
        private double similarFilterValue = 100;  // 0-100����ӳ��
        private double remapValue;              // ӳ��ɹ����ֵ

        // 3.�����޸����
        // �޸�ģʽ
        private E_ExecuteType executeType = E_ExecuteType.DeleteAll;

        // 4.�������
        // ����ģʽ
        private E_ExportType exportType = E_ExportType.ExportAll;


        // ���캯��
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

        #region ͼƬ����ťUI�¼�
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Import_Click(object sender, EventArgs e)
        {
            importLogic.SelectAndLoadImages();
        }

        /// <summary>
        /// ���ͼƬ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            // ��������
            mainPanel.Controls.Clear();
            // ���ͼƬ��Ϣ�б�
            ClearImageList();
            // ���ͼƬUI�б�
            ClearPictureboxList();
            // ���GC
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// ����Ŀ��ͼƬ
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
                MessageBox.Show("��ѡ��һ��ͼƬ��");
                return;
            }
            activePictureBox = selectPictures_List[0];
            activePictureBox.BorderStyle = BorderStyle.FixedSingle;
            activePictureBox.BorderColor = Color.Red;
        }

        /// <summary>
        /// �ȶ�ͼƬ��ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Compare2Img_Click(object sender, EventArgs e)
        {
            if (activePictureBox == null)
            {
                MessageBox.Show("����ѡ��Ŀ��ͼƬ��");
                return;
            }
            form2 = new Form2(activePictureBox);
            form2.FormClosed += Form2ClosedEvent;
            form2.Show();
            isForm2Open = true;
            LockButtons(true);
        }

        #endregion

        #region �������UI�¼�

        /// <summary>
        /// ���㰴ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Calculate_Click(object sender, EventArgs e)
        {
            if (activePictureBox == null)
            {
                MessageBox.Show("��ѡ��Ŀ��ͼƬ!");
                return;
            }
            ulong t = 0;
            switch (sortType)
            {
                case E_SortType.AHash:
                    t = activePictureBox.img_data.GetImageAHash();
                    foreach (var imgData in importedImage_List)
                    {
                        // ��ֵ��ϣ
                        imgData.GetImageAHash();
                        imgData.HammingDistance(t);
                    }
                    break;
                case E_SortType.DHash:
                    t = activePictureBox.img_data.GetImageDHash();
                    foreach (var imgData in importedImage_List)
                    {
                        // ��ֵ��ϣ
                        imgData.GetImageDHash();
                        imgData.HammingDistance(t);
                    }
                    break;
                case E_SortType.HistogramCompare:
                    activePictureBox.img_data.GenerateLowResolutionMap(GlobalConstants.h_binSize);
                    Bitmap b = new Bitmap(activePictureBox.img_data.originalImagePath);
                    foreach (var imgData in importedImage_List)
                    {
                        // ֱ��ͼ
                        imgData.CompareHistogram(b);
                    }
                    b.Dispose();
                    break;
            }


            pictureBoxes_List.Sort(new DistanceComparer(true));

            RemapFilterValue();

            // ˢ�����
            if (similarFilterOn)
                RefreshFlowLayoutPanel(remapValue);
            else
                RefreshFlowLayoutPanel();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// �㷨ѡ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAlgoBox_OnChanged(object sender, EventArgs e)
        {
            string selectedAlgorithm = selectBox_Algo.SelectedItem.ToString();
            switch (selectedAlgorithm)
            {
                case "��ֵ��ϣ AHash":
                    sortType = E_SortType.AHash;
                    CtrlHPanelVisibile(false);
                    break;
                case "��ֵ��ϣ DHash":
                    CtrlHPanelVisibile(false);
                    sortType = E_SortType.DHash;
                    break;
                case "ֱ��ͼ H":
                    CtrlHPanelVisibile(true);
                    sortType = E_SortType.HistogramCompare;
                    break;
            }
        }

        /// <summary>
        /// ���ƶȻ���������
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
        /// ���ƶ�ѡ��
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
        /// ֱ��ͼ����
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
        /// ���2�����¼�
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

        #region ͼƬ�����޸�/����/ɾ��UI�¼�
        /// <summary>
        /// ִ��ͼƬ����(��������ɾ���ȵ�)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Execute_Click(object sender, EventArgs e)
        {
            if(mainPanel.Controls.Count == 0)
            {
                MessageBox.Show("�뵼��ͼƬ�򽵵�ɸѡ����");
                return;
            }
            List<PictureBox_Advance> executeList = new List<PictureBox_Advance>();
            switch (executeType)
            {
                // ɾ��������ʾͼƬ
                case E_ExecuteType.DeleteAll:
                    foreach (PictureBox_Advance item in mainPanel.Controls)
                        executeList.Add(item);
                    deleteLogic.DeleteMultipleImgs(executeList);
                    // �ͷ�ͼƬ��Ϣ�洢�ṹ�еĻ���
                    ClearImageList();
                    // �ͷ�ͼƬUI�еĻ���
                    ClearPictureboxList();
                    // ˢ�����
                    RefreshFlowLayoutPanel();
                    break;
                // ɾ��������ѡͼƬ
                case E_ExecuteType.DeleteSelect:
                    if(selectPictures_List.Count == 0)
                    {
                        MessageBox.Show("��ѡ��ͼƬ");
                        return;
                    }
                    deleteLogic.DeleteMultipleImgs(selectPictures_List);
                    // �ͷ�ͼƬ��Ϣ�洢�ṹ�еĻ���
                    ClearImageListFromPictureBoxList(selectPictures_List);
                    // �ͷ�ͼƬUI�еĻ���
                    ClearPictureboxListFromList(selectPictures_List); 
                    // ˢ�����
                    RefreshFlowLayoutPanel();
                    break;
                // ������������ʾͼƬ
                case E_ExecuteType.RenameAllBaseOnTarget:
                    if (mainPanel.Controls.Count == 0)
                    {
                        MessageBox.Show("�뵼��ͼƬ");
                        return;
                    }
                    foreach (PictureBox_Advance item in mainPanel.Controls)
                        executeList.Add(item);
                    renameLogic.RenameMultipleImage(executeList);
                    break;
                // ��������ѡͼƬ
                case E_ExecuteType.RenameSelectBaseOnTarget:
                    if (selectPictures_List.Count == 0)
                    {
                        MessageBox.Show("��ѡ��ͼƬ");
                        return;
                    }
                    renameLogic.RenameMultipleImage(selectPictures_List);
                    break;
            }
        }

        /// <summary>
        /// ѡ��ִ�в���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectExecuteBox_OnChanged(object sender, EventArgs e)
        {
            string selectExecuteWay = selectBox_ModifyWay.SelectedItem.ToString();
            switch (selectExecuteWay)
            {
                case "ɾ��Ŀǰ��ʾ����ͼƬ":
                    executeType = E_ExecuteType.DeleteAll;
                    break;
                case "ɾ��Ŀǰѡ��ͼƬ":
                    executeType = E_ExecuteType.DeleteSelect;
                    break;
                case "����������Ŀǰ������ʾͼƬ":
                    executeType = E_ExecuteType.RenameAllBaseOnTarget;
                    break;
                case "������������ѡͼƬ":
                    executeType = E_ExecuteType.RenameSelectBaseOnTarget;
                    break;
            }
        }

        /// <summary>
        /// ������ť
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
            // ��������ģ�����
            exportLogic.ExportImgs(exportList);
        }

        /// <summary>
        /// ѡ�񵼳���ʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBox_ExportWay_OnChanged(object sender, EventArgs e)
        {
            string selectExportWay = selectBox_ExportWay.SelectedItem.ToString();
            switch (selectExportWay)
            {
                case "����Ŀǰ��ʾ":
                    exportType = E_ExportType.ExportAll;
                    break;
                case "������ѡ":
                    exportType = E_ExportType.ExportSelect;
                    break;
            }
        }


        #endregion

        #region ��巽��(ˢ��)

        /// <summary>
        /// ����ֱ��ͼ��忪��
        /// </summary>
        /// <param name="isVisible"></param>
        private void CtrlHPanelVisibile(bool isVisible)
        {
            selectBox_HBin.Visible = isVisible;
            lab_sampleFrequency.Visible = isVisible;
        }

        /// <summary>
        /// ��ӳ��Filterֵ
        /// </summary>
        private double RemapFilterValue()
        {
            remapValue = Math.Abs(pictureBoxes_List[pictureBoxes_List.Count - 1].img_data.distance - pictureBoxes_List[0].img_data.distance);
            remapValue = remapValue * similarFilterValue / 100d + pictureBoxes_List[0].img_data.distance;
            return remapValue;
        }

        /// <summary>
        /// ˢ�����(û��ɸѡ����ʱ����)
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
        /// ˢ�����(��ɸѡ������ʱ����)
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
            // ��ֹ��Ϊ����Ŀ��ͼƬû�г���
            if (mainPanel.Controls.Count == 0)
                mainPanel.Controls.Add(activePictureBox);
        }

        #endregion

        #region ����UI
        /// <summary>
        /// �������а�ť
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

        #region ͼƬ��� ��ѡ ��ѡ
        /// <summary>
        /// ͼƬ��ѡ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox_Advance clickedPictureBox = sender as PictureBox_Advance;
            if (clickedPictureBox == null) return;

            if (ModifierKeys == Keys.Control) // Ctrl + �������ѡ
            {
                ToggleSelectionCtrl(clickedPictureBox);
            }
            else if (ModifierKeys == Keys.Shift) // Shift + �������Χѡ��
            {
                ToggleSelectionShift(clickedPictureBox);
            }
            else // ��ͨ���� ȡ��ѡ������ͼƬ
            {
                ToggleSelectionSingle(clickedPictureBox);
            }


        }

        /// <summary>
        /// Ctrl ��ѡ
        /// </summary>
        /// <param name="clickedPictureBox"></param>
        private void ToggleSelectionCtrl(PictureBox_Advance clickedPictureBox)
        {
            if (selectPictures_List.Contains(clickedPictureBox))
            {
                selectPictures_List.Remove(clickedPictureBox);
                clickedPictureBox.BorderStyle = BorderStyle.None; // ȡ������
            }
            else
            {
                selectPictures_List.Add(clickedPictureBox);
                clickedPictureBox.BorderStyle = BorderStyle.Fixed3D; // ����
            }
        }

        /// <summary>
        /// ��ͨ��ѡ
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
                // ˢ�����
                selectPictures_List[0].BorderStyle = BorderStyle.None;
                selectPictures_List[0] = clickedPictureBox;
            }
            clickedPictureBox.BorderStyle = BorderStyle.Fixed3D;

            // ����Ǳȶ�ģʽ ��Ҫ���±ȶ�ͼƬ
            if (isForm2Open && form2 != null)
            {
                form2.UpdateComparePictureBox(selectPictures_List[0]);
            }

        }

        /// <summary>
        /// Shift ��ѡ
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

        #region ��������

        /// <summary>
        /// �������ͼƬ��Ϣ�б�
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
        /// ����ָ��ͼƬ��Ϣ
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
        /// �������ͼƬUI�б�
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
        /// ���ͼƬUIָ��Ԫ��
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
