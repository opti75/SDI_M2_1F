using System;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace SDI_LCS
{
    public class AGV_Logic
    {
        Form1 Main;
        public string E_Call_Time = "";
        public string E_Command_ID = "";
        public string E_Carrier_ID = "";
        public string E_Source_Port = "";
        public string E_Dest_Port = "";
        public int E_LGV_No = -1;
        public string E_Alloc_Time = "";
        public string E_Load_Move_Time = "";
        public string E_Load_Time = "";
        public string E_UnLoad_Move_Time = "";
        public string E_UnLoad_Time = "";
        public string E_Alloc_State = "";
        public string E_Load_Move_Time_Result = "";
        public string E_Load_Time_Result = "";
        public string E_UnLoad_Move_Time_Result = ""; 
        public string E_UnLoad_Time_Result = "";
        public string E_Total_Time_Result = "";
        public string E_Complete_Time = "";
        public string[] Alloc_Current = new string[Form1.LGV_NUM];


        #region 배열 선언
        public int[] PLCReadData = new int[100];
        public int[,] ShutterReadData = new int[Form1.SHUTTER_NUM, 100];
        string[] MCS_Send_State = new string[Form1.LGV_NUM];
        public int[,] LGVReadData = new int[Form1.LGV_NUM, 100];
        public short[] LGVSendData = new short[100];
        public short[] LGVSendData_Traffic = new short[100];
        string Source_Port_Num = "";
        string Dest_Port_Num = "";
        public string Wait_Command_ID = "";
        public string Wait_Proiority = "";
        public string Wait_Carrier_ID = "";
        public string Wait_Source_Port = "";
        public string Wait_Dest_Port = "";
        public string Wait_Carrier_Type = "";
        public string Wait_Carrier_LOC = "";
        public string Wait_Process_ID = "";
        public string Wait_Batch_ID = "";
        public string Wait_LOT_ID = "";
        public string Wait_Carrier_S_Count = "";
        public string[] Wait_Carrier_S_ID = new string[10];
        public string Wait_Alloc_State = "";
        public string Wait_Transfer_State = "";
        public string Wait_LGV_No = "";
        public string Wait_Call_Time = "";
        public string Wait_Alloc_Time = "";
        public string Wait_Complete_Time = "";
        public string Wait_Quantity = "";
        public int[,] FLAG_Error = new int[Form1.LGV_NUM, 100];
        public int[] FLAG_Error_DB = new int[Form1.LGV_NUM];
        public string Alloc_Time;
        public string End_Time;
        ArrayList Wait_AGV = new ArrayList();

        ArrayList AGVCount_Wait;       // 대기중인 AGV 리스트

        #endregion
        //상수 선언
        public const string COMMAND_WAIT = "0";
        public const string COMMAND_ING = "1";
        public const string COMMAND_COMPLETE = "2";
        public const string COMMAND_CANCEL = "3";
        public const string COMMAND_ABORT = "4";
        public const string COMMAND_ING_CANCEL_X = "5";
        public const string COMMAND_ING_ABORT_X = "6";
        public const int MOVE = 1;
        public const int LOAD = 2;
        public const int UNLOAD = 3;
        public const int MOVE_LOAD = 4;
        public const int MOVE_UNLOAD = 5;
        public const int MOVE_LOAD_2F = 6;
        public const int MOVE_UNLOAD_2F = 7;
        public const int LOAD_2F = 8;
        public const int UNLOAD_2F = 9;

        public string[] Save_CarrierID = new string[Form1.LGV_NUM];

        public string[] Lift_ID = new string[2];
        public string[] Work_End_3 = new string[2];
        public string[] Work_End_1 = new string[2];
        public string[] Carrier_3 = new string[2];
        public string[] Carrier_1 = new string[2];
        public string[] Lift_State = new string[2];
        public string[] Lift_Mode = new string[2];
        public string[] Link = new string[2];
        

        //다른차가 충전소에 있는지 확인
        public int[] FLAG_Check_Charge_Station_Minus_499 = new int[Form1.LGV_NUM];
        //public int[] FLAG_Check_Charge_Station_Minus_44 = new int[Form1.LGV_NUM]; // 수정 필요
        public int[] FLAG_Check_Charge_Station_Minus_166 = new int[Form1.LGV_NUM]; // 20200826 충전소 이설 수정

        public int[] FLAG_Check_Charge_Station_Plus_1499 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_Plus_1041 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_Plus_1237 = new int[Form1.LGV_NUM];

        public int[] FLAG_Check_Charge_Station_REEL_1438 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_REEL_438 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_REEL_496 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_REEL_442 = new int[Form1.LGV_NUM];
        public int[] FLAG_Check_Charge_Station_REEL_1556 = new int[Form1.LGV_NUM];

        //다른차가 음극 창고영역에 있는지 확인
        public int[] FLAG_Check_WareHouse_Area_Minus_499 = new int[Form1.LGV_NUM];
        //다른차가 음극 프레스 영역에 있는지 확인
        public int[] FLAG_Check_WareHouse_Area_Minus_2 = new int[Form1.LGV_NUM];

        //다른차가 양극 창고영역에 있는지 확인
        public int[] FLAG_Check_WareHouse_Area_Plus_1499 = new int[Form1.LGV_NUM];
        //다른차가 양극 프레스 영역에 있는지 확인
        public int[] FLAG_Check_WareHouse_Area_Plus_2 = new int[Form1.LGV_NUM];

        //생성자
        public AGV_Logic(Form1 CS_Main)
        {
            Main = CS_Main;         
            for(int i = 0; i<Form1.LGV_NUM;i++)
            {
                Alloc_Current[i] = "";
            }
        }
        public void Init_LOG()
        {
            E_Call_Time = "";
            E_Command_ID = "";
            E_Carrier_ID = "";
            E_Source_Port = "";
            E_Dest_Port = "";
            E_LGV_No = -1;
            E_Alloc_Time = "";
            E_Load_Move_Time = "";
            E_Load_Time = "";
            E_UnLoad_Move_Time = "";
            E_UnLoad_Time = "";
            E_Alloc_State = "";
            E_Load_Move_Time_Result = "";
            E_Load_Time_Result = "";
            E_UnLoad_Move_Time_Result = "";
            E_UnLoad_Time_Result = "";
            E_Total_Time_Result = "";
            E_Complete_Time = "";
        }
        public void Init_WaitCommand()
        {
            Wait_Command_ID = "";
            Wait_Proiority = "";
            Wait_Carrier_ID = "";
            Wait_Source_Port = "";
            Wait_Dest_Port = "";
            Wait_Carrier_Type = "";
            Wait_Carrier_LOC = "";
            Wait_Process_ID = "";
            Wait_Batch_ID = "";
            Wait_LOT_ID = "";
            Wait_Carrier_S_Count = "";
            Wait_Alloc_State = "";
            Wait_Transfer_State = "";
            Wait_LGV_No = "";
            Wait_Call_Time = "";
            Wait_Alloc_Time = "";
            Wait_Complete_Time = "";
            Wait_Quantity = "";
        }

        public void Find_Charge_Area_AGV(int idx)
        {
            //다른차가 충전소에 있는지 확인
            FLAG_Check_Charge_Station_Minus_499[idx] = 0;
            //FLAG_Check_Charge_Station_Minus_44[idx] = 0; // 수정 필요
            FLAG_Check_Charge_Station_Minus_166[idx] = 0; //20200826 충전소 이설

            FLAG_Check_Charge_Station_Plus_1499[idx] = 0;
            FLAG_Check_Charge_Station_Plus_1041[idx] = 0;
            FLAG_Check_Charge_Station_Plus_1237[idx] = 0;

            FLAG_Check_Charge_Station_REEL_1438[idx] = 0;
            FLAG_Check_Charge_Station_REEL_438[idx] = 0;
            FLAG_Check_Charge_Station_REEL_496[idx] = 0;

            FLAG_Check_Charge_Station_REEL_442[idx] = 0;
            FLAG_Check_Charge_Station_REEL_1556[idx] = 0;

            //다른차가 음극 창고영역에 있는지 확인
            FLAG_Check_WareHouse_Area_Minus_499[idx] = 0;

            //다른차가 양극 창고영역에 있는지 확인
            FLAG_Check_WareHouse_Area_Plus_1499[idx] = 0;

            //충전소 비었는지 확인
            for (int Other_AGV = 0; Other_AGV < Form1.LGV_NUM; Other_AGV++)
            {
                //자기 자신 제외
                if (idx == Other_AGV) continue;

                if (Main.m_stAGV[Other_AGV].dqGoal.Count > 0)
                {
                    for (int order = 0; order < Main.m_stAGV[Other_AGV].dqGoal.Count; order++)
                    {
                        //충전소로 가는 명령이 있는지 확인
                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 8 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 499)
                        {
                            FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                        }

                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1012 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1499)
                        {
                            FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                        }

                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1041 && Main.m_stAGV[Other_AGV].MCS_Vehicle_Command_ID == "")
                        {
                            FLAG_Check_Charge_Station_Plus_1041[idx] = 1;
                        }

                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1438 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1097 && !(idx == 11 || idx == 12))
                        {
                            FLAG_Check_Charge_Station_REEL_1438[idx] = 1;
                        }

                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 438 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 173 && !(idx == 11 || idx == 12))
                        {
                            FLAG_Check_Charge_Station_REEL_438[idx] = 1;
                        }

                        if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 496 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 277 && !(idx == 11 || idx == 12))
                        {
                            FLAG_Check_Charge_Station_REEL_496[idx] = 1;
                        }

                        //M동하강리프트, 음극7프레스 출고작업이 아닌 58인 경우
                        if ((Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 442 //|| Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1369 
                            || (Main.m_stAGV[Other_AGV].MCS_Source_Port != "CA7FLF01_BBP01" && Main.m_stAGV[Other_AGV].MCS_Source_Port != "CA7PRE01_UBP01"
                            && Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 58)) && (idx == 11 || idx == 12))
                        {
                            FLAG_Check_Charge_Station_REEL_442[idx] = 1;
                        }

                        /*if (Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 1556 || Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 276) //|| Convert.ToInt32(Main.m_stAGV[Other_AGV].dqGoal[order]) == 278)
                        {
                            FLAG_Check_Charge_Station_REEL_1556[idx] = 1;
                        }*/
                    }
                }

                //음극 창고 영역에 있는지 확인 - 영역에 차량 있으면 충전소로 못가게 막기
                if ((Main.m_stAGV[Other_AGV].current >= 1 && Main.m_stAGV[Other_AGV].current <= 27)
                 || (Main.m_stAGV[Other_AGV].current >= 401 && Main.m_stAGV[Other_AGV].current <= 412)
                 || (Main.m_stAGV[Other_AGV].current >= 497 && Main.m_stAGV[Other_AGV].current <= 499))
                {
                    FLAG_Check_WareHouse_Area_Minus_499[idx] = 1;
                }

                //양극 창고 영역에 있는지 확인 - 영역에 차량 있으면 충전소로 못가게 막기
                if ((Main.m_stAGV[Other_AGV].current >= 1001 && Main.m_stAGV[Other_AGV].current <= 1026)
                 || (Main.m_stAGV[Other_AGV].current >= 1401 && Main.m_stAGV[Other_AGV].current <= 1412)
                 || (Main.m_stAGV[Other_AGV].current >= 1497 && Main.m_stAGV[Other_AGV].current <= 1499)
                 || (Main.m_stAGV[Other_AGV].current >= 1137 && Main.m_stAGV[Other_AGV].current <= 1141))
                {
                    FLAG_Check_WareHouse_Area_Plus_1499[idx] = 1;
                }

                //음극 롤창고
                if ((Main.m_stAGV[Other_AGV].current >= 497 && Main.m_stAGV[Other_AGV].current <= 499) && (Main.m_stAGV[Other_AGV].Goal == 8 || Main.m_stAGV[Other_AGV].Goal == 499))
                {
                    FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                }
                //음극 충전기_2 20200826 충전기 이설 수정
                //if ((Main.m_stAGV[Other_AGV].current >= 44 && Main.m_stAGV[Other_AGV].current <= 44) || Main.m_stAGV[Other_AGV].Goal == 44) // 수정 필요
                if ((Main.m_stAGV[Other_AGV].current >= 165 && Main.m_stAGV[Other_AGV].current <= 167) || Main.m_stAGV[Other_AGV].Goal == 166) // 수정 필요
                {
                    //FLAG_Check_Charge_Station_Minus_44[idx] = 1; // 수정 필요
                    FLAG_Check_Charge_Station_Minus_166[idx] = 1; // 수정 필요
                }
                //양극 롤창고
                if ((Main.m_stAGV[Other_AGV].current >= 1497 && Main.m_stAGV[Other_AGV].current <= 1499) || Main.m_stAGV[Other_AGV].Goal == 1012 || Main.m_stAGV[Other_AGV].Goal == 1499)
                {
                    FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                }
                //양극 충전기_2
                if ((Main.m_stAGV[Other_AGV].current >= 1041 && Main.m_stAGV[Other_AGV].current <= 1041) && Main.m_stAGV[Other_AGV].Goal == 1041)
                {
                    FLAG_Check_Charge_Station_Plus_1041[idx] = 1;
                }
                //양극 충전기_3 - 0122 양극 충전소 추가
                if ((Main.m_stAGV[Other_AGV].current >= 1237 && Main.m_stAGV[Other_AGV].current <= 1237)
                 || (Main.m_stAGV[Other_AGV].Goal == 1075 && Main.m_stAGV[Other_AGV].MCS_Vehicle_Command_ID == "")
                 || Main.m_stAGV[Other_AGV].Goal == 1237)
                {
                    FLAG_Check_Charge_Station_Plus_1237[idx] = 1;
                }
                //양극 충전기
                if (Main.m_stAGV[Other_AGV].current >= 1437 && Main.m_stAGV[Other_AGV].current <= 1438 && !(idx == 11 && idx == 12))
                {
                    FLAG_Check_Charge_Station_REEL_1438[idx] = 1;
                }
                //음극 충전소_1로 가는 차량이 있는지 확인
                if (Main.m_stAGV[Other_AGV].current >= 437 && Main.m_stAGV[Other_AGV].current <= 438 && !(idx == 11 && idx == 12))
                {
                    FLAG_Check_Charge_Station_REEL_438[idx] = 1;
                }
                //음극 충전소_2로 가는 차량이 있는지 확인
                if (Main.m_stAGV[Other_AGV].current >= 494 && Main.m_stAGV[Other_AGV].current <= 496 && !(idx == 11 && idx == 12))
                {
                    FLAG_Check_Charge_Station_REEL_496[idx] = 1;
                }
                // M동 하강 리프트쪽으로 가는 차량이 있는지 확인
                if (((Main.m_stAGV[Other_AGV].current >= 440 && Main.m_stAGV[Other_AGV].current <= 442)
                    || (Main.m_stAGV[Other_AGV].current >= 369 && Main.m_stAGV[Other_AGV].current <= 374 && Main.m_stAGV[Other_AGV].Goal == 442)
                    || (Main.m_stAGV[Other_AGV].current >= 1369 && Main.m_stAGV[Other_AGV].current <= 1374 && Main.m_stAGV[Other_AGV].Goal == 442)
                    || (Main.m_stAGV[Other_AGV].current >= 90 && Main.m_stAGV[Other_AGV].current <= 95 && Main.m_stAGV[Other_AGV].Goal == 442)
                    || (Main.m_stAGV[Other_AGV].current >= 57 && Main.m_stAGV[Other_AGV].current <= 58 && Main.m_stAGV[Other_AGV].Goal == 442)
                    || (Main.m_stAGV[Other_AGV].current >= 121 && Main.m_stAGV[Other_AGV].current <= 123 && Main.m_stAGV[Other_AGV].Goal == 442)) && (idx == 11 || idx==12)) 
                {
                    FLAG_Check_Charge_Station_REEL_442[idx] = 1;
                }
                // 오버브릿지 리프트쪽으로 가는 차량이 있는지 확인
                if (Main.m_stAGV[Other_AGV].current >= 1554 && Main.m_stAGV[Other_AGV].current <= 1556 && (idx == 11 || idx == 12)) 
                {
                    FLAG_Check_Charge_Station_REEL_1556[idx] = 1;
                }
            }
        }
        
        //충전 위치 이동
        public void Move_Charge_Station_Logic(int idx, string Type)
        {
            int Check_Minus_Area = 0;
            int FLAG_Check_SLT1 = 0;
            if (Main.m_stAGV[idx].state == 0 || Main.m_stAGV[idx].state == 4 || Main.m_stAGV[idx].state == 5 
                || Main.m_stAGV[idx].state == 6 || Main.m_stAGV[idx].state == 8 || Main.m_stAGV[idx].state == 9)
            { 
                if(Type == "충전소")
                {
                    if (Main.CS_AGV_C_Info[idx].Type == "ROLL_음극")
                    { 
                        Work_Insert(idx, 8, MOVE);
                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "ROLL_양극")
                    {
                        Work_Insert(idx, 1012, MOVE);
                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "광폭_음극")
                    {
                        Work_Insert(idx, 322, MOVE);
                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "광폭_양극")
                    {
                        Work_Insert(idx, 1178, MOVE);
                    }
                }
                else if(Type == "음극_대기장소_1") 
                {
                    if (Main.m_stAGV[idx].current == 499)
                    {
                        Work_Insert(idx, 8, MOVE);
                        //Work_Insert(idx, 44, MOVE); // 수정 필요
                        Work_Insert(idx, 166, MOVE); //20200826 충전기 이설 수정 
                    }
                    else
                    {
                        //Work_Insert(idx, 44, MOVE); // 수정 필요
                        Work_Insert(idx, 166, MOVE);  //20200826 충전기 이설 수정
                    }
                }
                else if (Type == "음극_대기장소_2")
                {           
                    if (Main.m_stAGV[idx].current == 499)
                    {
                        Work_Insert(idx, 8, MOVE);
                        //Work_Insert(idx, 67, MOVE);
                        Work_Insert(idx, 163, MOVE); //20200826 충전기 이설 수정
                    }
                    else
                    {
                        //Work_Insert(idx, 67, MOVE);
                        Work_Insert(idx, 163, MOVE);
                    }
                }
                else if (Type == "양극_대기장소_1")
                {
                    if (Main.m_stAGV[idx].current == 1499)
                    {
                        Work_Insert(idx, 1012, MOVE);
                        Work_Insert(idx, 1041, MOVE);
                    }
                    else
                    {
                        Work_Insert(idx, 1041, MOVE);
                    }
                }
                else if (Type == "양극_대기장소_2")
                {
                    if (Main.m_stAGV[idx].current == 1499)
                    {
                        Work_Insert(idx, 1012, MOVE);
                        Work_Insert(idx, 1075, MOVE);
                    }
                    else
                    {
                        Work_Insert(idx, 1075, MOVE);
                    }
                }
                //1438
                else if (Type == "릴_충전소_1")
                {
                    if (Main.m_stAGV[idx].current == 1084 && !(idx == 11 || idx == 12))

                    {
                        for (int i = 0; i < Form1.LGV_NUM; i++)
                        {
                            if (i == idx) continue;
                            if ((Main.m_stAGV[i].current >= 1085 && Main.m_stAGV[i].current <= 1102)
                            || (Main.m_stAGV[i].current >= 1431 && Main.m_stAGV[i].current <= 1436)
                            || (Main.m_stAGV[i].current >= 1183 && Main.m_stAGV[i].current <= 1183)
                            || (Main.m_stAGV[i].current >= 174 && Main.m_stAGV[i].current <= 183))
                            {
                                FLAG_Check_SLT1 = 1;
                            }
                        }

                        if (FLAG_Check_SLT1 == 1 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 1062, MOVE);
                        }
                        else if (FLAG_Check_SLT1 == 0 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 1097, MOVE);
                        }

                    }

                    else if (Main.m_stAGV[idx].current == 1084 && (idx == 11 || idx == 12))
                    {

                        Work_Insert(idx, 1369, MOVE);
                        //Work_Insert(idx, 93, MOVE);

                        Work_Insert(idx, 442, MOVE);
                    }

                    else if (Main.m_stAGV[idx].current != 1084 && !(idx == 11 || idx == 12))
                    {
                        Work_Insert(idx, 1097, MOVE);
                    }
                    
                }
               
                //438
                else if (Type == "릴_충전소_2" && !(idx == 11 || idx == 12))
                {
                    Work_Insert(idx, 173, MOVE);
                }
                //496
                else if (Type == "릴_충전소_3" && !(idx == 11 || idx == 12))
                {
                    Work_Insert(idx, 277, MOVE);
                }

                //442
                else if (Type == "릴_충전소_4" && (idx == 11 || idx == 12))
                {
                    if ((Main.m_stAGV[idx].current >= 57 && Main.m_stAGV[idx].current <= 64)
                                || (Main.m_stAGV[idx].current >= 440 && Main.m_stAGV[idx].current <= 442)
                                || (Main.m_stAGV[idx].current >= 90 && Main.m_stAGV[idx].current <= 95)
                                || (Main.m_stAGV[idx].current >= 121 && Main.m_stAGV[idx].current <= 123))
                    {
                        //Work_Insert(idx, 93, MOVE);
                        Work_Insert(idx, 58, MOVE);
                        Work_Insert(idx, 442, MOVE); // 추가 0904
                    }
                    else if ((Main.m_stAGV[idx].current >= 108 && Main.m_stAGV[idx].current <= 116)
                          || (Main.m_stAGV[idx].current >= 262 && Main.m_stAGV[idx].current <= 282))
                    {
                        //Work_Insert(idx, 173, MOVE);
                        Work_Insert(idx, 1369, MOVE);
                       // Work_Insert(idx, 93, MOVE);
                        Work_Insert(idx, 442, MOVE);
                    }
                    //음극영역
                    else if (Main.m_stAGV[idx].current < 1000)
                    {
                        Work_Insert(idx, 1369, MOVE);
                        //Work_Insert(idx, 93, MOVE);

                        Work_Insert(idx, 442, MOVE);
                    }
                    //양극영역
                    else
                    {
                        Work_Insert(idx, 1369, MOVE);
                        //Work_Insert(idx, 93, MOVE);

                        Work_Insert(idx, 442, MOVE);
                    }

                    /*if (Main.m_stAGV[idx].current <= 1000)
                    {
                        Work_Insert(idx, 369, MOVE);
                    }
                    else
                    {
                        Work_Insert(idx, 1369, MOVE);
                    }*/
                   
                }
                //1556
                /*else if (Type == "릴_충전소_5" && (idx == 11 || idx == 12))
                {
                    Work_Insert(idx, 276, MOVE);
                }*/
            }
        }

        //충전 위치 이동 - 배터리 없을때
        public void Move_Charge_LowBattary(int idx, string Type)
        {
            Find_Charge_Area_AGV(idx);
            if (Main.m_stAGV[idx].state == 0 || Main.m_stAGV[idx].state == 4 || Main.m_stAGV[idx].state == 5
                || Main.m_stAGV[idx].state == 6 || Main.m_stAGV[idx].state == 8 || Main.m_stAGV[idx].state == 9)
            {
                if (Type == "충전소")
                {
                    if (Main.CS_AGV_C_Info[idx].Type == "REEL_음극" || Main.CS_AGV_C_Info[idx].Type == "REEL_양극" 
                     || Main.CS_AGV_C_Info[idx].Type == "REEL_공용")
                    {
                        //양극 충전소가 비었을때
                        if (FLAG_Check_Charge_Station_REEL_1438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            if (Main.m_stAGV[idx].current == 1084)
                            {
                                Work_Insert(idx, 1062, MOVE);
                                Work_Insert(idx, 1097, MOVE);
                                Work_Insert(idx, 1438, MOVE);
                            }
                            else if (Main.m_stAGV[idx].current != 1084)
                            {
                                Work_Insert(idx, 1097, MOVE);
                                Work_Insert(idx, 1438, MOVE);
                            }

                        }
                        else if (FLAG_Check_Charge_Station_REEL_438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            if (Main.m_stAGV[idx].current >= 1084 && Main.m_stAGV[idx].current <= 1102)
                            {
                                Work_Insert(idx, 1062, MOVE);
                                Work_Insert(idx, 173, MOVE);
                                Work_Insert(idx, 438, MOVE);
                            }
                            else if (!(Main.m_stAGV[idx].current >= 1084 && Main.m_stAGV[idx].current <= 1102))
                            {
                                Work_Insert(idx, 173, MOVE);
                                Work_Insert(idx, 438, MOVE);
                            }
                        }
                        else if (FLAG_Check_Charge_Station_REEL_496[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 277, MOVE);
                            Work_Insert(idx, 496, MOVE);
                        }

                        //20200820 REEL 충전소 추가 적용
                        else if (FLAG_Check_Charge_Station_REEL_442[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            if (Main.m_stAGV[idx].current == 58)
                            {
                                //Work_Insert(idx, 1369, MOVE);
                                //Work_Insert(idx, 93, MOVE);
                                Work_Insert(idx, 442, MOVE);
                            }
                            else if ((Main.m_stAGV[idx].current >= 369 && Main.m_stAGV[idx].current <= 374)
                                || (Main.m_stAGV[idx].current >= 1369 && Main.m_stAGV[idx].current <= 1374))
                            {
                                //Work_Insert(idx, 93, MOVE);
                                Work_Insert(idx, 442, MOVE);
                            }
                            else if ((Main.m_stAGV[idx].current >= 262 && Main.m_stAGV[idx].current <= 282)
                                || (Main.m_stAGV[idx].current >= 108 && Main.m_stAGV[idx].current <= 116))
                            {
                                Work_Insert(idx, 1369, MOVE);
                                //Work_Insert(idx, 369, MOVE);
                                //Work_Insert(idx, 93, MOVE);
                                Work_Insert(idx, 442, MOVE);
                            }
                            //음극영역
                            else if (Main.m_stAGV[idx].current < 1000)
                            {
                                Work_Insert(idx, 1369, MOVE);
                                //Work_Insert(idx, 93, MOVE);
                                Work_Insert(idx, 442, MOVE);
                            }
                            //양극영역
                            else
                            {
                                Work_Insert(idx, 1369, MOVE);
                                //Work_Insert(idx, 93, MOVE);
                                Work_Insert(idx, 442, MOVE);
                            }
                        }

                        /*else if (FLAG_Check_Charge_Station_REEL_1556[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 276, MOVE);
                            Work_Insert(idx, 1556, MOVE);
                        }*/
                    }


                    else if (Main.CS_AGV_C_Info[idx].Type == "ROLL_음극")
                    {
                        if (FLAG_Check_Charge_Station_Minus_499[idx] == 0 && FLAG_Check_WareHouse_Area_Minus_499[idx] == 0) //CS_AGV_Logic.FLAG_Check_Charge_Station_Minus_499[idx] == 0 && CS_AGV_Logic.FLAG_Check_WareHouse_Area_Minus_499[idx] == 0
                        {
                            Work_Insert(idx, 8, MOVE);
                            Work_Insert(idx, 499, MOVE);
                        }
                        //충전장소가 찼을때
                        //else if (Main.m_stAGV[idx].current != 64 && Main.m_stAGV[idx].current != 58)
                        else if (Main.m_stAGV[idx].current != 64 && Main.m_stAGV[idx].current != 58) //20200826 충전기 이설 수정
                        {
                            //166번 이동
                            //Move_Charge_Station_Logic(idx, "음극_대기장소_2");

                            if (FLAG_Check_Charge_Station_Minus_166[idx] == 0)
                            {
                                Work_Insert(idx, 166, 1);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            else if (FLAG_Check_Charge_Station_Minus_166[idx] == 1)
                            {
                                Work_Insert(idx, 163, 1);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                        }
                        //else if (Main.m_stAGV[idx].current == 64 || Main.m_stAGV[idx].current == 58)
                        else if (Main.m_stAGV[idx].current == 64 || Main.m_stAGV[idx].current == 58) //20200826 충전기 이설 수정
                        {
                            //163번 이동
                            //Move_Charge_Station_Logic(idx, "음극_대기장소_1");
                            if(FLAG_Check_Charge_Station_Minus_166[idx] == 0)
                            {
                                Work_Insert(idx, 166, 1);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            else if (FLAG_Check_Charge_Station_Minus_166[idx] == 1)
                            {
                                Work_Insert(idx, 163, 1);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                        }

                    }

                    else if (Main.CS_AGV_C_Info[idx].Type == "ROLL_양극")
                    {
                        if (FLAG_Check_Charge_Station_Plus_1499[idx] == 0)
                        {
                            Work_Insert(idx, 1012, MOVE);
                            Work_Insert(idx, 1499, MOVE);
                        }
                        else if (FLAG_Check_Charge_Station_Plus_1499[idx] == 1 && FLAG_Check_Charge_Station_Plus_1041[idx] == 0)
                        {
                            Work_Insert(idx, 1041, MOVE);
                        }
                        else if (FLAG_Check_Charge_Station_Plus_1499[idx] == 1 && FLAG_Check_Charge_Station_Plus_1041[idx] == 1 && FLAG_Check_Charge_Station_Plus_1237[idx] == 0)
                        {
                            Work_Insert(idx, 1075, MOVE);
                            Work_Insert(idx, 1237, MOVE);
                        }
                    }
                }
            }
        }

        
        public string LGV_Error_Msg(int Error)
        {
            string Msg = "";

            switch (Error)
            {
                case 0: Msg = "정상"; break;
                case 1: Msg = "비상정지"; break;
                case 2: Msg = "범퍼 충돌"; break;
                case 3: Msg = "라인 이탈"; break;
                case 4: Msg = "위치에러"; break;
                case 5: Msg = "보드에러"; break;
                case 6: Msg = "리프트 상승 리밋"; break;
                case 7: Msg = "리프트 하강 리밋"; break;
                case 8: Msg = "리프트 걸림 센서"; break;
                case 9: Msg = "정지위치 에러"; break;
                case 10: Msg = "전방 조향 좌 리밋"; break;
                case 11: Msg = "전방 조향 우 리밋"; break;
                case 12: Msg = "후방 조향 좌 리밋"; break;
                case 13: Msg = "후방 조향 우 리밋"; break;
                case 14: Msg = "전방 주행 모터 에러"; break;
                case 15: Msg = "후방 주행 모터 에러"; break;
                case 16: Msg = "전방 조향 모터 에러"; break;
                case 17: Msg = "후방 조향 모터 에러"; break;
                case 18: Msg = "리프트 모터 에러"; break;
                case 19: Msg = "적재 타임 아웃"; break;
                case 20: Msg = "이재 타임 아웃"; break;
                case 21: Msg = "PIO에러"; break;
                case 22: Msg = "경로 계획 에러"; break;
                case 24: Msg = "정적 장애물"; break;
                case 25: Msg = "입고대 에러"; break;
                case 26: Msg = "출고대 에러"; break;
                case 27: Msg = "제품 감지 불량"; break;
                case 28: Msg = "리프트 높이 에러"; break;
                case 29: Msg = "제품 없음"; break;
                case 30: Msg = "주행중 제품감지불량"; break;
                case 31: Msg = "자동문 안열림"; break;
                case 32: Msg = "충전 에러"; break;
                case 33: Msg = "주행 방향 에러"; break;
                case 34: Msg = "범퍼 프리(주행중)"; break;
                case 35: Msg = "작업 지연"; break;
                case 36: Msg = "이중 입고 (LGV)"; break;
                case 37: Msg = "이중 입고 (LCS)"; break;
                case 38: Msg = "리프트 작업 타임 아웃"; break;
                case 39: Msg = "트래픽 타임아웃"; break; //0331 kjh 트래픽 타임아웃 추가
            }
            return Msg;
        }

        public string LGV_State_Msg(int Error)
        {
            string Msg = "";

            switch (Error)
            {
                case 0: Msg = "대기"; break;
                case 1: Msg = "주행중"; break;
                case 2: Msg = "적재중"; break;
                case 3: Msg = "이재중"; break;
                case 4: Msg = "도착"; break;
                case 5: Msg = "적재완료"; break;
                case 6: Msg = "이재완료"; break;
                case 7: Msg = "트래픽"; break;
                case 8: Msg = "충전시작"; break;
                case 9: Msg = "충전중"; break;
                case 10: Msg = "충전완료"; break;
            }
            return Msg;
        }

        // 광폭 기재랙 이중 입고시 AGV 알람 전송. lkw20190129
        public void Chk_AGV_Duplication(int idx)
        {
            // 1. 광폭 기재만 확인하기 때문에 9호기(음극 광폭), 11호기(양극 광폭)만 진행한다.
            if (!(idx == 8 || idx == 10))
            {
                return;
            }
            /*
            // 2. 현재 작업중이 아니면 RETRUN 한다.
            if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
            {
                return;
            }
            */
            // 3. 현재 작업중인 AGV의 목적지를 가져와 조회한다.
            int port_load = Main.CS_Work_DB.Select_DB_Duplication(idx, Main.m_stAGV[idx].MCS_Dest_Port);

            // 4. AGV의 현재 위치를 가져온다.
            int current = Main.m_stAGV[idx].current;

            // 5. 위치에 따라 port_load = 1이면 에러를 전송한다.
            // 9호기(음극 광폭)의 경우
            if (idx == 8)
            {
                if ((current >= 331 && current <= 378) && port_load == 1)
                {
                    Main.CS_Connect_LGV.DataSend_Work_Duplocation(idx,1);
                    Main.Log("이중입고 전송", "호기 : " + (idx + 1) + ", 1 전송");
                }
                else if ((current >= 331 && current <= 378) && (port_load == 0 || port_load == 2))
                {
                    Main.CS_Connect_LGV.DataSend_Work_Duplocation(idx, 0);
                    Main.Log("이중입고 전송", "호기 : " + (idx + 1) + ", 0 전송");
                }
            }
            // 11호기(양극 광폭)의 경우
            else
            {
                if (((current >= 1199 && current <= 1210) || (current >= 199 && current <= 210)) && port_load == 1)
                {
                    Main.CS_Connect_LGV.DataSend_Work_Duplocation(idx,1);
                    Main.Log("이중입고 전송", "호기 : " + (idx + 1) + ", 1 전송");
                }
                else if(((current >= 1199 && current <= 1210)  || (current >= 199 && current <= 210)) && (port_load == 0 || port_load == 2))
                {
                    Main.CS_Connect_LGV.DataSend_Work_Duplocation(idx, 0);
                    Main.Log("이중입고 전송", "호기 : "+ (idx+1) + ", 0 전송");
                }        
            }
        }

        // AGV_Error 메세지. bit 단위로 따로 전송함. lkw20190116
        public void Chk_AGV_Error(int idx, int Error, int Error_1, int Error_2, int Error_3)
        {
            string Msg = "";
            int Error_Code;
            string Start_Date = "";
            int Error_Code_Size = 40; // 에러는 0~39까지 있다.
            Start_Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // 모니터링에 표시할 에러 메세지
            Msg = LGV_Error_Msg(Error);
            
            Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
            {
                // 에러는 0~38까지 있다.
                for (int i = 0; i < Error_Code_Size; i++)
                {
                    // 0~15
                    if (i <= 15)
                    {
                        int count_1 = i;

                        // 발생한 에러가 아니면 continue
                        if ((Error_1 & (int)Math.Pow(2, count_1)) == 0)
                            continue;
                    }
                    // 16~31
                    else
                    if (i >= 16 && i <= 31)
                    {
                        int count_2 = i - 16;

                        // 발생한 에러가 아니면 continue
                        if ((Error_2 & (int)Math.Pow(2, count_2)) == 0)
                            continue;
                    }
                    // 32~47
                    else
                    {
                        int count_3 = i - 32;

                        // 발생한 에러가 아니면 continue
                        if ((Error_3 & (int)Math.Pow(2, count_3)) == 0)
                            continue;
                    }

                    Error_Code = i;

                    if (FLAG_Error[idx, i] == 0)
                    {
                        // 에러 상태가 유지중일때 중복 보고를 막기 위해 플레그 사용.
                        FLAG_Error[idx, i] = 1;

                        //Main.CS_Work_DB.Select_DB_Error_Log();
                        //차량 변수에 추가
                        Main.m_stAGV[idx].MCS_Alarm_ID = Convert.ToString(Error_Code);
                        Main.m_stAGV[idx].MCS_Alarm_Text = Main.Alarm_List[Error_Code];
                        //알람 보고 시나리오
                        if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 128, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 102, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 702, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 209, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }
                        else if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 128, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 702, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }
                        else
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 128, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 702, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }

                        //충전 에러 발생시 차량을 재충전 시키기 위해 이동 후 복귀. lkw20190530
                        if (Error_Code == 32)
                        {
                            Main.Move_Charge_Station_Retry(idx);
                        }
                    } 
                }
                // 에러 해제 보고.
                for (int i = 0; i < Error_Code_Size; i++)
                {
                    // 0~15
                    if (i <= 15)
                    {
                        int count_1 = i;

                        // 발생한 에러가 클리어 아니면 continue
                        if ((Error_1 & (int)Math.Pow(2, count_1)) > 0)
                            continue;
                    }
                    // 16~31
                    else
                    if (i >= 16 && i <= 31)
                    {
                        int count_2 = i - 16;

                        // 발생한 에러가 클리어 아니면 continue
                        if ((Error_2 & (int)Math.Pow(2, count_2)) > 0)
                            continue;
                    }
                    // 32~47
                    else
                    {
                        int count_3 = i - 32;

                        // 발생한 에러가 클리어 아니면 continue
                        if ((Error_3 & (int)Math.Pow(2, count_3)) > 0)
                            continue;
                    }

                    Error_Code = i;

                    if (FLAG_Error[idx, i] == 1)
                    {
                        //차량 변수에 추가
                        Main.m_stAGV[idx].MCS_Alarm_ID = Convert.ToString(Error_Code);
                        Main.m_stAGV[idx].MCS_Alarm_Text = Main.Alarm_List[Error_Code];

                        if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 101, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 210, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                        }
                        else if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }
                        else
                        {
                            Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }

                        // 에러 상태가 해제되면 초기화.
                        Main.m_stAGV[idx].MCS_Alarm_ID = "0";
                        Main.m_stAGV[idx].MCS_Alarm_Text = "";
                        FLAG_Error[idx, i] = 0;
                    }
                }

                // 통신 DisConnect 에러. 다시 붙었으면 이 함수를 들어오기 때문에 바로 초기화.
                if (Main.m_stAGV[idx].MCS_Alarm_ID == "23")
                {
                    if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                    {
                        Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                        Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 101, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 210, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    else if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                    {
                        Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                        Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    else
                    {
                        Main.Form_MCS.SendS5F1(Main.Form_MCS.m_nDeviceID, 0, Main.m_stAGV[idx].MCS_Alarm_ID, Main.m_stAGV[idx].MCS_Alarm_Text);
                        Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 701, 0, Convert.ToUInt16(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                    }
                    Main.m_stAGV[idx].MCS_Alarm_ID = "0";
                    Main.m_stAGV[idx].MCS_Alarm_Text = "";
                }

                Main.TB_Error[idx].Text = Msg;
                

                // 모니터링 에러 상태 색상 표시.
                if (Main.m_stAGV[idx].Error != 0)
                {
                    Main.TB_Error[idx].BackColor = Color.Red;

                    if (FLAG_Error_DB[idx] == 0)
                    {
                        FLAG_Error_DB[idx] = 1;
                        Msg = Main.Alarm_List[Error];
                        Main.CS_Work_DB.Insert_Current_Error(Start_Date, Main.m_stAGV[idx].MCS_Vehicle_ID, Msg, Main.m_stAGV[idx].current, Main.m_stAGV[idx].MCS_Carrier_ID);
                    }
                }
                else
                {
                    Main.TB_Error[idx].BackColor = Color.Lime;

                    Main.m_stAGV[idx].MCS_Alarm_ID = "0";
                    Main.m_stAGV[idx].MCS_Alarm_Text = "";

                    if (FLAG_Error_DB[idx] == 1)
                    {
                        FLAG_Error_DB[idx] = 0;
                    }
                    
                }
                    
            }));
        }

        // AGV_State 메세지
        public void Chk_AGV_State(int idx, int state)
        {
            String Msg = "";
            switch (state)
            {
                case 0: Msg = "대기"; break;
                case 1: Msg = "주행중"; break;
                case 2: Msg = "적재중"; break;
                case 3: Msg = "이재중"; break;
                case 4: Msg = "도착"; break;
                case 5: Msg = "적재완료"; break;
                case 6: Msg = "이재완료"; break;
                case 7: Msg = "트래픽"; break;
                case 8: Msg = "충전시작"; break;
                case 9: Msg = "충전중"; break;
                case 10: Msg = "충전완료"; break;
            }
            Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
            {
                Main.TB_State[idx].Text = Msg;
            }));
        }

        public void Find_Middle_LGV(int idx)
        {
            int Wait_Job_Count = 0;
            int Low_Battery_Num = -1;
            int Check_Middle_Battery_LGV = 0;

            Wait_Job_Count = Find_Wait_Job_Count(idx);
            Low_Battery_Num = Find_Low_Battery_LGV(idx);

            for(int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (i == idx) continue;
                if(Main.m_stAGV[i].FLAG_Middle_Battery_LGV == 1)
                {
                    Check_Middle_Battery_LGV = 1;
                }
            }
            //다른 차량 플래그 확인, 설정값(60%)이하, 설정 작업 개수 이하, 배터리가 제일 작으면 플래그 ON
            if (Check_Middle_Battery_LGV == 0 && Main.m_stAGV[idx].Battery < Main.m_stAGV[idx].Setting_Middle_Charge_Value &&
                idx == Low_Battery_Num && Wait_Job_Count <= Main.m_stAGV[idx].Setting_Low_Battery_Job_Count)
            {
                if(Main.m_stAGV[idx].current != 1054 && Main.m_stAGV[idx].current != 1313 && Main.m_stAGV[idx].current != 1268 && Main.m_stAGV[idx].current != 1286)
                {
                    //중간 배터리 플래그 살리기
                    Main.m_stAGV[idx].FLAG_Middle_Battery_LGV = 1;
                }
                
            }

            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (Main.m_stAGV[i].FLAG_Middle_Battery_LGV == 1 && Wait_Job_Count >= Main.m_stAGV[idx].Setting_Low_Battery_Job_Count)
                {
                    Main.m_stAGV[i].FLAG_Middle_Battery_LGV = 0;
                }
            }
        }

        public int Find_Wait_Job_Count(int idx)
        {
            int Wait_Job_Count = 0;

            for(int wait_Job_Count = 0; wait_Job_Count < Main.CS_Work_DB.waitCommand.Count; wait_Job_Count++)
            {
                if (Main.m_stCommand[wait_Job_Count].Alloc_State != "0") continue;

                for(int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    if(Main.CS_Work_Path[Work_Station_Count].Work_Station == Main.m_stCommand[wait_Job_Count].Source_Port)
                    {
                        if(Main.CS_AGV_C_Info[idx].Type == Main.CS_Work_Path[Work_Station_Count].Type)
                        {
                            Wait_Job_Count++;
                        }
                    }
                }
            }

            return Wait_Job_Count;
        }

        public int Find_Low_Battery_LGV(int idx)
        {
            int Low_Battery_LGV = -1;
            int Min_Battery = Main.m_stAGV[idx].Battery;

            for(int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (idx == i) continue; //자기 자신 제외
                if (Main.m_stAGV[i].connect == 0) continue;
                //같은 종류만 비교
                if (Main.CS_AGV_C_Info[idx].Type == Main.CS_AGV_C_Info[i].Type)
                {
                    //배터리 가장 낮은놈 찾기
                    if (Min_Battery > Main.m_stAGV[i].Battery)
                    {
                        Min_Battery = Main.m_stAGV[i].Battery;
                        Low_Battery_LGV = i;
                    }
                }
            }
            //가장 낮은놈이 없을때
            if (Low_Battery_LGV == -1)
            {
                Low_Battery_LGV = idx;
            }

            return Low_Battery_LGV;
        }
        // 대기중인 AGV List
        public ArrayList WaitAGVList()
        {
            //0: 대기, 1 : 주행중, 2 : 적재중,3 : 이재중, 4:도착, 5:적재완료, 6:이재완료, 7:트래픽
            Wait_AGV.Clear();
 
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                if (Main.m_stAGV[i].Error != 0) continue;                      //에러가 있으면 대기 상태가 아님. lkw20190530
                if (Main.m_stAGV[i].current == 0) continue;
                if (Main.m_stAGV[i].FLAG_LGV_Charge == 1) continue;
                if (Main.m_stAGV[i].dqGoal.Count > 0) continue;
                if (Main.m_stAGV[i].dqWork.Count > 0) continue;
                if (Main.m_stAGV[i].connect == 0) continue;
                if (Main.m_stAGV[i].state != 0 && Main.m_stAGV[i].state != 4 && Main.m_stAGV[i].state != 5 
                    && Main.m_stAGV[i].state != 6 && Main.m_stAGV[i].state != 8 && Main.m_stAGV[i].state != 9
                    && Main.m_stAGV[i].state != 10) continue;

                if (Main.m_stAGV[i].MCS_Vehicle_Command_ID == ""
                    && Main.m_stAGV[i].mode == 1)
                {
                    Find_Middle_LGV(i);
                    if(Main.m_stAGV[i].FLAG_Middle_Battery_LGV != 1)
                    {
                        Wait_AGV.Add(i);
                    }
                    

                }
            }
            return Wait_AGV;
        }
        
        //작업 할당 (1) - 타이머
        public void WorkAlloc()
        {
            int FLAG_Check_Wait_Station;
            int FLAG_Check_Port_Source = 0; ;
            int FLAG_Check_Port_Dest = 0;
            int FLAG_Check_Type_Source = 0;
            int FLAG_Check_Type_Dest = 0;

            int idx = -1;
           
            //대기중인 차량 탐색
            Wait_AGV = WaitAGVList();

            for (int i = 0; i < Wait_AGV.Count; i++)
            {
                //대기중인 명령 검색
                Main.CS_WorkSchedule.waitCommand(Convert.ToInt32(Wait_AGV[i]));
                idx = Convert.ToInt32(Convert.ToInt32(Wait_AGV[i]));

                if(Main.CS_Work_DB.waitCommand.Count > 0)
                {
                    //대기중인 차량이 배터리가 설정한 값보다 낮을땐 충전장소 가기
                    if (Main.m_stAGV[idx].Battery < Main.m_stAGV[idx].Setting_Charge_Value
                    && ((Main.m_stAGV[idx].current >= 1268 && Main.m_stAGV[idx].current <= 1270)
                      || Main.m_stAGV[idx].current == 1313 || Main.m_stAGV[idx].current == 1054 || Main.m_stAGV[idx].current == 1286))
                    {
                        if (Main.m_stAGV[idx].current == 1054)
                        {
                            //작업 없는지 확인 사살
                            if (Main.m_stAGV[idx].dqGoal.Count == 0 && Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                            {
                                if (!(Wait_Source_Port == "CA7FLF01_BBP01" &&
                                   (Wait_Dest_Port == "CC8SLT01_UBP01" || Wait_Dest_Port == "CC8SLT01_UBP02"
                                 || Wait_Dest_Port == "CC9SLT01_UBP01" || Wait_Dest_Port == "CC9SLT01_UBP02"
                                 || Wait_Dest_Port == "CCASLT01_UBP01" || Wait_Dest_Port == "CCASLT01_UBP02"
                                 || Wait_Dest_Port == "CA7SLT01_UBP01" || Wait_Dest_Port == "CA7SLT01_UBP02"
                                 || Wait_Dest_Port == "CA8SLT01_UBP01" || Wait_Dest_Port == "CA8SLT01_UBP02"
                                 || Wait_Dest_Port == "CA9SLT01_UBP01" || Wait_Dest_Port == "CA9SLT01_UBP02")))
                                {
                                    if (Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 1438 && Main.m_stAGV[idx].current != 442 // 20200820 442 위치 조건 추가
                                     && Main.m_stAGV[idx].current != 438 && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 1556 // 20200820 1556 위치 조건 추가
                                     //&& Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041)
                                     && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041) //20200826 충전기 이설 수정
                                    {
                                        //충전 장소로 갔다가 바로 안나오게 막기
                                        Main.m_stAGV[idx].FLAG_LGV_Charge = 1;
                                        //충전위치 이동 함수   
                                        Move_Charge_LowBattary(idx, "충전소");
                                        Main.Charge_Count[idx] = 0;
                                    }
                                }
                            }
                        }
                        else if (Main.m_stAGV[idx].current != 1054)
                        {
                            //작업 없는지 확인 사살
                            if (Main.m_stAGV[idx].dqGoal.Count == 0 && Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                            {
                                if (Wait_Source_Port != "CC8FLF02_BBP01" && Wait_Source_Port != "CC8ERB01_BBP01"
                                 && Wait_Source_Port != "CC8ERB01_BBP02" && Wait_Source_Port != "CC8ERB01_BBP03"
                                 && Wait_Source_Port != "CC8ERB01_BBP04")
                                {
                                    if (Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 1438 && Main.m_stAGV[idx].current != 442 // 20200820 442 위치 조건 추가
                                     && Main.m_stAGV[idx].current != 438 && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 1556 // 20200820 1556 위치 조건 추가
                                    // && Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041)
                                     && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041)  //20200826 충전기 이설 수정
                                    {
                                        //충전 장소로 갔다가 바로 안나오게 막기
                                        Main.m_stAGV[idx].FLAG_LGV_Charge = 1;
                                        //충전위치 이동 함수   
                                        Move_Charge_LowBattary(idx, "충전소");
                                        Main.Charge_Count[idx] = 0;
                                    }
                                }
                            }
                        }

                    }
                    else if (Main.m_stAGV[idx].Battery < Main.m_stAGV[idx].Setting_Charge_Value
                    && (!(Main.m_stAGV[idx].current >= 1268 && Main.m_stAGV[idx].current <= 1270)
                       && Main.m_stAGV[idx].current != 1313 && Main.m_stAGV[idx].current != 1054 && Main.m_stAGV[idx].current != 1286))
                    {
                        //작업 없는지 확인 사살
                        if (Main.m_stAGV[idx].dqGoal.Count == 0 && Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "")
                        {
                            if (Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 1438 && Main.m_stAGV[idx].current != 442 // 20200820 442 위치 조건 추가
                             && Main.m_stAGV[idx].current != 438 && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 1556 // 20200820 1556 위치 조건 추가
                             //&& Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041)
                             && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041) //20200826 충전기 이설 수정
                            {
                                //충전 장소로 갔다가 바로 안나오게 막기
                                Main.m_stAGV[idx].FLAG_LGV_Charge = 1;
                                //충전위치 이동 함수   
                                Move_Charge_LowBattary(idx, "충전소");
                                Main.Charge_Count[idx] = 0;
                            }
                        }
                    }
                }
                
                
                if (idx != -1 && Main.CS_Work_DB.waitCommand.Count > 0 && Main.Flag_MCS_Auto == 1)
                {
                    if (Wait_Source_Port != "" && Wait_Dest_Port != "" && Wait_Command_ID != "" && Wait_Alloc_State == COMMAND_WAIT
                        && Main.m_stAGV[idx].MCS_Vehicle_Command_ID == "" && Main.m_stAGV[idx].FLAG_LGV_Charge == 0 
                        && Main.m_stAGV[idx].dqGoal.Count == 0 && Main.m_stAGV[idx].dqWork.Count == 0)
                    {
                        FLAG_Check_Wait_Station = 0;
                        for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                        {
                            //받은 명령이 Form_Work_Path_Setting에서 셋팅 되어있는지 확인
                            if (Main.CS_Work_Path[Work_Station_Count].Work_Station == Wait_Source_Port)
                            {
                                FLAG_Check_Port_Source = 1;
                                //종류 확인
                                if ((Main.CS_Work_Path[Work_Station_Count].Type == Main.CS_AGV_C_Info[idx].Type) 
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "기재" 
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "REEL_공용" && (idx == 0 || idx == 1 || idx == 9 || idx == 11 || idx == 12))
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(음)" && (idx == 5 || idx == 6 || idx == 7 || idx == 8))
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(양)" && (idx == 2 || idx == 3 || idx == 4 || idx == 10))))
                                {
                                    FLAG_Check_Type_Source = 1;
                                }
                            }

                            else if (Main.CS_Work_Path[Work_Station_Count].Work_Station == Wait_Dest_Port)
                            {
                                FLAG_Check_Port_Dest = 1;
                                //종류 확인
                                if ((Main.CS_Work_Path[Work_Station_Count].Type == Main.CS_AGV_C_Info[idx].Type)
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "기재"
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "REEL_공용" && (idx == 0 || idx == 1 || idx == 9 || idx == 11 || idx == 12))
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(음)" && (idx == 5 || idx == 6 || idx == 7 || idx == 8))
                                 || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(양)" && (idx == 2 || idx == 3 || idx == 4 || idx == 10))))
                                {
                                    FLAG_Check_Type_Dest = 1;
                                }
                            }

                            if (FLAG_Check_Port_Source == 1 && FLAG_Check_Port_Dest == 1 && FLAG_Check_Type_Source == 1 && FLAG_Check_Type_Dest == 1)
                            {
                                break;
                            }
                        }
                        //차가 대기 위치에 있는지 확인
                        for (int Wait_Station_Count = 0; Wait_Station_Count < Main.CS_Work_DB.Path_Count; Wait_Station_Count++)
                        {
                            if ((Main.m_stAGV[idx].current
                            == Convert.ToInt32(Main.CS_Work_Path[Wait_Station_Count].Goal_Area))
                            && Main.CS_Work_Path[Wait_Station_Count].Type == "충전소")
                            {
                                FLAG_Check_Wait_Station = 1;
                                break;
                            }

                        }
                        //조건 다맞으면 작업 할당 - 정상 시나리오
                        if (FLAG_Check_Port_Source == 1 && FLAG_Check_Port_Dest == 1
                         && FLAG_Check_Type_Source == 1 && FLAG_Check_Type_Dest == 1)
                        {
                            Alloc_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                            Main.CS_Work_DB.Update_Command_Info(idx, Wait_Command_ID, "5", idx, Alloc_Time, "");
                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

                            Alloc_Current[idx] = Convert.ToString(Main.m_stAGV[idx].current);

                            Main.m_stAGV[idx].MCS_Vehicle_Command_ID = Wait_Command_ID;
                            Main.m_stAGV[idx].MCS_Source_Port = Wait_Source_Port;
                            Main.m_stAGV[idx].MCS_Dest_Port = Wait_Dest_Port;

                            if (FLAG_Check_Wait_Station == 1)
                            {
                                DoWorkAlloc(idx, "Exit_Wait_Station");
                            }

                            DoWorkAlloc(idx, "Normal");

                            Save_CarrierID[idx] = Wait_Carrier_ID;
                            
                            Main.m_stAGV[idx].MCS_Carrier_LOC = Wait_Source_Port;
                            Main.m_stAGV[idx].Source_Port_Num = Source_Port_Num;
                            Main.m_stAGV[idx].Dest_Port_Num = Dest_Port_Num;

                            Main.m_stAGV[idx].MCS_Vehicle_Priority = Wait_Proiority;
                            //Main.CS_Work_DB.waitCommand.Clear();
    
                            Init_WaitCommand();
                            Main.CS_Work_DB.Update_AGV_Info_WaitCarrierID(idx,Main.m_stAGV[idx].MCS_Vehicle_ID, Save_CarrierID[idx]);
                            //MCS보고 - VehicleAssigned(604)
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 604, 0, Convert.ToUInt32(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Log("SendMCSLOG", "VehicleAssigned 604 차량 = " + (idx + 1));
                            break;
                        }

                        else if (Wait_Source_Port == Main.m_stAGV[idx].MCS_Vehicle_ID
                             && FLAG_Check_Type_Dest == 1 && FLAG_Check_Port_Dest == 1)
                        {

                            Alloc_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            //할당시작 시간 입력
                            Main.CS_Work_DB.Update_Command_Info(idx, Wait_Command_ID, "5", idx, Alloc_Time, "");
                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();

                            Main.m_stAGV[idx].MCS_Vehicle_Command_ID = Wait_Command_ID;
                            Main.m_stAGV[idx].MCS_Source_Port = Wait_Source_Port;
                            Main.m_stAGV[idx].MCS_Dest_Port = Wait_Dest_Port;

                            if (FLAG_Check_Wait_Station == 1)
                            {
                                DoWorkAlloc(idx, "Exit_Wait_Station");
                            }
                       
                            DoWorkAlloc(idx, "Abnormal");

                            Save_CarrierID[idx] = Wait_Carrier_ID;
                            Main.m_stAGV[idx].MCS_Carrier_LOC = Wait_Source_Port;
                            Main.m_stAGV[idx].Source_Port_Num = Source_Port_Num;
                            Main.m_stAGV[idx].Dest_Port_Num = Dest_Port_Num;
                            Main.m_stAGV[idx].MCS_Vehicle_Command_ID = Wait_Command_ID;
                            Main.m_stAGV[idx].MCS_Vehicle_Priority = Wait_Proiority;
                            // Main.CS_Work_DB.waitCommand.Clear();
                            

                            Init_WaitCommand();
                            Main.CS_Work_DB.Update_AGV_Info_WaitCarrierID(idx, Main.m_stAGV[idx].MCS_Vehicle_ID, Save_CarrierID[idx]);

                            //MCS보고 - VehicleAssigned(604)
                            Main.Form_MCS.SendS6F11(Main.Form_MCS.m_nDeviceID, 0, 6, 11, 0, 604, 0, Convert.ToUInt32(idx), Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.Log("SendMCSLOG", "VehicleAssigned 604 차량 = " + (idx + 1));
                      
                            break;
                        }
                    }
                }
                
            }
        }

        public void Move_Charge_Station()
        {
            for(int idx = 0; idx < Form1.LGV_NUM; idx++)
            {
                if (Main.Flag_MCS_Auto == 1 && Main.m_stAGV[idx].mode == 1 && Main.m_stAGV[idx].dqGoal.Count == 0
                && (Main.m_stAGV[idx].state == 0 || Main.m_stAGV[idx].state == 4))
                {
                    Find_Charge_Area_AGV(idx);
                    if (Main.CS_AGV_C_Info[idx].Type == "ROLL_음극")
                    {
                        //8번에서 명령 없을때 충전소로 고고싱
                        if (Main.m_stAGV[idx].current == 8)
                        {
                            if (FLAG_Check_Charge_Station_Minus_499[idx] == 0 && FLAG_Check_WareHouse_Area_Minus_499[idx] == 0)
                            {
                                FLAG_Check_Charge_Station_Minus_499[idx] = 1;
                                Work_Insert(idx, 499, MOVE);
                            }
                            else if (FLAG_Check_Charge_Station_Minus_166[idx] == 0)
                            {
                                Work_Insert(idx, 166, MOVE);
                            }
                            else if (FLAG_Check_Charge_Station_Minus_166[idx] == 1)
                            {
                                Work_Insert(idx, 163, MOVE);
                            }
                            else
                            {
                                Work_Insert(idx, 163, MOVE);
                            }
                        }
                        
                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "ROLL_양극")
                    {
                        //1012번에서 명령 없을때 충전소로 고고싱
                        if (Main.m_stAGV[idx].current == 1012)
                        {
                            FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                            Work_Insert(idx, 1499, MOVE);
                        }

                        else if (Main.m_stAGV[idx].current == 1075)
                        {
                            //충전장소가 비었을때
                            if (FLAG_Check_Charge_Station_Plus_1499[idx] == 0 && FLAG_Check_WareHouse_Area_Plus_1499[idx] == 0)
                            {
                                FLAG_Check_Charge_Station_Plus_1499[idx] = 1;
                                Work_Insert(idx, 1012, MOVE);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            //충전장소가 찼을때
                            else if ((FLAG_Check_Charge_Station_Plus_1499[idx] == 1 || FLAG_Check_WareHouse_Area_Plus_1499[idx] == 1)
                                   && FLAG_Check_Charge_Station_Plus_1041[idx] == 0)
                            {
                                FLAG_Check_Charge_Station_Plus_1041[idx] = 1;
                                //1041번 이동
                                Work_Insert(idx, 1041, MOVE);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                            //충전장소가 찼을때
                            else if (FLAG_Check_Charge_Station_Plus_1041[idx] == 1 && FLAG_Check_Charge_Station_Plus_1499[idx] == 1)
                            {
                                FLAG_Check_Charge_Station_Plus_1237[idx] = 1;
                                Work_Insert(idx, 1237, MOVE);
                                Main.m_stAGV[idx].Flag_Wait_Station = 0;
                            }
                        }
                    }
                    //1, 2, 10호기 충전소 할당
                    else if (Main.CS_AGV_C_Info[idx].Type == "REEL_음극" || Main.CS_AGV_C_Info[idx].Type == "REEL_양극" || Main.CS_AGV_C_Info[idx].Type == "REEL_공용")
                    {
                        //1097번에서 명령 없을때 충전소로 고고싱
                        if (Main.m_stAGV[idx].current == 1097 && FLAG_Check_Charge_Station_REEL_1438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 1438, MOVE);
                        }
                        //277번에서 명령 없을때 충전소로 고고싱
                        else if (Main.m_stAGV[idx].current == 277 && FLAG_Check_Charge_Station_REEL_496[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 496, MOVE);
                        }
                        //173번에서 명령 없을때 충전소로 고고싱
                        else if (Main.m_stAGV[idx].current == 173 && FLAG_Check_Charge_Station_REEL_438[idx] == 0 && !(idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 438, MOVE);
                        }
                        //1369번에서 명령 없을때 충전소로 고고싱 (20200820 충전소 이동 조건 추가)
                        else if ((Main.m_stAGV[idx].current == 58 || Main.m_stAGV[idx].current == 1369) && FLAG_Check_Charge_Station_REEL_442[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 442, MOVE);
                        }

                        /*
                        //276번에서 명령 없을때 충전소로 고고싱 (20200820 충전소 이동 조건 추가)
                        else if (Main.m_stAGV[idx].current == 276 && FLAG_Check_Charge_Station_REEL_1556[idx] == 0 && (idx == 11 || idx == 12))
                        {
                            Work_Insert(idx, 1556, MOVE);
                        }*/

                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "광폭_양극")
                    {
                        //1178번에서 명령 없을때 충전소로 고고싱
                        if (Main.m_stAGV[idx].current == 1178)
                        {
                            Work_Insert(idx, 1496, MOVE);
                        }
                    }
                    else if (Main.CS_AGV_C_Info[idx].Type == "광폭_음극")
                    {
                        //322번에서 명령 없을때 충전소로 고고싱
                        if (Main.m_stAGV[idx].current == 322)
                        {
                            Work_Insert(idx, 665, MOVE);
                        }
                    }
                }
            }
        }

        //작업 할당(2)
        public void DoWorkAlloc(int idx, string Type)
        {
            if (Type == "Normal")
            {
                for (int i = 0; i < 2; i++)
                {
                    // 0 : source
                    // 1 : des

                    string t_sStr;
                    if (i == 0)
                    {
                        t_sStr = Wait_Source_Port;
                        
                        //오버브릿지 갈때는 다른 경로 주기
                        if (Wait_Source_Port == "CC9SLT01_UBP01" && Wait_Dest_Port == "CA7FLF02_BBP01")
                        {
                            t_sStr = "CC9SLT01_UBP01_2";
                        }
                        else if (Wait_Source_Port == "CC9SLT01_UBP02" && Wait_Dest_Port == "CA7FLF02_BBP01")
                        {
                            t_sStr = "CC9SLT01_UBP02_2";
                        }
                        
                        //M동하강리프트
                        if (Wait_Source_Port == "CA7FLF01_BBP01" && 
                            ((Main.m_stAGV[idx].current >= 440 && Main.m_stAGV[idx].current <= 442)
                            || (Main.m_stAGV[idx].current >= 90 && Main.m_stAGV[idx].current <= 95)
                            || (Main.m_stAGV[idx].current >= 57 && Main.m_stAGV[idx].current <= 64)))
                        {
                            t_sStr = "CA7FLF01_BBP01_2";
                        }
                        

                    }
                    else
                    {
                        t_sStr = Wait_Dest_Port;
                    }

                    for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                    {
                        if (Main.CS_Work_Path[Work_Station_Count].Work_Station == t_sStr)
                        {
                            //출발지
                            if (i == 0)
                            {
                                //출발지 하강리프트
                                if (Wait_Source_Port == "CC8FLF02_BBP01")
                                {
                                    //도착지가 버퍼1~4 양극 3식 슬리터일때
                                    //도착지가 릴코어회수 양극#2 일때. lkw20190508
                                    if (Wait_Dest_Port == "CC8ERB01_BBP01" || Wait_Dest_Port == "CC8ERB01_BBP02"
                                    || Wait_Dest_Port == "CC8ERB01_BBP03" || Wait_Dest_Port == "CC8ERB01_BBP04"
                                    || Wait_Dest_Port == "CCASLT01_UBP01" || Wait_Dest_Port == "CCASLT01_UBP02"
                                    || Wait_Dest_Port == "CC8RCS02_BBP01")
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_LOAD);

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_LOAD);
                                        }

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_LOAD);
                                        }

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                        Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                        Work_Insert(idx, 1268, MOVE_UNLOAD);
                                    }

                                    //오버 상승일때
                                    else if (Wait_Dest_Port == "CA7FLF02_BBP01")
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_LOAD);

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_LOAD);
                                        }

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_LOAD);
                                        }

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                        Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                        Work_Insert(idx, 1351, MOVE_UNLOAD);
                                        Work_Insert(idx, 1062, MOVE_UNLOAD);
                                    }
                                    //버퍼,3식슬리터가 아닐때, 오버상승이 아닐때
                                    else if (Wait_Dest_Port != "CC8ERB01_BBP01" && Wait_Dest_Port != "CC8ERB01_BBP02"
                                         && Wait_Dest_Port != "CC8ERB01_BBP03" && Wait_Dest_Port != "CC8ERB01_BBP04"
                                         && Wait_Dest_Port != "CCASLT01_UBP01" && Wait_Dest_Port != "CCASLT01_UBP02"
                                         && Wait_Dest_Port != "CA7FLF02_BBP01")
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_LOAD);

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_LOAD);
                                        }

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_LOAD);
                                        }

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                        Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                        Work_Insert(idx, 1351, MOVE_UNLOAD);
                                    }
                                }
                                else if (Wait_Source_Port != "CC8FLF02_BBP01")
                                {
                                    if (Main.m_stAGV[idx].current == 1237)
                                    {
                                        if (Wait_Source_Port == "CC9PRE01_LBP01")
                                        {
                                            Work_Insert(idx, 1475, MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                            Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE_UNLOAD);

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE_UNLOAD);
                                            }

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE_UNLOAD);
                                            }
                                        }
                                        else if (Wait_Source_Port == "CC9COT01_UBP01")
                                        {
                                            Work_Insert(idx, 1238, MOVE_LOAD);

                                            Work_Insert(idx, 1469, MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                            Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE_UNLOAD);

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE_UNLOAD);
                                            }

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE_UNLOAD);
                                            }
                                        }
                                        else if (Wait_Source_Port == "CC9COT01_UBP02" || Wait_Source_Port == "CC9COT01_LBP01")
                                        {
                                            Work_Insert(idx, 1231, MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                            Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE_UNLOAD);

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE_UNLOAD);
                                            }

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE_UNLOAD);
                                            }
                                        }
                                        else
                                        {
                                            Work_Insert(idx, 1062, MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_LOAD);

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_LOAD);
                                            }

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_LOAD);
                                            }

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                            Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE_UNLOAD);

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE_UNLOAD);
                                            }

                                            if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                            != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                            {
                                                Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE_UNLOAD);
                                            }
                                        }
                                    }
                                    else if (Main.m_stAGV[idx].current != 1237)
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_LOAD);

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_LOAD);
                                        }

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_LOAD);
                                        }

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_LOAD);

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), LOAD);
                                        Source_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE_UNLOAD);

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE_UNLOAD);
                                        }

                                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                        {
                                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE_UNLOAD);
                                        }
                                    }

                                }

                            }
                            else if (i == 1)
                            {
                                if (Wait_Source_Port == "CC8FLF01_BBP01" && Wait_Dest_Port == "CA7FLF02_BBP01")
                                {
                                    Work_Insert(idx, 1268, MOVE_UNLOAD);

                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_UNLOAD);

                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), UNLOAD);
                                    Dest_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                    Work_Insert(idx, 1313, MOVE);

                                }
                                else if (!(Wait_Source_Port == "CC8FLF01_BBP01" && Wait_Dest_Port == "CA7FLF02_BBP01"))
                                {
                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_UNLOAD);

                                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                                    != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_UNLOAD);
                                    }

                                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                                    != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_UNLOAD);
                                    }

                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_UNLOAD);

                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), UNLOAD);
                                    Dest_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                                    Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE);

                                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                                    != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE);
                                    }

                                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                                    != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                                    {
                                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            //source가 대기 차량 일때
            else if (Type == "Abnormal")
            {
                for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    if (Main.CS_Work_Path[Work_Station_Count].Work_Station == Wait_Dest_Port)
                    {
                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1), MOVE_UNLOAD);

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_1)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2), MOVE_UNLOAD);
                        }

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_2)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Stop_Area_3), MOVE_UNLOAD);
                        }

                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), MOVE_UNLOAD);
                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area), UNLOAD);
                        Source_Port_Num = "0";
                        Dest_Port_Num = Main.CS_Work_Path[Work_Station_Count].Goal_Area;

                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE);

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE);
                        }

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                        }
                        break;
                    }
                }
            }
            else if (Type == "Exit_Wait_Station")
            {
                for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    //차량 위치가 충전소 인지 확인
                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area) == Main.m_stAGV[idx].current)
                    {
                        //충전소 경유지 1이동
                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE);
                        //충전소 경유지 2이동 - 1이랑 같으면 패스
                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE);
                        }

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                        }
                        break;
                    }
                }
            }
            else if (Type == "Exit_Goal_Node")
            {
                for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    //차량 위치가 작업지 인지 확인
                    if ((Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area) == Main.m_stAGV[idx].current) ||
                        (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area) == Main.m_stAGV[idx].current + 1) ||
                        (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Goal_Area) == Main.m_stAGV[idx].current + 2))
                    {
                        //작업지 경유지 1이동
                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1), MOVE);
                        //충전소 경유지 2이동 - 1이랑 같으면 패스
                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE);
                        }

                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                        }
                        break;
                    }
                }
            }
            else if (Type == "Exit_Exit1_Node")
            {
                for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    //차량 위치가 탈출지1 인지 확인
                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1) == Main.m_stAGV[idx].current)
                    {
                        //탈출지 2이동
                        Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2), MOVE);
                        //탈출지 2이동 - 1이랑 같으면 패스
                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2)
                        != Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3))
                        {
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                        }

                        break;
                    }
                }
            }
            else if (Type == "Exit_Exit2_Node")
            {
                for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                {
                    //차량 위치가 충전소 인지 확인
                    if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_2) == Main.m_stAGV[idx].current)
                    {
                        //차량 위치가 탈출지1 인지 확인
                        if (Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_1) == Main.m_stAGV[idx].current)
                        {
                            //탈출지 3이동
                            Work_Insert(idx, Convert.ToInt32(Main.CS_Work_Path[Work_Station_Count].Exit_Area_3), MOVE);
                            break;
                        }
                    }
                }
            }
        }
        //작업할당(3)
        public void Work_Insert(int idx, int Goal, int Work)
        {     
            if(Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP09" || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP10"
            || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP11" || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP12"
            || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP13" || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP14"
            || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP15" || Main.m_stAGV[idx].MCS_Source_Port == "CC8PLB02_BBP16")
            {
                if(Work == 4)
                {
                    Work = Work + 2;
                }
                else if(Work == 2)
                {
                    Work = Work + 6;
                }
            }
            else if(Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP09" || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP10"
            || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP11" || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP12"
            || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP13" || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP14"
            || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP15" || Main.m_stAGV[idx].MCS_Dest_Port == "CC8PLB02_BBP16")
            {
                if (Work == 5)
                {
                    Work = Work + 2;
                }
                else if (Work == 3)
                {
                    Work = Work + 6;
                }
            }
            
            Main.m_stAGV[idx].dqGoal.Add(Goal);//목적지
            Main.m_stAGV[idx].dqWork.Add(Work);//작업
            //회복 DB 값 추기 해주기
            Main.CS_Work_DB.Insert_DB_Recovery(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, Convert.ToString(Goal), Convert.ToString(Work));
            //차량에 명령 보내기
            LGV_SendData(idx);
            Thread.Sleep(10);
        }
        public void Test_LGV_Data(int LGV_No)
        {
            string log;
            double Battey;


            Main.m_stAGV[LGV_No].current = Convert.ToInt32((LGVReadData[LGV_No, 0] & 0xff));
            Main.m_stAGV[LGV_No].current |= Convert.ToInt32(((LGVReadData[LGV_No, 1] & 0xff) << 8));

            Main.m_stAGV[LGV_No].Goal = Convert.ToInt32((LGVReadData[LGV_No, 2] & 0xff));
            Main.m_stAGV[LGV_No].Goal |= Convert.ToInt32(((LGVReadData[LGV_No, 3] & 0xff) << 8));

            Main.m_stAGV[LGV_No].mode = Convert.ToInt32((LGVReadData[LGV_No, 4] & 0xff));
            Main.m_stAGV[LGV_No].mode |= Convert.ToInt32(((LGVReadData[LGV_No, 5] & 0xff) << 8));

            Main.m_stAGV[LGV_No].state = Convert.ToInt32((LGVReadData[LGV_No, 6] & 0xff));
            Main.m_stAGV[LGV_No].state |= Convert.ToInt32(((LGVReadData[LGV_No, 7] & 0xff) << 8));

            Main.m_stAGV[LGV_No].Error = Convert.ToInt32((LGVReadData[LGV_No, 8] & 0xff));
            Main.m_stAGV[LGV_No].Error |= Convert.ToInt32(((LGVReadData[LGV_No, 9] & 0xff) << 8));

            Main.m_stAGV[LGV_No].x = Convert.ToInt32((LGVReadData[LGV_No, 10] & 0xff));
            Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 11] & 0xff) << 8));
            Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 12] & 0xff) << 16));
            Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 13] & 0xff) << 24));

            Main.m_stAGV[LGV_No].y = Convert.ToInt32((LGVReadData[LGV_No, 14] & 0xff));
            Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 15] & 0xff) << 8));
            Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 16] & 0xff) << 16));
            Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 17] & 0xff) << 24));

            Main.m_stAGV[LGV_No].t = Convert.ToInt32((LGVReadData[LGV_No, 18] & 0xff));
            Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 19] & 0xff) << 8));
            Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 20] & 0xff) << 16));
            Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 21] & 0xff) << 24));

            Main.m_stAGV[LGV_No].Battery = Convert.ToInt32((LGVReadData[LGV_No, 22] & 0xff));
            Main.m_stAGV[LGV_No].Battery |= Convert.ToInt32(((LGVReadData[LGV_No, 23] & 0xff) << 8));
            
            Battey = Math.Truncate((((double)Main.m_stAGV[LGV_No].Battery - 2400) / 380) * 100);

            //MCS보고용 상태로 바꿔주기
            if (Main.m_stAGV[LGV_No].state == 1 || Main.m_stAGV[LGV_No].state == 7)
            {
                MCS_Send_State[LGV_No] = "3";
            }
            else if (Main.m_stAGV[LGV_No].state == 2)
            {
                MCS_Send_State[LGV_No] = "5";
            }
            else if (Main.m_stAGV[LGV_No].state == 3)
            {
                MCS_Send_State[LGV_No] = "6";
            }
            else if (Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10
                  || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 0)
            {
                MCS_Send_State[LGV_No] = "4";
            }
            
            if (Battey > 100)
            {
                Battey = 100;
            }
            else if (Battey < 0)
            {
                Battey = 0;
            }

            Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
            {
                //Main.CS_DRAW_INFO.DRAW_AGV(LGV_No);
                Main.TB_Current[LGV_No].Text = Convert.ToString(Main.m_stAGV[LGV_No].current);
                Main.TB_Goal[LGV_No].Text = Convert.ToString(Main.m_stAGV[LGV_No].Goal);

                // 저전압 충전이아니면 20181212 lhc
                if (Main.m_stAGV[LGV_No].Charge == 0)
                {
                    Main.TB_Battery[LGV_No].BackColor = Color.White;
                    Main.TB_Battery[LGV_No].Text = Battey + "%";
                }


                Chk_AGV_State(LGV_No, Main.m_stAGV[LGV_No].state);
                Chk_AGV_Error(LGV_No, Main.m_stAGV[LGV_No].Error, Main.m_stAGV[LGV_No].Error_1, Main.m_stAGV[LGV_No].Error_2, Main.m_stAGV[LGV_No].Error_3);

                if (Main.m_stAGV[LGV_No].mode == 0)
                {
                    Main.GB_LGV_Info[LGV_No].ForeColor = System.Drawing.Color.Orange;
                    Main.GB_LGV_Info[LGV_No].Text = "LGV " + (LGV_No + 1) + "호(수동)";
                }
                else if (Main.m_stAGV[LGV_No].mode == 1)
                {
                    Main.GB_LGV_Info[LGV_No].ForeColor = System.Drawing.Color.Green;
                    Main.GB_LGV_Info[LGV_No].Text = "LGV " + (LGV_No + 1) + "호(자동)";
                }
            }));
        }
        // LGV READ 데이터
        public void LGV_Data(int LGV_No)
        {
            string log;
            double Battey;
            string LGV_Error = "";
            string LGV_State = "";
            int FW_Angle = 0;
            int BW_Angle = 0;

            try
            {
                Main.m_stAGV[LGV_No].x = Convert.ToInt32((LGVReadData[LGV_No, 1] & 0xff));
                Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 0] & 0xff) << 8));
                Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 3] & 0xff) << 16));
                Main.m_stAGV[LGV_No].x |= Convert.ToInt32(((LGVReadData[LGV_No, 2] & 0xff) << 24));

                Main.m_stAGV[LGV_No].y = Convert.ToInt32((LGVReadData[LGV_No, 5] & 0xff));
                Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 4] & 0xff) << 8));
                Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 7] & 0xff) << 16));
                Main.m_stAGV[LGV_No].y |= Convert.ToInt32(((LGVReadData[LGV_No, 6] & 0xff) << 24));

                Main.m_stAGV[LGV_No].t = Convert.ToInt32((LGVReadData[LGV_No, 9] & 0xff));
                Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 8] & 0xff) << 8));
                Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 11] & 0xff) << 16));
                Main.m_stAGV[LGV_No].t |= Convert.ToInt32(((LGVReadData[LGV_No, 10] & 0xff) << 24));

                Main.m_stAGV[LGV_No].current = Convert.ToInt32((LGVReadData[LGV_No, 13] & 0xff));
                Main.m_stAGV[LGV_No].current |= Convert.ToInt32(((LGVReadData[LGV_No, 12] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Goal = Convert.ToInt32((LGVReadData[LGV_No, 15] & 0xff));
                Main.m_stAGV[LGV_No].Goal |= Convert.ToInt32(((LGVReadData[LGV_No, 14] & 0xff) << 8));

                Main.m_stAGV[LGV_No].target = Convert.ToInt32((LGVReadData[LGV_No, 17] & 0xff));
                Main.m_stAGV[LGV_No].target |= Convert.ToInt32(((LGVReadData[LGV_No, 16] & 0xff) << 8));

                Main.m_stAGV[LGV_No].state = Convert.ToInt32((LGVReadData[LGV_No, 19] & 0xff));
                Main.m_stAGV[LGV_No].state |= Convert.ToInt32(((LGVReadData[LGV_No, 18] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Error = Convert.ToInt32((LGVReadData[LGV_No, 21] & 0xff));
                Main.m_stAGV[LGV_No].Error |= Convert.ToInt32(((LGVReadData[LGV_No, 20] & 0xff) << 8));

                Main.m_stAGV[LGV_No].mode = Convert.ToInt32((LGVReadData[LGV_No, 23] & 0xff));
                Main.m_stAGV[LGV_No].mode |= Convert.ToInt32(((LGVReadData[LGV_No, 22] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Battery = Convert.ToInt32((LGVReadData[LGV_No, 25] & 0xff));
                Main.m_stAGV[LGV_No].Battery |= Convert.ToInt32(((LGVReadData[LGV_No, 24] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Product_Sensor = Convert.ToInt32((LGVReadData[LGV_No, 27] & 0xff));
                Main.m_stAGV[LGV_No].Product_Sensor |= Convert.ToInt32(((LGVReadData[LGV_No, 26] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Working_Error = Convert.ToInt32((LGVReadData[LGV_No, 29] & 0xff));
                Main.m_stAGV[LGV_No].Working_Error |= Convert.ToInt32(((LGVReadData[LGV_No, 28] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Ask_Abort = Convert.ToInt32((LGVReadData[LGV_No, 31] & 0xff));
                Main.m_stAGV[LGV_No].Ask_Abort |= Convert.ToInt32(((LGVReadData[LGV_No, 30] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Traffic_Skip = Convert.ToInt32((LGVReadData[LGV_No, 33] & 0xff));
                Main.m_stAGV[LGV_No].Traffic_Skip |= Convert.ToInt32(((LGVReadData[LGV_No, 32] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Lift_Height = Convert.ToInt32((LGVReadData[LGV_No, 35] & 0xff));
                Main.m_stAGV[LGV_No].Lift_Height |= Convert.ToInt32(((LGVReadData[LGV_No, 34] & 0xff) << 8));

                Main.m_stAGV[LGV_No].FW_Driving_Motor_Angle = Convert.ToInt32((LGVReadData[LGV_No, 37] & 0xff));
                Main.m_stAGV[LGV_No].FW_Driving_Motor_Angle |= Convert.ToInt32(((LGVReadData[LGV_No, 36] & 0xff) << 8));

                Main.m_stAGV[LGV_No].BW_Driving_Motor_Angle = Convert.ToInt32((LGVReadData[LGV_No, 39] & 0xff));
                Main.m_stAGV[LGV_No].BW_Driving_Motor_Angle |= Convert.ToInt32(((LGVReadData[LGV_No, 38] & 0xff) << 8));

                Main.m_stAGV[LGV_No].LGV_Way = Convert.ToInt32((LGVReadData[LGV_No, 41] & 0xff));
                Main.m_stAGV[LGV_No].LGV_Way |= Convert.ToInt32(((LGVReadData[LGV_No, 40] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Error_1 = Convert.ToInt32((LGVReadData[LGV_No, 43] & 0xff));
                Main.m_stAGV[LGV_No].Error_1 |= Convert.ToInt32(((LGVReadData[LGV_No, 42] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Error_2 = Convert.ToInt32((LGVReadData[LGV_No, 45] & 0xff));
                Main.m_stAGV[LGV_No].Error_2 |= Convert.ToInt32(((LGVReadData[LGV_No, 44] & 0xff) << 8));

                Main.m_stAGV[LGV_No].Error_3 = Convert.ToInt32((LGVReadData[LGV_No, 47] & 0xff));
                Main.m_stAGV[LGV_No].Error_3 |= Convert.ToInt32(((LGVReadData[LGV_No, 46] & 0xff) << 8));


                Battey = Math.Truncate((((double)Main.m_stAGV[LGV_No].Battery - 2400) / 380) * 100);



                //MCS보고용 상태로 바꿔주기
                if (Main.m_stAGV[LGV_No].state == 1 || Main.m_stAGV[LGV_No].state == 7)
                {
                    MCS_Send_State[LGV_No] = "3";
                }
                else if (Main.m_stAGV[LGV_No].state == 2)
                {
                    MCS_Send_State[LGV_No] = "5";
                }
                else if (Main.m_stAGV[LGV_No].state == 3)
                {
                    MCS_Send_State[LGV_No] = "6";
                }
                else if (Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10
                      || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 0)
                {
                    MCS_Send_State[LGV_No] = "4";
                }

                Main.m_stAGV[LGV_No].MCS_Vehicle_Current_Position = Convert.ToString(Main.m_stAGV[LGV_No].current);
                Main.m_stAGV[LGV_No].MCS_Vehicle_Goal = Convert.ToString(Main.m_stAGV[LGV_No].Goal);
                Main.m_stAGV[LGV_No].MCS_Vehicle_Next_Postion = Convert.ToString(Main.m_stAGV[LGV_No].target);
                Main.m_stAGV[LGV_No].MCS_Vehicle_State = MCS_Send_State[LGV_No];

                Main.CS_Work_DB.Update_AGV_Info(LGV_No, Main.m_stAGV[LGV_No].MCS_Vehicle_ID, MCS_Send_State[LGV_No],
                    Convert.ToString(Main.m_stAGV[LGV_No].current), Convert.ToString(Main.m_stAGV[LGV_No].target), Convert.ToString(Main.m_stAGV[LGV_No].Goal),
                    Main.m_stAGV[LGV_No].MCS_Carrier_ID, Main.m_stAGV[LGV_No].MCS_Install_Time, Main.m_stAGV[LGV_No].MCS_Source_Port, Main.m_stAGV[LGV_No].MCS_Dest_Port,
                    Main.m_stAGV[LGV_No].MCS_Carrier_LOC, Main.m_stAGV[LGV_No].MCS_Carrier_Type, Main.m_stAGV[LGV_No].MCS_Alarm_ID, Main.m_stAGV[LGV_No].MCS_Alarm_Text,
                    Main.m_stAGV[LGV_No].Source_Port_Num, Main.m_stAGV[LGV_No].Dest_Port_Num, Main.m_stAGV[LGV_No].MCS_Vehicle_Command_ID, Main.m_stAGV[LGV_No].MCS_Vehicle_Priority);

                //  Main.CS_Work_DB.Select_MCS_AGV_Info();


                Main.Send_MCS(LGV_No);
                if (Battey > 100)
                {
                    Battey = 100;
                }
                else if (Battey < 0)
                {
                    Battey = 0;
                }
                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    Main.CS_Draw_Node_AGV.AGV_Location_Nav(Main.m_stAGV[LGV_No].x, Main.m_stAGV[LGV_No].y, Main.m_stAGV[LGV_No].t, LGV_No);
                    Main.TB_Current[LGV_No].Text = Convert.ToString(Main.m_stAGV[LGV_No].current);
                    Main.TB_Goal[LGV_No].Text = Convert.ToString(Main.m_stAGV[LGV_No].Goal);

                    Chk_AGV_State(LGV_No, Main.m_stAGV[LGV_No].state);
                    Chk_AGV_Error(LGV_No, Main.m_stAGV[LGV_No].Error, Main.m_stAGV[LGV_No].Error_1, Main.m_stAGV[LGV_No].Error_2, Main.m_stAGV[LGV_No].Error_3);
                    // AGV 광폭 기재랙 이중 입고 시 알람 전송. lkw20190129
                    Chk_AGV_Duplication(LGV_No);

                    LGV_Error = LGV_Error_Msg(Main.m_stAGV[LGV_No].Error);
                    LGV_State = LGV_State_Msg(Main.m_stAGV[LGV_No].state);

                    // 저전압 충전이아니면 20181212 lhc
                    if (Main.m_stAGV[LGV_No].FLAG_LGV_Charge == 0)
                    {
                        Main.TB_Battery[LGV_No].BackColor = Color.White;
                        Main.TB_Battery[LGV_No].Text = Battey + "%";
                    }

                    if (Main.m_stAGV[LGV_No].mode == 0)
                    {
                        Main.GB_LGV_Info[LGV_No].ForeColor = System.Drawing.Color.Yellow;
                        Main.GB_LGV_Info[LGV_No].Text = "LGV " + (LGV_No + 1) + "호(수동)";
                    }
                    else if (Main.m_stAGV[LGV_No].mode == 1)
                    {
                        Main.GB_LGV_Info[LGV_No].ForeColor = System.Drawing.Color.Lime;
                        Main.GB_LGV_Info[LGV_No].Text = "LGV " + (LGV_No + 1) + "호(자동)";
                    }

                    if (Main.m_stAGV[LGV_No].FW_Driving_Motor_Angle > 180)
                    {
                        FW_Angle = Main.m_stAGV[LGV_No].FW_Driving_Motor_Angle - 360;
                    }
                    else
                    {
                        FW_Angle = Main.m_stAGV[LGV_No].FW_Driving_Motor_Angle;
                    }

                    if (Main.m_stAGV[LGV_No].BW_Driving_Motor_Angle > 180)
                    {
                        BW_Angle = Main.m_stAGV[LGV_No].BW_Driving_Motor_Angle - 360;
                    }
                    else
                    {
                        BW_Angle = Main.m_stAGV[LGV_No].BW_Driving_Motor_Angle;
                    }

                    //pgb 관상용 프로그램 190424 ------------------------------------------------------------------------------------------------
                    Main.CS_Work_DB.Update_LGV_Info_View_LCS(LGV_No, Main.m_stAGV[LGV_No].x, Main.m_stAGV[LGV_No].y, Main.m_stAGV[LGV_No].t, LGV_State, Main.m_stAGV[LGV_No].current, Main.m_stAGV[LGV_No].Goal, Main.m_stAGV[LGV_No].mode, LGV_Error, Battey + "%");
                    //----------------------------------------------------------------------------------------------------------------------------

                    log = string.Format("current = {0},\tGoal = {1},\tTarget = {2},\tState = {3},\tError = {4},\tMode = {5},\tBattey = ({6}){7}%,\tX : {8},\tY : {9},\tT : {10} ," +
                                        "\tAbort : {11},\tSensor : {12},\tRetryCount : {13},\t저전압 : {14},\t트래픽 무시 : {15},\t리프트 높이 : {16},\t전방 조향 각도 : {17},\t후방 조향 각도 : {18},\t주행 방향 : {19},\t중간 저전압 : {20},\t에러_1 : {21},\t에러_2 : {22},\t에러_3 : {23}"
                                            , Main.m_stAGV[LGV_No].current, Main.m_stAGV[LGV_No].Goal, Main.m_stAGV[LGV_No].target
                                            , LGV_State, LGV_Error, Main.m_stAGV[LGV_No].mode,
                                              Main.m_stAGV[LGV_No].Battery, Battey, Main.m_stAGV[LGV_No].x, Main.m_stAGV[LGV_No].y,
                                              Main.m_stAGV[LGV_No].t, Main.m_stAGV[LGV_No].Ask_Abort, Main.m_stAGV[LGV_No].Product_Sensor,
                                              Main.FLAG_Retry_Count[LGV_No], Main.m_stAGV[LGV_No].FLAG_LGV_Charge, Main.m_stAGV[LGV_No].Traffic_Skip
                                            , Main.m_stAGV[LGV_No].Lift_Height, FW_Angle, BW_Angle, Main.m_stAGV[LGV_No].LGV_Way, Main.m_stAGV[LGV_No].FLAG_Middle_Battery_LGV, Main.m_stAGV[LGV_No].Error_1, Main.m_stAGV[LGV_No].Error_2, Main.m_stAGV[LGV_No].Error_3);
                    Main.Log("AGV_0" + (LGV_No + 1) + "---- AGV_Info", log);
                }));
            }
            catch (Exception Ex)
            {
                Main.Log("TryCatch LGV_Data", Convert.ToString(Ex));
            }
        }

        // LGV READ 데이터
        public void Shutter_Data(int Shutter_No)
        {
            string Shutter_Name = "";
            string log = "";

            try
            {
                Main.CS_Shutter_Info[Shutter_No].InPut_Open_Sensor = Convert.ToInt32((ShutterReadData[Shutter_No, 0] & 0xff));
                Main.CS_Shutter_Info[Shutter_No].InPut_Open_Sensor |= Convert.ToInt32(((ShutterReadData[Shutter_No, 1] & 0xff) << 8));

                Main.CS_Shutter_Info[Shutter_No].InPut_Close_Sensor = Convert.ToInt32((ShutterReadData[Shutter_No, 2] & 0xff));
                Main.CS_Shutter_Info[Shutter_No].InPut_Close_Sensor |= Convert.ToInt32(((ShutterReadData[Shutter_No, 3] & 0xff) << 8));

                Main.CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor = Convert.ToInt32((ShutterReadData[Shutter_No, 4] & 0xff));
                Main.CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor |= Convert.ToInt32(((ShutterReadData[Shutter_No, 5] & 0xff) << 8));

                Main.CS_Shutter_Info[Shutter_No].OutPut_Close_Sensor = Convert.ToInt32((ShutterReadData[Shutter_No, 6] & 0xff));
                Main.CS_Shutter_Info[Shutter_No].OutPut_Close_Sensor |= Convert.ToInt32(((ShutterReadData[Shutter_No, 7] & 0xff) << 8));

                //HeartBit 추가. lkw20190124
                Main.CS_Shutter_Info[Shutter_No].HeartBit = Convert.ToInt32((ShutterReadData[Shutter_No, 8] & 0xff));
                Main.CS_Shutter_Info[Shutter_No].HeartBit |= Convert.ToInt32(((ShutterReadData[Shutter_No, 9] & 0xff) << 8));


                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    Shutter_Name = Main.Find_Shutter_Name(Shutter_No);

                    log = string.Format("입고대 열림 센서 = {0},\t입고대 닫힘 센서 = {1},\t출고대 열림 센서 = {2},\t출고대 닫힘 센서 = {3} ,\t하트비트 = {4}",
                                                Main.CS_Shutter_Info[Shutter_No].InPut_Open_Sensor, Main.CS_Shutter_Info[Shutter_No].InPut_Close_Sensor,
                                                Main.CS_Shutter_Info[Shutter_No].OutPut_Open_Sensor, Main.CS_Shutter_Info[Shutter_No].OutPut_Close_Sensor,
                                                Main.CS_Shutter_Info[Shutter_No].HeartBit);

                    Main.Log(Shutter_Name + "셔터 정보" , log);

                    //HeartBit 추가. lkw20190124
                    if (Main.CS_Shutter_Info[Shutter_No].HeartBit == 0)
                    {
                        Main.CS_Connect_Shutter.DataSendRequest_HeartBit(Shutter_No, 1);
                     
                    }
                    else
                    {
                        Main.CS_Connect_Shutter.DataSendRequest_HeartBit(Shutter_No, 0);

                    }
                }));
            }
            catch(Exception Ex)
            {
                Main.Log("TryCatch Shutter_Data", Convert.ToString(Ex));
            }
            
            
        }
        //명령 RETRY
        public void Command_Retry(int idx)
        {
            //Main.Log("AGV_0" + (7) + "---- LGV_WorkSchedule", "RETRY");
            if (Main.m_stAGV[idx].dqGoal.Count > 0 && (Main.m_stAGV[idx].state == 0 || Main.m_stAGV[idx].state == 4
                || Main.m_stAGV[idx].state == 5 || Main.m_stAGV[idx].state == 6 || Main.m_stAGV[idx].state == 8
                || Main.m_stAGV[idx].state == 9 || Main.m_stAGV[idx].state == 10))
            {
                //Main.Log("AGV_0" + (7) + "---- LGV_WorkSchedule", "RETRY GO");
                LGV_SendData(idx);
            }
        }

        // LGV SEND 데이터 ---차량에 명령 보내기
        public void LGV_SendData(int idx)
        {
            try
            {
                string TempLog = "";
                string OutLog = "";
                string Work_Type = "";

                //모드가 자동이고 상태가 대기,도착,적재완료,이재완료 일때만 작업 전송
                if (Main.m_stAGV[idx].mode == 1 && Main.Flag_MCS_Auto == 1
                        && (Main.m_stAGV[idx].state == 0 || Main.m_stAGV[idx].state == 4 || Main.m_stAGV[idx].state == 5 
                        || Main.m_stAGV[idx].state == 6 || Main.m_stAGV[idx].state == 8 || Main.m_stAGV[idx].state == 9 || Main.m_stAGV[idx].state == 10)
                        && Main.m_stAGV[idx].dqGoal.Count != 0 && Main.m_stAGV[idx].dqWork.Count != 0)
                {
                    Main.m_stAGV[idx].SendData = 1;
                    LGVSendData[0] = Convert.ToByte(Main.m_stAGV[idx].SendData);
                    LGVSendData[1] = Convert.ToInt16(Main.m_stAGV[idx].dqGoal[0]);
                    LGVSendData[2] = Convert.ToByte(Main.m_stAGV[idx].dqWork[0]);

                    Main.CS_Connect_LGV.DataSendRequest(idx);
                    Main.m_stAGV[idx].Working = 1;
                }
                Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                    {
                        if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE)
                        {
                            Work_Type = "이동";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD)
                        {
                            Work_Type = "적재";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD)
                        {
                            Work_Type = "이재";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD)
                        {
                            Work_Type = "이동 적재";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD)
                        {
                            Work_Type = "이동 이재";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD_2F)
                        {
                            Work_Type = "적재 이동(2층)";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD_2F)
                        {
                            Work_Type = "이재 이동(2층)";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD_2F)
                        {
                            Work_Type = "적재(2층)";
                        }
                        else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD_2F)
                        {
                            Work_Type = "이재(2층)";
                        }

                        TempLog = string.Format("\r\n{0}-{1}", Main.m_stAGV[idx].dqGoal[i], Work_Type);
                        OutLog += TempLog;

                        //로그 출력
                        Main.TB_Schedule[idx].Clear();
                        Main.TB_Schedule[idx].Text = OutLog;
                        Main.Log("AGV_0" + (idx + 1) + "---- LGV_SendData", OutLog);

                        Main.GB_LGV_Schedule[idx].Text = "스케줄(" + Main.m_stAGV[idx].dqGoal.Count + ")개";

                    }
                }));

            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_AGV_LGV_SendData", Convert.ToString(ex));
            }
        }
        //LGV SEND 데이터(트래픽) - 차량에 명령 보내기
        public void LGV_SendData_Traffic(int idx, int Type)
        {
            if ((Main.m_stAGV[idx].state == 1 || Main.m_stAGV[idx].state == 7))
            {
                LGVSendData_Traffic[0] = Convert.ToByte(Type);
                Main.CS_Connect_LGV.DataSendRequest_Traffic(idx);
            }
        }
        //작업 종료 - 타이머
        public void WorkStepEnd(int idx)
        {
            string OutLog = "";
            string TempLog = "";
            double Battey;
            string Work_Type = "";
            string dqGoal;
            string dqWork;
            string Command_ID = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            //simulator Test Value//
            string[] M_S_CarrID_Test = new string[10];

            string[] Source_Test_Logic_1 = new string[Form1.LGV_NUM];
            string[] Source_Test_Logic_2 = new string[Form1.LGV_NUM];
            string[] Source_Test_Logic_3 = new string[Form1.LGV_NUM];
            string[] Source_Test_Logic_4 = new string[Form1.LGV_NUM];
            //simulator Test//

            //Main.T_Work_End[idx].Enabled = false;
            try
            {
                End_Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                Battey = Math.Truncate((((double)Main.m_stAGV[idx].Battery - 2350) / 430) * 100);

                if (Battey > 100)
                {
                    Battey = 100;
                }
                else if (Battey < 0)
                {
                    Battey = 0;
                }

                //명령이 주행이고 차량 상태가 도착 이고 목적지와 차량 현재 위치가 같을때
                if ((Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == MOVE || Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == MOVE_LOAD ||
                     Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == MOVE_UNLOAD || Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == MOVE_LOAD_2F
                  || Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == MOVE_UNLOAD_2F)
                    && Main.m_stAGV[idx].state == 4 && (Main.m_stAGV[idx].current == Convert.ToInt32(Main.m_stAGV[idx].dqGoal[0])))
                {
                    if (Main.m_stAGV[idx].Dest_Port_Num != "")
                    {
                        if (Main.m_stAGV[idx].current == Convert.ToInt32(Main.m_stAGV[idx].Dest_Port_Num))
                        {
                            //작업 상태 바꿔주기 - 중단 불가능
                            Main.CS_Work_DB.Update_Command_State_AbortX(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, "6", idx);

                            //이재장소 도착 시간 저장
                            Main.CS_Work_DB.Update_Unload_Move_Time(idx, End_Time, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        }

                    }

                    if (Main.m_stAGV[idx].Source_Port_Num != "")
                    {
                        if (Main.m_stAGV[idx].current == Convert.ToInt32(Main.m_stAGV[idx].Source_Port_Num))
                        {
                            //적재장소 도착 시간 저장
                            Main.CS_Work_DB.Update_Load_Move_Time(idx, End_Time, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                        }

                    }
                    Main.m_stAGV[idx].dqGoal.Remove(Main.m_stAGV[idx].dqGoal[0]);
                    Main.m_stAGV[idx].dqWork.Remove(Main.m_stAGV[idx].dqWork[0]);
                    LGV_SendData(idx);

                    #region 로그 출력
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                        {
                            dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                            dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);

                            if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE)
                            {
                                Work_Type = "이동";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD)
                            {
                                Work_Type = "적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD)
                            {
                                Work_Type = "이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD)
                            {
                                Work_Type = "이동 적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD)
                            {
                                Work_Type = "이동 이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD_2F)
                            {
                                Work_Type = "적재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD_2F)
                            {
                                Work_Type = "이재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD_2F)
                            {
                                Work_Type = "적재(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD_2F)
                            {
                                Work_Type = "이재(2층)";
                            }

                            TempLog = string.Format("\r\n{0}-{1}", Main.m_stAGV[idx].dqGoal[i], Work_Type);
                            OutLog += TempLog;
                        }
                        Main.Log("AGV_0" + (idx + 1) + "---- LGV_WorkSchedule", OutLog);

                        Main.TB_Schedule[idx].Clear();
                        Main.TB_Schedule[idx].Text = OutLog;

                        #endregion
                        //남은 작업이 0일때 완료 처리
                        if (Main.m_stAGV[idx].dqGoal.Count == 0)
                        {
                            if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                            {
                            
                                Main.CS_Work_DB.Update_Command_Info_End(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, "2", idx, End_Time);
                                
                                //작업시간 업데이트
                                Main.CS_Work_DB.Select_Result_Total_Time(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                                Main.CS_Work_DB.Select_MCS_Command_Info_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                                Main.CS_Work_DB.Delete_Work_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                                //엑셀에 작업 로그 저장
                                Main.CS_Work_DB.Insert_Excel_Data_Log(E_Call_Time, E_Command_ID, E_Carrier_ID, E_Source_Port, E_Dest_Port, E_LGV_No, E_Alloc_Time, E_Load_Move_Time,
                                    E_Load_Time, E_UnLoad_Move_Time, E_UnLoad_Time, "완료", E_Load_Move_Time_Result, E_Load_Time_Result, E_UnLoad_Move_Time_Result,
                                    E_UnLoad_Time_Result, E_Total_Time_Result, E_Complete_Time, Alloc_Current[idx]);
                            }

                            //작업 종료 업데이트                  
                            Main.m_stAGV[idx].Working = 0;
                            Main.TB_Schedule[idx].Clear();
                            Init_AGVInfo(idx);

                            //if (Main.m_stAGV[idx].current != 67 && Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438
                            if (Main.m_stAGV[idx].current != 163 && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438 //20200826 충전기 이설 수정
                            && Main.m_stAGV[idx].current != 1237 && Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 438
                            && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 8 && Main.m_stAGV[idx].current != 1012
                            && Main.m_stAGV[idx].current != 1131 && Main.m_stAGV[idx].current != 1097 && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 277
                            && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1178 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 322
                            && Main.m_stAGV[idx].current != 665 && Main.m_stAGV[idx].current != 442) //&& Main.m_stAGV[idx].current != 1369//&& Main.m_stAGV[idx].current != 276 && Main.m_stAGV[idx].current != 1556)
                            //20200820 1369, 442, 276, 1556 충전소 / 대기 장소 조건 추가
                            {
                                Main.m_stAGV[idx].Flag_Wait_Station = 1;
                            }

                            Main.CS_Work_DB.DELETE_DB_Recovery(idx);
                            //Select_MCS_Command_Info
                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        }
                    }));
                    //회복 데이터 넣어주기

                    Main.CS_Work_DB.DELETE_DB_Recovery(idx);
                    for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                    {
                        dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                        dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);
                        Main.CS_Work_DB.Insert_DB_Recovery(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, dqGoal, dqWork);
                    }

                }
                //명령이 적재이고 적재완료, 목적지와 차량 현재 위치가 같을때
                else if ((Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == LOAD || Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == LOAD_2F)
                       && Main.m_stAGV[idx].state == 5 && Main.m_stAGV[idx].current == Convert.ToInt32(Main.m_stAGV[idx].dqGoal[0]))
                {
                    Main.m_stAGV[idx].dqGoal.Remove(Main.m_stAGV[idx].dqGoal[0]);
                    Main.m_stAGV[idx].dqWork.Remove(Main.m_stAGV[idx].dqWork[0]);
                    LGV_SendData(idx);

                    //적재완료일때 시간 저장
                    Main.CS_Work_DB.Update_Load_Time(idx, End_Time, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                    #region 로그 출력
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                        {
                            dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                            dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);

                            if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE)
                            {
                                Work_Type = "이동";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD)
                            {
                                Work_Type = "적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD)
                            {
                                Work_Type = "이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD)
                            {
                                Work_Type = "이동 적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD)
                            {
                                Work_Type = "이동 이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD_2F)
                            {
                                Work_Type = "적재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD_2F)
                            {
                                Work_Type = "이재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD_2F)
                            {
                                Work_Type = "적재(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD_2F)
                            {
                                Work_Type = "이재(2층)";
                            }

                            TempLog = string.Format("\r\n{0}-{1}", Main.m_stAGV[idx].dqGoal[i], Work_Type);
                            OutLog += TempLog;
                        }
                        Main.Log("AGV_0" + (idx + 1) + "---- LGV_WorkSchedule", OutLog);

                        Main.TB_Schedule[idx].Clear();
                        Main.TB_Schedule[idx].Text = OutLog;


                        #endregion
                        //남은 작업이 0일때 완료 처리
                        if (Main.m_stAGV[idx].dqGoal.Count == 0)
                        {
                            Main.CS_Work_DB.Update_Command_Info_End(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, "2", idx, End_Time);

                            //작업시간 업데이트
                            Main.CS_Work_DB.Select_Result_Total_Time(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                            Main.CS_Work_DB.Delete_Work_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            if (Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                            {
                                //엑셀에 작업 로그 저장
                                Main.CS_Work_DB.Insert_Excel_Data_Log(E_Call_Time, E_Command_ID, E_Carrier_ID, E_Source_Port, E_Dest_Port, E_LGV_No, E_Alloc_Time, E_Load_Move_Time,
                                    E_Load_Time, E_UnLoad_Move_Time, E_UnLoad_Time, "완료", E_Load_Move_Time_Result, E_Load_Time_Result, E_UnLoad_Move_Time_Result,
                                    E_UnLoad_Time_Result, E_Total_Time_Result, E_Complete_Time, Alloc_Current[idx]);
                            }

                            //작업 종료 업데이트                  
                            Main.m_stAGV[idx].Working = 0;
                            Main.TB_Schedule[idx].Clear();
                            Init_AGVInfo(idx);


                            //if (Main.m_stAGV[idx].current != 67 && Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438
                            if (Main.m_stAGV[idx].current != 163 && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438 //20200826 충전기 이설 수정
                            && Main.m_stAGV[idx].current != 1237 && Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 438
                            && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 8 && Main.m_stAGV[idx].current != 1012
                            && Main.m_stAGV[idx].current != 1131 && Main.m_stAGV[idx].current != 1097 && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 277
                            && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1178 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 322
                            && Main.m_stAGV[idx].current != 665 && Main.m_stAGV[idx].current != 442) //&& Main.m_stAGV[idx].current != 1369 && Main.m_stAGV[idx].current != 276 && Main.m_stAGV[idx].current != 1556)
                            //20200820 1369, 442, 276, 1556 충전소 / 대기 장소 조건 추가
                            {
                                Main.m_stAGV[idx].Flag_Wait_Station = 1;
                            }

                            Main.CS_Work_DB.DELETE_DB_Recovery(idx);
                            //Select_MCS_Command_Info
                            //Main.CS_Work_DB.Select_MCS_Command_Info_Main();
                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        }

                    }));

                    //회복 데이터 넣어주기
                    Main.CS_Work_DB.DELETE_DB_Recovery(idx);
                    for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                    {
                        dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                        dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);
                        Main.CS_Work_DB.Insert_DB_Recovery(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, dqGoal, dqWork);
                    }

                }
                //명령이 이재 이고 이재완료, 목적지와 차량 현재 위치가 같을때
                else if ((Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == UNLOAD || Convert.ToInt32(Main.m_stAGV[idx].dqWork[0]) == UNLOAD_2F)
                      && Main.m_stAGV[idx].state == 6 && Main.m_stAGV[idx].current == Convert.ToInt32(Main.m_stAGV[idx].dqGoal[0]))
                {
                    Main.m_stAGV[idx].dqGoal.Remove(Main.m_stAGV[idx].dqGoal[0]);
                    Main.m_stAGV[idx].dqWork.Remove(Main.m_stAGV[idx].dqWork[0]);
                    LGV_SendData(idx);

                    //이재완료일때 시간 저장
                    Main.CS_Work_DB.Update_Unload_Time(idx, End_Time, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                    //작업시간 업데이트
                    Main.CS_Work_DB.Select_Result_Work_Time(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                    Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                    Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                    Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                    #region 로그 출력
                    Main.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                        {
                            dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                            dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);

                            if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE)
                            {
                                Work_Type = "이동";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD)
                            {
                                Work_Type = "적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD)
                            {
                                Work_Type = "이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD)
                            {
                                Work_Type = "이동 적재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD)
                            {
                                Work_Type = "이동 이재";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_LOAD_2F)
                            {
                                Work_Type = "적재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == MOVE_UNLOAD_2F)
                            {
                                Work_Type = "이재 이동(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == LOAD_2F)
                            {
                                Work_Type = "적재(2층)";
                            }
                            else if (Convert.ToInt32(Main.m_stAGV[idx].dqWork[i]) == UNLOAD_2F)
                            {
                                Work_Type = "이재(2층)";
                            }

                            TempLog = string.Format("\r\n{0}-{1}", Main.m_stAGV[idx].dqGoal[i], Work_Type);
                            OutLog += TempLog;
                        }
                        Main.Log("AGV_0" + (idx + 1) + "---- LGV_WorkSchedule", OutLog);

                        Main.TB_Schedule[idx].Clear();
                        Main.TB_Schedule[idx].Text = OutLog;

                        #endregion
                        //남은 작업이 0일때 완료 처리
                        if (Main.m_stAGV[idx].dqGoal.Count == 0)
                        {
                            Main.CS_Work_DB.Update_Command_Info_End(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, "2", idx, End_Time);

                            //작업시간 업데이트
                            Main.CS_Work_DB.Select_Result_Total_Time(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                            Main.CS_Work_DB.Delete_Work_Log(Main.m_stAGV[idx].MCS_Vehicle_Command_ID);

                            if(Main.m_stAGV[idx].MCS_Vehicle_Command_ID != "")
                            {
                                //엑셀에 작업 로그 저장
                                Main.CS_Work_DB.Insert_Excel_Data_Log(E_Call_Time, E_Command_ID, E_Carrier_ID, E_Source_Port, E_Dest_Port, E_LGV_No, E_Alloc_Time, E_Load_Move_Time,
                                    E_Load_Time, E_UnLoad_Move_Time, E_UnLoad_Time, "완료", E_Load_Move_Time_Result, E_Load_Time_Result, E_UnLoad_Move_Time_Result,
                                    E_UnLoad_Time_Result, E_Total_Time_Result, E_Complete_Time, Alloc_Current[idx]);
                            }
                            

                            //작업 종료 업데이트                  
                            Main.m_stAGV[idx].Working = 0;
                            Main.TB_Schedule[idx].Clear();
                            Init_AGVInfo(idx);

                            //if (Main.m_stAGV[idx].current != 67 && Main.m_stAGV[idx].current != 44 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438
                            if (Main.m_stAGV[idx].current != 163 && Main.m_stAGV[idx].current != 166 && Main.m_stAGV[idx].current != 1041 && Main.m_stAGV[idx].current != 1438 //20200826 충전기 이설 수정
                            && Main.m_stAGV[idx].current != 1237 && Main.m_stAGV[idx].current != 499 && Main.m_stAGV[idx].current != 1499 && Main.m_stAGV[idx].current != 438
                            && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 8 && Main.m_stAGV[idx].current != 1012
                            && Main.m_stAGV[idx].current != 1131 && Main.m_stAGV[idx].current != 1097 && Main.m_stAGV[idx].current != 173 && Main.m_stAGV[idx].current != 277
                            && Main.m_stAGV[idx].current != 496 && Main.m_stAGV[idx].current != 1178 && Main.m_stAGV[idx].current != 1496 && Main.m_stAGV[idx].current != 322
                            && Main.m_stAGV[idx].current != 665 && Main.m_stAGV[idx].current != 442) // && Main.m_stAGV[idx].current != 1369 && Main.m_stAGV[idx].current != 276 && Main.m_stAGV[idx].current != 1556)
                            //20200820 1369, 442, 276, 1556 충전소 / 대기 장소 조건 추가
                            {
                                Main.m_stAGV[idx].Flag_Wait_Station = 1;
                            }

                            Main.CS_Work_DB.Select_MCS_Command_Info(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Main(idx);
                            Main.CS_Work_DB.Select_MCS_Command_Info_Send_MCS();
                        }
                    }));

                    //회복 데이터 넣어주기
                    Main.CS_Work_DB.DELETE_DB_Recovery(idx);
                    for (int i = 0; i < Main.m_stAGV[idx].dqGoal.Count; i++)
                    {
                        dqGoal = Convert.ToString(Main.m_stAGV[idx].dqGoal[i]);
                        dqWork = Convert.ToString(Main.m_stAGV[idx].dqWork[i]);
                        Main.CS_Work_DB.Insert_DB_Recovery(idx, Main.m_stAGV[idx].MCS_Vehicle_Command_ID, dqGoal, dqWork);
                    }

                    string[] Test_Type = new string[Form1.LGV_NUM];
                    
                }
            }
            catch (Exception ex)
            {
                Main.Log("Try Catch_Work_End", Convert.ToString(ex));
            }
            finally
            {
                //Main.T_Work_End[idx].Enabled = true;
            }

        }
        public void Init_AGVInfo(int idx)
        {
            Main.m_stAGV[idx].MCS_Source_Port = "";
            Main.m_stAGV[idx].MCS_Dest_Port = "";
            Main.m_stAGV[idx].Source_Port_Num = "";
            Main.m_stAGV[idx].Dest_Port_Num = "";
            Main.m_stAGV[idx].MCS_Vehicle_Command_ID = "";
            Main.m_stAGV[idx].MCS_Vehicle_Priority = "";
        }
    }
}
