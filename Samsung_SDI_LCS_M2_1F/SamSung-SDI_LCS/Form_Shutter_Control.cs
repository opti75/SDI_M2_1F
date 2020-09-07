using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using MySql.Data.MySqlClient;
using System.Threading;

namespace SDI_LCS
{
    public partial class Form_Shutter_Control : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public int Shutter_Control_Count = 0;
        public int Shutter_Num = 0;
        public MySqlConnection SQL_connect_DB_NoIDX = new MySqlConnection();
        string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";
        public Form_Shutter_Control()
        {
            InitializeComponent();
        }

        public Form_Shutter_Control(Form1 CS_Main)
        {
            Main = CS_Main;

            InitializeComponent();

            SQL_connect_DB_NoIDX = new MySqlConnection(Mariadb);
        }
        public void Insert_Shutter_Port(int Shutter_No, string IP, int Port,int S_Station_Input, int E_Station_Input, int S_Goal_Input
            , int E_Goal_Input, int S_Station_Output, int E_Station_Output, int S_Goal_Output, int E_Goal_Output, string Machine_Name,string Name)
        {
            try
            {
                string Mariadb = "SERVER=localhost; DATABASE=samsung_sdi_db; UID=root; PASSWORD=rootmydb;";

                using (MySqlConnection Conn = new MySqlConnection(Mariadb))
                {
                    Conn.Open();
//                    string sql1, sql2, tmp;
                    string tmp;

                    string sql = "INSERT INTO tb_shutter_info(SHUTTER_NO,IP,PORT,START_STATION_INPUT,END_STATION_INPUT,START_GOAL_INPUT,END_GOAL_INPUT,START_STATION_OUTPUT,END_STATION_OUTPUT,START_GOAL_OUTPUT,END_GOAL_OUTPUT,MACHINE_NAME,NAME) " +
                        "VALUES(" + (Shutter_No + 1) + ",'" + IP + "'," + Port + ",'" + S_Station_Input + "','" + E_Station_Input + "','" + S_Goal_Input + "','" + E_Goal_Input + "','" + S_Station_Output + "','" + E_Station_Output + "','" + S_Goal_Output + "','" + E_Goal_Output + "','" + Machine_Name + "','" + Name + "')";

                    tmp = sql;
                    using (MySqlCommand Cmd = new MySqlCommand(tmp, Conn))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (MySqlException ex)
            {
                Main.Log("DB_ERROR", "Insert_AGV_List" + Convert.ToString(ex));
                return;
            }
        }
        //셔터 정보 검색 - 통신
        public void Select_ShutterList()
        {
            try
            {
                if (SQL_connect_DB_NoIDX.State == ConnectionState.Closed)
                {
                    SQL_connect_DB_NoIDX.Open();
                }
//                string sql1, sql2, tmp;

                string sql = "SELECT * FROM tb_shutter_info ORDER BY SHUTTER_NO";
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, SQL_connect_DB_NoIDX);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //추가한 만큼 차량 번호 매겨주기
                    Shutter_Control_Count = ds.Tables[0].Rows.Count;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Main.CS_Shutter_C_Info[i].IP = Convert.ToString(ds.Tables[0].Rows[i][1]);
                        Main.CS_Shutter_C_Info[i].Port = Convert.ToInt32(ds.Tables[0].Rows[i][2]);

                        Main.CS_Shutter_C_Info[i].Start_Station_InPut = Convert.ToInt32(ds.Tables[0].Rows[i][3]);
                        Main.CS_Shutter_C_Info[i].End_Station_InPut = Convert.ToInt32(ds.Tables[0].Rows[i][4]);
                        Main.CS_Shutter_C_Info[i].Start_Goal_InPut = Convert.ToInt32(ds.Tables[0].Rows[i][5]);
                        Main.CS_Shutter_C_Info[i].End_Goal_InPut = Convert.ToInt32(ds.Tables[0].Rows[i][6]);

                        Main.CS_Shutter_C_Info[i].Start_Station_OutPut = Convert.ToInt32(ds.Tables[0].Rows[i][7]);
                        Main.CS_Shutter_C_Info[i].End_Station_OutPut = Convert.ToInt32(ds.Tables[0].Rows[i][8]);
                        Main.CS_Shutter_C_Info[i].Start_Goal_OutPut = Convert.ToInt32(ds.Tables[0].Rows[i][9]);
                        Main.CS_Shutter_C_Info[i].End_Goal_OutPut = Convert.ToInt32(ds.Tables[0].Rows[i][10]);

                        Main.CS_Shutter_C_Info[i].Machine_Name = Convert.ToString(ds.Tables[0].Rows[i][11]);

                        Main.CS_Shutter_C_Info[i].Name = Convert.ToString(ds.Tables[0].Rows[i][12]);

                    }
                }

                Grid_Shutter_list.DataSource = ds.Tables[0];

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


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int idx = -1;
            string ip = "";
            int port = 0;


            int Start_Station_Input = 0;
            int End_Station_Input = 0;
            int Start_Goal_Input = 0;
            int End_Goal_Input = 0;

            int Start_Station_Output = 0;
            int End_Station_Output = 0;
            int Start_Goal_Output = 0;
            int End_Goal_Output = 0;
            string Machine_Name = "";
            string Name = "";

            if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox11.Text != ""
              & textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && textBox10.Text != "" && comboBox1.Text != "")
            {
                idx = Shutter_Control_Count;
                ip = textBox1.Text;
                port = Convert.ToInt32(textBox2.Text);

                Start_Station_Input = Convert.ToInt32(textBox3.Text);
                End_Station_Input = Convert.ToInt32(textBox4.Text);
                Start_Goal_Input = Convert.ToInt32(textBox5.Text);
                End_Goal_Input = Convert.ToInt32(textBox6.Text);

                Start_Station_Output = Convert.ToInt32(textBox7.Text);
                End_Station_Output = Convert.ToInt32(textBox8.Text);
                Start_Goal_Output = Convert.ToInt32(textBox9.Text);
                End_Goal_Output = Convert.ToInt32(textBox10.Text);

                Machine_Name = Convert.ToString(comboBox1.Text);

                Name = Convert.ToString(textBox11.Text);

                if (idx < Form1.SHUTTER_NUM)
                {
                    Insert_Shutter_Port(idx, ip, port, Start_Station_Input, End_Station_Input, Start_Goal_Input, End_Goal_Input, Start_Station_Output, End_Station_Output, Start_Goal_Output, End_Goal_Output, Machine_Name, Name);
                    Select_ShutterList();
                }
                else
                {
                    MessageBox.Show("더이상 추가 할 수 없습니다.");
                }

            }
        }

        private void Form_Shutter_Control_Shown(object sender, EventArgs e)
        {
            Select_ShutterList();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int idx = -1;
            string ip = "";
            int port = 0;


            int Start_Station_Input = 0;
            int End_Station_Input = 0;
            int Start_Goal_Input = 0;
            int End_Goal_Input = 0;

            int Start_Station_Output = 0;
            int End_Station_Output = 0;
            int Start_Goal_Output = 0;
            int End_Goal_Output = 0;
            string Machine_Name = "";
            string Name = "";

            if (gridView1.RowCount > 0)
            {
                Main.CS_Work_DB.DELETE_DB_Shutter();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    idx = (i+1);
                    ip = Convert.ToString(gridView1.GetDataRow(i)[1]);
                    port = Convert.ToInt32(gridView1.GetDataRow(i)[2]);

                    Start_Station_Input = Convert.ToInt32(gridView1.GetDataRow(i)[3]);
                    End_Station_Input = Convert.ToInt32(gridView1.GetDataRow(i)[4]);
                    Start_Goal_Input = Convert.ToInt32(gridView1.GetDataRow(i)[5]);
                    End_Goal_Input = Convert.ToInt32(gridView1.GetDataRow(i)[6]);

                    Start_Station_Output = Convert.ToInt32(gridView1.GetDataRow(i)[7]);
                    End_Station_Output = Convert.ToInt32(gridView1.GetDataRow(i)[8]);
                    Start_Goal_Output = Convert.ToInt32(gridView1.GetDataRow(i)[9]);
                    End_Goal_Output = Convert.ToInt32(gridView1.GetDataRow(i)[10]);


                    Machine_Name = Convert.ToString(gridView1.GetDataRow(i)[11]);
                    Name = Convert.ToString(gridView1.GetDataRow(i)[12]);
                    Thread.Sleep(1);

                    Insert_Shutter_Port(idx, ip, port, Start_Station_Input, End_Station_Input, Start_Goal_Input, End_Goal_Input, Start_Station_Output, End_Station_Output, Start_Goal_Output, End_Goal_Output, Machine_Name, Name);
                }
                Select_ShutterList();
            }
        }
    }
}