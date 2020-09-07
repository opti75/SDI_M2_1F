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
    public class Connect_PLC
    {
        Form1 Main;
        public Socket PLC_Sock;
        //PLC가 보내주는 데이터
        public byte[] BufferData = new byte[500];
        //상태 요청 -> BufferData에 값들어옴
        public byte[] BufDataRead = new byte[21];
        //영역 진입
        public byte[] BufDataSend_InPut_Area = new byte[23]; 
        //에어 샤워
        public byte[] BufDataSend_InPut_Air = new byte[23];
        //자동문
        public byte[] BufDataSend_AutoDoor = new byte[23];

        public int Area_Enter = 0;
        public int Area_Using = 0;

        public int Auto_Door_Open = 0;
        public int Auto_Door_Close = 0;

        public int Door_Open_1 = 0;
        public int Door_Close_1 = 0; 
        public int Door_Open_2 = 0;
        public int Door_Close_2 = 0;

        public byte Send_Door_Open_1 = 0;
        public byte Send_Door_Close_1 = 0;
        public byte Send_Door_Open_2 = 0;
        public byte Send_Door_Close_2 = 0;

        public byte Send_Enter_Ok = 0;
        public byte Send_Enter_ING = 0;

        public byte Send_AutoDoor_Open = 0;

        int temp;
        int Data_Length_L, Data_Length_H;

        public static ManualResetEvent Conn_Done = new ManualResetEvent(false);
        public static ManualResetEvent Receive_Done = new ManualResetEvent(false);
        public static ManualResetEvent Send_Done = new ManualResetEvent(false);

        //생성자
        public Connect_PLC()
        {

        }
        //생성자
        public Connect_PLC(Form1 CS_Main)
        {
            Main = CS_Main;

            PLC_Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
        }

        //PLC 접속
        public void Connect(string IP, int Port)
        {
            IAsyncResult result;
            try
            {
                if (PLC_Sock.Connected == false)
                {
                    PLC_Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint PLC_IP = new IPEndPoint(IPAddress.Parse(IP), Port);
                    result = PLC_Sock.BeginConnect(PLC_IP, new AsyncCallback(PLC_Connected), PLC_Sock);
                    //Conn_Done.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Connect", Convert.ToString(ex));
            }
        }

        //접속 됐을때
        private void PLC_Connected(IAsyncResult IAR)
        {
            Socket tempSock = (Socket)IAR.AsyncState;
            string IP = "";
            string IP_List = "";
            try
            {
                if (PLC_Sock.Connected == true && tempSock.Connected == true)
                {
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        //판넬로 접속 여부 확인
                        Main.panel6.BackColor = Color.Lime;
                        //데이터 받기
                        PLC_Sock.BeginReceive(BufferData, 0, BufferData.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveData), PLC_Sock);

                    }));
                    tempSock.EndConnect(IAR);
                    Conn_Done.Set();
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Connected", Convert.ToString(ex));
            }

        }

        //차량 접속 끊기
        public void Dis_Connect()
        {
            try
            {
                if (PLC_Sock.Connected == true)
                {
                    PLC_Sock.Shutdown(SocketShutdown.Both);
                    PLC_Sock.Close();
                    PLC_Sock.Dispose();
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        //판넬로 접속 끊겼는지 확인
                        Main.panel6.BackColor = System.Drawing.Color.Red;
   
                    }));

                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_Dis_Connect", Convert.ToString(ex));
            }

            GC.Collect();
        }

        //정보 받기
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
                if (PLC_Sock.Connected == true)
                {
                    bufferSize = Receive_Socket.EndReceive(IAR);

                    if (BufferData[7] == Data_Length_L && BufferData[8] == Data_Length_H)
                    {
                        #region 데이터 파싱!
                        Area_Enter = (BufferData[11] & 0x01);  //영역 진입 InterLock
                        Area_Using = ((BufferData[11] >> 1) & 0x01);

                        Door_Open_1 = (BufferData[13] & 0x01);  //영역 진입 InterLock
                        Door_Close_1 = ((BufferData[13] >> 1) & 0x01);
                        Door_Open_2 = ((BufferData[13] >> 2) & 0x01);
                        Door_Close_2 = ((BufferData[13] >> 3) & 0x01);

                        Auto_Door_Open = (BufferData[15] & 0x01);  //영역 진입 InterLock
                        Auto_Door_Close = ((BufferData[15] >> 1) & 0x01);

                        //데이터 받기
                        Receive_Socket.BeginReceive(BufferData, 0, BufferData.Length, SocketFlags.None, ReceiveData, Receive_Socket);
                        //통신되고 있는지 파악하는 플래그
                        Main.Flag_ReReceive_PLC = 0;

                        Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            Main.Log("PLC_ReceiveData_Info", string.Format("영역 진입: {0}, 영역 사용: {1}, 도어 열림_1: {2}, 도어 닫음_1: {3}, 도어 열림_2: {4}, 도어 닫음_2: {5}, 자동문 열림: {6}, 자동문 닫힘: {7}",
                                                                       Area_Enter, Area_Using, Door_Open_1, Door_Close_1, Door_Open_2, Door_Close_2, Auto_Door_Open, Auto_Door_Close));

                        }));

                        #endregion
                    }
                    else
                    {
                        Receive_Socket.BeginReceive(BufferData, 0, BufferData.Length, SocketFlags.None, ReceiveData, Receive_Socket);
                    }

                }
                Receive_Done.Set();
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_ReceiveData", Convert.ToString(ex));
            }
        }
        //글로벌텍 데이터 요청 함수(Read) - 값은 ReceiveData구문 으로 들어감
        public void DataReadBufferRequest()
        {
            try
            {
                //int CheckSum = 0;

                if (PLC_Sock.Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    //6000번지 읽기
                    long RMP_StartAdd = 6000;
                    short RMP_Length = 6;

                    //------------------------- 서브헤더
                    BufDataRead[0] = 0x50;
                    BufDataRead[1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataRead[2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataRead[3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataRead[4] = 0xFF;
                    BufDataRead[5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataRead[6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataRead[7] = 12;       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataRead[8] = 0x00;     // 워드단위 Binary 읽기는 12 고정
                                                           //------------------------- CPU 감지타이머
                    BufDataRead[9] = 0x10;
                    BufDataRead[10] = 0x00;
                    //------------------------- 커맨드
                    BufDataRead[11] = 0x01;    // Binary로 읽음
                    BufDataRead[12] = 0x04;
                    //------------------------- 서브커맨드
                    BufDataRead[13] = 0x00;    // Binary로 읽음
                    BufDataRead[14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataRead[15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataRead[16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataRead[17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataRead[18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataRead[19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataRead[20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    temp = BufDataRead[19];
                    temp = temp | (BufDataRead[20] << 8);

                    temp = temp * 2 + 2;

                    Data_Length_L = (temp) & 0xff;
                    Data_Length_H = (temp >> 8) & 0xff;

                    RequestAGVDataCommunication(1);
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try Catch_Shutter_DataReadBufferRequest", Convert.ToString(ex));
            }
        }
        //영역진입 신호 보내주기 Write
        public void DataSendRequest_BufDataSend_InPut_Area()
        {
            try
            {
                int Data = 0;
                //int CheckSum = 0;

                if (PLC_Sock.Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 6100;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_InPut_Area[0] = 0x50;
                    BufDataSend_InPut_Area[1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_InPut_Area[2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_InPut_Area[3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_InPut_Area[4] = 0xFF;
                    BufDataSend_InPut_Area[5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_InPut_Area[6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_InPut_Area[7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_InPut_Area[8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                 //------------------------- CPU 감지타이머
                    BufDataSend_InPut_Area[9] = 0x10;
                    BufDataSend_InPut_Area[10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_InPut_Area[11] = 0x01;    // Binary로 읽음
                    BufDataSend_InPut_Area[12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_InPut_Area[13] = 0x00;    // Binary로 읽음
                    BufDataSend_InPut_Area[14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_InPut_Area[15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_InPut_Area[16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_InPut_Area[17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                               //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                               //------------------------- 디바이스 코드
                    BufDataSend_InPut_Area[18] = 0xA8;    //'D'영역
                                                           //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_InPut_Area[19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_InPut_Area[20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    Data = (Send_Enter_Ok & 0x01);
                    Data |= ((Send_Enter_ING & 0x01) << 1);

                    BufDataSend_InPut_Area[21] = Convert.ToByte(Data);
                    BufDataSend_InPut_Area[22] = 0x00;

                    RequestAGVDataCommunication(2);
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSendRequest_BufDataSend_InPut_Area", Convert.ToString(ex));
            }
        }

        //에어샤워 신호 보내주기 Write
        public void DataSendRequest_BufDataSend_InPut_Air()
        {
            try
            {
                //int CheckSum = 0;

                if (PLC_Sock.Connected == true)
                {
                    #region AGV PLC ReadData 요청
                    int Data = 0;
                    long RMP_StartAdd = 6101;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_InPut_Air[0] = 0x50;
                    BufDataSend_InPut_Air[1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_InPut_Air[2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_InPut_Air[3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_InPut_Air[4] = 0xFF;
                    BufDataSend_InPut_Air[5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_InPut_Air[6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_InPut_Air[7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_InPut_Air[8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                                //------------------------- CPU 감지타이머
                    BufDataSend_InPut_Air[9] = 0x10;
                    BufDataSend_InPut_Air[10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_InPut_Air[11] = 0x01;    // Binary로 읽음
                    BufDataSend_InPut_Air[12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_InPut_Air[13] = 0x00;    // Binary로 읽음
                    BufDataSend_InPut_Air[14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_InPut_Air[15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_InPut_Air[16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_InPut_Air[17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                              //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                              //------------------------- 디바이스 코드
                    BufDataSend_InPut_Air[18] = 0xA8;    //'D'영역
                                                          //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_InPut_Air[19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_InPut_Air[20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High


                    Data = (Send_Door_Open_1 & 0x01);
                    Data |= ((Send_Door_Close_1 & 0x01) << 1);
                    Data |= ((Send_Door_Open_2 & 0x01) << 2);
                    Data |= ((Send_Door_Close_2 & 0x01) << 3);

                    BufDataSend_InPut_Air[21] = Convert.ToByte(Data);
                    BufDataSend_InPut_Air[22] = 0x00;

                    RequestAGVDataCommunication(3);
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSendRequest_BufDataSend_InPut_Air", Convert.ToString(ex));
            }
        }

        //자동문 신호 보내주기 Write
        public void DataSendRequest_BufDataSend_AutoDoor()
        {
            try
            {
                //int CheckSum = 0;
                int Data = 0;
                if (PLC_Sock.Connected == true)
                {
                    #region AGV PLC ReadData 요청

                    long RMP_StartAdd = 6102;
                    short RMP_Length = 1;
                    int RMP_Length_2 = (RMP_Length * 2) + 12;

                    //------------------------- 서브헤더
                    BufDataSend_AutoDoor[0] = 0x50;
                    BufDataSend_AutoDoor[1] = 0x00;
                    //------------------------- 네트워크 번호
                    BufDataSend_AutoDoor[2] = 0x01;
                    //------------------------- PLC 번호
                    BufDataSend_AutoDoor[3] = 0x01;
                    //------------------------- 요구상대모듈 IO번호
                    BufDataSend_AutoDoor[4] = 0xFF;
                    BufDataSend_AutoDoor[5] = 0x03;
                    //------------------------- 요구상대모듈 국번호
                    BufDataSend_AutoDoor[6] = 0x01;
                    //------------------------- 요구데이터 길이
                    BufDataSend_AutoDoor[7] = Convert.ToByte(RMP_Length_2 & 0xff);       // [CPU 감지타이머]부터 [디바이스 점수]까지의 바이트 길이
                    BufDataSend_AutoDoor[8] = Convert.ToByte((RMP_Length_2 >> 8) & 0xff);     // 워드단위 Binary 읽기는 12 고정
                                                                                               //------------------------- CPU 감지타이머
                    BufDataSend_AutoDoor[9] = 0x10;
                    BufDataSend_AutoDoor[10] = 0x00;
                    //------------------------- 커맨드
                    BufDataSend_AutoDoor[11] = 0x01;    // Binary로 읽음
                    BufDataSend_AutoDoor[12] = 0x14;
                    //------------------------- 서브커맨드
                    BufDataSend_AutoDoor[13] = 0x00;    // Binary로 읽음
                    BufDataSend_AutoDoor[14] = 0x00;
                    //------------------------- 선두 디바이스 (Hex변환) -> 900(0x000384)
                    BufDataSend_AutoDoor[15] = Convert.ToByte((RMP_StartAdd) & 0xff); // low
                    BufDataSend_AutoDoor[16] = Convert.ToByte((RMP_StartAdd >> 8) & 0xff); // middle
                    BufDataSend_AutoDoor[17] = Convert.ToByte((RMP_StartAdd >> 16) & 0xff); // high
                                                                                             //m_ucMCP_Read[17] = (MCP_StartAdd >> 24) & 0xff; // high
                                                                                             //------------------------- 디바이스 코드
                    BufDataSend_AutoDoor[18] = 0xA8;    //'D'영역
                                                         //------------------------- 디바이스 점수 : R9400~9479(m_ucRead)
                    BufDataSend_AutoDoor[19] = Convert.ToByte((RMP_Length) & 0xff); // Low
                    BufDataSend_AutoDoor[20] = Convert.ToByte((RMP_Length >> 8) & 0xff); // High

                    Data = (Send_AutoDoor_Open & 0x01);

                    BufDataSend_AutoDoor[21] = Convert.ToByte(Data);
                    BufDataSend_AutoDoor[22] = 0x00;

                    RequestAGVDataCommunication(4);
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Main.Log("Try DataSendRequest_BufDataSend_AutoDoor", Convert.ToString(ex));
            }
        }

        //상태 요청, 차량에 데이터 쓰기 요청
        public void RequestAGVDataCommunication(int type)
        {
            try
            {
                if (PLC_Sock.Connected == true)
                {
                    if (type == 1)
                    {
                        PLC_Sock.BeginSend(BufDataRead, 0, BufDataRead.Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), PLC_Sock);
                        Send_Done.WaitOne();
                        /*
                        PLC_Sock.BeginReceive(BufferData, 0, BufferData.Length, SocketFlags.None,
                                            new AsyncCallback(ReceiveData), PLC_Sock);
                                            */
                    }
                    else if (type == 2)
                    {
                        PLC_Sock.BeginSend(BufDataSend_InPut_Area, 0, BufDataSend_InPut_Area.Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), PLC_Sock);
                        Send_Done.WaitOne();
                    }
                    else if (type == 3)
                    {
                        PLC_Sock.BeginSend(BufDataSend_InPut_Air, 0, BufDataSend_InPut_Air.Length, 0,
                        new AsyncCallback(RequestAGVDataCommunicationCallBack), PLC_Sock);
                        Send_Done.WaitOne();
                    }
                    else if (type == 4)
                    {
                        PLC_Sock.BeginSend(BufDataSend_AutoDoor, 0, BufDataSend_AutoDoor.Length, 0,
                         new AsyncCallback(RequestAGVDataCommunicationCallBack), PLC_Sock);
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
            string log;
            string IP = "";
            string IP_List = "";

            try
            {
                Socket Send_Socket = (Socket)IAR.AsyncState;

                Send_Socket.EndReceive(IAR);
                Send_Done.Set();
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Shutter_RequestAGVDataCommunicationCallBack", Convert.ToString(ex));
            }
        }

    }
}
