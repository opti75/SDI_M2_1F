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

namespace SDI_LCS
{
    public partial class Form_Error_Log : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public Form_Error_Log()
        {
            InitializeComponent();
        }
        public Form_Error_Log(Form1 CS_Main)
        {
            InitializeComponent();
            Main = CS_Main;
        }
        public void Init_Oracle_GridView() // 그리드뷰 설정
        {
            gridView1.Columns[0].Caption = "발생 시간";
            gridView1.Columns[1].Caption = "제 품 명";
            gridView1.Columns[2].Caption = "차 량 명";
            gridView1.Columns[3].Caption = "에러 내용";
            gridView1.Columns[4].Caption = "위 치";
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Error_Log_Shown(object sender, EventArgs e)
        {
            Main.CS_Work_DB.Select_DB_Error_Log();
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string DataTime = "";
            DataTime = dateEdit1.Text;
            Main.CS_Work_DB.Select_DB_Error_Log(DataTime);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string Today = dateEdit1.Text;

            if (dateEdit1.Text != "")
            {
                XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
                xlsxOptions.ShowGridLines = true;   // 라인출력 
                Grid_Error_Log.ExportToXlsx("C:\\에러로그\\(" + Today + ")_Error_Log.xlsx", xlsxOptions);
            }
        }
    }
}