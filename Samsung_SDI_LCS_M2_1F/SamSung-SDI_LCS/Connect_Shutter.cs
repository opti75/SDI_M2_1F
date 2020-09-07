using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public class Connect_Shutter
    { 
        Form1 Main;
        public Socket[] Shutter_Sock;
        //차량이 보내주는 데이터
        public byte[][] BufferData = new byte[Form1.SHUTTER_NUM][];
        //상태 요청 -> BufferData에 값들어옴
        public byte[][] BufDataRead = new byte[Form1.SHUTTER_NUM][];
        //작업 전송
        public byte[][] BufDataSend_InPut_Open = new byte[Form1.SHUTTER_NUM][];
        public byte[][] BufDataSend_InPut_Close = new byte[Form1.SHUTTER_NUM][];

        public byte[][] BufDataSend_OutPut_Open = new byte[Form1.SHUTTER_NUM][];
        public byte[][] BufDataSend_OutPut_Close = new byte[Form1.SHUTTER_NUM][];
        public byte[][] BufDataSend_HeartBit = new byte[Form1.SHUTTER_NUM][];
        
        int temp;
        int Data_Length_L, Data_Length_H;
        string Read_PLC = "D006000";
        char[] Read_Device;
        
        public static ManualResetEvent Conn_Done = new ManualResetEvent(false);
        public static ManualResetEvent Receive_Done = new ManualResetEvent(false);
        public static ManualResetEvent Send_Done = new ManualResetEvent(false);

        //생성자
        public Connect_Shutter()
        {

        }
        //생성자
        public Connect_Shutter(Form1 CS_Main)
        {
            Main = CS_Main;

            Shutter_Sock = new Socket[Form1.SHUTTER_NUM];

            for (int i = 0; i < Form1.SHUTTER_NUM; i++)
            {
                BufferData[i] = new byte[500];
                BufDataRead[i] = new byte[21];
                BufDataSend_InPut_Open[i] = new byte[23];
                BufDataSend_InPut_Close[i] = new byte[23];
                BufDataSend_OutPut_Open[i] = new byte[23];
                BufDataSend_OutPut_Close[i] = new byte[23];
                BufDataSend_HeartBit[i] = new byte[23];
            }

            //Shutter_Sock
            for (int i = 0; i < Form1.SHUTTER_NUM; i++)
            {
                Shutter_Sock[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        //차량 접속
        public void Connect(int Shutter_No, string IP, int Port)
        {
            IAsyncResult result;
            try
            {
                if (Shutter_Sock[Shutter_No].Connected == false)
                {
                    Shutter_Sock[Shutter_No] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint Shutter_IP = new IPEndPoint(IPAddress.Parse(IP), Port);
                    result = Shutter_Sock[Shutter_No].BeginConnect(Shutter_IP, new AsyncCallback(Shutter_Connected), Shutter_Sock[Shutter_No]);
                    
                    Main.Log("AGV_Shutter_Connecting", (Shutter_No + 1) + "호 접속중...");
                    //Conn_Done.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Connect", Convert.ToString(ex));
            }
        }

        //차량 접속 됐을때
        private void Shutter_Connected(IAsyncResult IAR)
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            string IP = "";
            string IP_List = "";
            try
            {
                for (int Shutter_No = 0; Shutter_No < Form1.SHUTTER_NUM; Shutter_No++)
                {
                    if (tempSock.Connected)
                    {
                        //접속된 차량 ip따오기
                        IP = Convert.ToString(tempSock.RemoteEndPoint);
                        //차량 통신 클래스에 있는 ip따오기
                        IP_List = Main.CS_Shutter_C_Info[Shutter_No].IP + ":" + Main.CS_Shutter_C_Info[Shutter_No].Port;

                        if (IP_List == IP)
                        {
                            if (Shutter_Sock[Shutter_No].Connected == true && tempSock.Connected == true) //수정 대상
                            {
                                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                                {
                                        //음극 코터
                                        if (Shutter_No == 0)
                                    {
                                        Main.P_M_COT1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 코터
                                        else if (Shutter_No == 1)
                                    {
                                        Main.P_P_COT1.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 롤창고 1
                                        else if (Shutter_No == 2)
                                    {
                                        Main.P_M_WareHouse_1.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 롤창고 2
                                        else if (Shutter_No == 3)
                                    {
                                        Main.P_M_WareHouse_2.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 롤창고 1
                                        else if (Shutter_No == 4)
                                    {
                                        Main.P_P_WareHouse_1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 롤창고 2
                                        else if (Shutter_No == 5)
                                    {
                                        Main.P_P_WareHouse_2.BackColor = Color.RoyalBlue;
                                    }
                                        //광폭 코터
                                        else if (Shutter_No == 6)
                                    {
                                        Main.P_M_B_COT.BackColor = Color.RoyalBlue;
                                    }
                                        //광폭 코터
                                        else if (Shutter_No == 7)
                                    {
                                        Main.P_P_B_COT.BackColor = Color.RoyalBlue;
                                    }

                                        //음극 프레스 1식
                                        else if (Shutter_No == 8)
                                    {
                                        Main.P_M_Press1.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 프레스 2식
                                        else if (Shutter_No == 9)
                                    {
                                        Main.P_M_Press2.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 프레스 3식
                                        else if (Shutter_No == 10)
                                    {
                                        Main.P_M_Press3.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 프레스 4식
                                        else if (Shutter_No == 11)
                                    {
                                        Main.P_M_Press4.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 1식
                                        else if (Shutter_No == 12)
                                    {
                                        Main.P_P_Press1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 2식
                                        else if (Shutter_No == 13)
                                    {
                                        Main.P_P_Press2.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 3식
                                        else if (Shutter_No == 14)
                                    {
                                        Main.P_P_Press3.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 4식
                                        else if (Shutter_No == 15)
                                    {
                                        Main.P_P_Press4.BackColor = Color.RoyalBlue;
                                    }

                                        //음극 슬리터 1식
                                        else if (Shutter_No == 16)
                                    {
                                        Main.P_M_SLT1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 3식
                                        else if (Shutter_No == 17)
                                    {
                                        Main.P_M_SLT2.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 4식
                                        else if (Shutter_No == 18)
                                    {
                                        Main.P_M_SLT3.BackColor = Color.RoyalBlue;
                                    }

                                        //양극 프레스 1식
                                        else if (Shutter_No == 19)
                                    {
                                        Main.P_P_SLT1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 2식
                                        else if (Shutter_No == 20)
                                    {
                                        Main.P_P_SLT2.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 프레스 3식
                                        else if (Shutter_No == 21)
                                    {
                                        Main.P_P_SLT3.BackColor = Color.RoyalBlue;
                                    }
                                        //SFL 코터
                                        else if (Shutter_No == 22)
                                    {
                                        Main.p_SFL.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 롤 R/W_1
                                        else if (Shutter_No == 23)
                                    {
                                        Main.P_M_ROLL_RW_2.BackColor = Color.RoyalBlue;
                                    }
                                        //음극 롤 R/W_2
                                        else if (Shutter_No == 24)
                                    {
                                        Main.P_M_ROLL_RW_1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 롤 R/W
                                        else if (Shutter_No == 25)
                                    {
                                        Main.P_P_ROLL_RW_1.BackColor = Color.RoyalBlue;
                                    }
                                        //양극 롤 R/W
                                        else if (Shutter_No == 26)
                                    {
                                        Main.P_P_ROLL_RW_2.BackColor = Color.RoyalBlue;
                                    }
                                    Shutter_Sock[Shutter_No].BeginReceive(BufferData[Shutter_No], 0, BufferData[Shutter_No].Length, SocketFlags.None,
                                new AsyncCallback(ReceiveData), Shutter_Sock[Shutter_No]);
                                        //Receive_Done.WaitOne();
                                        Main.Log("AGV_Shutter_Connecting", (Shutter_No + 1) + "호 접속완료...");
                                }));

                                tempSock.EndConnect(IAR);
                                Conn_Done.Set();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Connected", Convert.ToString(ex));
            }


        }

        //차량 접속 끊기
        public void Dis_Connect(int Shutter_No)
        {
            try
            {
                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    Shutter_Sock[Shutter_No].Shutdown(SocketShutdown.Both);
                    Shutter_Sock[Shutter_No].Close();
                    Shutter_Sock[Shutter_No].Dispose();
                    
                    BufferData[Shutter_No] = new byte[500];
                    BufDataRead[Shutter_No] = new byte[21];
                    BufDataSend_InPut_Open[Shutter_No] = new byte[23];
                    BufDataSend_InPut_Close[Shutter_No] = new byte[23];
                    BufDataSend_OutPut_Open[Shutter_No] = new byte[23];
                    BufDataSend_OutPut_Close[Shutter_No] = new byte[23];
                    BufDataSend_HeartBit[Shutter_No] = new byte[23];

                    Main.CS_Shutter_Info[Shutter_No].InPut_Close_Sensor = 0;
                    Main.CS_Shutter_Info[Shutter_No].InPut_Open_Sensor = 0;
                    Main.CS_Shutter_Info[Shutter_No].OutPut_Close_Sensor = 0;
                    Main.CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor = 0;

                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        //음극 코터
                        if (Shutter_No == 0)
                        {
                            Main.P_M_COT1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 코터
                        else if (Shutter_No == 1)
                        {
                            Main.P_P_COT1.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 롤창고 1
                        else if (Shutter_No == 2)
                        {
                            Main.P_M_WareHouse_1.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 롤창고 2
                        else if (Shutter_No == 3)
                        {
                            Main.P_M_WareHouse_2.BackColor = System.Drawing.Color.Red;
                        }

                        //양극 롤창고 1
                        else if (Shutter_No == 4)
                        {
                            Main.P_P_WareHouse_1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 롤창고 2
                        else if (Shutter_No == 5)
                        {
                            Main.P_P_WareHouse_2.BackColor = System.Drawing.Color.Red;
                        }
                        //광폭 코터
                        else if (Shutter_No == 6)
                        {
                            Main.P_M_B_COT.BackColor = System.Drawing.Color.Red;
                        }
                        //광폭 코터
                        else if (Shutter_No == 7)
                        {
                            Main.P_P_B_COT.BackColor = System.Drawing.Color.Red;
                        }


                        //음극 프레스 1식
                        else if (Shutter_No == 8)
                        {
                            Main.P_M_Press1.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 프레스 2식
                        else if (Shutter_No == 9)
                        {
                            Main.P_M_Press2.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 프레스 3식
                        else if (Shutter_No == 10)
                        {
                            Main.P_M_Press3.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 프레스 4식
                        else if (Shutter_No == 11)
                        {
                            Main.P_M_Press4.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 1식
                        else if (Shutter_No == 12)
                        {
                            Main.P_P_Press1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 2식
                        else if (Shutter_No == 13)
                        {
                            Main.P_P_Press2.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 3식
                        else if (Shutter_No == 14)
                        {
                            Main.P_P_Press3.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 4식
                        else if (Shutter_No == 15)
                        {
                            Main.P_P_Press4.BackColor = System.Drawing.Color.Red;
                        }

                        //음극 슬리터 1식
                        else if (Shutter_No == 16)
                        {
                            Main.P_M_SLT1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 3식
                        else if (Shutter_No == 17)
                        {
                            Main.P_M_SLT2.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 4식
                        else if (Shutter_No == 18)
                        {
                            Main.P_M_SLT3.BackColor = System.Drawing.Color.Red;
                        }

                        //양극 프레스 1식
                        else if (Shutter_No == 19)
                        {
                            Main.P_P_SLT1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 2식
                        else if (Shutter_No == 20)
                        {
                            Main.P_P_SLT2.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 프레스 3식
                        else if (Shutter_No == 21)
                        {
                            Main.P_P_SLT3.BackColor = System.Drawing.Color.Red;
                        }

                        //SFL 코터
                        else if (Shutter_No == 22)
                        {
                            Main.p_SFL.BackColor = System.Drawing.Color.Red;
                        }

                        //음극 롤 R/W_1
                        else if (Shutter_No == 23)
                        {
                            Main.P_M_ROLL_RW_2.BackColor = System.Drawing.Color.Red;
                        }
                        //음극 롤 R/W_2
                        else if (Shutter_No == 24)
                        {
                            Main.P_M_ROLL_RW_1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 롤 R/W
                        else if (Shutter_No == 25)
                        {
                            Main.P_P_ROLL_RW_1.BackColor = System.Drawing.Color.Red;
                        }
                        //양극 롤 R/W
                        else if (Shutter_No == 26)
                        {
                            Main.P_P_ROLL_RW_2.BackColor = System.Drawing.Color.Red;
                        }

                    }));
                    Main.Log("AGV_Shutter_Connecting", (Shutter_No + 1) + "호 접속끊김...");
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Dis_Connect", Convert.ToString(ex));
            }

            GC.Collect();
        }

        //차량 정보 받기
        public void ReceiveData(IAsyncResult IAR)
        {
//            string log;
//            int tmp;
//            string temp = "";
            string IP = "";
            string IP_List = "";
            int bufferSize = 0;

            Socket Receive_Socket = (Socket)IAR.AsyncState;

            try
            {
                for (int i = 0; i < Form1.SHUTTER_NUM; i++)
                {
                    if (Receive_Socket.Connected == true)
                    {
                        IP = Convert.ToString(Receive_Socket.RemoteEndPoint);
                        IP_List = Main.CS_Shutter_C_Info[i].IP + ":" + Main.CS_Shutter_C_Info[i].Port;
                        {
                            if (IP_List == IP)
                            {
                                if (Shutter_Sock[i].Connected == true)
                                {
                                    bufferSize = Receive_Socket.EndReceive(IAR);

                                    if (BufferData[i][7] == Data_Length_L && BufferData[i][8] == Data_Length_H)
                                    {
                                        #region 데이터 파싱!

                                        Main.CS_AGV_Logic.ShutterReadData[i, 0] = BufferData[i][11];  //입고대 열림 센서 
                                        Main.CS_AGV_Logic.ShutterReadData[i, 1] = BufferData[i][12];  //입고대 열림 센서

                                        Main.CS_AGV_Logic.ShutterReadData[i, 2] = BufferData[i][13];  //입고대 닫힘 센서
                                        Main.CS_AGV_Logic.ShutterReadData[i, 3] = BufferData[i][14];  //입고대 닫힘 센서

                                        Main.CS_AGV_Logic.ShutterReadData[i, 4] = BufferData[i][15];  //출고대 열림 센서 
                                        Main.CS_AGV_Logic.ShutterReadData[i, 5] = BufferData[i][16];  //출고대 열림 센서 

                                        Main.CS_AGV_Logic.ShutterReadData[i, 6] = BufferData[i][17];  //출고대 닫힘 센서 
                                        Main.CS_AGV_Logic.ShutterReadData[i, 7] = BufferData[i][18];  //출고대 닫힘 센서 

                                        //HeartBit 추가. lkw20190124
                                        Main.CS_AGV_Logic.ShutterReadData[i, 8] = BufferData[i][19];  //HeartBit 추가.
                                        Main.CS_AGV_Logic.ShutterReadData[i, 9] = BufferData[i][20];  //HeartBit 추가.

                                        Main.CS_AGV_Logic.Shutter_Data(i);

                                        Shutter_Sock[i].BeginReceive(BufferData[i], 0, BufferData[i].Length, SocketFlags.None,
                                            new AsyncCallback(ReceiveData), Shutter_Sock[i]);
                                        Main.Flag_ReReceive_Shutter[i] = 0;
                                        #endregion

                                    }
                                    else
                                    {
                                        Main.Flag_ReReceive_Shutter[i] = 0;
                                        Shutter_Sock[i].BeginReceive(BufferData[i], 0, BufferData[i].Length, SocketFlags.None,
                                            new AsyncCallback(ReceiveData), Shutter_Sock[i]);
                                    }

                                }
                            }
                        }
                    }
                }
                Receive_Done.Set();
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_ReceiveData", Convert.ToString(ex));
            }
        }

        public void DataReadBufferRequest(int Shutter_No)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8500;
                    //HeartBit 추가. 자리수 +1 증가. lkw20190124
                    //short RMP_Length = 4;
                    short RMP_Length = 5;

                    //------------------------- 서브헤더
                    BufDataRead[Shutter_No][0] = 0x50;
                    BufDataRead[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataRead[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataRead[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataRead[Shutter_No][4] = 0xFF;
                    BufDataRead[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataRead[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataRead[Shutter_No][7] = 12;       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataRead[Shutter_No][8] = 0x00;     // 워드단위 Binary 읽기는 12 고정
                                                           //------------------------- CPU 감지타이머
                    BufDataRead[Shutter_No][9] = 0x10;
                    BufDataRead[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataRead[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataRead[Shutter_No][12] = 0x04;
                    //------------------------- 서브커맨드
                    BufDataRead[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataRead[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataRead[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataRead[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataRead[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataRead[Shutter_No][18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataRead[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataRead[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    temp = BufDataRead[Shutter_No][19];
                    temp = temp | (BufDataRead[Shutter_No][20] << 8);

                    temp = temp * 2 + 2;

                    Data_Length_L = (temp) & 0xff;
                    Data_Length_H = (temp >> 8) & 0xff;

                    RequestAGVDataCommunication(1, Shutter_No);


                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataReadBufferRequest", Convert.ToString(ex));
            }
        }

        //HeartBit 0/1 전송. lkw20190124
        public void DataSendRequest_HeartBit(int Shutter_No, int Heartbit)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8604;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_HeartBit[Shutter_No][0] = 0x50;
                    BufDataSend_HeartBit[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_HeartBit[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_HeartBit[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_HeartBit[Shutter_No][4] = 0xFF;
                    BufDataSend_HeartBit[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_HeartBit[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_HeartBit[Shutter_No][7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_HeartBit[Shutter_No][8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                          //------------------------- CPU 감지타이머
                    BufDataSend_HeartBit[Shutter_No][9] = 0x10;
                    BufDataSend_HeartBit[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_HeartBit[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataSend_HeartBit[Shutter_No][12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_HeartBit[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataSend_HeartBit[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_HeartBit[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_HeartBit[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_HeartBit[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                                        //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high

                    //------------------------- 디바이스 코드
                    BufDataSend_HeartBit[Shutter_No][18] = 0xA8;    //'D'영역
                                                                    //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_HeartBit[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_HeartBit[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    BufDataSend_HeartBit[Shutter_No][21] = Convert.ToByte(Heartbit);
                    BufDataSend_HeartBit[Shutter_No][22] = 0x00;

                    // Type 6 추가. HeartBit 용도. lkw20190124
                    RequestAGVDataCommunication(6, Shutter_No);
                    #endregion
                }
                // Main.Flag_Read_Request = 0;
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataSendRequest_OutPut_Close", Convert.ToString(ex));
            }
        }

        public void DataSendRequest_InPut_Open(int Shutter_No, int Open)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8600;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_InPut_Open[Shutter_No][0] = 0x50;
                    BufDataSend_InPut_Open[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_InPut_Open[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_InPut_Open[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_InPut_Open[Shutter_No][4] = 0xFF;
                    BufDataSend_InPut_Open[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_InPut_Open[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_InPut_Open[Shutter_No][7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_InPut_Open[Shutter_No][8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                 //------------------------- CPU 감지타이머
                    BufDataSend_InPut_Open[Shutter_No][9] = 0x10;
                    BufDataSend_InPut_Open[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_InPut_Open[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataSend_InPut_Open[Shutter_No][12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_InPut_Open[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataSend_InPut_Open[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_InPut_Open[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_InPut_Open[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_InPut_Open[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataSend_InPut_Open[Shutter_No][18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_InPut_Open[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_InPut_Open[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    BufDataSend_InPut_Open[Shutter_No][21] = Convert.ToByte(Open);
                    BufDataSend_InPut_Open[Shutter_No][22] = 0x00;

                    RequestAGVDataCommunication(2, Shutter_No);
                    #endregion
                }
                // Main.Flag_Read_Request = 0;
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataSendRequest_InPut_Open", Convert.ToString(ex));
            }
        }

        public void DataSendRequest_InPut_Close(int Shutter_No, int Close)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8601;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_InPut_Close[Shutter_No][0] = 0x50;
                    BufDataSend_InPut_Close[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_InPut_Close[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_InPut_Close[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_InPut_Close[Shutter_No][4] = 0xFF;
                    BufDataSend_InPut_Close[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_InPut_Close[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_InPut_Close[Shutter_No][7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_InPut_Close[Shutter_No][8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                 //------------------------- CPU 감지타이머
                    BufDataSend_InPut_Close[Shutter_No][9] = 0x10;
                    BufDataSend_InPut_Close[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_InPut_Close[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataSend_InPut_Close[Shutter_No][12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_InPut_Close[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataSend_InPut_Close[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_InPut_Close[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_InPut_Close[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_InPut_Close[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataSend_InPut_Close[Shutter_No][18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_InPut_Close[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_InPut_Close[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    BufDataSend_InPut_Close[Shutter_No][21] = Convert.ToByte(Close);
                    BufDataSend_InPut_Close[Shutter_No][22] = 0x00;

                    RequestAGVDataCommunication(3, Shutter_No);
                    #endregion
                }
                // Main.Flag_Read_Request = 0;
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataSendRequest_InPut_Close", Convert.ToString(ex));
            }
        }
        public void DataSendRequest_OutPut_Open(int Shutter_No, int Open)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8602;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_OutPut_Open[Shutter_No][0] = 0x50;
                    BufDataSend_OutPut_Open[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_OutPut_Open[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_OutPut_Open[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_OutPut_Open[Shutter_No][4] = 0xFF;
                    BufDataSend_OutPut_Open[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_OutPut_Open[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_OutPut_Open[Shutter_No][7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_OutPut_Open[Shutter_No][8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                 //------------------------- CPU 감지타이머
                    BufDataSend_OutPut_Open[Shutter_No][9] = 0x10;
                    BufDataSend_OutPut_Open[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_OutPut_Open[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataSend_OutPut_Open[Shutter_No][12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_OutPut_Open[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataSend_OutPut_Open[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_OutPut_Open[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_OutPut_Open[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_OutPut_Open[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataSend_OutPut_Open[Shutter_No][18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_OutPut_Open[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_OutPut_Open[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    BufDataSend_OutPut_Open[Shutter_No][21] = Convert.ToByte(Open);
                    BufDataSend_OutPut_Open[Shutter_No][22] = 0x00;

                    RequestAGVDataCommunication(4, Shutter_No);
                    #endregion

                }
                // Main.Flag_Read_Request = 0;
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataSendRequest_OutPut_Open", Convert.ToString(ex));
            }
        }

        public void DataSendRequest_OutPut_Close(int Shutter_No, int Close)
        {
            try
            {
                //int CheckSum = 0;

                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 8603;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_OutPut_Close[Shutter_No][0] = 0x50;
                    BufDataSend_OutPut_Close[Shutter_No][1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_OutPut_Close[Shutter_No][2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_OutPut_Close[Shutter_No][3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_OutPut_Close[Shutter_No][4] = 0xFF;
                    BufDataSend_OutPut_Close[Shutter_No][5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_OutPut_Close[Shutter_No][6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_OutPut_Close[Shutter_No][7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_OutPut_Close[Shutter_No][8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                 //------------------------- CPU 감지타이머
                    BufDataSend_OutPut_Close[Shutter_No][9] = 0x10;
                    BufDataSend_OutPut_Close[Shutter_No][10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_OutPut_Close[Shutter_No][11] = 0x01;    // Binary로 읽음
                    BufDataSend_OutPut_Close[Shutter_No][12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_OutPut_Close[Shutter_No][13] = 0x00;    // Binary로 읽음
                    BufDataSend_OutPut_Close[Shutter_No][14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_OutPut_Close[Shutter_No][15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_OutPut_Close[Shutter_No][16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_OutPut_Close[Shutter_No][17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataSend_OutPut_Close[Shutter_No][18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_OutPut_Close[Shutter_No][19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_OutPut_Close[Shutter_No][20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    BufDataSend_OutPut_Close[Shutter_No][21] = Convert.ToByte(Close);
                    BufDataSend_OutPut_Close[Shutter_No][22] = 0x00;

                    RequestAGVDataCommunication(5, Shutter_No);
                    #endregion

                }
                // Main.Flag_Read_Request = 0;
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataSendRequest_OutPut_Close", Convert.ToString(ex));
            }
        }
        //상태 요청, 차량에 데이터 쓰기 요청
        public void RequestAGVDataCommunication(int type, int Shutter_No)
        {
            try
            {
                if (Shutter_Sock[Shutter_No].Connected == true)
                {
                    if (type == 1)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataRead[Shutter_No], 0, BufDataRead[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();

                    }
                    else if (type == 2)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataSend_InPut_Open[Shutter_No], 0, BufDataSend_InPut_Open[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();
                    }
                    else if (type == 3)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataSend_InPut_Close[Shutter_No], 0, BufDataSend_InPut_Close[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();
                    }
                    else if (type == 4)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataSend_OutPut_Open[Shutter_No], 0, BufDataSend_OutPut_Open[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();
                    }
                    else if (type == 5)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataSend_OutPut_Close[Shutter_No], 0, BufDataSend_OutPut_Close[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();
                    }
                    //HeartBit 추가. lkw20190124
                    else if (type == 6)
                    {
                        Shutter_Sock[Shutter_No].BeginSend(BufDataSend_HeartBit[Shutter_No], 0, BufDataSend_HeartBit[Shutter_No].Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), Shutter_Sock[Shutter_No]);
                        Send_Done.WaitOne();
                    }
                }

            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_RequestAGVDataCommunication", Convert.ToString(ex));
            }
        }
        private void RequestAGVDataCommunicationCallBack(IAsyncResult IAR)
        {
//            string log;
            string IP = "";
            string IP_List = "";

            try
            {
                Socket Send_Socket = (Socket)IAR.AsyncState;

                for (int i = 0; i < Form1.SHUTTER_NUM; i++)
                {
                    if (Send_Socket.Connected == true)
                    {
                        IP = Convert.ToString(Send_Socket.RemoteEndPoint);
                        IP_List = Main.CS_Shutter_C_Info[i].IP + ":" + Main.CS_Shutter_C_Info[i].Port;
                        if (IP_List == IP)
                        {
                            if (Shutter_Sock[i].Connected == true)
                            {
                                Send_Socket.EndReceive(IAR);
                                Send_Done.Set();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_RequestAGVDataCommunicationCallBack", Convert.ToString(ex));
            }
        }

    }
}
