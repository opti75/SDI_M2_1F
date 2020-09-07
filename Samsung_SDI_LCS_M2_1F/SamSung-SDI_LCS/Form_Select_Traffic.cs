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
    public partial class Form_Select_Traffic : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public Form_Select_Traffic()
        {
            InitializeComponent();
        }
        public Form_Select_Traffic(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
            Main.Form_Traffic_Cell.ShowDialog();
            
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
            Main.Form_Traffic_Basic.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Close();
            Main.Form_Traffic_Dest.ShowDialog();
        }
    }
}