using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SDI_LCS
{
    public class Work_DB
    {
        public int Carrier_1_Down = 0;
        public int Carrier_2_Down = 0;
        public int Carrier_3_Down = 0;

        public int Carrier_1_Up = 0;
        public int Carrier_2_Up = 0;
        public int Carrier_3_Up = 0;

        public string[] Floor3_DestPort = new string[Form1.MAX_COMMAND_SIZE];
        public string[] Rack_PortID = new string[Form1.MAX_RACK_COUNT];
        public MySqlConnection[] SQL_connect_DB = new MySqlConnection[Form1.LGV_NUM];
        public MySqlConnection SQL_connect_DB_NoIDX = new MySqlConnection();
        public MySqlCommand[] SQL_command_DB = new MySqlCommand[Form1.LGV_NUM];
        string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
        public int Work_Type_Count = 0;
        public string Work_Name;
        public string Loc;
        public string Step;
        public int Path_Count = 0;
        Form1 Main;
        public ArrayList waitCommand;
        public int Old_Count = 0;
        public int New_Count = 0;
        public int Working_Command_Count = 0;
        public int Command_Count_Floor3 = 0;
        public int Rack_Count = 0;
        
        //생성자
        public Work_DB(Form1 CS_Main)
        {
            Main = CS_Main;
            waitCommand = new ArrayList();
            SQL_connect_DB_NoIDX = new MySqlConnection(Mariadb);
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                SQL_connect_DB[i] = new MySqlConnection(Mariadb);
            }
            for(int i = 0; i < Form1.MAX_COMMAND_SIZE; i++)
            {
                Floor3_DestPort[i] = "";
            }
            for (int i = 0; i < Form1.MAX_RACK_COUNT; i++)
            {
                Rack_PortID[i] = "";
            }
        }

 
        // 광폭 기재랙 입고 확인. 입고된 기재가 있으면 1, 없으면 0을 RETURN. 에러는 2를 RETURN. lkw20190129
        public int Select_DB_Duplication(int idx, string Dest_Port_Name)
        {
            int result = 2;

            try
            {
                using (MySqlConnection SQL_connect_DB = new MySqlConnection(Mariadb))
                {
                    SQL_connect_DB.Open();

                    string sql = "SELECT PORT_LOAD FROM TB_RACK_INFO WHERE PORT_NAME = '" + Dest_Port_Name + "'";

                    MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    adp.Dispose();
                    ds.Dispose();
                    adp.Dispose();

                    SQL_connect_DB.Close();
                    SQL_connect_DB.Dispose();
                }
                   
                return result;
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_DB_Duplication" + Convert.ToString(ex));
                return result = 2;
            }
        }

        //오버브릿지 리프트 검색
        public void Select_OverBridge_Info()
        {
            string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
            string log;

            try
            {
                using (MySqlConnection SQL_connect_DB = new MySqlConnection(Mariadb))
                {
                    SQL_connect_DB.Open();

                    DataSet DS_DB_Process = new DataSet();

                    string sql = "SELECT * FROM tb_overbridge_lift_info2";

                    MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB);
                    adp.Fill(DS_DB_Process);

                    if (DS_DB_Process.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                        {
                            if(Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]) == "CC8FLF02")
                            {
                                Carrier_1_Down = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][1]);
                                Carrier_2_Down = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][2]);
                            }
                            if (Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]) == "CC1FLF02")
                            {
                                Carrier_3_Down = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][3]);
                            }

                            if (Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]) == "CA7FLF02")
                            {
                                Carrier_1_Up = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][1]);
                                Carrier_2_Up = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][2]);
                            }
                            if (Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]) == "CA1FLF02")
                            {
                                Carrier_3_Up = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][3]);
                            }
                        }
                    }
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        if(Carrier_1_Down == 0)
                        {
                            Main.P_Carrier_D_1.BackColor = Color.Black;
                        }
                        else if (Carrier_1_Down == 1)
                        {
                            Main.P_Carrier_D_1.BackColor = Color.Lime;
                        }

                        if (Carrier_2_Down == 0)
                        {
                            Main.P_Carrier_D_2.BackColor = Color.Black;
                        }
                        else if (Carrier_2_Down == 1)
                        {
                            Main.P_Carrier_D_2.BackColor = Color.Lime;
                        }

                        if (Carrier_3_Down == 0)
                        {
                            Main.P_Carrier_D_3.BackColor = Color.Black;
                        }
                        else if (Carrier_3_Down == 1)
                        {
                            Main.P_Carrier_D_3.BackColor = Color.Lime;
                        }

                        if (Carrier_1_Up == 0)
                        {
                            Main.P_Carrier_U_1.BackColor = Color.Black;
                        }
                        else if (Carrier_1_Up == 1)
                        {
                            Main.P_Carrier_U_1.BackColor = Color.Lime;
                        }

                        if (Carrier_2_Up == 0)
                        {
                            Main.P_Carrier_U_2.BackColor = Color.Black;
                        }
                        else if (Carrier_2_Up == 1)
                        {
                            Main.P_Carrier_U_2.BackColor = Color.Lime;
                        }

                        if (Carrier_3_Up == 0)
                        {
                            Main.P_Carrier_U_3.BackColor = Color.Black;
                        }
                        else if (Carrier_3_Up == 1)
                        {
                            Main.P_Carrier_U_3.BackColor = Color.Lime;
                        }

                        log = string.Format("하강리프트_1 = {0}, 하강리프트_2 = {1}, 하강리프트_3 = {2}, 상승리프트_1 = {3}, 상승리프트_2 = {4}, 상승리프트_3 = {5}"
                                            , Carrier_1_Down, Carrier_2_Down, Carrier_3_Down, Carrier_1_Up, Carrier_2_Up, Carrier_3_Up);
                        Main.Log("오버브릿지 리프트 정보", log);


                    }));;
                    adp.Dispose();
                    DS_DB_Process.Dispose();

                    SQL_connect_DB.Close();
                    SQL_connect_DB.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_OverBridge_Info" + Convert.ToString(ex));
            }
        }

        //렉정보 검색
        public void Select_Rack_Info()
        {
            string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";


            try
            {
                using (MySqlConnection SQL_connect_DB_Floor3 = new MySqlConnection(Mariadb))
                {
                    SQL_connect_DB_Floor3.Open();

                    DataSet DS_DB_Process = new DataSet();

                    string sql = "SELECT PORT_NAME FROM tb_rack_info";

                    MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_Floor3);
                    adp.Fill(DS_DB_Process);
                    Rack_Count = DS_DB_Process.Tables[0].Rows.Count;

                    if (DS_DB_Process.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                        {
                            Rack_PortID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                        }
                    }
                    adp.Dispose();
                    DS_DB_Process.Dispose();

                    SQL_connect_DB_Floor3.Close();
                    SQL_connect_DB_Floor3.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Floor3" + Convert.ToString(ex));
            }
        }

        //MCS - 3층 작업 테이블 검색
        public void Select_MCS_Command_Info_Floor3()
        {
            string Mariadb = "SERVER=17.91.229.138; DATABASE=samsung_sdi_db; UID=root; PASSWORD=mydb;";

            try
            {
                using (MySqlConnection SQL_connect_DB_Floor3 = new MySqlConnection(Mariadb))
                {
                    SQL_connect_DB_Floor3.Open();

                    DataSet DS_DB_Process = new DataSet();

                    string sql = "SELECT DEST_PORT FROM tb_transfer_command_info WHERE ALLOC_STATE = 5 OR ALLOC_STATE = 6 OR ALLOC_STATE = 2";

                    MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_Floor3);
                    adp.Fill(DS_DB_Process);
                    Command_Count_Floor3 = DS_DB_Process.Tables[0].Rows.Count;

                    if (DS_DB_Process.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                        {
                            Floor3_DestPort[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                        }
                    }
                    adp.Dispose();
                    DS_DB_Process.Dispose();

                    SQL_connect_DB_Floor3.Close();
                    SQL_connect_DB_Floor3.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Floor3" + Convert.ToString(ex));
            }
        }

        //작업 로그 추가 해주기
        public void Insert_Excel_Data_Log(string Call_Time, string Command_ID, string Carrier_ID, string Source_Port, string Dest_Port, int LGV_No,
                                      string Alloc_TIME, string Load_Move_Time, string Load_Time, string UnLoad_Move_Time, string UnLoad_Time,
                                      string Alloc_State, string Load_Move_Time_Result, string Load_Time_Result, string UnLoad_Move_Time_Result, string UnLoad_Time_Result,
                                      string Total_Time_Result, string Complete_Time, string Current)
        {
            try
            {
                /*
                OleDbCommand dbCommand;
                string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\\SDI_DATA\\Work_Log.xlsx;Extended Properties=""Excel 12.0;HDR=YES;""";
                OleDbConnection excelConnection = new OleDbConnection(conStr);
                excelConnection.Open();
                */

                //신규 로그 추가. lkw20190109
                OleDbCommand dbCommand;
                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyyMMdd");

                string Mapping_Source_Port = "";
                string Mapping_Dest_Port = "";

                string ori_FilePath = Main.f_dir + "\\Config\\Work_Log.xlsx";
                string FilePath = Main.f_dir + "\\" + date + "\\" + date + "_WorkLog.xlsx";
                FileInfo fi = new FileInfo(FilePath);

                if (fi.Exists != true)
                {
                    System.IO.File.Copy(ori_FilePath, FilePath, true);
                }
                string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FilePath + "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
                OleDbConnection excelConnection = new OleDbConnection(conStr);
                excelConnection.Open();


                Mapping_Source_Port = Select_Mapping_WorkStation(Source_Port);
                Mapping_Dest_Port = Select_Mapping_WorkStation(Dest_Port);

                dbCommand = new OleDbCommand("INSERT INTO [Sheet$](콜시간,작업명,제품명,출발지,도착지,차량_번호,할당_시간,할당_위치,적재장소도착,적재완료,이재장소도착,이재완료,작업완료시간,작업_상태,이동시간_적재,작업시간_적재,이동시간_이재,작업시간_이재,최종작업시간) " +
                "VALUES ('" + Call_Time + "','" + Command_ID + "','" + Carrier_ID + "','" + Mapping_Source_Port + "','" + Mapping_Dest_Port + "','"+ LGV_No + "','"+ Alloc_TIME + "','" + Current + "'," +
                "'" + Load_Move_Time + "','"+ Load_Time + "','"+ UnLoad_Move_Time + "','"+ UnLoad_Time + "','"+ Complete_Time + "','"+ Alloc_State + "','"+ Load_Move_Time_Result + "'," +
                "'"+ Load_Time_Result + "','"+ UnLoad_Move_Time_Result + "','"+ UnLoad_Time_Result + "','"+ Total_Time_Result + "')", excelConnection);
                
              
                dbCommand.ExecuteNonQuery();
                excelConnection.Close();

                //Select_MCS_Command_Info_Log_View();
            }
            catch (OleDbException ex)
            {
                Main.Log("Excel_ERROR", "Insert_Excel_Data" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //MCS 작업 테이블 검색 - 로그
        public void Select_MCS_Command_Info_Log_View()
        {
            string Mapping_Source_Port = "";
            string Mapping_Dest_Port = "";
            try
            {

                //신규 로그 추가. lkw20190109
                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyyMMdd");

                string ori_FilePath = Main.f_dir + "\\Config\\Work_Log.xlsx";
                string FilePath = Main.f_dir + "\\" + date + "\\" + date + "_WorkLog.xlsx";
                FileInfo fi = new FileInfo(FilePath);

                if (fi.Exists != true)
                {
                    System.IO.File.Copy(ori_FilePath, FilePath, true);
                }
                
                string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FilePath + "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
                OleDbConnection excelConnection = new OleDbConnection(conStr);
                excelConnection.Open();

                DataTable Grid_Table = new DataTable();
                string strSQL = "SELECT * FROM [Sheet$]";
                OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

                dataAdapter.Fill(Grid_Table);
                Main.Form_Work_Log.label3.Text = Grid_Table.Rows.Count + "개";

                if (Grid_Table.Rows.Count > 0)
                {
                    for (int i = 0; i < Grid_Table.Rows.Count; i++)
                    {
                        #region 차량 번호 맵핑
                        if ((string)Grid_Table.Rows[i][5] == "-1")
                        {
                            Grid_Table.Rows[i][5] = "미할당";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "1")
                        {
                            Grid_Table.Rows[i][5] = "1호-양극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "2")
                        {
                            Grid_Table.Rows[i][5] = "2호-음극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "3")
                        {
                            Grid_Table.Rows[i][5] = "3호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "4")
                        {
                            Grid_Table.Rows[i][5] = "4호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "5")
                        {
                            Grid_Table.Rows[i][5] = "5호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "6")
                        {
                            Grid_Table.Rows[i][5] = "6호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "7")
                        {
                            Grid_Table.Rows[i][5] = "7호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "8")
                        {
                            Grid_Table.Rows[i][5] = "8호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "9")
                        {
                            Grid_Table.Rows[i][5] = "음극 광폭";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "10")
                        {
                            Grid_Table.Rows[i][5] = "10호";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "11")
                        {
                            Grid_Table.Rows[i][5] = "양극 광폭";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "12")
                        {
                            Grid_Table.Rows[i][5] = "12호-음극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "13")
                        {
                            Grid_Table.Rows[i][5] = "13호-양극Reel";
                        }
                        #endregion
                    }
                    
                }
                Main.Form_Work_Log.Grid_Work_Log.DataSource = Grid_Table;
                
                // dispose used objects
                Grid_Table.Dispose();
                dataAdapter.Dispose();
                dbCommand.Dispose();

                excelConnection.Close();
                excelConnection.Dispose();
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Log" + Convert.ToString(ex));
                return;
            }
        }
        
        //MCS 작업 테이블 검색 - 로그
        public void Select_MCS_Command_Info_Log_View(string date)
        {
            string Mapping_Source_Port = "";
            string Mapping_Dest_Port = "";
            string Date = "";
            string ori_FilePath = Main.f_dir + "\\Config\\Work_Log.xlsx";
            Date = date.Replace("-", "");
            string FilePath = Main.f_dir + "\\" + Date + "\\" + Date + "_WorkLog.xlsx";
            string DirPath = Main.f_dir + "\\" + Date + "\\";
            FileInfo fi = new FileInfo(FilePath);
            DirectoryInfo di = new DirectoryInfo(DirPath);
            try 
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);

                if (fi.Exists != true)
                {
                    System.IO.File.Copy(ori_FilePath, FilePath, true);
                }
                
                string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FilePath + "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
                OleDbConnection excelConnection = new OleDbConnection(conStr);
                excelConnection.Open();

                DataTable Grid_Table = new DataTable();
                string strSQL = "SELECT * FROM [Sheet$]";
                OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

                dataAdapter.Fill(Grid_Table);
                Main.Form_Work_Log.label3.Text = Grid_Table.Rows.Count + "개";

                if (Grid_Table.Rows.Count > 0)
                {
                    for (int i = 0; i < Grid_Table.Rows.Count; i++)
                    {
                      
                        #region 차량 번호 맵핑
                        if ((string)Grid_Table.Rows[i][5] == "-1")
                        {
                            Grid_Table.Rows[i][5] = "미할당";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "1")
                        {
                            Grid_Table.Rows[i][5] = "1호-양극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "2")
                        {
                            Grid_Table.Rows[i][5] = "2호-음극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "3")
                        {
                            Grid_Table.Rows[i][5] = "3호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "4")
                        {
                            Grid_Table.Rows[i][5] = "4호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "5")
                        {
                            Grid_Table.Rows[i][5] = "5호-양극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "6")
                        {
                            Grid_Table.Rows[i][5] = "6호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "7")
                        {
                            Grid_Table.Rows[i][5] = "7호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "8")
                        {
                            Grid_Table.Rows[i][5] = "8호-음극Roll";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "9")
                        {
                            Grid_Table.Rows[i][5] = "음극 광폭";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "10")
                        {
                            Grid_Table.Rows[i][5] = "10호";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "11")
                        {
                            Grid_Table.Rows[i][5] = "양극 광폭";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "12")
                        {
                            Grid_Table.Rows[i][5] = "12호-음극Reel";
                        }
                        else if ((string)Grid_Table.Rows[i][5] == "13")
                        {
                            Grid_Table.Rows[i][5] = "13호-양극Reel";
                        }

                        #endregion
                    }

                }
                Main.Form_Work_Log.Grid_Work_Log.DataSource = Grid_Table;

                // dispose used objects
                Grid_Table.Dispose();
                dataAdapter.Dispose();
                dbCommand.Dispose();

                excelConnection.Close();
                excelConnection.Dispose();
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Log" + Convert.ToString(ex));
                return;
            }
        }
        
        public void Update_AGV_Info_WaitCarrierID(int idx, string AGV_ID, string Wait_CarrierID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info SET WAIT_CARRIER_ID  = '" + Wait_CarrierID + "' WHERE VEHICLE_ID = '" + AGV_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_AGV_Info_WaitCarrierID" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        public void Update_AGV_Info_Setting_Value(string AGV_ID, string Setting_Low_Battery, string Setting_Middle_Value, string Setting_Job_Count)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info SET SETTING_LOW_BATTERY  = '" + Setting_Low_Battery + "', SETTING_MIDDLE_BATTERY  = '" + Setting_Middle_Value + "', SETTING_JOB_COUNT  = '" + Setting_Job_Count + "' WHERE VEHICLE_ID = '" + AGV_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_AGV_Info_WaitCarrierID" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        public void Update_AGV_Info_State(int idx, string AGV_ID, string State)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
                
                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info SET VEHICLE_STATE  = '" + State + "' WHERE VEHICLE_ID = '" + AGV_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_AGV_Info_State" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }
        //포장실 캐리어 아이디 올려주기
        public void Update_CarrierID_Packing(string Name, string CarrierID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_rack_info SET CARRIER_ID  = '" + CarrierID + "' WHERE PORT_NAME = '" + Name + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                    Main.Log("UPDATE_CarrierID", "NAME = " + Name +", Carrier_ID = " + CarrierID);

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_CarrierID_Packing" + Convert.ToString(ex));
            }
        }
        
        //차량 정보 테이블 업데이트
        public void Update_AGV_Info(int idx, string AGV_ID, string State, string Current, string Target, string Goal
            , string Carrier_ID, string Install_Time, string Source_Port, string Dest_Post, string Carrier_Loc, string Carrier_Type, string Alarm_ID, string Alarm_Text
            , string Source_Port_Num, string Dest_Port_Num, string Command_ID, string Priority)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info SET VEHICLE_STATE  = '" + State + "', VEHICLE_CURRENT_POSITION  = '" + Current +
                     "', VEHICLE_NEXT_POSITION  = '" + Target + "', VEHICLE_GOAL  = '" + Goal +
                     "', CARRIER_ID  = '" + Carrier_ID + "', INSTALL_TIME  = '" + Install_Time +
                     "', SOURCE_PORT  = '" + Source_Port + "', DEST_PORT  = '" + Dest_Post +
                     "', CARRIER_LOC  = '" + Carrier_Loc + "', CARRIER_TYPE  = '" + Carrier_Type +
                     "', VEHICLE_ALARM_ID  = '" + Alarm_ID + "', VEHICLE_ALARM_TEXT  = '" + Alarm_Text +
                     "', SOURCE_PORT_NUM  = '" + Source_Port_Num + "', DEST_PORT_NUM  = '" + Dest_Port_Num +
                     "', VEHICLE_COMMAND_ID  = '" + Command_ID + "', VEHICLE_PRIORITY  = '" + Priority +
                     "' WHERE VEHICLE_ID = '" + AGV_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_AGV_Info" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }
        #region 작업시간 업데이트
        //적재 완료 시간 업데이트
        public void Update_Load_Time(int idx, string Load_Time, string Command_ID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET LOAD_TIME  = '" + Load_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Load_Time" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //적재 장소 도착 시간 업데이트 - 주행
        public void Update_Load_Move_Time(int idx, string Load_Move_Time, string Command_ID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET LOAD_MOVE_TIME  = '" + Load_Move_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Load_Move_Time" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }

        }
        //이재 완료 시간 업데이트
        public void Update_Unload_Time(int idx, string Unload_Time, string Command_ID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET UNLOAD_TIME  = '" + Unload_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Unload_Time" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }
        //이재 장소 도착 시간 업데이트
        public void Update_Unload_Move_Time(int idx, string Unload_Move_Time, string Command_ID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET UNLOAD_MOVE_TIME  = '" + Unload_Move_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Unload_Move_Time" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }

        }
        #endregion
        //차량 정보 테이블 업데이트
        public void Update_Command_Info(int idx, string Command_ID, string Alloc_State, int LGV_No, string Alloc_Time, string End_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
 
                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE  = '" + Alloc_State + "', LGV_NO  = '" + (LGV_No+1) +
                     "', ALLOC_TIME  = '" + Alloc_Time +
                     "', COMPLETE_TIME  = '" + End_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Info" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }


        }

        //명령 취소 , 중단업데이트 함수
        public void Update_Command_Abort(string Command_ID, string Alloc_State)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE  = '" + Alloc_State + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Info" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }


        }

        //차량 정보 테이블 업데이트
        public void Update_Command_Info_End(int idx, string Command_ID, string Alloc_State, int LGV_No, string End_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE  = '" + Alloc_State + "', LGV_NO  = '" + (LGV_No + 1) +
                  "', COMPLETE_TIME  = '" + End_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Info" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //차량 정보 테이블 업데이트
        public void Update_Command_State_AbortX(int idx, string Command_ID, string Alloc_State, int LGV_No)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE  = '" + Alloc_State + "', LGV_NO  = '" + (LGV_No+1) + "' WHERE COMMAND_ID = '" + Command_ID + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Info" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //우선순위 업데이트
        public void Update_Command_Priority(string Command_ID, string Priority)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET PROIORITY = '" + Priority + "' WHERE COMMAND_ID = '" + Command_ID + "'");
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Priority" + Convert.ToString(ex));
                return;
            }
        }

        //Transfer UpDATE
        public void Update_Command_Transfer(string Command_ID, string Carrier_ID, string Dest_Port)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET CARRIER_ID = '" + Carrier_ID + "',DEST_PORT = '" + Dest_Port + "'" +
                   " WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Command_Transfer" + Convert.ToString(ex));
                return;
            }
        }

        //MCS - 작업 테이블 업데이트 - ALLOC_STATE
        public void Update_MCS_Command_ALLOC_STATE(int idx, string Command_ID, string Alloc_State)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE = '" + Alloc_State + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }
        }

        public void Update_Load_Move_Result(int idx, string Command_ID, string Work_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET LOAD_MOVE_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }
        }

        public void Update_Load_Result(int idx, string Command_ID, string Work_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET LOAD_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }

        }

        public void Update_UnLoad_Move_Result(int idx, string Command_ID, string Work_Time)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET UNLOAD_MOVE_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }



        }

        public void Update_UnLoad_Result(int idx, string Command_ID, string Work_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET UNLOAD_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }
        }


        public void Update_Total_Time_Result(int idx, string Command_ID, string Work_Time)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_transfer_command_info SET TOTAL_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;
                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
                Select_MCS_Command_Info(0);
                Select_MCS_Command_Info_Main(0);
                Select_MCS_Command_Info_Send_MCS();
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_MCS_Command_ALLOC_STATE" + Convert.ToString(ex));
                return;
            }

        }

        public void Select_Result_Work_Time(int idx, string Command_ID)
        {
            DateTime Alloc_Time = new DateTime();
            DateTime Load_Move_Time = new DateTime();
            DateTime Load_Time = new DateTime();
            DateTime UnLoad_Move_Time = new DateTime();
            DateTime UnLoad_Time = new DateTime();
            //DateTime Complete_Time = new DateTime();


            try
            {
                if (SQL_connect_DB[idx].State == ConnectionState.Closed)
                {
                    SQL_connect_DB[idx].Open();
                }
                DataSet DS_DB_Process = new DataSet();

                string sql = "SELECT * FROM tb_transfer_command_info WHERE COMMAND_ID = '" + Command_ID + "'";

                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB[idx]);
                adp.Fill(DS_DB_Process);
                

                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                {
                    Alloc_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][25]);
                    //Complete_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][26]);
                    Load_Move_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][28]);
                    Load_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][29]);
                    UnLoad_Move_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][30]);
                    UnLoad_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][31]);
                }


                TimeSpan Load_Move_Time_Result = Load_Move_Time - Alloc_Time;
                TimeSpan Load_Time_Result = Load_Time - Load_Move_Time;
                TimeSpan UnLoad_Move_Time_Result = UnLoad_Move_Time - Load_Time;
                TimeSpan UnLoad_Time_Result = UnLoad_Time - UnLoad_Move_Time;
                //TimeSpan Total_Time_Result = Complete_Time - Alloc_Time;



                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB[idx].State == ConnectionState.Open)
                {
                    SQL_connect_DB[idx].Close();
                }
                Update_Load_Move_Result(idx, Command_ID, Convert.ToString(Load_Move_Time_Result));
                Update_Load_Result(idx, Command_ID, Convert.ToString(Load_Time_Result));
                Update_UnLoad_Move_Result(idx, Command_ID, Convert.ToString(UnLoad_Move_Time_Result));
                Update_UnLoad_Result(idx, Command_ID, Convert.ToString(UnLoad_Time_Result));
                //Update_Total_Time_Result(idx, Command_ID, Convert.ToString(Total_Time_Result));

            }
            catch (MySqlException ex)
            {
                Main.Form_MCS.SendS2F50(Main.Form_MCS.m_nDeviceID, 0, 5);
                Main.Log("DB_ERROR", "Select_Result_Work_Time" + Convert.ToString(ex));

            }
        }

        public void Select_Result_Total_Time(int idx, string Command_ID)
        {

            DateTime Complete_Time = new DateTime();
            DateTime Alloc_Time = new DateTime();

            try
            {
                if (SQL_connect_DB[idx].State == ConnectionState.Closed)
                {
                    SQL_connect_DB[idx].Open();
                }
                DataSet DS_DB_Process = new DataSet();

                string sql = "SELECT * FROM tb_transfer_command_info WHERE COMMAND_ID = '" + Command_ID + "'";

                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB[idx]);
                adp.Fill(DS_DB_Process);
          

                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                {
                    Alloc_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][25]);
                    Complete_Time = Convert.ToDateTime(DS_DB_Process.Tables[0].Rows[0][26]);

                }
                TimeSpan Total_Time_Result = Complete_Time - Alloc_Time;
                


                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB[idx].State == ConnectionState.Open)
                {
                    SQL_connect_DB[idx].Close();
                }
                Update_Total_Time_Result(idx, Command_ID, Convert.ToString(Total_Time_Result));

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_Result_Total_Time" + Convert.ToString(ex));

            }
        }
        public void Init_Command()
        {
            for(int i = 0; i < Form1.MAX_COMMAND_SIZE; i++)
            {
                Main.m_stCommand[i].Command_ID = "";
                Main.m_stCommand[i].Proiority = "";
                Main.m_stCommand[i].Carrier_ID = "";
                Main.m_stCommand[i].Source_Port = "";
                Main.m_stCommand[i].Dest_Port = "";
                Main.m_stCommand[i].Carrier_Type = "";
                Main.m_stCommand[i].Carrier_LOC = "";
                Main.m_stCommand[i].Process_ID = "";
                Main.m_stCommand[i].Batch_ID = "";
                Main.m_stCommand[i].LOT_ID = "";
                Main.m_stCommand[i].Carrier_S_Count = "";
                Main.m_stCommand[i].Alloc_State = "";
                Main.m_stCommand[i].LGV_No = "";
                Main.m_stCommand[i].Call_Time = "";
                Main.m_stCommand[i].Alloc_Time = "";
                Main.m_stCommand[i].Complete_Time = "";
                Main.m_stCommand[i].Transfer_State = "";
                Main.m_stCommand[i].Quantity = "";

                Main.m_stCommand[i].Work_Name = "";
                Main.m_stCommand[i].Work_Type = "";
                Main.m_stCommand[i].Alloc_Ok = "";
                Main.m_stCommand[i].Start_Loc = "";
                Main.m_stCommand[i].End_Loc = "";

                Main.m_stCommand[i].Load_Move_Time = "";
                Main.m_stCommand[i].Load_Time = "";
                Main.m_stCommand[i].UnLoad_Move_Time = "";
                Main.m_stCommand[i].UnLoad_Time = "";

            }
        }
        //MCS - 작업 테이블 검색
        public void Select_MCS_Command_Info(int idx)
        {
            /*
            ALLOC_STATE
            0 = 대기중
            1 = 진행중
            2 = 정상 완료
            3 = 취소
            4 = 중단
            5 = 진행중 (취소 불가)
            6 = 진행중 (중단 불가)
            */
            try
            {
                if (SQL_connect_DB[idx].State == ConnectionState.Closed)
                {
                    SQL_connect_DB[idx].Open();
                }
                DataSet DS_DB_Process = new DataSet();

                string sql = "SELECT * FROM tb_transfer_command_info ORDER BY CALL_TIME";

                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB[idx]);
                adp.Fill(DS_DB_Process);

                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                {
                    waitCommand.Clear();
                    Init_Command();

                    for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                    {
                        Main.m_stCommand[i].Command_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                        Main.m_stCommand[i].Proiority = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][1]);
                        Main.m_stCommand[i].Carrier_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][2]);
                        Main.m_stCommand[i].Source_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                        Main.m_stCommand[i].Dest_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][4]);
                        Main.m_stCommand[i].Carrier_Type = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][5]);
                        Main.m_stCommand[i].Carrier_LOC = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][6]);
                        Main.m_stCommand[i].Process_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][7]);
                        Main.m_stCommand[i].Batch_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][8]);
                        Main.m_stCommand[i].LOT_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][9]);
                        Main.m_stCommand[i].Carrier_S_Count = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][10]);
                        Main.m_stCommand[i].Carrier_S_ID[0] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][11]);
                        Main.m_stCommand[i].Carrier_S_ID[1] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][12]);
                        Main.m_stCommand[i].Carrier_S_ID[2] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][13]);
                        Main.m_stCommand[i].Carrier_S_ID[3] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][14]);
                        Main.m_stCommand[i].Carrier_S_ID[4] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][15]);
                        Main.m_stCommand[i].Carrier_S_ID[5] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][16]);
                        Main.m_stCommand[i].Carrier_S_ID[6] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][17]);
                        Main.m_stCommand[i].Carrier_S_ID[7] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][18]);
                        Main.m_stCommand[i].Carrier_S_ID[8] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][19]);
                        Main.m_stCommand[i].Carrier_S_ID[9] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][20]);
                        Main.m_stCommand[i].Alloc_State = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][21]);
                        Main.m_stCommand[i].Transfer_State = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][22]);
                        Main.m_stCommand[i].LGV_No = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][23]);
                        Main.m_stCommand[i].Call_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][24]);
                        Main.m_stCommand[i].Alloc_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][25]);
                        Main.m_stCommand[i].Complete_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][26]);
                        Main.m_stCommand[i].Quantity = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][27]);
                        Main.m_stCommand[i].Load_Move_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][28]);
                        Main.m_stCommand[i].Load_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][29]);
                        Main.m_stCommand[i].UnLoad_Move_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][30]);
                        Main.m_stCommand[i].UnLoad_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][31]);
                        waitCommand.Add(Main.m_stCommand[i]);
                    }
                }
                else
                {
                    waitCommand.Clear();
                }

                //그리드 출력 및 그리드 초기화
                Main.Form_Command_Info.Grid_Command_Info.DataSource = DS_DB_Process.Tables[0];
                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB[idx].State == ConnectionState.Open)
                {
                    SQL_connect_DB[idx].Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Form_MCS.SendS2F50(Main.Form_MCS.m_nDeviceID, 0, 5);
                Main.Log("DB_ERROR", "Select_MCS_Command_Info" + Convert.ToString(ex));

            }
        }
        //리프트 상태 받아오기
        public void Select_Lift_Info()
        {  
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataSet DS_DB_Process = new DataSet();

                string sql = "SELECT * FROM tb_lift_info";

                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);

                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                { 
                    for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                    {
                        Main.CS_AGV_Logic.Lift_ID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                        Main.CS_AGV_Logic.Work_End_3[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][1]);
                        Main.CS_AGV_Logic.Work_End_1[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][2]);
                        Main.CS_AGV_Logic.Carrier_3[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                        Main.CS_AGV_Logic.Carrier_1[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][4]);
                        Main.CS_AGV_Logic.Lift_State[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][5]);
                        Main.CS_AGV_Logic.Lift_Mode[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][6]);
                        Main.CS_AGV_Logic.Link[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][7]);
                    }
                }
                

                //그리드 출력 및 그리드 초기화
                Main.Form_Command_Info.Grid_Command_Info.DataSource = DS_DB_Process.Tables[0];
                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_Lift_Info" + Convert.ToString(ex));

            }
        }





        /*
        //적재 위치 이동시간 업데이트
        public void Update_DB_Load_Move_WorkTime(int idx, string Command_ID, DateTime End_Date)
        {
            string sql1, tmp;

            TimeSpan Work_Time = End_Date - Main.m_stAGV[0].Work_Start_Time;

            if (Command_ID == "")
            {
                return;
            }

            sql1 = ("UPDATE tb_transfer_command_info SET LOAD_MOVE_TIME_RESULT = '" + Work_Time + "' WHERE COMMAND_ID = '" + Command_ID + "'"); // 테이블 이름
            tmp = sql1;

            if (SQL_connect_DB[idx].State == ConnectionState.Closed)
            {
                SQL_connect_DB[idx].Open();
            }

            SQL_command_DB[idx] = new MySqlCommand(tmp, SQL_connect_DB[idx]);
            SQL_command_DB[idx].ExecuteNonQuery();
            SQL_command_DB[idx].Dispose();

            if (SQL_connect_DB[idx].State == ConnectionState.Open)
            {
                SQL_connect_DB[idx].Close();
            }
            Select_MCS_Command_Info(idx);
            Select_MCS_Command_Info_Main(idx);

        }
        */
        //작업 로그 삭제
        public void Delete_Work_Log(string Command_ID)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string tmp;

                    string sql = ("DELETE FROM tb_transfer_command_info WHERE COMMAND_ID = '" + Command_ID + "'"); // 테이블 이름
                                                                                                                   // 테이블 이름
                    tmp = sql;
                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Delete_Work_Log" + Convert.ToString(ex));
                return;
            }
        }

        //MCS - 작업 테이블 업데이트 - Call_Time
        public void Update_Work_ReOrder(string Command_ID)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;

                    sql1 = ("UPDATE tb_transfer_command_info SET ALLOC_STATE = '0' WHERE COMMAND_ID = '" + Command_ID + "'");                         // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_Work_ReOrder" + Convert.ToString(ex));
                return;
            }

        }
        //MCS 작업 테이블 검색 - 로그
        public void Select_MCS_Command_Info_Log(string Command_ID)
        {

            /*
            ALLOC_STATE
            0 = 대기중
            1 = 진행중
            2 = 정상 완료
            3 = 취소
            4 = 중단
            5 = 진행중 (취소 불가)
            6 = 진행중 (중단 불가)
            */
    
            try
            {

                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataSet DS_DB_Process = new DataSet();
                string sql = "SELECT   CALL_TIME, COMMAND_ID, CARRIER_ID,SOURCE_PORT,DEST_PORT,LGV_NO," +
                                       "ALLOC_TIME,LOAD_MOVE_TIME,LOAD_TIME,UNLOAD_MOVE_TIME,UNLOAD_TIME, COMPLETE_TIME," +
                                       "ALLOC_STATE,LOAD_MOVE_TIME_RESULT,LOAD_TIME_RESULT,UNLOAD_MOVE_TIME_RESULT,UNLOAD_TIME_RESULT,TOTAL_TIME_RESULT  " +
                                       "FROM tb_transfer_command_info WHERE COMMAND_ID = '"+ Command_ID + "'";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);


                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                {
                    Main.CS_AGV_Logic.Init_LOG();
                    Main.CS_AGV_Logic.E_Call_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][0]);
                    Main.CS_AGV_Logic.E_Command_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][1]);
                    Main.CS_AGV_Logic.E_Carrier_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][2]);
                    Main.CS_AGV_Logic.E_Source_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][3]);
                    Main.CS_AGV_Logic.E_Dest_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][4]);
                    Main.CS_AGV_Logic.E_LGV_No = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[0][5]);

                    if(Convert.ToString(DS_DB_Process.Tables[0].Rows[0][6]) != "0") Main.CS_AGV_Logic.E_Alloc_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][6]);
                    Main.CS_AGV_Logic.E_Load_Move_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][7]);
                    Main.CS_AGV_Logic.E_Load_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][8]);
                    Main.CS_AGV_Logic.E_UnLoad_Move_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][9]);
                    Main.CS_AGV_Logic.E_UnLoad_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][10]);
                    Main.CS_AGV_Logic.E_Complete_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][11]);
                    Main.CS_AGV_Logic.E_Alloc_State = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][12]);
                    Main.CS_AGV_Logic.E_Load_Move_Time_Result = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][13]);
                    Main.CS_AGV_Logic.E_Load_Time_Result = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][14]);
                    Main.CS_AGV_Logic.E_UnLoad_Move_Time_Result = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][15]);
                    Main.CS_AGV_Logic.E_UnLoad_Time_Result = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][16]);
                    Main.CS_AGV_Logic.E_Total_Time_Result = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][17]);
                }
                //그리드 출력 및 그리드 초기화
               
                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Log" + Convert.ToString(ex));
                return;
            }
        }

        public void Select_MCS_Command_Info_Send_MCS()
        {
            /*
            ALLOC_STATE
            0 = 대기중
            1 = 진행중
            2 = 정상 완료
            3 = 취소
            4 = 중단
            5 = 진행중 (취소 불가)
            6 = 진행중 (중단 불가)
            */

            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataSet DS_DB_Process = new DataSet();
                string sql = "SELECT * FROM tb_transfer_command_info WHERE ALLOC_STATE = '0' OR ALLOC_STATE ='1' OR ALLOC_STATE ='5' ORDER BY CALL_TIME";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);
                Working_Command_Count = DS_DB_Process.Tables[0].Rows.Count;

                if (DS_DB_Process.Tables[0].Rows.Count > 0)
                {
                    Main.Form_MCS.Init_Mes_Send();
                    for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                    {
                        Main.Form_MCS.MCS_Carrier_ID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][2]);

                        if (Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][23]) == -1)
                        {
                            Main.Form_MCS.MCS_Vehicle_ID[i] = "";
                            Main.Form_MCS.MCS_Carrier_LOC[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                            Main.Form_MCS.MCS_Install_Time[i] = "";
                            Main.Form_MCS.MCS_Carrier_Type[i] = "0";
                            Main.Form_MCS.MCS_Source_Port[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                            Main.Form_MCS.MCS_Dest_Port[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][4]);
                            Main.Form_MCS.MCS_Command_ID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                            Main.Form_MCS.MCS_Transfer_State[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][21]);
                            Main.Form_MCS.MCS_PROIORITY[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][1]);
                     
                        }
                        else if (Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][23]) != -1)
                        {
                            int idx = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][23]) - 1;

                            Main.Form_MCS.MCS_Vehicle_ID[i] = Main.m_stAGV[idx].MCS_Vehicle_ID;
                            Main.Form_MCS.MCS_Carrier_LOC[i] = Main.m_stAGV[idx].MCS_Carrier_LOC;
                            Main.Form_MCS.MCS_Install_Time[i] = Main.m_stAGV[idx].MCS_Install_Time;

                            if (Main.m_stAGV[idx].MCS_Carrier_Type == "" || Main.m_stAGV[idx].MCS_Carrier_Type == null)
                            {
                                Main.Form_MCS.MCS_Carrier_Type[i] = "0";
                            }
                            else if (Main.m_stAGV[idx].MCS_Carrier_Type != "" && Main.m_stAGV[idx].MCS_Carrier_Type != null)
                            {
                                Main.Form_MCS.MCS_Carrier_Type[i] = Main.m_stAGV[idx].MCS_Carrier_Type;
                            }
                            Main.Form_MCS.MCS_Source_Port[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                            Main.Form_MCS.MCS_Dest_Port[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][4]);
                            Main.Form_MCS.MCS_Command_ID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                            Main.Form_MCS.MCS_Transfer_State[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][21]);
                            Main.Form_MCS.MCS_PROIORITY[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][1]);
                        }

                    }
                }

                DS_DB_Process.Dispose();
                adp.Dispose();

                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Send_MCS" + Convert.ToString(ex));
                return;
            }
        }


        //MCS 작업 테이블 검색 - 메인폼
        public void Select_MCS_Command_Info_Main(int idx)
        {
            /*
            ALLOC_STATE
            0 = 대기중
            1 = 진행중
            2 = 정상 완료
            3 = 취소
            4 = 중단
            5 = 진행중 (취소 불가)
            6 = 진행중 (중단 불가)
            */
            string Mapping_Source_Port = "";
            string Mapping_Dest_Port = "";
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataSet DS_DB_Process = new DataSet();
                string sql = "SELECT   COMMAND_ID,CALL_TIME, CARRIER_ID,SOURCE_PORT,DEST_PORT,LGV_NO,ALLOC_STATE FROM tb_transfer_command_info WHERE ALLOC_STATE = '0' OR ALLOC_STATE ='1' OR ALLOC_STATE ='5'OR ALLOC_STATE ='6' ORDER BY CALL_TIME";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);
                

                for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                {
                    Mapping_Source_Port = Select_Mapping_WorkStation((string)DS_DB_Process.Tables[0].Rows[i][3]);
                    Mapping_Dest_Port = Select_Mapping_WorkStation((string)DS_DB_Process.Tables[0].Rows[i][4]);
                    
                    if ((string)DS_DB_Process.Tables[0].Rows[i][6] == "0")
                    {
                        DS_DB_Process.Tables[0].Rows[i][6] = "대기중";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][6] == "1")
                    {
                        DS_DB_Process.Tables[0].Rows[i][6] = "진행중";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][6] == "5")
                    {
                        DS_DB_Process.Tables[0].Rows[i][6] = "진행중(취소 X)";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][6] == "6")
                    {
                        DS_DB_Process.Tables[0].Rows[i][6] = "진행중(중단 X)";
                    }
                    
                    if (Mapping_Source_Port != "")
                    {
                        DS_DB_Process.Tables[0].Rows[i][3] = Mapping_Source_Port;
                    }
                    if (Mapping_Dest_Port != "")
                    {
                        DS_DB_Process.Tables[0].Rows[i][4] = Mapping_Dest_Port;
                    }
                    
                    #region 차량 번호 맵핑
                    if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "-1")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "미할당";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "1")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "1호-양극Reel";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "2")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "2호-음극Reel";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "3")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "3호-양극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "4")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "4호-양극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "5")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "5호-양극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "6")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "6호-음극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "7")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "7호-음극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "8")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "8호-음극Roll";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "9")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "음극 광폭";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "10")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "10호";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "11")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "양극 광폭";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "12")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "12호-음극Reel";
                    }
                    else if ((string)DS_DB_Process.Tables[0].Rows[i][5] == "13")
                    {
                        DS_DB_Process.Tables[0].Rows[i][5] = "13호-양극Reel";
                    }
                    #endregion

                }
                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    //그리드 출력 및 그리드 초기화
                    Main.Grid_Command_Wait.DataSource = DS_DB_Process.Tables[0];
                    Main.Init_Work_GridView();
                }));
                DS_DB_Process.Dispose();
                adp.Dispose();

                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_Command_Info_Main" + Convert.ToString(ex));
                return;
            }
        }

        //AGV정보 테이블 검색
        public void Select_MCS_AGV_Setting_Battery()
        {
            try
            {
                double Convert_Low_Volt = 0;
                double Convert_Middle_Volt = 0;

                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open(); 
                }
                DataSet DS_DB_Process = new DataSet();
                string sql = "SELECT VEHICLE_ID, SETTING_LOW_BATTERY, SETTING_MIDDLE_BATTERY, SETTING_JOB_COUNT FROM tb_agv_info ORDER BY VEHICLE_ID";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);

                for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                {
                    Convert_Low_Volt = Math.Truncate((Convert.ToDouble(DS_DB_Process.Tables[0].Rows[i][1]) / 100) * 380 + 2400);
                    Convert_Middle_Volt = Math.Truncate((Convert.ToDouble(DS_DB_Process.Tables[0].Rows[i][2]) / 100) * 380 + 2400);

                    Main.m_stAGV[i].Setting_Charge_Value = Convert.ToInt32(Convert_Low_Volt);
                    Main.m_stAGV[i].Setting_Middle_Charge_Value = Convert.ToInt32(Convert_Middle_Volt);
                    Main.m_stAGV[i].Setting_Low_Battery_Job_Count = Convert.ToInt32(DS_DB_Process.Tables[0].Rows[i][3]);
                }
                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    //그리드 출력 및 그리드 초기화
                    Main.Form_Setting_Charge.Grid_Setting_Charge.DataSource = DS_DB_Process.Tables[0];
                }));

                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_AGV_Info" + Convert.ToString(ex));
                return;
            }
        }

        //AGV정보 테이블 검색
        public void Select_MCS_AGV_Info()
        {
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataSet DS_DB_Process = new DataSet();
                string sql = "SELECT * FROM tb_agv_info ORDER BY VEHICLE_ID";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                adp.Fill(DS_DB_Process);

                for (int i = 0; i < DS_DB_Process.Tables[0].Rows.Count; i++)
                {
                    Main.m_stAGV[i].MCS_Vehicle_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][0]);
                    Main.m_stAGV[i].MCS_Vehicle_State = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][1]);
                    Main.m_stAGV[i].MCS_Vehicle_Current_Position = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][2]);
                    Main.m_stAGV[i].MCS_Vehicle_Next_Postion = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][3]);
                    Main.m_stAGV[i].MCS_Vehicle_Goal = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][4]);
                    Main.m_stAGV[i].MCS_Carrier_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][5]);
                    Main.m_stAGV[i].MCS_Install_Time = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][6]);
                    Main.m_stAGV[i].MCS_Source_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][7]);
                    Main.m_stAGV[i].MCS_Dest_Port = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][8]);
                    Main.m_stAGV[i].MCS_Carrier_LOC = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][9]);
                    Main.m_stAGV[i].MCS_Carrier_Type = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][10]);
                    Main.m_stAGV[i].MCS_Alarm_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][11]);
                    Main.m_stAGV[i].MCS_Alarm_Text = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][12]);
                    Main.m_stAGV[i].Source_Port_Num = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][13]);
                    Main.m_stAGV[i].Dest_Port_Num = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][14]);
                    Main.m_stAGV[i].MCS_Vehicle_Command_ID = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][15]);
                    Main.m_stAGV[i].MCS_Vehicle_Priority = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][16]);
                    Main.CS_AGV_Logic.Save_CarrierID[i] = Convert.ToString(DS_DB_Process.Tables[0].Rows[i][17]);
                }
                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    //그리드 출력 및 그리드 초기화
                    Main.Form_AGV_Info.Grid_AGV_Info.DataSource = DS_DB_Process.Tables[0];
                }));

                adp.Dispose();
                DS_DB_Process.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_MCS_AGV_Info" + Convert.ToString(ex));
                return;
            }
        }

        //MCS 작업 테이블 추가
        public void Insert_DB_MCS_Work(string sBuff_CarrierID, string[] sBuff_Carrier_S_ID, string sBuff_CommandID,
                                       string sBuff_Source, string sBuff_Dest, string sBuff_ProcessID, string sBuff_Batchid,
                                       string sBuff_LotID, ushort U2Data_Priority, ushort Quantity, string Call_Time)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, sql2, tmp;

                    sql1 = ("INSERT INTO tb_transfer_command_info (CARRIER_ID, COMMAND_ID, SOURCE_PORT, DEST_PORT, PROCESS_ID, BATCH_ID, LOT_ID, PROIORITY," +
                                                                 "CARRIER_S_ID_1, CARRIER_S_ID_2,CARRIER_S_ID_3,CARRIER_S_ID_4,CARRIER_S_ID_5,CARRIER_S_ID_6,CARRIER_S_ID_7," +
                                                                 "CARRIER_S_ID_8, CARRIER_S_ID_9, CARRIER_S_ID_10, QUANTITY, CALL_TIME)");
                    sql2 = ("VALUES ('" + sBuff_CarrierID + "','" + sBuff_CommandID + "','" + sBuff_Source + "','" +
                                          sBuff_Dest + "','" + sBuff_ProcessID + "','" + sBuff_Batchid + "','" + sBuff_LotID + "','" +
                                          U2Data_Priority + "','" + sBuff_Carrier_S_ID[0] + "','" + sBuff_Carrier_S_ID[1] + "','" +
                                          sBuff_Carrier_S_ID[2] + "','" + sBuff_Carrier_S_ID[3] + "','" + sBuff_Carrier_S_ID[4] + "','" +
                                          sBuff_Carrier_S_ID[5] + "','" + sBuff_Carrier_S_ID[6] + "','" + sBuff_Carrier_S_ID[7] + "','" +
                                          sBuff_Carrier_S_ID[8] + "','" + sBuff_Carrier_S_ID[9] + "','" + Quantity + "', '" + Call_Time + "')");
                    tmp = sql1 + sql2;


                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_DB_MCS_Work" + Convert.ToString(ex));
                return;
            }



        }
        //AGV 추가 - 통신
        public void Insert_AGV_List(int idx, string IP, int Port, string Type)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
