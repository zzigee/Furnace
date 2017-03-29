using System;
using System.Linq;

namespace FurnaceControl
{
    internal class MainClass
    {


        /*****************************************************************************
         * Define Object  
         ******************************************************************************/
        public MainForm m_MainForm;

        public SQLClass m_SQLClass;         // SQL 관리 
        public SysLogClass m_SysLogClass;   // 로그관리
        public UtilsClass m_UtilsClass;     // 범용함수

        public DefineClass m_Define_Class;  // 사전정의

        public TrackingClass m_TrackingClass;           // 소재관리 
        public ThermalModelClass m_ThermalModelClass;   // 온도예측
        public L1LinkClass m_L1LinkClass;               // L1 연결 (OPC)
        public L3LinkClass m_L3LinkClass;               // L3 연결 (TCP or Other) 




        /*****************************************************************************
         * Define Variables 
         ******************************************************************************/
        public bool isLoginUser;            // 관리자 접속 확인 DefineClass




        /*****************************************************************************
         * Define Struct 
         ******************************************************************************/
        public DefineClass.ST_BILLET_INFOMATION[] stBILLET_INFOMATION;      // Chat 에 정보를 보여주기위한 데이터를 관리하는 구조체 

        public DefineClass.ST_FURNACE_REALTIME_INFORMATION stFURNACE_REALTIME_INFORMATION;  // 실시간으로 변화되는 공업로의 상태정보  
        public DefineClass.ST_BILLET_INFORMATION_FOR_DANJIN[] stTHERAMLMODEL_FOR_DANJIN;    // 열모델에게 사용되는 소재정보 (배열)  


        public DefineClass.ST_DANJIN_STURCT stDANJIN_STRUCT;        // 당진 테스트 용 






        public MainClass(MainForm mf)
        {

            /*****************************************************************************
             * Initialize Object  
             ******************************************************************************/
            // Current MainForm Ojbect (for Handling GUI)
            this.m_MainForm = mf;

            this.m_SQLClass = new SQLClass(this);
            this.m_SysLogClass = new SysLogClass(this);
            this.m_UtilsClass = new UtilsClass(this);
            
            this.m_Define_Class = new DefineClass(this);
            // SQL 연결 수행 (접속이 실패할 경우 에러 코드를 포함한 Message 가 팝업됨)
            
            this.m_SQLClass.SqlConnect();



            /*****************************************************************************
             * Initialize Variable
             ******************************************************************************/
            this.isLoginUser = false;
            


            /*****************************************************************************
             * Initialize Struct 
             ******************************************************************************/
            this.stBILLET_INFOMATION = new DefineClass.ST_BILLET_INFOMATION[this.m_Define_Class.MAX_BILLET_IN_FURNACE];

            this.stFURNACE_REALTIME_INFORMATION = new DefineClass.ST_FURNACE_REALTIME_INFORMATION();
            this.stTHERAMLMODEL_FOR_DANJIN = new DefineClass.ST_BILLET_INFORMATION_FOR_DANJIN[this.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];

            this.stDANJIN_STRUCT = new DefineClass.ST_DANJIN_STURCT();      // 당진 테스트 용 
            this.stDANJIN_STRUCT.nDataCount = 0;
            this.stDANJIN_STRUCT.strCreateTime = new string[this.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];
            this.stDANJIN_STRUCT.strZoneTemp = new int[this.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];
            this.stDANJIN_STRUCT.strBilletPredictTemp = new int[this.m_Define_Class.MAX_BILLET_IN_FURNACE_FOR_DANJIN];


            this.m_TrackingClass = new TrackingClass(this, (int)DefineClass.TIMER_INTERVAL.ONE_SEC);
            this.m_ThermalModelClass = new ThermalModelClass(this, (int)DefineClass.TIMER_INTERVAL.TEN_SEC);

            this.m_L1LinkClass = new L1LinkClass(this, (int)DefineClass.TIMER_INTERVAL.TEN_SEC);
            this.m_L3LinkClass = new L3LinkClass(this, (int)DefineClass.TIMER_INTERVAL.TEN_SEC);
        }
    }
}