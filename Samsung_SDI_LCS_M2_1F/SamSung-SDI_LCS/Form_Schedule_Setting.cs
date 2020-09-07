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
    public partial class Form_Schedule_Setting : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        int Work_Type;
        string Work_Name = "";
        string[] D_Work_Type;

        public Form_Schedule_Setting()
        {
            InitializeComponent();
            D_Work_Type = new string[500];
            for (int i = 0; i < 500; i++)
            {
                D_Work_Type[i] = "";
            }
        }
        public Form_Schedule_Setting(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
            D_Work_Type = new string[500];
            for (int i = 0; i < 500; i++)
            {
                D_Work_Type[i] = "";
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            string Table_Name;
            Table_Name = TB_Table_Name.Text;
            Main.CS_Work_DB.CREATE_DB_Table(Table_Name);
            Main.CS_Work_DB.SELECT_DB_Table();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Work_Name = comboBox1.Text;
            if (Work_Name == "")
            {
                MessageBox.Show("스케줄 이름을 선택 하세요");
            }
            else
            {
                Main.CS_Work_DB.SELETE_DB_Work_Type(Work_Name);

                Grid_Work_Setting.DataSource = Main.CS_Work_DB.DS_Work_Schudule.Tables[0];
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            string Goal;

            if (Work_Name == "")
            {
                MessageBox.Show("스케줄 이름을 선택 하세요");
            }
            else
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    Work_Type = 1;
                }
                else if (comboBox2.SelectedIndex == 1)
                {
                    Work_Type = 2;
                }
                else if (comboBox2.SelectedIndex == 2)
                {
                    Work_Type = 3;
                }

                Goal = textBox2.Text;
                Main.CS_Work_DB.Insert_DB_Work_Type(Work_Name, Goal, Convert.ToString(Work_Type));
                Main.CS_Work_DB.SELETE_DB_Work_Type(Work_Name);
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            int i;
            if (MessageBox.Show("데이터가 손실 됩니다. 삭제 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
                DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];


                for (i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                }


                for (i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    D_Work_Type[i] = Convert.ToString(rows[i][0]);
                    Main.CS_Work_DB.DELETE_DB_Work_Type(Work_Name, D_Work_Type[i]);
                }
                Main.CS_Work_DB.SELETE_DB_Work_Type(Work_Name);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string Goal;
            string Work;
            int Priority;

            if (Work_Name == "")
            {
                MessageBox.Show("스케줄 이름을 선택 하세요");
            }
            else
            {
                if (gridView1.RowCount > 0)
                {
                    Main.CS_Work_DB.DELETE_DB_Work_Type_All(Work_Name);
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        Goal = Convert.ToString(gridView1.GetDataRow(i)[1]);
                        Work = Convert.ToString(gridView1.GetDataRow(i)[2]);
                        Priority = i;
                        Thread.Sleep(1);

                        if (Work == "이동")
                        {
                            Work = "1";
                        }
                        else if (Work == "적재")
                        {
                            Work = "2";
                        }
                        else if (Work == "이재")
                        {
                            Work = "3";
                        }

                        Main.CS_Work_DB.Insert_DB_Work_Type_Save(Work_Name, Goal, Work, Convert.ToString(Priority + 1));
                    }
                    Main.CS_Work_DB.SELETE_DB_Work_Type(Work_Name);
                }
            }
        }
    }
}