//                    string sql1, sql2, tmp;

                    string sql = "INSERT INTO tb_agv_wirelessinfo(AGV_NO,IP,PORT,AGV_TYPE) VALUES(" + (idx + 1) + ",'" + IP + "'," + Port + ",'" + Type + "')";

//                    tmp = sql;
//                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    using (MySqlCommand Cmd = new MySqlCommand(sql, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_AGV_List" + Convert.ToString(ex));
                return;
            }



        }

        //차량 정보 검색 - 통신
        public void Select_AGVList()
        {
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
//                string sql1, sql2, tmp;

                string sql = "SELECT * FROM tb_agv_wirelessinfo ORDER BY AGV_NO";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                //추가한 만큼 차량 번호 매겨주기
                Main.Form_Insert_LGV.AGV_List = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Main.CS_AGV_C_Info[i].IP = Convert.ToString(ds.Tables[0].Rows[i][1]);
                        Main.CS_AGV_C_Info[i].Port = Convert.ToInt32(ds.Tables[0].Rows[i][2]);
                        Main.CS_AGV_C_Info[i].Type = Convert.ToString(ds.Tables[0].Rows[i][3]);
                        Main.CS_Connect_LGV.Connect(i, Main.CS_AGV_C_Info[i].IP, Main.CS_AGV_C_Info[i].Port);
                        Main.Insert_Control_AGV(i);
                    }
                }

                Main.Form_Insert_LGV.Grid_AGV_list.DataSource = ds.Tables[0];

                adp.Dispose();
                ds.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_AGVList" + Convert.ToString(ex));
                return;
            }

        }

        public void Delete_AGV(string AGV_No)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();

