using System;
using System.Linq;

namespace FurnaceControl
{
    internal class DefineClass
    {
        /*
         * [Check - 2016.02.22] -> [Result - ] 
         * 차후 프로그램 배포시 자유롭게 수정이 가능하도록 공업로 최대 영역 수를 외부 파일 참조로 변경 필요  
         **/
        public readonly int MAX_ZONE_IN_FURNACE = 10;// 가열 존의 갯수 
        public readonly int MAX_BILLET_IN_FURNACE = 50;// 로내 최대 빌렛 갯수 

        // Parent Object Class 객체 생성 
        public MainClass m_MainClass;
        
        public int nBillet_Count_On_FUrnace;

        public DefineClass(MainClass mc)
        {
            this.m_MainClass = mc;
        }

        public enum RETURN_VALUE
        {
            SUCCESS,
            FAULT
        }

        public enum TIMER_INTERVAL
        {
            ONE_SEC = 1000,
            TWO_SEC = ONE_SEC * 2,
            THREE_SEC = ONE_SEC * 3,
            FOUR_SEC = ONE_SEC * 4,
            FIVE_SEC = ONE_SEC * 5,
            TEN_SEC = 10000,
            TWENTY_SEC = TEN_SEC * 2,
            THIRTY_SEC = TEN_SEC * 3,
            FORTY_SEC = TEN_SEC * 4,
            FIFTY_SEC = TEN_SEC * 5
        }

        /**
         * Main Page의 Chart 표현을 위한 구조체 
         */
        public struct ST_BILLET_INFOMATION
        {
            public string nBillet_Order_Number;// 빌렛 주문번호
            public string nBillet_Heat_Id;// 빌렛 히트번호
            public int nBillet_Charge_Temperature;// 빌렛 장입온도
            public int nBillet_Discharge_Target_Temperature;// 빌렛 목표 배출 온도    
            public int nBillet_Predict_Discharge_Billet_Temperature;// 빌렛 예상 배출 온도
            public int nBillet_Predict_Current_Billet_Temperature;// 현재 빌렛 예상 온도
            public int nOrderOfBillet;// 가열로내 소재 순서 
            public int nZone_Average_Temperature;// 존 평균 온도 
        }
    }
}