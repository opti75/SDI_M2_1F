using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SDI_LCS
{
    public class Connect_LGV
    {
        Form1 Main;
        string Read_PLC = "D005000";
        char[] Read_Device;

        string Write_PLC = "D006000";
        char[] Write_Device;

        string Write_PLC_Link = "D006007";
        char[] Write_Device_Link;

        string Write_PLC_Traffic = "D006010";
        char[] Write_Device_Traffic;

        string Write_PLC_Enter = "D006011";
        char[] Write_Device_Enter;

        string Write_PLC_Abort = "D006005";
        char[] Write_Device_Abort;

        string Write_PLC_Work_Station_Source = "D006020";
        char[] Write_DeviceWork_Station_Source;

        string Write_PLC_Work_Station_Dest = "D006021";
        char[] Write_DeviceWork_Station_Dest;

        // AGV 이중입고 시 알람 전송. lkw20190129
        string Write_PLC_Duplication = "D006022";
        char[] Write_DeviceWork_Duplication;



        #region 버퍼 배열 초기화
        //2
        public byte[][] BufDataSend_Work = new byte[Form1.LGV_NUM][];
        //3
        public byte[][] BufDataSend_Traffic = new byte[Form1.LGV_NUM][];
        //4
        public byte[][] BufDataSend_Link = new byte[Form1.LGV_NUM][];
        //5
        public byte[][] BufDataSend_Enter = new byte[Form1.LGV_NUM][];
        //6
        public byte[][] BufDataSend_Abort = new byte[Form1.LGV_NUM][];
        //7
        public byte[][] BufDataSend_Source = new byte[Form1.LGV_NUM][];
        //8
        public byte[][] BufDataSend_Dest = new byte[Form1.LGV_NUM][];
        //9 AGV 광폭 기재랙 이중입고 데이터 전송. lkw20190129
        public byte[][] BufDataSend_Duplication = new byte[Form1.LGV_NUM][];

        //차량이 보내주는 데이터
        public byte[][] BufferData = new byte[Form1.LGV_NUM][];
        //차량이 보내주는 데이터 - 변환
        public byte[][] Change_BufferData = new byte[Form1.LGV_NUM][];
        //차량이 보내주는 데이터 저장
        public byte[][] MSG_BufferData = new byte[Form1.LGV_NUM][];
        //차량이 보낸 데이터 개수 저장
        int[] SaveBuf = new int[Form1.LGV_NUM];
        //상태 요청 -> BufferData에 값들어옴
        public byte[][] BufDataRead = new byte[Form1.LGV_NUM][];


        //작업 전송 - 데이터 저장
        public byte[][] Save_BufDataSend = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_Link = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_Traffic = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_Enter = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_Abort = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_WorkStation_Source = new byte[Form1.LGV_NUM][];
        public byte[][] Save_BufDataSend_WorkStation_Dest = new byte[Form1.LGV_NUM][];
        // AGV 광폭 기재랙 이중입고 데이터 전송. lkw20190129
        public byte[][] Save_BufDataSend_WorkStation_Duplication = new byte[Form1.LGV_NUM][];
        int temp = 0;
        int Data_Length_L = 0;
        int Data_Length_H = 0;

        public static ManualResetEvent[] Conn_Done = new ManualResetEvent[Form1.LGV_NUM];
        public static ManualResetEvent[] Receive_Done = new ManualResetEvent[Form1.LGV_NUM];
        public static ManualResetEvent[] Send_Done = new ManualResetEvent[Form1.LGV_NUM];
        public static ManualResetEvent[] DisConnect_Done = new ManualResetEvent[Form1.LGV_NUM];
        #endregion
        //LGV소켓
        public Socket[] LGV_Sock;
        //생성자
        public Connect_LGV()
        {

        }
        //생성자
        public Connect_LGV(Form1 CS_Main)
        {
            Read_Device = Read_PLC.ToCharArray();
            Write_Device = Write_PLC.ToCharArray();
            Write_Device_Link = Write_PLC_Link.ToCharArray();
            Write_Device_Traffic = Write_PLC_Traffic.ToCharArray();
            Write_Device_Enter = Write_PLC_Enter.ToCharArray();
            Write_Device_Abort = Write_PLC_Abort.ToCharArray();
            Write_DeviceWork_Station_Source = Write_PLC_Work_Station_Source.ToCharArray();
            Write_DeviceWork_Station_Dest = Write_PLC_Work_Station_Dest.ToCharArray();
            Write_DeviceWork_Duplication = Write_PLC_Duplication.ToCharArray();
            Main = CS_Main;
            
            LGV_Sock = new Socket[Form1.LGV_NUM];

            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                SaveBuf[i] = new int();
                BufferData[i] = new byte[500];
                MSG_BufferData[i] = new byte[500];
                BufDataRead[i] = new byte[21];

                BufDataSend_Work[i] = new byte[37];
                BufDataSend_Traffic[i] = new byte[25];
                BufDataSend_Link[i] = new byte[25];
                BufDataSend_Enter[i] = new byte[25];
                BufDataSend_Source[i] = new byte[25];
                BufDataSend_Dest[i] = new byte[25];
                // AGV 광폭 기재랙 이중입고 알람 전송. lkw20190129
                BufDataSend_Duplication[i] = new byte[25];
                BufDataSend_Abort[i] = new byte[25];

                Change_BufferData[i] = new byte[500];
                Save_BufDataSend[i] = new byte[20];
                Save_BufDataSend_Link[i] = new byte[20];
                Save_BufDataSend_Traffic[i] = new byte[20];
                Save_BufDataSend_Enter[i] = new byte[20];
                Save_BufDataSend_Abort[i] = new byte[20];
                Save_BufDataSend_WorkStation_Source[i] = new byte[20];
                Save_BufDataSend_WorkStation_Dest[i] = new byte[20];
                // AGV 광폭 기재랙 이중입고 알람 전송. lkw20190129
                Save_BufDataSend_WorkStation_Duplication[i] = new byte[20];

                Conn_Done[i] = new ManualResetEvent(false);
                Receive_Done[i] = new ManualResetEvent(false);
                Send_Done[i] = new ManualResetEvent(false);
                DisConnect_Done[i] = new ManualResetEvent(false);
            }

            //LGV_Sock
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                LGV_Sock[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            
        }
        //차량 접속
        public void Connect(int LGV_No, string IP,int Port)
        {
            IAsyncResult result;
            try
            {
                if(LGV_Sock[LGV_No].Connected == false && Main.Flag_MCS_Auto == 1)
                {
                    LGV_Sock[LGV_No] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint LGV_IP = new IPEndPoint(IPAddress.Parse(IP), Port);
                    result = LGV_Sock[LGV_No].BeginConnect(LGV_IP, new AsyncCallback(AGV_Connected), LGV_Sock[LGV_No]);
                    Main.m_stAGV[LGV_No].IP = Convert.ToString(LGV_IP);
                    Conn_Done[LGV_No].WaitOne(10);
                }
                else
                {
                    if (LGV_No != -1)
                    {
                        LGV_Dis_Connect(LGV_No);
                        Main.Log("접속 끊기", "라인 173 = " + (LGV_No + 1) + "호");
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Connect", Convert.ToString(ex));
            }
        }

        //차량 접속 됐을때
        private void AGV_Connected(IAsyncResult IAR)
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            string IP = "";
            string IP_List = "";
            int idx = -1;
            try
            {
                if(tempSock.Connected == true)
                {
                    for (int LGV_No = 0; LGV_No < Form1.LGV_NUM; LGV_No++)
                    {
                        //접속된 차량 ip따오기
                        IP = Convert.ToString(tempSock.RemoteEndPoint);
                        //차량 통신 클래스에 있는 ip따오기
                        IP_List = Main.CS_AGV_C_Info[LGV_No].IP + ":" + Main.CS_AGV_C_Info[LGV_No].Port;
                        if (IP_List == IP)
                        {
                            idx = LGV_No;
                            break;
                        }
                    }
                }
               
                if (tempSock.Connected)
                {
                    if(idx != -1)
                    {
                        if (LGV_Sock[idx].Connected == true)
                        {
                            //차량 접속 플래그
                            Main.m_stAGV[idx].connect = 1;
                            Main.CS_Work_DB.Update_LGV_Info_View_LCS_Connect(idx, "1");

                            Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                            {
                                if (idx == 0)
                                {
                                    Main.p_LGV01.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 1)
                                {
                                    Main.P_LGV02.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 2)
                                {
                                    Main.P_LGV03.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 3)
                                {
                                    Main.P_LGV04.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 4)
                                {
                                    Main.P_LGV05.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 5)
                                {
                                    Main.P_LGV06.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 6)
                                {
                                    Main.P_LGV07.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 7)
                                {
                                    Main.P_LGV08.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 8)
                                {
                                    Main.P_LGV09.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 9)
                                {
                                    Main.panel10.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 10)
                                {
                                    Main.panel11.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 11)
                                {
                                    Main.P_LGV12.BackColor = System.Drawing.Color.Lime;
                                }
                                else if (idx == 12)
                                {
                                    Main.P_LGV13.BackColor = System.Drawing.Color.Lime;
                                }
                                LGV_Sock[idx].BeginReceive(BufferData[idx], 0, BufferData[idx].Length, SocketFlags.None,
                                new AsyncCallback(ReceiveData), LGV_Sock[idx]);
                                Receive_Done[idx].WaitOne(10);

                                Main.Log("AGV_Connect_Info", (idx + 1) + "호 접속");

                                //메인폼에 접속 상태 표시해주기
                                Main.GB_LGV_Info[idx].ForeColor = System.Drawing.Color.Lime;

                            }));
                            tempSock.EndConnect(IAR);
                            Conn_Done[idx].Set();
                            return;
                        }
                        if (idx != -1)
                        {
                            LGV_Dis_Connect(idx);
                            Main.Log("접속 끊기", "라인 272 = " + (idx + 1) + "호");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_Connected", Convert.ToString(ex));

                if (idx != -1)
                {
                    LGV_Dis_Connect(idx);
                    Main.Log("접속 끊기", "라인 284 = "+(idx+1)+"호");
                }
            }
        }
        //중단 완료 보내기
        public void DataSendRequest_AbortOK(int LGV_No, int Ok)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //short RMP_Length = 1;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Abort[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Abort[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Abort[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Abort[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Abort[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Abort[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Abort[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Abort[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Abort[LGV_No][8] = Convert.ToByte(Write_Device_Abort[0]);
                    BufDataSend_Abort[LGV_No][9] = Convert.ToByte(Write_Device_Abort[1]);
                    BufDataSend_Abort[LGV_No][10] = Convert.ToByte(Write_Device_Abort[2]);
                    BufDataSend_Abort[LGV_No][11] = Convert.ToByte(Write_Device_Abort[3]);
                    BufDataSend_Abort[LGV_No][12] = Convert.ToByte(Write_Device_Abort[4]);
                    BufDataSend_Abort[LGV_No][13] = Convert.ToByte(Write_Device_Abort[5]);
                    BufDataSend_Abort[LGV_No][14] = Convert.ToByte(Write_Device_Abort[6]);

                    BufDataSend_Abort[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Abort[LGV_No][16] = Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_Abort[LGV_No][0] = 0x00;
                    Save_BufDataSend_Abort[LGV_No][1] = Convert.ToByte(Ok); // READ OK
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Abort[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_Abort[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Abort[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_Abort[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Abort[LGV_No][i];
                    }

                    BufDataSend_Abort[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Abort[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Abort[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Abort[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(6, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSendRequest_AbortOK", Convert.ToString(ex));
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
                    LGV_Sock[LGV_No].BeginDisconnect(true, new AsyncCallback(DisconnectCallback), LGV_Sock[LGV_No]);
                    DisConnect_Done[LGV_No].WaitOne(10);
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_LGV_Dis_Connect", Convert.ToString(ex));
            }
        }

        private void DisconnectCallback(IAsyncResult IAR)
        {
            try
            {
                string IP = "";
                string IP_List = "";
                int idx = -1;
                Socket tempSock = (Socket)IAR.AsyncState;

                if(tempSock.Connected == true)
                {
                    for (int LGV_No = 0; LGV_No < Form1.LGV_NUM; LGV_No++)
                    {
                        //접속된 차량 ip따오기
                        IP = Convert.ToString(tempSock.RemoteEndPoint);
                        //차량 통신 클래스에 있는 ip따오기
                        IP_List = Main.CS_AGV_C_Info[LGV_No].IP + ":" + Main.CS_AGV_C_Info[LGV_No].Port;
                        if (IP_List == IP)
                        {
                            idx = LGV_No;
                            break;
                        }
                    }
                }
                
                if(idx != -1)
                {
                    LGV_Sock[idx].Close();
                    LGV_Sock[idx].Dispose();

                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        if (idx == 0)
                        {
                            Main.p_LGV01.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 1)
                        {
                            Main.P_LGV02.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 2)
                        {
                            Main.P_LGV03.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 3)
                        {
                            Main.P_LGV04.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 4)
                        {
                            Main.P_LGV05.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 5)
                        {
                            Main.P_LGV06.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 6)
                        {
                            Main.P_LGV07.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 7)
                        {
                            Main.P_LGV08.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 8)
                        {
                            Main.P_LGV09.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 9)
                        {
                            Main.panel10.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 10)
                        {
                            Main.panel11.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 11)
                        {
                            Main.P_LGV12.BackColor = System.Drawing.Color.Red;
                        }
                        else if (idx == 12)
                        {
                            Main.P_LGV13.BackColor = System.Drawing.Color.Red;
                        }

                        Main.Log("AGV_Connect_Info", (idx + 1) + "호 끊김");
                        Main.GB_LGV_Info[idx].ForeColor = System.Drawing.Color.Red;
                        Main.m_stAGV[idx].connect = 0;
                        Main.CS_Work_DB.Update_LGV_Info_View_LCS_Connect(idx, "0");
                        Main.CS_Draw_Node_AGV.ImageAGV[idx].Location = new System.Drawing.Point(20, 20);

                    }));
                    LGV_Sock[idx].EndDisconnect(IAR);
                    DisConnect_Done[idx].Set();
                } 
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch DisconnectCallback", Convert.ToString(ex));
            }
        }

        //BCC 계산
        public Byte MakeBCC(Byte[] RxBuf, int RxCount)
        {
            int i;
            Byte CRC = 0;
            for (i = 0; i < RxCount; i++)
            {
                if (i == 0) CRC = RxBuf[i];
                else if (i != RxCount - 3) CRC ^= RxBuf[i];
            }
            return CRC;
        }

        public char Hex2Char(char InHex)
        {
            char OutChar = '0';

            InHex &= Convert.ToChar(0x0f);              // 하위 4비트만 사용

            if (InHex >= 10)
            {
                OutChar = Convert.ToChar(InHex - 10 + 'A');
            }
            else
            {
                OutChar = Convert.ToChar(InHex + '0');
            }
            return OutChar;
        }
        //AGV 상태 요청
        public void DataReadBufferRequest(int LGV_No)
        {
            try
            {
                //int CheckSum = 0;

                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청 
                    short ChkSum = 0;

                    //------------------------- 서브헤더
                    BufDataRead[LGV_No][0] = 0x05;
                    //------------------------- 국번
                    BufDataRead[LGV_No][1] = Convert.ToByte('0');   
                    BufDataRead[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataRead[LGV_No][3] = Convert.ToByte('F');
                    BufDataRead[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataRead[LGV_No][5] = Convert.ToByte('Q');
                    BufDataRead[LGV_No][6] = Convert.ToByte('R');
                    //------------------------- 스테이먼트 대기
                    BufDataRead[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 선두 디바이스
                    BufDataRead[LGV_No][8] = Convert.ToByte(Read_Device[0]);
                    BufDataRead[LGV_No][9] = Convert.ToByte(Read_Device[1]);
                    BufDataRead[LGV_No][10] = Convert.ToByte(Read_Device[2]);
                    BufDataRead[LGV_No][11] = Convert.ToByte(Read_Device[3]);
                    BufDataRead[LGV_No][12] = Convert.ToByte(Read_Device[4]);
                    BufDataRead[LGV_No][13] = Convert.ToByte(Read_Device[5]);
                    BufDataRead[LGV_No][14] = Convert.ToByte(Read_Device[6]);
                    //------------------------- 디바이스 점수
                    BufDataRead[LGV_No][15] = Convert.ToByte('1');
                    BufDataRead[LGV_No][16] = Convert.ToByte('C');

                    for (int i = 1; i < 17; i++)
                    {
                        ChkSum += Convert.ToInt16(BufDataRead[LGV_No][i]);
                    }

                    BufDataRead[LGV_No][17] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufDataRead[LGV_No][18] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));

                    BufDataRead[LGV_No][19] = 0x0d;         // DLE
                    BufDataRead[LGV_No][20] = 0x0a;         // ETX

                    RequestAGVDataCommunication(1, LGV_No);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_DataReadBufferRequest", Convert.ToString(ex));
            }
        }
        public void ArrangeInData(int idx)
        {
            try
            {
                #region 데이터 파싱!
                Main.CS_AGV_Logic.LGVReadData[idx, 0] = Change_BufferData[idx][0];       // AGV->RCS : X축
                Main.CS_AGV_Logic.LGVReadData[idx, 1] = Change_BufferData[idx][1];       // AGV->RCS : X축
                Main.CS_AGV_Logic.LGVReadData[idx, 2] = Change_BufferData[idx][2];       // AGV->RCS : X축
                Main.CS_AGV_Logic.LGVReadData[idx, 3] = Change_BufferData[idx][3];       // AGV->RCS : X축

                Main.CS_AGV_Logic.LGVReadData[idx, 4] = Change_BufferData[idx][4];       // AGV->RCS : Y축
                Main.CS_AGV_Logic.LGVReadData[idx, 5] = Change_BufferData[idx][5];       // AGV->RCS : Y축
                Main.CS_AGV_Logic.LGVReadData[idx, 6] = Change_BufferData[idx][6];       // AGV->RCS : Y축
                Main.CS_AGV_Logic.LGVReadData[idx, 7] = Change_BufferData[idx][7];       // AGV->RCS : Y축

                Main.CS_AGV_Logic.LGVReadData[idx, 8] = Change_BufferData[idx][8];       // AGV->RCS : 각도
                Main.CS_AGV_Logic.LGVReadData[idx, 9] = Change_BufferData[idx][9];       // AGV->RCS : 각도
                Main.CS_AGV_Logic.LGVReadData[idx, 10] = Change_BufferData[idx][10];      // AGV->RCS : 각도 
                Main.CS_AGV_Logic.LGVReadData[idx, 11] = Change_BufferData[idx][11];      // AGV->RCS : 각도

                Main.CS_AGV_Logic.LGVReadData[idx, 12] = Change_BufferData[idx][12];  //Current    
                Main.CS_AGV_Logic.LGVReadData[idx, 13] = Change_BufferData[idx][13];  //Current    

                Main.CS_AGV_Logic.LGVReadData[idx, 14] = Change_BufferData[idx][14];  //Goal    
                Main.CS_AGV_Logic.LGVReadData[idx, 15] = Change_BufferData[idx][15];  //Goal    

                Main.CS_AGV_Logic.LGVReadData[idx, 16] = Change_BufferData[idx][16];  //Target    
                Main.CS_AGV_Logic.LGVReadData[idx, 17] = Change_BufferData[idx][17];  //Target  

                Main.CS_AGV_Logic.LGVReadData[idx, 18] = Change_BufferData[idx][18];  //State   
                Main.CS_AGV_Logic.LGVReadData[idx, 19] = Change_BufferData[idx][19];  //State

                Main.CS_AGV_Logic.LGVReadData[idx, 20] = Change_BufferData[idx][20];  //Error
                Main.CS_AGV_Logic.LGVReadData[idx, 21] = Change_BufferData[idx][21];  //Error

                Main.CS_AGV_Logic.LGVReadData[idx, 22] = Change_BufferData[idx][22];  //Mode
                Main.CS_AGV_Logic.LGVReadData[idx, 23] = Change_BufferData[idx][23];  //Mode

                Main.CS_AGV_Logic.LGVReadData[idx, 24] = Change_BufferData[idx][24];  //Battery
                Main.CS_AGV_Logic.LGVReadData[idx, 25] = Change_BufferData[idx][25];  //Battery

                Main.CS_AGV_Logic.LGVReadData[idx, 26] = Change_BufferData[idx][26];  //제품 유/무
                Main.CS_AGV_Logic.LGVReadData[idx, 27] = Change_BufferData[idx][27];  //제품 유/무

                Main.CS_AGV_Logic.LGVReadData[idx, 28] = Change_BufferData[idx][28];  //PIO
                Main.CS_AGV_Logic.LGVReadData[idx, 29] = Change_BufferData[idx][29];  //이적재 에러

                Main.CS_AGV_Logic.LGVReadData[idx, 30] = Change_BufferData[idx][30];  //ASK ABORT
                Main.CS_AGV_Logic.LGVReadData[idx, 31] = Change_BufferData[idx][31];  //ASK ABORT

                Main.CS_AGV_Logic.LGVReadData[idx, 32] = Change_BufferData[idx][32];  //트래픽 무시
                Main.CS_AGV_Logic.LGVReadData[idx, 33] = Change_BufferData[idx][33];  //트래픽 무시

                Main.CS_AGV_Logic.LGVReadData[idx, 34] = Change_BufferData[idx][34];  //리프트 높이
                Main.CS_AGV_Logic.LGVReadData[idx, 35] = Change_BufferData[idx][35];  //리프트 높이

                Main.CS_AGV_Logic.LGVReadData[idx, 36] = Change_BufferData[idx][36];  //전방 조향 각도
                Main.CS_AGV_Logic.LGVReadData[idx, 37] = Change_BufferData[idx][37];  //전방 조향 각도

                Main.CS_AGV_Logic.LGVReadData[idx, 38] = Change_BufferData[idx][38];  //후방 조향 각도
                Main.CS_AGV_Logic.LGVReadData[idx, 39] = Change_BufferData[idx][39];  //후방 조향 각도

                Main.CS_AGV_Logic.LGVReadData[idx, 40] = Change_BufferData[idx][40];  //D5020 주행 방향
                Main.CS_AGV_Logic.LGVReadData[idx, 41] = Change_BufferData[idx][41];  //D5020 주행 방향

                Main.CS_AGV_Logic.LGVReadData[idx, 42] = Change_BufferData[idx][42];  //D5021 Error_01
                Main.CS_AGV_Logic.LGVReadData[idx, 43] = Change_BufferData[idx][43];  //D5021 Error_01

                Main.CS_AGV_Logic.LGVReadData[idx, 44] = Change_BufferData[idx][44];  //D5021 Error_02
                Main.CS_AGV_Logic.LGVReadData[idx, 45] = Change_BufferData[idx][45];  //D5021 Error_02

                Main.CS_AGV_Logic.LGVReadData[idx, 46] = Change_BufferData[idx][46];  //D5021 Error_03
                Main.CS_AGV_Logic.LGVReadData[idx, 47] = Change_BufferData[idx][47];  //D5021 Error_03

                #endregion
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_ArrangeInData", Convert.ToString(ex));
            }
        }
        char Char2Hex(char InChar)
        {
            char OutHex = '0';

            if (InChar >= 'A')
            {
                // A~F 일때
                OutHex =  Convert.ToChar(InChar - 'A' + 10);
            }
            else
            {
                OutHex = Convert.ToChar(InChar - '0');
            }

            return OutHex;
        }

        public void USART_2_Parse(int idx, int Count)
        {
            try
            {
                int i;
                int ChkSum = 0;
                char ChkSum_H = '0';
                char ChkSum_L = '0';
                int DataIndex = 0;
                int OddEven = 0;

                for (i = 1; i < Count - 4; i++)
                {
                    ChkSum += MSG_BufferData[idx][i];
                }

                ChkSum_H = Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4));
                ChkSum_L = Hex2Char(Convert.ToChar((ChkSum & 0xff)));

                // 체크섬 확인
                if (ChkSum_H != MSG_BufferData[idx][Count - 4] || ChkSum_L != MSG_BufferData[idx][Count - 3])
                {
                    LGV_Sock[idx].BeginReceive(BufferData[idx], 0, BufferData[idx].Length, SocketFlags.None,
                            new AsyncCallback(ReceiveData), LGV_Sock[idx]);

                    return;
                }
                //
                Main.Send_Link[idx] = 1;

                Change_BufferData[idx].Initialize();

                for (i = 5; i < Count - 5; i++)
                {
                    if (OddEven == 0)
                    {   // 상위
                        Change_BufferData[idx][DataIndex] = Convert.ToByte((Char2Hex(Convert.ToChar(MSG_BufferData[idx][i])) << 4));
                        OddEven = 1;
                    }
                    else
                    {
                        //하위
                        Change_BufferData[idx][DataIndex] |= Convert.ToByte(Char2Hex(Convert.ToChar(MSG_BufferData[idx][i])));
                        DataIndex++;

                        OddEven = 0;
                    }
                }
                ArrangeInData(idx);
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_USART_2_Parse", Convert.ToString(ex));
            }
        }

        //차량 정보 받기
        public void ReceiveData(IAsyncResult IAR)
        {
            string IP = "";
            string IP_List = "";
            int bufferSize = 0;
            string Save_Buffer = "";
            int idx = -1;

            Socket Receive_Socket = (Socket)IAR.AsyncState;

            try
            {
                if(Receive_Socket.Connected == true)
                {
                    for (int i = 0; i < Form1.LGV_NUM; i++)
                    {
                        IP = Convert.ToString(Receive_Socket.RemoteEndPoint);
                        IP_List = Main.m_stAGV[i].IP;
                        if (IP_List == IP)
                        {
                            idx = i;
                            break;
                        }
                    }
                }
                
                if (idx != -1)
                {
                    if (LGV_Sock[idx].Connected == true)
                    {
                        bufferSize = LGV_Sock[idx].EndReceive(IAR);

                        if (BufferData[idx][0] == 0x02
                        && BufferData[idx][1] == '0'
                        && BufferData[idx][2] == '0'
                        && BufferData[idx][3] == 'F'
                        && BufferData[idx][4] == 'F')
                        {
                            SaveBuf[idx] = 0;
                        }
                        if (BufferData[idx][0] == 0x15
                              && BufferData[idx][1] == '0'
                              && BufferData[idx][2] == '0'
                              && BufferData[idx][3] == 'F'
                              && BufferData[idx][4] == 'F')
                        {
                            SaveBuf[idx] = 0;
                        }
                        if (BufferData[idx][0] == 0x21
                              && BufferData[idx][1] == '0'
                              && BufferData[idx][2] == '0'
                              && BufferData[idx][3] == 'F'
                              && BufferData[idx][4] == 'F')
                        {
                            SaveBuf[idx] = 0;
                        }
                        if (BufferData[idx][0] == 0x06
                             && BufferData[idx][1] == '0'
                             && BufferData[idx][2] == '0'
                             && BufferData[idx][3] == 'F'
                             && BufferData[idx][4] == 'F')
                        {
                            SaveBuf[idx] = 0;
                        }

                        for (int Buf_Count = 0; Buf_Count < bufferSize; Buf_Count++)
                        {
                            MSG_BufferData[idx][SaveBuf[idx]] = BufferData[idx][Buf_Count];
                            SaveBuf[idx]++;

                            if (MSG_BufferData[idx][120] == 0x0D && MSG_BufferData[idx][121] == 0x0A && SaveBuf[idx] >= 122)
                            {
                                break;
                            }
                        }

                        if (MSG_BufferData[idx][0] == 2
                         && MSG_BufferData[idx][120] == 0x0D && MSG_BufferData[idx][121] == 0x0A && SaveBuf[idx] >= 122)
                        {
                            USART_2_Parse(idx, SaveBuf[idx]);
                            SaveBuf[idx] = 0;

                            Main.CS_AGV_Logic.LGV_Data(idx);
                            Main.Flag_ReReceive[idx] = 0;

                            Main.Flag_ReReceive[idx] = 0;
                            if (Receive_Socket.Connected == true)
                            {
                                BufferData[idx].Initialize();
                                MSG_BufferData[idx].Initialize();
                                LGV_Sock[idx].BeginReceive(BufferData[idx], 0, BufferData[idx].Length, SocketFlags.None,
                                new AsyncCallback(ReceiveData), LGV_Sock[idx]);
                            }
                        }
                        else
                        {
                            Main.Flag_ReReceive[idx] = 0;
                            if (Receive_Socket.Connected == true)
                            {
                                BufferData[idx].Initialize();
                                LGV_Sock[idx].BeginReceive(BufferData[idx], 0, BufferData[idx].Length, SocketFlags.None,
                                new AsyncCallback(ReceiveData), LGV_Sock[idx]);
                            }
                        }
                        if (SaveBuf[idx] > 200)
                        {
                            SaveBuf[idx] = 0;
                        }
                    }
                    Receive_Done[idx].Set();
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_ReceiveData", Convert.ToString(ex));
                if (idx != -1)
                {
                    LGV_Dis_Connect(idx);
                    Main.Log("접속 끊기", "라인 840 = " + (idx + 1) + "호");
                }
            }
        }
        //트래픽 보내기
        public void DataSendRequest_Traffic(int LGV_No)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //long RMP_StartAdd = 6000;
                    //short RMP_Length = 4;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Traffic[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Traffic[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Traffic[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Traffic[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Traffic[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Traffic[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Traffic[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Traffic[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Traffic[LGV_No][8] = Convert.ToByte(Write_Device_Traffic[0]);
                    BufDataSend_Traffic[LGV_No][9] = Convert.ToByte(Write_Device_Traffic[1]);
                    BufDataSend_Traffic[LGV_No][10] = Convert.ToByte(Write_Device_Traffic[2]);
                    BufDataSend_Traffic[LGV_No][11] = Convert.ToByte(Write_Device_Traffic[3]);
                    BufDataSend_Traffic[LGV_No][12] = Convert.ToByte(Write_Device_Traffic[4]);
                    BufDataSend_Traffic[LGV_No][13] = Convert.ToByte(Write_Device_Traffic[5]);
                    BufDataSend_Traffic[LGV_No][14] = Convert.ToByte(Write_Device_Traffic[6]);

                    BufDataSend_Traffic[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Traffic[LGV_No][16] = Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_Traffic[LGV_No][0] = 0x00;
                    Save_BufDataSend_Traffic[LGV_No][1] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData_Traffic[0]); // READ OK
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Traffic[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_Traffic[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Traffic[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_Traffic[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Traffic[LGV_No][i];
                    }

                    BufDataSend_Traffic[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Traffic[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Traffic[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Traffic[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(3, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_DataSendRequest_Traffic", Convert.ToString(ex));
            }
        }
        //진입허가 보내기
        public void DataSendRequest_Shutter(int LGV_No, int Order)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //short RMP_Length = 1;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Enter[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Enter[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Enter[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Enter[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Enter[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Enter[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Enter[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Enter[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Enter[LGV_No][8] = Convert.ToByte(Write_Device_Enter[0]);
                    BufDataSend_Enter[LGV_No][9] = Convert.ToByte(Write_Device_Enter[1]);
                    BufDataSend_Enter[LGV_No][10] = Convert.ToByte(Write_Device_Enter[2]);
                    BufDataSend_Enter[LGV_No][11] = Convert.ToByte(Write_Device_Enter[3]);
                    BufDataSend_Enter[LGV_No][12] = Convert.ToByte(Write_Device_Enter[4]);
                    BufDataSend_Enter[LGV_No][13] = Convert.ToByte(Write_Device_Enter[5]);
                    BufDataSend_Enter[LGV_No][14] = Convert.ToByte(Write_Device_Enter[6]);

                    BufDataSend_Enter[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Enter[LGV_No][16] = Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_Enter[LGV_No][0] = 0x00;
                    Save_BufDataSend_Enter[LGV_No][1] = Convert.ToByte(Order); // READ OK
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Enter[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_Enter[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Enter[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_Enter[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Enter[LGV_No][i];
                    }

                    BufDataSend_Enter[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Enter[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Enter[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Enter[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(5, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_DataSendRequest_Enter", Convert.ToString(ex));
            }
        }
        //차량에 데이터 쓰기
        public void DataSendRequest(int LGV_No)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //long RMP_StartAdd = 6000;
                    //short RMP_Length = 4;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Work[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Work[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Work[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Work[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Work[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Work[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Work[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Work[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Work[LGV_No][8] = Convert.ToByte(Write_Device[0]);
                    BufDataSend_Work[LGV_No][9] = Convert.ToByte(Write_Device[1]);
                    BufDataSend_Work[LGV_No][10] = Convert.ToByte(Write_Device[2]);
                    BufDataSend_Work[LGV_No][11] = Convert.ToByte(Write_Device[3]);
                    BufDataSend_Work[LGV_No][12] = Convert.ToByte(Write_Device[4]);
                    BufDataSend_Work[LGV_No][13] = Convert.ToByte(Write_Device[5]);
                    BufDataSend_Work[LGV_No][14] = Convert.ToByte(Write_Device[6]);

                    BufDataSend_Work[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Work[LGV_No][16] = Convert.ToByte('4');

                    //----------------------------
                    Save_BufDataSend[LGV_No][0] = 0x00;
                    Save_BufDataSend[LGV_No][1] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData[0]); // READ OK

                    Save_BufDataSend[LGV_No][2] = Convert.ToByte((Main.CS_AGV_Logic.LGVSendData[1] >> 8) & 0xff);
                    Save_BufDataSend[LGV_No][3] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData[1] & 0xff);  // 목적지

                    Save_BufDataSend[LGV_No][4] = 0x00;
                    Save_BufDataSend[LGV_No][5] = Convert.ToByte(Main.CS_AGV_Logic.LGVSendData[2]); // 작업 종류

                    Save_BufDataSend[LGV_No][6] = 0x00;
                    Save_BufDataSend[LGV_No][7] = 0x00;
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 8; i++)
                    {
                        BufDataSend_Work[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Work[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Work[LGV_No][i];
                    }

                    BufDataSend_Work[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Work[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Work[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Work[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(2, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_DataSendRequest", Convert.ToString(ex));
            }
        }

        //차량에 출발지,목적지 노드 번호 보내기
        public void DataSend_Work_Station_Source(int LGV_No, int Source)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //short RMP_Length = 1;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Source[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Source[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Source[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Source[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Source[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Source[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Source[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Source[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Source[LGV_No][8] = Convert.ToByte(Write_DeviceWork_Station_Source[0]);
                    BufDataSend_Source[LGV_No][9] = Convert.ToByte(Write_DeviceWork_Station_Source[1]);
                    BufDataSend_Source[LGV_No][10] = Convert.ToByte(Write_DeviceWork_Station_Source[2]);
                    BufDataSend_Source[LGV_No][11] = Convert.ToByte(Write_DeviceWork_Station_Source[3]);
                    BufDataSend_Source[LGV_No][12] = Convert.ToByte(Write_DeviceWork_Station_Source[4]);
                    BufDataSend_Source[LGV_No][13] = Convert.ToByte(Write_DeviceWork_Station_Source[5]);
                    BufDataSend_Source[LGV_No][14] = Convert.ToByte(Write_DeviceWork_Station_Source[6]);

                    BufDataSend_Source[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Source[LGV_No][16] = Convert.ToByte('1');


                    Save_BufDataSend_WorkStation_Source[LGV_No][0] = Convert.ToByte((Source >> 8) & 0xff);
                    Save_BufDataSend_WorkStation_Source[LGV_No][1] = Convert.ToByte(Source & 0xff);  //
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Source[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_WorkStation_Source[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Source[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_WorkStation_Source[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Source[LGV_No][i];
                    }

                    BufDataSend_Source[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Source[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Source[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Source[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(7, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSend_Work_Station_Source", Convert.ToString(ex));
            }
        }

        //차량에 출발지,목적지 노드 번호 보내기
        public void DataSend_Work_Station_Dest(int LGV_No, int Dest)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //short RMP_Length = 1;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Dest[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Dest[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Dest[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Dest[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Dest[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Dest[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Dest[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Dest[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Dest[LGV_No][8] = Convert.ToByte(Write_DeviceWork_Station_Dest[0]);
                    BufDataSend_Dest[LGV_No][9] = Convert.ToByte(Write_DeviceWork_Station_Dest[1]);
                    BufDataSend_Dest[LGV_No][10] = Convert.ToByte(Write_DeviceWork_Station_Dest[2]);
                    BufDataSend_Dest[LGV_No][11] = Convert.ToByte(Write_DeviceWork_Station_Dest[3]);
                    BufDataSend_Dest[LGV_No][12] = Convert.ToByte(Write_DeviceWork_Station_Dest[4]);
                    BufDataSend_Dest[LGV_No][13] = Convert.ToByte(Write_DeviceWork_Station_Dest[5]);
                    BufDataSend_Dest[LGV_No][14] = Convert.ToByte(Write_DeviceWork_Station_Dest[6]);

                    BufDataSend_Dest[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Dest[LGV_No][16] = Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_WorkStation_Dest[LGV_No][0] = Convert.ToByte((Dest >> 8) & 0xff);
                    Save_BufDataSend_WorkStation_Dest[LGV_No][1] = Convert.ToByte(Dest & 0xff);  //
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Dest[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_WorkStation_Dest[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Dest[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_WorkStation_Dest[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Dest[LGV_No][i];
                    }

                    BufDataSend_Dest[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Dest[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Dest[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Dest[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    RequestAGVDataCommunication(8, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSend_Work_Station_Dest", Convert.ToString(ex));
            }
        }

        //차량에 이중입고 알람 전송. lkw20190129
        public void DataSend_Work_Duplocation(int LGV_No, int Data)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC Duplication Data 전송
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //short RMP_Length = 1;

                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Duplication[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Duplication[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Duplication[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Duplication[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Duplication[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Duplication[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Duplication[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Duplication[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Duplication[LGV_No][8] = Convert.ToByte(Write_DeviceWork_Duplication[0]);
                    BufDataSend_Duplication[LGV_No][9] = Convert.ToByte(Write_DeviceWork_Duplication[1]);
                    BufDataSend_Duplication[LGV_No][10] = Convert.ToByte(Write_DeviceWork_Duplication[2]);
                    BufDataSend_Duplication[LGV_No][11] = Convert.ToByte(Write_DeviceWork_Duplication[3]);
                    BufDataSend_Duplication[LGV_No][12] = Convert.ToByte(Write_DeviceWork_Duplication[4]);
                    BufDataSend_Duplication[LGV_No][13] = Convert.ToByte(Write_DeviceWork_Duplication[5]);
                    BufDataSend_Duplication[LGV_No][14] = Convert.ToByte(Write_DeviceWork_Duplication[6]);

                    BufDataSend_Duplication[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Duplication[LGV_No][16] = Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_WorkStation_Duplication[LGV_No][0] = 0x00;
                    Save_BufDataSend_WorkStation_Duplication[LGV_No][1] = Convert.ToByte(Data);
                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Duplication[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_WorkStation_Duplication[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Duplication[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_WorkStation_Duplication[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Duplication[LGV_No][i];
                    }

                    BufDataSend_Duplication[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Duplication[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Duplication[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Duplication[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;

                    // AGV 이중입고 시, 차량에 DATA 전송. lkw20190129
                    RequestAGVDataCommunication(9, LGV_No);
                    #endregion
                }

            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSend_Work_Duplocation", Convert.ToString(ex));
            }
        }

        public void DataSendRequest_Link(int LGV_No, int Link)
        {
            try
            {
                //int CheckSum = 0;
                if (LGV_Sock[LGV_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    short ChkSum = 0;
                    int BufIndex = 0;
                    //long RMP_StartAdd = 6000;
                    //short RMP_Length = 4;
                    //int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- ENQ
                    BufDataSend_Link[LGV_No][0] = 0x05;

                    //------------------------- 국번
                    BufDataSend_Link[LGV_No][1] = Convert.ToByte('0');
                    BufDataSend_Link[LGV_No][2] = Convert.ToByte('0');
                    //------------------------- PLC 번호
                    BufDataSend_Link[LGV_No][3] = Convert.ToByte('F');
                    BufDataSend_Link[LGV_No][4] = Convert.ToByte('F');
                    //------------------------- 커맨드
                    BufDataSend_Link[LGV_No][5] = Convert.ToByte('Q');
                    BufDataSend_Link[LGV_No][6] = Convert.ToByte('W');
                    //------------------------- 스태이먼트 대기
                    BufDataSend_Link[LGV_No][7] = Convert.ToByte('0');
                    //------------------------- 요구데이터 길이
                    BufDataSend_Link[LGV_No][8] = Convert.ToByte(Write_Device_Link[0]);
                    BufDataSend_Link[LGV_No][9] = Convert.ToByte(Write_Device_Link[1]);
                    BufDataSend_Link[LGV_No][10] = Convert.ToByte(Write_Device_Link[2]);
                    BufDataSend_Link[LGV_No][11] = Convert.ToByte(Write_Device_Link[3]);
                    BufDataSend_Link[LGV_No][12] = Convert.ToByte(Write_Device_Link[4]);
                    BufDataSend_Link[LGV_No][13] = Convert.ToByte(Write_Device_Link[5]);
                    BufDataSend_Link[LGV_No][14] = Convert.ToByte(Write_Device_Link[6]);

                    BufDataSend_Link[LGV_No][15] = Convert.ToByte('0');
                    BufDataSend_Link[LGV_No][16] = Convert.ToByte('1');//Convert.ToByte(Main.Send_Link[LGV_No]);//Convert.ToByte('1');

                    //----------------------------
                    Save_BufDataSend_Link[LGV_No][0] = 0x00;
                    Save_BufDataSend_Link[LGV_No][1] = Convert.ToByte(Main.Send_Link[LGV_No]);//Convert.ToByte(Link); // READ OK

                    Main.Send_Link[LGV_No] = 0;

                    //-------------------------------
                    BufIndex = 17;
                    for (int i = 0; i < 2; i++)
                    {
                        BufDataSend_Link[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((Save_BufDataSend_Link[LGV_No][i] & 0xff) >> 4)));
                        BufIndex++;

                        BufDataSend_Link[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar(Save_BufDataSend_Link[LGV_No][i] & 0xff)));
                        BufIndex++;
                    }

                    // 체크섬 계산
                    for (int i = 1; i < BufIndex; i++)
                    {
                        ChkSum += BufDataSend_Link[LGV_No][i];
                    }

                    BufDataSend_Link[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff) >> 4)));
                    BufIndex++;

                    BufDataSend_Link[LGV_No][BufIndex] = Convert.ToByte(Hex2Char(Convert.ToChar((ChkSum & 0xff))));
                    BufIndex++;

                    BufDataSend_Link[LGV_No][BufIndex] = 0x0d;         // DLE
                    BufIndex++;

                    BufDataSend_Link[LGV_No][BufIndex] = 0x0a;         // ETX
                    BufIndex++;


                    RequestAGVDataCommunication(4, LGV_No);
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_DataSendRequest_Link", Convert.ToString(ex));
            }
        }

        //상태 요청, 차량에 데이터 쓰기 요청
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
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 2)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Work[LGV_No], 0, BufDataSend_Work[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 3)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Traffic[LGV_No], 0, BufDataSend_Traffic[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 4)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Link[LGV_No], 0, BufDataSend_Link[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 5)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Enter[LGV_No], 0, BufDataSend_Enter[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 6)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Abort[LGV_No], 0, BufDataSend_Abort[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 7)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Source[LGV_No], 0, BufDataSend_Source[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    else if (type == 8)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Dest[LGV_No], 0, BufDataSend_Dest[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                    //AGV 광폭 기재랙 이중입고 시 알람 전송. lkw20190129
                    else if (type == 9)
                    {
                        LGV_Sock[LGV_No].BeginSend(BufDataSend_Duplication[LGV_No], 0, BufDataSend_Duplication[LGV_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), LGV_Sock[LGV_No]);
                        Send_Done[LGV_No].WaitOne(10);
                    }
                }

            }
            catch (Exception ex)
            {
                Main.Log("Try Catch RequestAGVDataCommunication", Convert.ToString(ex));
            }
        }

        private void RequestAGVDataCommunicationCallBack(IAsyncResult IAR)
        {
//            string log;
            string IP = "";
            string IP_List = "";
            int idx = -1;
            try
            {
                Socket Send_Socket = (Socket)IAR.AsyncState;

                if(Send_Socket.Connected == true)
                {
                    for (int i = 0; i < Form1.LGV_NUM; i++)
                    {
                        IP = Convert.ToString(Send_Socket.RemoteEndPoint);
                        IP_List = Main.CS_AGV_C_Info[i].IP + ":" + Main.CS_AGV_C_Info[i].Port;
                        if (IP_List == IP)
                        {
                            idx = i;
                            break;
                        }
                    }
                }
                

                if (Send_Socket.Connected == true)
                {
                    if(idx != -1)
                    {
                        if (LGV_Sock[idx].Connected == true)
                        {
                            LGV_Sock[idx].EndSend(IAR);
                            Send_Done[idx].Set();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch RequestAGVDataCommunicationCallBack", Convert.ToString(ex));
            }
        }
    }
}