//                    string sql1, tmp;

                    string sql = ("DELETE FROM tb_agv_wirelessinfo WHERE AGV_NO = '" + AGV_No + "'");                         // 테이블 이름

//                    tmp = sql;
//                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    using (MySqlCommand Cmd = new MySqlCommand(sql, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Delete_AGV" + Convert.ToString(ex));
                return;
            }

        }

        public string Select_Mapping_WorkStation(string Port)
        {
            string WorkStation_Name = "";
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    DataSet DS_DB_Process = new DataSet();

                    string sql = ("SELECT TEXT FROM tb_work_station_path WHERE WORK_STATION = '" + Port + "'");                         // 테이블 이름
                    MySqlDataAdapter adp = new MySqlDataAdapter(sql, Conn);
                    adp.Fill(DS_DB_Process);
                    if (DS_DB_Process.Tables[0].Rows.Count > 0)
                    {
                        WorkStation_Name = Convert.ToString(DS_DB_Process.Tables[0].Rows[0][0]);
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_Mapping_WorkStation" + Convert.ToString(ex));
            }

            return WorkStation_Name;
        }

        //작업 경로 검색
        public void Select_Work_Path()
        {
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }

//                string sql1, sql2, tmp;

                string sql = "SELECT * FROM tb_work_station_path ORDER BY WORK_STATION";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                Path_Count = ds.Tables[0].Rows.Count;

                Main.Form_Manual_Command.CB_Source_Port.Items.Clear();
                Main.Form_Manual_Command.CB_Dest_Port.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Main.Form_Manual_Command.CB_Source_Port.Items.Add(ds.Tables[0].Rows[i][0] + "(" + ds.Tables[0].Rows[i][9] + ")");
                    Main.Form_Manual_Command.CB_Dest_Port.Items.Add(ds.Tables[0].Rows[i][0] + "(" + ds.Tables[0].Rows[i][9] + ")");
                    Main.CS_Work_Path[i].Work_Station = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    Main.CS_Work_Path[i].Stop_Area_1 = Convert.ToString(ds.Tables[0].Rows[i][1]);
                    Main.CS_Work_Path[i].Stop_Area_2 = Convert.ToString(ds.Tables[0].Rows[i][2]);
                    Main.CS_Work_Path[i].Stop_Area_3 = Convert.ToString(ds.Tables[0].Rows[i][3]);
                    Main.CS_Work_Path[i].Goal_Area = Convert.ToString(ds.Tables[0].Rows[i][4]);
                    Main.CS_Work_Path[i].Exit_Area_1 = Convert.ToString(ds.Tables[0].Rows[i][5]);
                    Main.CS_Work_Path[i].Exit_Area_2 = Convert.ToString(ds.Tables[0].Rows[i][6]);
                    Main.CS_Work_Path[i].Exit_Area_3 = Convert.ToString(ds.Tables[0].Rows[i][7]);
                    Main.CS_Work_Path[i].Type = Convert.ToString(ds.Tables[0].Rows[i][8]);

                }

                Main.Form_Work_Path_Setting.Grid_Work_Path_Setting.DataSource = ds.Tables[0];
                Main.Form_Work_Path_Setting.Init_Work_GridView();

                ds.Dispose();
                adp.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_Work_Path" + Convert.ToString(ex));
                return;
            }

        }
        //작업 경로 추가
        public void Insert_DB_Work_Path(string Work_Station, string Stop_Area1, string Stop_Area2, string Stop_Area3,
                                        string Goal_Area, string Exit_Area1, string Exit_Area2, string Exit_Area3, string Type, string Text)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();

                    string sql1, sql2, tmp;
                    string Work_Date = "";
                    string Command_ID = "";
