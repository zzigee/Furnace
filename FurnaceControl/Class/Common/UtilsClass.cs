using System;
using System.Linq;

namespace FurnaceControl
{
    internal class UtilsClass
    {
        private readonly MainClass m_MainClass;

        public UtilsClass(MainClass mc)
        {
            this.m_MainClass = mc;
        }

        static public String getCurrentTime()
        {
            return System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        }

        static public String getCurrentTimeMS()
        {
            return System.DateTime.Now.ToString("fff");
        }

        public void printMsg()
        {
            Console.WriteLine("Class of Utils");
        }
        /*
        var timespan = new TimeSpan((DateTime.Now.Ticks - this.nPreMsec));
        Console.WriteLine(string.Format("[{0}]{1}", timespan.Seconds.ToString(), this.strTimerName));
        this.nPreMsec = DateTime.Now.Ticks;
        */
    }
}