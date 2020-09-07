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
    public partial class Form_Insert_LGV : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public int AGV_List = 0;

        public Form_Insert_LGV()
        {
            InitializeComponent();
        }
        public Form_Insert_LGV(Form1 CS_Main)
        {
            InitializeComponent();
            Main = CS_Main;
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int idx = -1;
            string ip = "";
            int port = 0;
            string Type = "";
            if (textBox1.Text != "" && textBox2.Text != "" && comboBox1.Text != "")
            {
                idx = AGV_List;
                ip = textBox1.Text;
                port = Convert.ToInt32(textBox2.Text);
                Type = comboBox1.Text;
                if(idx < 13)
                {
                    Main.CS_Work_DB.Insert_AGV_List(idx, ip, port, Type);
                    Main.CS_Work_DB.Select_AGVList();
                }
                else
                {
                    MessageBox.Show("더이상 추가 할 수 없습니다.");
                }
                
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("차량 정보를 삭제 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int i;
                if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
                DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];

                for (i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
                }
                for (i = 0; i < gridView1.SelectedRowsCount; i++)
                {
                    Main.CS_Work_DB.Delete_AGV(Convert.ToString(rows[i][0]));
                }
                Main.CS_Work_DB.Select_AGVList();

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        { 
            int Name = 0;
            string IP = "";
            int Port = 0;
            string Type = "";
 

            if (gridView1.RowCount > 0)
            {
                Main.CS_Work_DB.DELETE_DB_AGV_Info();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    Name = Convert.ToInt32(gridView1.GetDataRow(i)[0]);
                    IP = Convert.ToString(gridView1.GetDataRow(i)[1]);
                    Port = Convert.ToInt32(gridView1.GetDataRow(i)[2]);
                    Type = Convert.ToString(gridView1.GetDataRow(i)[3]);
                    Thread.Sleep(1);

                    Main.CS_Work_DB.Insert_AGV_List((Name-1), IP, Port, Type);
                }
                Main.CS_Work_DB.Select_AGVList();

            }
            
        }

        private void Form_Insert_LGV_Load(object sender, EventArgs e)
        {

        }
    }
}