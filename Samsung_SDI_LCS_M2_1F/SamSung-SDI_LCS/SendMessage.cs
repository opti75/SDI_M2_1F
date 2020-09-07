using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public partial class Form_MCS
    {
        public string[] MCS_Carrier_ID = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Vehicle_ID = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Carrier_LOC = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Install_Time = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Carrier_Type = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Source_Port = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Dest_Port = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Transfer_State = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_Command_ID = new string[Form1.MAX_COMMAND_SIZE];
        public string[] MCS_PROIORITY = new string[Form1.MAX_COMMAND_SIZE];

        public void Init_Mes_Send()
        {
            for(int i = 0; i < Form1.MAX_COMMAND_SIZE; i++)
            {
                MCS_Carrier_ID[i] = "";
                MCS_Vehicle_ID[i] = "";
                MCS_Carrier_LOC[i] = "";
                MCS_Install_Time[i] = "";
                MCS_Carrier_Type[i] = "";
                MCS_Source_Port[i] = "";
                MCS_Dest_Port[i] = "";
                MCS_Transfer_State[i] = "";
                MCS_Command_ID[i] = "";
                MCS_PROIORITY[i] = "";
            }
        }




        const sbyte VALUE_I1 = (sbyte)10;
        const short VALUE_I2 = (short)32555;
        const int VALUE_I4 = (int)655360;
        const int VALUE_I8 = (int)240000001;
        const byte VALUE_U1 = (byte)250;
        const ushort VALUE_U2 = (ushort)65000;
        const uint VALUE_U4 = (uint)4294967290;
        const uint VALUE_U8 = (uint)1844674400;
        const float VALUE_F4 = (float)1234.567;
        const double VALUE_F8 = (double)123456789.8765321;
        const bool VALUE_BOOL = true;
        const byte VALUE_BINARY = (byte)10;
        const string VALUE_STRING = "STRING; Linkgenesis XComPro Sample";
        const string VALUE_JIS8 = "JIS8; Linkgenesis XComPro Sample";
        sbyte[] VALUE_I1_ARR = new sbyte[] { (sbyte)1, (sbyte)2, (sbyte)3, (sbyte)4, (sbyte)6, (sbyte)7 };
        short[] VALUE_I2_ARR = new short[] { (short)1, (short)2, (short)3, (short)4, (short)6, (short)7 };
        int[] VALUE_I4_ARR = new int[] { (int)1, (int)2, (int)3, (int)4, (int)6, (int)7 };
        long[] VALUE_I8_ARR = new long[] { (long)1, (long)2, (long)3, (long)4, (long)6, (long)7 };
        byte[] VALUE_U1_ARR = new byte[] { (byte)1, (byte)2, (byte)3, (byte)4, (byte)6, (byte)7 };
        ushort[] VALUE_U2_ARR = new ushort[] { (ushort)1, (ushort)2, (ushort)3, (ushort)4, (ushort)6, (ushort)7 };
        uint[] VALUE_U4_ARR = new uint[] { (uint)1, (uint)2, (uint)3, (uint)4, (uint)6, (uint)7 };
        ulong[] VALUE_U8_ARR = new ulong[] { (ulong)1, (ulong)2, (ulong)3, (ulong)4, (ulong)6, (ulong)7 };
        float[] VALUE_F4_ARR = new float[] { (float)1, (float)2, (float)3, (float)4, (float)6, (float)7 };
        double[] VALUE_F8_ARR = new double[] { (double)1, (double)2, (double)3, (double)4, (double)6, (double)7 };
        bool[] VALUE_BOOL_ARR = new bool[] { true, false, true, false, true, false, true };
        byte[] VALUE_BINARY_ARR = new byte[] { (byte)1, (byte)2, (byte)3, (byte)4, (byte)6, (byte)7 };


        const string EQNAME_F1 = "CCAAGV01";
        const string EQNAME_F3 = "CCAAGV03";

        const string AGV_Name_01 = "CCAAGV01_V001";
        const string AGV_Name_02 = "CCAAGV01_V001";
        const string AGV_Name_03 = "CCAAGV01_V001";



        // 전역변수 추가
        const string STR_EQPNAME = "EQ_AGV";
        public string Command_Type = "";

        public void SendS1F1(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 1, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F1: error={0}", ret);
            }
            else
            {
                AddLog("S1F1 was sent successfully");
            }
            #endregion
        }

        public void SendS1F2(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 2, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetListItem(lMsgId, 2);
            ret = m_XComPro.SetAsciiItem(lMsgId, "AGVRCS");
            ret = m_XComPro.SetAsciiItem(lMsgId, "0.1");

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F2: error={0}", ret);
            }
            else
            {
                AddLog("S1F2 was sent successfully");
            }
            #endregion
        }

        public void SendS1F4(short DeviceID, int lSysbyte, ushort SVID)
        {
            int lMsgId = 0;
            int AGV_Count = 0;
            int Command_Count = 0;
            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 4, lSysbyte);
            #region Return Values
            
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }

            #endregion
            #region SVID 21 EnhancedCarriers
            if (SVID == 21)
            {
                ret = m_XComPro.SetListItem(lMsgId, 1);
                ret = m_XComPro.SetListItem(lMsgId, Main.CS_Work_DB.Working_Command_Count);

                for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                {
                    ret = m_XComPro.SetListItem(lMsgId, 5);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Carrier_ID[i]);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Vehicle_ID[i]);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Carrier_LOC[i]);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Install_Time[i]);
                    ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(MCS_Carrier_Type[i]));
                }
            }
            #endregion
            #region SVID 23 EnhancedTransfers
            else if (SVID == 23)
            {
                Command_Count = 0;
                ret = m_XComPro.SetListItem(lMsgId, 1);

                for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                {
                    Command_Count++;
                }
                ret = m_XComPro.SetListItem(lMsgId, Command_Count);

                for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                {
                    ret = m_XComPro.SetListItem(lMsgId, 3);
                    ret = m_XComPro.SetListItem(lMsgId, 2);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Command_ID[i]);

                    if (MCS_PROIORITY[i] == "" || MCS_PROIORITY[i] == null)
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, 0);
                    }
                    else
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(MCS_PROIORITY[i]));
                    }

                    if (MCS_Transfer_State[i] == "" || MCS_Transfer_State[i] == null)
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, 0);
                    }
                    else
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(MCS_Transfer_State[i]));
                    }


                    ret = m_XComPro.SetListItem(lMsgId, 1);
                    ret = m_XComPro.SetListItem(lMsgId, 3);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Carrier_ID[i]);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Source_Port[i]);
                    ret = m_XComPro.SetAsciiItem(lMsgId, MCS_Dest_Port[i]);
                }
            }
            #endregion
            #region SVID 25 EnhancedVehicles
            else if (SVID == 25)
            {
                AGV_Count = 0;
                ret = m_XComPro.SetListItem(lMsgId, 1);

                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    AGV_Count++;
                }
                ret = m_XComPro.SetListItem(lMsgId, AGV_Count);
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    ret = m_XComPro.SetListItem(lMsgId, 2);

                    ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[i].MCS_Vehicle_ID);
                    if (Main.m_stAGV[i].MCS_Vehicle_State == "" || Main.m_stAGV[i].MCS_Vehicle_State == null)
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, 0);
                    }
                    else
                    {
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[i].MCS_Vehicle_State));
                    }

                }

            }
            #endregion
            #region SVID 61 Enhanced_Unit_Alarms
            else if (SVID == 61)
            {
                AGV_Count = 0;

                ret = m_XComPro.SetListItem(lMsgId, 1);
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (Main.m_stAGV[i].MCS_Alarm_ID != "0" && Main.m_stAGV[i].MCS_Alarm_ID != null && Main.m_stAGV[i].MCS_Alarm_ID != "")
                    {
                        AGV_Count++;
                    }
                }
                ret = m_XComPro.SetListItem(lMsgId, AGV_Count);
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (Main.m_stAGV[i].MCS_Alarm_ID != "0" && Main.m_stAGV[i].MCS_Alarm_ID != null && Main.m_stAGV[i].MCS_Alarm_ID != "")
                    {
                        ret = m_XComPro.SetListItem(lMsgId, 3);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[i].MCS_Vehicle_ID);

                        if (Main.m_stAGV[i].MCS_Alarm_ID == "0" && Main.m_stAGV[i].MCS_Alarm_ID == null && Main.m_stAGV[i].MCS_Alarm_ID != "")
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 0);
                        }
                        else
                        {
                            ret = m_XComPro.SetU4Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[i].MCS_Alarm_ID));
                        }

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[i].MCS_Alarm_Text);
                    }
                }

            }
            #endregion
            else
            {
                ret = m_XComPro.SetListItem(lMsgId, 1);
                ret = m_XComPro.SetU2Item(lMsgId, 2);
            }

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F4: error={0}", ret);
            }
            else
            {
                AddLog("S1F4 was sent successfully");
            }
            #endregion
        }


        public void SendS1F12(short DeviceID, int lSysbyte, ushort SVID, string SVNAME, string SVUNIT)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 12, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            ret = m_XComPro.SetListItem(lMsgId, 1);

            ret = m_XComPro.SetListItem(lMsgId, 3);
            ret = m_XComPro.SetU2Item(lMsgId, SVID);
            ret = m_XComPro.SetAsciiItem(lMsgId, SVNAME);
            ret = m_XComPro.SetAsciiItem(lMsgId, SVUNIT);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F13: error={0}", ret);
            }
            else
            {
                AddLog("S1F13 was sent successfully");
            }
            #endregion
        }


        public void SendS1F13(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 13, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            ret = m_XComPro.SetListItem(lMsgId, 2);
            ret = m_XComPro.SetAsciiItem(lMsgId, "AGVRCS");
            ret = m_XComPro.SetAsciiItem(lMsgId, "0.1");


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F13: error={0}", ret);
            }
            else
            {
                AddLog("S1F13 was sent successfully");
            }
            #endregion
        }


        public void SendS1F14(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 14, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetListItem(lMsgId, 2);
            ret = m_XComPro.SetBinaryItem(lMsgId, 0);

            ret = m_XComPro.SetListItem(lMsgId, 2);
            ret = m_XComPro.SetAsciiItem(lMsgId, "ACSF1");
            ret = m_XComPro.SetAsciiItem(lMsgId, "0.01");

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F14: error={0}", ret);
            }
            else
            {
                AddLog("S1F14 was sent successfully");
            }
            #endregion
        }

        public void SendS1F16(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 16, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F18: error={0}", ret);
            }
            else
            {
                AddLog("S1F18 was sent successfully");
            }
            #endregion
        }

        public void SendS1F18(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 1, 18, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S1F18: error={0}", ret);
            }
            else
            {
                AddLog("S1F18 was sent successfully");
            }
            #endregion
        }

        public void SendS2F16(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 16, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F16: error={0}", ret);
            }
            else
            {
                AddLog("S2F16 was sent successfully");
            }
            #endregion
        }


        public void SendS2F17(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 17, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion



            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F17: error={0}", ret);
            }
            else
            {
                AddLog("S2F17 was sent successfully");
            }
            #endregion
        }


        public void SendS2F30(short DeviceID, int lSysbyte, ushort ECID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 30, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            ret = m_XComPro.SetListItem(lMsgId, 1);
            ret = m_XComPro.SetListItem(lMsgId, 6);


            ret = m_XComPro.SetU2Item(lMsgId, ECID);
            ret = m_XComPro.SetAsciiItem(lMsgId, "ECNAME");
            ret = m_XComPro.SetU2Item(lMsgId, 0);
            ret = m_XComPro.SetU2Item(lMsgId, 0);
            ret = m_XComPro.SetU2Item(lMsgId, 0);

            ret = m_XComPro.SetAsciiItem(lMsgId, "UNIT");




            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F30: error={0}", ret);
            }
            else
            {
                AddLog("S2F30 was sent successfully");
            }
            #endregion
        }


        public void SendS2F32(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 32, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F32: error={0}", ret);
            }
            else
            {
                AddLog("S2F32 was sent successfully");
            }
            #endregion
        }

        public void SendS2F34(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 34, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F34: error={0}", ret);
            }
            else
            {
                AddLog("S2F34 was sent successfully");
            }
            #endregion
        }


        public void SendS2F36(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 36, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F36: error={0}", ret);
            }
            else
            {
                AddLog("S2F36 was sent successfully");
            }
            #endregion
        }

        public void SendS2F38(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 38, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F38: error={0}", ret);
            }
            else
            {
                AddLog("S2F38 was sent successfully");
            }
            #endregion
        }



        public void SendS2F42(short DeviceID, int lSysbyte, byte HCACK)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 42, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion
            if (HCACK == 3)
            {
                ret = m_XComPro.SetListItem(lMsgId, 2);
                ret = m_XComPro.SetBinaryItem(lMsgId, HCACK);
                ret = m_XComPro.SetListItem(lMsgId, 1);
                ret = m_XComPro.SetListItem(lMsgId, 2);

                ret = m_XComPro.SetAsciiItem(lMsgId, "CP_PARA");
                ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            }
            else
            {
                ret = m_XComPro.SetListItem(lMsgId, 2);
                ret = m_XComPro.SetBinaryItem(lMsgId, HCACK);
                ret = m_XComPro.SetListItem(lMsgId, 0);

            }


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F42: error={0}", ret);
            }
            else
            {
                AddLog("S2F42 was sent successfully");
            }
            #endregion
        }


        public void SendS2F50(short DeviceID, int lSysbyte, byte HCACK)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 50, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            if (HCACK == 3)
            {
                ret = m_XComPro.SetListItem(lMsgId, 2);
                ret = m_XComPro.SetBinaryItem(lMsgId, HCACK);
                ret = m_XComPro.SetListItem(lMsgId, 1);
                ret = m_XComPro.SetListItem(lMsgId, 2);

                ret = m_XComPro.SetAsciiItem(lMsgId, "CP_PARA");
                ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            }
            else
            {
                ret = m_XComPro.SetListItem(lMsgId, 2);
                ret = m_XComPro.SetBinaryItem(lMsgId, HCACK);
                ret = m_XComPro.SetListItem(lMsgId, 0);

            }


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F50: error={0}", ret);
            }
            else
            {
                AddLog("S2F50 was sent successfully");
            }
            #endregion
        }




        public void SendS5F1(short DeviceID, byte Alarm_Code, string Alarm_ID, string Alarm_Text)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 5, 1, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            ret = m_XComPro.SetListItem(lMsgId, 3);

            ret = m_XComPro.SetBinaryItem(lMsgId, Alarm_Code);
            ret = m_XComPro.SetU4Item(lMsgId, Convert.ToUInt32(Alarm_ID));
            ret = m_XComPro.SetAsciiItem(lMsgId, Alarm_Text);

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S5F1_Vehicle_State: error={0}", ret);
            }
            else
            {
                AddLog("S5F1_Vehicle_State was sent successfully");
            }
            #endregion
        }


        public void SendS5F4(short DeviceID, int lSysbyte)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 5, 4, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.SetBinaryItem(lMsgId, 0);


            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S5F4: error={0}", ret);
            }
            else
            {
                AddLog("S5F4 was sent successfully");
            }
            #endregion
        }

        public void SendS5F6(short DeviceID, int lSysbyte, uint[] ALID_List, int ALID_Num)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 5, 6, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            ret = m_XComPro.SetListItem(lMsgId, ALID_Num);
            for (int i = 0; i < ALID_Num; i++)
            {
                ret = m_XComPro.SetListItem(lMsgId, 3);

                ret = m_XComPro.SetBinaryItem(lMsgId, 128);
                ret = m_XComPro.SetU4Item(lMsgId, ALID_List[i]);
                ret = m_XComPro.SetAsciiItem(lMsgId, Main.Alarm_List[ALID_List[i]]);
            }



            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S5F1_Vehicle_State: error={0}", ret);
            }
            else
            {
                AddLog("S5F1_Vehicle_State was sent successfully");
            }
            #endregion
        }

        // CEID 받아서 RPTID 선택
        public ushort GetRPTID(ushort CEID)
        {
            ushort RPTID = 0;

            switch (CEID)
            {
                case 1:
                case 2:
                case 3:

                case 103:
                case 104:
                case 105:
                case 106:
                case 107:

                    RPTID = 1;
                    break;


                case 101:
                case 102:

                    RPTID = 2;
                    break;


                case 201:

                    RPTID = 3;
                    break;

                case 202:
                case 203:
                case 204:
                case 205:
                case 206:

                case 208:
                case 209:
                case 210:
                case 211:

                    RPTID = 4;
                    break;


                case 207:

                    RPTID = 5;
                    break;

                case 301:
                case 302:

                    RPTID = 6;
                    break;

                case 501:

                    RPTID = 8;
                    break;

                case 601:
                case 605:

                    RPTID = 9;
                    break;

                case 602:
                case 603:
                case 606:
                case 607:

                    RPTID = 10;
                    break;

                case 604:
                case 610:

                    RPTID = 11;
                    break;

                case 608:
                case 609:
                case 612:
                case 613:

                    RPTID = 12;
                    break;

                case 701:
                case 702:

                    RPTID = 13;
                    break;

                case 503:
                case 504:

                    RPTID = 14;
                    break;

                case 502:

                    RPTID = 15;
                    break;

                case 611:

                    RPTID = 16;
                    break;

            }


            return RPTID;
        }
        // S6F11 메세지 전송 함수 , S6F16 메세지 전송 함수, S6F20
        public void SendS6F11(short DeviceID, int lSysbyte, short Stream, short Fnc, ushort RPTID, ushort CEID,
                              uint DataID, uint AGV_No, string Cmd_ID)
        {
            // 상태 변수
            string EqpName = "EQ_AGV";
            string CommandID = "CMD000001";
            string CommandType = "NORMAL";
            string VehicleID = "AGV_001";
            ushort VehicleState = 0;
            string CarrierID = "CAR000001";
            string SourcePort = "SOS123456";
            string DestPort = "DST123456";
            string CarrierLoc = "0001";
            ushort CarrierType = 1;
            ushort ResultCode = 0;

            string TransferPort = "POT000001";
            string UnitID = "UID000001";
            uint AlarmID = 0;
            string AlarmText = "None";
            string VehicleCurrPos = "0002";
            int CMD_Index = -1;

            //
            int lMsgId = 0;
            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, Stream, Fnc, lSysbyte);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion


            try
            {

                if (Stream == 6 && (Fnc == 11 || Fnc == 16))
                {
                    RPTID = GetRPTID(CEID);

                    ret = m_XComPro.SetListItem(lMsgId, 3);

                    ret = m_XComPro.SetU4Item(lMsgId, DataID);
                    ret = m_XComPro.SetU2Item(lMsgId, CEID);

                    ret = m_XComPro.SetListItem(lMsgId, 1);
                    ret = m_XComPro.SetListItem(lMsgId, 2);

                    ret = m_XComPro.SetU2Item(lMsgId, RPTID);
                }
                else if (Stream == 6 && Fnc == 20)
                {
                    ret = m_XComPro.SetListItem(lMsgId, 1);
                }

                switch (RPTID)
                {
                    case 1:
                        ret = m_XComPro.SetListItem(lMsgId, 1);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Form1.EQPNAME);
                        break;

                    case 2:
                        ret = m_XComPro.SetListItem(lMsgId, 2);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_Command_ID);

                        ret = m_XComPro.SetListItem(lMsgId, 2);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        if (Main.m_stAGV[AGV_No].MCS_Vehicle_State == "" || Main.m_stAGV[AGV_No].MCS_Vehicle_State == null)
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 2);
                        }
                        else
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[AGV_No].MCS_Vehicle_State));
                        }
                        break;

                    case 3:

                        for (int i = 0; i < Main.CS_Work_DB.Working_Command_Count; i++)
                        {
                            if (Main.Form_MCS.MCS_Command_ID[i] == Cmd_ID)
                            {
                                ret = m_XComPro.SetListItem(lMsgId, 2);
                                ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);

                                ret = m_XComPro.SetListItem(lMsgId, 1);
                                ret = m_XComPro.SetListItem(lMsgId, 2);
                                ret = m_XComPro.SetListItem(lMsgId, 3);

                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Carrier_ID[i]);
                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Source_Port[i]);
                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Dest_Port[i]);

                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_LOC);

                                if (Main.m_stAGV[AGV_No].MCS_Carrier_ID == "")
                                {
                                    Main.m_stAGV[AGV_No].MCS_Carrier_LOC = "";
                                    Main.m_stAGV[AGV_No].MCS_Carrier_Type = "";
                                }
                                else if (Main.m_stAGV[AGV_No].MCS_Carrier_ID != "")
                                {
                                    Main.m_stAGV[AGV_No].MCS_Carrier_LOC = Main.m_stAGV[AGV_No].MCS_Vehicle_ID;

                                    if (AGV_No == 0 || AGV_No == 1 || AGV_No == 9 || AGV_No == 11 || AGV_No == 12)
                                    {
                                        //REEL
                                        Main.m_stAGV[AGV_No].MCS_Carrier_Type = "3";
                                    }
                                 
                                    else if (AGV_No == 2 || AGV_No == 3 || AGV_No == 4)
                                    {
                                        //ROLL
                                        Main.m_stAGV[AGV_No].MCS_Carrier_Type = "5";
                                    }
                                    else if (AGV_No == 5 || AGV_No == 6 || AGV_No == 7)
                                    {
                                        //ROLL
                                        Main.m_stAGV[AGV_No].MCS_Carrier_Type = "6";
                                    }
                                    else if (AGV_No == 8)
                                    {
                                        //ROLL
                                        Main.m_stAGV[AGV_No].MCS_Carrier_Type = "9";
                                    }
                                }
                             
                                Main.CS_Work_DB.Select_MCS_Command_Info_Log(Cmd_ID);

                                Main.CS_Work_DB.Delete_Work_Log(Cmd_ID);

                                //엑셀에 작업 로그 저장
                                Main.CS_Work_DB.Insert_Excel_Data_Log(Main.CS_AGV_Logic.E_Call_Time, Main.CS_AGV_Logic.E_Command_ID, Main.CS_AGV_Logic.E_Carrier_ID, Main.CS_AGV_Logic.E_Source_Port, Main.CS_AGV_Logic.E_Dest_Port, Main.CS_AGV_Logic.E_LGV_No, Main.CS_AGV_Logic.E_Alloc_Time, Main.CS_AGV_Logic.E_Load_Move_Time,
                                    Main.CS_AGV_Logic.E_Load_Time, Main.CS_AGV_Logic.E_UnLoad_Move_Time, Main.CS_AGV_Logic.E_UnLoad_Time, "중단", Main.CS_AGV_Logic.E_Load_Move_Time_Result, Main.CS_AGV_Logic.E_Load_Time_Result, Main.CS_AGV_Logic.E_UnLoad_Move_Time_Result,
                                    Main.CS_AGV_Logic.E_UnLoad_Time_Result, Main.CS_AGV_Logic.E_Total_Time_Result, Main.CS_AGV_Logic.E_Complete_Time, Main.CS_AGV_Logic.Alloc_Current[AGV_No]);

                                Main.CS_Work_DB.Select_MCS_Command_Info(Convert.ToInt32(AGV_No));
                                Main.CS_Work_DB.Select_MCS_Command_Info_Main(Convert.ToInt32(AGV_No));
                                Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

                                Main.m_stAGV[Convert.ToInt32(AGV_No)].dqGoal.Clear();
                                Main.m_stAGV[Convert.ToInt32(AGV_No)].dqWork.Clear();
                                Main.m_stAGV[Convert.ToInt32(AGV_No)].Working = 0;
                                Main.TB_Schedule[Convert.ToInt32(AGV_No)].Clear();
                                Main.m_stAGV[Convert.ToInt32(AGV_No)].MCS_Vehicle_Command_ID = "";


                                Main.TB_Schedule[Convert.ToInt32(AGV_No)].Clear();

                                Main.CS_AGV_Logic.Init_AGVInfo(Convert.ToInt32(AGV_No));
                                Main.m_stAGV[Convert.ToInt32(AGV_No)].Flag_Wait_Station = 1;

                                Main.CS_Work_DB.DELETE_DB_Recovery(Convert.ToInt32(AGV_No));

                                break;
                            }

                        }
                        break;

                    case 4:
                        ret = m_XComPro.SetListItem(lMsgId, 1);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);

                        if (CEID == 204)
                        {

                            Main.CS_Work_DB.Select_MCS_Command_Info_Log(Cmd_ID);

                            Main.CS_Work_DB.Delete_Work_Log(Cmd_ID);

                            //엑셀에 작업 로그 저장
                            Main.CS_Work_DB.Insert_Excel_Data_Log(Main.CS_AGV_Logic.E_Call_Time, Main.CS_AGV_Logic.E_Command_ID, Main.CS_AGV_Logic.E_Carrier_ID, Main.CS_AGV_Logic.E_Source_Port, Main.CS_AGV_Logic.E_Dest_Port, Main.CS_AGV_Logic.E_LGV_No, Main.CS_AGV_Logic.E_Alloc_Time, Main.CS_AGV_Logic.E_Load_Move_Time,
                                Main.CS_AGV_Logic.E_Load_Time, Main.CS_AGV_Logic.E_UnLoad_Move_Time, Main.CS_AGV_Logic.E_UnLoad_Time, "취소", Main.CS_AGV_Logic.E_Load_Move_Time_Result, Main.CS_AGV_Logic.E_Load_Time_Result, Main.CS_AGV_Logic.E_UnLoad_Move_Time_Result,
                                Main.CS_AGV_Logic.E_UnLoad_Time_Result, Main.CS_AGV_Logic.E_Total_Time_Result, Main.CS_AGV_Logic.E_Complete_Time,"");

                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(Convert.ToInt32(AGV_No));
                            Main.CS_Work_DB.Select_MCS_Command_Info(Convert.ToInt32(AGV_No));
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

                        }
                        break;

                    case 5:
                        ret = m_XComPro.SetListItem(lMsgId, 3);
                        ret = m_XComPro.SetListItem(lMsgId, 2);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[AGV_No].MCS_Vehicle_Priority));

                        ret = m_XComPro.SetListItem(lMsgId, 1);
                        ret = m_XComPro.SetListItem(lMsgId, 2);
                        ret = m_XComPro.SetListItem(lMsgId, 3);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_ID);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Source_Port);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Dest_Port);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_LOC);
                        
                        if (Main.m_stAGV[AGV_No].Working_Error == 0)
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, ResultCode);
                        }
                        else if (Main.m_stAGV[AGV_No].Working_Error == 1
                             && Main.m_stAGV[AGV_No].Goal == Convert.ToInt32(Main.m_stAGV[AGV_No].Source_Port_Num))
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 5);
                        }
                        else if (Main.m_stAGV[AGV_No].Working_Error == 1
                             && Main.m_stAGV[AGV_No].Goal == Convert.ToInt32(Main.m_stAGV[AGV_No].Dest_Port_Num))
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 6);
                        }


                        break;

                    case 6:
                        ret = m_XComPro.SetListItem(lMsgId, 5);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_ID);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_LOC);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[AGV_No].MCS_Carrier_Type));
                        break;

                    case 7:
                        //없음
                        break;

                    case 8:
                        CMD_Index = Main.Get_Command_Index(Cmd_ID);
                        if (CMD_Index != -1)
                        {
                            ret = m_XComPro.SetListItem(lMsgId, 7);

                            ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Command_Type);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Carrier_ID[CMD_Index]);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Source_Port[CMD_Index]);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.Form_MCS.MCS_Dest_Port[CMD_Index]);
                            ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.Form_MCS.MCS_PROIORITY[CMD_Index]));

                            ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(Main.Form_MCS.MCS_Carrier_Type[CMD_Index]));
                        }


                        break;

                    case 9:
                        ret = m_XComPro.SetListItem(lMsgId, 2);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);

                        if (CEID == 601)
                        {
                            if (Main.m_stAGV[AGV_No].MCS_Vehicle_Current_Position == Main.m_stAGV[AGV_No].Source_Port_Num)
                            {
                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Source_Port);
                            }
                            else if (Main.m_stAGV[AGV_No].MCS_Vehicle_Current_Position == Main.m_stAGV[AGV_No].Dest_Port_Num)
                            {
                                ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Dest_Port);
                            }
                        }
                        else if (CEID == 605)
                        {
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Dest_Port);
                        }

                        break;

                    case 10:
                        ret = m_XComPro.SetListItem(lMsgId, 3);

                        if (CEID == 606 || CEID == 607)
                        {
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Dest_Port);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_ID);
                        }
                        else if (CEID == 602 || CEID == 603)
                        {
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Source_Port);
                            ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Carrier_ID);
                        }
                        break;

                    case 11:

                        ret = m_XComPro.SetListItem(lMsgId, 2);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);
                        break;

                    case 12:
                        ret = m_XComPro.SetListItem(lMsgId, 1);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        break;

                    case 13:
                        ret = m_XComPro.SetListItem(lMsgId, 3);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        ret = m_XComPro.SetU4Item(lMsgId, Convert.ToUInt16(Main.m_stAGV[AGV_No].MCS_Alarm_ID));
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Alarm_Text);
                        break;

                    case 14:
                        ret = m_XComPro.SetListItem(lMsgId, 2);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Cmd_ID);
                        ret = m_XComPro.SetU2Item(lMsgId, Convert.ToUInt16(MCS_Priority));
                        if (CEID == 503)
                        {
                            Main.CS_Work_DB.Update_Command_Priority(Cmd_ID, Convert.ToString(MCS_Priority));
                        }

                        break;

                    case 15:
                        ret = m_XComPro.SetListItem(lMsgId, 2);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);
                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_Current_Position);
                        break;

                    case 16:

                        ret = m_XComPro.SetListItem(lMsgId, 2);

                        ret = m_XComPro.SetAsciiItem(lMsgId, Main.m_stAGV[AGV_No].MCS_Vehicle_ID);



                        if (Main.m_stAGV[AGV_No].connect == 1)
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 1);
                        }
                        else if (Main.m_stAGV[AGV_No].connect == 0)
                        {
                            ret = m_XComPro.SetU2Item(lMsgId, 2);
                        }


                        break;

                }

                ret = m_XComPro.Send(lMsgId);


                #region Return Values
                if (ret < 0)
                {
                    AddLog("Failed to send S6F11_Vehicle_EqpName: error={0}", ret);
                }
                else
                {
                    AddLog("S{0}F{1}_PRTID={2} was sent successfully", Stream, Fnc, RPTID);
                }

                #endregion

            }
            catch (Exception ex)
            {
                Main.Log("TryCatch sS6F11", Convert.ToString(ex) + "CEID :" + CEID + "AGV_NO :" + AGV_No);
            }
        }





        public void SendS2F17(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 2, 17, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S2F17: error={0}", ret);
            }
            else
            {
                AddLog("S2F17 was sent successfully");
            }
            #endregion
        }

        // S9 그룹
        public void SendS9F1(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 1, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F1: error={0}", ret);
            }
            else
            {
                AddLog("S9F1 was sent successfully");
            }
            #endregion
        }


        public void SendS9F3(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 3, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F3: error={0}", ret);
            }
            else
            {
                AddLog("S9F3 was sent successfully");
            }
            #endregion
        }

        public void SendS9F5(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 5, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F5: error={0}", ret);
            }
            else
            {
                AddLog("S9F5 was sent successfully");
            }
            #endregion
        }

        public void SendS9F7(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 7, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F5: error={0}", ret);
            }
            else
            {
                AddLog("S9F5 was sent successfully");
            }
            #endregion
        }

        public void SendS9F9(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 9, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F5: error={0}", ret);
            }
            else
            {
                AddLog("S9F5 was sent successfully");
            }
            #endregion
        }

        public void SendS9F11(short DeviceID)
        {
            int lMsgId = 0;

            int ret = m_XComPro.MakeSecsMsg(ref lMsgId, DeviceID, 9, 11, 0);
            #region Return Values
            if (ret < 0)
            {
                AddLog("MakeSecsMsg failed: error={0}", ret);
                return;
            }
            #endregion

            ret = m_XComPro.Send(lMsgId);
            #region Return Values
            if (ret < 0)
            {
                AddLog("Failed to send S9F5: error={0}", ret);
            }
            else
            {
                AddLog("S9F5 was sent successfully");
            }
            #endregion
        }

    }
}
