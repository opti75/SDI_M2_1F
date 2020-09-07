namespace SDI_LCS
{
    partial class Form_Lift_Plus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Lift_Plus));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_Up = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Down = new DevExpress.XtraEditors.SimpleButton();
            this.lb_End_3 = new System.Windows.Forms.Label();
            this.lb_End_1 = new System.Windows.Forms.Label();
            this.P_Carrier_3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.P_Carrier_2 = new System.Windows.Forms.Panel();
            this.P_Carrier_1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 1;
            this.ribbon.Name = "ribbon";
            this.ribbon.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowCategoryInCaption = false;
            this.ribbon.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
            this.ribbon.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbon.ShowQatLocationSelector = false;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(366, 30);
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(284, 40);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(79, 37);
            this.simpleButton3.TabIndex = 19;
            this.simpleButton3.Text = "닫 기";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label3.Location = new System.Drawing.Point(4, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 30);
            this.label3.TabIndex = 18;
            this.label3.Text = "양극 리프트";
            // 
            // Btn_Up
            // 
            this.Btn_Up.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Up.Appearance.Options.UseBackColor = true;
            this.Btn_Up.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.Btn_Up.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Up.ImageOptions.Image")));
            this.Btn_Up.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.Btn_Up.Location = new System.Drawing.Point(100, 220);
            this.Btn_Up.Name = "Btn_Up";
            this.Btn_Up.Size = new System.Drawing.Size(37, 29);
            this.Btn_Up.TabIndex = 17;
            this.Btn_Up.Visible = false;
            // 
            // Btn_Down
            // 
            this.Btn_Down.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Btn_Down.Appearance.Options.UseBackColor = true;
            this.Btn_Down.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.Btn_Down.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Btn_Down.ImageOptions.Image")));
            this.Btn_Down.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.Btn_Down.Location = new System.Drawing.Point(230, 220);
            this.Btn_Down.Name = "Btn_Down";
            this.Btn_Down.Size = new System.Drawing.Size(37, 29);
            this.Btn_Down.TabIndex = 16;
            this.Btn_Down.Visible = false;
            // 
            // lb_End_3
            // 
            this.lb_End_3.AutoSize = true;
            this.lb_End_3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_End_3.ForeColor = System.Drawing.Color.Yellow;
            this.lb_End_3.Location = new System.Drawing.Point(317, 94);
            this.lb_End_3.Name = "lb_End_3";
            this.lb_End_3.Size = new System.Drawing.Size(46, 30);
            this.lb_End_3.TabIndex = 15;
            this.lb_End_3.Text = "3층";
            // 
            // lb_End_1
            // 
            this.lb_End_1.AutoSize = true;
            this.lb_End_1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_End_1.ForeColor = System.Drawing.Color.Yellow;
            this.lb_End_1.Location = new System.Drawing.Point(4, 332);
            this.lb_End_1.Name = "lb_End_1";
            this.lb_End_1.Size = new System.Drawing.Size(46, 30);
            this.lb_End_1.TabIndex = 14;
            this.lb_End_1.Text = "1층";
            // 
            // P_Carrier_3
            // 
            this.P_Carrier_3.BackColor = System.Drawing.Color.White;
            this.P_Carrier_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P_Carrier_3.Location = new System.Drawing.Point(230, 92);
            this.P_Carrier_3.Name = "P_Carrier_3";
            this.P_Carrier_3.Size = new System.Drawing.Size(81, 39);
            this.P_Carrier_3.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.P_Carrier_2);
            this.panel2.Location = new System.Drawing.Point(143, 92);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(81, 276);
            this.panel2.TabIndex = 12;
            // 
            // P_Carrier_2
            // 
            this.P_Carrier_2.BackColor = System.Drawing.Color.White;
            this.P_Carrier_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P_Carrier_2.Location = new System.Drawing.Point(0, 119);
            this.P_Carrier_2.Name = "P_Carrier_2";
            this.P_Carrier_2.Size = new System.Drawing.Size(81, 39);
            this.P_Carrier_2.TabIndex = 5;
            // 
            // P_Carrier_1
            // 
            this.P_Carrier_1.BackColor = System.Drawing.Color.White;
            this.P_Carrier_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P_Carrier_1.Location = new System.Drawing.Point(56, 329);
            this.P_Carrier_1.Name = "P_Carrier_1";
            this.P_Carrier_1.Size = new System.Drawing.Size(81, 39);
            this.P_Carrier_1.TabIndex = 11;
            // 
            // Form_Lift_Plus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 390);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Btn_Up);
            this.Controls.Add(this.Btn_Down);
            this.Controls.Add(this.lb_End_3);
            this.Controls.Add(this.lb_End_1);
            this.Controls.Add(this.P_Carrier_3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.P_Carrier_1);
            this.Controls.Add(this.ribbon);
            this.Name = "Form_Lift_Plus";
            this.Ribbon = this.ribbon;
            this.Text = "Form_Lift_Plus";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.Label label3;
        public DevExpress.XtraEditors.SimpleButton Btn_Up;
        public DevExpress.XtraEditors.SimpleButton Btn_Down;
        public System.Windows.Forms.Label lb_End_3;
        public System.Windows.Forms.Label lb_End_1;
        public System.Windows.Forms.Panel P_Carrier_3;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel P_Carrier_1;
        public System.Windows.Forms.Panel P_Carrier_2;
    }
}