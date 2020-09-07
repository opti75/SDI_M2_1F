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
    public partial class Form_Work_Log : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        string[] D_CommandID = new string[500];
        public Form_Work_Log()
        {
            InitializeComponent();
        }
        public Form_Work_Log(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();

            for(int i = 0; i < 500; i ++)
            {
                D_CommandID[i] = "";
            }
        }
        public void Init_Work_GridView() // 그리드뷰 설정
        {
            gridView1.Columns[0].Caption = "콜시간";
            gridView1.Columns[1].Caption = "작 업 명";
            gridView1.Columns[2].Caption = "제 품 명";
            gridView1.Columns[3].Caption = "출 발 지";
            gridView1.Columns[4].Caption = "도 착 지";
            gridView1.Columns[5].Caption = "차량 번호";
            gridView1.Columns[6].Caption = "할당 시간";
            gridView1.Columns[7].Caption = "대기-출발";
            gridView1.Columns[8].Caption = "적재-완료";
            gridView1.Columns[9].Caption = "완료-출발";
            gridView1.Columns[10].Caption = "이재-완료";
            gridView1.Columns[11].Caption = "종료 시간";
            gridView1.Columns[12].Caption = "작업 상태";

           
            gridView1.Columns[13].Caption = "이동시간-적재";
            gridView1.Columns[14].Caption = "작업시간-적재";
            gridView1.Columns[15].Caption = "이동시간-이재";
            gridView1.Columns[16].Caption = "작업시간-이재";
            gridView1.Columns[17].Caption = "최종작업시간";

        }

        private void Form_Work_Log_Shown(object sender, EventArgs e)
        {
            Main.CS_Work_DB.Select_MCS_Command_Info_Log_View();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
   
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
             
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string DataTime = "";
            DataTime = dateEdit1.Text;
            if(DataTime != "")
            {
                XlsxExportOptionsEx xlsxOptions = new XlsxExportOptionsEx();
                xlsxOptions.ShowGridLines = true;   // 라인출력 
                Grid_Work_Log.ExportToXlsx("C:\\작업로그\\(" + DataTime + ")_Work_Log.xlsx", xlsxOptions);
            }
            
             
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            

        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
           
        }

        private void simpleButton2_Click_2(object sender, EventArgs e)
        {
            string DataTime = "";
            DataTime = dateEdit1.Text;
            if(DataTime != "")
            { 
                Main.CS_Work_DB.Select_MCS_Command_Info_Log_View(DataTime);
            }
           
        }
    }
}