//                    string Alloc_Ok = "";


                    Command_ID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    Work_Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                    Alloc_Ok = "N";

                    sql1 = ("INSERT INTO tb_work_station_path (WORK_STATION,STOP_AREA_1,STOP_AREA_2,STOP_AREA_3,GOAL_AREA,EXIT_AREA_1,EXIT_AREA_2, EXIT_AREA_3, TYPE, TEXT)");
                    sql2 = ("VALUES ('" + Work_Station + "','" + Stop_Area1 + "','" + Stop_Area2 + "', '" + Stop_Area3 + "', '" + Goal_Area + "','" + Exit_Area1 + "','" + Exit_Area2 + "','" + Exit_Area3 + "','" + Type + "','"+ Text + "'  )");
                    tmp = sql1 + sql2;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_DB_Work_Path" + Convert.ToString(ex));
                return;
            }
        }
        //작업 경로 삭제
        public void Delete_Work_Path(string Work_Station)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();

                    string tmp;

                    string sql = ("DELETE FROM tb_work_station_path WHERE WORK_STATION = '" + Work_Station + "'"); // 테이블 이름

                    tmp = sql;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Delete_Work_Path" + Convert.ToString(ex));
                return;
            }


        }
        //작업 경로 삭제
        public void Delete_Error_Log(string Date)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();

                    string tmp;

                    string sql = ("DELETE FROM tb_error_log WHERE START_DATE_TIME = '" + Date + "'"); // 테이블 이름
                    tmp = sql;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Delete_Error_Log" + Convert.ToString(ex));
                return;
            }
        }

        public void Select_DB_Error_Log()
        {
            try
            {
                string Start_Time = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                string End_Time = DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
                /*
                string Start_Time = DateTime.Now.ToString("yyyy/MM/dd");
                string End_Time = DateTime.Now.ToString("yyyy/MM/dd 23:59:999");
                */
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataTable Grid_Table = new DataTable();
                string sql = "SELECT * FROM tb_error_log WHERE  START_DATE_TIME > curdate()"; 
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);

        
                adp.Fill(Grid_Table);
                Main.Form_Error_Log.label3.Text = Grid_Table.Rows.Count + "개";

                //그리드 출력 및 그리드 초기화
                Main.Form_Error_Log.Grid_Error_Log.DataSource = Grid_Table;
                Main.Form_Error_Log.Init_Oracle_GridView();
                adp.Dispose();
                Grid_Table.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_DB_Error_Log" + Convert.ToString(ex));
                return;
            }


        }
        public void Select_DB_Error_Log(string date)
        {
            try
            {
                string Start_Time = date + " 00:00:00";
                string End_Time = date + " 23:59:59";
                /*
                string Start_Time = DateTime.Now.ToString("yyyy/MM/dd");
                string End_Time = DateTime.Now.ToString("yyyy/MM/dd 23:59:999");
                */
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
                DataTable Grid_Table = new DataTable();
                string sql = "SELECT * FROM tb_error_log WHERE START_DATE_TIME BETWEEN '" + Start_Time + "' AND '" + End_Time + "'";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);


                adp.Fill(Grid_Table);
                Main.Form_Error_Log.label3.Text = Grid_Table.Rows.Count + "개";

                //그리드 출력 및 그리드 초기화
                Main.Form_Error_Log.Grid_Error_Log.DataSource = Grid_Table;
                Main.Form_Error_Log.Init_Oracle_GridView();
                adp.Dispose();
                Grid_Table.Dispose();
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
                {
                    SQL_connect_DB_NoIDX.Close();
                }
            }

            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_DB_Error_Log" + Convert.ToString(ex));
                return;
            }
        }
        //현재 에러 추가
        public void Insert_Current_Error(string Start_Date, string AGV_ID, string Error_Desc, int LOC_ID, string Carrier_ID)
        {

            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();

                    string sql1, sql2, tmp;

                    sql1 = ("INSERT INTO tb_error_log (START_DATE_TIME, CARRIER_ID,LGV_ID,ERROR_DESC,LOC_ID)");
                    sql2 = ("VALUES ('" + Start_Date + "','" + Carrier_ID + "','" + AGV_ID + "','" + Error_Desc + "','" + LOC_ID + "')");
                    tmp = sql1 + sql2;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_Current_Error" + Convert.ToString(ex));
                return;
            }

        }
        public void DELETE_DB_Recovery(int idx)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    int LGV_No = idx + 1;
                    string sql1;//, sql2, tmp;
                    sql1 = ("DELETE FROM tb_recovery_lgv" + LGV_No + "");                         // 테이블 이름
