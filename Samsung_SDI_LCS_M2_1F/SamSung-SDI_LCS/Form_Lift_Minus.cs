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
    public partial class Form_Lift_Minus : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public Form_Lift_Minus()
        {
            InitializeComponent();
        }
        public Form_Lift_Minus(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}