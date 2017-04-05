using System;
using System.Linq;

namespace FurnaceControl
{
    internal class DefineClass
    {

        // Parent Object Class 객체 생성 
        public MainClass m_MainClass;
        
        public DefineClass(MainClass mc)
        {
            this.m_MainClass = mc;
        }


        /*
         * 당진 테슽용 
         */
        public bool isDataLogging = false;                  // 실시간 데이터 DB 저장 여부 
        public int nDataLoggingIndex;                       // 실시간 데이터 배열 인덱스 
        public DateTime dateDataLoggingStartTime;           // 실시간 데이터 저장 시작 시간  
        public int nDangjinThermalCalPeriod = (int)DefineClass.TIMER_INTERVAL.ONE_SEC;


        /*
         * [Check - 2016.02.22] -> [Result - ] 
         * 차후 프로그램 배포시 자유롭게 수정이 가능하도록 공업로 최대 영역 수를 외부 파일 참조로 변경 필요  
         **/
        public readonly int MAX_ZONE_IN_FURNACE = 10;       // 가열 존의 갯수 
        public readonly int MAX_BILLET_IN_FURNACE = 100000;   // 로내 최대 빌렛 갯수 

        public int nBillet_Count_On_FUrnace;



        /**
         * 공업로 안에서 실시간 소재 정보 (공업로 내의 최대 소재 갯수 만큼 배열생성)
         */
        public struct ST_BILLET_INFOMATION
        {
            public string nBillet_Order_Number;                         // 빌렛 주문번호
            public string nBillet_Heat_Id;                              // 빌렛 히트번호
            public int nBillet_Charge_Temperature;                      // 빌렛 장입온도
            public int nBillet_Discharge_Target_Temperature;            // 빌렛 목표 배출 온도    
            public int nBillet_Predict_Discharge_Billet_Temperature;    // 빌렛 예상 배출 온도
            public int nBillet_Predict_Current_Billet_Temperature;      // 현재 빌렛 예상 온도
            public int nOrderOfBillet;                                  // 가열로내 소재 순서 (소재갯수에 의존)
            public int nBilletPosition;                                 // 가여로내 소재 위치 (cm) (가열로내 소재 라인에 의존)
            public bool bIsSplitBillet;                                 // 한라인에 두 개의 소재 유무  
            public int nZone_Average_Temperature;                       // 존 평균 온도 
        }

        /*
         * 공업로 실시간 상태정보 (공업로 갯수 만큼 생성 : 하나만 존재) 
         */
        public struct ST_FURNACE_REALTIME_INFORMATION
        {
            public string strCurrentDate;           // 작성시간
            public int[] nZone_Temperature;         // 존의 온도
            public int[] nZone_Start_Position;      // 존의 위치(시작)
            public int[] nZone_End_Position;        // 존의 위치(종료)
        }

        public enum LOG_CODE
        {
            LOG,
            ERROR
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
    }
}