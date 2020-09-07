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
using DevExpress.XtraPrinting;

namespace SDI_LCS
{
    public partial class Form_Work_Path_Setting : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;

        public Form_Work_Path_Setting()
        {
            InitializeComponent();
       
        }
        public Form_Work_Path_Setting(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        public void Init_Work_GridView() // 그리드뷰 설정
        {
            gridView1.Columns[0].Caption = "작 업 지";
            gridView1.Columns[1].Caption = "1 - 경유지";
            gridView1.Columns[2].Caption = "2 - 경유지";
            gridView1.Columns[3].Caption = "3 - 경유지";
            gridView1.Columns[4].Caption = "목 적 지";
            gridView1.Columns[5].Caption = "1 - 탈출지";
            gridView1.Columns[6].Caption = "2 - 탈출지";
            gridView1.Columns[7].Caption = "3 - 탈출지";
            gridView1.Columns[8].Caption = "종 류";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            string Work_Station = "";
            string Stop_Area_1 = "";
            string Stop_Area_2 = "";
            string Stop_Area_3 = "";
            string Goal_Area = "";
            string Exit_Area_1 = "";
            string Exit_Area_2 = "";
            string Exit_Area_3 = "";
            string Type = "";
            string Text = "없음";

            Work_Station = TB_Work_Station.Text;
            Stop_Area_1 = TB_Stop_Area1.Text;
            Stop_Area_2 = TB_Stop_Area2.Text;
            Stop_Area_3 = TB_Stop_Area3.Text;
            Goal_Area = TB_Goal_Area.Text;
            Exit_Area_1 = TB_Exit_Area1.Text;
            Exit_Area_2 = TB_Exit_Area2.Text;
            Exit_Area_3 = TB_Exit_Area3.Text;
            Type = comboBox1.Text;

            if (Work_Station != "" && Stop_Area_1 != "" && Stop_Area_2 != "" && Stop_Area_2 != "" && Goal_Area != "" && Exit_Area_1 != "" && Exit_Area_2 != "" && Exit_Area_3 != "" && Type != "")
            {
                Main.CS_Work_DB.Insert_DB_Work_Path(Work_Station, Stop_Area_1, Stop_Area_2, Stop_Area_3,
                                                    Goal_Area, Exit_Area_1, Exit_Area_2, Exit_Area_3, Type, Text);
                Main.CS_Work_DB.Select_Work_Path();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
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
                Main.CS_Work_DB.Delete_Work_Path(Convert.ToString(rows[i][0]));
            }
            Main.CS_Work_DB.Select_Work_Path();
        }

        private void simpleButton7_Click_1(object sender, EventArgs e)
        {
            XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
            xlsxOptions.ShowGridLines = true;   // 라인출력 
            Grid_Work_Path_Setting.ExportToXlsx("C:\\SDI_DATA\\Work_Path_Setting.xlsx", xlsxOptions);
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string Work_Station = "";
            string Stop_Area_1 = "";
            string Stop_Area_2 = "";
            string Stop_Area_3 = "";
            string Goal_Area = "";
            string Exit_Area_1 = "";
            string Exit_Area_2 = "";
            string Exit_Area_3 = "";
            string Text = "";
            string Type = "";

            if (gridView1.RowCount > 0)
            {
                Main.CS_Work_DB.DELETE_DB_Work_Type_All();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    Work_Station = Convert.ToString(gridView1.GetDataRow(i)[0]);
                    Stop_Area_1 = Convert.ToString(gridView1.GetDataRow(i)[1]);
                    Stop_Area_2 = Convert.ToString(gridView1.GetDataRow(i)[2]);
                    Stop_Area_3 = Convert.ToString(gridView1.GetDataRow(i)[3]);
                    Goal_Area = Convert.ToString(gridView1.GetDataRow(i)[4]);
                    Exit_Area_1 = Convert.ToString(gridView1.GetDataRow(i)[5]);
                    Exit_Area_2 = Convert.ToString(gridView1.GetDataRow(i)[6]);
                    Exit_Area_3 = Convert.ToString(gridView1.GetDataRow(i)[7]);
                    Type = Convert.ToString(gridView1.GetDataRow(i)[8]);
                    Text = Convert.ToString(gridView1.GetDataRow(i)[9]);
                    Thread.Sleep(1);

                    Main.CS_Work_DB.Insert_DB_Work_Path(Work_Station, Stop_Area_1, Stop_Area_2, Stop_Area_3,
                                                    Goal_Area, Exit_Area_1, Exit_Area_2, Exit_Area_3, Type, Text);
                }
                Main.CS_Work_DB.Select_Work_Path();

                
            }
        }
    }
}