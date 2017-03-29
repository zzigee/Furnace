using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.Charting;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using System.ComponentModel;


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.Charting;
using Telerik.WinControls.UI;


namespace FurnaceControl
{


    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        /************************************************************************
         * Define Variables 
         ************************************************************************/
        private MainClass m_MainClass;
        private int val;
        private String query;

        // For Paing Check 
        enum Page { Main, Schedule, Furnace, Grade_Set, L2_Data, Program_Log };
        private int nCurrentPage;



        delegate void SetTextCallback(string text);

        public void Set_txtDanjin_Current_Date(string text)
        {
            if (this.txtDanjin_Current_Date.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Set_txtDanjin_Current_Date);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtDanjin_Current_Date.Text = text;
            }
        }

        public void Set_txtDanjin_Delta_Time(string text)
        {
            if (this.txtDanjin_Delta_Time.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Set_txtDanjin_Delta_Time);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtDanjin_Delta_Time.Text = text;
            }
        }

        public void Set_txtDanjin_Operation_Time(string text)
        {
            if (this.txtDanjin_Operation_Time.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Set_txtDanjin_Operation_Time);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtDanjin_Operation_Time.Text = text;
            }
        }


        /************************************************************************
         * Start Form Load  
         ************************************************************************/
        private void RadForm1_Load(object sender, EventArgs e)
        {
            this.nCurrentPage = (int)Page.Main;

            this.Timer_GUI_Update.Start();
            this.Timer_DB_Update.Start();

            //this.billeT_JOINTableAdapter.Fill(this.furnaceControlDataSet.BILLET_JOIN);
            
            this.tbGrade.ReadOnly = true;
            this.tbSetNo.ReadOnly = true;
            this.tbDetatil.ReadOnly = true;

            InitializeChart();
        }




        /************************************************************************
         * Creator  
         ************************************************************************/
        public MainForm()
        {
            // 메인클래스 객체 생성
            this.m_MainClass = new MainClass(this);

            this.InitializeComponent();

            // 전체 테마 설정 
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Light";
        }







        /************************************************************************
         * Main Form Tab Change Event   
         ************************************************************************/
        private void MainTap_SelectedPageChanged(object sender, EventArgs e)
        {
            switch (MainTap.SelectedPage.Text)
            {
                case "Main":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "Main Tap Changed");
                    this.nCurrentPage = (int)Page.Main;
                    //this.BilletJoinTableAdapter.Fill(this.furnaceControlDataSet.BILLET_JOIN);
                    break;



                case "Schedule":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "Schedule Tap Changed");
                    this.nCurrentPage = (int)Page.Schedule;
                    //this.BilletJoinTableAdapter.FillBy_PreFurnaceBillet(this.furnaceControlDataSet.BILLET_JOIN);
                    break;



                case "Furnace":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "Furnace Tap Changed");
                    this.nCurrentPage = (int)Page.Furnace;
                    //this.sYSTEM_EVENT_LOGTableAdapter.Fill(this.furnaceControlDataSet.SYSTEM_EVENT_LOG);
                    break;



                case "Grade Set":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "Grade Set Tap Changed");
                    this.nCurrentPage = (int)Page.Grade_Set;
                    //this.gradE_TableAdapter.Fill(this.furnaceControlDataSet.GRADE);
                    //this.gradE_DETAILTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);
                    break;



                case "L2 Data":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "L2 Data Tap Changed");
                    this.nCurrentPage = (int)Page.L2_Data;
                    break;



                case "Program Log":
                    this.m_MainClass.m_SysLogClass.DebugLog(this, "Program Log Tap Changed");
                    this.nCurrentPage = (int)Page.Program_Log;
                    //this.systeM_EVENT_JOINTableAdapter.Fill(this.furnaceControlDataSet.SYSTEM_EVENT_JOIN);
                    break;



                default:
                    this.nCurrentPage = 99;
                    break;
            }
        }


        private void InitializeChart()
        {

            CategoricalAxis horizontalAxis = radChartView1.Axes.Get<CategoricalAxis>(0);
            // or verticalAxis = series.VerticalAxis as LinearAxis;
            //verticalAxis.Minimum = -10;
            //verticalAxis.Maximum = 1000; //this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE;
            //verticalAxis.MajorStep = 20;
            //verticalAxis.HorizontalLocation = AxisHorizontalLocation.Left;

            LinearAxis verticalAxis = radChartView1.Axes.Get<LinearAxis>(1);
            // or horizontalAxis = series.HorizontalAxis as LinearAxis;
            verticalAxis.Minimum = 0;
            verticalAxis.Maximum = 2000;
            verticalAxis.MajorStep = 200;



            //verticalAxis = radChartView1.Axes.Get<LinearAxis>(0);
            //// or verticalAxis = series.VerticalAxis as LinearAxis;
            //verticalAxis.Minimum = 0;
            //verticalAxis.Maximum = this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE;
            //verticalAxis.MajorStep = 10;
            //verticalAxis.HorizontalLocation = AxisHorizontalLocation.Left;

            //horizontalAxis = radChartView1.Axes.Get<LinearAxis>(1);
            //// or horizontalAxis = series.HorizontalAxis as LinearAxis;
            //horizontalAxis.Minimum = 0;
            //horizontalAxis.Maximum = 1800;
            //horizontalAxis.MajorStep = 300;
             
            

        }



        /**
         * 외부에서 해당 Form 에 MessageBox 를 띄위기 위한 함수
         */
        public void ShowMessageBox(String message)
        {
            MessageBox.Show(message);
        }



        private void btnGradeInsert_Click(object sender, EventArgs e)
        {
            try
            {
                //this.gradE_TableAdapter.Insert(this.tbGrade.Text, int.Parse(this.tbSetNo.Text), this.tbDetatil.Text);
                //this.ShowMessageBox("정상적으로 처리 되었습니다.");
                //this.gradE_TableAdapter.Fill(this.furnaceControlDataSet.GRADE);
            }
            catch (Exception ex)
            {
                this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
            }
        }



        private void btnGradeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //this.gradE_TableAdapter.UpdateQuery(this.tbGrade.Text, int.Parse(this.tbSetNo.Text), this.tbDetatil.Text, this.tbGrade.Text);
                //this.ShowMessageBox("정상적으로 처리 되었습니다.");
                //this.gradE_TableAdapter.Fill(this.furnaceControlDataSet.GRADE);
            }
            catch (Exception ex)
            {
                this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
            }
        }



        private void btnGradeDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("해당 데이터를 삭제하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //this.gradE_TableAdapter.DeleteQuery(this.tbGrade.Text);
                    //this.ShowMessageBox("정상적으로 처리 되었습니다.");
                    //this.gradE_TableAdapter.Fill(this.furnaceControlDataSet.GRADE);

                }
                catch (Exception ex)
                {
                    this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                    this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
                }
            }
        }



        private void btnGradeClear_Click(object sender, EventArgs e)
        {
            this.tbGrade.PromptChar = '_';
            this.tbSetNo.PromptChar = '_';
            this.tbDetatil.Text = "";
        }



        private void btnGDetail_Insert_Click(object sender, EventArgs e)
        {
            try
            {
                this.gradE_DETAILTableAdapter.InsertQuery(int.Parse(this.tbSetNo_GradeDetail.Text), int.Parse(this.tbAimTemp.Text), int.Parse(this.tbTOPmax_1.Text), int.Parse(this.tbTOPmin_1.Text), int.Parse(this.tbBOTmax_1.Text), int.Parse(this.tbBOTmin_1.Text), int.Parse(this.tbTOPmax_2.Text), int.Parse(this.tbTOPmin_2.Text), int.Parse(this.tbBOTmax_2.Text), int.Parse(this.tbBOTmin_2.Text), int.Parse(this.tbTOPmax_3.Text), int.Parse(this.tbTOPmin_3.Text), int.Parse(this.tbBOTmax_3.Text), int.Parse(this.tbBOTmin_3.Text), int.Parse(this.tbTOPmax_4.Text), int.Parse(this.tbTOPmin_4.Text), int.Parse(this.tbBOTmax_4.Text), int.Parse(this.tbBOTmin_4.Text), int.Parse(this.tbTOPmax_5.Text), int.Parse(this.tbTOPmin_5.Text), int.Parse(this.tbBOTmax_5.Text), int.Parse(this.tbBOTmin_5.Text), int.Parse(this.tbTOPmax_6.Text), int.Parse(this.tbTOPmin_6.Text), int.Parse(this.tbBOTmax_6.Text), int.Parse(this.tbBOTmin_6.Text), int.Parse(this.tbTOPmax_7.Text), int.Parse(this.tbTOPmin_7.Text), int.Parse(this.tbBOTmax_7.Text), int.Parse(this.tbBOTmin_7.Text), int.Parse(this.tbTOPmax_8.Text), int.Parse(this.tbTOPmin_8.Text), int.Parse(this.tbBOTmax_8.Text), int.Parse(this.tbBOTmin_8.Text), int.Parse(this.tbTOPmax_9.Text), int.Parse(this.tbTOPmin_9.Text), int.Parse(this.tbBOTmax_9.Text), int.Parse(this.tbBOTmin_9.Text), int.Parse(this.tbTOPmax_10.Text), int.Parse(this.tbTOPmin_10.Text), int.Parse(this.tbBOTmax_10.Text), int.Parse(this.tbBOTmin_10.Text));

                this.ShowMessageBox("정상적으로 처리 되었습니다.");
                this.gradE_DETAILTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);
            }
            catch (Exception ex)
            {
                this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
            }
        }



        private void btnGDetail_Update_Click(object sender, EventArgs e)
        {
            try
            {
                this.gradE_DETAILTableAdapter.UpdateQuery(int.Parse(this.tbSetNo_GradeDetail.Text), int.Parse(this.tbAimTemp.Text), int.Parse(this.tbTOPmax_1.Text), int.Parse(this.tbTOPmin_1.Text), int.Parse(this.tbBOTmax_1.Text), int.Parse(this.tbBOTmin_1.Text), int.Parse(this.tbTOPmax_2.Text), int.Parse(this.tbTOPmin_2.Text), int.Parse(this.tbBOTmax_2.Text), int.Parse(this.tbBOTmin_2.Text), int.Parse(this.tbTOPmax_3.Text), int.Parse(this.tbTOPmin_3.Text), int.Parse(this.tbBOTmax_3.Text), int.Parse(this.tbBOTmin_3.Text), int.Parse(this.tbTOPmax_4.Text), int.Parse(this.tbTOPmin_4.Text), int.Parse(this.tbBOTmax_4.Text), int.Parse(this.tbBOTmin_4.Text), int.Parse(this.tbTOPmax_5.Text), int.Parse(this.tbTOPmin_5.Text), int.Parse(this.tbBOTmax_5.Text), int.Parse(this.tbBOTmin_5.Text), int.Parse(this.tbTOPmax_6.Text), int.Parse(this.tbTOPmin_6.Text), int.Parse(this.tbBOTmax_6.Text), int.Parse(this.tbBOTmin_6.Text), int.Parse(this.tbTOPmax_7.Text), int.Parse(this.tbTOPmin_7.Text), int.Parse(this.tbBOTmax_7.Text), int.Parse(this.tbBOTmin_7.Text), int.Parse(this.tbTOPmax_8.Text), int.Parse(this.tbTOPmin_8.Text), int.Parse(this.tbBOTmax_8.Text), int.Parse(this.tbBOTmin_8.Text), int.Parse(this.tbTOPmax_9.Text), int.Parse(this.tbTOPmin_9.Text), int.Parse(this.tbBOTmax_9.Text), int.Parse(this.tbBOTmin_9.Text), int.Parse(this.tbTOPmax_10.Text), int.Parse(this.tbTOPmin_10.Text), int.Parse(this.tbBOTmax_10.Text), int.Parse(this.tbBOTmin_10.Text), int.Parse(this.tbSetNo_GradeDetail.Text));

                this.ShowMessageBox("정상적으로 처리 되었습니다.");
                this.gradE_DETAILTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);
            }
            catch (Exception ex)
            {
                this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
            }
        }



        private void btnGDetail_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당 데이터를 삭제하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    this.gradE_DETAILTableAdapter.DeleteQuery(int.Parse(this.tbSetNo_GradeDetail.Text));
                    this.ShowMessageBox("정상적으로 처리 되었습니다.");
                    this.gradE_DETAILTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);
                }
                catch (Exception ex)
                {
                    this.ShowMessageBox("오류가 발생하였습니다. Message {" + ex.ToString() + "}");
                    this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
                }
            }
        }



        private void btnGDetail_Clear_Click(object sender, EventArgs e)
        {
            this.tbSetNo_GradeDetail.Text = "";
            this.tbAimTemp.Text = "";
            this.tbTOPmax_1.Text = "";
            this.tbTOPmin_1.Text = "";
            this.tbBOTmax_1.Text = "";
            this.tbBOTmin_1.Text = "";
            this.tbTOPmax_1.Text = "";
            this.tbTOPmin_1.Text = "";
            this.tbBOTmax_1.Text = "";
            this.tbBOTmin_1.Text = "";
            this.tbTOPmax_2.Text = "";
            this.tbTOPmin_2.Text = "";
            this.tbBOTmax_2.Text = "";
            this.tbBOTmin_2.Text = "";
            this.tbTOPmax_3.Text = "";
            this.tbTOPmin_3.Text = "";
            this.tbBOTmax_3.Text = "";
            this.tbBOTmin_3.Text = "";
            this.tbTOPmax_4.Text = "";
            this.tbTOPmin_4.Text = "";
            this.tbBOTmax_4.Text = "";
            this.tbBOTmin_4.Text = "";
            this.tbTOPmax_5.Text = "";
            this.tbTOPmin_5.Text = "";
            this.tbBOTmax_5.Text = "";
            this.tbBOTmin_5.Text = "";
            this.tbTOPmax_6.Text = "";
            this.tbTOPmin_6.Text = "";
            this.tbBOTmax_6.Text = "";
            this.tbBOTmin_6.Text = "";
            this.tbTOPmax_7.Text = "";
            this.tbTOPmin_7.Text = "";
            this.tbBOTmax_7.Text = "";
            this.tbBOTmin_7.Text = "";
            this.tbTOPmax_8.Text = "";
            this.tbTOPmin_8.Text = "";
            this.tbBOTmax_8.Text = "";
            this.tbBOTmin_8.Text = "";
            this.tbTOPmax_9.Text = "";
            this.tbTOPmin_9.Text = "";
            this.tbBOTmax_9.Text = "";
            this.tbBOTmin_9.Text = "";
            this.tbTOPmax_10.Text = "";
            this.tbTOPmin_10.Text = "";
            this.tbBOTmax_10.Text = "";
            this.tbBOTmin_10.Text = "";
        }



        private void radButton8_Click(object sender, EventArgs e)
        {
            if (this.m_MainClass.isLoginUser == false)
            {
                if (this.txID.Text == "" || this.txPWD.Text == "")
                {
                    MessageBox.Show("ID 와 Password 를 입력해 주세요");
                }
                else
                {
                    if (this.m_MainClass.m_SQLClass.getUserInfo(this.txID.Text, this.txPWD.Text) > 0)
                    {
                        this.m_MainClass.isLoginUser = true;
                        this.btnLogin.BackColor = Color.Lime;
                        MessageBox.Show("로그인 되었습니다.");
                        this.btnLogin.Text = "Disable Editing";

                        this.btnGDetail_Clear.Enabled = true;
                        this.btnGDetail_Delete.Enabled = true;
                        this.btnGDetail_Insert.Enabled = true;
                        this.btnGDetail_Update.Enabled = true;
                        this.btnGradeClear.Enabled = true;
                        this.btnGradeDelete.Enabled = true;
                        this.btnGradeInsert.Enabled = true;
                        this.btnGradeUpdate.Enabled = true;


                        this.tbGrade.ReadOnly = false;
                        this.tbSetNo.ReadOnly = false;
                        this.tbDetatil.ReadOnly = false;
                    }
                    else
                    {
                        MessageBox.Show("ID 와 Password 가 잘못 입력 되었습니다.");
                    }
                }
            }
            else
            {
                this.m_MainClass.isLoginUser = false;
                MessageBox.Show("로그아웃 되었습니다.");
                this.btnLogin.BackColor = Color.FromArgb(233, 240, 249);
                this.btnLogin.Text = "Enable Editing";

                this.btnGDetail_Clear.Enabled = false;
                this.btnGDetail_Delete.Enabled = false;
                this.btnGDetail_Insert.Enabled = false;
                this.btnGDetail_Update.Enabled = false;
                this.btnGradeClear.Enabled = false;
                this.btnGradeDelete.Enabled = false;
                this.btnGradeInsert.Enabled = false;
                this.btnGradeUpdate.Enabled = false;

                this.tbGrade.ReadOnly = true;
                this.tbSetNo.ReadOnly = true;
                this.tbDetatil.ReadOnly = true;
            }
        }



        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.m_MainClass.m_SQLClass.SqlCon != null) this.m_MainClass.m_SQLClass.SqlCon.Close();
            Application.Exit();
        }



        private void gridView_Grade_Click(object sender, EventArgs e)
        {
            var view = sender as RadGridView;

            //MessageBox.Show(view.SelectedRows.ToString());
        }





        private void sYSTEMEVENTLOGBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.m_MainClass.m_SysLogClass.DebugLog(this, "sYSTEMEVENTLOGBindingSource_CurrentChanged");
        }



        private void Timer_DB_Update_Tick(object sender, EventArgs e)
        {
            if (this.nCurrentPage == (int)Page.Main)
            {
                //this.m_MainClass.m_SQLClass.updateBilletStruct();        // Update Data


                DataRow row;
                Random rnd = new Random();

                this.furnaceControlDataSet.DataTable1.Clear();
                for (int i = 0; i < 50; i++)
                {
                    row = this.furnaceControlDataSet.DataTable1.NewRow();
                    row["data"] = i;
                    row["value"] = rnd.Next(0, 1600);
                    //this.furnaceControlDataSet.DataTable1.Rows.Add(row);
                    //this.furnaceControlDataSet.DataTable1.AcceptChanges();
                }

                //this.charT_VIEW_BILLETTableAdapter.Fill(this.furnaceControlDataSet.CHART_VIEW_BILLET);
                //this.charT_VIEW_ZONE_STATUSTableAdapter.Fill(this.furnaceControlDataSet.CHART_VIEW_ZONE_STATUS);





            }
            else if (this.nCurrentPage == (int)Page.Schedule)
            {

            }
            else if (this.nCurrentPage == (int)Page.Furnace)
            {

            }
            else if (this.nCurrentPage == (int)Page.Grade_Set)
            {
            }
            else if (this.nCurrentPage == (int)Page.L2_Data)
            {

            }
            else if (this.nCurrentPage == (int)Page.Program_Log)
            {

            }

            //this.m_MainClass.m_SysLogClass.SystemLog(this, "Call Timer DB Update Timer");

        }


        /*
 * Chart Update 
 * public string nBillet_Order_Number;                       // 빌렛 주문번호
 * public string nBillet_Heat_Id;                            // 빌렛 히트번호
 * public int nBillet_Charge_Temperature;                    // 빌렛 장입온도
 * public int nBillet_Discharge_Target_Temperature;          // 빌렛 목표 배출 온도    
 * public int nBillet_Predict_Discharge_Billet_Temperature;  // 빌렛 예상 배출 온도
 * public int nBillet_Predict_Current_Billet_Temperature;    // 현재 빌렛 예상 온도
 * public int nOrderOfBillet;                                // 가열로내 소재 순서 
 * public int nZone_Average_Temperature;                     // 존 평균 온도 
 */
        private void RefreshChartViewer()
        {
            //this.radChartView.Update();
            //this.radChartView1.Update();

            SteplineSeries series_zone_temp = new SteplineSeries();
            LineSeries series_billet_temp = new LineSeries();

            if (radChartView1.Series != null) radChartView1.Series.Clear();
            
            //for (int i = 0; i < this.m_MainClass.stDANJIN_STRUCT.nDataCount; i++)
            for (int i = 0; i < this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN ; i++)
            {
                if (this.m_MainClass.stDANJIN_STRUCT.nDataCount >= i)
                {
                    series_zone_temp.DataPoints.Add(new CategoricalDataPoint(this.m_MainClass.stDANJIN_STRUCT.strZoneTemp[i], i));
                    series_billet_temp.DataPoints.Add(new CategoricalDataPoint(this.m_MainClass.stDANJIN_STRUCT.strBilletPredictTemp[i], i));
                }
                else
                {
                //    series_zone_temp.DataPoints.Add(new CategoricalDataPoint(null, i));
                //    series_billet_temp.DataPoints.Add(new CategoricalDataPoint(null, i));
                }
            }
            radChartView1.Series.Add(series_zone_temp);
            radChartView1.Series.Add(series_billet_temp);

        }


        private void Timer_Update_GUI(object sender, EventArgs e)
        {

            if (this.nCurrentPage == (int)Page.Main)
            {
                //this.BilletJoinTableAdapter.Fill(this.furnaceControlDataSet.BILLET_JOIN);
                //this.GradeDetailTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);

                RefreshChartViewer();
            }
            else if (this.nCurrentPage == (int)Page.Schedule)
            {
                // this.BilletJoinTableAdapter.FillBy_PreFurnaceBillet(this.furnaceControlDataSet.BILLET_JOIN);
            }
            else if (this.nCurrentPage == (int)Page.Furnace)
            {

            }
            else if (this.nCurrentPage == (int)Page.Grade_Set)
            {

            }
            else if (this.nCurrentPage == (int)Page.L2_Data)
            {

            }
            else if (this.nCurrentPage == (int)Page.Program_Log)
            {

            }

            //this.BilletJoinTableAdapter.ClearBeforeFill = true;
            //this.BilletJoinTableAdapter.Fill(this.furnaceControlDataSet.BILLET_JOIN);           
            // TODO: 이 코드는 데이터를 'furnaceControlDataSet.SYSTEM_EVENT_JOIN' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            //this.sYSTEM_EVENT_JOINTableAdapter.Fill(this.furnaceControlDataSet.SYSTEM_EVENT_JOIN);
            // TODO: 이 코드는 데이터를 'furnaceControlDataSet.GRADE_DETAIL' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            //this.GradeDetailTableAdapter.Fill(this.furnaceControlDataSet.GRADE_DETAIL);
            // TODO: 이 코드는 데이터를 'furnaceControlDataSet.GRADE' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            //this.GradeTableAdapter.Fill(this.furnaceControlDataSet.GRADE);

            //this.m_MainClass.m_SysLogClass.SystemLog(this, "Call Timer Update GUI Timer");
        }

        private void gridView_Grade_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            var view = sender as RadGridView;
            int idx = e.CurrentRow.Index;

            this.tbGrade.Text = view.Rows[idx].Cells[0].Value.ToString();
            this.tbSetNo.Text = view.Rows[e.CurrentRow.Index].Cells[1].Value.ToString();
            this.tbDetatil.Text = view.Rows[e.CurrentRow.Index].Cells[2].Value.ToString();
        }

        private void gridView_GradeDetail_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            var view = sender as RadGridView;
            int idx = e.CurrentRow.Index;

            this.tbSetNo_GradeDetail.Text = view.Rows[idx].Cells[0].Value.ToString();
            this.tbAimTemp.Text = view.Rows[idx].Cells[1].Value.ToString();

            this.tbTOPmax_1.Text = view.Rows[idx].Cells[2].Value.ToString();
            this.tbTOPmin_1.Text = view.Rows[idx].Cells[2].Value.ToString();
            this.tbBOTmax_1.Text = view.Rows[idx].Cells[3].Value.ToString();
            this.tbBOTmin_1.Text = view.Rows[idx].Cells[4].Value.ToString();

            this.tbTOPmax_2.Text = view.Rows[idx].Cells[5].Value.ToString();
            this.tbTOPmin_2.Text = view.Rows[idx].Cells[6].Value.ToString();
            this.tbBOTmax_2.Text = view.Rows[idx].Cells[7].Value.ToString();
            this.tbBOTmin_2.Text = view.Rows[idx].Cells[8].Value.ToString();

            this.tbTOPmax_3.Text = view.Rows[idx].Cells[9].Value.ToString();
            this.tbTOPmin_3.Text = view.Rows[idx].Cells[10].Value.ToString();
            this.tbBOTmax_3.Text = view.Rows[idx].Cells[11].Value.ToString();
            this.tbBOTmin_3.Text = view.Rows[idx].Cells[12].Value.ToString();

            this.tbTOPmax_4.Text = view.Rows[idx].Cells[13].Value.ToString();
            this.tbTOPmin_4.Text = view.Rows[idx].Cells[14].Value.ToString();
            this.tbBOTmax_4.Text = view.Rows[idx].Cells[15].Value.ToString();
            this.tbBOTmin_4.Text = view.Rows[idx].Cells[16].Value.ToString();

            this.tbTOPmax_5.Text = view.Rows[idx].Cells[17].Value.ToString();
            this.tbTOPmin_5.Text = view.Rows[idx].Cells[18].Value.ToString();
            this.tbBOTmax_5.Text = view.Rows[idx].Cells[19].Value.ToString();
            this.tbBOTmin_5.Text = view.Rows[idx].Cells[20].Value.ToString();

            this.tbTOPmax_6.Text = view.Rows[idx].Cells[21].Value.ToString();
            this.tbTOPmin_6.Text = view.Rows[idx].Cells[22].Value.ToString();
            this.tbBOTmax_6.Text = view.Rows[idx].Cells[23].Value.ToString();
            this.tbBOTmin_6.Text = view.Rows[idx].Cells[24].Value.ToString();

            this.tbTOPmax_7.Text = view.Rows[idx].Cells[25].Value.ToString();
            this.tbTOPmin_7.Text = view.Rows[idx].Cells[26].Value.ToString();
            this.tbBOTmax_7.Text = view.Rows[idx].Cells[27].Value.ToString();
            this.tbBOTmin_7.Text = view.Rows[idx].Cells[28].Value.ToString();

            this.tbTOPmax_8.Text = view.Rows[idx].Cells[29].Value.ToString();
            this.tbTOPmin_8.Text = view.Rows[idx].Cells[30].Value.ToString();
            this.tbBOTmax_8.Text = view.Rows[idx].Cells[31].Value.ToString();
            this.tbBOTmin_8.Text = view.Rows[idx].Cells[32].Value.ToString();

            this.tbTOPmax_9.Text = view.Rows[idx].Cells[33].Value.ToString();
            this.tbTOPmin_9.Text = view.Rows[idx].Cells[34].Value.ToString();
            this.tbBOTmax_9.Text = view.Rows[idx].Cells[35].Value.ToString();
            this.tbBOTmin_9.Text = view.Rows[idx].Cells[36].Value.ToString();

            this.tbTOPmax_10.Text = view.Rows[idx].Cells[37].Value.ToString();
            this.tbTOPmin_10.Text = view.Rows[idx].Cells[38].Value.ToString();
            this.tbBOTmax_10.Text = view.Rows[idx].Cells[39].Value.ToString();
            this.tbBOTmin_10.Text = view.Rows[idx].Cells[40].Value.ToString();

        }

        private void btnDataLogging_Click(object sender, EventArgs e)
        {
            if (this.m_MainClass.m_Define_Class.isDataLogging == false) 
            {
                this.m_MainClass.m_Define_Class.isDataLogging = true;
                this.btnDataLogging.BackColor = Color.Lime;

                this.txtDanjin_Delta_Time.Text = this.m_MainClass.m_Define_Class.nDangjinThermalCalPeriod.ToString();
                this.m_MainClass.m_Define_Class.nDataLoggingIndex = 0;    // 열모델 계산 결과 배열 인덱스 초기화 
 
                this.m_MainClass.m_Define_Class.dateDataLoggingStartTime = DateTime.Now;
                this.txtDanjin_Start_Date.Text = this.m_MainClass.m_Define_Class.dateDataLoggingStartTime.ToString();

                // Initialize Struct for Dangjin 
                this.m_MainClass.stDANJIN_STRUCT = new DefineClass.ST_DANJIN_STURCT();      // 당진 테스트 용 
                this.m_MainClass.stDANJIN_STRUCT.nDataCount = 0;
                this.m_MainClass.stDANJIN_STRUCT.strCreateTime = new string[this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];
                this.m_MainClass.stDANJIN_STRUCT.strZoneTemp = new int[this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];
                this.m_MainClass.stDANJIN_STRUCT.strBilletPredictTemp = new int[this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];
                this.m_MainClass.stDANJIN_STRUCT.strStartTime = DateTime.Now.ToString();
                
            }
            else
            {
                this.m_MainClass.m_Define_Class.isDataLogging = false; 
                this.btnDataLogging.BackColor = Color.LightGray;

                this.m_MainClass.stDANJIN_STRUCT.strStartTime = DateTime.Now.ToString();
            }
        }

        private void btnOPC_Connect_Click(object sender, System.EventArgs e)
        {
            this.m_MainClass.m_L1LinkClass.InitializeOPC(txtOPCProgID.Text, txtOPCServerAddress.Text);
        }

        private void radButton1_Click(object sender, System.EventArgs e)
        {
            m_MainClass.m_L1LinkClass.getReadGroupTags("IncrementGroup");
        }
    }
}