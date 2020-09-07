using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace SDI_LCS
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        
        //셔터 I/O모듈 
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_000();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_001();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_002();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_003();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_004();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInit_005();

        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_000();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_001();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_002();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_003();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_004();


        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_AirShower_0();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_AirShower_1();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_AirShower_2();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_AirShower_3();
        [DllImport("SimpleIO.DLL")]
        private static extern long DioInpBit_AirShower_5();
  

        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_000(byte Data);
        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_001(byte Data);
        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_002(byte Data);
        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_003(byte Data);
        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_004(byte Data);

        //열기 신호 - 기재
        [DllImport("SimpleIO.DLL")]
        private static extern long DioOutByte_AirShower_0(byte Data);
        public int Retry_AGV_Num = 0;
        //로그 카운트
        public int[] Count_Drive_Command = new int[LGV_NUM];
        public int[] Count_Drive = new int[LGV_NUM];
        public int[] Count_Drive_Load = new int[LGV_NUM];
        public int[] Count_Drive_Load_Over = new int[LGV_NUM];
        public int[] Count_Traffic = new int[LGV_NUM];
        public int[] Count_Wait_Auto = new int[LGV_NUM];
        public int[] Count_Wait_Manual = new int[LGV_NUM];
        public int[] Count_Charge = new int[LGV_NUM];
        public int[] Count_Loading = new int[LGV_NUM];
        public int[] Count_UnLoading = new int[LGV_NUM];
        public int[] Count_Error = new int[LGV_NUM];
        public int[] Count_etc = new int[LGV_NUM];
        public int[] Count_Traffic_Command = new int[LGV_NUM];
        

        double[] Drive_Command_Rate = new double[LGV_NUM];
        double[] Drive_Rate = new double[LGV_NUM];
        double[] Drive_Load_Rate = new double[LGV_NUM];
        double[] Drive_Load_Over_Rate = new double[LGV_NUM];
        double[] Traffic_Rate = new double[LGV_NUM];
        double[] Wait_Auto_Rate = new double[LGV_NUM];
        double[] Wait_Manual_Rate = new double[LGV_NUM];
        double[] Charge_Rate = new double[LGV_NUM];
        double[] Load_Rate = new double[LGV_NUM];
        double[] UnLoad_Rate = new double[LGV_NUM];
        double[] Error_Rate = new double[LGV_NUM];
        double[] Traffic_Command_Rate = new double[LGV_NUM];


        int[] Sum_Count = new int[LGV_NUM];
        public int idx = -1;
        public int Reconnect_Shutter_Num = 0;
        public int FLAG_Start_Count = 0;
        public int[] Move_Charge_Count = new int[LGV_NUM]; 
        public int T_Shutter_No = 0;
        public bool Mode_AGV_1 = false;
        public bool Mode_AGV_2 = false;
        public bool Mode_Drive = false;
        public bool Mode_Normal = false;
        public bool M_7_SLT_First_Mode = false;
        public bool M_7_SLT_Minus_First_Mode = false;
        public bool M_7_SLT_Normal = true;
        int[] FLAG_Charge_LGV = new int[LGV_NUM];
        int[] Remove_Count = new int[LGV_NUM];
        int Work_Station_LGV_No = 0;
        int IO_Shutter_LGV_NUM = 0;
        public int Shutter_Control_No = 0;
        //상수 선언
        public const string log_on_password = "1231";   //임시 적용 패스워드
        public int Move_Charge_Num = 0;
        public int IO_Sutter_Close_Count_To_Plus = 0;
        public int IO_Sutter_Close_Count_To_Minus = 0;
        public int ReQuest_Abort_AGV_Num = 0;
        public const int SHUTTER_NUM = 150;
        public const int SHUTTER_CONNECT_NUM = 30;
        public const int LGV_NUM = 14;
        public const int MAX_COMMAND_SIZE = 500;
        public const int MAX_RACK_COUNT = 100;
        public const int MAX_TRAFFIC_SIZE = 800;
        public const int MAX_ALARM_SIZE = 50;
        public const int MAX_WORK_STATION = 300;
        public const string EQPNAME = "CCAAGV01";
        public const string EQP_VER = "0.01";
        public string[] Alarm_List = new string[MAX_ALARM_SIZE];
        public int[] Charge_Count = new int[LGV_NUM];
        int[] FLAG_Init_AGV = new int[LGV_NUM];
        int[] FLAG_Init_AGV_Count = new int[LGV_NUM];

        int[] FLAG_Init_AGV_Send_MES = new int[LGV_NUM];
        int[] FLAG_Init_AGV_Count_Send_MES = new int[LGV_NUM];
        public int Move_Charge_Station_Num = 0;

      
        int Close_OK_1__ = 0; //m동 문닫힘센서
        int Close_OK_2__ = 0; //기재 문닫힘센서

        int Open_OK_1__ = 0; //m동 문열림센서
        int Open_OK_2__ = 0; //기재 문열림센서

        int FLAG_OPEN_AIRSHOWER_1 = 0; //m동 문열기
        int FLAG_OPEN_AIRSHOWER_2 = 0; //기재 문열기

        int FLAG_CLOSE_AIRSHOWER = 0; //문닫기


        int FLAG_CLOSE_DOOR = 0; //일정위치에서만 문닫기

        int AutoDoor_Open_OK = 0;//자동문 열림 센서
        int AutoDoor_Open_OK_Packing = 0;//자동문 열림 센서

        #region 통신 관련 변수
        public int[] Send_Abort_Ok = new int[LGV_NUM];
        public int[] Send_Link = new int[LGV_NUM];
        public int[] Send_Traffic = new int[LGV_NUM];
        public int[] Send_Enter = new int[LGV_NUM];
        public int[] Send_Source_Port = new int[LGV_NUM];
        public int[] Send_Dest_Port = new int[LGV_NUM];
        public int[] Send_LGV_Type = new int[LGV_NUM];
        public int[] FLAG_Send_Data_ToAGV = new int[LGV_NUM];
        #endregion


        DateTime[] Charge_Start_Time = new DateTime[LGV_NUM];
        DateTime[] Charge_End_Time = new DateTime[LGV_NUM];
        TimeSpan Charge_Time_Result;

        //폼 선언
        public Form_Setting_Charge Form_Setting_Charge;
        public Form_Select_Traffic Form_Select_Traffic;
        public Form_Traffic_Basic Form_Traffic_Basic;
        public Form_Traffic_Cell Form_Traffic_Cell;
        public Form_Insert_LGV Form_Insert_LGV;
        public Form_MCS Form_MCS;
        public Form_Command_Info Form_Command_Info;
        public Form_AGV_Info Form_AGV_Info;
        public Form_Manual_Command Form_Manual_Command;
        public Form_Work_Path_Setting Form_Work_Path_Setting;
        public Form_Work_Log Form_Work_Log;
        public Form_Error_Log Form_Error_Log;

        public Form_Shutter_Control Form_Shutter_Control;

        public Form_Traffic_Dest Form_Traffic_Dest;

        public Form_Password Form_Password; //임시 패스워드 폼

        //클래스 선언
        //public Connect_TestLGV CS_Connect_LGV;
        public Connect_LGV CS_Connect_LGV;
        public Command_Info[] m_stCommand;
        public Work_DB CS_Work_DB;
        public AGV_Info[] m_stAGV;
        public Work_Path[] CS_Work_Path;
        public WorkSchedule CS_WorkSchedule;
        public Draw_Node_AGV CS_Draw_Node_AGV;
        public Node_DB CS_Node_DB;
        public Traffic CS_Traffic;
        public Traffic_Info[] m_stTraffic;
        public Traffic_Cell[] m_stTraffic_Cell;
        public Traffic_Dest[] m_stTraffic_Dest;

        public AGV_C_Info[] CS_AGV_C_Info;
        public AGV_Logic CS_AGV_Logic;
        
        public Shutter_C_Info[] CS_Shutter_C_Info;
        public Connect_Shutter CS_Connect_Shutter;
        public Shutter_Info[] CS_Shutter_Info;
        public Connect_PLC CS_Connect_PLC;

        //타이머 변수 초기화
        int Read_AGV_Num = 0;
        int Link_AGV_Num = 0;
        int Work_End_LGV_No = 0;
        public int Send_MCS_AGV_Num = 0;
        int Shutter_No = -1;
        int IO_Shutter_No = -1;
        //int Shutter_LGV_No = -1;
        int IO_Shutter_LGV_No = -1;
        int[] FLAG_SEND_AGVInfo = new int[LGV_NUM];
        string[] Before_State = new string[LGV_NUM];
        string[] Before_State_MCS = new string[LGV_NUM];
        int[] Before_Current = new int[LGV_NUM];
        int[] Check_Wait_Position = new int[LGV_NUM];
        public int FLAG_Shtter_On_InPut = 0;
        public int FLAG_Shtter_On_OutPut = 0;
        public int FLAG_IO_Shutter_0 = 0;


        public int FLAG_IO_Shutter_1_To_Plus = 0;
        public int FLAG_IO_Shutter_2_To_Plus = 0;

        public int FLAG_IO_Shutter_1_To_Minus = 0;
        public int FLAG_IO_Shutter_2_To_Minus = 0;
        //변수 선언       
        public int FLAG_Avoid_LGV = 0;
        public int FLAG_TEST_MODE = 0;
        //public int Retry_AGV_Num = 0;
        public int Alarm_AGV_Num = 0;
        public int AlarmClear_AGV_Num = 0;
        private Boolean drag = false;
        private Point Old_point;
        private Point New_point;
        public int[] FLAG_Change_Current = new int[Form1.LGV_NUM];
        public int[] FLAG_Change_State = new int[Form1.LGV_NUM];
        public int[] FLAG_Report_Connect = new int[Form1.LGV_NUM];
        public int[] FLAG_Working_Error = new int[Form1.LGV_NUM];
        public int Flag_MCS_Auto = 0;
        int[] FLAG_MCS_SEND_Assigned = new int[LGV_NUM];
        string[] D_CommandID;
        public int Flag_ReReceive_PLC;
        public int[] Flag_ReReceive_Shutter;
        public int[] Flag_ReReceive;
        public int[] Flag_ReReceive_CallPanel;
        public int[] Flag_ReReceive_Machine;
        public int Real_x; 
        public int Real_y;

        public int[] FLAG_Retry_Count = new int[LGV_NUM];
        double[] Battey = new double[LGV_NUM];
        double[] Battey_End = new double[LGV_NUM];

        public int FLAG_LOG_ON_PASSWORD = 0;        //암호입력이 틀린지 확인
        //타이머 선언
        public System.Timers.Timer[] T_DisConnect_Send_MES = new System.Timers.Timer[LGV_NUM];
        public System.Timers.Timer[] T_DisConnect_Init = new System.Timers.Timer[LGV_NUM];
        public System.Timers.Timer[] T_ReConnect = new System.Timers.Timer[LGV_NUM];
        public System.Timers.Timer[] T_Traffic = new System.Timers.Timer[LGV_NUM];
        public System.Timers.Timer[] T_Change = new System.Timers.Timer[LGV_NUM];
        public System.Timers.Timer[] T_Work_Rate = new System.Timers.Timer[LGV_NUM];
        //public System.Timers.Timer[] T_Work_End = new System.Timers.Timer[LGV_NUM];

        //public System.Timers.Timer[] T_ReConnect_Shutter = new System.Timers.Timer[SHUTTER_NUM];
        public System.Timers.Timer[] T_ReadReQuest_Shutter = new System.Timers.Timer[SHUTTER_CONNECT_NUM];

        //신규 로그 추가. lkw20190109
        public string f_dir, Save_Days, Delete_Time;

        //컨트롤 선언
        #region 컨트롤 선언
        public GroupBox[] GB_LGV_Info = new GroupBox[LGV_NUM];
        public GroupBox[] GB_LGV_Schedule = new GroupBox[LGV_NUM];
        public TextBox[] TB_Schedule = new TextBox[LGV_NUM];
        public Label[] LB_Current = new Label[LGV_NUM];
        public Label[] LB_State = new Label[LGV_NUM];
        public Label[] LB_Goal = new Label[LGV_NUM];
        public Label[] LB_Error = new Label[LGV_NUM];
        public Label[] LB_Battery = new Label[LGV_NUM];
        public TextBox[] TB_Current = new TextBox[LGV_NUM];
        public TextBox[] TB_State = new TextBox[LGV_NUM];
        public TextBox[] TB_Goal = new TextBox[LGV_NUM];
        public TextBox[] TB_Error = new TextBox[LGV_NUM];
        public TextBox[] TB_Battery = new TextBox[LGV_NUM];
        public DevExpress.XtraEditors.SimpleButton[] Btn_Delete_Command = new DevExpress.XtraEditors.SimpleButton[LGV_NUM];
        #endregion

        public Form1()
        {
            InitializeComponent();
            
            //폼 선언
            Form_Shutter_Control = new Form_Shutter_Control(this);
            Form_Setting_Charge = new Form_Setting_Charge(this);
            Form_Work_Log = new Form_Work_Log(this);
            Form_Work_Path_Setting = new Form_Work_Path_Setting(this);
            Form_Select_Traffic = new Form_Select_Traffic(this);
            Form_Traffic_Basic = new Form_Traffic_Basic(this);
            Form_Traffic_Cell = new Form_Traffic_Cell(this);
            Form_MCS = new Form_MCS(this);
            Form_Insert_LGV = new Form_Insert_LGV(this);
            Form_Command_Info = new Form_Command_Info(this);

            Form_AGV_Info = new Form_AGV_Info(this);
            Form_Manual_Command = new Form_Manual_Command(this);
            Form_Error_Log = new Form_Error_Log(this);
            Form_Traffic_Dest = new Form_Traffic_Dest(this);
            Form_Password = new Form_Password(this);

            //클래스 선언
            //CS_Connect_LGV = new Connect_TestLGV(this);
            CS_Connect_LGV = new Connect_LGV(this);
            CS_Connect_PLC = new Connect_PLC(this);
            CS_Connect_Shutter = new Connect_Shutter(this);
            CS_Shutter_Info = new Shutter_Info[SHUTTER_NUM];
            CS_Shutter_C_Info = new Shutter_C_Info[SHUTTER_NUM];
            CS_Work_Path = new Work_Path[MAX_WORK_STATION];
            
            m_stCommand = new Command_Info[MAX_COMMAND_SIZE];
            CS_WorkSchedule = new WorkSchedule(this);
            CS_Work_DB = new Work_DB(this);
            m_stAGV = new AGV_Info[LGV_NUM];
            Flag_ReReceive = new int[LGV_NUM];
            CS_Draw_Node_AGV = new Draw_Node_AGV(this);
            CS_Node_DB = new Node_DB(this);
            CS_Traffic = new Traffic(this);
            m_stTraffic = new Traffic_Info[MAX_TRAFFIC_SIZE];
            m_stTraffic_Cell = new Traffic_Cell[MAX_TRAFFIC_SIZE];
            m_stTraffic_Dest = new Traffic_Dest[MAX_TRAFFIC_SIZE];
            CS_AGV_C_Info = new AGV_C_Info[LGV_NUM];
            CS_AGV_Logic = new AGV_Logic(this);
            //배열 초기화
            D_CommandID = new string[500];
            Flag_ReReceive_Shutter = new int[SHUTTER_CONNECT_NUM];

            for(int i = 0; i < SHUTTER_CONNECT_NUM; i++)
            {
                //셔터 정보 요청 타이머
                T_ReadReQuest_Shutter[i] = new System.Timers.Timer();
                T_ReadReQuest_Shutter[i].Elapsed += new System.Timers.ElapsedEventHandler(Shutter_Read_ReQuest);  //주기마다 실행되는 이벤트 등록
                T_ReadReQuest_Shutter[i].Enabled = true;
                T_ReadReQuest_Shutter[i].Interval = 500;
                Flag_ReReceive_Shutter[i] = 0;
            }

            for (int i = 0; i < SHUTTER_NUM; i ++)
            {
                CS_Shutter_Info[i] = new Shutter_Info();
                CS_Shutter_C_Info[i] = new Shutter_C_Info();

                //셔터 재접속 타이머
                //T_ReConnect_Shutter[i] = new System.Timers.Timer();
                //T_ReConnect_Shutter[i].Elapsed += new System.Timers.ElapsedEventHandler(Shutter_ReConnect);  //주기마다 실행되는 이벤트 등록
                //T_ReConnect_Shutter[i].Enabled = true;
                //T_ReConnect_Shutter[i].Interval = 1000;
                
            }

            for(int i = 0; i < MAX_WORK_STATION; i ++)
            {
                CS_Work_Path[i] = new Work_Path(); 
            }
            for (int i = 0; i < MAX_COMMAND_SIZE; i++)
            {
                m_stCommand[i] = new Command_Info();
            }
            for (int i = 0; i < MAX_TRAFFIC_SIZE; i++)
            {
                m_stTraffic[i] = new Traffic_Info();
                m_stTraffic_Cell[i] = new Traffic_Cell();
                m_stTraffic_Dest[i] = new Traffic_Dest();
            }
            for (int i = 0; i < LGV_NUM; i++)
            {
                Battey[i] = 0;
                Battey_End[i] = 0;
                Charge_Start_Time[i] = new DateTime(2011,11,11,11,11,11);
                Charge_End_Time[i] = new DateTime(2011, 11, 11, 11, 11, 11);

                Move_Charge_Count[i] = 0;
                FLAG_Init_AGV_Count[i] = 0;
                FLAG_Init_AGV[i] = 0;
                Before_State_MCS[i] = "";
                Check_Wait_Position[i] = 0;
                FLAG_Retry_Count[i] = 0;
                m_stAGV[i] = new AGV_Info();
                FLAG_MCS_SEND_Assigned[i] = 0;
                CS_AGV_C_Info[i] = new AGV_C_Info();
                GB_LGV_Info[i] = new GroupBox();
                GB_LGV_Schedule[i] = new GroupBox();
                TB_Schedule[i] = new TextBox();
                LB_Current[i] = new Label();
                LB_State[i] = new Label();
                LB_Goal[i] = new Label();
                LB_Error[i] = new Label();
                LB_Battery[i] = new Label();
                TB_Current[i] = new TextBox();
                TB_State[i] = new TextBox();
                TB_Goal[i] = new TextBox();
                TB_Error[i] = new TextBox();
                TB_Battery[i] = new TextBox();
                Btn_Delete_Command[i] = new DevExpress.XtraEditors.SimpleButton();

                T_ReConnect[i] = new System.Timers.Timer();
                T_Traffic[i] = new System.Timers.Timer();
                T_Change[i] = new System.Timers.Timer();
                T_DisConnect_Init[i] = new System.Timers.Timer();
                T_DisConnect_Send_MES[i] = new System.Timers.Timer();
                T_Work_Rate[i] = new System.Timers.Timer();
                //T_Work_End[i] = new System.Timers.Timer();
                //T_Retry[i] = new System.Timers.Timer();
                T_DisConnect_Send_MES[i].Elapsed += new System.Timers.ElapsedEventHandler(DisConnect_Send_MES_Tick);  //주기마다 실행되는 이벤트 등록
                T_Change[i].Elapsed += new System.Timers.ElapsedEventHandler(T_Change_Tick);  //주기마다 실행되는 이벤트 등록
                T_ReConnect[i].Elapsed += new System.Timers.ElapsedEventHandler(Re_Connect_Tick);  //주기마다 실행되는 이벤트 등록
                T_Traffic[i].Elapsed += new System.Timers.ElapsedEventHandler(Traffic_Tick);  //주기마다 실행되는 이벤트 등록
                //T_Retry[i].Elapsed += new System.Timers.ElapsedEventHandler(Retry_Tick);  //주기마다 실행되는 이벤트 등록
                T_DisConnect_Init[i].Elapsed += new System.Timers.ElapsedEventHandler(DisConnect_Init_Tick);  //주기마다 실행되는 이벤트 등록
                T_Work_Rate[i].Elapsed += new System.Timers.ElapsedEventHandler(Work_Rate_Tick);  //주기마다 실행되는 이벤트 등록
                //T_Work_End[i].Elapsed += new System.Timers.ElapsedEventHandler(Work_End_Tick);  //주기마다 실행되는 이벤트 등록
            }

            #region 알람 리스트
            Alarm_List[0] = "Normal";
            Alarm_List[1] = "Emergency Stop";
            Alarm_List[2] = "Bumper Error";
            Alarm_List[3] = "Line Out";
            Alarm_List[4] = "Localization Data Error";
            Alarm_List[5] = "Navigation Board Error";
            Alarm_List[6] = "Lift Up Limit";
            Alarm_List[7] = "Lift Down Limit";
            Alarm_List[8] = "Lift Down Touch Error";
            Alarm_List[9] = "Stop Position Error";
            Alarm_List[10] = "FW Steering Left Limit";
            Alarm_List[11] = "FW Steering Right Limit";
            Alarm_List[12] = "BW Steering Left Limit";
            Alarm_List[13] = "BW Steering Right Limit";
            Alarm_List[14] = "FW Driving Motor Error";
            Alarm_List[15] = "BW Driving Motor Error";
            Alarm_List[16] = "FW Steering Motor Error";
            Alarm_List[17] = "BW Steering Motor Error";
            Alarm_List[18] = "Lift Motor Error";
            Alarm_List[19] = "Loading Work Time Out";
            Alarm_List[20] = "Unloading Work Time Out";
            Alarm_List[21] = "PIO Error";
            Alarm_List[22] = "Path Plan Error";
            Alarm_List[24] = "Obstacle";
            Alarm_List[25] = "InPut Station Error";
            Alarm_List[26] = "OutPut Station Error";
            Alarm_List[27] = "Product Sensor Error";
            Alarm_List[28] = "Lift Height NG";
            Alarm_List[29] = "Product Empty";
            Alarm_List[30] = "Product Sensor Error to Working";
            Alarm_List[31] = "Shutter Not Open";
            Alarm_List[32] = "Battery Not Charge";
            Alarm_List[33] = "Drive Way Error";
            Alarm_List[34] = "Bumper FREE (Auto Mode)";
            Alarm_List[35] = "Work Delay Error";
            Alarm_List[36] = "Doubleness Load(LGV)";
            Alarm_List[37] = "Doubleness Load(LCS)";
            Alarm_List[38] = "Lift Work Time Out";
            Alarm_List[39] = "Traffic Time Out";//0331 kjh 트래픽 타임아웃 추가

            #endregion
            for (int i = 0; i < MAX_COMMAND_SIZE; i++)
            {
                D_CommandID[i] = "";
            }
            Form_MCS.Init_Mes_Send();
        }
        public void Init_Work_GridView() // 그리드뷰 설정
        {
            gridView1.Columns[0].Caption = "작 업 명";
            gridView1.Columns[1].Caption = "콜 시 간";
            gridView1.Columns[2].Caption = "제 품 명";
            gridView1.Columns[3].Caption = "출 발 지";
            gridView1.Columns[4].Caption = "도 착 지";
            gridView1.Columns[5].Caption = "차량 번호";
            gridView1.Columns[6].Caption = "작업 상태";
            
        }

        //셔터 재접속 - 타이머
        private void Shutter_ReConnect(int Shutter_Num)
        {
            try
            {
                if(Flag_MCS_Auto == 1)
                {
                    if (CS_Shutter_C_Info[Shutter_Num].IP != "" && CS_Shutter_C_Info[Shutter_Num].Port != 0)
                    {
                        Flag_ReReceive_Shutter[Shutter_Num]++;
                    } 
                        
                    if (Flag_ReReceive_Shutter[Shutter_Num] > 10)
                    {
                        if (CS_Connect_Shutter.Shutter_Sock[Shutter_Num].Connected == true)
                        {
                            Flag_ReReceive_Shutter[Shutter_Num] = 0;
                            CS_Connect_Shutter.Dis_Connect(Shutter_Num);
                        }
                        else if (CS_Connect_Shutter.Shutter_Sock[Shutter_Num].Connected == false)
                        {
                            
                            Flag_ReReceive_Shutter[Shutter_Num] = 0;

                            CS_Connect_Shutter.Connect(Shutter_Num, CS_Shutter_C_Info[Shutter_Num].IP, CS_Shutter_C_Info[Shutter_Num].Port);


                        }
                    }
                }
                
            }
            catch (SocketException ex)
            {
                Log("Try Catch_Re_Connect_Tick", Convert.ToString(ex));
            }
            
        }


        //셔터 상태 요청 - 타이머
        private void Shutter_Read_ReQuest(object sender, EventArgs e)
        {
            if (Flag_MCS_Auto == 1)
            {
                int T_Read_Shutter_No = -1;

                for (int i = 0; i < SHUTTER_CONNECT_NUM; i++)
                {
                    if (T_ReadReQuest_Shutter[i] == sender)
                    {
                        T_Read_Shutter_No = i;
                        break;
                    }
                }
                if (T_Read_Shutter_No != -1)
                {
                    CS_Connect_Shutter.DataReadBufferRequest(T_Read_Shutter_No);
                }
            }

        }

        //셔터 상태 요청 - 타이머
        private void Retry_Tick(object sender, EventArgs e)
        {
            /*
            if(Flag_MCS_Auto == 1)
            {
                int Retry_AGV_Num = -1;

                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (T_Retry[i] == sender)
                    {
                        Retry_AGV_Num = i;
                        break;
                    }
                }
                if (Retry_AGV_Num != -1)
                {
                    try
                    {
                        if (m_stAGV[Retry_AGV_Num].dqGoal.Count > 0 && m_stAGV[Retry_AGV_Num].mode == 1
                    && (m_stAGV[Retry_AGV_Num].state == 0 || m_stAGV[Retry_AGV_Num].state == 4 || m_stAGV[Retry_AGV_Num].state == 8
                     || m_stAGV[Retry_AGV_Num].state == 5 || m_stAGV[Retry_AGV_Num].state == 6 || m_stAGV[Retry_AGV_Num].state == 9 || m_stAGV[Retry_AGV_Num].state == 10))
                        {
                            FLAG_Retry_Count[Retry_AGV_Num]++;
                        }
                        else if (m_stAGV[Retry_AGV_Num].dqGoal.Count > 0
                            && (m_stAGV[Retry_AGV_Num].mode == 0 || (m_stAGV[Retry_AGV_Num].state != 0 && m_stAGV[Retry_AGV_Num].state != 4 && m_stAGV[Retry_AGV_Num].state != 8
                         && m_stAGV[Retry_AGV_Num].state != 5 && m_stAGV[Retry_AGV_Num].state != 6 && m_stAGV[Retry_AGV_Num].state != 9 && m_stAGV[Retry_AGV_Num].state != 10)))
                        {
                            FLAG_Retry_Count[Retry_AGV_Num] = 0;
                        }


                        if (FLAG_Retry_Count[Retry_AGV_Num] >= 3)
                        {
                            CS_AGV_Logic.Command_Retry(Retry_AGV_Num);
                            FLAG_Retry_Count[Retry_AGV_Num] = 0;
                        }
                   
                    }
                    catch (Exception ex)
                    {
                        Log("TryCatch Retry_Tick", Convert.ToString(ex));
                    }
                }
            }
            */
        }

        //차량 접속상태 - 보고용
        private void DisConnect_Send_MES_Tick(object sender, EventArgs e)
        {
            int T_DisConnect_LGV_No = -1;

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_DisConnect_Send_MES[i] == sender)
                {
                    T_DisConnect_LGV_No = i;
                    break;
                }
            }
            if (T_DisConnect_LGV_No != -1 && Flag_MCS_Auto == 1)
            {
                if (m_stAGV[T_DisConnect_LGV_No].connect == 0 && FLAG_Init_AGV_Send_MES[T_DisConnect_LGV_No] == 0)
                {
                    FLAG_Init_AGV_Count_Send_MES[T_DisConnect_LGV_No]++;
                }

                if (FLAG_Init_AGV_Count_Send_MES[T_DisConnect_LGV_No] > 120 && FLAG_Init_AGV_Send_MES[T_DisConnect_LGV_No] == 0)
                {
                    Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        FLAG_Init_AGV_Send_MES[T_DisConnect_LGV_No] = 1;
                        FLAG_Init_AGV_Count_Send_MES[T_DisConnect_LGV_No] = 0;

                        //차량 변수에 추가
                        m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_ID = "23";
                        m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_Text = "Vehicle Down";

                        CS_Work_DB.Update_AGV_Info_State(T_DisConnect_LGV_No, m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_ID, "2");

                        if (m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID != "")
                        {
                            Form_MCS.SendS5F1(Form_MCS.m_nDeviceID, 128, m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_ID, m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_Text);
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 102, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 702, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 209, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                        }
                        else if (m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID == "")
                        {
                            Form_MCS.SendS5F1(Form_MCS.m_nDeviceID, 128, m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_ID, m_stAGV[T_DisConnect_LGV_No].MCS_Alarm_Text);
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 702, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                        }
                        //MCS보고 - VehicleStatusChanged(611)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 611, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                        Log("차량 접속 보고", "차량 : " + (T_DisConnect_LGV_No + 1) + "끊김");
                    }));
                }
                else if (FLAG_Init_AGV_Send_MES[T_DisConnect_LGV_No] == 1 && m_stAGV[T_DisConnect_LGV_No].connect == 1)
                {
                    FLAG_Init_AGV_Send_MES[T_DisConnect_LGV_No] = 0;
                    //MCS보고 - VehicleStatusChanged(611)
                    Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 611, 0, Convert.ToUInt16(T_DisConnect_LGV_No), m_stAGV[T_DisConnect_LGV_No].MCS_Vehicle_Command_ID);
                    Log("차량 접속 보고", "차량 : " + (T_DisConnect_LGV_No + 1) + "접속 완료");
                }

            }
        }

        //차량 접속상태 - 타이머
        private void DisConnect_Init_Tick(object sender, EventArgs e)
        {
            int T_DisConnect_LGV_No = -1;

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_DisConnect_Init[i] == sender)
                {
                    T_DisConnect_LGV_No = i;
                    break;
                }
            }
            if (T_DisConnect_LGV_No != -1)
            {
                if (m_stAGV[T_DisConnect_LGV_No].connect == 0 && FLAG_Init_AGV[T_DisConnect_LGV_No] == 0)
                {
                    FLAG_Init_AGV_Count[T_DisConnect_LGV_No]++;
                }

                if (FLAG_Init_AGV_Count[T_DisConnect_LGV_No] > 180 && FLAG_Init_AGV[T_DisConnect_LGV_No] == 0)
                // 2020 05 18 kjh 5분 간격을 3분으로 수정
                {
                    Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        FLAG_Init_AGV[T_DisConnect_LGV_No] = 1;
                        FLAG_Init_AGV_Count[T_DisConnect_LGV_No] = 0;
                        m_stAGV[T_DisConnect_LGV_No].current = 0;
                        TB_Current[T_DisConnect_LGV_No].Text = "0";
                        m_stAGV[T_DisConnect_LGV_No].Goal = 0;
                        TB_Goal[T_DisConnect_LGV_No].Text = "0";
                        m_stAGV[T_DisConnect_LGV_No].state = 0;
                        TB_State[T_DisConnect_LGV_No].Text = "대기";

                    }));
                }
                else if (FLAG_Init_AGV[T_DisConnect_LGV_No] == 1 && m_stAGV[T_DisConnect_LGV_No].connect == 1)
                {
                    FLAG_Init_AGV[T_DisConnect_LGV_No] = 0;
                }
            }
        }

        //충전 시간 카운트 - 타이머
        private void T_Change_Tick(object sender, EventArgs e)
        {
            int T_Change_LGV_No = -1;
            int Check_WaitStation;
            int min = 0;
            int sec = 0;
            string time = "";

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_Change[i] == sender)
                {
                    T_Change_LGV_No = i;
                    break;
                }
            }

            if (T_Change_LGV_No != -1)
            {
                if ((m_stAGV[T_Change_LGV_No].current == 499 || m_stAGV[T_Change_LGV_No].current == 1499 || m_stAGV[T_Change_LGV_No].current == 1438 || m_stAGV[T_Change_LGV_No].current == 166 || m_stAGV[T_Change_LGV_No].current == 1237
                  || m_stAGV[T_Change_LGV_No].current == 438 || m_stAGV[T_Change_LGV_No].current == 496 || m_stAGV[T_Change_LGV_No].current == 1496 || m_stAGV[T_Change_LGV_No].current == 665
                  || m_stAGV[T_Change_LGV_No].current == 1041 || m_stAGV[T_Change_LGV_No].current == 163 || m_stAGV[T_Change_LGV_No].current == 442 || m_stAGV[T_Change_LGV_No].current == 1556) //20200820 신규 충전소 추가
                 && (m_stAGV[T_Change_LGV_No].mode == 1 && m_stAGV[T_Change_LGV_No].state == 9 && m_stAGV[T_Change_LGV_No].dqGoal.Count == 0))
                {
                    Charge_Count[T_Change_LGV_No]++;   
                }
                else if(m_stAGV[T_Change_LGV_No].mode == 0)
                {
                    m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge = 0;
                    Charge_Count[T_Change_LGV_No] = 0;
                    Log("FLAG_LGV_Charge", "리셋 버튼 해제" + (T_Change_LGV_No + 1) + "호, 위치 :" + m_stAGV[T_Change_LGV_No].current);
                }
                
                // 저전압 충전 타임 화면 표시 20181212 lhc
                    if (m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge == 1)
                    {
                        min = (Form_Setting_Charge.Charge_Count - Charge_Count[T_Change_LGV_No]) / 60;               // 남은 시간 분 (설정 충전 시간 - 충전 진행 시간) / 60
                        sec = (Form_Setting_Charge.Charge_Count - Charge_Count[T_Change_LGV_No]) - min * 60;         // 남은 시간 초 (설정 충전 시간 - 충전 진행 시간) / (남은 시간 분 * 60) 

                        time = Convert.ToString((String.Format("{0:D2}:", min))) + Convert.ToString((String.Format("{0:D2}", sec)));

                        Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            TB_Battery[T_Change_LGV_No].Text = time;
                            TB_Battery[T_Change_LGV_No].BackColor = Color.Orange;
                        }));
                    }
          
                //충전소 에서 정해진 시간 동안 충전하기
                if (Charge_Count[T_Change_LGV_No] >= Form_Setting_Charge.Charge_Count)
                {
                    if (CS_AGV_C_Info[T_Change_LGV_No].Type == "ROLL_음극")
                    {
                        if (m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge == 1)
                        {
                            m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge = 0;
                            Charge_Count[T_Change_LGV_No] = 0;
                            Log("FLAG_LGV_Charge", "충전 시간 카운트 해제" + (T_Change_LGV_No + 1) + "호, 위치 :" + m_stAGV[T_Change_LGV_No].current);
                        }
                    }
                    else if (CS_AGV_C_Info[T_Change_LGV_No].Type == "ROLL_양극" )
                    {
                        if (m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge == 1)
                        {
                            m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge = 0;
                            Charge_Count[T_Change_LGV_No] = 0;
                            Log("FLAG_LGV_Charge", "충전 시간 카운트 해제" + (T_Change_LGV_No + 1) + "호, 위치 :" + m_stAGV[T_Change_LGV_No].current);
                        }
                    }
                    
                    else if (CS_AGV_C_Info[T_Change_LGV_No].Type == "REEL_양극" || CS_AGV_C_Info[T_Change_LGV_No].Type == "REEL_음극" 
                          || CS_AGV_C_Info[T_Change_LGV_No].Type == "REEL_공용")
                    { 
                        if (m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge == 1)
                        {
                            m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge = 0;
                            Charge_Count[T_Change_LGV_No] = 0;
                            Log("FLAG_LGV_Charge", "충전 시간 카운트 해제" + (T_Change_LGV_No + 1) + "호, 위치 :" + m_stAGV[T_Change_LGV_No].current);
                        }
                    }

                    else if (CS_AGV_C_Info[T_Change_LGV_No].Type == "광폭_음극" || CS_AGV_C_Info[T_Change_LGV_No].Type == "광폭_양극")
                    {
                        if (m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge == 1)
                        {
                            m_stAGV[T_Change_LGV_No].FLAG_LGV_Charge = 0;
                            Charge_Count[T_Change_LGV_No] = 0;
                            Log("FLAG_LGV_Charge", "충전 시간 카운트 해제" + (T_Change_LGV_No + 1) + "호, 위치 :" + m_stAGV[T_Change_LGV_No].current);
                        }
                    }
                }
            }
        }

        //차량 재접속 - 타이머
        private void Re_Connect_Tick(object sender, EventArgs e)
        {
            try
            {
                int ReConnect_LGV_No = -1;

                for (int i = 0; i < LGV_NUM; i++)
                {
                    if (T_ReConnect[i] == sender)
                    {
                        ReConnect_LGV_No = i;
                        break;
                    }
                } 
                if (ReConnect_LGV_No != -1)
                {
                    Flag_ReReceive[ReConnect_LGV_No]++;

                    if (Flag_ReReceive[ReConnect_LGV_No] > 10) // 0414 3초 → 10초로 수정 
                    {
                        if (CS_Connect_LGV.LGV_Sock[ReConnect_LGV_No].Connected == true)
                        {
                            Flag_ReReceive[ReConnect_LGV_No] = 0;
                            //0309 통신 수정 - 0414 주석 해제 
                            CS_Connect_LGV.LGV_Dis_Connect(ReConnect_LGV_No);
                        }
                        else if (CS_Connect_LGV.LGV_Sock[ReConnect_LGV_No].Connected == false)
                        {
                            Flag_ReReceive[ReConnect_LGV_No] = 0;
                            CS_Connect_LGV.Connect(ReConnect_LGV_No, CS_AGV_C_Info[ReConnect_LGV_No].IP, CS_AGV_C_Info[ReConnect_LGV_No].Port);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Log("Try Catch_Re_Connect_Tick", Convert.ToString(ex));
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Form_Password.ShowDialog();

            if (FLAG_LOG_ON_PASSWORD == 1)
            {
                FLAG_LOG_ON_PASSWORD = 0;   //암호일치여부 초기화;
                Form_Work_Path_Setting.ShowDialog();
            }
        }
        //현재시간 가져오기
        public string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }
        //로그 찍기
        public void Log(string LogName, string str)
        {
            /*
            string FilePath = Application.StartupPath + @"\Logs\" + DateTime.Today.ToString("yyyyMMdd") + LogName + ".log";
            string DirPath = Application.StartupPath + @"\Logs";
            string temp;
            */
            //신규 로그 추가. lkw20190109
            DateTime dt = DateTime.Now;
            string date = dt.ToString("yyyyMMdd");
            string FilePath = f_dir + "\\" + date + "\\" + DateTime.Today.ToString("yyyyMMdd") + LogName + ".log";
            string DirPath = f_dir + "\\" + date + "\\";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);

                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0} : {1}]", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0} : {1}]", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Log("TraCatch LOG", Convert.ToString(e));
            }
        }
        
        private void Form1_Shown(object sender, EventArgs e)
        {
            //this.Text = this.Text + "  " +  ;

            /****************************************************************************************************/
            #region Init
            Ini_init_read();        //20190107 lhc
            T_Log_Delete.Enabled = true;
            #endregion
            /****************************************************************************************************/

            for (int i = 0; i < LGV_NUM; i++)
            {
                CS_Work_DB.Select_DB_Recovery(i);
                //가동률 로그 정지함. work_rate의 데이터가 많으면 많을 수록
                //프로그램이 다시 시작하는데 많은 시간이 걸림. lkw20190416
                //Init_Count_Info(i);
            }

            Form_Shutter_Control.Select_ShutterList();
            CS_Work_DB.Select_Work_Path();
            CS_Work_DB.Select_MCS_Command_Info(0);
            CS_Work_DB.Select_MCS_Command_Info_Main(0);
            CS_Work_DB.Select_MCS_AGV_Setting_Battery();
            CS_Work_DB.Select_MCS_AGV_Info();
             
            //CS_Work_DB.Select_DB_Error_Log();
            CS_Work_DB.Select_Rack_Info();
            //CS_Node_DB.Select_Excel_Data();
            //CS_Node_DB.Select_Excel_Path();
            //CS_Draw_Node_AGV.Draw_Node();
            //CS_Draw_Node_AGV.Draw_Path();
            CS_Work_DB.Select_AGVList();
            Form_Traffic_Basic.Select_Traffic_Basic();
            Form_Traffic_Cell.Select_Traffic_Cell();
            Form_Traffic_Dest.Select_Traffic_Dest();
            
            
            CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
            //MCS OPEN
            Form_MCS.m_XComPro.OnSecsEvent += new ON_SECSEVENT(Form_MCS.m_XComPro_OnSecsEvent);
            Form_MCS.m_XComPro.OnSecsMsg += new ON_SECSMSG(Form_MCS.m_XComPro_OnSecsMsg);
            int ret = Form_MCS.m_XComPro.Initialize("EqSample.cfg");
            if (ret < 0)
            {
                Form_MCS.AddLog("XComPro initialization failed: XComPro error={0}", ret);
                Form_MCS.btStartXCom.Enabled = false;
                Form_MCS.btStopXCom.Enabled = false;
                return;
            }
            Form_MCS.m_XComPro.Start();
            Form_MCS.AddLog("XComPro was initialized");           
        }

        //************************************************************
        // Ini file  read       // 20190107 lhc
        //************************************************************
        public void Ini_init_read()
        {
            try
            {
                //  log data
                f_dir = IniControl.getIni("", "LOG", "LogDir");
                if (f_dir == "error")
                    f_dir = @"C:\LCS_LOG\";

                Save_Days = IniControl.getIni("", "LOG", "Save_Days");
                Delete_Time = IniControl.getIni("", "LOG", "Delete_time");
            }
            catch (Exception e)
            {

            }

        }

        //************************************************************
        //로그 삭제   // 20190107 lhc
        //************************************************************
        public void log_data_delete()
        {
            string dir;
            string run_time = Delete_Time;
            DateTime dt = DateTime.Now;
            dt = dt.AddDays(-1 * Convert.ToDouble(Save_Days));       // 현재날짜에서 30일 이전
            string now_date = dt.ToString("yyyyMMdd");
            string now_time = dt.ToString("HH");

            // 00시 삭제한다.
            if (now_time != run_time) return;

            try
            {
                dir = f_dir;

                DirectoryInfo dinfo = new DirectoryInfo(dir);
                System.IO.DirectoryInfo[] dir_list = dinfo.GetDirectories();

                string ck_list = dir_list[0].Name;
                if (Convert.ToInt32(ck_list) < Convert.ToInt32(now_date))
                {
                    dir_list[0].Delete(true);
                }

                //DB 에러 로그 삭제. 삭제 기준은 LOG 삭제 기준일과 동일하다. lkw20190610
                CS_Work_DB.delete_tb_error_log(dt);
            }
            catch (Exception e)
            {

            }
        }

        //************************************************************
        //로그 삭제 타이머   // 20190107 lhc
        //************************************************************
        private void T_Log_Delete_Tick(object sender, EventArgs e)
        {
            log_data_delete();
        }


        private void Traffic_Tick(object sender, EventArgs e)
        {
            int Traffic_LGV_No = -1;

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_Traffic[i] == sender)
                {
                    Traffic_LGV_No = i;
                    
                    break;
                }
            }
            if (Traffic_LGV_No != -1)
            {
                CS_Traffic.Move_Traffic(Traffic_LGV_No);
            }
        }
        
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Form_Work_Log.ShowDialog();
        }
       
        public int Get_Command_Index(string CMD_ID)
        {
            for (int i = 0; i < CS_Work_DB.Working_Command_Count; i++)
            {
                if(Form_MCS.MCS_Command_ID[i] == CMD_ID)
                {
                    return i;
                }
            }
            return -1;
        }
        private void simpleButton11_Click(object sender, EventArgs e)
        {
            int i;

            if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
            DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];
            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
            }

            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                if(Convert.ToString(rows[i][6]) == "대기중")
                {
                    D_CommandID[i] = Convert.ToString(rows[i][0]);
                    Form_MCS.Command_Type = "CANCEL";

                    Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 501, 0, 0, D_CommandID[i]);
                    Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 204, 0, 0, D_CommandID[i]);
         
                    CS_AGV_Logic.Init_WaitCommand();
                    CS_Work_DB.Select_MCS_Command_Info_Main(0);
                    CS_Work_DB.Select_MCS_Command_Info(0);
                    CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                    Log("Command Manual", "취소" + D_CommandID[i]);
                }
                else if (Convert.ToString(rows[i][6]) == "진행중" || Convert.ToString(rows[i][6]) == "진행중(취소 X)")
                {
                    D_CommandID[i] = Convert.ToString(rows[i][0]);
                    Form_MCS.Command_Type = "ABORT";


                    if ((Convert.ToString(rows[i][5])) == "1호-양극Reel")
                    {
                        idx = 0;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "2호-음극Reel")
                    {
                        idx = 1;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "3호-양극Roll")
                    {
                        idx = 2;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "4호-양극Roll")
                    {
                        idx = 3;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "5호-양극Roll")
                    {
                        idx = 4;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "6호-음극Roll")
                    {
                        idx = 5;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "7호-음극Roll")
                    {
                        idx = 6;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "8호-음극Roll")
                    {
                        idx = 7;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "음극 광폭")
                    {
                        idx = 8;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "10호")
                    {
                        idx = 9;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "양극 광폭")
                    {
                        idx = 10;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "12호-음극Reel")
                    {
                        idx = 11;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "13호-양극Reel")
                    {
                        idx = 12;
                    }

                    if (idx > -1)
                    {
                        if (m_stAGV[idx].MCS_Carrier_ID == "")
                        {
                            m_stAGV[idx].MCS_Carrier_LOC = "";
                            m_stAGV[idx].MCS_Carrier_Type = "";
                        }
                        else if (m_stAGV[idx].MCS_Carrier_ID != "")
                        {
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Vehicle_ID;

                            if (idx == 0 || idx == 1 || idx == 9 || idx == 11 || idx == 12)
                            {
                                //REEL
                                m_stAGV[idx].MCS_Carrier_Type = "3";
                            }
                            else if (idx == 2 || idx == 3 || idx == 4)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "5";
                            }
                            else if (idx == 5 || idx == 6 || idx == 7)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "6";
                            }
                            else if (idx == 8)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "9";
                            }
                        }
                         
                        // 사양서와 다름 수정함(3/9)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 501, 0, Convert.ToUInt16(idx), D_CommandID[i]);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 203, 0, Convert.ToUInt16(idx), D_CommandID[i]);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 610, 0, Convert.ToUInt16(idx), D_CommandID[i]);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 201, 0, Convert.ToUInt16(idx), D_CommandID[i]);

                        CS_AGV_Logic.Init_WaitCommand();
                        m_stAGV[idx].dqGoal.Clear();
                        m_stAGV[idx].dqWork.Clear();
                        m_stAGV[idx].Working = 0;
                        TB_Schedule[idx].Clear();
                        m_stAGV[idx].MCS_Vehicle_Command_ID = "";
                        CS_Work_DB.DELETE_DB_Recovery(idx);

                        CS_Work_DB.Delete_Work_Log(D_CommandID[i]);
                        CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                        CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        Log("Command Manual", "중단" + D_CommandID[i]);
                        //작업 종료 업데이트                
                        m_stAGV[idx].Working = 0;

                        Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            TB_Schedule[idx].Clear();
                        }));

                        CS_AGV_Logic.Init_AGVInfo(idx);
                    }
                }
                else if (Convert.ToString(rows[i][6]) == "진행중(중단 X)")
                {
                    D_CommandID[i] = Convert.ToString(rows[i][0]);

                    if ((Convert.ToString(rows[i][5])) == "1호-양극Reel")
                    {
                        idx = 0;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "2호-음극Reel")
                    {
                        idx = 1;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "3호-양극Roll")
                    {
                        idx = 2;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "4호-양극Roll")
                    {

                        idx = 3;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "5호-양극Roll")
                    {
                        idx = 4;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "6호-음극Roll")
                    {
                        idx = 5;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "7호-음극Roll")
                    {
                        idx = 6;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "8호-음극Roll")
                    {
                        idx = 7;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "음극 광폭")
                    {
                        idx = 8;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "10호")
                    {
                        idx = 9;
                    }
                    else if ((Convert.ToString(rows[i][5])) == "양극 광폭")
                    {
                        idx = 10;
                    }

                    if (idx > -1)
                    {
                        CS_AGV_Logic.Init_WaitCommand();
                        m_stAGV[idx].dqGoal.Clear();
                        m_stAGV[idx].dqWork.Clear();
                        m_stAGV[idx].Working = 0;
                        TB_Schedule[idx].Clear();
                        m_stAGV[idx].MCS_Vehicle_Command_ID = "";
                        
                        CS_Work_DB.DELETE_DB_Recovery(idx);

                        CS_Work_DB.Select_MCS_Command_Info_Log(D_CommandID[i]);

                        CS_Work_DB.Delete_Work_Log(D_CommandID[i]);

                        //엑셀에 작업 로그 저장
                        CS_Work_DB.Insert_Excel_Data_Log(CS_AGV_Logic.E_Call_Time, CS_AGV_Logic.E_Command_ID, CS_AGV_Logic.E_Carrier_ID, CS_AGV_Logic.E_Source_Port, CS_AGV_Logic.E_Dest_Port, CS_AGV_Logic.E_LGV_No, CS_AGV_Logic.E_Alloc_Time, CS_AGV_Logic.E_Load_Move_Time,
                            CS_AGV_Logic.E_Load_Time, CS_AGV_Logic.E_UnLoad_Move_Time, CS_AGV_Logic.E_UnLoad_Time, "삭제", CS_AGV_Logic.E_Load_Move_Time_Result, CS_AGV_Logic.E_Load_Time_Result, CS_AGV_Logic.E_UnLoad_Move_Time_Result,
                            CS_AGV_Logic.E_UnLoad_Time_Result, CS_AGV_Logic.E_Total_Time_Result, CS_AGV_Logic.E_Complete_Time, CS_AGV_Logic.Alloc_Current[idx]);

                        CS_Work_DB.Select_MCS_Command_Info(idx);
                        CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                        CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        //작업 종료 업데이트                
                        m_stAGV[idx].Working = 0;

                        Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            TB_Schedule[idx].Clear();
                        }));

                        CS_AGV_Logic.Init_AGVInfo(idx);
                    }
                }
            }
        }

        private void simpleButton1_Click_2(object sender, EventArgs e)
        {
            int i;
            int idx = -1;
            if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
            DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];
            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
            }

            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                
                
            }
        }

        //컨트롤 폼 만들기 - 차량추가
        public void Insert_Control_AGV(int LGV_No)
        {
            int AGV_No = LGV_No + 1;
            Point LOT_GB_LGV_Info = new System.Drawing.Point();
            //LGV정보 Groupbox 위치 정해주기

            /*if (LGV_No == 0) LOT_GB_LGV_Info = new System.Drawing.Point(3,5);
            if (LGV_No == 1) LOT_GB_LGV_Info = new System.Drawing.Point(211, 5);
            if (LGV_No == 2) LOT_GB_LGV_Info = new System.Drawing.Point(3, 165);
            if (LGV_No == 3) LOT_GB_LGV_Info = new System.Drawing.Point(211, 165);
            if (LGV_No == 4) LOT_GB_LGV_Info = new System.Drawing.Point(3, 325);
            if (LGV_No == 5) LOT_GB_LGV_Info = new System.Drawing.Point(211, 325);
            if (LGV_No == 6) LOT_GB_LGV_Info = new System.Drawing.Point(3, 485);
            if (LGV_No == 7) LOT_GB_LGV_Info = new System.Drawing.Point(211, 485);
            if (LGV_No == 8) LOT_GB_LGV_Info = new System.Drawing.Point(3, 645);
            if (LGV_No == 9) LOT_GB_LGV_Info = new System.Drawing.Point(211, 645);
            if (LGV_No == 10) LOT_GB_LGV_Info = new System.Drawing.Point(3, 805);
            if (LGV_No == 11) LOT_GB_LGV_Info = new System.Drawing.Point(211, 805);*/


            if (LGV_No == 0) LOT_GB_LGV_Info = new System.Drawing.Point(3, 3);
            if (LGV_No == 1) LOT_GB_LGV_Info = new System.Drawing.Point(210, 3);
            if (LGV_No == 2) LOT_GB_LGV_Info = new System.Drawing.Point(3, 124);
            if (LGV_No == 3) LOT_GB_LGV_Info = new System.Drawing.Point(210, 124);
            if (LGV_No == 4) LOT_GB_LGV_Info = new System.Drawing.Point(3, 245);
            if (LGV_No == 5) LOT_GB_LGV_Info = new System.Drawing.Point(210, 245);
            if (LGV_No == 6) LOT_GB_LGV_Info = new System.Drawing.Point(3, 366);
            if (LGV_No == 7) LOT_GB_LGV_Info = new System.Drawing.Point(210, 366);
            if (LGV_No == 8) LOT_GB_LGV_Info = new System.Drawing.Point(3, 487);
            if (LGV_No == 9) LOT_GB_LGV_Info = new System.Drawing.Point(210, 487);
            if (LGV_No == 10) LOT_GB_LGV_Info = new System.Drawing.Point(3, 608);
            if (LGV_No == 11) LOT_GB_LGV_Info = new System.Drawing.Point(210, 608);
            if (LGV_No == 12) LOT_GB_LGV_Info = new System.Drawing.Point(3, 729);
            if (LGV_No == 13) LOT_GB_LGV_Info = new System.Drawing.Point(210, 729);
            if (LGV_No == 14) LOT_GB_LGV_Info = new System.Drawing.Point(3, 850);


            T_DisConnect_Init[LGV_No].Enabled = true;
            T_DisConnect_Init[LGV_No].Interval = 1000;

            T_ReConnect[LGV_No].Enabled = true;
            T_ReConnect[LGV_No].Interval = 1000;

            T_Traffic[LGV_No].Enabled = true;
            T_Traffic[LGV_No].Interval = 300;

            T_Change[LGV_No].Enabled = true;
            T_Change[LGV_No].Interval = 1000;

            //가동률 로그 정지함. work_rate의 데이터가 많으면 많을 수록
            //프로그램이 다시 시작하는데 많은 시간이 걸림. lkw20190416
            T_Work_Rate[LGV_No].Enabled = false;
            //T_Work_Rate[LGV_No].Enabled = true;
            T_Work_Rate[LGV_No].Interval = 1000;

            T_DisConnect_Send_MES[LGV_No].Enabled = true;
            T_DisConnect_Send_MES[LGV_No].Interval = 1000;
            //T_Work_End[LGV_No].Enabled = true;
            //T_Work_End[LGV_No].Interval = 300;


            /* GB_LGV_Schedule[LGV_No].Visible = true;
             GB_LGV_Schedule[LGV_No].BackColor = Color.FromArgb(51, 51, 51);
             GB_LGV_Schedule[LGV_No].Width = 90;
             GB_LGV_Schedule[LGV_No].Height = 104;
             GB_LGV_Schedule[LGV_No].Text = "스케줄";
             GB_LGV_Schedule[LGV_No].Location = new System.Drawing.Point(104, 15);
             GB_LGV_Schedule[LGV_No].PerformLayout();
             GB_LGV_Schedule[LGV_No].BringToFront();

             GB_LGV_Schedule[LGV_No].ForeColor = Color.FromArgb(192, 255, 192);
             GB_LGV_Schedule[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

             TB_Schedule[LGV_No].Visible = true;
             TB_Schedule[LGV_No].Multiline = true;
             TB_Schedule[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_Schedule[LGV_No].Location = new System.Drawing.Point(4, 17);
             TB_Schedule[LGV_No].Visible = true;
             TB_Schedule[LGV_No].Size = new System.Drawing.Size(82, 57);

             Btn_Delete_Command[LGV_No].Visible = true;
             Btn_Delete_Command[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             Btn_Delete_Command[LGV_No].Location = new System.Drawing.Point(4, 78);
             Btn_Delete_Command[LGV_No].ForeColor = Color.FromArgb(255, 128, 128);
             Btn_Delete_Command[LGV_No].BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
             Btn_Delete_Command[LGV_No].Click += new System.EventHandler(this.Delete_Command_Click);
             Btn_Delete_Command[LGV_No].Text = "명령 삭제";
             Btn_Delete_Command[LGV_No].Size = new System.Drawing.Size(81, 22);

             GB_LGV_Info[LGV_No].Visible = true;
             GB_LGV_Info[LGV_No].BackColor = Color.FromArgb(51, 51, 51);
             GB_LGV_Info[LGV_No].Width = 200;
             GB_LGV_Info[LGV_No].Height = 155;
             GB_LGV_Info[LGV_No].Text = "LGV " + AGV_No + "호";
             GB_LGV_Info[LGV_No].Location = LOT_GB_LGV_Info;
             GB_LGV_Info[LGV_No].PerformLayout();
             GB_LGV_Info[LGV_No].ForeColor = Color.Red;
             GB_LGV_Info[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

             TB_Current[LGV_No].Visible = true;
             TB_Current[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_Current[LGV_No].Location = new System.Drawing.Point(57, 22);
             TB_Current[LGV_No].Visible = true;
             TB_Current[LGV_No].Size = new System.Drawing.Size(46, 22);

             TB_Goal[LGV_No].Visible = true;
             TB_Goal[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_Goal[LGV_No].Location = new System.Drawing.Point(57, 47);
             TB_Goal[LGV_No].Visible = true;
             TB_Goal[LGV_No].Size = new System.Drawing.Size(46, 22);

             TB_State[LGV_No].Visible = true;
             TB_State[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_State[LGV_No].Location = new System.Drawing.Point(57, 72);
             TB_State[LGV_No].Visible = true;
             TB_State[LGV_No].Size = new System.Drawing.Size(46, 22);

             TB_Error[LGV_No].Visible = true;
             TB_Error[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_Error[LGV_No].Location = new System.Drawing.Point(57, 122);
             TB_Error[LGV_No].Visible = true;
             TB_Error[LGV_No].Size = new System.Drawing.Size(137, 22);
             TB_Error[LGV_No].TextAlign = HorizontalAlignment.Center;

             TB_Battery[LGV_No].Visible = true;
             TB_Battery[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
             TB_Battery[LGV_No].Location = new System.Drawing.Point(57, 97);
             TB_Battery[LGV_No].Visible = true;
             TB_Battery[LGV_No].Size = new System.Drawing.Size(46, 22);

             LB_Current[LGV_No].Visible = true;
             LB_Current[LGV_No].Size = new System.Drawing.Size(42, 15);
             LB_Current[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
             LB_Current[LGV_No].Text = "위 치 :";
             LB_Current[LGV_No].Location = new System.Drawing.Point(13, 26);
            // LB_Current[LGV_No].BackColor = Color.Red;
             LB_Current[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);
             LB_Current[LGV_No].Visible = true;
             LB_Current[LGV_No].SendToBack();

             LB_Goal[LGV_No].Visible = true;
             LB_Goal[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
             LB_Goal[LGV_No].Text = "목적지 :";
             LB_Goal[LGV_No].Location = new System.Drawing.Point(5, 50);
             LB_Goal[LGV_No].Visible = true;
             LB_Goal[LGV_No].Size = new System.Drawing.Size(50, 15);
             //LB_Goal[LGV_No].BackColor = Color.Red;
             LB_Goal[LGV_No].ForeColor = Color.FromArgb(255, 255, 128); 
             LB_Goal[LGV_No].SendToBack();

             LB_State[LGV_No].Visible = true;
             LB_State[LGV_No].Size = new System.Drawing.Size(42, 15);
             LB_State[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
             LB_State[LGV_No].Text = "상 태 :";
             LB_State[LGV_No].Location = new System.Drawing.Point(13, 76);
             LB_State[LGV_No].Visible = true;
            // LB_State[LGV_No].BackColor = Color.Red;
             LB_State[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);
             LB_State[LGV_No].SendToBack();

             LB_Error[LGV_No].Visible = true;
             LB_Error[LGV_No].Size = new System.Drawing.Size(42, 15);
             LB_Error[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
             LB_Error[LGV_No].Text = "에 러 :";
             LB_Error[LGV_No].Location = new System.Drawing.Point(13, 126);
             LB_Error[LGV_No].Visible = true;
            // LB_Error[LGV_No].BackColor = Color.Red;
             LB_Error[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);
             LB_Error[LGV_No].SendToBack();

             LB_Battery[LGV_No].Visible = true;
             LB_Battery[LGV_No].Size = new System.Drawing.Size(50, 15);
             LB_Battery[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
             LB_Battery[LGV_No].Text = "배터리 :";
             LB_Battery[LGV_No].Location = new System.Drawing.Point(5, 101);
             LB_Battery[LGV_No].Visible = true;
             //LB_Battery[LGV_No].BackColor = Color.Red;
             LB_Battery[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);
             LB_Battery[LGV_No].SendToBack();

             GB_LGV_Schedule[LGV_No].Controls.Add(TB_Schedule[LGV_No]);
             GB_LGV_Schedule[LGV_No].Controls.Add(Btn_Delete_Command[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(TB_Current[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(TB_Goal[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(TB_State[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(TB_Error[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(TB_Battery[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(LB_Current[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(LB_Goal[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(LB_State[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(LB_Error[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(LB_Battery[LGV_No]);
             GB_LGV_Info[LGV_No].Controls.Add(GB_LGV_Schedule[LGV_No]);*/



            GB_LGV_Schedule[LGV_No].Visible = true;
            GB_LGV_Schedule[LGV_No].BackColor = Color.FromArgb(51, 51, 51);
            GB_LGV_Schedule[LGV_No].Width = 91;
            GB_LGV_Schedule[LGV_No].Height = 82;
            GB_LGV_Schedule[LGV_No].Text = "스케줄";
            GB_LGV_Schedule[LGV_No].Location = new System.Drawing.Point(105, 9);
            GB_LGV_Schedule[LGV_No].PerformLayout();
            GB_LGV_Schedule[LGV_No].TabIndex = 44;
            GB_LGV_Schedule[LGV_No].ForeColor = Color.FromArgb(192, 255, 192);
            GB_LGV_Schedule[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));


            TB_Schedule[LGV_No].Visible = true;
            TB_Schedule[LGV_No].Multiline = true;
            TB_Schedule[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_Schedule[LGV_No].TabIndex = 13;
            TB_Schedule[LGV_No].Location = new System.Drawing.Point(4, 15);
            TB_Schedule[LGV_No].Visible = true;
            TB_Schedule[LGV_No].Size = new System.Drawing.Size(82, 38);

            Btn_Delete_Command[LGV_No].Visible = true;
            Btn_Delete_Command[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            Btn_Delete_Command[LGV_No].Location = new System.Drawing.Point(3, 55);
            Btn_Delete_Command[LGV_No].ForeColor = Color.FromArgb(255, 128, 128);
            Btn_Delete_Command[LGV_No].BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            Btn_Delete_Command[LGV_No].Click += new System.EventHandler(this.Delete_Command_Click);
            Btn_Delete_Command[LGV_No].Text = "명령 삭제";
            Btn_Delete_Command[LGV_No].Size = new System.Drawing.Size(84, 23);

            GB_LGV_Info[LGV_No].Visible = true;
            GB_LGV_Info[LGV_No].BackColor = Color.FromArgb(51, 51, 51);
            GB_LGV_Info[LGV_No].Width = 200;
            GB_LGV_Info[LGV_No].Height = 118;
            GB_LGV_Info[LGV_No].Text = "LGV " + AGV_No + "호";
            GB_LGV_Info[LGV_No].Location = LOT_GB_LGV_Info;
            GB_LGV_Info[LGV_No].PerformLayout();
            GB_LGV_Info[LGV_No].TabIndex = 44;
            GB_LGV_Info[LGV_No].ForeColor = Color.Red;
            GB_LGV_Info[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

            TB_Current[LGV_No].Visible = true;
            TB_Current[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_Current[LGV_No].TabIndex = 13;
            TB_Current[LGV_No].Location = new System.Drawing.Point(52, 18);
            TB_Current[LGV_No].Visible = true;
            TB_Current[LGV_No].Size = new System.Drawing.Size(44, 21);

            TB_Goal[LGV_No].Visible = true;
            TB_Goal[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_Goal[LGV_No].TabIndex = 13;
            TB_Goal[LGV_No].Location = new System.Drawing.Point(52, 43);
            TB_Goal[LGV_No].Visible = true;
            TB_Goal[LGV_No].Size = new System.Drawing.Size(44, 21);

            TB_State[LGV_No].Visible = true;
            TB_State[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_State[LGV_No].TabIndex = 13;
            TB_State[LGV_No].Location = new System.Drawing.Point(52, 68);
            TB_State[LGV_No].Visible = true;
            TB_State[LGV_No].Size = new System.Drawing.Size(44, 21);

            TB_Error[LGV_No].Visible = true;
            TB_Error[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_Error[LGV_No].TabIndex = 13;
            TB_Error[LGV_No].Location = new System.Drawing.Point(52, 90);
            TB_Error[LGV_No].Visible = true;
            TB_Error[LGV_No].Size = new System.Drawing.Size(44, 21);
            TB_Error[LGV_No].TextAlign = HorizontalAlignment.Center;

            TB_Battery[LGV_No].Visible = true;
            TB_Battery[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            TB_Battery[LGV_No].TabIndex = 13;
            TB_Battery[LGV_No].Location = new System.Drawing.Point(152, 91);
            TB_Battery[LGV_No].Visible = true;
            TB_Battery[LGV_No].Size = new System.Drawing.Size(44, 21);

            LB_Current[LGV_No].Visible = true;
            LB_Current[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            LB_Current[LGV_No].TabIndex = 10;
            LB_Current[LGV_No].Text = "위   치 :";
            LB_Current[LGV_No].Location = new System.Drawing.Point(3, 23);
            LB_Current[LGV_No].BackColor = Color.Transparent;
            LB_Current[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);
            LB_Current[LGV_No].Visible = true;


            LB_Goal[LGV_No].Visible = true;
            LB_Goal[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            LB_Goal[LGV_No].TabIndex = 10;
            LB_Goal[LGV_No].Text = "목적지 :";
            LB_Goal[LGV_No].Location = new System.Drawing.Point(3, 47);
            LB_Goal[LGV_No].Visible = true;
            LB_Goal[LGV_No].BackColor = Color.Transparent;
            LB_Goal[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);

            LB_State[LGV_No].Visible = true;
            LB_State[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            LB_State[LGV_No].TabIndex = 10;
            LB_State[LGV_No].Text = "상   태 :";
            LB_State[LGV_No].Location = new System.Drawing.Point(3, 71);
            LB_State[LGV_No].Visible = true;
            LB_State[LGV_No].BackColor = Color.Transparent;
            LB_State[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);

            LB_Error[LGV_No].Visible = true;
            LB_Error[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            LB_Error[LGV_No].TabIndex = 10;
            LB_Error[LGV_No].Text = "에   러 :";
            LB_Error[LGV_No].Location = new System.Drawing.Point(3, 95);
            LB_Error[LGV_No].Visible = true;
            LB_Error[LGV_No].BackColor = Color.Transparent;
            LB_Error[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);

            LB_Battery[LGV_No].Visible = true;
            LB_Battery[LGV_No].Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            LB_Battery[LGV_No].TabIndex = 10;
            LB_Battery[LGV_No].Text = "배터리 :";
            LB_Battery[LGV_No].Location = new System.Drawing.Point(103, 95);
            LB_Battery[LGV_No].Visible = true;
            LB_Battery[LGV_No].BackColor = Color.Transparent;
            LB_Battery[LGV_No].ForeColor = Color.FromArgb(255, 255, 128);

            GB_LGV_Schedule[LGV_No].Controls.Add(TB_Schedule[LGV_No]);
            GB_LGV_Schedule[LGV_No].Controls.Add(Btn_Delete_Command[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(TB_Current[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(TB_Goal[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(TB_State[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(TB_Error[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(TB_Battery[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(LB_Current[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(LB_Goal[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(LB_State[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(LB_Error[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(LB_Battery[LGV_No]);
            GB_LGV_Info[LGV_No].Controls.Add(GB_LGV_Schedule[LGV_No]);

            panel2.Controls.Add(GB_LGV_Info[LGV_No]);


            panel2.Controls.Add(GB_LGV_Info[LGV_No]);
        }
        private void Delete_Command_Click(object sender, EventArgs e)
        {
            int Delete_AGV_Num = -1;
            for (int i = 0; i < LGV_NUM; i++)
            {
                if (Btn_Delete_Command[i] == sender)
                {
                    Delete_AGV_Num = i;
                    break;
                }
            }
            if (Delete_AGV_Num != -1)
            {
                if(m_stAGV[Delete_AGV_Num].MCS_Vehicle_Command_ID == "")
                {
                    TB_Schedule[Delete_AGV_Num].Clear();
                    m_stAGV[Delete_AGV_Num].dqGoal.Clear();
                    m_stAGV[Delete_AGV_Num].dqWork.Clear();
                    m_stAGV[Delete_AGV_Num].Working = 0;
                    m_stAGV[Delete_AGV_Num].MCS_Vehicle_Command_ID = "";
                    CS_Work_DB.DELETE_DB_Recovery(Delete_AGV_Num);
                    CS_AGV_Logic.Init_AGVInfo(Delete_AGV_Num);
                    CS_AGV_Logic.Wait_Command_ID = "";
                }
                else if (m_stAGV[Delete_AGV_Num].MCS_Vehicle_Command_ID != "")
                {
                    MessageBox.Show("명령 스케줄이 잡힌 차량 입니다.");
                }

            }

        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            Form_Insert_LGV.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CS_AGV_Logic.LGV_SendData_Traffic(1, 2);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Form_Manual_Command.ShowDialog();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            Form_Password.ShowDialog();

            if (FLAG_LOG_ON_PASSWORD == 1)
            {
                FLAG_LOG_ON_PASSWORD = 0;   //암호일치여부 초기화;
                Form_Shutter_Control.ShowDialog();
            } 
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Form_AGV_Info.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            T_Work_Alloc.Enabled = true;
            simpleButton14.Visible = true;
            simpleButton13.Visible = false;
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            T_Work_Alloc.Enabled = false;
            simpleButton14.Visible = false;
            simpleButton13.Visible = true;
        }

        private void T_Work_Alloc_Tick(object sender, EventArgs e)
        {

            if (FLAG_Start_Count < 51)
            {
                FLAG_Start_Count++;
            }
            
            if (FLAG_Start_Count >= 50)
            {
                CS_AGV_Logic.WorkAlloc();
                CS_AGV_Logic.Move_Charge_Station();
            }
            
        }

        private void ImageNode_MouseDown_1(object sender, MouseEventArgs e)
        {
            /*
            //Change_Real_XY(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                drag = true;

                New_point.X = e.X;
                New_point.Y = e.Y;
                Old_point = New_point;
            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //long IO_Data = 0;
            //DioInit_001();
            //IO_Data = DioInpBit_001();
           // textBox1.Text = Convert.ToString(IO_Data);
            label4.Text = System.DateTime.Now.ToString("yyyy년MM월dd일 HH시mm분ss초");

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Form_Error_Log.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form_MCS.ShowDialog();
        }

        private void ImageNode_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (drag == true)
            {
                New_point.X = e.X;
                New_point.Y = e.Y;

                ImageNode.Left += (New_point.X - Old_point.X);
                ImageNode.Top += (New_point.Y - Old_point.Y);

                Old_point = New_point;
            }
            */
        }

        private void ImageNode_MouseUp(object sender, MouseEventArgs e)
        {
           // drag = false;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            
            //Form_MCS.ShowDialog();
            //Flag_MCS_Auto = 1;
            /*
            //엑셀에 작업 로그 저장
            CS_Work_DB.Insert_Excel_Data_Log("", "", "", "", "", 0, "", "",
                "", "", "", "완료", "", "", "",

                "", "", "","");
             */

        }


        private void T_ReadReQuest_LGV_Tick(object sender, EventArgs e)
        {
            if(m_stAGV[Read_AGV_Num].connect == 1)
            {
                CS_Connect_LGV.DataReadBufferRequest(Read_AGV_Num);
            }

            Read_AGV_Num++;

            if (Read_AGV_Num >= LGV_NUM)
            {
                Read_AGV_Num = 0;
            } 
        }
 
        public void Send_MCS(int idx)
        {
            int FLAG_Carrier_ID = 0;
            int FLAG_Check_Goal_Node = 0;
            int FLAG_Check_Exit_1_Node = 0;
            int FLAG_Check_Exit_2_Node = 0;
  
            string Now_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            string log = "";

            try
            {
                if (Flag_MCS_Auto == 1)
                {
                    panel1.BackColor = Color.Lime;
                    P_MCS.BackColor = Color.Lime;

                    #region 차량 IDLE,DOWN보고
                    if (m_stAGV[idx].connect == 1 && FLAG_Report_Connect[idx] == 0)
                    {
                        FLAG_Report_Connect[idx] = 1;
                        //MCS보고 - VehicleInstalled(608)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 608, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    else if (m_stAGV[idx].connect == 0 && FLAG_Report_Connect[idx] == 1)
                    {
                        FLAG_Report_Connect[idx] = 0;
                        //MCS보고 - VehicleRemove(609)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 609, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    #endregion
                    #region 이적재 에러 시나리오
                    /*
                    if (m_stAGV[idx].connect == 1 && m_stAGV[idx].Working_Error == 1 && FLAG_Working_Error[idx] == 0
                   && (m_stAGV[idx].Goal == Convert.ToInt32(m_stAGV[idx].Source_Port_Num)
                    || m_stAGV[idx].Goal == Convert.ToInt32(m_stAGV[idx].Dest_Port_Num)))
                    {

                        if (m_stAGV[idx].MCS_Carrier_ID == "")
                        {
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Source_Port;
                        }
                        else if (m_stAGV[idx].MCS_Carrier_ID != "")
                        {
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Vehicle_ID;
                        }

                        for (int i = 0; i < CS_Work_DB.waitCommand.Count; i++)
                        {
                            if (m_stAGV[idx].MCS_Vehicle_Command_ID == m_stCommand[i].Command_ID)
                            {
                                m_stAGV[idx].MCS_Carrier_ID = m_stCommand[i].Carrier_ID;
                                break;
                            }
                        }
                        FLAG_Working_Error[idx] = 1;
                        //MCS보고 - VehicleUnassigned(610)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 610, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);

                        //MCS보고 - TransferCompleted(207)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 207, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);


                        CS_Work_DB.Update_Command_Abort(m_stAGV[idx].MCS_Vehicle_Command_ID, "2");
                        //CS_Work_DB.Update_Command_Info(idx, m_stAGV[idx].MCS_Vehicle_Command_ID, "2", Convert.ToString(idx), CS_AGV_Logic.Alloc_Time, CS_AGV_Logic.End_Time);

              
                        //차가 작업지, 경유지에 있는지 확인
                        for (int Wait_Station_Count = 0; Wait_Station_Count < CS_Work_DB.Path_Count; Wait_Station_Count++)
                        {
                            if ((m_stAGV[idx].current + 2) == Convert.ToInt32(CS_Work_Path[Wait_Station_Count].Goal_Area)
                            && CS_Work_Path[Wait_Station_Count].Type != "충전소")
                            {
                                FLAG_Check_Goal_Node = 1;
                                break;
                            }
                            else if ((m_stAGV[idx].current
                            == Convert.ToInt32(CS_Work_Path[Wait_Station_Count].Exit_Area_1))
                            && CS_Work_Path[Wait_Station_Count].Type != "충전소")
                            {
                                FLAG_Check_Exit_1_Node = 1;
                                break;
                            }
                            else if ((m_stAGV[idx].current
                            == Convert.ToInt32(CS_Work_Path[Wait_Station_Count].Exit_Area_2))
                            && CS_Work_Path[Wait_Station_Count].Type != "충전소")
                            {
                                FLAG_Check_Exit_2_Node = 1;
                                break;
                            }
                        }
                        for (int i = 0; i < m_stAGV[idx].dqGoal.Count; i++)
                        {
                            if (Convert.ToInt32(m_stAGV[idx].dqGoal[i]) == Convert.ToInt32(m_stAGV[idx].Source_Port_Num))
                            {
                                FLAG_Carrier_ID = 1;
                            }
                        }

                        if (FLAG_Carrier_ID == 1)
                        {
                            m_stAGV[idx].MCS_Carrier_ID = "";

                        }
                        //작업 종료 업데이트             
                        CS_AGV_Logic.Init_AGVInfo(idx);
                        m_stAGV[idx].Working = 0;
                        m_stAGV[idx].dqGoal.Clear();
                        m_stAGV[idx].dqWork.Clear();
                        Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            TB_Schedule[idx].Clear();
                        }));
                        
                        CS_Work_DB.DELETE_DB_Recovery(idx);

                        if (FLAG_Check_Goal_Node == 1)
                        {
                            CS_AGV_Logic.DoWorkAlloc(idx, "Exit_Goal_Node");
                        }
                        else if (FLAG_Check_Exit_1_Node == 1)
                        {
                            CS_AGV_Logic.DoWorkAlloc(idx, "Exit_Exit1_Node");
                        }
                        else if (FLAG_Check_Exit_2_Node == 1)
                        {
                            CS_AGV_Logic.DoWorkAlloc(idx, "Exit_Exit2_Node");
                        }
                    }
                    else if (m_stAGV[idx].connect == 1 && m_stAGV[idx].Working_Error == 0)
                    {
                        FLAG_Working_Error[idx] = 0;
                    }
                    */
                    #endregion
                    #region 위치 바뀔때 마다 보고
                    if (m_stAGV[idx].connect == 1 && m_stAGV[idx].current != m_stAGV[idx].Before_Current)
                    {
                        
                        m_stAGV[idx].Before_Current = m_stAGV[idx].current;
                        //MCS보고 - VehiclePositionChanged(502)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 502, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    #endregion
                    #region 작업 시나리오 
                    if (m_stAGV[idx].Source_Port_Num != "" && m_stAGV[idx].Dest_Port_Num != ""
                    && m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                    {
                        //Source도착 했을때
                        if ( m_stAGV[idx].state == 4 && (m_stAGV[idx].Before_State != m_stAGV[idx].state) &&
                          (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Source_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;
                            //MCS보고 - VehicleArrived(601)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 601, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "601_1 차량 = " +(idx+1));
                        }
                        //적재중 -----
                        else if (m_stAGV[idx].state == 2 && (m_stAGV[idx].Before_State != m_stAGV[idx].state) &&
                          (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Source_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;

                            m_stAGV[idx].MCS_Carrier_ID = CS_AGV_Logic.Save_CarrierID[idx];
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Vehicle_ID;
                            if (idx == 0 || idx == 1 || idx == 9 || idx == 11 || idx == 12)
                            {
                                //REEL
                                m_stAGV[idx].MCS_Carrier_Type = "3";
                            }
                            else if (idx == 2 || idx == 3 || idx == 4)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "5";
                            }
                            else if (idx == 5 || idx == 6 || idx == 7)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "6";
                            }
                            else if (idx == 8)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "9";
                            }

              

                            //MCS보고 - Transferring(211)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 211, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "211 차량 = " + (idx + 1));

                            //MCS보고 - VehicleAcquireStarted(602)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 602, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "602 차량 = " + (idx + 1));
                        }
                        //적재완료 -----
                        else if (m_stAGV[idx].state == 5 && (m_stAGV[idx].Before_State != m_stAGV[idx].state)
                         && (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Source_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;

                            string Install_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                            m_stAGV[idx].MCS_Carrier_ID = CS_AGV_Logic.Save_CarrierID[idx];
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Vehicle_ID;
                            if (idx == 0 || idx == 1 || idx == 9 || idx == 11 || idx == 12)
                            {
                                //REEL
                                m_stAGV[idx].MCS_Carrier_Type = "3";
                            }
                            else if (idx == 2 || idx == 3 || idx == 4)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "5";
                            }
                            else if (idx == 5 || idx == 6 || idx == 7)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "6";
                            }
                            else if (idx == 8)
                            {
                                //ROLL
                                m_stAGV[idx].MCS_Carrier_Type = "9";
                            }

                            m_stAGV[idx].MCS_Install_Time = Install_Time;
     

                            //MCS보고 - CarrierInstalled(301)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 301, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "301 차량 = " + (idx + 1));

                            //MCS보고 - VehicleAcquireCompleted(603)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 603, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "603 차량 = " + (idx + 1));
                        }
                        //Dest 이동 -----
                        else if (m_stAGV[idx].state == 1 && (m_stAGV[idx].Before_State != m_stAGV[idx].state)
                          && (m_stAGV[idx].Goal == Convert.ToInt32(m_stAGV[idx].Dest_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;
                            //MCS보고 - VehicleDeparted(605)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 605, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "605 차량 = " + (idx + 1));
                        }
                        //Dest 도착 -----
                        else if (m_stAGV[idx].state == 4 && (m_stAGV[idx].Before_State != m_stAGV[idx].state) &&
                          (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Dest_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;
                            //MCS보고 - VehicleArrived(601)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 601, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "601_2 차량 = " + (idx + 1));
                        }
                        //이재중 -----
                        else if (m_stAGV[idx].state == 3 && (m_stAGV[idx].Before_State != m_stAGV[idx].state) &&
                          (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Dest_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;
                            //MCS보고 - VehicleDepositStarted(606)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 606, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "606 차량 = " + (idx + 1));

                            //MCS보고 - CarrierRemoved(302)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "302 차량 = " + (idx + 1));

                        }  
                        //이재완료 -----
                        else if (m_stAGV[idx].state == 6 && (m_stAGV[idx].Before_State != m_stAGV[idx].state) &&
                          (m_stAGV[idx].current == Convert.ToInt32(m_stAGV[idx].Dest_Port_Num)))
                        {
                            m_stAGV[idx].Before_State = m_stAGV[idx].state;

                            for (int i = 0; i < CS_Work_DB.Rack_Count; i ++)
                            {
                                if (m_stAGV[idx].MCS_Dest_Port == CS_Work_DB.Rack_PortID[i])
                                {
                                    CS_Work_DB.Update_CarrierID_Packing(m_stAGV[idx].MCS_Dest_Port, "");
                                    CS_Work_DB.Update_CarrierID_Packing(m_stAGV[idx].MCS_Dest_Port, m_stAGV[idx].MCS_Carrier_ID);
                                    break;
                                }
                            }
                            
                            m_stAGV[idx].MCS_Carrier_LOC = m_stAGV[idx].MCS_Dest_Port;
                            //MCS보고 - VehicleDepositCompleted(607)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 607, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "607 차량 = " + (idx + 1));

                            //MCS보고 - VehicleUnassigned(610)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 610, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "610 차량 = " + (idx + 1));

                            //MCS보고 - TransferCompleted(207)
                            Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 207, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Log("SendMCSLOG", "207 차량 = " + (idx + 1));

                            
                            m_stAGV[idx].MCS_Carrier_ID = "";
                            m_stAGV[idx].MCS_Install_Time = "";
                            m_stAGV[idx].MCS_Carrier_Type = "";
                            CS_AGV_Logic.Save_CarrierID[idx] = "";
                            CS_Work_DB.Update_AGV_Info_WaitCarrierID(idx, m_stAGV[idx].MCS_Vehicle_ID, "");
                        }
                    }
                    #endregion
                    #region 충전 보고
                    
                    if ((m_stAGV[idx].current == 499 || m_stAGV[idx].current == 1499 || m_stAGV[idx].current == 1438 || m_stAGV[idx].current == 665
                        || m_stAGV[idx].current == 438 || m_stAGV[idx].current == 1496 || m_stAGV[idx].current == 496 || m_stAGV[idx].current == 166
                        //|| m_stAGV[idx].current == 438 || m_stAGV[idx].current == 1496 || m_stAGV[idx].current == 496 || m_stAGV[idx].current == 166
                        //|| m_stAGV[idx].current == 44 || m_stAGV[idx].current == 1041) //20200826 충전기 이설 수정 
                        || m_stAGV[idx].current == 163 || m_stAGV[idx].current == 1041) 
                     && (m_stAGV[idx].state == 9 || m_stAGV[idx].state == 0)  && FLAG_Charge_LGV[idx] == 0)
                    {
                        Battey[idx] = Math.Truncate((((double)m_stAGV[idx].Battery - 2400) / 380) * 100);

                        if (Battey[idx] > 100)
                        {
                            Battey[idx] = 100;
                        }
                        else if (Battey[idx] < 0)
                        {
                            Battey[idx] = 0;
                        }

                        Charge_Start_Time[idx] = Convert.ToDateTime(Now_Time);

                        FLAG_Charge_LGV[idx] = 1;
                        //MCS보고 - VehicleChargingStarted 
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 612, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_ID);

                        
                    }
                    else if((m_stAGV[idx].current == 499 || m_stAGV[idx].current == 1499 || m_stAGV[idx].current == 1438 || m_stAGV[idx].current == 665 || m_stAGV[idx].current == 163
                        || m_stAGV[idx].current == 438 || m_stAGV[idx].current == 1496 || m_stAGV[idx].current == 496 || m_stAGV[idx].current == 166 || m_stAGV[idx].current == 1041)
                        && (m_stAGV[idx].state != 9 && m_stAGV[idx].state != 8 && m_stAGV[idx].state != 0 && m_stAGV[idx].state != 4) && FLAG_Charge_LGV[idx] == 1)
                    {
                        Charge_End_Time[idx] = Convert.ToDateTime(Now_Time);
                        Battey_End[idx] = Math.Truncate((((double)m_stAGV[idx].Battery - 2400) / 380) * 100);
                        FLAG_Charge_LGV[idx] = 0;
                        //MCS보고 - VehicleChargingFinished  
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 613, 0, Convert.ToUInt16(idx), m_stAGV[idx].MCS_Vehicle_ID);

                        if (Charge_Start_Time[idx] != Convert.ToDateTime("2011-11-11 11:11:11"))
                        {
                            Charge_Time_Result = Charge_End_Time[idx] - Charge_Start_Time[idx];

                        }

                        log = string.Format("배터리 = {0}%, 시작 시간 = {1},배터리 = {2}%, 종료 시간 = {3}, 충전 시간 = {4}",
                                           Battey[idx], Charge_Start_Time[idx], Battey_End[idx], Charge_End_Time[idx], Charge_Time_Result);
                        Log("AGV_0" + (idx + 1) + "---Charge Data", log);

                    }
                    
                    #endregion
                }
                else
                {
                    panel1.BackColor = Color.Red;
                    P_MCS.BackColor = Color.Red;
                }
            }
            catch (Exception EX)
            {
                Log("TryCatch MCS_SendMSG", Convert.ToString(EX));
            }
        }
        public void Retry_LGV(int idx)
        {

        }


        public int[] Station_Type_1 = new int[LGV_NUM];
        public int[] Station_Type_2 = new int[LGV_NUM];
        public int[] Station_Type_3 = new int[LGV_NUM];
        public int[] Station_Type_4 = new int[LGV_NUM];
        int[] FLAG_Same_Port = new int[4];

        
        private void simpleButton8_Click_2(object sender, EventArgs e)
        {
            
        }

  
        private void simpleButton12_Click_3(object sender, EventArgs e)
        {
            Form_Setting_Charge.ShowDialog();
        }

      
        public void Move_Charge_Wait_Station(int idx)
        {     
            try
            {
                if (m_stAGV[idx].dqGoal.Count == 0 && m_stAGV[idx].mode == 1
                     && (m_stAGV[idx].state == 0 || m_stAGV[idx].state == 4
                      || m_stAGV[idx].state == 5 || m_stAGV[idx].state == 6)
                     //&& (m_stAGV[idx].current != 67 && m_stAGV[idx].current != 44
                     && (m_stAGV[idx].current != 166 && m_stAGV[idx].current != 163 //20200826 충전기 이설 수정
                     && m_stAGV[idx].current != 1237 && m_stAGV[idx].current != 1041
                     && m_stAGV[idx].current != 1438 && m_stAGV[idx].current != 438
                     && m_stAGV[idx].current != 499 && m_stAGV[idx].current != 1499
                     && m_stAGV[idx].current != 8 && m_stAGV[idx].current != 1012
                     && m_stAGV[idx].current != 173
                     && m_stAGV[idx].current != 1131
                     && m_stAGV[idx].current != 442 && m_stAGV[idx].current != 1556) //충전소 조건 추가 20200820
                     && (m_stAGV[idx].Flag_Wait_Station == 1))

                     /*
                 && //(m_stAGV[Move_Charge_Station_Num].current != 67 && m_stAGV[Move_Charge_Station_Num].current != 44
                 && //m_stAGV[Move_Charge_Station_Num].current != 1438 && m_stAGV[Move_Charge_Station_Num].current != 438
                 && m_stAGV[Move_Charge_Station_Num].current != 1041 && m_stAGV[Move_Charge_Station_Num].current != 1237
                 && //m_stAGV[Move_Charge_Station_Num].current != 499 && m_stAGV[Move_Charge_Station_Num].current != 1499
                 && //m_stAGV[Move_Charge_Station_Num].current != 1011 && m_stAGV[Move_Charge_Station_Num].current != 8 
                 && m_stAGV[Move_Charge_Station_Num].current != 1131 && m_stAGV[Move_Charge_Station_Num].current != 1097
                 && m_stAGV[Move_Charge_Station_Num].current != 1178 && m_stAGV[Move_Charge_Station_Num].current != 1496
                 && m_stAGV[Move_Charge_Station_Num].current != 277 && m_stAGV[Move_Charge_Station_Num].current != 496
                 && m_stAGV[Move_Charge_Station_Num].current != 173 && m_stAGV[Move_Charge_Station_Num].current != 322
                 && m_stAGV[Move_Charge_Station_Num].current != 665 && m_stAGV[Move_Charge_Station_Num].current != 1369
                 && m_stAGV[Move_Charge_Station_Num].current != 442 && m_stAGV[Move_Charge_Station_Num].current != 278
                 && m_stAGV[Move_Charge_Station_Num].current != 1556 && m_stAGV[Move_Charge_Station_Num].current != 369)*/
                {
                    CS_AGV_Logic.Find_Charge_Area_AGV(idx);

                    if (CS_AGV_C_Info[idx].Type == "ROLL_음극")
                    {
                        //충전장소가 비었을때
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 0)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if (m_stAGV[idx].current != 64 && m_stAGV[idx].current != 58 && m_stAGV[idx].current != 1156)
                        {
                            //166번 이동
                            //CS_AGV_Logic.Move_Charge_Station_Logic(idx, "음극_대기장소_2");
                            //m_stAGV[idx].Flag_Wait_Station = 0;

                            if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_166[idx] == 0)
                            {
                                CS_AGV_Logic.Work_Insert(idx, 166, 1);
                                m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_166[idx] == 1)
                            {
                                CS_AGV_Logic.Work_Insert(idx, 163, 1);
                                m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                        }
                        else if(m_stAGV[idx].current == 64 || m_stAGV[idx].current == 58 || m_stAGV[idx].current == 1156)
                        {
                            //163번 이동
                            //CS_AGV_Logic.Move_Charge_Station_Logic(idx, "음극_대기장소_1");
                            //m_stAGV[idx].Flag_Wait_Station = 0;
                            
                            if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_166[idx] == 0)
                            {
                                CS_AGV_Logic.Work_Insert(idx, 166, 1);
                                m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_166[idx] == 1)
                            {
                                CS_AGV_Logic.Work_Insert(idx, 163, 1);
                                m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                        }
                    } 
                    else if (CS_AGV_C_Info[idx].Type == "ROLL_양극")
                    {
                        //충전장소가 비었을때
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Plus_1499[idx] == 0)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if ((CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] == 1 || CS_AGV_Logic.FLAG_Check_WareHouse_Area_Plus_1499[idx] == 1)
                               && (CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1041[idx] == 0 || (m_stAGV[idx].current >= 1001 && m_stAGV[idx].current <= 1040)))
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1041[idx] = 1;
                            //1041번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "양극_대기장소_1");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1041[idx] == 1 && CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] == 1)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1237[idx] = 1;
                            //1237번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "양극_대기장소_2");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                    }
                    else if (CS_AGV_C_Info[idx].Type == "REEL_양극" || CS_AGV_C_Info[idx].Type == "REEL_음극" || CS_AGV_C_Info[idx].Type == "REEL_공용")
                    {
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_1438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            //1097번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "릴_충전소_1");
                            CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_1438[idx] = 1;
                        }
                        // 충전소 438 충전 문제로 우선 순위 439와 변경, lkw20190421
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_496[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            //277번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "릴_충전소_3");
                            CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_496[idx] = 1;
                        }
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            //173번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "릴_충전소_2");
                            CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_438[idx] = 1;
                        }
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_442[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            //442번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "릴_충전소_4");
                            CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_442[idx] = 1;
                        }
                       /* else if (CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_1556[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            //1556번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "릴_충전소_5");
                            CS_AGV_Logic.FLAG_Check_Charge_Station_REEL_1556[idx] = 1;
                        }*/

                    }
                    else if (CS_AGV_C_Info[idx].Type ==
                        "광폭_음극" || CS_AGV_C_Info[idx].Type == "광폭_양극" || CS_AGV_C_Info[idx].Type == "광폭")
                    {
                        //322번 이동
                        CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                    }

                }
            }
            catch(Exception ex)
            {
                Log("Try Catch Move_Charge_Station", Convert.ToString(ex));
            }

            
        }

        #region 충전 에러 발생 시, 경유지까지 이동 후 충전소 복귀 로직. lkw20190530
        //---------------------------------------------------------------------------------------------
        //충전 에러 발생 시, 경유지 보냈다가 다시 충전소로 보내기
        public void Move_Charge_Station_Retry(int idx)
        {
            switch(m_stAGV[idx].current)
            {
                //-----------------------------------------
                // 1호기, 2호기, 10호기 (릴대차 LGV)
                //-----------------------------------------
                //릴_충전소_3 277-496
                
                case 496:
                    CS_AGV_Logic.Work_Insert(idx, 277, 1);
                    CS_AGV_Logic.Work_Insert(idx, 496, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //릴_충전소_2 173-438
                case 438:
                    CS_AGV_Logic.Work_Insert(idx, 173, 1);
                    CS_AGV_Logic.Work_Insert(idx, 438, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //릴_충전소_1 1097-1438
                case 1438:
                    CS_AGV_Logic.Work_Insert(idx, 1097, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1438, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //릴_충전소_4 1369-442
                case 442:
                    CS_AGV_Logic.Work_Insert(idx, 1369, 1);
                    CS_AGV_Logic.Work_Insert(idx, 442, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                /*case 1556:
                //릴_충전소_5 278-1556
                    CS_AGV_Logic.Work_Insert(idx, 276, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1556, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;*/
                //-----------------------------------------
                // 3호기   , 4호기, 5호기 (양극 LGV)
                //-----------------------------------------
                //양극 충전소 1011-1499
                case 1499:
                    CS_AGV_Logic.Work_Insert(idx, 1012, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1499, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //양극_대기장소_1 1041-1041
                case 1041:
                    CS_AGV_Logic.Work_Insert(idx, 1075, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1041, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //양극_대기장소_2 1075-1237
                case 1237:
                    CS_AGV_Logic.Work_Insert(idx, 1235, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1237, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //-----------------------------------------
                // 6호기, 7호기, 8호기 (음극 LGV)
                //-----------------------------------------
                //음극 충전소 8-499
                case 499:
                    CS_AGV_Logic.Work_Insert(idx, 8, 1);
                    CS_AGV_Logic.Work_Insert(idx, 499, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //음극_대기장소_1 44-44
                //case 44:
                case 166:
                    //CS_AGV_Logic.Work_Insert(idx, 67, 1);
                    CS_AGV_Logic.Work_Insert(idx, 163, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //음극_대기장소_1 67-67
                //case 67:
                case 163:
                    //CS_AGV_Logic.Work_Insert(idx, 44, 1);
                    CS_AGV_Logic.Work_Insert(idx, 166, 1);
                    //CS_AGV_Logic.Work_Insert(idx, 163, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //-----------------------------------------
                // 9호기 (음극 광폭 LGV)
                //-----------------------------------------
                //음극 광폭 충전소 322-665
                case 665:
                    CS_AGV_Logic.Work_Insert(idx, 322, 1);
                    CS_AGV_Logic.Work_Insert(idx, 665, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
                //-----------------------------------------
                // 10호기 (양극 광폭 LGV)
                //-----------------------------------------
                //양극 광폭 충전소 1178-1496
                case 1496:
                    CS_AGV_Logic.Work_Insert(idx, 1178, 1);
                    CS_AGV_Logic.Work_Insert(idx, 1496, 1);
                    m_stAGV[idx].Flag_Wait_Station = 0;
                    break;
            }
        }

        #endregion

        public void Prevent_Double_Charge(int idx)
        {
            /*
            if(idx == 0)
            {
                if(m_stAGV[9].Battery - m_stAGV[0].Battery <= 20 && m_stAGV[0].Battery < 2552)
                {
                    m_stAGV[0].FLAG_Prevent_Double_Charge = 1;
                    Move_Charge_Station(0);
                }
            }
            else if (idx == 9)
            {
                if (m_stAGV[0].Battery - m_stAGV[9].Battery <= 20 && m_stAGV[9].Battery < 2552)
                {
                    m_stAGV[9].FLAG_Prevent_Double_Charge = 1;
                    Move_Charge_Station(9);
                }
            }
            */
        }

        private void T_Move_Charge_Station_Tick(object sender, EventArgs e)
        {
            //작업 종료했을때 5초동안 명령이 없으면 대기 장소로 보내기
            if (m_stAGV[Move_Charge_Station_Num].dqGoal.Count == 0 && m_stAGV[Move_Charge_Station_Num].mode == 1
                 && (m_stAGV[Move_Charge_Station_Num].state == 0 || m_stAGV[Move_Charge_Station_Num].state == 4
                  || m_stAGV[Move_Charge_Station_Num].state == 5 || m_stAGV[Move_Charge_Station_Num].state == 6)
                 //&& (m_stAGV[Move_Charge_Station_Num].current != 67 && m_stAGV[Move_Charge_Station_Num].current != 44
                 && (m_stAGV[Move_Charge_Station_Num].current != 166 && m_stAGV[Move_Charge_Station_Num].current != 163
                 && m_stAGV[Move_Charge_Station_Num].current != 1438 && m_stAGV[Move_Charge_Station_Num].current != 438
                 && m_stAGV[Move_Charge_Station_Num].current != 1041 && m_stAGV[Move_Charge_Station_Num].current != 1237
                 && m_stAGV[Move_Charge_Station_Num].current != 499 && m_stAGV[Move_Charge_Station_Num].current != 1499
                 && m_stAGV[Move_Charge_Station_Num].current != 1012 && m_stAGV[Move_Charge_Station_Num].current != 8 
                 && m_stAGV[Move_Charge_Station_Num].current != 1131 && m_stAGV[Move_Charge_Station_Num].current != 1097
                 && m_stAGV[Move_Charge_Station_Num].current != 1178 && m_stAGV[Move_Charge_Station_Num].current != 1496
                 && m_stAGV[Move_Charge_Station_Num].current != 277 && m_stAGV[Move_Charge_Station_Num].current != 496
                 && m_stAGV[Move_Charge_Station_Num].current != 173 && m_stAGV[Move_Charge_Station_Num].current != 322
                 && m_stAGV[Move_Charge_Station_Num].current != 665 //&& m_stAGV[Move_Charge_Station_Num].current != 1369
                 && m_stAGV[Move_Charge_Station_Num].current != 442 //&& m_stAGV[Move_Charge_Station_Num].current != 276
                 && m_stAGV[Move_Charge_Station_Num].current != 1556 //&& m_stAGV[Move_Charge_Station_Num].current != 369)
                 ) && (m_stAGV[Move_Charge_Station_Num].Flag_Wait_Station == 1))
            {
                Move_Charge_Count[Move_Charge_Station_Num]++;
            }
            else
            {
                Move_Charge_Count[Move_Charge_Station_Num] = 0;
            }

            if(Move_Charge_Count[Move_Charge_Station_Num] > 2)
            {
                if(Move_Charge_Station_Num == 0 || Move_Charge_Station_Num == 1 || Move_Charge_Station_Num == 9 || Move_Charge_Station_Num == 11 || Move_Charge_Station_Num == 12) // 20200820 충전소 이동 조건 추가 20200822 수정
                {
                    if (m_stAGV[Move_Charge_Station_Num].current == 1313 || m_stAGV[Move_Charge_Station_Num].current == 1268
                     || m_stAGV[Move_Charge_Station_Num].current == 1286)
                    {
                        //CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 1062, 1);

                        if ((Move_Charge_Station_Num == 0 || Move_Charge_Station_Num == 1 || Move_Charge_Station_Num == 9) && !(Move_Charge_Station_Num == 11 || Move_Charge_Station_Num == 12)) // 0
                        {
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 1062, 1);
                        }
                        else if ((Move_Charge_Station_Num == 11 || Move_Charge_Station_Num == 12) && !(Move_Charge_Station_Num == 0 || Move_Charge_Station_Num == 1 || Move_Charge_Station_Num == 9))
                        {
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 1369, 1);
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 442, 1); // 0904 추가
                        }
                    }
                    else if(m_stAGV[Move_Charge_Station_Num].current == 1054)
                    {
                        //CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 281, 1);

                        if ((Move_Charge_Station_Num == 0 || Move_Charge_Station_Num == 1 || Move_Charge_Station_Num == 9) && !(Move_Charge_Station_Num == 11 || Move_Charge_Station_Num == 12)) // 0
                        {
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 281, 1);
                        }
                        else if ((Move_Charge_Station_Num == 11 || Move_Charge_Station_Num == 12) && !(Move_Charge_Station_Num == 0 || Move_Charge_Station_Num == 1 || Move_Charge_Station_Num == 9))
                        {
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 1369, 1);
                            CS_AGV_Logic.Work_Insert(Move_Charge_Station_Num, 442, 1); // 0904 추가
                        }
                    }
                    else if(m_stAGV[Move_Charge_Station_Num].current != 1313 && m_stAGV[Move_Charge_Station_Num].current != 1268
                         && m_stAGV[Move_Charge_Station_Num].current != 1286 && m_stAGV[Move_Charge_Station_Num].current != 1054)
                    {
                        Move_Charge_Wait_Station(Move_Charge_Station_Num);
                        Move_Charge_Count[Move_Charge_Station_Num] = 0;
                    }
                }
                else if(Move_Charge_Station_Num != 0 && Move_Charge_Station_Num != 1 && Move_Charge_Station_Num != 9 && Move_Charge_Station_Num != 11 && Move_Charge_Station_Num != 12)
                {
                    Move_Charge_Wait_Station(Move_Charge_Station_Num);
                    Move_Charge_Count[Move_Charge_Station_Num] = 0;
                }
            }

            Move_Charge_Station_Num++;

            if (Move_Charge_Station_Num >= LGV_NUM)
            {
                Move_Charge_Station_Num = 0;   
            }
            
        }

        private void simpleButton15_Click_1(object sender, EventArgs e)
        {
            CS_Draw_Node_AGV.AGV_Location_Nav(43000, 59073, 0, 1);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            CS_Draw_Node_AGV.AGV_Location_Nav(40508, 59073, 0, 0);
        }

        public string Find_Shutter_Name(int Shutter_No)
        {
            string Shutter_Name = "";

            if (Shutter_No == 0)
            {
                Shutter_Name = "음극_코터";
            }
            else if (Shutter_No == 1)
            {
                Shutter_Name = "양극_코터";
            }
            else if (Shutter_No == 2)
            {
                Shutter_Name = "음극_창고#1";
            }
            else if (Shutter_No == 3)
            {
                Shutter_Name = "음극_창고#2";
            }
            else if (Shutter_No == 4)
            {
                Shutter_Name = "양극_창고#1";
            }
            else if (Shutter_No == 5)
            {
                Shutter_Name = "양극_창고#2";
            }
            else if (Shutter_No == 6)
            {
                Shutter_Name = "음극_광폭";
            }
            else if (Shutter_No == 7)
            {
                Shutter_Name = "양극_광폭";
            }

            else if (Shutter_No == 8)
            {
                Shutter_Name = "음극_프레스_1";
            }
            else if (Shutter_No == 9 )
            {
                Shutter_Name = "음극_프레스_2";
            }
            else if (Shutter_No == 10)
            {
                Shutter_Name = "음극_프레스_3";
            }
            else if (Shutter_No == 11)
            {
                Shutter_Name = "음극_프레스_4";
            }
            else if (Shutter_No == 12 )
            {
                Shutter_Name = "양극_프레스_1";
            }
            else if (Shutter_No == 13)
            {
                Shutter_Name = "양극_프레스_2";
            }
            else if (Shutter_No == 14)
            {
                Shutter_Name = "양극_프레스_3";
            }
            else if (Shutter_No == 15)
            {
                Shutter_Name = "양극_프레스_4";
            }

            else if (Shutter_No == 16)
            { 
                Shutter_Name = "음극_슬리터_1";
            }
            else if (Shutter_No == 17)
            {
                Shutter_Name = "음극_슬리터_2";
            }
            else if (Shutter_No == 18)
            {
                Shutter_Name = "음극_슬리터_3";
            }

            else if (Shutter_No == 19)
            {
                Shutter_Name = "양극_슬리터_1";
            }
            else if (Shutter_No == 20)
            {
                Shutter_Name = "양극_슬리터_2";
            }
            else if (Shutter_No == 21)
            {
                Shutter_Name = "양극_슬리터_3";
            }
            else if (Shutter_No == 22)
            {
                Shutter_Name = "SFL_코터";
            }

            else if (Shutter_No == 23)
            {
                Shutter_Name = "음극_롤RW_1";
            }

            else if (Shutter_No == 24)
            {
                Shutter_Name = "음극_롤RW_2";
            }
            else if (Shutter_No == 25)
            {
                Shutter_Name = "양극_롤RW_1";
            }
            else if (Shutter_No == 26)
            {
                Shutter_Name = "양극_롤RW_2";
            }


            return Shutter_Name;
        }

        public int Find_Shutter_Num(string Shutter_Name)
        {
            int Num = -1;

            if(Shutter_Name == "음극_코터")
            {
                Num = 0;
            }
            else if (Shutter_Name == "양극_코터")
            {
                Num = 1;
            }
            else if (Shutter_Name == "음극_창고#1")
            {
                Num = 2;
            }
            else if (Shutter_Name == "음극_창고#2")
            {
                Num = 3;
            }
            else if (Shutter_Name == "양극_창고#1")
            {
                Num = 4;
            }
            else if (Shutter_Name == "양극_창고#2")
            {
                Num = 5;
            }
            else if (Shutter_Name == "음극_광폭")
            {
                Num = 6;
            }
            else if (Shutter_Name == "양극_광폭")
            {
                Num = 7;
            }

            else if (Shutter_Name == "음극_프레스_1")
            {
                Num = 8;
            }
            else if (Shutter_Name == "음극_프레스_2")
            {
                Num = 9;
            }
            else if (Shutter_Name == "음극_프레스_3")
            {
                Num = 10;
            }
            else if (Shutter_Name == "음극_프레스_4")
            {
                Num = 11;
            }
            else if (Shutter_Name == "양극_프레스_1")
            {
                Num = 12;
            }
            else if (Shutter_Name == "양극_프레스_2")
            {
                Num = 13;
            }
            else if (Shutter_Name == "양극_프레스_3")
            {
                Num = 14;
            }
            else if (Shutter_Name == "양극_프레스_4")
            {
                Num = 15;
            }

            else if (Shutter_Name == "음극_슬리터_1")
            {
                Num = 16;
            }
            else if (Shutter_Name == "음극_슬리터_2")
            {
                Num = 17;
            }
            else if (Shutter_Name == "음극_슬리터_3")
            {
                Num = 18;
            }

            else if (Shutter_Name == "양극_슬리터_1")
            {
                Num = 19;
            }
            else if (Shutter_Name == "양극_슬리터_2")
            {
                Num = 20;
            }
            else if (Shutter_Name == "양극_슬리터_3")
            {
                Num = 21;
            }
            else if (Shutter_Name == "SFL_코터")
            {
                Num = 22;
            }

            else if (Shutter_Name == "음극_롤RW_1")
            {
                Num = 23;
            }

            else if (Shutter_Name == "음극_롤RW_2")
            {
                Num = 24;
            }
            else if (Shutter_Name == "양극_롤RW_1")
            {
                Num = 25;
            }
            else if (Shutter_Name == "양극_롤RW_2")
            {
                Num = 26;
            }
            return Num;
        }

        public void Shutter_Control(int Shutter_Control_No)
        {
            string log = "";
            Shutter_No = -1;
            idx = -1;
            FLAG_Shtter_On_InPut = 0;
            FLAG_Shtter_On_OutPut = 0;

            try
            {
                for (int Shutter_Count = 0; Shutter_Count < SHUTTER_NUM; Shutter_Count++)
                {
                    if (CS_Shutter_C_Info[Shutter_Control_No].Name == CS_Shutter_C_Info[Shutter_Count].Name)
                    {
                        for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
                        {
                            if (((m_stAGV[Other_AGV].current >= CS_Shutter_C_Info[Shutter_Count].Start_Station_InPut
                                    && m_stAGV[Other_AGV].current <= CS_Shutter_C_Info[Shutter_Count].End_Station_InPut) &&
                                   (m_stAGV[Other_AGV].Goal >= CS_Shutter_C_Info[Shutter_Count].Start_Goal_InPut
                                    && m_stAGV[Other_AGV].Goal <= CS_Shutter_C_Info[Shutter_Count].End_Goal_InPut)) ||
                                    ((m_stAGV[Other_AGV].current >= CS_Shutter_C_Info[Shutter_Count].Start_Station_OutPut
                                    && m_stAGV[Other_AGV].current <= CS_Shutter_C_Info[Shutter_Count].End_Station_OutPut) &&
                                   (m_stAGV[Other_AGV].Goal >= CS_Shutter_C_Info[Shutter_Count].Start_Goal_OutPut
                                    && m_stAGV[Other_AGV].Goal <= CS_Shutter_C_Info[Shutter_Count].End_Goal_OutPut)))
                            {
                                if (CS_Shutter_C_Info[Shutter_Count].Machine_Name == "입고대")
                                {
                                    idx = Other_AGV;
                                    FLAG_Shtter_On_InPut = 1;
                                    break;
                                    
                                }
                                else if (CS_Shutter_C_Info[Shutter_Count].Machine_Name == "출고대")
                                {
                                    idx = Other_AGV;
                                    FLAG_Shtter_On_OutPut = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                
                if (FLAG_Shtter_On_InPut == 1)
                {
                    Shutter_No = Find_Shutter_Num(CS_Shutter_C_Info[Shutter_Control_No].Name);
                    CS_Connect_Shutter.DataSendRequest_InPut_Close(Shutter_No, 0);
                    CS_Connect_Shutter.DataSendRequest_InPut_Open(Shutter_No, 1);

                    //문다열렸으면 고고
                    if (CS_Shutter_Info[Shutter_No].InPut_Open_Sensor == 1)
                    {
                        CS_Connect_LGV.DataSendRequest_Shutter(idx, 1);
                        log = string.Format("설비명 = {0}_입고대,\t호기 = {1}", CS_Shutter_C_Info[Shutter_Control_No].Name, idx);
                        Log("셔터 출발 신호", log);
                    }
                    else if (CS_Shutter_Info[Shutter_No].InPut_Open_Sensor == 0)
                    {
                        CS_Connect_LGV.DataSendRequest_Shutter(idx, 0);

                    }
                }
                else if (FLAG_Shtter_On_InPut == 0)
                {
                    Shutter_No = Find_Shutter_Num(CS_Shutter_C_Info[Shutter_Control_No].Name);
                    if (Shutter_No != -1)
                    {
                        CS_Connect_Shutter.DataSendRequest_InPut_Close(Shutter_No, 1);
                        CS_Connect_Shutter.DataSendRequest_InPut_Open(Shutter_No, 0);
                    }
                }

                if (FLAG_Shtter_On_OutPut == 1)
                {
                    Shutter_No = Find_Shutter_Num(CS_Shutter_C_Info[Shutter_Control_No].Name);
                    CS_Connect_Shutter.DataSendRequest_OutPut_Close(Shutter_No, 0);
                    CS_Connect_Shutter.DataSendRequest_OutPut_Open(Shutter_No, 1);

           
                    //문다열렸으면 고고
                    if (CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor == 1)
                    {
                        CS_Connect_LGV.DataSendRequest_Shutter(idx, 1);
                        log = string.Format("설비명 = {0}_출고대,\t호기 = {1}", CS_Shutter_C_Info[Shutter_Control_No].Name, idx);
                        Log("셔터 출발 신호", log);
                    }
                    else if (CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor == 0)
                    {
                        CS_Connect_LGV.DataSendRequest_Shutter(idx, 0);

                    }
                }
                else if (FLAG_Shtter_On_OutPut == 0)
                {
                    Shutter_No = Find_Shutter_Num(CS_Shutter_C_Info[Shutter_Control_No].Name);
                    if (Shutter_No != -1)
                    {
                        CS_Connect_Shutter.DataSendRequest_OutPut_Close(Shutter_No, 1);
                        CS_Connect_Shutter.DataSendRequest_OutPut_Open(Shutter_No, 0);
                    }
                }
            }
            catch(Exception ex)
            {
                Log("Try Catch Shutter_Control", Convert.ToString(ex));
            }
        }

        private void T_Shutter_Tick(object sender, EventArgs e)
        {
            Shutter_Control(Shutter_Control_No);
            Shutter_Control_No++;
            if(Shutter_Control_No >= SHUTTER_NUM)
            {
                Shutter_Control_No = 0;
            }
        }

        private void T_Remove_CarrierID_Tick(object sender, EventArgs e)
        {
            if(Flag_MCS_Auto == 1)
            {
                for (int i = 0; i < LGV_NUM; i++)
                {
                    if (m_stAGV[i].MCS_Vehicle_Command_ID == "" && m_stAGV[i].dqGoal.Count == 0 && m_stAGV[i].dqWork.Count == 0
                     && m_stAGV[i].Product_Sensor == 0 && m_stAGV[i].MCS_Carrier_ID != ""
                    && (m_stAGV[i].state == 0 || m_stAGV[i].state == 4 || m_stAGV[i].state == 5 || m_stAGV[i].state == 6 || m_stAGV[i].state == 8 ||
                        m_stAGV[i].state == 9 || m_stAGV[i].state == 10))
                    {
                        Remove_Count[i]++;
                    }
                    if (m_stAGV[i].MCS_Vehicle_Command_ID == "" && Remove_Count[i] > 30 && m_stAGV[i].MCS_Carrier_ID != "" && m_stAGV[i].Product_Sensor == 0)
                    {
                        //Carrier Remove
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 302, 0, Convert.ToUInt16(i), "");
                        m_stAGV[i].MCS_Carrier_ID = "";

                        m_stAGV[i].MCS_Carrier_Type = "";
                        m_stAGV[i].MCS_Install_Time = "";
                        m_stAGV[i].MCS_Carrier_LOC = "";
                        CS_AGV_Logic.Save_CarrierID[i] = "";
                        Remove_Count[i] = 0;
                    }
                    if (Remove_Count[i] > 100)
                    {
                        Remove_Count[i] = 0;
                    }
                }
            }
            
            
        }

        private void T_Abort_ReQuest_Tick(object sender, EventArgs e)
        {
            int FLAG_Check_Load_Order = 0;
      
            if (m_stAGV[ReQuest_Abort_AGV_Num].Ask_Abort == 1)
            {
                if (m_stAGV[ReQuest_Abort_AGV_Num].dqGoal.Count > 0)
                {
                    if(m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID != "")
                    {
                        Form_MCS.Command_Type = "ABORT";
                        if (m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_ID == "")
                        {
                            m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_LOC = "";
                            m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_Type = "";
                        }
                        else if (m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_ID != "")
                        {
                            m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_LOC = m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_ID;

                            if (ReQuest_Abort_AGV_Num == 0 || ReQuest_Abort_AGV_Num == 1 || ReQuest_Abort_AGV_Num == 9 || ReQuest_Abort_AGV_Num == 11 || ReQuest_Abort_AGV_Num == 12)
                            {
                                //REEL
                                m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_Type = "3";
                            }
                            else if (ReQuest_Abort_AGV_Num == 2 || ReQuest_Abort_AGV_Num == 3 || ReQuest_Abort_AGV_Num == 4)
                            {
                                //ROLL
                                m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_Type = "5";
                            }
                            else if (ReQuest_Abort_AGV_Num == 5 || ReQuest_Abort_AGV_Num == 6 || ReQuest_Abort_AGV_Num == 7)
                            {
                                //ROLL
                                m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_Type = "6";
                            }
                            else if (ReQuest_Abort_AGV_Num == 8)
                            {

                                m_stAGV[ReQuest_Abort_AGV_Num].MCS_Carrier_Type = "9";
                            }
                        }
                        // 사양서와 다름 수정함(3/9)
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 501, 0, Convert.ToUInt16(ReQuest_Abort_AGV_Num), m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 203, 0, Convert.ToUInt16(ReQuest_Abort_AGV_Num), m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 610, 0, Convert.ToUInt16(ReQuest_Abort_AGV_Num), m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID);
                        Form_MCS.SendS6F11(Form_MCS.m_nDeviceID, 0, 6, 11, 0, 201, 0, Convert.ToUInt16(ReQuest_Abort_AGV_Num), m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID);

                        CS_AGV_Logic.Init_WaitCommand();
                        m_stAGV[ReQuest_Abort_AGV_Num].dqGoal.Clear();
                        m_stAGV[ReQuest_Abort_AGV_Num].dqWork.Clear();
                        m_stAGV[ReQuest_Abort_AGV_Num].Working = 0;
                        TB_Schedule[ReQuest_Abort_AGV_Num].Clear();
                        CS_Work_DB.Delete_Work_Log(m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID);
                        //작업 중단
                        CS_Connect_LGV.DataSendRequest_AbortOK(ReQuest_Abort_AGV_Num, 1);
                        //작업 종료 업데이트                
                        m_stAGV[ReQuest_Abort_AGV_Num].Working = 0;
                        Log("LGV_Manual_Abort", "명령o 중단 작업명:" + m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID + "차량 = "+ (ReQuest_Abort_AGV_Num+1));

                        Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            TB_Schedule[ReQuest_Abort_AGV_Num].Clear();
                        }));
                        m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID = "";
                        CS_AGV_Logic.Init_AGVInfo(ReQuest_Abort_AGV_Num);
                        
                    }
                    else if(m_stAGV[ReQuest_Abort_AGV_Num].MCS_Vehicle_Command_ID == "")
                    {
                        m_stAGV[ReQuest_Abort_AGV_Num].dqGoal.Clear();
                        m_stAGV[ReQuest_Abort_AGV_Num].dqWork.Clear();
                        m_stAGV[ReQuest_Abort_AGV_Num].Working = 0;
                        TB_Schedule[ReQuest_Abort_AGV_Num].Clear();
                        //작업 종료 업데이트                
                        m_stAGV[ReQuest_Abort_AGV_Num].Working = 0;
                        m_stAGV[ReQuest_Abort_AGV_Num].Flag_Wait_Station = 1;
                        //작업 중단
                        CS_Connect_LGV.DataSendRequest_AbortOK(ReQuest_Abort_AGV_Num, 1);
                        Log("LGV_Manual_Abort", "명령x 중단" + (ReQuest_Abort_AGV_Num+1) + "호");
                    }
                    
                }
            }
            ReQuest_Abort_AGV_Num++;
            if (ReQuest_Abort_AGV_Num >= LGV_NUM)
            {
                ReQuest_Abort_AGV_Num = 0;
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            int i;
            int idx = -1;
            if (gridView1 == null || gridView1.SelectedRowsCount == 0) return;
            DataRow[] rows = new DataRow[gridView1.SelectedRowsCount];
            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                rows[i] = gridView1.GetDataRow(gridView1.GetSelectedRows()[i]);
            }

            for (i = 0; i < gridView1.SelectedRowsCount; i++)
            {
                
            }
            
        }

        private void T_Lift_Info_Tick(object sender, EventArgs e)
        {
            CS_Work_DB.Select_Lift_Info();
            #region 음극 리프트 상태 확인
            //3층 물건 상태 - 음극
            if (CS_AGV_Logic.Carrier_3[0] == "1")
            {
                Down_3.BackColor = Color.Lime;
            }
            else
            {
                Down_3.BackColor = Color.Black;
            }
            //1층 물건 상태 - 음극
            if (CS_AGV_Logic.Carrier_1[0] == "1")
            {
                Down_1.BackColor = Color.Lime;
            }
            else
            {
                Down_1.BackColor = Color.Black;
            }  
            //중앙 확인
            if (CS_AGV_Logic.Lift_State[0] == "1")
            {
                Down_2.BackColor = Color.Lime;
            }
            else
            {
                Down_2.BackColor = Color.Black;
            }
            #endregion
            #region 양극 리프트 상태 확인
           
            //3층 물건 상태 - 양극
            if (CS_AGV_Logic.Carrier_3[1] == "1")
            {
                UP_3.BackColor = Color.Lime;
            }
            else
            {
                UP_3.BackColor = Color.Black;
            }
            //1층 물건 상태 - 양극
            if (CS_AGV_Logic.Carrier_1[1] == "1")
            {
                UP_1.BackColor = Color.Lime;
            }
            else
            {
                UP_1.BackColor = Color.Black;
            }

            //중앙 확인
            if (CS_AGV_Logic.Lift_State[1] == "1")
            {
                UP_2.BackColor = Color.Lime;
            }
            else
            {
                UP_2.BackColor = Color.Black;
            }
            #endregion
           
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
  
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            for (int i = 0; i < SHUTTER_NUM;i++)
            {
                CS_Connect_Shutter.Dis_Connect(i);
            }
        }
        public void IO_Shutter_Control_Enter()
        {
            try
            {
                int FLAG_OPEN_AutoDoor = 0;
                int Go_AGV = -1;
                int Open_OK_1 = 0;
                int Open_OK_2 = 0;
                for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
                {
                    //고장난 음극 자동문 수리에 따른 변경. lkw20190503
                    if (((m_stAGV[Other_AGV].current >= 110 && m_stAGV[Other_AGV].current <= 116) && (m_stAGV[Other_AGV].Goal == 173 || m_stAGV[Other_AGV].Goal == 183 || 
                                                                                                      m_stAGV[Other_AGV].Goal == 438 || m_stAGV[Other_AGV].Goal >= 1000))
                     || ((m_stAGV[Other_AGV].current >= 173 && m_stAGV[Other_AGV].current <= 180))
                     || ((m_stAGV[Other_AGV].current >= 116 && m_stAGV[Other_AGV].current <= 120)))
                    {
                        Go_AGV = Other_AGV;
                        FLAG_OPEN_AutoDoor = 1;
                        break;
                    }
                }

                if (FLAG_OPEN_AutoDoor == 1)
                {
                    DioInit_002();
                    DioOutByte_002(1);
                    DioInit_001();
                    DioOutByte_001(1);

                    Open_OK_1 = Convert.ToInt32(DioInpBit_001());
                    Open_OK_2 = Convert.ToInt32(DioInpBit_002());

                    if (Open_OK_1 == 1 && Open_OK_2 == 1)
                    {
                        CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                        Log("IO Shutter Data", "출발 신호");
                    }
                }
                else if (FLAG_OPEN_AutoDoor == 0)
                {
                    DioInit_001();
                    DioOutByte_001(0);
                    DioInit_002();
                    DioOutByte_002(0);
                }
            }
            catch(Exception ex)
            {
                Log("IO_Shutter_Error", Convert.ToString(ex));
            }
            
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
       
            if (Flag_MCS_Auto == 1)
            {
                IO_Shutter_Control_Enter();
                IO_Shutter_Control_AutoDoor();
                IO_Shutter_Control_AirShower_Enter();
                //11.05 포장실 셔터 추가
                IO_Shutter_Control_PackingRoom();
                //11.22 음/양 자동문 추가
                IO_Shutter_Control_P_TO_M_Door();
            }
            
        }

        public void IO_Shutter_Control_PackingRoom()
        {
            int FLAG_OPEN_AutoDoor = 0;
            int Go_AGV = -1;


            for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
            {
                if (((m_stAGV[Other_AGV].current >= 1155 && m_stAGV[Other_AGV].current <= 1160) && (m_stAGV[Other_AGV].Goal >= 1161 && m_stAGV[Other_AGV].Goal <= 1161))
                    || ((m_stAGV[Other_AGV].current >= 1158 && m_stAGV[Other_AGV].current <= 1170) && (m_stAGV[Other_AGV].Goal >= 1156 && m_stAGV[Other_AGV].Goal <= 1156)))
                {
                    Go_AGV = Other_AGV;
                    FLAG_OPEN_AutoDoor = 1;
                    break; 
                }
            }

            if (FLAG_OPEN_AutoDoor == 1)
            {
                DioInit_000();
                DioOutByte_000(1);
                Log("IO Shutter Data", "포장실 오픈 요청");
                AutoDoor_Open_OK_Packing = Convert.ToInt32(DioInpBit_000());
                Log("IO Shutter Data", "Sensor = " + AutoDoor_Open_OK_Packing + "");

                if (AutoDoor_Open_OK_Packing == 1)
                {
                    CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                    Log("IO Shutter Data", "포장실 출발 신호 차량 번호 = "+ (Go_AGV + 1));
                }
            }
            else if (FLAG_OPEN_AutoDoor == 0)
            {
                DioInit_000();
                DioOutByte_000(0);
            }
            
        }

        public void IO_Shutter_Control_AutoDoor()
        {

            int FLAG_OPEN_AutoDoor = 0;
            int Go_AGV = -1;
            

            for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
            {
                if(((m_stAGV[Other_AGV].current >= 188 && m_stAGV[Other_AGV].current <= 195) && (m_stAGV[Other_AGV].Goal >= 200 && m_stAGV[Other_AGV].Goal <= 221))
                    || ((m_stAGV[Other_AGV].current >= 191 && m_stAGV[Other_AGV].current <= 203) && !(m_stAGV[Other_AGV].Goal == 221)))
                {
                    Go_AGV = Other_AGV;
                    FLAG_OPEN_AutoDoor = 1;
                    break;
                }
            }

            if(FLAG_OPEN_AutoDoor == 1)
            {
                DioInit_003();
                DioOutByte_003(1);
                Log("IO Shutter Data", "음극 믹싱라인 오픈 요청");
                //AutoDoor_Open_OK = Convert.ToInt32(DioInpBit_003());
                Log("IO Shutter Data", "Sensor = " + AutoDoor_Open_OK + "");
                if (AutoDoor_Open_OK == 1)
                {
                    CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                    Log("IO Shutter Data", "출발 신호");
                }
            }
            else if(FLAG_OPEN_AutoDoor == 0)
            {
                DioInit_003();
                DioOutByte_003(0);
            }
        }

        public void IO_Shutter_Control_P_TO_M_Door()
        {
            int FLAG_OPEN_AutoDoor = 0;
            int Go_AGV = -1;
            int P_TO_M_Door_Open_OK = 0;


            for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
            {
                if ((m_stAGV[Other_AGV].current >= 1380 && m_stAGV[Other_AGV].current <= 1385)
                 || ((m_stAGV[Other_AGV].current >= 1103 && m_stAGV[Other_AGV].current <= 1104) && (m_stAGV[Other_AGV].Goal == 1064 || m_stAGV[Other_AGV].Goal == 1369)))
                {
                    Go_AGV = Other_AGV;
                    FLAG_OPEN_AutoDoor = 1;
                    break;
                }
            }

            if (FLAG_OPEN_AutoDoor == 1)
            {
                DioInit_005();
                DioOutByte_004(1);
                Log("IO Shutter Data", "음극/양극 자동문 오픈 요청");
                P_TO_M_Door_Open_OK = Convert.ToInt32(DioInpBit_004());
                Log("IO Shutter Data", "Sensor = " + AutoDoor_Open_OK + "");
                if (P_TO_M_Door_Open_OK == 1)
                {
                    CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                    Log("IO Shutter Data", "출발 신호");
                }
            }
            else if (FLAG_OPEN_AutoDoor == 0)
            {
                DioInit_005();
                DioOutByte_004(0);
            }
        }


        private void simpleButton20_Click(object sender, EventArgs e)
        {
           // int Open_OK = Convert.ToInt32(DioInpBit_C());
        }

        private void Pic_Map_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

            Form_Password.ShowDialog();

            if (FLAG_LOG_ON_PASSWORD == 1)
            {
                FLAG_LOG_ON_PASSWORD = 0;   //암호일치여부 초기화;
                Form_Select_Traffic.ShowDialog();
            }
        }

        public void Avoid_AGV(int idx)
        {
            int FLAG_Avoid_166Node = 0; //20200826 충전기 이설 수정
            int FLAG_Avoid_163Node = 0;  //20200826 충전기 이설 수정

            int FLAG_Avoid_1237Node = 0;
            int FLAG_Avoid_1041Node = 0;



            if (m_stAGV[idx].dqGoal.Count == 0 && m_stAGV[idx].mode == 1
            && (m_stAGV[idx].state == 0 || m_stAGV[idx].state == 4 || m_stAGV[idx].state == 8 || m_stAGV[idx].state == 9 || m_stAGV[idx].state == 10))
            {
                CS_AGV_Logic.Find_Charge_Area_AGV(idx);

                if (m_stAGV[idx].current == 166)
                {
                    for (int i = 0; i < LGV_NUM; i++)
                    {
                        if (idx == i) continue;
                        if(i == 5 || i == 6 || i == 7)
                        {
                            if ((m_stAGV[i].current >= 46 && m_stAGV[i].current <= 52) && (m_stAGV[i].Goal == 166))
                            {
                                FLAG_Avoid_166Node = 1;
                            }
                        }
                        
                    }
                    if (FLAG_Avoid_166Node == 1)
                    {
                        //충전장소가 비었을때
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 0)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 1 || CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 1)
                        {
                            //44번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "음극_대기장소_2");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                    }
                }
                else if (m_stAGV[idx].current == 163)
                {
                    for (int i = 0; i < LGV_NUM; i++)
                    {
                        if (idx == i) continue;

                        if ((m_stAGV[i].current >= 67 && m_stAGV[i].current <= 75) && m_stAGV[i].Goal == 163)
                        {
                            FLAG_Avoid_163Node = 1;
                        }
                    }

                    if (FLAG_Avoid_163Node == 1)
                    {
                        //충전장소가 비었을때
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 0)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 1 || CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 1)
                        {
                            //67번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "음극_대기장소_1");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                    }
                }

                //양극 - 확인 필요
                if (m_stAGV[idx].current == 1237)
                {
                    for (int i = 0; i < LGV_NUM; i++)
                    {
                        if (idx == i) continue;

                        if ((m_stAGV[i].current >= 1233 && m_stAGV[i].current <= 1236) && (m_stAGV[i].Goal >= 1236 && m_stAGV[i].Goal <= 1239))
                        {
                            FLAG_Avoid_1237Node = 1;
                        }

                        if ((m_stAGV[i].Source_Port_Num == "1475" || m_stAGV[i].Source_Port_Num == "1466"
                        || ((m_stAGV[i].Dest_Port_Num == "1475" || m_stAGV[i].Dest_Port_Num == "1466") && m_stAGV[i].MCS_Carrier_ID != ""))
                        && m_stAGV[i].current >= 1071 && m_stAGV[i].current <= 1074)
                        {
                            FLAG_Avoid_1237Node = 1;
                        }

                        //양극 광폭출고대#1, 양극 프레스 9호기 입고대 작업시 # 2020-04-29
                        if ((m_stAGV[i].current >= 1070 && m_stAGV[i].current <= 1072) //상대차량이 1070-1072에 있고 
                        && ((m_stAGV[i].Source_Port_Num == "1469" || m_stAGV[i].Source_Port_Num == "1475")
                        || ((m_stAGV[i].Dest_Port_Num == "1469" || m_stAGV[i].Dest_Port_Num == "1475") && m_stAGV[i].MCS_Carrier_ID != "")))
                        {
                            FLAG_Avoid_1237Node = 1;
                        }
                    }
                    if (FLAG_Avoid_1237Node == 1)
                    {
                        //1062로 회피
                        CS_AGV_Logic.Work_Insert(idx, 1062, 1);
                    }
                }
                else if (m_stAGV[idx].current == 1041)
                {
                    for (int i = 0; i < LGV_NUM; i++)
                    {
                        if (idx == i) continue;

                        if (((m_stAGV[i].current == 1037 && m_stAGV[i].target != 1120) ||
                            ((m_stAGV[i].current >= 1034 && m_stAGV[i].current <= 1039) && m_stAGV[i].Goal == 1040) ||
                            ((m_stAGV[i].current >= 1038 && m_stAGV[i].current <= 1039) && m_stAGV[i].Goal >= 1040)))
                        {
                            FLAG_Avoid_1041Node = 1;
                        }
                    }
                    if (FLAG_Avoid_1041Node == 1)
                    {
                        //충전장소가 비었을때
                        if (CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Plus_1499[idx] == 0)
                        {
                            CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "충전소");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                        //충전장소가 찼을때
                        else if (CS_AGV_Logic.FLAG_Check_Charge_Station_Plus_1499[idx] == 1 || CS_AGV_Logic.FLAG_Check_WareHouse_Area_Plus_1499[idx] == 1)
                        {
                            //1075번 이동
                            CS_AGV_Logic.Move_Charge_Station_Logic(idx, "양극_대기장소_2");
                            m_stAGV[idx].Flag_Wait_Station = 0;
                        }
                    }
                }
            }
        }

        private void T_Avoid_LGV_Tick(object sender, EventArgs e)
        {
            Avoid_AGV(FLAG_Avoid_LGV);
            FLAG_Avoid_LGV++;
            if(FLAG_Avoid_LGV >= LGV_NUM)
            {
                FLAG_Avoid_LGV = 0;
            }
        }

        private void T_Send_WorkStation_Tick(object sender, EventArgs e)
        {
            
            if (m_stAGV[Work_Station_LGV_No].Source_Port_Num != "")
            {
                //출발지, 목적지 보내주기
                CS_Connect_LGV.DataSend_Work_Station_Source(Work_Station_LGV_No, Convert.ToInt32(m_stAGV[Work_Station_LGV_No].Source_Port_Num));
            }
            else
            {
                //출발지, 목적지 보내주기
                CS_Connect_LGV.DataSend_Work_Station_Source(Work_Station_LGV_No, 0);
            }

            if (m_stAGV[Work_Station_LGV_No].Dest_Port_Num != "")
            {
                //출발지, 목적지 보내주기
                CS_Connect_LGV.DataSend_Work_Station_Dest(Work_Station_LGV_No, Convert.ToInt32(m_stAGV[Work_Station_LGV_No].Dest_Port_Num));
            }
            else
            {
                //출발지, 목적지 보내주기
                CS_Connect_LGV.DataSend_Work_Station_Dest(Work_Station_LGV_No, 0);
            }

            Work_Station_LGV_No++;
            if(Work_Station_LGV_No >= LGV_NUM)
            {
                Work_Station_LGV_No = 0;
            }
            
        }

        private void T_ReConnect_Shutter_Tick(object sender, EventArgs e)
        {
            Shutter_ReConnect(Reconnect_Shutter_Num);
            Reconnect_Shutter_Num++;

            if(Reconnect_Shutter_Num >= SHUTTER_CONNECT_NUM)
            {
                Reconnect_Shutter_Num = 0;
            }
        }
  

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            CS_AGV_Logic.Chk_AGV_Duplication(10);
        }

        private void simpleButton10_Click_1(object sender, EventArgs e)
        {
            DioInit_002();
            DioOutByte_002(0);
        }

        public void Move_Charge(int idx)
        {
            if(m_stAGV[idx].current == 8 && m_stAGV[idx].dqGoal.Count == 0 
            && (m_stAGV[idx].state == 0 || m_stAGV[idx].state == 4 || m_stAGV[idx].state == 5 || m_stAGV[idx].state == 6))
            {

            }
        }

        private void T_Move_Charge_Tick(object sender, EventArgs e)
        {
            Move_Charge(Move_Charge_Num);
            Move_Charge_Num++;
            if(Move_Charge_Num >= LGV_NUM)
            {
                Move_Charge_Num = 0;
            }

        }

        private void T_ReConnect_PLC_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Flag_MCS_Auto == 1)
                {
                    Flag_ReReceive_PLC++;
                    if (Flag_ReReceive_PLC > 10)
                    {
                        if (CS_Connect_PLC.PLC_Sock.Connected == true)
                        {
                            Flag_ReReceive_PLC = 0;
                            CS_Connect_PLC.Dis_Connect();
                        }
                        else if (CS_Connect_PLC.PLC_Sock.Connected == false)
                        {

                            Flag_ReReceive_PLC = 0;

                            CS_Connect_PLC.Connect("17.91.225.234", 4001);


                        }
                    }
                }

            }
            catch (SocketException ex)
            {
                Log("Try Catch_Re_Connect_Tick", Convert.ToString(ex));
            }
        }

        private void T_ReadReQuest_PLC_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Flag_MCS_Auto == 1)
                {
                    CS_Connect_PLC.DataReadBufferRequest();

                    DioInit_004();
                    DioInit_003();


                    //기재 열림 센서
                    Open_OK_2__ = Convert.ToInt32(DioInpBit_AirShower_0());
                    //M동 열림 센서
                    Open_OK_1__ = Convert.ToInt32(DioInpBit_AirShower_1());
                    //기재 닫힘 센서 확인
                    Close_OK_2__ = Convert.ToInt32(DioInpBit_AirShower_2());
                    //m동 닫힘확인
                    Close_OK_1__ = Convert.ToInt32(DioInpBit_AirShower_3());

                    //자동문 열림 확인            
                    AutoDoor_Open_OK = Convert.ToInt32(DioInpBit_003());
                    //에어샤워 열림 상태
                    CS_Connect_PLC.Send_Door_Open_2 = Convert.ToByte(DioInpBit_AirShower_0());
                    CS_Connect_PLC.Send_Door_Open_1 = Convert.ToByte(DioInpBit_AirShower_1());
                    CS_Connect_PLC.Send_Door_Close_2 = Convert.ToByte(DioInpBit_AirShower_2());
                    CS_Connect_PLC.Send_Door_Close_1 = Convert.ToByte(DioInpBit_AirShower_3());


                    //자동문 열림 상태
                    CS_Connect_PLC.Send_AutoDoor_Open = Convert.ToByte(DioInpBit_003());



                    #region 믹싱 -> ACS 수신 데이터 표시
                    if (CS_Connect_PLC.Auto_Door_Open == 1)
                    {
                        LB_Mix_AutoDoor_Open.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Auto_Door_Open == 0)
                    {
                        LB_Mix_AutoDoor_Open.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Auto_Door_Close == 1)
                    {
                        LB_Mix_AutoDoor_Close.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Auto_Door_Close == 0)
                    {
                        LB_Mix_AutoDoor_Close.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Area_Enter == 1)
                    {
                        LB_Mix_Enter.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Area_Enter == 0)
                    {
                        LB_Mix_Enter.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Area_Enter == 1)
                    {
                        LB_Mix_Enter.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Area_Enter == 0)
                    {
                        LB_Mix_Enter.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Area_Using == 1)
                    {
                        LB_Mix_Using.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Area_Using == 0)
                    {
                        LB_Mix_Using.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Door_Open_1 == 1)
                    {
                        LB_Mix_Open_Door_1.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Door_Open_1 == 0)
                    {
                        LB_Mix_Open_Door_1.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Door_Close_1 == 1)
                    {
                        LB_Mix_Close_Door_1.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Door_Close_1 == 0)
                    {
                        LB_Mix_Close_Door_1.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Door_Open_2 == 1)
                    {
                        LB_Mix_Open_Door_2.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Door_Open_2 == 0)
                    {
                        LB_Mix_Open_Door_2.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Door_Close_2 == 1)
                    {
                        LB_Mix_Close_Door_2.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Door_Close_2 == 0)
                    {
                        LB_Mix_Close_Door_2.ForeColor = Color.White;
                    }
                    #endregion
                    #region ACS -> 믹싱 송신 데이터 표시
                    if (CS_Connect_PLC.Send_AutoDoor_Open == 1)
                    {
                        LB_ACS_Auto_Door_Open.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_AutoDoor_Open == 0)
                    {
                        LB_ACS_Auto_Door_Open.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Enter_Ok == 1)
                    {
                        LB_ACS_Enter_Ok.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Enter_Ok == 0)
                    {
                        LB_ACS_Enter_Ok.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Enter_ING == 1)
                    {
                        LB_ACS_Using.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Enter_ING == 0)
                    {
                        LB_ACS_Using.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Door_Open_1 == 1)
                    {
                        LB_ACS_Door_Open_1.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Door_Open_1 == 0)
                    {
                        LB_ACS_Door_Open_1.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Door_Close_1 == 1)
                    {
                        LB_ACS_Door_Close_1.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Door_Close_1 == 0)
                    {
                        LB_ACS_Door_Close_1.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Door_Open_2 == 1)
                    {
                        LB_ACS_Door_Open_2.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Door_Open_2 == 0)
                    {
                        LB_ACS_Door_Open_2.ForeColor = Color.White;
                    }

                    if (CS_Connect_PLC.Send_Door_Close_2 == 1)
                    {
                        LB_ACS_Door_Close_2.ForeColor = Color.Lime;
                    }
                    else if (CS_Connect_PLC.Send_Door_Close_2 == 0)
                    {
                        LB_ACS_Door_Close_2.ForeColor = Color.White;
                    }
                    #endregion

                    Mixing_AGV_Control();
                    IO_Shutter_Control_AirShower_Enter();
                    CS_Connect_PLC.DataSendRequest_BufDataSend_InPut_Air();
                    CS_Connect_PLC.DataSendRequest_BufDataSend_InPut_Area();
                    CS_Connect_PLC.DataSendRequest_BufDataSend_AutoDoor();
                }
            }
            catch(Exception ex)
            {
                Log("Try Catch T_ReadReQuest_PLC_Tick", Convert.ToString(ex));
            }
            
            
        }
        public void IO_Shutter_Control_AirShower_Enter()
        {
            int Go_AGV = -1;

            FLAG_OPEN_AIRSHOWER_1 = 0;
            FLAG_OPEN_AIRSHOWER_2 = 0;
            FLAG_CLOSE_DOOR = 0;
            for (int Other_AGV = 0; Other_AGV < LGV_NUM; Other_AGV++)
            {
                //M동->기재반입구 진입시 215에서 213로 변경. 더 빨리 열기 위해서. lkw20190627
                if (((m_stAGV[Other_AGV].current >= 213 && m_stAGV[Other_AGV].current <= 218) && m_stAGV[Other_AGV].Goal == 221)
                   || ((m_stAGV[Other_AGV].current >= 217 && m_stAGV[Other_AGV].current <= 218) && !(m_stAGV[Other_AGV].Goal == 221)))
                {
                    Go_AGV = Other_AGV;
                    //M동 열기
                    FLAG_OPEN_AIRSHOWER_1 = 1;
                    break;
                }
                //M동->기재반입구 진입시 220에서 219로 변경. 차량이 220에서 서다가 219~220에서 서는 것으로 변경됨. lkw20190627
                else if (((m_stAGV[Other_AGV].current >= 219 && m_stAGV[Other_AGV].current <= 221) && m_stAGV[Other_AGV].Goal == 221)
                      || ((m_stAGV[Other_AGV].current >= 220 && m_stAGV[Other_AGV].current <= 221) && !(m_stAGV[Other_AGV].Goal == 221)))
                {
                    Go_AGV = Other_AGV;
                    //기재 열기
                    FLAG_OPEN_AIRSHOWER_2 = 1;
                    break;
                }
                else if ((m_stAGV[Other_AGV].current >= 211 && m_stAGV[Other_AGV].current <= 214)
                      || (m_stAGV[Other_AGV].current >= 446 && m_stAGV[Other_AGV].current <= 448))
                {
                    Go_AGV = Other_AGV;
                    //일정 위치에서만 문닫기
                    FLAG_CLOSE_DOOR = 1;
                    break;
                }


            }
            //우리차가 지나 갈때 문컨트롤 해주기
            if (FLAG_OPEN_AIRSHOWER_1 == 1 && FLAG_OPEN_AIRSHOWER_2 == 0 && FLAG_CLOSE_DOOR == 0)
            {
                DioInit_004();

                //기재가 닫혔을때만 문열기
                if(Close_OK_2__ == 1)
                {
                    //M동 문열기
                    DioOutByte_AirShower_0(2);
                }
                //문열렸으면 문닫기
                else if(Open_OK_2__ == 1)
                {
                    //문닫기
                    DioOutByte_AirShower_0(4);
                }
                Log("IO Shutter Data", "M동 문열기");

                Log("IO Shutter Data", "Sensor = " + Open_OK_1__ + "");
                //M동 문열기 센서
                if (Open_OK_1__ == 1 && Close_OK_2__ == 1)
                {
                    CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                    Log("IO Shutter Data", "출발 신호");
                }
            }
            else if (FLAG_OPEN_AIRSHOWER_1 == 0 && FLAG_OPEN_AIRSHOWER_2 == 1 && FLAG_CLOSE_DOOR == 0)
            {
                DioInit_004();

                //m동이 닫혔을때만 문열기
                if (Close_OK_1__ == 1)
                {
                    //기재 문열기
                    DioOutByte_AirShower_0(1);
                }
                //문열렸으면 문닫기
                else if (Open_OK_1__ == 1)
                {
                    //문닫기
                    DioOutByte_AirShower_0(4);
                }

                //기재 열기 센서
                if (Open_OK_2__ == 1 && Close_OK_1__ == 1)
                {
                    CS_Connect_LGV.DataSendRequest_Shutter(Go_AGV, 1);
                    Log("IO Shutter Data", "출발 신호");
                }
            }
            //다른 차량이 지나갈때 문컨트롤 해주기
            if ((CS_Connect_PLC.Door_Open_1 == 1 && CS_Connect_PLC.Door_Open_2 == 0
           && (CS_Connect_PLC.Door_Close_1 == 0 && CS_Connect_PLC.Door_Close_2 == 0)))
            {
                //M동 문열기
                DioInit_004();
                DioOutByte_AirShower_0(2);
                Log("Mixing AGV Door Data", "M동라인 문열기 요청, Sensor_1 = " + Open_OK_1__ + ", Sensor_2 = " + Open_OK_2__ + "");
            }
            else if (CS_Connect_PLC.Door_Open_2 == 1 && CS_Connect_PLC.Door_Open_1 == 0
                && (CS_Connect_PLC.Door_Close_1 == 0 && CS_Connect_PLC.Door_Close_2 == 0))
            {
                //기재 문열기
                DioInit_004();
                DioOutByte_AirShower_0(1);
                Log("Mixing AGV Door Data", "기재라인 문열기 요청, Sensor_1 = " + Open_OK_1__ + ", Sensor_2 = " + Open_OK_2__ + "");
            }
            else if ((CS_Connect_PLC.Door_Close_1 == 1 || CS_Connect_PLC.Door_Close_2 == 1 || FLAG_CLOSE_DOOR == 1)
                 && (CS_Connect_PLC.Door_Open_2 == 0 && CS_Connect_PLC.Door_Open_1 == 0)
                 && (FLAG_OPEN_AIRSHOWER_1 == 0 && FLAG_OPEN_AIRSHOWER_2 == 0))
            {
                if (Open_OK_1__ == 1 || Open_OK_2__ == 1)
                {
                    //문닫기
                    DioOutByte_AirShower_0(4);
                    Log("Mixing AGV Door Data", "문닫기 요청, Sensor_1 = " + Close_OK_1__ + ", Sensor_2 = " + Close_OK_2__ + "");
                }

            }
            else if ((CS_Connect_PLC.Door_Close_1 == 0 && CS_Connect_PLC.Door_Close_2 == 0)
                 && (CS_Connect_PLC.Door_Open_2 == 0 && CS_Connect_PLC.Door_Open_1 == 0)
                 && (FLAG_OPEN_AIRSHOWER_1 == 0 && FLAG_OPEN_AIRSHOWER_2 == 0 && FLAG_CLOSE_DOOR == 0))
            {
                if (Close_OK_1__ == 1 && Close_OK_2__ == 1)
                {
                    //닫기 초기화
                    DioOutByte_AirShower_0(0);
                }
            }
        }
        public void Mixing_AGV_Control()
        {
            int Check_Mixing_Area = 0;
            for(int i = 0; i < LGV_NUM;i++)
            {
                //글로벌텍 - 영역 사용중 전송
                if((m_stAGV[i].current >= 186 && m_stAGV[i].current <= 221)
                || (m_stAGV[i].current >= 660 && m_stAGV[i].current <= 662)
                || (m_stAGV[i].current >= 446 && m_stAGV[i].current <= 448)
                || (m_stAGV[i].current >= 1524 && m_stAGV[i].current <= 1547)
                || (m_stAGV[i].current == 150 || m_stAGV[i].current == 151 || m_stAGV[i].current == 1185))
                {
                    Check_Mixing_Area = 1;       
                }
            }

            if(Check_Mixing_Area == 1)
            {
                CS_Connect_PLC.Send_Enter_ING = 1;
                CS_Connect_PLC.Send_Enter_Ok = 0;
            }
            else if(Check_Mixing_Area == 0)
            {
                CS_Connect_PLC.Send_Enter_ING = 0;
                CS_Connect_PLC.Send_Enter_Ok = 1;
            }

            if (CS_Connect_PLC.Auto_Door_Open == 1)
            {
                DioInit_003();
                DioOutByte_003(1);
            }
            else if(CS_Connect_PLC.Auto_Door_Close == 1)
            {
                DioInit_003();
                DioOutByte_003(0);
            }
        }

        private void T_Link_LGV_Tick(object sender, EventArgs e)
        {

            if (m_stAGV[Link_AGV_Num].connect == 1)
            {
                CS_Connect_LGV.DataSendRequest_Link(Link_AGV_Num, 1);
            }

            Link_AGV_Num++;

            if (Link_AGV_Num >= LGV_NUM)
            {
                Link_AGV_Num = 0;
            }
        }

        public void Init_Count_Info(int idx)
        {
            try
            {

                /*
                //1 : 주행(명령o), 3: 주행(명령x), 5: 주행(적재), 7: 주행(적재)_오버브릿지, 9 : 트래픽, 11 : 대기(자동), 13 : 대기(수동), 15: 충전, 17 : 적재, 19 : 이재, 21 : 에러
                string path = Application.StartupPath + @"\Logs\" + DateTime.Today.ToString("yyyyMMdd") + "AGV_0" + (idx + 1) + "---Working Data" + ".log";
                */

                //신규 로그 추가. lkw20190109
                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyyMMdd");
                //1 : 주행(명령o), 3: 주행(명령x), 5: 주행(적재), 7: 주행(적재)_오버브릿지, 9 : 트래픽, 11 : 대기(자동), 13 : 대기(수동), 15: 충전, 17 : 적재, 19 : 이재, 21 : 에러
                string path = f_dir + "\\" + date + "\\" + date + "AGV_0" + (idx + 1) + "---Working Data" + ".log";
                string[] Text_Value = System.IO.File.ReadAllLines(path);
                string Value = "";
                string[] Save_Data;
                char[] sp = { '=', ',' };

                if (Text_Value.Length > 0)
                {
                    Value = Text_Value[Text_Value.Length - 1];
                }
                Save_Data = Value.Split(sp);
                if (Save_Data[idx].Length > 0)
                {
                    Count_Drive_Command[idx] = Convert.ToInt32(Save_Data[1]);
                    Count_Drive[idx] = Convert.ToInt32(Save_Data[3]);
                    Count_Drive_Load[idx] = Convert.ToInt32(Save_Data[5]);
                    Count_Drive_Load_Over[idx] = Convert.ToInt32(Save_Data[7]);
                    Count_Traffic[idx] = Convert.ToInt32(Save_Data[9]);
                    Count_Wait_Auto[idx] = Convert.ToInt32(Save_Data[11]);
                    Count_Wait_Manual[idx] = Convert.ToInt32(Save_Data[13]);
                    Count_Charge[idx] = Convert.ToInt32(Save_Data[15]);
                    Count_Loading[idx] = Convert.ToInt32(Save_Data[17]);
                    Count_UnLoading[idx] = Convert.ToInt32(Save_Data[19]);
                    Count_Error[idx] = Convert.ToInt32(Save_Data[21]);
                    Count_Traffic_Command[idx] = Convert.ToInt32(Save_Data[23]);

                }
            }
            catch (Exception ex)
            {
                Log("TRY Catch Init_Count_Info", Convert.ToString(ex));
            }

        }

        public void Count_LOG_Data(int idx)
        {
            int Now_Time = Convert.ToInt32(DateTime.Now.ToString("1HHmmss"));

            T_Work_Rate[idx].Enabled = false;
            //00시 넘으면 카운트 초기화
            if (Now_Time <= 1000030)
            {
                Count_Drive_Load[idx] = 0;
                Count_Drive_Command[idx] = 0;
                Count_Drive[idx] = 0;
                Count_Traffic[idx] = 0;
                Count_Traffic_Command[idx] = 0;
                Count_Wait_Auto[idx] = 0;
                Count_Wait_Manual[idx] = 0;
                Count_Charge[idx] = 0;
                Count_Loading[idx] = 0;
                Count_UnLoading[idx] = 0;
                Count_Error[idx] = 0;
                Count_Drive_Load_Over[idx] = 0;
                // TEST 로그
                Count_etc[idx] = 0;
            }

            try
            {
                string log = "";

                if (m_stAGV[idx].Error == 0)
                {
                    //주행중(적재)_오버브릿지
                    if (m_stAGV[idx].state == 1 && m_stAGV[idx].Product_Sensor == 1 && m_stAGV[idx].current >= 1500 && m_stAGV[idx].current <= 1502)
                    {

                        Count_Drive_Load_Over[idx]++;
                    }
                    //주행중(적재)
                    else if (m_stAGV[idx].state == 1 && m_stAGV[idx].Product_Sensor == 1
                       && !(m_stAGV[idx].current >= 1500 && m_stAGV[idx].current <= 1502))
                    {
                        Count_Drive_Load[idx]++;
                    }
                    //주행중(명령o)
                    else if (m_stAGV[idx].state == 1 && m_stAGV[idx].Product_Sensor == 0 && m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                    {
                        Count_Drive_Command[idx]++;
                    }
                    //주행중(명령x)
                    else if (m_stAGV[idx].state == 1 && m_stAGV[idx].Product_Sensor == 0 && m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                    {
                        Count_Drive[idx]++;
                    }
                    //트래픽 LOG, 명령O, 명령X 구분 추가. lkw20190111
                    //트래픽(명령o)
                    else if (m_stAGV[idx].state == 7 && m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                    {
                        Count_Traffic_Command[idx]++;
                    }
                    //트래픽(명령x)
                    else if (m_stAGV[idx].state == 7 && m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                    {
                        Count_Traffic[idx]++;
                    }
                    //대기 - 충전중
                    //충전 시작 전 state = 4가 되기 때문에, 도착(4)상태이면서, 현재 위치가 충전소인 경우
                    //충전중인 상태로 표시한다. lkw20181231
                    else if ((m_stAGV[idx].state == 8 || m_stAGV[idx].state == 9) ||
                             ((m_stAGV[idx].state == 4) &&
                              (m_stAGV[idx].current == 499 ||           // 음극 ROLL 충전소
                               m_stAGV[idx].current == 1499 ||          // 양극 ROLL 충전소
                               m_stAGV[idx].current == 438 ||           // 음극 REEL 충전소 #1
                               m_stAGV[idx].current == 1438 ||          // 양극 REEL 충전소
                               m_stAGV[idx].current == 496 ||           // 음극 REEL 충전소 #2
                               m_stAGV[idx].current == 1496 ||          // 광폭 충전소
                               m_stAGV[idx].current == 166 ||            // 길거리 음극 ROLL 충전소
                               m_stAGV[idx].current == 1041))           // 길거리 양극 ROLL 충전소
                            )
                    {
                        Count_Charge[idx]++;
                    }
                    //대기 - 자동
                    else if ((m_stAGV[idx].state == 0 || m_stAGV[idx].state == 4) && m_stAGV[idx].mode == 1)
                    {
                        Count_Wait_Auto[idx]++;
                    }
                    //대기 - 수동
                    else if ((m_stAGV[idx].state == 0 || m_stAGV[idx].state == 4) && m_stAGV[idx].mode == 0)
                    {
                        Count_Wait_Manual[idx]++;
                    }

                    //대기 - 적재중
                    else if (m_stAGV[idx].state == 2 || m_stAGV[idx].state == 5)
                    {
                        Count_Loading[idx]++;
                    }
                    //대기 - 이재중
                    else if (m_stAGV[idx].state == 3 || m_stAGV[idx].state == 6)
                    {
                        Count_UnLoading[idx]++;
                    }
                    else
                    {
                        // TEST 로그
                        Count_etc[idx]++;

                        log = string.Format("MODE= {0}, STATUS= {1}, SENSOR= {2}, CURRNENT= {3}, COUNT= {4}, COMMAND= " + m_stAGV[idx].MCS_Vehicle_Command_ID
                                            , m_stAGV[idx].mode
                                            , m_stAGV[idx].state
                                            , m_stAGV[idx].Product_Sensor
                                            , m_stAGV[idx].current
                                            , Count_etc[idx]);

                        Log("AGV_0" + (idx + 1) + "---Working Etc", log);
                    }
                }
                else if (m_stAGV[idx].Error != 0)
                {
                    Count_Error[idx]++;
                }

                Sum_Count[idx] = Count_Drive[idx] + Count_Drive_Command[idx] + Count_Drive_Load[idx] + Count_Drive_Load_Over[idx] + Count_Traffic[idx] +
                                 Count_Traffic_Command[idx] + Count_Wait_Auto[idx] + Count_Wait_Manual[idx] + Count_Charge[idx] + Count_Loading[idx] +
                                 Count_UnLoading[idx] + Count_Error[idx];

                Drive_Command_Rate[idx] = ((double)Count_Drive_Command[idx] / (double)Sum_Count[idx]) * 100;
                Drive_Rate[idx] = ((double)Count_Drive[idx] / (double)Sum_Count[idx]) * 100;
                Drive_Load_Rate[idx] = ((double)Count_Drive_Load[idx] / (double)Sum_Count[idx]) * 100;
                Drive_Load_Over_Rate[idx] = ((double)Count_Drive_Load_Over[idx] / (double)Sum_Count[idx]) * 100;
                Traffic_Rate[idx] = ((double)Count_Traffic[idx] / (double)Sum_Count[idx]) * 100;
                Traffic_Command_Rate[idx] = ((double)Count_Traffic_Command[idx] / (double)Sum_Count[idx]) * 100;
                Wait_Auto_Rate[idx] = ((double)Count_Wait_Auto[idx] / (double)Sum_Count[idx]) * 100;
                Wait_Manual_Rate[idx] = ((double)Count_Wait_Manual[idx] / (double)Sum_Count[idx]) * 100;
                Charge_Rate[idx] = ((double)Count_Charge[idx] / (double)Sum_Count[idx]) * 100;
                Load_Rate[idx] = ((double)Count_Loading[idx] / (double)Sum_Count[idx]) * 100;
                UnLoad_Rate[idx] = ((double)Count_UnLoading[idx] / (double)Sum_Count[idx]) * 100;
                Error_Rate[idx] = ((double)Count_Error[idx] / (double)Sum_Count[idx]) * 100;

                log = string.Format("주행(명령o) = {0}, 명령(x) = {1}, 주행(적재) = {2}, 주행(적재_오버브릿지) = {3}, 트래픽(명령x) = {4}, 대기(자동) = {5}, 대기(수동) = {6}, 충전 = {7}, 적재중 = {8}, 이재중 = {9}, 에러 = {10}, 트래픽(명령O) = {11},"
                                            , Count_Drive_Command[idx]
                                            , Count_Drive[idx]
                                            , Count_Drive_Load[idx]
                                            , Count_Drive_Load_Over[idx]
                                            , Count_Traffic[idx]
                                            , Count_Wait_Auto[idx]
                                            , Count_Wait_Manual[idx]
                                            , Count_Charge[idx]
                                            , Count_Loading[idx]
                                            , Count_UnLoading[idx]
                                            , Count_Error[idx]
                                            , Count_Traffic_Command[idx]);
                Log("AGV_0" + (idx + 1) + "---Working Data", log);

                log = string.Format("[가동률 ---> 주행(명령o) = {0}%, 주행(명령x) = {1}%, 주행(적재) = {2}%, 주행(적재_오버브릿지) = {3}%, 트래픽(명령x) = {4}%, 대기(자동) = {5}%, 대기(수동) = {6}%, 충전 = {7}%, 적재 = {8}%, 이재 = {9}%, 에러 = {10}%, 트래픽(명령O) = {11}%]"
                                            , string.Format("{0:0.0}", Drive_Command_Rate[idx])
                                            , string.Format("{0:0.0}", Drive_Rate[idx])
                                            , string.Format("{0:0.0}", Drive_Load_Rate[idx])
                                            , string.Format("{0:0.0}", Drive_Load_Over_Rate[idx])
                                            , string.Format("{0:0.0}", Traffic_Rate[idx])
                                            , string.Format("{0:0.0}", Wait_Auto_Rate[idx])
                                            , string.Format("{0:0.0}", Wait_Manual_Rate[idx])
                                            , string.Format("{0:0.0}", Charge_Rate[idx])
                                            , string.Format("{0:0.0}", Load_Rate[idx])
                                            , string.Format("{0:0.0}", UnLoad_Rate[idx])
                                            , string.Format("{0:0.0}", Error_Rate[idx])
                                            , string.Format("{0:0.0}", Traffic_Command_Rate[idx]));
                Log("AGV_0" + (idx + 1) + "---Working Rate", log);
            }
            catch (Exception ex)
            {
                Log("TRY Catch Count_LOG_Data", Convert.ToString(ex));
            }
            finally
            {
                T_Work_Rate[idx].Enabled = true;
            }
        }



        private void Work_End_Tick(object sender, EventArgs e)
        {
            /*
            int T_WorkEnd_LGV_No = -1;

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_Work_End[i] == sender)
                {
                    T_WorkEnd_LGV_No = i;
                    break;
                }
            }
            if (T_WorkEnd_LGV_No != -1)
            {
                if(m_stAGV[T_WorkEnd_LGV_No].dqGoal.Count > 0)
                {
                    CS_AGV_Logic.WorkStepEnd(T_WorkEnd_LGV_No);
                }
                
            }
            */
        }


        private void Work_Rate_Tick(object sender, EventArgs e)
        {
            int T_WorkRate_LGV_No = -1;

            for (int i = 0; i < LGV_NUM; i++)
            {
                if (T_Work_Rate[i] == sender)
                {
                    T_WorkRate_LGV_No = i;
                    break;
                }
            }
            if (T_WorkRate_LGV_No != -1)
            {
                Count_LOG_Data(T_WorkRate_LGV_No);
            }
            
        }


        private void T_Check_OverLift_Tick(object sender, EventArgs e)
        {
            CS_Work_DB.Select_OverBridge_Info();
        }

        private void T_Retry_LGV_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_stAGV[Retry_AGV_Num].dqGoal.Count > 0 && m_stAGV[Retry_AGV_Num].mode == 1
            && (m_stAGV[Retry_AGV_Num].state == 0 || m_stAGV[Retry_AGV_Num].state == 4 || m_stAGV[Retry_AGV_Num].state == 8
             || m_stAGV[Retry_AGV_Num].state == 5 || m_stAGV[Retry_AGV_Num].state == 6 || m_stAGV[Retry_AGV_Num].state == 9 || m_stAGV[Retry_AGV_Num].state == 10))
                {
                    FLAG_Retry_Count[Retry_AGV_Num]++;
                }
                else if (m_stAGV[Retry_AGV_Num].dqGoal.Count > 0
                    && (m_stAGV[Retry_AGV_Num].mode == 0 || (m_stAGV[Retry_AGV_Num].state != 0 && m_stAGV[Retry_AGV_Num].state != 4 && m_stAGV[Retry_AGV_Num].state != 8
                 && m_stAGV[Retry_AGV_Num].state != 5 && m_stAGV[Retry_AGV_Num].state != 6 && m_stAGV[Retry_AGV_Num].state != 9 && m_stAGV[Retry_AGV_Num].state != 10)))
                {
                    FLAG_Retry_Count[Retry_AGV_Num] = 0;
                }


                if (FLAG_Retry_Count[Retry_AGV_Num] >= 2)
                {
                    CS_AGV_Logic.Command_Retry(Retry_AGV_Num);
                    FLAG_Retry_Count[Retry_AGV_Num] = 0;
                }
                Retry_AGV_Num++;
                if (Retry_AGV_Num >= LGV_NUM)
                {
                    Retry_AGV_Num = 0;
                }

            }
            catch (Exception ex)
            {
                Log("TryCatch T_Retry_LGV_Tick", Convert.ToString(ex));
            }
        }

        private void timer1_Tick_2(object sender, EventArgs e)
        {
            CS_Work_DB.Insert_Excel_Data_Log("", "", "", "", "", 0, "", "",
                "", "", "", "완료", "", "", "",

                "", "", "","");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            CS_AGV_Logic.Chk_AGV_Error(0,2,49156,7,0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CS_AGV_Logic.Chk_AGV_Error(0, 0, 0, 0, 0);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.ForeColor = Color.Yellow;
            radioButton2.ForeColor = Color.Silver;
            radioButton3.ForeColor = Color.Silver;
            M_7_SLT_First_Mode = true;
            M_7_SLT_Minus_First_Mode = false;
            M_7_SLT_Normal = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.ForeColor = Color.Silver;
            radioButton2.ForeColor = Color.Yellow;
            radioButton3.ForeColor = Color.Silver;
            M_7_SLT_First_Mode = false;
            M_7_SLT_Minus_First_Mode = true;
            M_7_SLT_Normal = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.ForeColor = Color.Silver;
            radioButton2.ForeColor = Color.Silver;
            radioButton3.ForeColor = Color.Yellow;

            M_7_SLT_First_Mode = false;
            M_7_SLT_Minus_First_Mode = false;
            M_7_SLT_Normal = true;
        }

        private void SFL_Test_Btn_Click(object sender, EventArgs e) //SFL 자동문 테스트용
        {
            //CS_Connect_Shutter.DataSendRequest_SFL_InPut_Open(22, 1);
            CS_Connect_Shutter.DataSendRequest_InPut_Open(22, 1);
            CS_Connect_Shutter.DataSendRequest_InPut_Close(22, 0);
            CS_Connect_Shutter.DataSendRequest_OutPut_Open(22, 1);
            CS_Connect_Shutter.DataSendRequest_OutPut_Close(22, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            //CS_Connect_Shutter.DataSendRequest_SFL_InPut_Open(22, 1);

            CS_Connect_Shutter.DataSendRequest_InPut_Close(22, 1);
            CS_Connect_Shutter.DataSendRequest_InPut_Open(22, 0);

            CS_Connect_Shutter.DataSendRequest_OutPut_Close(22, 1);
            CS_Connect_Shutter.DataSendRequest_OutPut_Open(22, 0);
        }

        private void T_Work_End_Tick(object sender, EventArgs e)
        {
            if (m_stAGV[Work_End_LGV_No].dqGoal.Count != 0 && m_stAGV[Work_End_LGV_No].dqWork.Count != 0)
            {
                CS_AGV_Logic.WorkStepEnd(Work_End_LGV_No);
            }

            Work_End_LGV_No++;

            if (Work_End_LGV_No >= LGV_NUM)
            {
                Work_End_LGV_No = 0;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int count = 0;
            count = CS_AGV_Logic.Find_Wait_Job_Count(0);
        }
    }
    
}
