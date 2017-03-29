using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FurnaceControl
{
    internal class SQLClass
    {
        private readonly MainClass m_MainClass;

        /*
         * [Check - 2016.02.22] -> [Result - ] 
         * 차후 연결 자유롭게 수정이 가능하도록 연결 문자열을 외부 파일 참조로 변경 
         **/
        //public readonly String sqlConnectionString = "Server=192.168.1.142;database=furnacecontrol;Integrated Security=false;User ID=sa;Password=1234";
        public readonly String sqlConnectionString = "Server=165.133.82.254;database=furnacecontrol;Integrated Security=false;User ID=sa;Password=1234";

        public SqlConnection SqlCon;
        private SqlCommand sqlCommand;

        private String sql;

        public SQLClass(MainClass mc)
        {
            this.m_MainClass = mc;
        }

        /**
         * SQL Server 연결 관리 
         */
        public void SqlConnect()
        {
            this.SqlCon = new SqlConnection();
            this.SqlCon.ConnectionString = this.sqlConnectionString;

            this.SqlCon.StateChange += new StateChangeEventHandler(this.Con_StateChange);   // Connection 상태변경 이벤트 등록
            this.SqlCon.InfoMessage += new SqlInfoMessageEventHandler(this.Con_InfoMessage);// 

            try
            {
                this.SqlCon.Open();
                this.SqlCon.Close();

            }
            catch (Exception ex)
            {
                this.m_MainClass.m_MainForm.ShowMessageBox(string.Format("SQL 연결 에러 발생 관리자에게 문의 바랍니다.  \r\n\r\n에러코드('{0}')", ex.Message));
                this.m_MainClass.m_SysLogClass.SystemLog((int)DefineClass.LOG_CODE.ERROR, this, ex.Message);
            }
        }

        public bool SqlConnectOpen()
        {
            if(this.SqlCon.State == ConnectionState.Closed)
            {
                this.SqlCon.Open();
                return true;
            }
            else
            {
                return false;
            }            
        }

        public bool SqlConnectClose()
        {
            if (this.SqlCon.State == ConnectionState.Open)
            {
                this.SqlCon.Close();
                return true;
            }
            else
            {
                return false;
            }
        }


        public int executeNonQuerySQL(String command, SqlConnection con)
        {
            try
            {
                this.sqlCommand = new SqlCommand(command, con);
                
                return this.sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.Message);
            }

            return 0;
        }

        public SqlDataReader executeReaderQuerySQL(String command, SqlConnection con)
        {
            try
            {
                this.sqlCommand = new SqlCommand(command, con);

                //SqlDataReader read = this.sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                SqlDataReader read = this.sqlCommand.ExecuteReader();
                
                //this.m_MainClass.m_SysLogClass.SystemLog(this, command);

                return read;
            }
            catch (Exception ex)
            {
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.Message);
            }

            return null;
        }
        
        public int getUserInfo(string id, string pw)
        {
            int retVal = 0;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = this.sqlConnectionString;

            try
            {

                con.Open();


                SqlDataReader reader = this.executeReaderQuerySQL(string.Format("select * from [user] where id='{0}' and pw='{1}'", id, pw), con);
                retVal = reader.HasRows == false ? 0 : 1;
                reader.Close();


                con.Close();
            }
            catch(Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                this.m_MainClass.m_SysLogClass.TryCatchLog(this, ex.ToString());
            }

            return retVal; 
            //this.sqlCommand = new SqlCommand(("select count(*) from [user] where id='" + id + "' and pw='" + pw + "'"), this.SqlCon);
            //return (int)this.sqlCommand.ExecuteScalar();
        }
    
        /*
         * 신규 빌렛 정보가 있는 경우 BHEADER 데이터 등록 (소재별 단 한개의 데이터 생성, ERP 로 부터 받은 데이터로 생성)
         */
        public void Billet_Register()
        {
            //sql = "insert into bheader (Level2Id, Level1Id, HeatId, HeatIndex, OrderNum, Grade,ChargeTime,Weight,Length,ChargeTemp,AimTemp,DeltaT,MeasuredWeight,MeasuredLength)";
            //sql = String.Format("insert into bheader (Level2Id, Level1Id, HeatId, HeatIndex, OrderNum, Grade,ChargeTime,Weight,Length,ChargeTemp,AimTemp,DeltaT,MeasuredWeight,MeasuredLength) VALUES ({0}, {1}, {2}, {3}, {4}, {5},{6},{7},{8},{9},{10},{11},{12},{13})",                     1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
        }

        /*
         * 소재가 반입되는 시점에 BHEADER 정보 갱신 (반입 온도, 반입 시간 등)
         */
        public void Billet_Charge()
        {
        }

        /*
         * 소재가 반출 되는 시점에 BHEADER 정보 갱신 (반출 온도, 반출 시간 등)
         */
        public void Billet_Discharge()
        {
        }

        /**
         * Main > Chart 표현을 위한 데이터 갱신
         */
        public int updateBilletStruct()
        {
            int nBillet_Count_On_FUrnace = 0;

            SqlConnection sqlCon = new SqlConnection();
            sqlCon.ConnectionString = this.sqlConnectionString;
            sqlCon.Open();

            for (int i = 0; i < this.m_MainClass.m_Define_Class.MAX_BILLET_IN_FURNACE; i++)
            {
                this.sql = string.Format("select A.ORDER_NUMBER, A.HEAT_ID, A.CHARGE_TEMPERATURE, A.DISCHARGE_TEMPERATURE, b.PREDICT_DISCHARGE_BILLET_TEMPERATURE, b.PREDICT_CURRENT_BILLET_TEMPERATURE, b.ORDER_OF_BILLET, b.ZONE_AVERAGE_TEMPERATURE from BILLET as A inner join BILLET_STATUS as b on CONVERT(CHAR(19), A.CREATE_TIME, 20) = CONVERT(CHAR(19), B.CREATE_TIME, 20 ) and A.LEVEL2ID = b.LEVEL2ID where A.BILLET_STATUS = 2 and b.ORDER_OF_BILLET = {0} order by b.ORDER_OF_BILLET DESC", (i + 1));

                SqlDataReader reader = this.executeReaderQuerySQL(sql, sqlCon);

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Order_Number = (reader.IsDBNull(1) == true) ? "" : reader.GetString(0);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Heat_Id = (reader.IsDBNull(1) == true) ? "" : reader.GetString(1);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Charge_Temperature = (reader.IsDBNull(2) == true) ? 0 : reader.GetInt16(2);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Discharge_Target_Temperature = (reader.IsDBNull(3) == true) ? 0 : reader.GetInt16(3);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Predict_Discharge_Billet_Temperature = (reader.IsDBNull(4) == true) ? 0 : reader.GetInt16(4);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Predict_Current_Billet_Temperature = (reader.IsDBNull(5) == true) ? 0 : reader.GetInt16(5);
                        this.m_MainClass.stBILLET_INFOMATION[i].nOrderOfBillet = (reader.IsDBNull(6) == true) ? 0 : reader.GetInt16(6);
                        this.m_MainClass.stBILLET_INFOMATION[i].nZone_Average_Temperature = (reader.IsDBNull(7) == true) ? 0 : reader.GetInt16(7);

                        nBillet_Count_On_FUrnace++;
                    }
                }
                reader.Close();
            }

            

            this.m_MainClass.m_Define_Class.nBillet_Count_On_FUrnace = nBillet_Count_On_FUrnace;


            sqlCon.Close();


            return nBillet_Count_On_FUrnace; 
        }

        /**
         * Main > Chart 표현을 위한 데이터 갱신 
         */
        public int updateChartStruct_ver2()
        {
            SqlCommand scomm;
            SqlDataReader reader;

            this.sql = string.Format("select A.ORDER_NUMBER, A.HEAT_ID, A.CHARGE_TEMPERATURE, A.DISCHARGE_TEMPERATURE, b.PREDICT_DISCHARGE_BILLET_TEMPERATURE, b.PREDICT_CURRENT_BILLET_TEMPERATURE, b.ORDER_OF_BILLET, b.ZONE_AVERAGE_TEMPERATURE from BILLET as A inner join BILLET_STATUS as b on CONVERT(CHAR(19), A.CREATE_TIME, 20) = CONVERT(CHAR(19), B.CREATE_TIME, 20 ) and A.LEVEL2ID = b.LEVEL2ID where A.BILLET_STATUS = 2 order by b.ORDER_OF_BILLET ASC");

            scomm = new SqlCommand();
            scomm.Connection = this.m_MainClass.m_SQLClass.SqlCon;
            scomm.CommandText = sql;

            this.m_MainClass.m_SQLClass.SqlCon.Open();

            reader = scomm.ExecuteReader();

            int nBillet_Count_On_FUrnace = 0;                

            
               // reader = this.executeReaderQuerySQL(sql);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int i = reader.GetInt16(6) - 1;     // b.ORDER_OF_BILLET (배열의 인덱스로 사용하기 위해 -1 처리)

                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Order_Number = (reader.IsDBNull(1) == true) ? "" : reader.GetString(0);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Heat_Id = (reader.IsDBNull(1) == true) ? "" : reader.GetString(1);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Charge_Temperature = (reader.IsDBNull(2) == true) ? 0 : reader.GetInt16(2);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Discharge_Target_Temperature = (reader.IsDBNull(3) == true) ? 0 : reader.GetInt16(3);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Predict_Discharge_Billet_Temperature = (reader.IsDBNull(4) == true) ? 0 : reader.GetInt16(4);
                        this.m_MainClass.stBILLET_INFOMATION[i].nBillet_Predict_Current_Billet_Temperature = (reader.IsDBNull(5) == true) ? 0 : reader.GetInt16(5);
                        this.m_MainClass.stBILLET_INFOMATION[i].nOrderOfBillet = (reader.IsDBNull(6) == true) ? 0 : reader.GetInt16(6);
                        this.m_MainClass.stBILLET_INFOMATION[i].nZone_Average_Temperature = (reader.IsDBNull(7) == true) ? 0 : reader.GetInt16(7);

                        nBillet_Count_On_FUrnace++;
                    }
                }
                
            reader.Close();

            this.m_MainClass.m_SQLClass.SqlCon.Close();

            this.m_MainClass.m_Define_Class.nBillet_Count_On_FUrnace = nBillet_Count_On_FUrnace;

            return nBillet_Count_On_FUrnace;
        }

        private void Con_StateChange(object sender, StateChangeEventArgs s)
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, string.Format("{0}:{1}", s.OriginalState, s.CurrentState));
        }

        private void Con_InfoMessage(object sender, SqlInfoMessageEventArgs s)
        {
            //this.m_MainClass.m_SysLogClass.SystemLog(this, s.Message);
        }
    }
}