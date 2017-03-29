using System;
using System.Linq;

namespace FurnaceControl
{
    internal class TrackingClass : TimerClass
    {
        private readonly MainClass m_MainClass;

        public TrackingClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "TrackingClassTimer");
        }

        /**
         * 주기적으로 실행하는 함수
         * 1. Receive PLC Data from L1 (using OPC) 
         * 2. Update L1 Data on L2 (Struct and SQL) 
         * 3. Send PLC Data to L1 (using OPC)
         **/
        public override void Run()
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, "TrackingClassTimer");

            /*
            * Start Data Logging 이 활성화 된 경우 실행 
            * 실행 주기는 열모델 연산 Delta 시간과 동일하게 DB_Timer 에 설정하면 됨. 
            */
            if (this.m_MainClass.m_Define_Class.isDataLogging)
            {
                // 당진테스트용 
                Danjin_Timer_Call_Method();

            }

        }


        /*
         * Start Data Logging 이 활성화 된 경우 실행 
         * 실행 주기는 열모델 연산 Delta 시간과 동일하게 DB_Timer 에 설정하면 됨. 
         */

        private void Danjin_Timer_Call_Method()
        {
           
            this.m_MainClass.m_Define_Class.nDataLoggingIndex++;
            this.m_MainClass.m_MainForm.Set_txtDanjin_Current_Date(DateTime.Now.ToString());
            TimeSpan result = DateTime.Now - this.m_MainClass.m_Define_Class.dateDataLoggingStartTime;
            this.m_MainClass.m_MainForm.Set_txtDanjin_Operation_Time("[" + this.m_MainClass.m_Define_Class.nDataLoggingIndex + "]" + result.ToString(@"h\:mm\:ss"));
            
            Random rnd = new Random();

            if(this.m_MainClass.stDANJIN_STRUCT.nDataCount > this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN)
            {
                this.m_MainClass.stDANJIN_STRUCT.nDataCount = 0;
            }

            this.m_MainClass.m_MainForm.dangjiN_DATATableAdapter.InsertQuery(
                this.m_MainClass.m_Define_Class.nDataLoggingIndex, 
                DateTime.Now.ToString(), 
                rnd.Next(1600).ToString(),
                rnd.Next(1600).ToString());

            this.m_MainClass.stDANJIN_STRUCT.strBilletPredictTemp[this.m_MainClass.stDANJIN_STRUCT.nDataCount] = rnd.Next(1600);
            this.m_MainClass.stDANJIN_STRUCT.strZoneTemp[this.m_MainClass.stDANJIN_STRUCT.nDataCount] = 1600;
            this.m_MainClass.stDANJIN_STRUCT.nDataCount++;
        }
    }
}