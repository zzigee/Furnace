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

        double getLagrange(double[] x, double[] y, int n, double t)
        {
            int i, j;
            double s, p;

            s = 0.0;

            for (i = 0; i < n; i++)
            {
                p = y[i];
                for (j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        p = p * (t - x[j]) / (x[i] - x[j]);
                    }
                }
                s = s + p;
            }
            return s;
        }
        
        /*********************************************************************************************
         *********************************************************************************************
         * 열모델 계산 코드 
         *********************************************************************************************
         *********************************************************************************************/
        public void calThermalModel()
        {
            this.m_MainClass.m_MainForm.Set_txtDanjin_Current_Date(DateTime.Now.ToString());
            TimeSpan result = DateTime.Now - this.m_MainClass.m_Define_Class.dateDataLoggingStartTime;
            this.m_MainClass.m_MainForm.Set_txtDanjin_Operation_Time("[" + this.m_MainClass.m_Define_Class.nDataLoggingIndex + "]" + result.ToString(@"h\:mm\:ss"));

            if (this.m_MainClass.m_Define_Class.nDataLoggingIndex > this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE)
            {
                this.m_MainClass.m_MainForm.ShowMessageBox("당진 테스트베드 측정 데이터 갯수가 최대 갯수를 초과하였습니다. \n\r측정을 중지합니다.");
                this.m_MainClass.m_MainForm.btnDataLogging.BackColor = System.Drawing.Color.LightGray;
                this.m_MainClass.m_Define_Class.isDataLogging = false;
            }



            Random rnd = new Random();

            int nPreditBilletTemp; //예측 소재 온도
            float nZoneTemprature = this.m_MainClass.stFURNACE_REALTIME_INFORMATION.nZone_Temperature[0];     // 현재 TC 온도 

            int num_cp = 26;
            int num_h = 26;
            int num_f = 26;
            int num_fn = 91;
            int num_wp = 91;
            float dt;
            float dens, thick;
            float sigma, eps;
            float temp_wp_init;
            float cp_s, h_s, f_s;

            //비열 등 계수 상수 값 대입, 이후 DB에서 읽어올 내용 //테스트 용
            float[,] cp = new float[,] { { 16, 0.4594f }, { 38, 0.46819f }, { 93, 0.48953f }, { 149, 0.51212f }, { 204, 0.53555f }, { 260, 0.55354f }, { 316, 0.56693f }, { 371, 0.59329f }, { 427, 0.6276f }, { 482, 0.66735f }, { 538, 0.71086f }, { 593, 0.75856f }, { 649, 0.81965f }, { 704, 0.97487f }, { 732, 1.17152f }, { 760, 0.99788f }, { 816, 0.88533f }, { 871, 0.79705f }, { 927, 0.76065f }, { 982, 0.705f }, { 1038, 0.66693f }, { 1093, 0.64894f }, { 1149, 0.6481f }, { 1204, 0.65061f}, { 1260, 0.66442f }, { 1316, 0.71295f} };
            float[,] h = new float[,] { { 16, 0.04f }, { 38, 0.04f }, { 93, 0.04f }, { 149, 0.04f }, { 204, 0.04f }, { 260, 0.04f }, { 316, 0.04f }, { 371, 0.04f }, { 427, 0.04f }, { 482, 0.04f }, { 538, 0.04f }, { 593, 0.04f }, { 649, 0.04f }, { 704, 0.04f }, { 732, 0.04f }, { 760, 0.04f }, { 816, 0.04f }, { 871, 0.04f }, { 927, 0.04f }, { 982, 0.04f }, { 1038, 0.04f }, { 1093, 0.04f }, { 1149, 0.04f }, { 1204, 0.04f }, { 1260, 0.04f }, { 1316, 0.04f } };
            float[,] f = new float[,] { { 16, 0.5f }, { 38, 0.5f }, { 93, 0.5f }, { 149, 0.5f }, { 204, 0.5f }, { 260, 0.5f }, { 316, 0.5f }, { 371, 0.5f }, { 427, 0.5f }, { 482, 0.5f }, { 538, 0.5f }, { 593, 0.5f }, { 649, 0.5f }, { 704, 0.5f }, { 732, 0.5f }, { 760, 0.5f }, { 816, 0.5f }, { 871, 0.5f }, { 927, 0.5f }, { 982, 0.5f }, { 1038, 0.5f }, { 1093, 0.5f }, { 1149, 0.5f }, { 1204, 0.5f }, { 1260, 0.5f }, { 1316, 0.5f } };
            //TC input data
            float[,] fn = new float[,] {{0, 200},{10, 210},{20, 220},{30, 230},{40, 240},{50, 250},{60, 260},{70, 270},{80, 280},{90, 290},{100, 300},{110, 315},{120, 330},{130, 345},{140, 360},{150, 375},{160, 390},{170, 405},{180, 420},{190, 435},{200, 450},{210, 475},{220, 500},{230, 525},{240, 550},{250, 575},{260, 600},{270, 625},{280, 650},{290, 675},{300, 700},{310, 735},{320, 770},{330, 805},{340, 840},{350, 875},{360, 910},{370, 945},{380, 980},{390, 1015},{400, 1050},{410, 1050},{420, 1050},{430, 1050},{440, 1050},{450, 1050},{460, 1050},{470, 1050},{480, 1050},{490, 1050},{500, 1150},{510, 1150},{520, 1150},{530, 1150},{540, 1150},{550, 1150},{560, 1150},{570, 1150},{580, 1150},{590, 1150},{600, 1200},{610, 1200},{620, 1200},{630, 1200},{640, 1200},{650, 1200},{660, 1200},{670, 1200},{680, 1200},{690, 1200},{700, 1300},{710, 1300},{720, 1300},{730, 1300},{740, 1300},{750, 1300},{760, 1300},{770, 1300},{780, 1300},{790, 1300},{800, 1250},{810, 1250},{820, 1250},{830,	1250},{840,	1250},{850,	1250},{860,	1250},{870,	1250},{880,	1250},{890,	1250},{900,	1250}};

            //상수 값 대입, 이후 DB에서 읽어올 내용
            dt = 1.0f;
            dens = 7851.354f;
            thick = 0.5f;
            sigma = 0.00000008f;
            eps = 0.2f;
            temp_wp_init = 50;

            cp_s=cp[0,0];
            h_s=h[0,0];
            f_s=f[0,0];

            //for(int k=0; k<num_cp-1; k++){
                //for(int j=0; j<num_cp-1; j++){
                //    if((nPreditBilletTemp[k]>=cp[j,0])&&(nPreditBilletTemp[k]<=cp[j+1, 0])){
                //        cp_s=cp[j,1]+(cp[j+1,1]-cp[j,1])/(cp[j+1, 0]-cp[j,0])*(nPreditBilletTemp[k]-cp[j,0]);
                //    }
                //}
                //for(int i=0; i<num_h-1; i++){
                //    if((nPreditBilletTemp[k]>=h[i,0])&&(nPreditBilletTemp[k]<=h[i+1,0])){
                //        h_s=h[i,1]+(h[i+1,1]-h[i,1])/(h[i+1,0]-h[i,0])*(nPreditBilletTemp[k]-h[i,0]);
                //    }
                //}
                //for(int j=0; j<num_f-1; j++){
                //    if((nPreditBilletTemp[k]>=f[j,0])&&(nPreditBilletTemp[k]<=f[j+1,0])){
                //        f_s=f[j,1]+(f[j+1,1]-f[j,1])/(f[j+1,0]-f[j,0])*(nPreditBilletTemp[k]-f[j,0]);
                //    }
                //}

                //nPreditBilletTemp[k+1] = nPreditBilletTemp[k]+(h_s*dt)*(fn[k,1]-nPreditBilletTemp[k])/(dens*cp_s*thick)+(sigma*eps*f_s*dt)*(Math.Pow(fn[k,1],4)-Math.Pow(nPreditBilletTemp[k],4))/(dens*cp_s*thick);

                this.m_MainClass.stBILLET_INFOMATION[this.m_MainClass.m_Define_Class.nDataLoggingIndex + 1].nBillet_Predict_Current_Billet_Temperature = 1;
            //}
            /** 
             * 이 값 구해주세요. 
             */


            //nPreditBilletTemp = rnd.Next(1600);     



            /**
             */




            //this.m_MainClass.m_MainForm.dANGJIN_DATATableAdapter.InsertQuery(    
            //    this.m_MainClass.m_Define_Class.nDataLoggingIndex,
            //    DateTime.Now.ToString(),
            //    nZoneTemprature.ToString(),
            //    nPreditBilletTemp.ToString());

            //this.m_MainClass.stBILLET_INFOMATION[this.m_MainClass.m_Define_Class.nDataLoggingIndex].nBillet_Predict_Current_Billet_Temperature = nPreditBilletTemp;
            //this.m_MainClass.stBILLET_INFOMATION[this.m_MainClass.m_Define_Class.nDataLoggingIndex].nZone_Average_Temperature = nZoneTemprature;
        }



        public override void Run()
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, "ThermalModelClassTimer");

            /*
            Start Data Logging 이 활성화 된 경우 실행 
            실행 주기는 열모델 연산 Delta 시간과 동일하게 DB_Timer 에 설정하면 됨. 
            */
            if (this.m_MainClass.m_Define_Class.isDataLogging)
            {
                calThermalModel();
                this.m_MainClass.m_Define_Class.nDataLoggingIndex++;    // 열모델 배열 증가
            }
        }
    }
}