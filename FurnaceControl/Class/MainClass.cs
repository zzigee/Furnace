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
        public DefineClass.ST_BILLET_INFOMATION[] stBILLET_INFOMATION;                      // 공업로 내의 빌렛 정보 
        public DefineClass.ST_FURNACE_REALTIME_INFORMATION stFURNACE_REALTIME_INFORMATION;  // 공업로 실시간 상태정보  



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
            this.stFURNACE_REALTIME_INFORMATION.nZone_Temp = new int[this.m_Define_Class.MAX_ZONE_IN_FURNACE];

            this.m_L1LinkClass = new L1LinkClass(this, (int)DefineClass.TIMER_INTERVAL.TWO_SEC);
            this.m_L3LinkClass = new L3LinkClass(this, (int)DefineClass.TIMER_INTERVAL.TWO_SEC);
            this.m_TrackingClass = new TrackingClass(this, (int)DefineClass.TIMER_INTERVAL.TWO_SEC); 
            //this.m_ThermalModelClass = new ThermalModelClass(this, (int)DefineClass.TIMER_INTERVAL.TWO_SEC);
            this.m_ThermalModelClass = new ThermalModelClass(this, this.m_Define_Class.nDangjinThermalCalPeriod);
        }
    }
}