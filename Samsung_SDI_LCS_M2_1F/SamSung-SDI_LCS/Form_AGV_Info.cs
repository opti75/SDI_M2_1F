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
    public partial class Form_AGV_Info : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Form1 Main;
   
        public Form_AGV_Info()
        {
            InitializeComponent();
           
        }
        public Form_AGV_Info(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_AGV_Info_Shown(object sender, EventArgs e)
        {
            Main.CS_Work_DB.Select_MCS_AGV_Info();


        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            int i;

            if (gridView2 == null || gridView2.SelectedRowsCount == 0) return;
            DataRow[] rows = new DataRow[gridView2.SelectedRowsCount];
            for (i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                rows[i] = gridView2.GetDataRow(gridView2.GetSelectedRows()[i]);
            }

            for (i = 0; i < gridView2.SelectedRowsCount; i++)
            {
                if (Convert.ToString(rows[i][0]) == "CCAAGV01_V001")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(0), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[0].MCS_Carrier_ID = "";
                    Main.m_stAGV[0].MCS_Carrier_Type = "";
                    Main.m_stAGV[0].MCS_Install_Time = "";
                    Main.m_stAGV[0].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[0] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V002")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(1), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[1].MCS_Carrier_ID = "";
                    Main.m_stAGV[1].MCS_Carrier_Type = "";
                    Main.m_stAGV[1].MCS_Install_Time = "";
                    Main.m_stAGV[1].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[1] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V003")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(2), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[2].MCS_Carrier_ID = "";
                    Main.m_stAGV[2].MCS_Carrier_Type = "";
                    Main.m_stAGV[2].MCS_Install_Time = "";
                    Main.m_stAGV[2].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[2] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V004")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(3), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[3].MCS_Carrier_ID = "";
                    Main.m_stAGV[3].MCS_Carrier_Type = "";
                    Main.m_stAGV[3].MCS_Install_Time = "";
                    Main.m_stAGV[3].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[3] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V005")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(4), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[4].MCS_Carrier_ID = "";
                    Main.m_stAGV[4].MCS_Carrier_Type = "";
                    Main.m_stAGV[4].MCS_Install_Time = "";
                    Main.m_stAGV[4].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[4] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V006")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(5), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[5].MCS_Carrier_ID = "";
                    Main.m_stAGV[5].MCS_Carrier_Type = "";
                    Main.m_stAGV[5].MCS_Install_Time = "";
                    Main.m_stAGV[5].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[5] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V007")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(6), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[6].MCS_Carrier_ID = "";
                    Main.m_stAGV[6].MCS_Carrier_Type = "";
                    Main.m_stAGV[6].MCS_Install_Time = "";
                    Main.m_stAGV[6].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[6] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V008")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(7), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[7].MCS_Carrier_ID = "";
                    Main.m_stAGV[7].MCS_Carrier_Type = "";
                    Main.m_stAGV[7].MCS_Install_Time = "";
                    Main.m_stAGV[7].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[7] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V009")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(8), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[8].MCS_Carrier_ID = "";
                    Main.m_stAGV[8].MCS_Carrier_Type = "";
                    Main.m_stAGV[8].MCS_Install_Time = "";
                    Main.m_stAGV[8].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[8] = "";
                }
                else if (Convert.ToString(rows[i][0]) == "CCAAGV01_V010")
                {
                    //Carrier Remove
                    Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(9), Convert.ToString(rows[i][15]));
                    Main.m_stAGV[9].MCS_Carrier_ID = "";
                    Main.m_stAGV[9].MCS_Carrier_Type = "";
                    Main.m_stAGV[9].MCS_Install_Time = "";
                    Main.m_stAGV[9].MCS_Carrier_LOC = "";
                    Main.CS_AGV_Logic.Save_CarrierID[9] = "";
                }
            }
            //CS_Work_DB.Select_MCS_Command_Info();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
        }
    }
}