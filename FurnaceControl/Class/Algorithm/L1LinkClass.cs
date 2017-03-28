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

            InitializeOPC();
        }

        /******************************************************************************
         * 주기적으로 실행하는 함수 
         * 
         * L1 의 데이터를 주기적으로 불러와서 내부 구조체에 저장하는 함수 
         ******************************************************************************/
        public override void Run()
        {
            this.m_MainClass.m_SysLogClass.SystemLog(this, "L1LinkClassTimer");
        }


        /**
         * Initialize OPC 
         */
        private void InitializeOPC()
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

        public void getReadGroupTags()
        {
            int nTagCnt = 0;
            object[] objReadVals = new object[10];
            int[] nQualities = new int[10];

            m_opcMgr.opcReadGroupTags("IncrementGroup", nTagCnt, objReadVals, nQualities);
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