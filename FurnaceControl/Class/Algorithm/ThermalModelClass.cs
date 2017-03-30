using System;
using System.Linq;

namespace FurnaceControl
{
    internal class ThermalModelClass : TimerClass
    {
        private readonly MainClass m_MainClass;

        private double dTotalCalTime = 0.0;

        public ThermalModelClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "ThermalModelClassTimer");
        }
        
        /*********************************************************************************************
         *********************************************************************************************
         * 열모델 계산 코드 
         *********************************************************************************************
         *********************************************************************************************/
        public void calThermalModel()
        {
            this.m_MainClass.m_MainForm.Set_txtDanjin_Current_Date(DateTime.Now.ToString());
            TimeSpan result = DateTime.Now - this.m_MainClass.m_Define_Class.dateDataLoggingStartTime;
            this.m_MainClass.m_MainForm.Set_txtDanjin_Operation_Time("[" + this.m_MainClass.m_Define_Class.nDataLoggingIndex + "]" + result.ToString(@"h\:mm\:ss"));

            if (this.m_MainClass.m_Define_Class.nDataLoggingIndex > this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE)
            {
                this.m_MainClass.m_MainForm.ShowMessageBox("당진 테스트베드 측정 데이터 갯수가 최대 갯수를 초과하였습니다. \n\r측정을 중지합니다.");
                this.m_MainClass.m_MainForm.btnDataLogging.BackColor = System.Drawing.Color.LightGray;
                this.m_MainClass.m_Define_Class.isDataLogging = false;
            }

            Random rnd = new Random();

            int nPreditBilletTemp;
            int nZoneTemprature = this.m_MainClass.stFURNACE_REALTIME_INFORMATION.nZone_Temperature[0];     // 현재 TC 온도 



            /** 
             * 이 값 구해주세요. 
             */



            nPreditBilletTemp = rnd.Next(1600);     



            /**
             */




            this.m_MainClass.m_MainForm.dANGJIN_DATATableAdapter.InsertQuery(    
                this.m_MainClass.m_Define_Class.nDataLoggingIndex,
                DateTime.Now.ToString(),
                nZoneTemprature.ToString(),
                nPreditBilletTemp.ToString());

            this.m_MainClass.stBILLET_INFOMATION[this.m_MainClass.m_Define_Class.nDataLoggingIndex].nBillet_Predict_Current_Billet_Temperature = nPreditBilletTemp;
            this.m_MainClass.stBILLET_INFOMATION[this.m_MainClass.m_Define_Class.nDataLoggingIndex].nZone_Average_Temperature = nZoneTemprature;
        }



        public override void Run()
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, "ThermalModelClassTimer");

            /*
            Start Data Logging 이 활성화 된 경우 실행 
            실행 주기는 열모델 연산 Delta 시간과 동일하게 DB_Timer 에 설정하면 됨. 
            */
            if (this.m_MainClass.m_Define_Class.isDataLogging)
            {
                calThermalModel();
                this.m_MainClass.m_Define_Class.nDataLoggingIndex++;    // 
            }
        }
    }
}