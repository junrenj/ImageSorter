using System.ComponentModel;
using ImageSorter.Data;
using ImageSorter.Logic;

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
        private E_ImgSize imgSize = E_ImgSize.All;
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
        /// �����ѡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ClearSlect_Click(object sender, EventArgs e)
        {
            foreach (var item in selectPictures_List)
            {
                pictureBoxes_List.Remove(item);
                mainPanel.Controls.Remove(item);
                importedImage_List.Remove(item.img_data);
                item.Destroy();
            }
            // ����һ�� �ÿ�
            if (activePictureBox != null)
            {
                activePictureBox.BorderStyle = BorderStyle.None;
                activePictureBox.BorderColor = Color.Transparent;
                activePictureBox = null;
            }
            // GC �ͷŻ���
            GC.Collect();
            GC.WaitForPendingFinalizers();
            // ˢ�����
            RefreshFlowLayoutPanel();
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
        /// ���㰴ť����
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

            progressBar_Calculate.Value = 0; // ��������ʼ��
            progressBar_Calculate.Visible = true;

            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// ��������ʼ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ulong t = 0;

            switch (sortType)
            {
                case E_SortType.AHash:
                    t = activePictureBox.img_data.GetImageAHash();
                    for (int i = 0; i < importedImage_List.Count; i++)
                    {
                        importedImage_List[i].GetImageAHash();
                        importedImage_List[i].HammingDistance(t);

                        // ���½���
                        int progress = (i + 1) * 100 / importedImage_List.Count;
                        worker.ReportProgress(progress);
                    }
                    break;

                case E_SortType.DHash:
                    t = activePictureBox.img_data.GetImageDHash();
                    for (int i = 0; i < importedImage_List.Count; i++)
                    {
                        importedImage_List[i].GetImageDHash();
                        importedImage_List[i].HammingDistance(t);

                        worker.ReportProgress((i + 1) * 100 / importedImage_List.Count);
                    }
                    break;

                case E_SortType.HistogramCompare:
                    activePictureBox.img_data.GenerateLowResolutionMap(GlobalConstants.h_binSize);
                    int[] h = activePictureBox.img_data.GetHistogram(activePictureBox.img_data.lowRes_Img);
                    for (int i = 0; i < importedImage_List.Count; i++)
                    {
                        importedImage_List[i].CompareHistogram(h);
                        worker.ReportProgress((i + 1) * 100 / importedImage_List.Count);
                    }
                    break;
                case E_SortType.SSIM:
                    if (imgSize == E_ImgSize.All || imgSize == E_ImgSize.Others)
                    {
                        MessageBox.Show("��ѡ��һ��ͼƬ�ߴ��ٽ��в���");
                        worker.ReportProgress(100);
                        return;
                    }
                    else if (activePictureBox.img_data.imgSize != imgSize)
                    {
                        MessageBox.Show("Ŀ��ͼƬ��ɸѡ���ߴ�");
                    }
                    else
                    {
                        //RefreshFlowLayoutPanel();
                        Bitmap b = new Bitmap(activePictureBox.Image);
                        for (int i = 0; i < mainPanel.Controls.Count; i++)
                        {
                            PictureBox_Advance p = mainPanel.Controls[i] as PictureBox_Advance;
                            p.img_data.CalculateSSIM(b);
                            worker.ReportProgress((i + 1) * 100 / mainPanel.Controls.Count);
                        }
                    }
                    break;
                case E_SortType.EMD:
                    int[] emd = activePictureBox.img_data.ComputeHueHistogram(); ;
                    for (int i = 0; i < importedImage_List.Count; i++)
                    {
                        importedImage_List[i].CalculateEMD_Hue(emd);
                        worker.ReportProgress((i + 1) * 100 / importedImage_List.Count);
                    }
                    break;
            }

            pictureBoxes_List.Sort(new DistanceComparer());
        }

        /// <summary>
        /// ���¼������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar_Calculate.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RemapFilterValue();

            // ˢ�����
            RefreshFlowLayoutPanel();

            progressBar_Calculate.Visible = false; // ���ؽ�����
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
                case "�ṹ����ָ�� SSIM":
                    CtrlHPanelVisibile(false);
                    sortType = E_SortType.SSIM;
                    MessageBox.Show("SSIM��֧�ֲ�����ͬ�ߴ��ͼƬ");
                    break;
                case "ֱ��ͼ�ƶ����� EMD":
                    CtrlHPanelVisibile(false);
                    sortType = E_SortType.EMD;
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
                RefreshFlowLayoutPanel();
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
            }
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
        /// ͼƬ�ߴ�ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectImgSizeBox_OnChnaged(object sender, EventArgs e)
        {
            string selectSize = selectBox_ImgSize.SelectedItem.ToString();
            switch (selectSize)
            {
                case "512":
                    imgSize = E_ImgSize.S_512;
                    break;
                case "1024":
                    imgSize = E_ImgSize.S_1024;
                    break;
                case "2048":
                    imgSize = E_ImgSize.S_2048;
                    break;
                case "None":
                    if (sortType == E_SortType.SSIM)
                    {
                        MessageBox.Show("SSIM�㷨һ��Ҫѡ��һ��ͼƬ�ߴ�");
                        selectBox_ImgSize.Text = "512";
                        return;
                    }
                    else
                        imgSize = E_ImgSize.All;
                    break;
            }

            RefreshFlowLayoutPanel();
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
            if (mainPanel.Controls.Count == 0)
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
                    if (deleteLogic.DeleteMultipleImgs(executeList))
                    {
                        // �ͷ�ͼƬ��Ϣ�洢�ṹ�еĻ���
                        ClearImageList();
                        // �ͷ�ͼƬUI�еĻ���
                        ClearPictureboxList();
                        // ˢ�����
                        RefreshFlowLayoutPanel();
                    }
                    break;
                // ɾ��������ѡͼƬ
                case E_ExecuteType.DeleteSelect:
                    if (selectPictures_List.Count == 0)
                    {
                        MessageBox.Show("��ѡ��ͼƬ");
                        return;
                    }
                    if (deleteLogic.DeleteMultipleImgs(selectPictures_List))
                    {
                        // �ͷ�ͼƬ��Ϣ�洢�ṹ�еĻ���
                        ClearImageListFromPictureBoxList(selectPictures_List);
                        // �ͷ�ͼƬUI�еĻ���
                        ClearPictureboxListFromList(selectPictures_List);
                        // ˢ�����
                        RefreshFlowLayoutPanel();
                    }
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
            exportLogic.ExportImgs(exportList, progressBar_Export);
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

        #region UI��巽��(ˢ��)

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
        /// ˢ�����
        /// </summary>
        private void RefreshFlowLayoutPanel()
        {
            mainPanel.Controls.Clear();
            if (!similarFilterOn && imgSize == E_ImgSize.All)
            {
                foreach (var pictureBox in pictureBoxes_List)
                {
                    mainPanel.Controls.Add(pictureBox);
                }
            }
            else if (similarFilterOn && imgSize == E_ImgSize.All)
            {
                foreach (var pictureBox in pictureBoxes_List)
                {
                    if (remapValue >= pictureBox.img_data.distance)
                    {
                        mainPanel.Controls.Add(pictureBox);
                    }
                }
            }
            else if (!similarFilterOn && imgSize != E_ImgSize.All)
            {
                foreach (var pictureBox in pictureBoxes_List)
                {
                    if (pictureBox.img_data.imgSize == imgSize)
                    {
                        mainPanel.Controls.Add(pictureBox);
                    }
                }
            }
            else
            {
                foreach (var pictureBox in pictureBoxes_List)
                {
                    if (remapValue >= pictureBox.img_data.distance)
                    {
                        if (imgSize == E_ImgSize.All || pictureBox.img_data.imgSize == imgSize)
                            mainPanel.Controls.Add(pictureBox);
                    }
                }
            }
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
            foreach (var pb in clearList)
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
