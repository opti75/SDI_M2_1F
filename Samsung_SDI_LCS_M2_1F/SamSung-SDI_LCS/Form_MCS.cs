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
using XComPro.Library;
using System.Runtime.InteropServices;

namespace SDI_LCS
{
    public partial class Form_MCS : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private bool m_SECSCommunicationOK = false;
        public XComProW m_XComPro = new XComProW();
        Form1 Main;
        public int MCS_Priority  = 0;
        public short m_nDeviceID = 6;
        public int FLAG_MCS_AUTO = 0;
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
        [DllImport("kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);
        public void ChangeTime(string Time)
        {
            ushort Year = 0;
            ushort Month = 0;
            ushort Day = 0;
            ushort Hour = 0;
            ushort Minute = 0;
            ushort Second = 0;
            ushort M_Second = 0;


            Year = Convert.ToUInt16(Time.Substring(0, 4));
            Month = Convert.ToUInt16(Time.Substring(4, 2));
            Day = Convert.ToUInt16(Time.Substring(6, 2));
            Hour = Convert.ToUInt16(Time.Substring(8, 2));
            Minute = Convert.ToUInt16(Time.Substring(10, 2));
            Second = Convert.ToUInt16(Time.Substring(12, 2));
            M_Second = Convert.ToUInt16(Time.Substring(14, 2));

            SYSTEMTIME st;
            st.wYear = Year;
            st.wMonth = Month;
            st.wDayOfWeek = 0;
            st.wDay = Day;
            st.wHour = Hour;
            st.wMinute = Minute;
            st.wSecond = Second;
            st.wMilliseconds = M_Second;
            SetLocalTime(ref st); ;    // UTC+0 시간을 설정한다.
        }

        public Form_MCS()
        {
            InitializeComponent();
        }
        public Form_MCS(Form1 CS_Main)
        {
            Main = CS_Main;
            InitializeComponent();
        }
        
        public void AddLog(string formatString, params object[] args)
        {
            /*
            string addString = string.Format(formatString, args);
            lsLog.Items.Add(addString);
            if (lsLog.Items.Count > 100)
            {
                int n = lsLog.Items.Count - 100;
                for (; n > 0; n--)
                {
                    lsLog.Items.RemoveAt(0);
                }
            }
            lsLog.TopIndex = lsLog.Items.Count - 1;
            */
        }

        string Dump<ItemType>(ItemType[] items)
        {
            if (items == null || items.Length < 1) return "";
            string ret = items[0].ToString();
            for (int i = 1; i < items.Length; i++)
            {
                ret = ret + " " + items[i].ToString();
            }
            return ret;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void btStartXCom_Click(object sender, EventArgs e)
        {
            int ret = m_XComPro.Start();
          
            if (ret < 0)
            {
                AddLog("XComPro start failed: error={0}", ret);
                return;
            }

            if (m_XComPro.GetParam("HSMS") == "true")
            {
                AddLog("XComPro started in HSMS mode");
            }
            else
            {
                AddLog("XComPro started in SECS-I mode");
                m_SECSCommunicationOK = false;
            }

            btStopXCom.Enabled = true;
            btStartXCom.Enabled = false;
        }

        private void btStopXCom_Click(object sender, EventArgs e)
        {
            int ret = m_XComPro.Stop();
            if (ret < 0)
            {
                AddLog("XComPro stop failed: error={0}", ret);
                return;
            }

            btStartXCom.Enabled = true;
            btStopXCom.Enabled = false;
            m_SECSCommunicationOK = false;

            AddLog("XComPro stopped");
        }

        private void Form_MCS_Load(object sender, EventArgs e)
        {
            
            
        }

        public void m_XComPro_OnSecsMsg()
        {
            int lMsgId = 0, lSysbyte = 0, lSMsgId = 0;
            short nStream = 0, nFunc = 0, nDeviceID = 0, nWbit = 0;
            //ushort U2Data = 0;
            int nReturn = 0;

            while (m_XComPro.LoadSecsMsg(ref lMsgId, ref nDeviceID, ref nStream, ref nFunc, ref lSysbyte, ref nWbit) >= 0)
            {
                AddLog("Received S{0}F{1}, Sysbyte={2:X8}", nStream, nFunc, lSysbyte);

                if (FLAG_MCS_AUTO == 0)
                {
                    if(nStream != 1)
                    {
                        m_XComPro.CloseSecsMsg(lMsgId);
                        return;
                    }   
                }
                    
                        
                if (nStream == 1 && nFunc == 1)
                {
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS1F2(nDeviceID, lSysbyte);
                }
                else if (nStream == 1 && nFunc == 2)
                {
                    // Clear
                    int nLen = 0;
                    string sBuff = "";
                    int iListCount = 0;
                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    //  SendS1F2(nDeviceID, lSysbyte);
                }
                else if (nStream == 1 && nFunc == 3)
                {
                    // Clear
                    int iListCount = 0;
                    ushort U2Data = 0;

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data);

                    m_XComPro.CloseSecsMsg(lMsgId);

                    Main.CS_Work_DB.Select_MCS_AGV_Info();
                    SendS1F4(nDeviceID, lSysbyte, U2Data);
                }
                else if (nStream == 1 && nFunc == 11)
                {
                    // Clear
                    int iListCount = 0;
                    ushort U2Data = 0;
                    string SVNAME = "";
                    string SVUNIT = "";

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data);

                    m_XComPro.CloseSecsMsg(lMsgId);
                    SendS1F12(nDeviceID, lSysbyte, U2Data, SVNAME, SVUNIT);
                }
                else if (nStream == 1 && nFunc == 13)
                {
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS1F14(nDeviceID, lSysbyte);
                }
                else if (nStream == 1 && nFunc == 15)
                {
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS1F16(nDeviceID, lSysbyte);

                    m_XComPro.Stop();
                    m_XComPro.Start();
                    SendS6F11(m_nDeviceID, 0, 6, 11, 0, 1, 0, 0, "");
                }
                else if (nStream == 1 && nFunc == 17)
                {
                    FLAG_MCS_AUTO = 1;
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS1F18(nDeviceID, lSysbyte);
                }
                else if(nStream == 2 && nFunc == 15)
                {
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS2F16(nDeviceID, lSysbyte);
                }
                else if (nStream == 2 && nFunc == 29)
                {
                    // Clear
//                    string CmdID = "";
                    int iListCount = 0;
//                    byte bData = 0;
//                    sbyte I1Data = 0;
//                    short sData = 0;
//                    int iData = 0;
//                    long lData = 0;
//                    byte U1Data = 0;
                    ushort U2Data = 0;
//                    uint U4Data = 0;
//                    ulong U8Data = 0;
//                    float fData = 0;
//                    double dData = 0;
//                    bool blData = false;
//                    string sBuff = "";
//                    int nLen = 0;

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS2F30(nDeviceID, lSysbyte, U2Data);
                }
                else if (nStream == 2 && nFunc == 31)
                {
                    // Clear

                    int nLen = 0;
                    string sBuff = "";

                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    AddLog("Time = {0}: Len={1}", sBuff, nReturn);
                    m_XComPro.CloseSecsMsg(lMsgId);
                    ChangeTime(sBuff);
                    SendS2F32(nDeviceID, lSysbyte);
                }
                else if (nStream == 2 && nFunc == 33)
                {
                    // Clear
                    int nLen = 0;
                    string sBuff = "";

                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    AddLog("Time = {0}: Len={1}", sBuff, nReturn);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS2F34(nDeviceID, lSysbyte);
                }
                else if (nStream == 2 && nFunc == 35)
                {
                    // Clear
                    int nLen = 0;
                    string sBuff = "";

                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    AddLog("Time = {0}: Len={1}", sBuff, nReturn);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS2F36(nDeviceID, lSysbyte);
                }
                else if (nStream == 2 && nFunc == 37)
                {
                    // Clear
                    int nLen = 0;
                    string sBuff = "";

                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                    AddLog("Time = {0}: Len={1}", sBuff, nReturn);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS2F38(nDeviceID, lSysbyte);
                }
                else if (nStream == 2 && nFunc == 41)
                {
                    // Clear
                    int Abort_LGV_NUM = -1;
//                    string CmdID = "";
                    int iListCount = 0;
//                    byte bData = 0;
//                    sbyte I1Data = 0;
//                    short sData = 0;
//                    int iData = 0;
//                    long lData = 0;
//                    byte U1Data = 0;
//                    ushort U2Data = 0;
                    ushort U2Data_Priority = 0;
//                    uint U4Data = 0;
//                    ulong U8Data = 0;
//                    float fData = 0;
//                    double dData = 0;
//                    bool blData = false;
                    string sBuff = "";
                    string sBuff_NoUse = "";
                    string sBuff_CommandID = "";
                    string sBuff_Carrier = "";
                    string sBuff_Dest = "";

                    int nLen = 0;
                    byte HcACK = 0;
                    int FLAG_Cancel_Ok = 0;
                    int FLAG_Abort_Ok = 0;
                    int Priority_Update_Ok = 0;
                    int FLAG_Transfer_Update_1_Ok = 0;
                    int FLAG_Transfer_Update_2_Ok = 0;
                    //HcACK - 4 : 정상, 1 : CommandID가 없을때, 
                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);

                    //명령 체크
                    if (sBuff == "PAUSE" || sBuff == "RESUME")
                    {
                        HcACK = 4;
                    }
                    else if (sBuff == "CANCEL" || sBuff == "ABORT")
                    {
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("{0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_CommandID, ref nLen);
                        AddLog("CmdID = {0}", sBuff_CommandID);

                        HcACK = 4;

                        FLAG_Cancel_Ok = 0;
                        FLAG_Abort_Ok = 0;

                        for(int idx = 0; idx < Form1.LGV_NUM; idx ++)
                        {
                            if(sBuff == "ABORT" && (Main.m_stAGV[idx].MCS_Vehicle_Command_ID == sBuff_CommandID))
                            {
                                for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                                {
                                    if ((Main.Form_MCS.MCS_Transfer_State[i] != "6" && Main.Form_MCS.MCS_Transfer_State[i] != "0"))
                                    {
                                        if(Main.m_stAGV[idx].state != 2)
                                        {
                                            Abort_LGV_NUM = idx;
                                            FLAG_Abort_Ok = 1;
                                            break;
                                        }
                                        
                                    }
                                }
                            } 
                        }
                        for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                        {
                            if (Main.Form_MCS.MCS_Command_ID[i] == sBuff_CommandID)
                            {
                                if ((sBuff == "CANCEL" && Main.Form_MCS.MCS_Transfer_State[i] == "0"))
                                {
                                    FLAG_Cancel_Ok = 1;
                                    break;
                                }   
                            }
                        }
                    }
                    else if (sBuff == "PRIORITYUPDATE")
                    {
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("{0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_CommandID, ref nLen);
                        AddLog("CmdID = {0}", sBuff_CommandID);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("{0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data_Priority);
                        AddLog("Priority = {0}", U2Data_Priority);

                        HcACK = 4;
                        Priority_Update_Ok = 0;
                        for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                        {
                            if (Main.Form_MCS.MCS_Command_ID[i] == sBuff_CommandID && Main.Form_MCS.MCS_Transfer_State[i] == "0")
                            {
                                Priority_Update_Ok = 1;
                                break;
                            }
                        }
                    }
                    else if (sBuff == "TRANSFERUPDATE")
                    {
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                        AddLog("{0}", sBuff);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_Carrier, ref nLen);
                        AddLog("Carri ID = {0}", sBuff_Carrier);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                        AddLog("{0}", sBuff);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_Dest, ref nLen);
                        AddLog("Dest = {0}", sBuff_Dest);
                        HcACK = 4;
                        FLAG_Transfer_Update_1_Ok = 0;
                        FLAG_Transfer_Update_2_Ok = 0;
                        for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                        {
                            if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                            {
                                FLAG_Transfer_Update_1_Ok = 1;
                                break;
                            }
                        }
                        for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                        {
                            if (Main.Form_MCS.MCS_Carrier_ID[i] == sBuff_Carrier && Main.Form_MCS.MCS_Transfer_State[i] == "0")
                            {
                                FLAG_Transfer_Update_2_Ok = 1;
                                break;
                            }
                        }

                    }
                    m_XComPro.CloseSecsMsg(lMsgId);

                    //ACK 보내기
                    SendS2F42(nDeviceID, lSysbyte, HcACK);

                    if (HcACK == 4)
                    {
                        if (sBuff == "RESUME")
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 103, 0, 0, "");
                            Main.Flag_MCS_Auto = 1;
                        }
                        if (sBuff == "PAUSE")
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 107, 0, 0, "");
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 105, 0, 0, "");
                            Main.Flag_MCS_Auto = 0;
                        }

