using System;
using System.Linq;

namespace FurnaceControl
{
    internal class ThermalModelClass : TimerClass
    {
        private readonly MainClass m_MainClass;
        
        public ThermalModelClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "ThermalModelClassTimer");
        }

        /**
         * 주기적으로 실행하는 함수 
         **/
        public override void Run()
        {

            this.m_MainClass.m_SysLogClass.SystemLog(this, "ThermalModelClassTimer");


        }
    }
}