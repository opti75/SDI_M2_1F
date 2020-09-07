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
using DevExpress.XtraPrinting;
using System.Data.OleDb;
using System.Collections;

namespace SDI_LCS
{
    public partial class Form_Traffic_Dest : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        DataTable Grid_Table;
        public ArrayList Traffic_Node;

        public Form_Traffic_Dest()
        {
            InitializeComponent();
        }
        public Form_Traffic_Dest(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();

            Traffic_Node = new ArrayList();
            Grid_Table = new DataTable();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string str_tmpFilterString;

            str_tmpFilterString = gridView1.ActiveFilterString;
            gridView1.ActiveFilterString = "";

            XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
            xlsxOptions.ShowGridLines = true;   // 라인출력 
            Grid_Traffic_Setting.ExportToXlsx("C:\\SDI_DATA\\Traffic_Dest.xlsx", xlsxOptions);
            Select_Traffic_Dest();

            gridView1.ActiveFilterString = str_tmpFilterString;
        }


        public void Select_Traffic_Dest()
        {
            //            string test;
            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\\SDI_DATA\\Traffic_Dest.xlsx;Extended Properties=""Excel 12.0;HDR=YES;""";
            OleDbConnection excelConnection = new OleDbConnection(conStr);
            excelConnection.Open();

            Grid_Table = new DataTable();
            string strSQL = "SELECT * FROM [Sheet$] ORDER BY 도착지_I";
            OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);


            dataAdapter.Fill(Grid_Table);
            Grid_Traffic_Setting.DataSource = Grid_Table;
            Traffic_Node.Clear();
            for (int i = 0; i < Grid_Table.Rows.Count; i++)
            {
                Main.m_stTraffic_Dest[i].idx_Current_Use = Convert.ToString(Grid_Table.Rows[i][0]);
                Main.m_stTraffic_Dest[i].idx_Start_Current = Convert.ToString(Grid_Table.Rows[i][1]);
                Main.m_stTraffic_Dest[i].idx_End_Current = Convert.ToString(Grid_Table.Rows[i][2]);

                Main.m_stTraffic_Dest[i].idx_Dest_Use = Convert.ToString(Grid_Table.Rows[i][3]);
                Main.m_stTraffic_Dest[i].idx_Dest = Convert.ToString(Grid_Table.Rows[i][4]);
                 
                Main.m_stTraffic_Dest[i].i_Current_Use = Convert.ToString(Grid_Table.Rows[i][5]);
                Main.m_stTraffic_Dest[i].i_Start_Current = Convert.ToString(Grid_Table.Rows[i][6]);
                Main.m_stTraffic_Dest[i].i_End_Current = Convert.ToString(Grid_Table.Rows[i][7]);

                Main.m_stTraffic_Dest[i].i_Goal_Use = Convert.ToString(Grid_Table.Rows[i][8]);
                Main.m_stTraffic_Dest[i].i_Start_Goal = Convert.ToString(Grid_Table.Rows[i][9]);
                Main.m_stTraffic_Dest[i].i_End_Goal = Convert.ToString(Grid_Table.Rows[i][10]);

                Traffic_Node.Add(Main.m_stTraffic_Dest[i]);
            }


            // dispose used objects
            Grid_Table.Dispose();
            dataAdapter.Dispose();
            dbCommand.Dispose();

            excelConnection.Close();
            excelConnection.Dispose();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Insert_Excel_Data();
            Select_Traffic_Dest();
        }

        public void Insert_Excel_Data()
        {
            string idx_Start_Current;
            string idx_End_Current;
            string idx_Dest;

            string i_Start_Current;
            string i_End_Current;
            string i_Start_Goal;
            string i_End_Goal;


            idx_Start_Current = TB_idx_S_Current.Text;
            idx_End_Current = TB_idx_E_Current.Text;
            idx_Dest = TB_idx_Dest.Text;

            i_Start_Current = TB_i_S_Current.Text;
            i_End_Current = TB_i_E_Current.Text;
            i_Start_Goal = TB_i_S_Goal.Text;
            i_End_Goal = TB_i_E_Goal.Text;


            //            string test;
            OleDbCommand dbCommand;
            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\\SDI_DATA\\Traffic_Dest.xlsx;Extended Properties=""Excel 12.0;HDR=YES;""";
            OleDbConnection excelConnection = new OleDbConnection(conStr);
            excelConnection.Open();


            dbCommand = new OleDbCommand("INSERT INTO [Sheet$](사용여부_C_I,시작위치_I,종료위치_I,사용여부_G_I,도착지_I,사용여부_C_U,시작위치_U,종료위치_U,사용여부_G_U,시작목적지_U,종료목적지_U) VALUES ("
              + "'사용'," + idx_Start_Current + "," + idx_End_Current + "," + "'사용'," + idx_Dest + "," + "'사용'," + i_Start_Current + "," + i_End_Current + "," + "'미사용'," + i_Start_Goal + "," + i_End_Goal + ")", excelConnection);
            dbCommand.ExecuteNonQuery();

            excelConnection.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int i = 0;

            if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
            DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];

            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
            }

            gridView1.BeginSort();

            try
            {
                foreach (DataRow row in rows)
                    row.Delete();
            }
            finally
            {
                gridView1.EndSort();
            }
            Grid_Traffic_Setting.ExportToXlsx("C:\\SDI_DATA\\Traffic_Dest.xlsx");
            Select_Traffic_Dest();
        }

        private void Form_Traffic_Dest_Shown(object sender, EventArgs e)
        {
            Select_Traffic_Dest();
        }
    }
}