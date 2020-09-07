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
using System.Threading;

namespace SDI_LCS
{
    public partial class Form_Manual_Command : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;

        string realTime = "";

        public Form_Manual_Command()
        {
            InitializeComponent();
        }
        public Form_Manual_Command(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int Goal = 0;
            int WorkType = 0;

            int LGV_No = -1;
            Goal = Convert.ToInt32(textBox1.Text);

            for (int i = 0; i < 34; i++)
            {
                if (comboBox2.SelectedIndex == i)
                {
                    LGV_No = i;
                }
            }

            if (comboBox1.SelectedIndex == 0)
            {
                WorkType = 1;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                WorkType = 2;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                WorkType = 3;
            }

            if (LGV_No != -1 && Goal != 0 && WorkType != 0)
            {
                Main.m_stAGV[LGV_No].Working = 1;
                Main.m_stAGV[LGV_No].dqGoal.Add(Goal);
                Main.m_stAGV[LGV_No].dqWork.Add(WorkType);
                Main.CS_AGV_Logic.LGV_SendData(LGV_No);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            string Command_ID = "";
            string Carrier_ID = "";
            string Source = "";
            string Dest = "";
            string Process_ID = "";
            string Batch_ID = "";
            string Lot_ID = "";
            string Proiority = "";
            string Quantity = "";

            string Call_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            Command_ID = "E"+TB_C_ID.Text+realTime;
            Carrier_ID = TB_Carrier_ID.Text;
     
            if (CB_Source_Port.Text != "CCAAGV01_V001" && CB_Source_Port.Text != "CCAAGV01_V002" 
             && CB_Source_Port.Text != "CCAAGV01_V003" && CB_Source_Port.Text != "CCAAGV01_V004"
             && CB_Source_Port.Text != "CCAAGV01_V005" && CB_Source_Port.Text != "CCAAGV01_V006"
             && CB_Source_Port.Text != "CCAAGV01_V007" && CB_Source_Port.Text != "CCAAGV01_V008"
             && CB_Source_Port.Text != "CCAAGV01_V009" && CB_Source_Port.Text != "CCAAGV01_V010" && CB_Source_Port.Text != "CCAAGV01_V011"
             && CB_Source_Port.Text != "CCAAGV01_V012" && CB_Source_Port.Text != "CCAAGV01_V013")
            {
                int t_SNum = CB_Source_Port.Text.IndexOf('('); ;
                Source = CB_Source_Port.Text;
                Source = Source.Remove(t_SNum, CB_Source_Port.Text.Length - t_SNum);
            }
            else
            {
                Source = CB_Source_Port.Text;
            }

            int t_DNum = CB_Dest_Port.Text.IndexOf('('); ;
            Dest = CB_Dest_Port.Text;
            Dest = Dest.Remove(t_DNum, CB_Dest_Port.Text.Length - t_DNum);



            //Source = CB_Source_Port.Text;
            //Dest = CB_Dest_Port.Text;
            Process_ID = TB_Process_ID.Text;
            Batch_ID = TB_Batch_ID.Text;
            Lot_ID = TB_Lot_ID.Text;
            Proiority = TB_Proiority.Text;
            Quantity = TB_Quantity.Text;

            string[] M_S_CarrID = new string[10];

            for (int i = 0; i < 10; i++)
            {
                M_S_CarrID[i] = "0";
            }

            Main.CS_Work_DB.Insert_DB_MCS_Work(Carrier_ID, M_S_CarrID, Command_ID,
                                                      Source, Dest, Process_ID, Batch_ID, Lot_ID,
                                                      Convert.ToUInt16(Proiority), Convert.ToUInt16(Quantity), Call_Time);

            Main.CS_Work_DB.Select_MCS_Command_Info(0);
            Main.CS_Work_DB.Select_MCS_Command_Info_Main(0);
            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 208, 0, 0, Command_ID);

            Main.Form_MCS.Command_Type = "TRANSFER";
            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 501, 0, 0, Command_ID);

            label5.Text = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Manual_Command_Shown(object sender, EventArgs e)
        {
            
            Main.CS_Work_DB.Select_Work_Path();
            realTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            TB_C_ID.Text = TB_Carrier_ID.Text;
            label5.Text = realTime;
        }

        private void TB_C_ID_TextChanged(object sender, EventArgs e)
        {
            TB_Carrier_ID.Text = TB_C_ID.Text;
        }

        private void TB_Carrier_ID_TextChanged(object sender, EventArgs e)
        {
            TB_C_ID.Text = TB_Carrier_ID.Text;
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            int Goal = 0;
            int WorkType = 0;


            Goal = Convert.ToInt32(textBox1.Text);
            int LGV_No = -1;
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (comboBox2.SelectedIndex == i)
                {
                    LGV_No = i;
                }
            }

            if (comboBox1.SelectedIndex == 0)
            {
                WorkType = 1;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                WorkType = 2;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                WorkType = 3;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                WorkType = 4;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                WorkType = 5;
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                WorkType = 6;
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                WorkType = 7;
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                WorkType = 8;
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                WorkType = 9;
            }


            if (LGV_No != -1 && Goal != 0 && WorkType != 0)
            {
                Main.m_stAGV[LGV_No].Working = 1;
                Main.m_stAGV[LGV_No].dqGoal.Add(Goal);
                Main.m_stAGV[LGV_No].dqWork.Add(WorkType);
                Main.CS_AGV_Logic.LGV_SendData(LGV_No);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}