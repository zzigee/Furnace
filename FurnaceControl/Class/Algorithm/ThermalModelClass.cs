using System;
using System.Linq;

namespace FurnaceControl
{
    internal class ThermalModelClass : TimerClass
    {
        private readonly MainClass m_MainClass;

        int Nb = 48;
        int Nfm = 8;
        double con1 = 0.00000015;
        double con2 = 100;
        double con3 = 0.0001;
        double Tb_init;
        double[] Tb = new double[100];
        double[] Tf = new double[100];
        double[] Tfm = new double[100];

        
        public ThermalModelClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "ThermalModelClassTimer");
        }

        /**
         * 주기적으로 실행하는 함수 
         **/
        public override void Run()
        {
            this.m_MainClass.m_SysLogClass.SystemLog(this, "ThermalModelClassTimer");
            CalTemp();

        }

        public void CalTemp()
        {
            for (int k = 0; k < 100; k++)
            {
//                Tb[k] = Tf[k] = Tfm[k] = 0;
                Tb[k] = Tf[k] = 0; //테스트용
            }
            Tfm[0] = 100;
            Tfm[1] = 500;
            Tfm[2] = 800;
            Tfm[3] = 1200;
            Tfm[4] = 1300;
            Tfm[5] = 1350;
            Tfm[6] = 1450;
            Tfm[7] = 1400;

            for (int k=0; k < Nfm; k++)
            {
//               Tfm[k] = ThemocoupleTemp[]; //level2로 부터 Tfm[]에 들어갈 배열 값을 전달 받아야함.
            }

            int q = 0;
            for (int i = 0; i < Nfm; i++)
            {
                if (i < 3)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            Tf[q] = j * (Tfm[1] - Tfm[0]) / 6 + Tfm[0];
                            q += 1;
                        }
                    }
                    if (i == 1)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            Tf[q] = j * (Tfm[2] - Tfm[1]) / 6 + Tfm[1];
                            q += 1;
                        }
                    }
                    if (i == 2)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            Tf[q] = j * (Tfm[3] - Tfm[2]) / 6 + Tfm[2];
                            q += 1;
                        }
                    }
                }
                else
                {
                    for(int j=0; j<6; j++){
                        Tf[q] = Tfm[i];
                        q += 1;
                    }
                }
            }
            Tb_init = 0.5 * Tfm[0];
            Tb[0] = Tb_init;

            for (int k = 0; k < Nb; k++)
            {
                Tb[k + 1] = Tb[k] + con1 * (con2 * (Tf[k] - Tb[k]) + con3 * (Math.Pow(Tf[k], 4) - Math.Pow(Tb[k], 4)));
            }
            //Tb 배열의 값을 level2로 내보내야 함

//                Console.WriteLine(">"+Tb[5]);
        }
    }
}