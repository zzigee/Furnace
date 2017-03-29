using System;
using System.Linq;

using opcNet.IF.SEM;
using System.Windows.Forms;

// 잘되나.

namespace FurnaceControl
{
    internal class L1LinkClass : TimerClass
    {
        private readonly MainClass m_MainClass;
        private opcMgrClass m_opcMgr = null;

        public L1LinkClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "L1LinkClassTimer");

            InitializeOPC("OPCsoft.opcSvrTS.1", "");
        }

        /******************************************************************************
         * 주기적으로 실행하는 함수 
         * 
         * L1 의 데이터를 주기적으로 불러와서 내부 구조체에 저장하는 함수 
         ******************************************************************************/
        public override void Run()
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, "L1LinkClassTimer");

            getOPCStatus();
        }


        private void getOPCStatus()
        {
            int code = 0;
            if(this.m_opcMgr != null)   code = this.m_opcMgr.opcGetSvrStatus();
            

            if(code == 1)       // 정상 상태 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.LOG, this, "OPC 정상 동작 중");
            }
            else if(code == 2)  // 비정상 종료 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC 비정상 종료");
            }
            else if(code == 3) // OPC Server 가 서비스 초기화 중 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Server 서비스 초기화 중");
            }
            else if (code == 4) // OPC Server Busy 상태 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Server Busy 상태");
            }
            else if (code == 5) // 테스트모드 동장 중 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC 테스트 모드 동작 중");
            }
            else if (code == 6 || code == 14)    // Disconnect 상태 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Server Disconnect 상태");
            }
            else if (code <= -11 && code >= -50)    // Errors about functio noperation  
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Errors about function operation");
            }
            else if (code == -91)    // Ping 에러
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Ping 에러");
            }
            else if (code == -92)    // 원격서버와의 연동 에러 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC 서버와 연동 에러");
            }
            else if (code <= -93)    // Exception Error 
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Exception Error");
            }
        }


        /**
         * Initialize OPC 
         */
        public void InitializeOPC(string strProdID, string strServerAddress)
        {
            int iResFunc = 0;
            m_opcMgr = new opcMgrClass();
            m_opcMgr.opcSetLogDirectory(Application.StartupPath + "\\NetIFLog");
            iResFunc = m_opcMgr.opcRegSvrEx("C:\\UsrAppConf.xml", "OPCsoft.opcSvrTS.1", "", "", 5);

            if (iResFunc == 1)
            {
                Console.WriteLine("OpcRegSvrEx() Success!");
            }
            else
            {
                Console.WriteLine("OpcRegSvrEx() Failed! Error Code:" + iResFunc);
            }
        }


        public void getReadGroupTags(string strGroup)
        {
            int iResFunc = 0;
            int nTagCnt = 3;
            object[] objReadVals = new object[nTagCnt];
            int[] nQualities = new int[nTagCnt];

            iResFunc= m_opcMgr.opcReadGroupTags(strGroup, nTagCnt, ref objReadVals, ref nQualities);

            this.m_MainClass.m_MainForm.txtOPCReadData_1.Text = objReadVals[0].ToString();
            this.m_MainClass.m_MainForm.txtOPCReadData_2.Text = objReadVals[1].ToString();
        }


        public void asdf()
        {
            if (m_opcMgr == null)
            {
                Console.WriteLine("Is NOT connected to OPC Server. Please, click opcRegSvr button first!");
                return;
            }

            int iResFunc = 0;
            int lTagCount = 3;
            string sGroupName = "IncrementGroup";

            object[] oReadVals = new object[lTagCount];
            int[] lQualities = new int[lTagCount];
            iResFunc = m_opcMgr.opcReadGroupTags(sGroupName, lTagCount, ref oReadVals, ref lQualities);
            if (iResFunc == 1)
            {  
                string sMsg = "";
                for (int i = 0; i < lTagCount; i++)
                {
                    if (i == 0) sMsg = oReadVals[i].ToString();
                    else sMsg = sMsg + ", " + oReadVals[i];
                    if (i > 5) break;
                }

                Console.WriteLine("opcReadGroupTags() Success! Read Value: " + sMsg);
            }
            else
            {
                Console.WriteLine("opcReadGroupTags() Failed! Error Code:" + iResFunc);
            }
        }


    }
}