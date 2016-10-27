using System;
using System.Linq;
using System.Threading;

namespace FurnaceControl
{
    internal class TimerClass
    {
        // for Log Time Stamp  
        public String strTimerName;
        public long nPreMsec;

        private Thread th;
        private int nDelayTime;
        private bool isRunThread = false;

        public bool Start(int delayTime, String timerName)
        {
            this.nDelayTime = delayTime;
            this.strTimerName = timerName;

            if (this.th == null)
            {
                try 
                { 
                    this.th = new Thread(new ThreadStart(this.ThreadProc));
                    this.th.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("[Timer 오류]{0}", ex.Message));
                }
            }
            else if (this.th.IsAlive == false)
            {
                try 
                { 
                    this.th = new Thread(new ThreadStart(this.ThreadProc));    
                    this.th.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("[Timer 오류]{0}", ex.Message));
                }
            }

            this.isRunThread = true;

            return true;
        }

        public void Stop()
        {
            if (this.th != null)
            {
                this.th.Join();
                this.th = null;
            }

            this.isRunThread = false;
        }

        public virtual void Run()
        {
            /*
            long  nPreMsec;
            TimeSpan timespan = new TimeSpan((DateTime.Now.Ticks - nPreMsec));
            Console.WriteLine("[" + timespan.Milliseconds.ToString() + "]" + strTimerName);
            nPreMsec = DateTime.Now.Ticks;
            */
        }

        public void WriteThreadTimeToConsole()
        {
            var timespan = new TimeSpan((DateTime.Now.Ticks - this.nPreMsec));

            //Console.WriteLine(string.Format("[{0}] [{1}{2}", UtilsClass.getCurrentTime(), this.strTimerName, string.Format("] 타이머 주기 {0} s", timespan.Seconds.ToString())));
            this.nPreMsec = DateTime.Now.Ticks;
        }

        private void ThreadProc()
        {
            while (this.isRunThread == true)
            {
                this.Run();
                System.Threading.Thread.Sleep(this.nDelayTime);

                // 실행 시간 로그 
                this.WriteThreadTimeToConsole();
            }
            this.th = null;
        }
    }
}