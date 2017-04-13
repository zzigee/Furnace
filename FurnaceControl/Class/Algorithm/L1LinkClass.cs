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

            //InitializeOPC("OPCsoft.opcSvrTS.1", "");
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
            if (this.m_MainClass.m_Define_Class.isOpcCon)
            {
                getDataFromOPC();
            }
        }
        

        /******************************************************************************
         * 주기적으로 실행하는 함수 
         * 
         * OPC 의 데이터를 주기적으로 읽어들여 구조체에 저장하는 함수  
         ******************************************************************************/
        private void getDataFromOPC()
        {
            if (this.m_opcMgr == null) return;

            int iResFunc = 0;
            int nTagCnt = 12;
            object[] objReadVals = new object[nTagCnt];
            int[] nQualities = new int[nTagCnt];

            ///////////////////////////////////////////////////////////////////////
            // 공업로 정보 갱신 (stFURNACE_REALTIME_INFORMATION)
            iResFunc = m_opcMgr.opcReadGroupTags("OPC", nTagCnt, ref objReadVals, ref nQualities);
                   

            /**
             *  버너  버너   버너 

             */
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.strCurrentDate = DateTime.Now.ToString();


            if (iResFunc != 1)
            {
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, "OPC Error 발생");
                return;
            }

            for(int i=0 ; i<12 ; i++)
            {
                if (int.Parse(nQualities[i].ToString()) == 0 || int.Parse(nQualities[i].ToString()) == 192)
                    this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[i] = float.Parse(objReadVals[i].ToString());
                else
                    this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[i] = this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[i];
            }

                /*
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[0] = float.Parse(objReadVals[0].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[1] = float.Parse(objReadVals[1].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[2] = float.Parse(objReadVals[2].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[3] = float.Parse(objReadVals[3].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[4] = float.Parse(objReadVals[4].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[5] = float.Parse(objReadVals[5].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[6] = float.Parse(objReadVals[6].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[7] = float.Parse(objReadVals[7].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[8] = float.Parse(objReadVals[8].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[9] = float.Parse(objReadVals[9].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[10] = float.Parse(objReadVals[10].ToString());
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Temperature[11] = float.Parse(objReadVals[11].ToString());
                */

            float fZoneTempratureAvg = 0.0f;

            float fMaxTemp = float.Parse(objReadVals[0].ToString());
            float fMinTemp = float.Parse(objReadVals[0].ToString());
            int nMaxIndex = 0;
            int nMinIndex = 0;
            int nMaxIndex_ext = 0;
            int nMinIndex_ext = 0;

            for (int i = 0; i < 12;  i++)
            {
                if(fMaxTemp < float.Parse(objReadVals[i].ToString()))
                {
                    fMaxTemp = float.Parse(objReadVals[i].ToString());
                    nMaxIndex = i;
                }
            }

            for (int i = 0; i < 12; i++)
            {
                if (fMinTemp > float.Parse(objReadVals[i].ToString()))
                {
                    fMinTemp = float.Parse(objReadVals[i].ToString());
                    nMinIndex = i;
                }
            }

            float fMaxTemp_ext = 0.0f;
            float fMinTemp_ext = 0.0f;

            if(nMaxIndex == 0)  fMaxTemp_ext = float.Parse(objReadVals[1].ToString());
            else fMaxTemp_ext = float.Parse(objReadVals[0].ToString());

            if (nMinIndex == 0) fMinTemp_ext = float.Parse(objReadVals[1].ToString());
            else fMinTemp_ext = float.Parse(objReadVals[0].ToString());

            for (int i = 0; i < 12; i++)
            {
                if (nMaxIndex != i)
                {
                    if (fMaxTemp_ext < float.Parse(objReadVals[i].ToString()))
                    {
                        fMaxTemp_ext = float.Parse(objReadVals[i].ToString());
                        nMaxIndex_ext = i;
                    }
                }
            }

            for (int i = 0; i < 12; i++)
            {
                if (nMinIndex != i)
                {
                    if (fMinTemp_ext > float.Parse(objReadVals[i].ToString()))
                    {
                        fMinTemp_ext = float.Parse(objReadVals[i].ToString());
                        nMinIndex_ext = i;
                    }
                }
            }


            for (int i = 0; i < 12; i++)
            {
                if (nMinIndex != i && nMaxIndex != i && nMaxIndex_ext != i && nMinIndex_ext != i)
                {
                    fZoneTempratureAvg += float.Parse(objReadVals[i].ToString());
                }
            }

            fZoneTempratureAvg = fZoneTempratureAvg / 8.0f;

            // 평균온도 계산 후 저장 
            this.m_MainClass.stFURNACE_REALTIME_INFORMATION.fZone_Avg_Temperature[0] = fZoneTempratureAvg;


            this.m_MainClass.m_MainForm.Set_txtDanjin_TC_TEMP("");






            ///////////////////////////////////////////////////////////////////////
            // Pusher 가 동작 (당진 테스트는 Delta 시간으로 처리)



            ///////////////////////////////////////////////////////////////////////
            // 소재 장입 처리 



            ///////////////////////////////////////////////////////////////////////
            // 소재 배출 처리 


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
            //iResFunc = m_opcMgr.opcRegSvrEx("C:\\UsrAppConf.xml", "OPCsoft.opcSvrTS.1", "", "", 5);
            iResFunc = m_opcMgr.opcRegSvrEx("C:\\UsrAppConf.xml", strProdID, strServerAddress, "", 5);

            if (iResFunc == 1)
            {
                this.m_MainClass.m_Define_Class.isOpcCon = true;
                Console.WriteLine("OpcRegSvrEx() Success!");
            }
            else
            {
                this.m_MainClass.m_Define_Class.isOpcCon = false;
                Console.WriteLine("OpcRegSvrEx() Failed! Error Code:" + iResFunc);
            }
        }


        public void getReadGroupTags(string strGroup, int cnt)
        {
            int iResFunc = 0;
            int nTagCnt = cnt;
            object[] objReadVals = new object[nTagCnt];
            int[] nQualities = new int[nTagCnt];

            iResFunc= m_opcMgr.opcReadGroupTags(strGroup, nTagCnt, ref objReadVals, ref nQualities);

            //this.m_MainClass.m_MainForm.tbOPC_Group.Text = objReadVals[0].ToString();
            //this.m_MainClass.m_MainForm.txtOPCReadData_2.Text = objReadVals[1].ToString();

            if (iResFunc != 1) return;

            this.m_MainClass.m_MainForm.radGridView13.TableElement.BeginUpdate();

            this.m_MainClass.m_MainForm.radGridView13.Rows.Clear();

            for(int i=0 ; i < cnt ; i++)
            {
                this.m_MainClass.m_MainForm.radGridView13.Rows.AddNew();
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["No"].Value = i + 1;
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["TagName"].Value = objReadVals[i].ToString();
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["Data"].Value = nQualities[i].ToString();
            }

            this.m_MainClass.m_MainForm.radGridView13.TableElement.EndUpdate();

        }



        public void getOpcGetTagList(string strProdID, string strServerAddress)
        {
            int iResFunc = 0;
            int iIdx = 0;
            opcMgrClass opcMgr = new opcMgrClass();
            object oTagName = new object[1];
            object oTagDataType = new object[1];

            //iResFunc = opcMgr.opcGetTagList(ref oTagName, ref oTagDataType, "OPCsoft.opcSvrTS.1", "", "1");
            iResFunc = opcMgr.opcGetTagList(ref oTagName, ref oTagDataType, strProdID, strServerAddress, "1");

            //this.m_MainClass.m_MainForm.tbOPC_Group.Text = objReadVals[0].ToString();
            //this.m_MainClass.m_MainForm.txtOPCReadData_2.Text = objReadVals[1].ToString();

            this.m_MainClass.m_MainForm.radGridView13.TableElement.BeginUpdate();

            this.m_MainClass.m_MainForm.radGridView13.Rows.Clear();

            Array o = (Array)oTagName;
            Array oDT = (Array)oTagDataType;


            for (int i = 0; i < oDT.Length; i++)
            {
                this.m_MainClass.m_MainForm.radGridView13.Rows.AddNew();
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["No"].Value = i + 1;
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["TagName"].Value = o.GetValue(i).ToString();
                this.m_MainClass.m_MainForm.radGridView13.Rows[i].Cells["Data"].Value = oDT.GetValue(i).ToString();
            }

            this.m_MainClass.m_MainForm.radGridView13.TableElement.EndUpdate();

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