//                   
                    using (MySqlCommand Cmd = new MySqlCommand(sql1, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "DELETE_DB_Recovery" + Convert.ToString(ex));
                return;
            }
        }
        public void Insert_DB_Recovery(int idx, string Command_ID, string GOAL, string WORK)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, sql2, tmp;
                    int LGV_No = idx + 1;

                    sql1 = ("INSERT INTO tb_recovery_lgv" + LGV_No + " (COMMAND_ID,GOAL,WORK)");
                    sql2 = ("VALUES ('" + Command_ID + "','" + GOAL + "','" + WORK + "')");
                    tmp = sql1 + sql2;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_DB_Recovery" + Convert.ToString(ex));
                return;
            }

        }


        public void Select_DB_Recovery(int idx)
        {   

            try
            {
                if (SQL_connect_DB[idx].State == ConnectionState.Closed)
                {
                    SQL_connect_DB[idx].Open();
                }
                int LGV_No = idx + 1;
                string sql = "SELECT COMMAND_ID, GOAL , WORK  FROM tb_recovery_lgv" + LGV_No + "";

                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB[idx]);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Main.m_stAGV[idx].MCS_Vehicle_Command_ID = Convert.ToString(ds.Tables[0].Rows[i][0]);
                        Main.m_stAGV[idx].dqGoal.Add(Convert.ToString(ds.Tables[0].Rows[i][1]));
                        Main.m_stAGV[idx].dqWork.Add(Convert.ToString(ds.Tables[0].Rows[i][2]));
                    }
                    Main.CS_AGV_Logic.LGV_SendData(idx);

                }
                adp.Dispose();
                ds.Dispose();
                if (SQL_connect_DB[idx].State == ConnectionState.Open)
                {
                    SQL_connect_DB[idx].Close();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Select_DB_Recovery" + Convert.ToString(ex));
                return;
            }
        }
        //작업 종류 삭제 (선택)
        public void DELETE_DB_Shutter()
        {
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
            {
                SQL_connect_DB_NoIDX.Open();
            }
            string sql1, tmp;

            //삭제구문
            sql1 = ("DELETE FROM tb_shutter_info");                         // 테이블 이름
            tmp = sql1;
            MySqlCommand SQL_command_DB_Basic = new MySqlCommand(tmp, SQL_connect_DB_NoIDX);
            SQL_command_DB_Basic.ExecuteNonQuery();
            SQL_command_DB_Basic.Dispose();
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
            {
                SQL_connect_DB_NoIDX.Close();
            }
        }
        //작업 종류 삭제 (선택)
        public void DELETE_DB_Work_Type_All()
        {
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
            {
                SQL_connect_DB_NoIDX.Open();
            }
            string sql1, tmp;

            //삭제구문
            sql1 = ("DELETE FROM tb_work_station_path");                         // 테이블 이름
            tmp = sql1;
            MySqlCommand SQL_command_DB_Basic = new MySqlCommand(tmp, SQL_connect_DB_NoIDX);
            SQL_command_DB_Basic.ExecuteNonQuery();
            SQL_command_DB_Basic.Dispose();
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
            {
                SQL_connect_DB_NoIDX.Close();
            }
        }
        
        //차량정보 삭제 
        public void DELETE_DB_AGV_Info()
        {
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
            {
                SQL_connect_DB_NoIDX.Open();
            }
            string sql1, tmp;

            //삭제구문
            sql1 = ("DELETE FROM tb_agv_wirelessinfo");                         // 테이블 이름
            tmp = sql1;
            MySqlCommand SQL_command_DB_Basic = new MySqlCommand(tmp, SQL_connect_DB_NoIDX);
            SQL_command_DB_Basic.ExecuteNonQuery();
            SQL_command_DB_Basic.Dispose();
            if (SQL_connect_DB_NoIDX.State == ConnectionState.Open)
            {
                SQL_connect_DB_NoIDX.Close();
            }
            //Select_AGVList();
        }

        //pgb 관상용 프로그램 190424
        public void Update_LGV_Info_View_LCS(int idx, int x, int y, int t, string State, int Current, int Goal, int Mode, string Error, string Bettary)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info_view SET X  = '" + x + "', Y  = '" + y + "', T  = '" + t + "', STATE  = '" + State + "', Current  = '" + Current + "', GOAL  = '" + Goal + "'," +
                            "MODE  = '" + Mode + "', ERROR  = '" + Error + "', BETTARY = '" + Bettary + "' WHERE VEHICLE_ID = '" + idx + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_LGV_Info_View_LCS" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //pgb 관상용 프로그램 190424
        public void Update_LGV_Info_View_LCS_Connect(int idx, string Connect)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql1, tmp;
                    sql1 = ("UPDATE tb_agv_info_view SET CONNECT = '" + Connect + "' WHERE VEHICLE_ID = '" + idx + "'");  // 테이블 이름
                    tmp = sql1;

                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Update_LGV_Info_View_LCS" + Convert.ToString(ex));
                //WaitForEndRead(idx);
            }
        }

        //DB 에러 로그 삭제. 삭제 기준은 LOG 삭제 기준일과 동일하다. lkw20190610
        public void delete_tb_error_log(DateTime dt)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
                    string sql;

                    sql = ("DELETE FROM tb__error_log WHERE START_DATE_TIME <'" + dt.ToString("yyyy-MM-dd 00:00:00") + "'");

                    using (MySqlCommand Cmd = new MySqlCommand(sql, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    Conn.Close();
                    Conn.Dispose();
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "delete_tb_error_log" + Convert.ToString(ex));
                return;
            }

        }
    }
}
