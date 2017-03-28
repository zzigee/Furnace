using System;
using System.Linq;

namespace FurnaceControl
{
    internal class DefineClass
    {
        
        public DefineClass(MainClass mc)
        {
            this.m_MainClass = mc;
        }

        /*
         * [Check - 2016.02.22] -> [Result - ] 
         * 차후 프로그램 배포시 자유롭게 수정이 가능하도록 공업로 최대 영역 수를 외부 파일 참조로 변경 필요  
         **/
        public readonly int MAX_ZONE_IN_FURNACE = 10;   // 가열 존의 갯수 
        public readonly int MAX_BILLET_IN_FURNACE = 50; // 로내 최대 빌렛 갯수 


        // Parent Object Class 객체 생성 
        public MainClass m_MainClass;
        
        public int nBillet_Count_On_FUrnace;






        /* 
         * 당진 테스트시 연속시 가열로를 모사하기 위해 시간을 소재의 위치로 치환하는데, 
         * 총 시간 / 계산시간 의 수 이상의 배열을 확보 
         */
        public readonly int MAX_BILLET_IN_FURNACE_FOR_DANJIN = 1000;

        public bool isDataLogging = false;                 // 실시간 데이터 DB 저장 여부 
        public int nDataLoggingIndex;               // 실시간 데이터 배열 인덱스 
        public DateTime dateDataLoggingStartTime;   // 실시간 데이터 저장 시작 시간  

        public int nDangjinThermalCalPeriod = (int)DefineClass.TIMER_INTERVAL.TWO_SEC;
        /*
         * 당진 테스트용 구조체 
         */
        public struct ST_DANJIN_STURCT
        {
            public string strStartTime;    // 데이터 로깅 시작시간
            public string strEndTime;      // 데이터 로깅 종료시간
            public string[] strCreateTime;// 측정 데이터 생성 시간 
            public string[] strZoneTemp;   // 존 온도
            public string[] strBilletPredictTemp;  // 빌렛 예측온도 
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

        /*
         * 실시간으로 변화되는 공업로의 상태정보 
         */
        public struct ST_FURNACE_REALTIME_INFORMATION
        {
            public double dZone_Temp;   // 현 시점에서의 존 분위기 온도
        }


        /**
         * 열모델에게 사용되는 소재정보 (배열로 저장됨)  
         */
        public struct ST_BILLET_INFORMATION_FOR_DANJIN
        {
            public double dZone_Temp;               // 계산 시점에서의 존 분위기 온도
            public double dPredict_Billet_Temp;     // 계산 시점에서의 빌렛 예측 온도 
        }


    }
}