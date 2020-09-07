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

namespace SDI_LCS
{
    public partial class Form_Setting_Charge : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public int Charge_Value = 2514;
        public int Charge_Count = 900;
        public int Charge_Value_ApplyLogic = 2628;
        public Form_Setting_Charge()
        {
            InitializeComponent();
        }
        public Form_Setting_Charge(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int Hour = Convert.ToInt32(TB_Hour.Text);
            int Min = Convert.ToInt32(TB_Min.Text);
            int Sec = Convert.ToInt32(TB_Sec.Text);


            string LGV_No = "";
            string Setting_Charge_Value = "";
            string Setting_Middle_Value = "";
            string Setting_Job_Count = "";

            if (gridView1.RowCount > 0)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    LGV_No = Convert.ToString(gridView1.GetDataRow(i)[0]);
                    Setting_Charge_Value = Convert.ToString(gridView1.GetDataRow(i)[1]);
                    Setting_Middle_Value = Convert.ToString(gridView1.GetDataRow(i)[2]);
                    Setting_Job_Count = Convert.ToString(gridView1.GetDataRow(i)[3]);

                    Main.CS_Work_DB.Update_AGV_Info_Setting_Value(LGV_No, Setting_Charge_Value, Setting_Middle_Value, Setting_Job_Count);
                }
                Main.CS_Work_DB.Select_MCS_AGV_Setting_Battery();
            }


            Charge_Count = (Hour * 3600) + (Min * 60) + Sec;

           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            /*
            double Convert_Volt = 0;
            double Battery = 0;


            if (textBox1.Text != "" && Convert.ToInt32(textBox1.Text) <= 100)
            {
                Battery = Convert.ToInt32(textBox1.Text);
                Convert_Volt = Math.Truncate((Battery / 100) * 380 + 2400);
                label8.Text = Convert.ToString(Convert_Volt / 100);
            }
            else if(textBox1.Text != "" && Convert.ToInt32(textBox1.Text) > 100)
            {
                textBox1.Text = "";
                MessageBox.Show("100% 이상을 입력 할 수 없습니다.");
            }
            */
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Setting_Charge_Shown(object sender, EventArgs e)
        {
            Main.CS_Work_DB.Select_MCS_AGV_Setting_Battery();
        }
    }
}