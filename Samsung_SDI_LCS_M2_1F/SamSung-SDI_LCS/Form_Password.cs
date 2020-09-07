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
    public partial class Form_Password : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
        public Form_Password()
        {
            InitializeComponent();
        }
        public Form_Password(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (tb_password.Text == Form1.log_on_password)
                Main.FLAG_LOG_ON_PASSWORD = 1;
            else
                MessageBox.Show("입력하신 암호가 아닙니다.\n 관리자에게 문의하세요");

            Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Password_Shown(object sender, EventArgs e)
        {
            tb_password.Clear();
            tb_password.Focus();
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_ok.PerformClick();  
        }

        private void Form_Password_Load(object sender, EventArgs e)
        {

        }
    }
}