                        if (sBuff == "CANCEL" && FLAG_Cancel_Ok == 1)
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 206, 0, 0, sBuff_CommandID);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 204, 0, 0, sBuff_CommandID);
                            //SendS6F11(m_nDeviceID, 0, 6, 11, 0, 610, 0, 0, sBuff_CommandID);
                        }
                        else if (sBuff == "CANCEL" && FLAG_Cancel_Ok == 0)
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 206, 0, 0, sBuff_CommandID);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 205, 0, 0, sBuff_CommandID);
                        }

                        if (sBuff == "ABORT" && FLAG_Abort_Ok == 1)
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 203, 0, Convert.ToUInt16(Abort_LGV_NUM), sBuff_CommandID);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 610, 0, Convert.ToUInt16(Abort_LGV_NUM), sBuff_CommandID);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 201, 0, Convert.ToUInt16(Abort_LGV_NUM), sBuff_CommandID);
                        }
                        else if (sBuff == "ABORT" && FLAG_Abort_Ok == 0)
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 203, 0, 0, sBuff_CommandID);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 202, 0, 0, sBuff_CommandID);
                        }

                        if(sBuff == "PRIORITYUPDATE" && Priority_Update_Ok == 1)
                        {
                            MCS_Priority = U2Data_Priority;
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 503, 0, 0, sBuff_CommandID);
                        }
                        else if (sBuff == "PRIORITYUPDATE" && Priority_Update_Ok == 0)
                        {
                            MCS_Priority = U2Data_Priority;
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 504, 0, 0, sBuff_CommandID);
                        }

                        if (sBuff == "TRANSFERUPDATE" && FLAG_Transfer_Update_1_Ok == 1 && FLAG_Transfer_Update_2_Ok == 1)
                        {
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 212, 0, 0, sBuff_CommandID);
                            Main.CS_Work_DB.Update_Command_Transfer(sBuff_CommandID, Convert.ToString(U2Data_Priority), sBuff_Dest);
                            Main.CS_Work_DB.Select_MCS_Command_Info(0);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(0);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        }
                        else if(sBuff == "TRANSFERUPDATE" && (FLAG_Transfer_Update_1_Ok == 0 || FLAG_Transfer_Update_2_Ok == 0))
                        {

                        }
                    }
                }
                else if (nStream == 2 && nFunc == 49)
                {
                    // Clear
                    int iListCount = 0;
                    uint U4Data = 0;
                    string sBuff = "";
                    string sBuff_NoUse = "";
                    int nLen = 0;
                    string sBuff_CarrierID = "";
                    string Carrier_S_ID = "";
                    string[] sBuff_Carrier_S_ID = new string[10];
                    string sBuff_CommandID = "";
                    string sBuff_Source = "";
                    string sBuff_Dest = "";
                    int sBuff_Source_Count = 0;
                    int sBuff_Dest_Count = 0;

                    string sBuff_ProcessID = "";
                    string sBuff_Batchid = "";
                    string sBuff_LotID = "";
                    ushort U2Data_Priority = 0;
                    ushort U2Data_Carrier_Type = 0;
                    ushort U2Data_Quantity = 0;
                    int FLAG_Insert = 0;
                    //int FLAG_Insert_Carrier_ID = 0;
                    int FLAG_Insert_Source = 0;
                    int FLAG_Insert_Dest = 0;
                    int FLAG_Insert_Type_1 = 0;
                    int FLAG_Insert_Type_2 = 0;
                    int FLAG_Insert_Type_3 = 0;
                    int FLAG_Insert_Type_4 = 0;
                    int FLAG_Insert_Type_5 = 0;
                    int FLAG_Insert_Type_6 = 0;
                    int FLAG_Insert_Type_7 = 0;
                    int FLAG_Insert_Type_8 = 0;
                    int FLAG_Insert_Type_9 = 0;
                  

                    try
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            sBuff_Carrier_S_ID[i] = "";
                        }
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetU4Item(lMsgId, ref U4Data);
                        AddLog("Data ID = {0}", U4Data);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("OBJSPEC = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("TRANSFER = {0}", sBuff_NoUse);

                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CPNAME = {0}", sBuff_NoUse);

                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CPNAME = {0}", sBuff_NoUse);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_CommandID, ref nLen);
                        AddLog("CPVAL = {0}", sBuff_CommandID);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CPNAME = {0}", sBuff_NoUse);

                        nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data_Priority);
                        AddLog("CPVAL = {0}", U2Data_Priority);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CPNAME = {0}", sBuff_NoUse);

                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CARRIERID = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_CarrierID, ref nLen);
                        AddLog("VAL = {0}", sBuff_CarrierID);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("SOURCEPORT = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_Source, ref nLen);
                        AddLog("VAL = {0}", sBuff_Source);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("DESTPORT = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_Dest, ref nLen);
                        AddLog("VAL = {0}", sBuff_Dest);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("CARRIERTYPE = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data_Carrier_Type);
                        AddLog("VAL = {0}", U2Data_Carrier_Type);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("PROCESSID = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_ProcessID, ref nLen);
                        AddLog("VAL = {0}", sBuff_ProcessID);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff, ref nLen);
                        AddLog("BATCHID = {0}", sBuff);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_Batchid, ref nLen);
                        AddLog("VAL = {0}", sBuff_Batchid);



                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("LOTID = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_LotID, ref nLen);
                        AddLog("VAL = {0}", sBuff_LotID);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog("QUANTITY = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetU2Item(lMsgId, ref U2Data_Quantity);
                        AddLog("VAL = {0}", U2Data_Quantity);


                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                        nReturn = m_XComPro.GetAsciiItem(lMsgId, ref sBuff_NoUse, ref nLen);
                        AddLog(" = {0}", sBuff_NoUse);
                        nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                        for (int i = 0; i < iListCount; i++)
                        {
                            nReturn = m_XComPro.GetAsciiItem(lMsgId, ref Carrier_S_ID, ref nLen);
                            sBuff_Carrier_S_ID[i] = Carrier_S_ID;
                        }

                        m_XComPro.CloseSecsMsg(lMsgId);


                        for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                        {

                            if ((Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Source)
                              || (sBuff_Source == "CCAAGV01_V001" || sBuff_Source == "CCAAGV01_V002"
                               || sBuff_Source == "CCAAGV01_V003" || sBuff_Source == "CCAAGV01_V004"
                               || sBuff_Source == "CCAAGV01_V005" || sBuff_Source == "CCAAGV01_V006"
                               || sBuff_Source == "CCAAGV01_V007" || sBuff_Source == "CCAAGV01_V008"
                               || sBuff_Source == "CCAAGV01_V009" || sBuff_Source == "CCAAGV01_V010"))
                            {
                                FLAG_Insert_Source = 1;
                                sBuff_Source_Count = Work_Station_Count;
                            }
                            if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                            {
                                FLAG_Insert_Dest = 1;
                                sBuff_Dest_Count = Work_Station_Count;
                            }
                            /*
                            if (sBuff_Source == "CCAAGV01_V001")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[0].Type)
                                    {
                                        FLAG_Insert_Type_1 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V002")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[1].Type)
                                    {
                                        FLAG_Insert_Type_2 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V003")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[2].Type)
                                    {
                                        FLAG_Insert_Type_3 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V004")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[3].Type)
                                    {
                                        FLAG_Insert_Type_4 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V005")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[4].Type)
                                    {
                                        FLAG_Insert_Type_5 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V006")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[5].Type)
                                    {
                                        FLAG_Insert_Type_6 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V007")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[6].Type)
                                    {
                                        FLAG_Insert_Type_7 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V008")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[7].Type)
                                    {
                                        FLAG_Insert_Type_8 = 1;
                                    }
                                }
                            }
                            else if (sBuff_Source == "CCAAGV01_V009")
                            {
                                if (Main.CS_Work_Path[Work_Station_Count].Work_Station == sBuff_Dest)
                                {
                                    if (Main.CS_Work_Path[Work_Station_Count].Type != Main.CS_AGV_C_Info[8].Type)
                                    {
                                        FLAG_Insert_Type_9 = 1;
                                    }
                                }
                            }
                            */


                            if (FLAG_Insert_Source == 1 && FLAG_Insert_Dest == 1)
                            {
                                break;
                            }
                        }
                        //Carrier_ID 중복 막기
                        for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                        {
                            //제품명, 작업명 같으면 52
                            if (Main.Form_MCS.MCS_Carrier_ID[i] == sBuff_CarrierID && Main.Form_MCS.MCS_Command_ID[i] == sBuff_CommandID)
                            {
                                FLAG_Insert = 1;
                                SendS2F50(nDeviceID, lSysbyte, Convert.ToByte(52));
                                Main.Log("MES_Order_HCACK", "제품명 중복" + sBuff_CarrierID);
                                break;
                            }
                            //작업명만 같으면 52
                            else if (Main.Form_MCS.MCS_Carrier_ID[i] != sBuff_CarrierID && Main.Form_MCS.MCS_Command_ID[i] == sBuff_CommandID)
                            {
                                FLAG_Insert = 1;
                                SendS2F50(nDeviceID, lSysbyte, Convert.ToByte(52));
                                Main.Log("MES_Order_HCACK", "제품명 중복" + sBuff_CarrierID);
                                break;
                            }
                            //제품명만 같으면 53
                            else if (Main.Form_MCS.MCS_Carrier_ID[i] == sBuff_CarrierID && Main.Form_MCS.MCS_Command_ID[i] != sBuff_CommandID)
                            {
                                FLAG_Insert = 1;
                                SendS2F50(nDeviceID, lSysbyte, Convert.ToByte(53));
                                Main.Log("MES_Order_HCACK", "제품명 중복" + sBuff_CarrierID);
                                break;
                            }
                        }


                        if (FLAG_Insert_Source == 0)
                        {
                            SendS2F50(nDeviceID, lSysbyte, Convert.ToByte(1F));
                            Main.Log("MES_Order_HCACK", "등록 안된 출발지");
                        }
                        if (FLAG_Insert_Dest == 0)
                        {
                            SendS2F50(nDeviceID, lSysbyte, Convert.ToByte(20));
                            Main.Log("MES_Order_HCACK", "등록 안된 도착지");
                        }

                        string Call_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


                        if (FLAG_Insert == 0 && FLAG_Insert_Source == 1 && FLAG_Insert_Dest == 1)
                        {
                            Main.CS_Work_DB.Insert_DB_MCS_Work(sBuff_CarrierID, sBuff_Carrier_S_ID, sBuff_CommandID,
                                                          sBuff_Source, sBuff_Dest, sBuff_ProcessID, sBuff_Batchid, sBuff_LotID,
                                                          U2Data_Priority, U2Data_Quantity, Call_Time);

                            Main.CS_Work_DB.Select_MCS_Command_Info(0);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(0);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

                            SendS2F50(nDeviceID, lSysbyte, 4);
                            SendS6F11(m_nDeviceID, 0, 6, 11, 0, 208, 0, 0, sBuff_CommandID);
                            Main.Log("MES_Order", "출발지 = " + sBuff_Source + "도착지" + sBuff_Dest);
                        }
                    }
                    
                    catch(Exception ex)
                    {
                        Main.Log("S2F49 ERROR", Convert.ToString(ex));
                    }
                    
                }
                else if (nStream == 2 && nFunc == 13)
                {
                    int iListCount = 0;
                    ushort[] nRecived;

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    AddLog("LIST {0}: next={1}", iListCount, nReturn);
                    nRecived = new ushort[iListCount];

                    for (int i = 0; i < iListCount; i++)
                    {
                        ushort nData = 0;
                        nReturn = m_XComPro.GetU2Item(lMsgId, ref nData);
                        nRecived[i] = nData;
                        AddLog("    UINT2 {0}: next={1}", nData, nReturn);
                    }
                    m_XComPro.CloseSecsMsg(lMsgId);

                    nReturn = m_XComPro.MakeSecsMsg(ref lSMsgId, nDeviceID, nStream, (short)(nFunc + 1), lSysbyte);
                    #region Return Values
                    if (nReturn < 0)
                    {
                        AddLog("MakeSecsMsg failed: error={0}", nReturn);
                        return;
                    }
                    #endregion
                    nReturn = m_XComPro.SetListItem(lSMsgId, 2);
                    nReturn = m_XComPro.SetBinaryItem(lSMsgId, 0);
                    nReturn = m_XComPro.SetListItem(lSMsgId, iListCount);
                    for (int i = 0; i < iListCount; i++)
                    {
                        nReturn = m_XComPro.SetU2Item(lSMsgId, nRecived[i]);
                    }
                    if (nWbit != 0)
                    {
                        if ((nReturn = m_XComPro.Send(lSMsgId)) < 0)
                        {
                            AddLog("Failed to reply S{0}F{1}: error={2}", nStream, nFunc + 1, nReturn);
                        }
                        else
                        {
                            AddLog("Reply S{0}F{1} was sent successfully", nStream, nFunc + 1);
                        }
                    }
                }
                else if (nStream == 5 && nFunc == 3)
                {
                    // Clear
                    m_XComPro.CloseSecsMsg(lMsgId);
                    SendS5F4(nDeviceID, lSysbyte);
                }

                else if (nStream == 5 && nFunc == 5)
                {
                    // Clear
                    int iListCount = 0;
                    uint[] A_List_No = new uint[30];
                    uint recv_ALID = 0;

                    for (int i = 0; i < 30; i++)
                    {
                        A_List_No[i] = 0;
                    }

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);

                    for (int i = 0; i < iListCount; i++)
                    {
                        nReturn = m_XComPro.GetU4Item(lMsgId, ref recv_ALID);

                        A_List_No[i] = recv_ALID;

                    }

                    m_XComPro.CloseSecsMsg(lMsgId);

                    SendS5F6(nDeviceID, lSysbyte, A_List_No, iListCount);
                }
                else if (nStream == 6 && nFunc == 12)
                {
                    // S6F12 Event Report Ack
                    byte bData = 0;
                    nReturn = m_XComPro.GetBinaryItem(lMsgId, ref bData);
                    m_XComPro.CloseSecsMsg(lMsgId);

                    AddLog("Report Ack S6F12 : ACKC6 = {0}", bData);

                    if (nWbit != 0)
                    {
                        if ((nReturn = m_XComPro.Send(lSMsgId)) < 0)
                        {
                            AddLog("Failed to reply S{0}F{1}: error={2}", nStream, nFunc + 1, nReturn);
                        }
                        else
                        {
                            AddLog("Reply S{0}F{1} was sent successfully", nStream, nFunc + 1);
                        }
                    }
                }
                else if (nStream == 6 && nFunc == 15)
                {
                    // Clear

                    // Clear
                    ushort CEID = 0;
                    int iListCount = 0;

                    nReturn = m_XComPro.GetListItem(lMsgId, ref iListCount);
                    nReturn = m_XComPro.GetU2Item(lMsgId, ref CEID);
                    m_XComPro.CloseSecsMsg(lMsgId);
                    SendS6F11(m_nDeviceID, lSysbyte, 6, 16, 0, CEID, 0, 0, "");
                }
                else if (nStream == 6 && nFunc == 19)
                {
                    // Clear

                    // Clear
                    ushort RPTID = 0;

                    nReturn = m_XComPro.GetU2Item(lMsgId, ref RPTID);
                    m_XComPro.CloseSecsMsg(lMsgId);


                    SendS6F11(m_nDeviceID, lSysbyte, 6, 20, RPTID, 0, 0, 0, "");
                }
                else
                {
                    AddLog("Undefined message received (S{0}F{1})", nStream, nFunc);
                    m_XComPro.CloseSecsMsg(lMsgId);
                    return;
                }

            }
        }
        public void m_XComPro_OnSecsEvent(short nEventId, int lParam)
        {

            int lSysByte = 0, lMsgId = 0; short nDevId = 0, nStrm = 0, nFunc = 0, nWbit = 0; int lResult = 0;

            switch (nEventId)
            {
                case 101:
                    Main.panel1.BackColor = Color.Red;
                    Main.P_MCS.BackColor = Color.Red;
                    AddLog("[EVENT] HSMS NOT CONNECTED");
                    m_SECSCommunicationOK = false;
                    break;
                case 102:
                    AddLog("[EVENT] HSMS NOT SELECTED");
                    m_SECSCommunicationOK = false;
                    break;
                case 103:
                    Main.panel1.BackColor = Color.Yellow;
                    Main.P_MCS.BackColor = Color.Yellow;
                    AddLog("[EVENT] HSMS SELECTED");
                    m_SECSCommunicationOK = true;
                    break;
                    /*
                case 203: // T3 timeout 
                    lMsgId = lParam;

                    lResult = m_XComPro.GetAlarmMsgInfo(lMsgId, ref nDevId, ref nStrm, ref nFunc, ref lSysByte, ref nWbit);
                    AddLog("[EVENT] T3 timeout: S{0}F{1}", nStrm, nFunc);

                    SendS9F9(m_nDeviceID);
                    break;
                    */
                default:
                    AddLog("[EVENT] Other event: eventId = {0}", nEventId);
                    break;
            }
        }

        private void btSendMessage_Click(object sender, EventArgs e)
        {
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendS1F1(m_nDeviceID, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendS1F13(m_nDeviceID);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //   SendS1F14(m_nDeviceID, lSysbyte);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //   SendS1F18(m_nDeviceID, lSysbyte);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendS2F17(m_nDeviceID, 0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SendS1F4(m_nDeviceID, 0, 21);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void lsLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lsLog.Items.Clear();

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001","0");
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "1");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "2");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "3");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "4");
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "5");
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_State("CCAAGV01_V001", "6");
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_Current("CCAAGV01_V001", textBox6.Text);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
    
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_Goal("CCAAGV01_V001", textBox7.Text);
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            Main.Flag_MCS_Auto = 1;
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_Alarm(Main.m_stAGV[0].MCS_Vehicle_ID, "1", Main.Alarm_List[1]);

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            //Main.CS_Work_DB.Update_AGV_Info_Alarm(Main.m_stAGV[0].MCS_Vehicle_ID, "0", "0");
            
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton15_Click_1(object sender, EventArgs e)
        {
   
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {

        }
    }
}