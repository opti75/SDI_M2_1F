using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public class Connect_TestLGV
    {
        Form1 Main;
        public Socket[] LGV_Sock;
        public byte[][] BufferData = new byte[Form1.LGV_NUM][];

        public byte[][] MSG_BufferData = new byte[Form1.LGV_NUM][];
        int[] SaveBuf = new int[Form1.LGV_NUM];
        public byte[][] BufDataRead = new byte[Form1.LGV_NUM][];
        public byte[][] BufDataSend = new byte[Form1.LGV_NUM][];
        public byte[][] BufDataSend_Link = new byte[Form1.LGV_NUM][];

        public Connect_TestLGV()
        {

        }
        public Connect_TestLGV(Form1 CS_Main)
        {
            Main = CS_Main;

            LGV_Sock = new Socket[Form1.LGV_NUM];

            //LGV_Sock

            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                LGV_Sock[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SaveBuf[i] = new int();
                BufferData[i] = new byte[300];
                BufDataRead[i] = new byte[5];
                BufDataSend[i] = new byte[5];
                BufDataSend_Link[i] = new byte[5];
            }

        }

        public void DataSend_Work_Duplocation(int a, int b)
        {

        }

        public void DataSendRequest_Shutter(int a, int b)
        {

        }

        public void DataSendRequest_AbortOK(int a, int b)
        {

        }

        public void DataSend_Work_Station_Source(int a, int b)
        {

        }

        public void DataSend_Work_Station_Dest(int a, int b)
        {

        }
        public void DataSendRequest_Link(int a, int b)
        {

        }

        

        public void Connect(int LGV_No, string IP, int Port)
        {
            IAsyncResult result;
            try
            {
                if (LGV_Sock[LGV_No].Connected == false)
                {
                    LGV_Sock[LGV_No] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint LGV_IP = new IPEndPoint(IPAddress.Parse(IP), Port);
                    result = LGV_Sock[LGV_No].BeginConnect(LGV_IP, new AsyncCallback(AGV_Connected), LGV_Sock[LGV_No]);
                    Main.m_stAGV[LGV_No].IP = IP + ":" + Port;

                }
            }
            catch (Exception ex)
            {
                //Main.Log("Try Catch_Connect", Convert.ToString(ex));
            }

            GC.Collect();
        }
        private void AGV_Connected(IAsyncResult IAR)
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            string IP = "";
            string IP_List = "";
            int LGV_NO = 0;
            try
            {
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (tempSock.Connected == true)
                    {
                        IP = Convert.ToString(tempSock.RemoteEndPoint);
                        IP_List = Main.CS_AGV_C_Info[i].IP + ":" + Main.CS_AGV_C_Info[i].Port;

                        if (IP_List == IP)
                        {
                            LGV_NO = i + 1;

                            if (LGV_Sock[i].Connected == true && tempSock.Connected == true)
                            {
                                Main.m_stAGV[i].connect = 1;
                                //Main.Log("AGV_" + (i + 1) + "Conncet", "AGV" + (i + 1) + "호 정보 :" + IP + "");
                                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                                {

                                }));
                                tempSock.EndConnect(IAR);
                                tempSock.BeginReceive(BufferData[i], 0, BufferData[i].Length, SocketFlags.None, new AsyncCallback(ReceiveData), LGV_Sock[i]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Main.Log("Try Catch_AGV_Connected", Convert.ToString(ex));
            }

        }

        //차량 접속 끊기
        public void LGV_Dis_Connect(int LGV_No)
        {
            try
            {
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    LGV_Sock[LGV_No].Shutdown(SocketShutdown.Both);
                    LGV_Sock[LGV_No].Close();
                    LGV_Sock[LGV_No].Dispose();

                    SaveBuf[LGV_No] = new int();
                    BufferData[LGV_No] = new byte[500];
                    MSG_BufferData[LGV_No] = new byte[500];
                    BufDataRead[LGV_No] = new byte[21];


                    BufDataSend_Link[LGV_No] = new byte[25];


                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        Main.GB_LGV_Info[LGV_No].ForeColor = System.Drawing.Color.Red;
                        Main.m_stAGV[LGV_No].connect = 0;
                        //Main.CS_DRAW_INFO.m_ImageAGV[LGV_No].Location = new System.Drawing.Point(20, 20);

                    }));
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_LGV_Dis_Connect", Convert.ToString(ex));
            }

            GC.Collect();
        }

        //트래픽 전송
        public void DataSendRequest_Traffic(int LGV_No)
        {
            string log;
            try
            {
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC WriteData 요청
                    BufDataSend[LGV_No][0] = 0x02;
                    BufDataSend[LGV_No][1] = (byte)'S';

                    //BufDataSend[LGV_No][2] = Convert.ToByte((Main.Send_Traffic[LGV_No] >> 8) & 0xff);
                  //  BufDataSend[LGV_No][3] = Convert.ToByte(Main.Send_Traffic[LGV_No] & 0xff);

                    RequestAGVDataCommunication(2, LGV_No);

                    #endregion

                }
            }
            catch (Exception ex)
            {
                // Main.Log("Try Catch_DataSendRequest_Traffic", Convert.ToString(ex));
            }
        }

        //차량 정보 받기
        public void ReceiveData(IAsyncResult IAR)
        {
            string log;
            int tmp;
            string temp = "";
            string IP = "";
            string IP_List = "";
            int bufferSize = 0;

            Socket Receive_Socket = (Socket)IAR.AsyncState;

            try
            {
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (Receive_Socket.Connected == true)
                    {
                        IP = Convert.ToString(Receive_Socket.RemoteEndPoint);
                        IP_List = Main.m_stAGV[i].IP;
                        {
                            if (IP_List == IP)
                            {
                                if (LGV_Sock[i].Connected == true)
                                {
                                    bufferSize = Receive_Socket.EndReceive(IAR);
                                    //if (BufferData[i][7] == Data_Length_L && BufferData[i][8] == Data_Length_H)
                                    {
                                        #region 데이터 파싱!

                                        Main.CS_AGV_Logic.LGVReadData[i, 0] = BufferData[i][11];  //Current    
                                        Main.CS_AGV_Logic.LGVReadData[i, 1] = BufferData[i][12];  //Current    

                                        Main.CS_AGV_Logic.LGVReadData[i, 2] = BufferData[i][13];  //Goal    
                                        Main.CS_AGV_Logic.LGVReadData[i, 3] = BufferData[i][14];  //Goal    

                                        Main.CS_AGV_Logic.LGVReadData[i, 4] = BufferData[i][15];  //Mode    
                                        Main.CS_AGV_Logic.LGVReadData[i, 5] = BufferData[i][16];  //Mode

                                        Main.CS_AGV_Logic.LGVReadData[i, 6] = BufferData[i][17];  //State   
                                        Main.CS_AGV_Logic.LGVReadData[i, 7] = BufferData[i][18];  //State

                                        Main.CS_AGV_Logic.LGVReadData[i, 8] = BufferData[i][19];  //Error
                                        Main.CS_AGV_Logic.LGVReadData[i, 9] = BufferData[i][20];  //Error

                                        Main.CS_AGV_Logic.LGVReadData[i, 10] = BufferData[i][21];  //x
                                        Main.CS_AGV_Logic.LGVReadData[i, 11] = BufferData[i][22];  //x
                                        Main.CS_AGV_Logic.LGVReadData[i, 12] = BufferData[i][23];  //x
                                        Main.CS_AGV_Logic.LGVReadData[i, 13] = BufferData[i][24];  //x

                                        Main.CS_AGV_Logic.LGVReadData[i, 14] = BufferData[i][25];  //y
                                        Main.CS_AGV_Logic.LGVReadData[i, 15] = BufferData[i][26];  //y
                                        Main.CS_AGV_Logic.LGVReadData[i, 16] = BufferData[i][27];  //y
                                        Main.CS_AGV_Logic.LGVReadData[i, 17] = BufferData[i][28];  //y

                                        Main.CS_AGV_Logic.LGVReadData[i, 18] = BufferData[i][29];  //t
                                        Main.CS_AGV_Logic.LGVReadData[i, 19] = BufferData[i][30];  //t
                                        Main.CS_AGV_Logic.LGVReadData[i, 20] = BufferData[i][31];  //t
                                        Main.CS_AGV_Logic.LGVReadData[i, 21] = BufferData[i][32];  //t

                                        Main.CS_AGV_Logic.LGVReadData[i, 22] = BufferData[i][33];  //배터리
                                        Main.CS_AGV_Logic.LGVReadData[i, 23] = BufferData[i][34];  //배터리


                                        Main.CS_AGV_Logic.Test_LGV_Data(i);
                                        Receive_Socket.BeginReceive(BufferData[i], 0, BufferData[i].Length, SocketFlags.None, ReceiveData, Receive_Socket);

                                        Main.Flag_ReReceive[i] = 0;
                                        #endregion
                                    }
                                    //else
                                    {
                                        //Main.CS_AGV_Logic.LGV_Data(i);
                                        Receive_Socket.BeginReceive(BufferData[i], 0, BufferData[i].Length, SocketFlags.None, ReceiveData, Receive_Socket);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_ReceiveData", Convert.ToString(ex));
            }
        }

        public void DataReadBufferRequest(int LGV_No)
        {
            string log;
            //Main.T_ReadReQuest[LGV_No].Enabled = false;
            try
            {
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    BufDataRead[LGV_No][0] = 0x02;
                    BufDataRead[LGV_No][1] = (byte)'A';
                    BufDataRead[LGV_No][2] = 0;
                    BufDataRead[LGV_No][3] = 0;
                    RequestAGVDataCommunication(1, LGV_No);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_DataReadBufferRequest", Convert.ToString(ex));
            }
        }

        public void DataSendRequest(int LGV_No)
        {
            string log;
            try
            {
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC WriteData 요청
                    BufDataSend[LGV_No][0] = 0x02;
                    BufDataSend[LGV_No][1] = (byte)'O';
                    BufDataSend[LGV_No][2] = Convert.ToByte((Main.CS_AGV_Logic.LGVSendData[1] >> 8) & 0xff);
                    BufDataSend[LGV_No][3] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData[1] & 0xff);  // 목표 노드
                    BufDataSend[LGV_No][4] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData[2]);  // 작업 종류

                    RequestAGVDataCommunication(2, LGV_No);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch DataSendRequest", Convert.ToString(ex));
            }
        }
        public void RequestAGVDataCommunication(int type, int LGV_No)
        {
            try
            {
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    if (type == 1)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataRead[LGV_No], 0, BufDataRead[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);

                    }
                    else if (type == 2)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend[LGV_No], 0, BufDataSend[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);

                    }

                    else if (type == 3)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Link[LGV_No], 0, BufDataSend_Link[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);

                    }

                }

            }
            catch (SocketException ex)
            {

            }
        }
        private void RequestAGVDataCommunicationCallBack(IAsyncResult IAR)
        {
            string log;
            string IP = "";
            string IP_List = "";

            Socket Send_Socket = (Socket)IAR.AsyncState;

            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (Send_Socket.Connected == true)
                {
                    IP = Convert.ToString(Send_Socket.RemoteEndPoint);
                    IP_List = Main.m_stAGV[i].IP;
                    if (IP_List == IP)
                    {
                        if (LGV_Sock[i].Connected == true)
                        {
                            Send_Socket.EndReceive(IAR);
                        }
                    }
                }
            }
        }
    }
}
