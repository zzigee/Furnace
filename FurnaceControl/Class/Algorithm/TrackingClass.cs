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
            this.m_MainClass.m_SysLogClass.SystemLog(this, "TrackingClassTimer");
        }

        private void updateChartInformation()
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, Convert.ToString(this.m_MainClass.m_SQLClass.updateChartStruct()));
        }
    }
}