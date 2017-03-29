using System;
using System.Linq;
using System.Data.SqlClient;

namespace FurnaceControl
{
    internal class SysLogClass
    {   
        public String strClassName;

        private readonly MainClass m_MainClass;

        public SysLogClass(MainClass mc)
        {
            this.m_MainClass = mc;
            strClassName = "SysLogClass";
        }

        public void GeneralLog(Object obj, String msg)
        {
            Console.WriteLine(string.Format("[{0}] GeneralLog Message[{1}]: [{2}]", UtilsClass.getCurrentTime(), obj.ToString(), msg));
        }

        public void TryCatchLog(Object obj, string msg)
        {
            Console.WriteLine(string.Format("[{0}] TryCatchLog Message[{1}]: [ {2} ]", UtilsClass.getCurrentTime(), obj.ToString(), msg));
        }

        public void SystemLog(int code, Object obj, string msg)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection();
                sqlCon.ConnectionString = this.m_MainClass.m_SQLClass.sqlConnectionString;
                sqlCon.Open();

                String query = string.Format("INSERT INTO [dbo].[SYSTEM_EVENT_LOG] ([TIMESTAMP]" +
                    ",[CODENO],[LOCATION],[MESSAGE],[VALUE_1],[VALUE_2]) VALUES (SYSDATETIME()" +
                    ",{0},'{1}','{2}','','')", code, obj.ToString(), msg);

                this.m_MainClass.m_SQLClass.executeNonQuerySQL(query, sqlCon);
                Console.WriteLine(string.Format("[{0}] SystemLog Message[{1}]: [ {2} ]", UtilsClass.getCurrentTime(), obj.ToString(), msg));

                sqlCon.Close();
            }
            catch (Exception e)
            {
                this.TryCatchLog(this, e.Message);
            }
        }

        public void DebugLog(Object obj, string msg)
        {
            Console.WriteLine(string.Format("[{0}] DebugLog Message[{1}]: [ {2} ]", UtilsClass.getCurrentTime(), obj.ToString(), msg));
        }
    }
}