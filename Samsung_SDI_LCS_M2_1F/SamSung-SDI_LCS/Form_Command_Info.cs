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
    public partial class Form_Command_Info : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public Form_Command_Info()
        {
            InitializeComponent();
        }
        public Form_Command_Info(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }
        public void Init_Work_GridView() // 그리드뷰 설정
        {
            gridView1.Columns[0].Caption = "작업 번호";
            gridView1.Columns[1].Caption = "작업 이름";
            gridView1.Columns[2].Caption = "공정명";
            gridView1.Columns[3].Caption = "호출 위치";
            gridView1.Columns[4].Caption = "작업 상태";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}