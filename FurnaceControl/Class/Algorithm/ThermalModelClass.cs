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

        /*
         * 열모델계산 함수 
         */
        public void calThermalModel(UInt64 idx)
        {
            // 현재 가열로의 분위기 온도 읽기 
            double dCurrentZoneTemp = m_MainClass.stFURNACE_REALTIME_INFORMATION.dZone_Temp;





            // 열모델을 통해 계산 및 저장되는 변수 
            double dZone_Temp = 0.0, dPredict_Billet_Temp = 0.0;

            /* 
             * 
             * 
             * 열모델 계산 코드 
             * 
             * 
             */

            // 계산 시점의 가열로 분위기 온도 및 예측 빌렛온도 저장 
            m_MainClass.stTHERAMLMODEL_FOR_DANJIN[idx].dZone_Temp = dZone_Temp;
            m_MainClass.stTHERAMLMODEL_FOR_DANJIN[idx].dPredict_Billet_Temp = dPredict_Billet_Temp;
        }


        /**
         * 주기적으로 실행하는 함수 (Period : 10 second)
         **/
        UInt64 idx = 0;

        public override void Run()
        {

            this.m_MainClass.m_SysLogClass.SystemLog(this, "ThermalModelClassTimer");
            
            /*
             * 10 초마다 한번씩 열모델 계산 수행 (수행 시간은 MainClass 에서 변경 가능)
             */
            calThermalModel(idx);
            
            idx++;

        }
    }
}