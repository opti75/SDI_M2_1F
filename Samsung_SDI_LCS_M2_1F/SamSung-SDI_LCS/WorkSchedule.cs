using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class WorkSchedule
    {
        Form1 Main;

        public int FLAG_Check_WorkType = 0;
        //출발지 타입 확인
        public int Check_WorkType_Source = 0;
        //도착지 타입 확인
        public int Check_WorkType_Dest = 0;

        //양극 광폭 입출고대 작업 확인
        public int Check_Plus_Big_Size_Input = 0; // 광폭 코터 입고대 작업 하는지 확인. lkw20190415

        //기재 반입구 
        public int FLAG_Check_Entry_LGV_Minus_Roll = 0; //음극 롤 차량 확인 - 기재 -> 음극 롤창고 작업 하는지 확인
        public int FLAG_Check_Entry_LGV_Plus_Roll = 0; //양극 롤 차량 확인 - 기재 -> 음극 롤창고 작업 하는지 확인
        public int FLAG_Check_Entry_LGV = 0; //기재 반입구, 양극 기재 렉 창고 -> 한차가 작업중 일때 작업 할당 금지
        public int FLAG_Check_Entry_LGV_Roll = 0; //기재 반입구, 양극 기재 렉 창고 -> 한차가 작업중 일때 작업 할당 금지

        public int Alloc_Entry_Minus_Roll = 0;
        public int Alloc_Entry_Plus_Roll = 0;
        public int Alloc_Entry_LGV = 0;

        //대기 장소 우선순위 작업 할당
        public int Check_Charge_Station_Minus = 0;
        public int Check_163_Node_Minus = 0;
        public int Check_166_Node_Minus = 0;

        public int Alloc_AGV_Charge = 0;
        public int Alloc_AGV_163 = 0;
        public int Alloc_AGV_166 = 0;

        public int Check_Charge_Station_Plus = 0;
        public int Check_1237_Node = 0;
        public int Check_1041_Node = 0;

        public int Alloc_AGV_Charge_Plus = 0;
        public int Alloc_AGV_1237 = 0;
        public int Alloc_AGV_1041 = 0;

        public int Check_Node_1286_AGV = 0;
        public int Check_Node_1313_AGV = 0;
        public int Check_Node_1268_AGV = 0;
        public int Check_Node_1054_AGV = 0;

        public int Alloc_Plus_Work_Node = 0;

        //릴타입 충전 장소
        public int Check_Reel_Charge_Station_1438 = 0;
        public int Check_Reel_Charge_Station_438 = 0;
        public int Check_Reel_Charge_Station_496 = 0;
        public int Check_Reel_Charge_Station_442 = 0; // 20200820 충전소 추가 설치 위치
        public int Check_Reel_Charge_Station_1556 = 0; // 20200820 충전소 추가 설치 위치

        public int Alloc_Reel_Charge_Station_1438 = 0;
        public int Alloc_Reel_Charge_Station_438 = 0;
        public int Alloc_Reel_Charge_Station_496 = 0;
        public int Alloc_Reel_Charge_Station_442 = 0;  // 20200820 충전소 추가 설치 위치
        public int Alloc_Reel_Charge_Station_1556 = 0; // 20200820 충전소 추가 설치 위치


        //오버브릿지 리프트 작업할당 플래그
        public int Check_Over_Lift_Order = 0;
        public int Alloc_Over_Lift_Order = 0;

        public int FLAG_Lift_Order = 0; //오버 상승 가는 차량 있는지 확인
        public int FLAG_Alloc_Lift_Order = 0;

        //상승 만재 일때 상승 명령 팅구기
        public int Check_UP_Lift_Carrier = 0;
        public int Alloc_UP_Lift_Carrier = 0;
        public int Check_Work_UpLift = 0;

        //11.07 포장실 작업할당 막기
        public int FLAG_Check_PackingRoom_Order = 0;
        public int Alloc_PackingRoom_Order = 0;

        //11.14 3층 하강 리프트 확인
        public int Check_Floor3_Down_Lift = 0;
        public int Alloc_Floor3_Down_Lift = 0;

        //11.26양극 3식 슬리터 차량 확인
        public int Check_Plus_SLT3 = 0;
        public int Alloc_Plus_SLT3 = 0;

        //12.07 양극 1식 슬리터 차량 확인
        public int Check_PLUS_SLT_1 = 0;
        public int Alloc_PLUS_SLT_1 = 0;

        //12.10 음극 2,3슬리터 차량 확인
        public int Check_23SLT_LGV = 0;
        public int Alloc_23SLT_LGV = 0;

        //01.08 음극 음극 3슬리터 차량 확인
        public int Check_3SLT_LGV = 0;
        public int Alloc_3SLT_LGV = 0;

        //12.10 1156에선 작업 안받기
        public int Alloc_1156_NoWork = 0;
        //20200903 1369 위치에서 M동 하강 리프트 이외 작업 할당하지 않음
        public int Alloc_1369_NoWork = 0;

        //12.18 M동 상승리프트 구간 차량 확인
        public int Check_3_Up_Lift = 0;
        public int Alloc_3_Up_Lift = 0;

        //11.12 3층 상승 리프트 확인
        public int Check_Floor3_Up_Lift_LGV = 0;
        public int Check_Floor3_Up_Lift = 0;
        public int Alloc_Floor3_Up_Lift = 0;

        //12.26 오버브릿지 버퍼 팅구기
        public int Check_Over_Buffer_AGV = 0;
        public int Alloc_Over_Buffer_Order = 0;

        //--------------------------------------------------후공정 작업할당
        //음/양 슬리터 명령 있는지 확인
        public int Check_Plus_SLT1_Order = 0;
        public int Check_Plus_SLT2_Order = 0;
        public int Check_Plus_SLT3_Order = 0;

        public int Check_Minus_SLT1_Order = 0;
        public int Check_Minus_SLT2_Order = 0;
        public int Check_Minus_SLT3_Order = 0;

        //음/양 롤 리와인더 명령 있는지 확인. lkw20190617
        public int Check_Plus_RRW1_Order = 0;
        public int Check_Plus_RRW2_Order = 0;

        public int Check_Minus_RRW1_Order = 0;
        public int Check_Minus_RRW2_Order = 0;

        //M동 하강리프트 명령 있는지 확인
        public int Check_M_Down_Lift_Order = 0;

        //음/양 공급 작업 있는지 학인
        public int Check_Plus_SLT1_Working_LGV = 0;
        public int Check_Plus_SLT2_Working_LGV = 0;
        public int Check_Plus_SLT3_Working_LGV = 0;

        public int Check_Minus_SLT1_Working_LGV = 0;
        public int Check_Minus_SLT2_Working_LGV = 0;
        public int Check_Minus_SLT3_Working_LGV = 0;

        //ROLL 리와인더 음/양 공급 작업 있는지 학인. lkw20190617
        public int Check_Plus_RRW1_Working_LGV = 0;
        public int Check_Plus_RRW2_Working_LGV = 0;

        public int Check_Minus_RRW1_Working_LGV = 0;
        public int Check_Minus_RRW2_Working_LGV = 0;


        //음/양 슬리터 작업할당 막기
        public int Alloc_Work_Plus_SLT1 = 0;
        public int Alloc_Work_Plus_SLT2 = 0;
        public int Alloc_Work_Plus_SLT3 = 0;

        public int Alloc_Work_Minus_SLT1 = 0;
        public int Alloc_Work_Minus_SLT2 = 0;
        public int Alloc_Work_Minus_SLT3 = 0;

        //음/양 롤 리와인더 작업할당 막기. lkw20190617 
        public int Alloc_Work_Plus_RRW1 = 0;
        public int Alloc_Work_Plus_RRW2 = 0;

        public int Alloc_Work_Minus_RRW1 = 0;
        public int Alloc_Work_Minus_RRW2 = 0;

        //------------------------------------------롤 LGV작업할당
        //롤창고 입고대 명령있는지 확인
        public int Check_Plus_Roll_InPut1_Working_LGV = 0;
        public int Check_Plus_Roll_InPut2_Working_LGV = 0;
        public int Check_Minus_Roll_InPut1_Working_LGV = 0;
        public int Check_Minus_Roll_InPut2_Working_LGV = 0;

        public int Alloc_Plus_Roll_InPut1 = 0;
        public int Alloc_Plus_Roll_InPut2 = 0;
        public int Alloc_Minus_Roll_InPut1 = 0;
        public int Alloc_Minus_Roll_InPut2 = 0;

        public int Check_Plus_Roll_OutPut1_Order = 0;
        public int Check_Plus_Roll_OutPut2_Order = 0;
        public int Check_Minus_Roll_OutPut1_Order = 0;
        public int Check_Minus_Roll_OutPut2_Order = 0;

        //음/양 롤타입 충전소 영역에 차있는지 확인 - 차있으면 할당 금지(충전소에서)
        public int Check_Charge_Station_M_Roll = 0;
        public int Alloc_Charge_Station_M_Roll = 0;

        public int Check_Charge_Station_P_Roll = 0;
        public int Alloc_Charge_Station_P_Roll = 0;


        public int Alloc_7SLT = 0;
        public void Init_FLAG()
        {
            #region 플래그 초기화
            //타입확인
            Alloc_7SLT = 0;
            FLAG_Check_WorkType = 0;
            Check_WorkType_Source = 0;
            Check_WorkType_Dest = 0;

            //양극 광폭 코터 확인.
            Check_Plus_Big_Size_Input = 0; // 광폭 코터 입고대 작업 하는지 확인. lkw20190415

            //기재반입구
            FLAG_Check_Entry_LGV_Minus_Roll = 0; //음극 롤 차량 확인 - 기재 -> 음극 롤창고 작업 하는지 확인
            FLAG_Check_Entry_LGV_Plus_Roll = 0; //양극 롤 차량 확인 - 기재 -> 음극 롤창고 작업 하는지 확인
            FLAG_Check_Entry_LGV = 0; //기재 반입구, 양극 기재 렉 창고 -> 한차가 작업중 일때 작업 할당 금지
            FLAG_Check_Entry_LGV_Roll = 0;

            Alloc_Entry_Minus_Roll = 0;
            Alloc_Entry_Plus_Roll = 0;
            Alloc_Entry_LGV = 0;

            //롤타입 대기장소
            Check_Charge_Station_Minus = 0;
            Check_163_Node_Minus = 0;
            Check_166_Node_Minus = 0;
            Alloc_AGV_Charge = 0;
            Alloc_AGV_163 = 0;
            Alloc_AGV_166 = 0;

            Check_Charge_Station_Plus = 0;
            Check_1237_Node = 0;
            Check_1041_Node = 0;
            Alloc_AGV_Charge_Plus = 0;
            Alloc_AGV_1237 = 0;
            Alloc_AGV_1041 = 0;

            //릴타입 충전 장소
            Check_Reel_Charge_Station_1438 = 0;
            Check_Reel_Charge_Station_438 = 0;
            Check_Reel_Charge_Station_496 = 0;
            Check_Reel_Charge_Station_1556 = 0;
            Check_Reel_Charge_Station_442 = 0;

            Alloc_Reel_Charge_Station_1438 = 0;
            Alloc_Reel_Charge_Station_438 = 0;
            Alloc_Reel_Charge_Station_496 = 0;
            Alloc_Reel_Charge_Station_1556 = 0;
            Alloc_Reel_Charge_Station_442 = 0;

            //오버브릿지 리프트 작업할당 플래그
            Check_Over_Lift_Order = 0;
            Alloc_Over_Lift_Order = 0;

            FLAG_Lift_Order = 0;
            FLAG_Alloc_Lift_Order = 0;

            //상승 만재 일때 상승 명령 팅구기
            Check_UP_Lift_Carrier = 0;
            Alloc_UP_Lift_Carrier = 0;
            Check_Work_UpLift = 0;

            //11.07 포장실 작업할당 막기
            FLAG_Check_PackingRoom_Order = 0;
            Alloc_PackingRoom_Order = 0;

            //11.14 3층 하강 리프트 확인
            Check_Floor3_Down_Lift = 0;
            Alloc_Floor3_Down_Lift = 0;

            //11.26양극 3식 슬리터 차량 확인
            Check_Plus_SLT3 = 0;
            Alloc_Plus_SLT3 = 0;

            //12.07 양극 1식 슬리터 차량 확인
            Check_PLUS_SLT_1 = 0;
            Alloc_PLUS_SLT_1 = 0;

            //12.10 음극 2,3슬리터 차량 확인
            Check_23SLT_LGV = 0;
            Alloc_23SLT_LGV = 0;

            //01.08 음극 음극 3슬리터 차량 확인
            Check_3SLT_LGV = 0;
            Alloc_3SLT_LGV = 0;

            //12.10 1156에선 작업 안받기
            Alloc_1156_NoWork = 0;
            //20200903 1369 위치에서 M동 하강 리프트 이외 작업 할당하지 않음
            Alloc_1369_NoWork = 0;

            //12.18 양극 상승리프트 구간 차량 확인
            Check_3_Up_Lift = 0;
            Alloc_3_Up_Lift = 0;

            //11.12 3층 상승 리프트 확인
            Check_Floor3_Up_Lift_LGV = 0;
            Check_Floor3_Up_Lift = 0;
            Alloc_Floor3_Up_Lift = 0;

            //12.26 오버브릿지 버퍼 팅구기
            Check_Over_Buffer_AGV = 0;
            Alloc_Over_Buffer_Order = 0;

            //--------------------------------------------------후공정 작업할당
            //음/양 슬리터 명령 있는지 확인
            Check_Plus_SLT1_Order = 0;
            Check_Plus_SLT2_Order = 0;
            Check_Plus_SLT3_Order = 0;

            Check_Minus_SLT1_Order = 0;
            Check_Minus_SLT2_Order = 0;
            Check_Minus_SLT3_Order = 0;

            //음/양 롤 리와인더 명령 있는지 확인. lkw20190617
            Check_Plus_RRW1_Order = 0;
            Check_Plus_RRW2_Order = 0;

            Check_Minus_RRW1_Order = 0;
            Check_Minus_RRW2_Order = 0;

            //M동 하강리프트 명령 있는지 확인
            Check_M_Down_Lift_Order = 0;


            //음/양 공급 작업 있는지 학인
            Check_Plus_SLT1_Working_LGV = 0;
            Check_Plus_SLT2_Working_LGV = 0;
            Check_Plus_SLT3_Working_LGV = 0;

            Check_Minus_SLT1_Working_LGV = 0;
            Check_Minus_SLT2_Working_LGV = 0;
            Check_Minus_SLT3_Working_LGV = 0;

            //ROLL 리와인더 음/양 공급 작업 있는지 학인. lkw20190617
            Check_Plus_RRW1_Working_LGV = 0;
            Check_Plus_RRW2_Working_LGV = 0;

            Check_Minus_RRW1_Working_LGV = 0;
            Check_Minus_RRW2_Working_LGV = 0;

            //음/양 슬리터 작업할당 막기
            Alloc_Work_Plus_SLT1 = 0;
            Alloc_Work_Plus_SLT2 = 0;
            Alloc_Work_Plus_SLT3 = 0;

            Alloc_Work_Minus_SLT1 = 0;
            Alloc_Work_Minus_SLT2 = 0;
            Alloc_Work_Minus_SLT3 = 0;

            //음/양 롤 리와인더 작업할당 막기. lkw20190617 
            Alloc_Work_Plus_RRW1 = 0;
            Alloc_Work_Plus_RRW2 = 0;

            Alloc_Work_Minus_RRW1 = 0;
            Alloc_Work_Minus_RRW2 = 0;

            //------------------------------------------롤 LGV작업할당
            //롤창고 입고대 명령있는지 확인
            Check_Plus_Roll_InPut1_Working_LGV = 0;
            Check_Plus_Roll_InPut2_Working_LGV = 0;
            Check_Minus_Roll_InPut1_Working_LGV = 0;
            Check_Minus_Roll_InPut2_Working_LGV = 0;

            Check_Plus_Roll_OutPut1_Order = 0;
            Check_Plus_Roll_OutPut2_Order = 0;
            Check_Minus_Roll_OutPut1_Order = 0;
            Check_Minus_Roll_OutPut2_Order = 0;

            Alloc_Plus_Roll_InPut1 = 0;
            Alloc_Plus_Roll_InPut2 = 0;
            Alloc_Minus_Roll_InPut1 = 0;
            Alloc_Minus_Roll_InPut2 = 0;

            Check_Node_1286_AGV = 0;
            Check_Node_1313_AGV = 0;
            Check_Node_1268_AGV = 0;
            Check_Node_1054_AGV = 0;

            Alloc_Plus_Work_Node = 0;

            //음/양 롤타입 충전소 영역에 차있는지 확인 - 차있으면 할당 금지(충전소에서)
            Check_Charge_Station_M_Roll = 0;
            Alloc_Charge_Station_M_Roll = 0;

            Check_Charge_Station_P_Roll = 0;
            Alloc_Charge_Station_P_Roll = 0;
            #endregion
        }

        // 생성자
        public WorkSchedule()
        {

        }
        public WorkSchedule(Form1 CS_Main)
        {
            Main = CS_Main;
        }
        //대기중인 명령 검색
        public void waitCommand(int LGV_No)
        {
            if (Main.CS_Work_DB.waitCommand.Count > 0)
            {
                for (int WaitCommandCount = 0; WaitCommandCount < Main.CS_Work_DB.waitCommand.Count; WaitCommandCount++)
                {
                    Init_FLAG();
                    #region 작업 우선순위 할당


                    #region 차량 위치, 명령 확인
                    for (int i = 0; i < Form1.LGV_NUM; i++)
                    {
                        if (i != LGV_No)
                        {
                            #region 자동일때만 막는 조건
                            if (Main.m_stAGV[i].mode != 0 && Main.m_stAGV[i].FLAG_LGV_Charge != 1)
                            {
                                //롤타입 음극 영역에 차량 있는지 확인
                                if ((Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 27)
                                || (Main.m_stAGV[i].current >= 401 && Main.m_stAGV[i].current <= 412)
                                || (Main.m_stAGV[i].current >= 455 && Main.m_stAGV[i].current <= 457)
                                || (Main.m_stAGV[i].current >= 87 && Main.m_stAGV[i].current <= 89))
                                {
                                    Check_Charge_Station_M_Roll = 1;
                                }

                                //롤타입 양극 영역에 차량 있는지 확인
                                if ((Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1033)
                                || (Main.m_stAGV[i].current >= 1455 && Main.m_stAGV[i].current <= 1457)
                                || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1412))
                                {
                                    Check_Charge_Station_P_Roll = 1;
                                }

                                //양극 1식 슬리터에 있는지 확인(1084에서 1식슬리터 작업하는 차량 있으면 팅구기)
                                if (Main.m_stAGV[i].MCS_Source_Port == "CC8SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CC8SLT01_UBP01"
                                || Main.m_stAGV[i].MCS_Source_Port == "CC8SLT01_UBP02" || Main.m_stAGV[i].MCS_Dest_Port == "CC8SLT01_UBP02")
                                {
                                    Check_PLUS_SLT_1 = 1;
                                }

                                //양극 상승리프트 구간 차량 있는지 확인
                                if (i == 0 || i == 1 || i == 9 || i == 11 || i == 12)
                                {
                                    if ((Main.m_stAGV[i].current >= 1106 && Main.m_stAGV[i].current <= 1112)
                                    || (Main.m_stAGV[i].current >= 1425 && Main.m_stAGV[i].current <= 1427))
                                    {
                                        Check_3_Up_Lift = 1;
                                    }
                                }

                                //오버브릿지 버퍼 명령을 받을수 있는 차가 있는지 확인
                                if (((Main.m_stAGV[i].current >= 1273 && Main.m_stAGV[i].current <= 1292)
                                || (Main.m_stAGV[i].current >= 1359 && Main.m_stAGV[i].current <= 1360)
                                || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1393)
                                || (Main.m_stAGV[i].current >= 1317 && Main.m_stAGV[i].current <= 1318)
                                || (Main.m_stAGV[i].current >= 1500 && Main.m_stAGV[i].current <= 1502))
                                && Main.m_stAGV[i].Goal == 1502 || Main.m_stAGV[i].Goal == 1393 || Main.m_stAGV[i].Goal == 1313)
                                {
                                    Check_Over_Buffer_AGV = 1;
                                }
                                //-----------------------------------------------후공정 작업 할당 개선                               
                                #region 음/양 슬리터 공급 작업하는 차량있는지 확인
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CC8SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CC8SLT01_UBP02")
                                {
                                    Check_Plus_SLT1_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CC9SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CC9SLT01_UBP02")
                                {
                                    Check_Plus_SLT2_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CC10SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CC10SLT01_UBP02")
                                {
                                    Check_Plus_SLT3_Working_LGV = 1;
                                }

                                if (Main.m_stAGV[i].MCS_Dest_Port == "CA7SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CA7SLT01_UBP02")
                                {
                                    Check_Minus_SLT1_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CA8SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CA8SLT01_UBP02")
                                {
                                    Check_Minus_SLT2_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CA9SLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CA9SLT01_UBP02")
                                {
                                    Check_Minus_SLT3_Working_LGV = 1;
                                }

                                if ((Main.m_stAGV[i].state == 0 || Main.m_stAGV[i].state == 4 || Main.m_stAGV[i].state == 5 || Main.m_stAGV[i].state == 6)
                                 && Main.m_stAGV[i].dqGoal.Count == 0)
                                {
                                    #region 음/양 슬리터 공급 작업완료한 차량 있는지 확인
                                    //양극 1식
                                    if (Main.m_stAGV[i].current >= 1091 && Main.m_stAGV[i].current <= 1095)
                                    {
                                        Check_Plus_SLT1_Working_LGV = 1;
                                    }
                                    //양극 2식
                                    if (Main.m_stAGV[i].current >= 1084 && Main.m_stAGV[i].current <= 1084)
                                    {
                                        Check_Plus_SLT2_Working_LGV = 1;
                                    }
                                    //양극 3식
                                    if (Main.m_stAGV[i].current >= 1286 && Main.m_stAGV[i].current <= 1286)
                                    {
                                        Check_Plus_SLT3_Working_LGV = 1;
                                    }

                                    //음극 1식
                                    if (Main.m_stAGV[i].current >= 113 && Main.m_stAGV[i].current <= 115)
                                    {
                                        Check_Minus_SLT1_Working_LGV = 1;
                                    }
                                    //음극 2식
                                    if (Main.m_stAGV[i].current >= 278 && Main.m_stAGV[i].current <= 280)
                                    {
                                        Check_Minus_SLT2_Working_LGV = 1;
                                    }
                                    //음극 3식
                                    if (Main.m_stAGV[i].current >= 265 && Main.m_stAGV[i].current <= 267)
                                    {
                                        Check_Minus_SLT3_Working_LGV = 1;
                                    }
                                    #endregion
                                    #region 음/양 롤창고 입고대 작업완료한 차량 있는지 확인
                                    if (Main.m_stAGV[i].current >= 1002 && Main.m_stAGV[i].current <= 1004)
                                    {
                                        Check_Plus_Roll_InPut1_Working_LGV = 1;
                                    }
                                    if (Main.m_stAGV[i].current >= 1020 && Main.m_stAGV[i].current <= 1022)
                                    {
                                        Check_Plus_Roll_InPut2_Working_LGV = 1;
                                    }
                                    if (Main.m_stAGV[i].current >= 24 && Main.m_stAGV[i].current <= 26)
                                    {
                                        Check_Minus_Roll_InPut1_Working_LGV = 1;
                                    }
                                    if (Main.m_stAGV[i].current >= 2 && Main.m_stAGV[i].current <= 4)
                                    {
                                        Check_Minus_Roll_InPut2_Working_LGV = 1;
                                    }
                                    #endregion
                                }
                                #endregion
                                //-----------------------------------------------후공정 롤 리와인드 작업 할당 개선. lkw20190617                            
                                #region 음/양 롤 리와인드 공급 작업하는 차량있는지 확인
                                // 양극
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CC6MRW01_LBP01")
                                {
                                    Check_Plus_RRW1_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CC6MRW02_LBP01")
                                {
                                    Check_Plus_RRW2_Working_LGV = 1;
                                }
                                //음극
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CA7MRW01_LBP01")
                                {
                                    Check_Minus_RRW1_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Dest_Port == "CA7MRW02_LBP01")
                                {
                                    Check_Minus_RRW2_Working_LGV = 1;
                                }

                                if ((Main.m_stAGV[i].state == 0 || Main.m_stAGV[i].state == 4 || Main.m_stAGV[i].state == 5 || Main.m_stAGV[i].state == 6)
                                 && Main.m_stAGV[i].dqGoal.Count == 0)
                                {
                                    #region 음/양 롤 리와인드 공급 작업완료한 차량 있는지 확인
                                    //양극 1식
                                    if (Main.m_stAGV[i].current == 1085)
                                    {
                                        Check_Plus_RRW1_Working_LGV = 1;
                                    }
                                    //양극 2식
                                    if (Main.m_stAGV[i].current == 1149)
                                    {
                                        Check_Plus_RRW2_Working_LGV = 1;
                                    }

                                    //음극 1식
                                    if (Main.m_stAGV[i].current == 277)
                                    {
                                        Check_Minus_RRW1_Working_LGV = 1;
                                    }
                                    //음극 2식
                                    if (Main.m_stAGV[i].current == 289)
                                    {
                                        Check_Minus_RRW2_Working_LGV = 1;
                                    }
                                    #endregion
                                }
                                #endregion
                                //------------------------------------------------롤타입 작업 할당 개선
                                #region 음/양 롤창고 입고대 하는 차량 있는지 확인
                                if (Main.m_stAGV[i].MCS_Source_Port != "CC8MIB01_BBP01" && Main.m_stAGV[i].MCS_Dest_Port == "CC8PLS01_LIP01" && Main.m_stAGV[LGV_No].current != 1002
                                    && Main.m_stAGV[LGV_No].current != 1004)
                                {
                                    Check_Plus_Roll_InPut1_Working_LGV = 1;
                                }
                                if (Main.m_stAGV[i].MCS_Source_Port != "CC8MIB01_BBP01" && Main.m_stAGV[i].MCS_Dest_Port == "CC8PLS02_LIP01" 
                                    && Main.m_stAGV[LGV_No].current != 1020 && Main.m_stAGV[LGV_No].current != 1022)
                                {
                  
                                    Check_Plus_Roll_InPut2_Working_LGV = 1;
                                }

                                if (Main.m_stAGV[i].MCS_Source_Port != "CC8MIB01_BBP01" && Main.m_stAGV[i].MCS_Dest_Port == "CA8PLS01_LIP01"
                                    && Main.m_stAGV[LGV_No].current != 24 && Main.m_stAGV[LGV_No].current != 26)
                                {
                              
                                    Check_Minus_Roll_InPut1_Working_LGV = 1;
                                }

                                if (Main.m_stAGV[i].MCS_Source_Port != "CC8MIB01_BBP01" && Main.m_stAGV[i].MCS_Dest_Port == "CA8PLS02_LIP01" 
                                    && Main.m_stAGV[LGV_No].current != 2 && Main.m_stAGV[LGV_No].current != 4)
                                {
                                    Check_Minus_Roll_InPut2_Working_LGV = 1;
                                }
                                #endregion

                            }
                            #endregion
                            //12.10 음극 2,3슬리터 차량 확인
                            if (((Main.m_stAGV[i].current >= 99 && Main.m_stAGV[i].current <= 108) && (Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 282))
                             || (Main.m_stAGV[i].current >= 262 && Main.m_stAGV[i].current <= 282)
                             || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 493)
                             || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172))
                            {
                                Check_23SLT_LGV = 1;
                            }

                            //01.08 음극 3슬리터 차량 확인
                            if ((Main.m_stAGV[i].current >= 262 && Main.m_stAGV[i].current <= 273)
                             || (Main.m_stAGV[i].current >= 488 && Main.m_stAGV[i].current <= 493)
                             || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172))
                            {
                                Check_3SLT_LGV = 1;
                            }

                            //양극 광폭 코터 할당 확인.
                            if (((Main.m_stAGV[i].current >= 1071 && Main.m_stAGV[i].current <= 1075) && Main.m_stAGV[i].Dest_Port_Num == "1466")
                             || (Main.m_stAGV[i].current >= 1222 && Main.m_stAGV[i].current <= 1232)
                             || (Main.m_stAGV[i].current >= 1322 && Main.m_stAGV[i].current <= 1331)
                             || (Main.m_stAGV[i].current >= 1464 && Main.m_stAGV[i].current <= 1466)
                             || (Main.m_stAGV[i].current >= 1470 && Main.m_stAGV[i].current <= 1472))
                            {
                                Check_Plus_Big_Size_Input = 1;
                            }


                            //기재 반입라인 차량 확인 - 음극 차량
                            if (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 186 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 151)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547))
                            
                            || (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 186 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 151)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547) && (Main.m_stAGV[i].Error == 4)))) // || (Main.m_stAGV[i].Error == 4))
                            {
                                FLAG_Check_Entry_LGV_Minus_Roll = 1;
                            }

                            //기재 반입라인 차량 확인 - 양극 차량
                            if (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1185)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547)
                            || (Main.m_stAGV[i].current >= 1494 && Main.m_stAGV[i].current <= 1495))
                            
                            || (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1185)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547)
                            || (Main.m_stAGV[i].current >= 1494 && Main.m_stAGV[i].current <= 1495) && (Main.m_stAGV[i].Error == 4))))
                            {
                                FLAG_Check_Entry_LGV_Plus_Roll = 1;
                            }

                            for (int dqGoal = 0; dqGoal < Main.m_stAGV[i].dqGoal.Count; dqGoal++)
                            {
                                if (Main.m_stAGV[i].dqGoal.Count > 0)
                                {
                                    if (i == 2 || i == 3 || i == 4)
                                    {
                                        if (Convert.ToInt32(Main.m_stAGV[i].dqGoal[dqGoal]) == 448)
                                        {
                                            FLAG_Check_Entry_LGV_Plus_Roll = 1;
                                        }
                                    }
                                }
                            }

                            //기재 반입 차량 확인 - 공통 - 할당 금지
                            if (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547))
                            
                            || (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547) && (Main.m_stAGV[i].Error == 4))))
                            {
                                FLAG_Check_Entry_LGV = 1;
                            }

                            //기재 반입 차량 확인 - 공통 - 할당 금지
                            if (((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1177 && Main.m_stAGV[i].current <= 1185)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547))

                            ||(((Main.m_stAGV[i].current >= 446 && Main.m_stAGV[i].current <= 448)
                            || (Main.m_stAGV[i].current >= 199 && Main.m_stAGV[i].current <= 221)
                            || (Main.m_stAGV[i].current >= 1177 && Main.m_stAGV[i].current <= 1185)
                            || (Main.m_stAGV[i].current >= 1191 && Main.m_stAGV[i].current <= 1221)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547) && (Main.m_stAGV[i].Error == 4)))) // && (Main.m_stAGV[i].Error == 4))
                            {
                                FLAG_Check_Entry_LGV_Roll = 1;
                            }



                            //포장실에 차 있으면 명령 팅궈잉
                            if ((Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1136)
                            || (Main.m_stAGV[i].current >= 1148 && Main.m_stAGV[i].current <= 1170)
                            || (Main.m_stAGV[i].current >= 1441 && Main.m_stAGV[i].current <= 1445))
                            {
                                FLAG_Check_PackingRoom_Order = 1;
                            }
                            //포장실 작업 있는지 확인 20200713 kjh 시작 위치 추가
                            if ((Main.m_stAGV[i].MCS_Dest_Port == "CC8MPB01_BBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CC8MPB01_BBP02")
                                || (Main.m_stAGV[i].MCS_Source_Port == "CC8MPB01_BBP01" || Main.m_stAGV[i].MCS_Source_Port == "CC8MPB01_BBP02"))
                            {
                                FLAG_Check_PackingRoom_Order = 1;
                            }
                            //오버브릿지 상승 리프트 작업 있는지 확인 (만재 조건 확인)
                            if (Main.m_stAGV[i].MCS_Dest_Port == "CA7FLF02_BBP01")
                            {
                                Check_Work_UpLift = 1;
                            }
                            //M동 상승리프트 작업있는 차량 확인 (만재 조건 확인)
                            if (Main.m_stAGV[i].MCS_Dest_Port == "CC8FLF01_BBP01")
                            {
                                Check_Floor3_Up_Lift_LGV = 1;
                            }

                            //양극 3식 슬리터에 차량 있는지 확인
                            if (Main.m_stAGV[i].MCS_Source_Port == "CCASLT01_UBP01" || Main.m_stAGV[i].MCS_Source_Port == "CCASLT01_UBP02"
                           || Main.m_stAGV[i].MCS_Dest_Port == "CCASLT01_UBP01" || Main.m_stAGV[i].MCS_Dest_Port == "CCASLT01_UBP02"
                           || ((Main.m_stAGV[i].current >= 1488 && Main.m_stAGV[i].current <= 1493)
                           || (Main.m_stAGV[i].current >= 1280 && Main.m_stAGV[i].current <= 1292)))
                            {
                                Check_Plus_SLT3 = 1;
                            }



                            if (Main.m_stAGV[i].dqGoal.Count != 0)
                            {
                                if (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 1393)
                                {
                                    //차가 상승리프트에 대기중이고, 하강 리프트가 꽉차면 할당 풀어주기
                                    if (Main.m_stAGV[i].current == 1501 && Main.m_stAGV[i].Goal == 1502
                                     && Main.CS_Work_DB.Carrier_1_Down == 1 && Main.CS_Work_DB.Carrier_2_Down == 1 && Main.CS_Work_DB.Carrier_3_Down == 1
                                     && Main.CS_Work_DB.Carrier_1_Up == 1 && Main.CS_Work_DB.Carrier_2_Up == 1 && Main.CS_Work_DB.Carrier_3_Up == 1)
                                    {
                                        continue;
                                    }

                                    FLAG_Lift_Order = 1;
                                }
                            }

                            //오버브릿지라인 있을때 팅구기
                            if (((Main.m_stAGV[i].current >= 1500 && Main.m_stAGV[i].current <= 1505) //상승, 하강 리프트 작업지
                            || (Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1318) //리프트 들어가기 직전 경로
                            || (Main.m_stAGV[i].current >= 1268 && Main.m_stAGV[i].current <= 1292) //리프트(Roll 방향) 슬리터 10호기 출고대쪽
                            || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395) //리프트 들어가기 직전 경로
                            || (Main.m_stAGV[i].current >= 1488 && Main.m_stAGV[i].current <= 1493) //슬리터 입고대
                            || (Main.m_stAGV[i].current >= 1354 && Main.m_stAGV[i].current <= 1364) //
                            || (((Main.m_stAGV[i].current >= 1482 && Main.m_stAGV[i].current <= 1487) || (Main.m_stAGV[i].current >= 1084 && Main.m_stAGV[i].current <= 1102)
                              || (Main.m_stAGV[i].current >= 1431 && Main.m_stAGV[i].current <= 1438) || (Main.m_stAGV[i].current >= 1336 && Main.m_stAGV[i].current <= 1336)
                              || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183) || (Main.m_stAGV[i].current >= 1183 && Main.m_stAGV[i].current <= 1183))
                            && Main.m_stAGV[i].Dest_Port_Num == "1502")) && Main.m_stAGV[i].mode == 1 && Main.m_stAGV[i].Error == 0)
                            {
                                //차가 상승리프트에 대기중이고, 하강 리프트가 꽉차면 할당 풀어주기
                                if (Main.m_stAGV[i].current == 1501 && Main.m_stAGV[i].Goal == 1502
                                 && Main.CS_Work_DB.Carrier_1_Down == 1 && Main.CS_Work_DB.Carrier_2_Down == 1 && Main.CS_Work_DB.Carrier_3_Down == 1
                                 && Main.CS_Work_DB.Carrier_1_Up == 1 && Main.CS_Work_DB.Carrier_2_Up == 1 && Main.CS_Work_DB.Carrier_3_Up == 1)
                                {
                                    continue;
                                }

                                FLAG_Lift_Order = 1;
                            }
                            //충전소 차량 확인
                            if ((Main.m_stAGV[i].state == 0 || Main.m_stAGV[i].state == 4 || Main.m_stAGV[i].state == 8 || Main.m_stAGV[i].state == 9)
                              && Main.m_stAGV[i].mode == 1 && Main.m_stAGV[i].dqGoal.Count == 0)
                            {
                                //----------------------------------------------음극------------------------------------------------------------------
                                //충전소에 있는지 확인
                                if ((Main.m_stAGV[i].current == 499 || Main.m_stAGV[i].current == 163 || Main.m_stAGV[i].current == 166) && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_Charge_Station_Minus = 1;
                                }
                                //33번에 있는지 확인
                                else if (Main.m_stAGV[i].current == 163 && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_163_Node_Minus = 1;
                                }
                                //44번에 있는지 확인
                                else if (Main.m_stAGV[i].current == 166 && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_166_Node_Minus = 1;
                                }
                                //----------------------------------------------양극------------------------------------------------------------------
                                //충전소에 있는지 확인
                                else if (Main.m_stAGV[i].current == 1499 && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_Charge_Station_Plus = 1;
                                }
                                //1237번에 있는지 확인
                                else if (Main.m_stAGV[i].current == 1237 && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_1237_Node = 1;
                                }
                                //1041번에 있는지 확인
                                else if (Main.m_stAGV[i].current == 1041 && Main.m_stAGV[i].FLAG_LGV_Charge == 0)
                                {
                                    Check_1041_Node = 1;
                                }


                                //----------------------------------------------릴타입------------------------------------------------------------------
                                else if (Main.m_stAGV[i].current == 1286)
                                {
                                    Check_Node_1286_AGV = 1;
                                }
                                else if (Main.m_stAGV[i].current == 1268)
                                {
                                    Check_Node_1268_AGV = 1;
                                }
                                else if (Main.m_stAGV[i].current == 1313)
                                {
                                    Check_Node_1313_AGV = 1;
                                }
                                else if (Main.m_stAGV[i].current == 1054)
                                {
                                    Check_Node_1054_AGV = 1;
                                }
                                //충전소_1에 있는지 확인
                                else if (Main.m_stAGV[i].current == 1438 && Main.m_stAGV[i].FLAG_LGV_Charge == 0 && Main.m_stAGV[i].MCS_Carrier_ID == "")
                                {
                                    Check_Reel_Charge_Station_1438 = 1;
                                }
                                //충전소_2에 있는지 확인
                                else if (Main.m_stAGV[i].current == 438 && Main.m_stAGV[i].FLAG_LGV_Charge == 0 && Main.m_stAGV[i].MCS_Carrier_ID == "")
                                {
                                    Check_Reel_Charge_Station_438 = 1;
                                }
                                //충전소_3에 있는지 확인
                                else if (Main.m_stAGV[i].current == 496 && Main.m_stAGV[i].FLAG_LGV_Charge == 0 && Main.m_stAGV[i].MCS_Carrier_ID == "")
                                {
                                    Check_Reel_Charge_Station_496 = 1;
                                }

                                //충전소_4에 있는지 확인
                                else if (Main.m_stAGV[i].current == 442 && Main.m_stAGV[i].FLAG_LGV_Charge == 0 && Main.m_stAGV[i].MCS_Carrier_ID == "")
                                {
                                    Check_Reel_Charge_Station_442 = 1;
                                }
                                //충전소_5에 있는지 확인
                                /*else if (Main.m_stAGV[i].current == 1556 && Main.m_stAGV[i].FLAG_LGV_Charge == 0 && Main.m_stAGV[i].MCS_Carrier_ID == "")
                                {
                                    Check_Reel_Charge_Station_1556 = 1;
                                }*/
                            }
                        }
                    }

                    //음극 롤 충전소 일때 
                    if (Main.m_stAGV[LGV_No].current == 499 && Check_Charge_Station_M_Roll == 1)
                    {
                        Alloc_Charge_Station_M_Roll = 1;
                    }
                    //양극 롤 충전소 일때 
                    if (Main.m_stAGV[LGV_No].current == 1499 && Check_Charge_Station_P_Roll == 1)
                    {
                        Alloc_Charge_Station_P_Roll = 1;
                    }
                    #endregion

                    if (LGV_No != 0 && LGV_No != 1 && LGV_No != 9 && LGV_No != 8 && LGV_No != 11 && LGV_No != 12)//20200820 중대형 Reel LGV 인덱스 추가
                    {
                        //----------------------------------------롤창고 작업 하당
                        #region 롤창고 입고대가는 차량 있으면 다른차량 출고대 명령 금지
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS01_UOP01" && Check_Plus_Roll_InPut1_Working_LGV == 1
                          && Main.m_stAGV[LGV_No].current != 1002 && Main.m_stAGV[LGV_No].current != 1004)
                        {
                            Alloc_Plus_Roll_InPut1 = 1;
                        }
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS02_UOP01" && Check_Plus_Roll_InPut2_Working_LGV == 1
                         && Main.m_stAGV[LGV_No].current != 1020 && Main.m_stAGV[LGV_No].current != 1022)
                        {
                            Alloc_Plus_Roll_InPut2 = 1;
                        }

                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS01_UOP01" && Check_Minus_Roll_InPut1_Working_LGV == 1
                         && Main.m_stAGV[LGV_No].current != 26 && Main.m_stAGV[LGV_No].current != 24)
                        {
                            Alloc_Minus_Roll_InPut1 = 1;
                        }
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS02_UOP01" && Check_Minus_Roll_InPut2_Working_LGV == 1
                         && Main.m_stAGV[LGV_No].current != 2 && Main.m_stAGV[LGV_No].current != 4)
                        {
                            Alloc_Minus_Roll_InPut2 = 1;
                        }

                        #endregion
                    }

                    if (LGV_No == 0 || LGV_No == 1 || LGV_No == 9 || LGV_No == 11 || LGV_No == 12)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01" && Check_3_Up_Lift == 1)
                        {
                            Alloc_3_Up_Lift = 1;
                        }

                        //----------------------------------------후공정 작업할당
                        #region 후공정 해당 슬리터 가고 있을때 다른차 할당 금지
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02")
                         && Check_Plus_SLT1_Working_LGV == 1)
                        {
                            Alloc_Work_Plus_SLT1 = 1;
                        }
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02")
                         && Check_Plus_SLT2_Working_LGV == 1)
                        {
                            Alloc_Work_Plus_SLT2 = 1;
                        }
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")
                         && Check_Plus_SLT3_Working_LGV == 1)
                        {
                            Alloc_Work_Plus_SLT3 = 1;
                        }

                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02")
                         && Check_Minus_SLT1_Working_LGV == 1)
                        {
                            Alloc_Work_Minus_SLT1 = 1;
                        }
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02")
                         && Check_Minus_SLT2_Working_LGV == 1)
                        {
                            Alloc_Work_Minus_SLT2 = 1;
                        }
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02")
                         && Check_Minus_SLT3_Working_LGV == 1)
                        {
                            Alloc_Work_Minus_SLT3 = 1;
                        }
                        #endregion

                        //----------------------------------------후공정 롤 리와인더 작업할당. lkw20190617
                        #region 후공정 해당 롤 리와인더 가고 있을때 다른차 할당 금지
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC6MRW01_UBP01")
                         && (Check_Plus_RRW1_Working_LGV == 1 ||    //양극 롤 리와인더 #1 입고 작업
                             Check_Plus_RRW2_Working_LGV == 1 ||    //양극 롤 리와인더 #2 입고 작업
                             FLAG_Check_PackingRoom_Order == 1))     //포장실 작업
                        {
                            Alloc_Work_Plus_RRW1 = 1;
                        }

                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7MRW01_UBP01")
                         && (Check_Minus_RRW1_Working_LGV == 1 ||
                             Check_Minus_RRW2_Working_LGV == 1))
                        {
                            Alloc_Work_Minus_RRW1 = 1;
                        }
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7MRW02_UBP01")
                         && (Check_Minus_RRW1_Working_LGV == 1 ||
                             Check_Minus_RRW2_Working_LGV == 1))
                        {
                            Alloc_Work_Minus_RRW2 = 1;
                        }
                        #endregion

                        //오버브릿지 하강, 오버 버퍼 작업일때 다른 차량이 오버브릿지 영역에 있으면 할당 막기
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01"
                        || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP01"
                        || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP02"
                        || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP03"
                        || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP04")
                        && FLAG_Lift_Order == 1 && Main.m_stAGV[LGV_No].current != 1313
                        && Main.m_stAGV[LGV_No].current != 1268 && Main.m_stAGV[LGV_No].current != 1286 && Main.m_stAGV[LGV_No].current != 1369)
                        {
                            FLAG_Alloc_Lift_Order = 1;
                        }

                        //음극 2,3식에 차량있으면 음극1식, 충전소에서 할당 안받기
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01")
                         || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02")
                         || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01")
                         || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"))
                         && Check_23SLT_LGV == 1
                         && ((Main.m_stAGV[LGV_No].current >= 108 && Main.m_stAGV[LGV_No].current <= 120)
                         || (Main.m_stAGV[LGV_No].current >= 437 && Main.m_stAGV[LGV_No].current <= 438)
                         || (Main.m_stAGV[LGV_No].current >= 173 && Main.m_stAGV[LGV_No].current <= 173)))
                        {
                            Alloc_23SLT_LGV = 1;
                        }

                        //음극 3식에 차량있으면, 1,2식 2식위치에서 할당 안받기
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01")
                         || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"))
                         && Check_3SLT_LGV == 1
                         && Main.m_stAGV[LGV_No].current >= 278 && Main.m_stAGV[LGV_No].current <= 280)
                        {
                            Alloc_3SLT_LGV = 1;
                        }
                    }

                    //기재 반입구 - 음극 롤 (음극 롤이 작업 할때 롤 할당 금지)
                    if (LGV_No == 5 || LGV_No == 6 || LGV_No == 7)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01"
                        && (Main.m_stCommand[WaitCommandCount].Dest_Port == "CA8PLS02_LIP01" || Main.m_stCommand[WaitCommandCount].Dest_Port == "CA8PLS01_LIP01"))
                        {
                            if (FLAG_Check_Entry_LGV == 1 || FLAG_Check_Entry_LGV_Minus_Roll == 1)
                            {
                                Alloc_Entry_Minus_Roll = 1;
                            }
                        }
                    }


                    //기재 반입구 - 양극 롤 (양극 롤이 작업 할때 롤 할당 금지)
                    if (LGV_No == 2 || LGV_No == 3 || LGV_No == 4)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01"
                        && (Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8PLS01_LIP01" || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8PLS02_LIP01"))
                        {
                            if (FLAG_Check_Entry_LGV == 1 || FLAG_Check_Entry_LGV_Plus_Roll == 1)
                            {
                                Alloc_Entry_Plus_Roll = 1;
                            }
                        }
                    }

                    if (LGV_No == 10)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01")
                        {
                            if (FLAG_Check_Entry_LGV_Roll == 1)
                            {
                                Alloc_Entry_LGV = 1;
                            }
                        }
                    }

                    //기재 반입구 - 공통
                    if (LGV_No != 0 && LGV_No != 1 && LGV_No != 9 && LGV_No != 11 && LGV_No != 12)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01")
                        {
                            if (FLAG_Check_Entry_LGV == 1)
                            {
                                Alloc_Entry_LGV = 1;
                            }
                        }
                    }

                    //기재 반입구 명령이 하나라도 있으면 절대 할당 금지 kjh 2020 06 05
                    /*if(Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01" || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8MIB01_BBP01")
                    {
                        Alloc_Entry_LGV = 1;
                    }

                    //기재 반입구 경로에 LGV가 한대라도 있으면 절대 할당 금지 kjh 2020 06 05
                    if (Alloc_Entry_Minus_Roll == 1 || Alloc_Entry_Plus_Roll == 1 || FLAG_Check_Entry_LGV_Roll == 1)
                    {
                        Alloc_Entry_LGV = 1;
                    }*/

                    //양극 기재 렉 창고 - 양극 롤 차량이 기재 반입구 가고 있으면 할당 금지
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP01"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP02"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP03"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP04"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP05"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP06"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP07"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP08"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP09"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP10"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP11"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP12"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP13"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP14"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP15"
                    || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLB02_BBP16")
                   && FLAG_Check_Entry_LGV_Plus_Roll == 1)
                    {
                        Alloc_Entry_LGV = 1;
                    }

                    //명령이 포장실인데 이미 다른 차량이 포장실 작업중이면 팅구기 2020 07 13 kjh 출발지 포장실 추가
                    if (((Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8MPB01_BBP01"
                      || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8MPB01_BBP02")
                      || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MPB01_BBP01"
                      || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MPB01_BBP02"))
                        && FLAG_Check_PackingRoom_Order == 1)
                    {
                        Alloc_PackingRoom_Order = 1;
                    }

                    //---------------------------------------------------------음극----------------------------------
                    if (Main.m_stAGV[LGV_No].current == 499)
                    {
                        //499에서 창고 명령이 아닌데 다른 차량이 대기장소에 있으면 팅구기
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS01_UOP01"
                         && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS02_UOP01"
                         && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8MIB01_BBP01")
                         && (Check_163_Node_Minus == 1 || Check_166_Node_Minus == 1))
                        {
                            Alloc_AGV_Charge = 1;
                        }
                    }
                    else if (Main.m_stAGV[LGV_No].current == 163 || Main.m_stAGV[LGV_No].current == 166)
                    {
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS01_UOP01"
                         || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS02_UOP01"
                         || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01")
                         && Check_Charge_Station_Minus == 1)
                        {
                            Alloc_AGV_163 = 1;
                        }
                    }

                    //---------------------------------------------------------양극----------------------------------
                    //1499 1순위 : 롤창고
                    else if (Main.m_stAGV[LGV_No].current == 1499)
                    {
                        //1499에서 창고 명령이 아닌데 다른 차량이 대기장소에 있으면 팅구기
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS01_UOP01"
                         && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS02_UOP01"
                         && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8MIB01_BBP01")
                         && (Check_1237_Node == 1 || Check_1041_Node == 1))
                        {
                            Alloc_AGV_Charge_Plus = 1;
                        }
                    }
                    //1041 : 창고, 1237부근 작업 팅구기
                    else if (Main.m_stAGV[LGV_No].current == 1041)
                    {
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS01_UOP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS02_UOP01"
                           || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8MIB01_BBP01")
                          && Check_Charge_Station_Plus == 1)
                          || ((Main.m_stCommand[WaitCommandCount].Source_Port == "CC9PRE01_LBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9COT01_UBP02"
                          || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9COT01_UBP01" || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9COT01_LBP01")
                          && Check_1237_Node == 1))
                        {
                            Alloc_AGV_1041 = 1;
                        }
                    }
                    //1237부근 작업 아니면 다 팅구기
                    else if (Main.m_stAGV[LGV_No].current == 1237)
                    {
                        // 양극 광폭 코터 입고대 근처에 작업이 있으면 충전소 ROLL 차량 할당 금지. lkw20190415
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC9PRE01_LBP01"
                          && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9COT01_UBP02"
                          && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9COT01_UBP01"
                          && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9COT01_LBP01")
                          && (Check_1041_Node == 1 || Check_Plus_Big_Size_Input == 1))
                        {
                            Alloc_AGV_1237 = 1;
                        }
                    }


                    //---------------------------------------------------------릴타입----------------------------------
                    else if (Main.m_stAGV[LGV_No].current == 1268 || Main.m_stAGV[LGV_No].current == 1286
                         || Main.m_stAGV[LGV_No].current == 1313 || Main.m_stAGV[LGV_No].current == 1054)
                    {
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"))
                         && Check_Reel_Charge_Station_1438 == 1)
                        {
                            Alloc_Plus_Work_Node = 1;
                        }
                    }

                    else if (Main.m_stAGV[LGV_No].current == 1438)
                    {
                        //음극 슬리터
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"))
                         && (Check_Reel_Charge_Station_438 == 1 || Check_Reel_Charge_Station_496 == 1
                          || Check_Node_1054_AGV == 1 || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_1438 = 1;
                        }
                        //M동 하강리프트
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01")
                         && (Check_Node_1054_AGV == 1 || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_1438 = 1;
                        }
                        //양극 3식 슬리터
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                               || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT02_UBP01")
                         && (Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_1438 = 1;
                        }
                    }
                    else if (Main.m_stAGV[LGV_No].current == 438)
                    {
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"))
                         && Check_Reel_Charge_Station_1438 == 1)
                        {
                            Alloc_Reel_Charge_Station_438 = 1;
                        }
                        //음극 2식 슬리터. 3식 슬리터
                        else if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01")
                               || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02")
                               || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01")
                               || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"))
                               && Check_Reel_Charge_Station_496 == 1)
                        {
                            Alloc_Reel_Charge_Station_438 = 1;
                        }
                        //M동 하강리프트
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01")
                               && (Check_Reel_Charge_Station_1438 == 1 || Check_Node_1054_AGV == 1 || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_438 = 1;
                        }
                        //양극 3식 슬리터
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                               || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT02_UBP01")
                               && (Check_Reel_Charge_Station_438 == 1 || Check_Reel_Charge_Station_1438 == 1 || Check_Node_1054_AGV == 1
                               || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_438 = 1;
                        }
                    }
                    else if (Main.m_stAGV[LGV_No].current == 496)
                    {
                        //양극 슬리터
                        if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"))
                         && (Check_Reel_Charge_Station_1438 == 1 || Check_Reel_Charge_Station_438 == 1))
                        {
                            Alloc_Reel_Charge_Station_496 = 1;
                        }
                        //음극 1식 슬리터
                        else if (((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01")
                              || (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02"))
                              && Check_Reel_Charge_Station_438 == 1)
                        {
                            Alloc_Reel_Charge_Station_496 = 1;
                        }
                        //M동 하강리프트
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01")
                               && (Check_Reel_Charge_Station_438 == 1 || Check_Reel_Charge_Station_1438 == 1 || Check_Node_1054_AGV == 1
                                || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_496 = 1;
                        }
                        //양극 3식 슬리터
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                               || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT02_UBP01")
                         && (Check_Reel_Charge_Station_438 == 1 || Check_Reel_Charge_Station_1438 == 1 || Check_Node_1054_AGV == 1
                                || Check_Node_1286_AGV == 1 || Check_Node_1268_AGV == 1 || Check_Node_1313_AGV == 1))
                        {
                            Alloc_Reel_Charge_Station_496 = 1;
                        }
                    }
                    #endregion

                    #region 오버브릿지 작업할당 관리

                    //오버브릿지 상승 리프트 물건이 다차있거나, 다찼을때 다른 차량이 가고 있으면 막기
                    if ((Main.CS_Work_DB.Carrier_1_Up == 1 && Main.CS_Work_DB.Carrier_2_Up == 1 && Main.CS_Work_DB.Carrier_3_Up == 1)
                    || (Main.CS_Work_DB.Carrier_1_Up == 1 && Main.CS_Work_DB.Carrier_2_Up == 1 && Main.CS_Work_DB.Carrier_3_Up == 0 && Check_Work_UpLift == 1)
                    || (Main.CS_Work_DB.Carrier_1_Up == 1 && Main.CS_Work_DB.Carrier_2_Up == 0 && Main.CS_Work_DB.Carrier_3_Up == 1 && Check_Work_UpLift == 1)
                    || (Main.CS_Work_DB.Carrier_1_Up == 0 && Main.CS_Work_DB.Carrier_2_Up == 1 && Main.CS_Work_DB.Carrier_3_Up == 1 && Check_Work_UpLift == 1))
                    {
                        Check_UP_Lift_Carrier = 1;
                    }

                    if ((Main.CS_AGV_Logic.Carrier_1[1] == "1" && Main.CS_AGV_Logic.Lift_State[1] == "1" && Main.CS_AGV_Logic.Carrier_3[1] == "1")
                     || (Main.CS_AGV_Logic.Carrier_1[1] == "1" && Main.CS_AGV_Logic.Lift_State[1] == "1" && Main.CS_AGV_Logic.Carrier_3[1] == "0" && Check_Floor3_Up_Lift_LGV == 1)
                     || (Main.CS_AGV_Logic.Carrier_1[1] == "1" && Main.CS_AGV_Logic.Lift_State[1] == "0" && Main.CS_AGV_Logic.Carrier_3[1] == "1" && Check_Floor3_Up_Lift_LGV == 1)
                     || (Main.CS_AGV_Logic.Carrier_1[1] == "0" && Main.CS_AGV_Logic.Lift_State[1] == "1" && Main.CS_AGV_Logic.Carrier_3[1] == "1" && Check_Floor3_Up_Lift_LGV == 1))
                    {
                        Check_Floor3_Up_Lift = 1;
                    }
                    
                    int Check_7SLT_ORDER = 0;

                    for (int i = 0; i < Main.CS_Work_DB.waitCommand.Count; i++)
                    {
                        if (Main.m_stCommand[i].Alloc_State != "0") continue;
                       
                        if (Main.M_7_SLT_First_Mode == true)
                        {
                            if (Main.m_stCommand[i].Source_Port == "CA7SLT01_UBP01"
                            && ((Main.m_stAGV[LGV_No].current >= 108 && Main.m_stAGV[LGV_No].current <= 116)
                            || (Main.m_stAGV[LGV_No].current >= 173 && Main.m_stAGV[LGV_No].current <= 173)
                            || (Main.m_stAGV[LGV_No].current >= 264 && Main.m_stAGV[LGV_No].current <= 282)
                            || (Main.m_stAGV[LGV_No].current >= 437 && Main.m_stAGV[LGV_No].current <= 439)
                            || (Main.m_stAGV[LGV_No].current >= 494 && Main.m_stAGV[LGV_No].current <= 496)

                            || (Main.m_stAGV[LGV_No].current >= 1090 && Main.m_stAGV[LGV_No].current <= 1098)
                            || (Main.m_stAGV[LGV_No].current >= 1083 && Main.m_stAGV[LGV_No].current <= 1085)
                            || (Main.m_stAGV[LGV_No].current >= 1437 && Main.m_stAGV[LGV_No].current <= 1439)))
                            {
                                Check_7SLT_ORDER = 1;
                            }
                        }
                        else if (Main.M_7_SLT_Minus_First_Mode == true)
                        {
                            if (Main.m_stCommand[i].Source_Port == "CA7SLT01_UBP01"
                            && ((Main.m_stAGV[LGV_No].current >= 108 && Main.m_stAGV[LGV_No].current <= 116)
                            || (Main.m_stAGV[LGV_No].current >= 173 && Main.m_stAGV[LGV_No].current <= 173)
                            || (Main.m_stAGV[LGV_No].current >= 264 && Main.m_stAGV[LGV_No].current <= 282)
                            || (Main.m_stAGV[LGV_No].current >= 437 && Main.m_stAGV[LGV_No].current <= 439)
                            || (Main.m_stAGV[LGV_No].current >= 494 && Main.m_stAGV[LGV_No].current <= 496)))
                            {
                                Check_7SLT_ORDER = 1;
                            }
                        }
                    }

                    if(Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Check_7SLT_ORDER == 1)
                    {
                        Alloc_7SLT = 1;
                    }

                    //대기중인 작업 탐색
                    for (int i = 0; i < Main.CS_Work_DB.waitCommand.Count; i++)
                    {
                        if (WaitCommandCount == i) continue;
                        if (Main.m_stCommand[i].Alloc_State != "0") continue;
                        
                        //오버브릿지 하강, 버퍼 명령 있는지 확인
                        if (Main.m_stCommand[i].Source_Port == "CC8FLF02_BBP01" || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP01"
                         || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP02" || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP03"
                         || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP04")
                        {
                            //오버브릿지 하강 리프트일때
                            if (Main.m_stCommand[i].Source_Port == "CC8FLF02_BBP01")
                            {
                                if (Main.m_stCommand[i].Dest_Port == "CC8ERB01_BBP01" || Main.m_stCommand[i].Dest_Port == "CC8ERB01_BBP02"
                                || Main.m_stCommand[i].Dest_Port == "CC8ERB01_BBP03" || Main.m_stCommand[i].Dest_Port == "CC8ERB01_BBP04")
                                {
                                    if (Check_Over_Buffer_AGV == 1)
                                    {
                                        continue;
                                    }
                                }
                            }
                            if (Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP01" || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP02"
                             || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP03" || Main.m_stCommand[i].Source_Port == "CC8ERB01_BBP04")
                            {
                                if (Check_Over_Buffer_AGV == 1)
                                {
                                    continue;
                                }
                            }
                            Check_Over_Lift_Order = 1;
                        }

                        //M동 하강리프트 명령 있는지 확인
                        if (Main.m_stCommand[i].Source_Port == "CA7FLF01_BBP01")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0)
                            {
                                Check_Floor3_Down_Lift = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01")
                            {
                                Check_Floor3_Down_Lift = 1;
                            }
                        }
                        #region 음/양 슬리터 작업 있는지 확인
                        //음극 1식 슬리터 작업 있는지 확인
                        if (Main.m_stCommand[i].Source_Port == "CA7SLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CA7SLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Minus_SLT1_Working_LGV == 0)
                            {
                                Check_Minus_SLT1_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Minus_SLT1_Working_LGV == 0)
                            {
                                Check_Minus_SLT1_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Minus_SLT1_Working_LGV == 0)
                            {
                                Check_Minus_SLT1_Order = 1;
                            }
                        }
                        //음극 2식 슬리터 작업 있는지 확인
                        else if (Main.m_stCommand[i].Source_Port == "CA8SLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CA8SLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Minus_SLT2_Working_LGV == 0)
                            {
                                Check_Minus_SLT2_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Minus_SLT2_Working_LGV == 0)
                            {
                                Check_Minus_SLT2_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Minus_SLT2_Working_LGV == 0)
                            {
                                Check_Minus_SLT2_Order = 1;
                            }
                        }
                        //음극 3식 슬리터 작업 있는지 확인
                        else if (Main.m_stCommand[i].Source_Port == "CA9SLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CA9SLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Minus_SLT3_Working_LGV == 0)
                            {
                                Check_Minus_SLT3_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Minus_SLT3_Working_LGV == 0)
                            {
                                Check_Minus_SLT3_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Minus_SLT3_Working_LGV == 0)
                            {
                                Check_Minus_SLT3_Order = 1;
                            }
                        }

                        //양극 1식 슬리터 작업 있는지 확인
                        else if (Main.m_stCommand[i].Source_Port == "CC8SLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CC8SLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Plus_SLT1_Working_LGV == 0)
                            {
                                Check_Plus_SLT1_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Plus_SLT1_Working_LGV == 0)
                            {
                                Check_Plus_SLT1_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Plus_SLT1_Working_LGV == 0)
                            {
                                Check_Plus_SLT1_Order = 1;
                            }
                        }

                        //양극 2식 슬리터 작업 있는지 확인
                        else if (Main.m_stCommand[i].Source_Port == "CC9SLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CC9SLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Plus_SLT2_Working_LGV == 0)
                            {
                                Check_Plus_SLT2_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Plus_SLT2_Working_LGV == 0)
                            {
                                Check_Plus_SLT2_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Plus_SLT2_Working_LGV == 0)
                            {
                                Check_Plus_SLT2_Order = 1;
                            }
                        }

                        //양극 3식 슬리터 작업 있는지 확인
                        else if (Main.m_stCommand[i].Source_Port == "CCASLT01_UBP01" || Main.m_stCommand[i].Source_Port == "CCASLT01_UBP02")
                        {
                            //도착지가 오버브릿지 상승리프트일때 만재 확인
                            if (Main.m_stCommand[i].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 0 && Check_Plus_SLT3_Working_LGV == 0)
                            {
                                Check_Plus_SLT3_Order = 1;
                            }
                            //도착지가 M동 상승리프트 일때 만재 확인
                            else if (Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 0 && Check_Plus_SLT3_Working_LGV == 0)
                            {
                                Check_Plus_SLT3_Order = 1;
                            }
                            else if (Main.m_stCommand[i].Dest_Port != "CA7FLF02_BBP01" && Main.m_stCommand[i].Dest_Port == "CC8FLF01_BBP01" && Check_Plus_SLT3_Working_LGV == 0)
                            {
                                Check_Plus_SLT3_Order = 1;
                            }
                        }
                        #endregion

                        #region 롤창고 출고대 작업 있는지 확인

                        if (Main.m_stCommand[i].Source_Port == "CC8PLS01_UOP01" && FLAG_Check_PackingRoom_Order == 0)
                        {  
                            Check_Plus_Roll_OutPut1_Order = 1;
                        }
                        if (Main.m_stCommand[i].Source_Port == "CC8PLS02_UOP01" && FLAG_Check_PackingRoom_Order == 0)
                        {
                            Check_Plus_Roll_OutPut2_Order = 1;
                        }
                        if (Main.m_stCommand[i].Source_Port == "CA8PLS01_UOP01" && FLAG_Check_PackingRoom_Order == 0)
                        {
                            Check_Minus_Roll_OutPut1_Order = 1;
                        }
                        if (Main.m_stCommand[i].Source_Port == "CA8PLS02_UOP01" && FLAG_Check_PackingRoom_Order == 0)
                        {
                            Check_Minus_Roll_OutPut2_Order = 1;
                        }
                        #endregion

                        #region 롤 리와인더 출고대 작업 있는지 확인. lkw20190617

                        if (Main.m_stCommand[i].Source_Port == "CC6MRW01_UBP01")
                        {
                            Check_Plus_RRW1_Order = 1;
                        }
                        if (Main.m_stCommand[i].Source_Port == "CA7MRW01_UBP01")
                        {
                            Check_Minus_RRW1_Order = 1;
                        }
                        if (Main.m_stCommand[i].Source_Port == "CA7MRW02_UBP01")
                        {
                            Check_Minus_RRW2_Order = 1;
                        }
                        #endregion

                    }
                    //양극 3식 슬리터 작업 일때 다른 차량이 3식슬리터 작업 하고 있으면 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01" || Main.m_stCommand[WaitCommandCount].Dest_Port == "CCASLT01_UBP01"
                        || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02" || Main.m_stCommand[WaitCommandCount].Dest_Port == "CCASLT01_UBP02")
                        && Check_Plus_SLT3 == 1)
                    {
                        Alloc_Plus_SLT3 = 1;
                    }

                    //오버 하강리프트 -> 버퍼 작업일때
                    if (Main.m_stAGV[LGV_No].current == 1313 || Main.m_stAGV[LGV_No].current == 1268)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01")
                        {
                            if (Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8ERB01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8ERB01_BBP02"
                            || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8ERB01_BBP03"
                            || Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8ERB01_BBP04")
                            {
                                //바로 뒤에 작업 가능한 차량이 있으면 팅구기
                                if (Check_Over_Buffer_AGV == 1)
                                {
                                    Alloc_Over_Buffer_Order = 1;
                                }
                            }
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP01"
                             || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP02"
                             || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP03"
                             || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP04")
                        {
                            //바로 뒤에 작업 가능한 차량이 있으면 팅구기
                            if (Check_Over_Buffer_AGV == 1)
                            {
                                Alloc_Over_Buffer_Order = 1;
                            }
                        }
                    }
                    //1313,1268에서는 버퍼, 오버하강리프트 우선순위 1등, M동 하강리프트 2등
                    if (Main.m_stAGV[LGV_No].current == 1313 || Main.m_stAGV[LGV_No].current == 1268)
                    {
                        if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC8FLF02_BBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP01"
                             && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP02" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP03"
                             && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP04" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP04")
                            && Check_Over_Lift_Order == 1)
                        {
                            Alloc_Over_Lift_Order = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC8FLF02_BBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP01"
                             && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP02" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP03"
                             && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP04" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP04")
                            && (Check_Over_Lift_Order == 0 && Check_Floor3_Down_Lift == 1))
                        {
                            Alloc_Over_Lift_Order = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP03"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP04"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01")

                        {
                            Alloc_Over_Lift_Order = 0;
                        }
                    }
                    //양극 3식 슬리터 탈출지 우선순위 3식슬리터 1순위, (하강리프트,버퍼) 2순위, m동하강 리프트 3순위
                    if (Main.m_stAGV[LGV_No].current == 1286)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP02"
                            && Check_Plus_SLT3_Order == 1)
                        {
                            Alloc_Over_Lift_Order = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP02"
                            && Check_Plus_SLT3_Order == 0 && Check_Over_Lift_Order == 1)
                        {
                            Alloc_Over_Lift_Order = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8FLF02_BBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP01"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP02" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP03"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8ERB01_BBP04")
                            && (Check_Plus_SLT3_Order == 0 && Check_Over_Lift_Order == 0 && Check_Floor3_Down_Lift == 1))
                        {
                            Alloc_Over_Lift_Order = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP03"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8ERB01_BBP04"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")

                        {
                            Alloc_Over_Lift_Order = 0;
                        }
                    }

                    //1054에서는 M동하강 리프트 우선순위1
                    if ((Main.m_stAGV[LGV_No].current >= 1054 && Main.m_stAGV[LGV_No].current <= 1054))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA7FLF01_BBP01" && Check_Floor3_Down_Lift == 1)
                        {
                            Alloc_Floor3_Down_Lift = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01")
                        {
                            Alloc_Floor3_Down_Lift = 0;
                        }
                    }

                    //음극 1식 - 1순위 : 1식슬리터, 2순위 : 음극 슬리터, 3순위 : 양극 슬리터
                    if ((Main.m_stAGV[LGV_No].current >= 113 && Main.m_stAGV[LGV_No].current <= 115))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP02"
                            && Check_Minus_SLT1_Order == 1)
                        {
                            Alloc_Work_Minus_SLT1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP02"
                            && Check_Minus_SLT1_Order == 0 && (Check_Minus_SLT2_Order == 1 || Check_Minus_SLT3_Order == 1))
                        {
                            Alloc_Work_Minus_SLT1 = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP02")
                            && (Check_Minus_SLT1_Order == 0 && Check_Minus_SLT2_Order == 0 && Check_Minus_SLT3_Order == 0 &&
                               (Check_Plus_SLT1_Order == 1 || Check_Plus_SLT2_Order == 1 || Check_Plus_SLT3_Order == 1)))
                        {
                            Alloc_Work_Minus_SLT1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"

                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")

                        {
                            Alloc_Work_Minus_SLT1 = 0;
                        }
                    }

                    //음극 2식 - 1순위 : 2식슬리터, 2순위 : 음극 슬리터, 3순위 : 양극 슬리터
                    if ((Main.m_stAGV[LGV_No].current >= 278 && Main.m_stAGV[LGV_No].current <= 280))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP02"
                            && Check_Minus_SLT2_Order == 1)
                        {
                            Alloc_Work_Minus_SLT2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP02"
                            && Check_Minus_SLT2_Order == 0 && (Check_Minus_SLT1_Order == 1 || Check_Minus_SLT3_Order == 1))
                        {
                            Alloc_Work_Minus_SLT2 = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP02")
                            && (Check_Minus_SLT1_Order == 0 && Check_Minus_SLT2_Order == 0 && Check_Minus_SLT3_Order == 0 &&
                               (Check_Plus_SLT1_Order == 1 || Check_Plus_SLT2_Order == 1 || Check_Plus_SLT3_Order == 1)))
                        {
                            Alloc_Work_Minus_SLT2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"

                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")

                        {
                            Alloc_Work_Minus_SLT2 = 0;
                        }
                    }

                    //음극 3식 - 1순위 : 3식슬리터, 2순위 : 음극 슬리터, 3순위 : 양극 슬리터
                    if ((Main.m_stAGV[LGV_No].current >= 265 && Main.m_stAGV[LGV_No].current <= 267))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP02"
                            && Check_Minus_SLT3_Order == 1)
                        {
                            Alloc_Work_Minus_SLT3 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP02"
                            && Check_Minus_SLT3_Order == 0 && (Check_Minus_SLT1_Order == 1 || Check_Minus_SLT2_Order == 1))
                        {
                            Alloc_Work_Minus_SLT3 = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA8SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CA9SLT01_UBP02")
                            && (Check_Minus_SLT1_Order == 0 && Check_Minus_SLT2_Order == 0 && Check_Minus_SLT3_Order == 0 &&
                               (Check_Plus_SLT1_Order == 1 || Check_Plus_SLT2_Order == 1 || Check_Plus_SLT3_Order == 1)))
                        {
                            Alloc_Work_Minus_SLT3 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02"

                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")

                        {
                            Alloc_Work_Minus_SLT3 = 0;
                        }
                    }

                    //양극 롤창고 1 - 1순위 출고대_1, 2순위 출고대_2
                    if (Main.m_stAGV[LGV_No].current == 1002)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS01_UOP01" && Check_Plus_Roll_OutPut1_Order == 1)
                        {
                            Alloc_Plus_Roll_InPut1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS01_UOP01" && (Check_Plus_Roll_OutPut1_Order == 0 && Check_Plus_Roll_OutPut2_Order == 1))
                        {
                            Alloc_Plus_Roll_InPut1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS01_UOP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS02_UOP01")

                        {
                            Alloc_Plus_Roll_InPut1 = 0;
                        }
                    }

                    //양극 롤창고 2 - 1순위 출고대_2, 2순위 출고대_1
                    if (Main.m_stAGV[LGV_No].current == 1020)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS02_UOP01" && Check_Plus_Roll_OutPut2_Order == 1)
                        {
                            Alloc_Plus_Roll_InPut2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8PLS02_UOP01" && (Check_Plus_Roll_OutPut2_Order == 0 && Check_Plus_Roll_OutPut1_Order == 1))
                        {
                            Alloc_Plus_Roll_InPut2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS01_UOP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8PLS02_UOP01")

                        {
                            Alloc_Plus_Roll_InPut2 = 0;
                        }
                    }

                    //음극 롤창고 1 - 1순위 출고대_1, 2순위 출고대_2
                    if (Main.m_stAGV[LGV_No].current == 1002)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS01_UOP01" && Check_Minus_Roll_OutPut1_Order == 1)
                        {
                            Alloc_Minus_Roll_InPut1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS01_UOP01" && (Check_Minus_Roll_OutPut1_Order == 0 && Check_Minus_Roll_OutPut2_Order == 1))
                        {
                            Alloc_Minus_Roll_InPut1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS01_UOP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS02_UOP01")

                        {
                            Alloc_Minus_Roll_InPut1 = 0;
                        }
                    }

                    //음극 롤창고 2 - 1순위 출고대_2, 2순위 출고대_1
                    if (Main.m_stAGV[LGV_No].current == 4)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS02_UOP01" && Check_Minus_Roll_OutPut2_Order == 1)
                        {
                            Alloc_Minus_Roll_InPut2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA8PLS02_UOP01" && (Check_Minus_Roll_OutPut2_Order == 0 && Check_Minus_Roll_OutPut1_Order == 1))
                        {
                            Alloc_Minus_Roll_InPut2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS01_UOP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8PLS02_UOP01")

                        {
                            Alloc_Minus_Roll_InPut2 = 0;
                        }
                    }

                    //양극 1식 - 1순위 : 1식슬리터, 2순위 : 양극 슬리터, 3순위 : M동 하강,오버하강 리프트
                    if ((Main.m_stAGV[LGV_No].current >= 1091 && Main.m_stAGV[LGV_No].current <= 1097))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP02"
                            && Check_Plus_SLT1_Order == 1)
                        {
                            Alloc_Work_Plus_SLT1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP02"
                            && Check_Plus_SLT1_Order == 0 && (Check_Plus_SLT2_Order == 1 || Check_Plus_SLT3_Order == 1))
                        {
                            Alloc_Work_Plus_SLT1 = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP02")
                            && (Check_Plus_SLT1_Order == 0 && Check_Plus_SLT2_Order == 0 && Check_Plus_SLT3_Order == 0 &&
                               (Check_Over_Lift_Order == 1 || Check_Floor3_Down_Lift == 1)))
                        {
                            Alloc_Work_Plus_SLT1 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01"

                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")
                        {
                            Alloc_Work_Plus_SLT1 = 0;
                        }
                    }

                    //양극 2식 - 1순위 : 2식슬리터, 2순위 : 양극 슬리터, 3순위 : M동 하강,오버하강 리프트
                    if (Main.m_stAGV[LGV_No].current >= 1084 && Main.m_stAGV[LGV_No].current <= 1084)
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP02"
                            && Check_Plus_SLT2_Order == 1)
                        {
                            Alloc_Work_Plus_SLT2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP02"
                            && Check_Plus_SLT2_Order == 0 && (Check_Plus_SLT1_Order == 1 || Check_Plus_SLT3_Order == 1))
                        {
                            Alloc_Work_Plus_SLT2 = 1;
                        }
                        else if ((Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC8SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CC9SLT01_UBP02"
                               && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP01" && Main.m_stCommand[WaitCommandCount].Source_Port != "CCASLT01_UBP02")
                            && (Check_Plus_SLT1_Order == 0 && Check_Plus_SLT2_Order == 0 && Check_Plus_SLT3_Order == 0 &&
                               (Check_Over_Lift_Order == 1 || Check_Floor3_Down_Lift == 1)))
                        {
                            Alloc_Work_Plus_SLT2 = 1;
                        }
                        else if (Main.m_stCommand[WaitCommandCount].Source_Port == "CA7FLF01_BBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8FLF02_BBP01"

                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CC9SLT01_UBP02"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP01"
                            || Main.m_stCommand[WaitCommandCount].Source_Port == "CCASLT01_UBP02")

                        {
                            Alloc_Work_Plus_SLT2 = 0;
                        }
                    }

                    //음극 1식 슬리터 배출 작업일때 다른 차량이 1식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02") && Check_Minus_SLT1_Working_LGV == 1)
                    {
                        Alloc_Work_Minus_SLT1 = 1;
                    }

                    //음극 2식 슬리터 배출 작업일때 다른 차량이 2식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02") && Check_Minus_SLT2_Working_LGV == 1)
                    {
                        Alloc_Work_Minus_SLT2 = 1;
                    }

                    //음극 3식 슬리터 배출 작업일때 다른 차량이 3식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02") && Check_Minus_SLT3_Working_LGV == 1)
                    {
                        Alloc_Work_Minus_SLT3 = 1;
                    }

                    //양극 1식 슬리터 배출 작업일때 다른 차량이 1식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA7SLT01_UBP02") && Check_Minus_SLT1_Working_LGV == 1)
                    {
                        Alloc_Work_Plus_SLT1 = 1;
                    }

                    //양극 2식 슬리터 배출 작업일때 다른 차량이 2식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA8SLT01_UBP02") && Check_Minus_SLT2_Working_LGV == 1)
                    {
                        Alloc_Work_Plus_SLT2 = 1;
                    }

                    //양극 3식 슬리터 배출 작업일때 다른 차량이 3식슬리터로 공급 작업중일때 할당 막기
                    if ((Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CA9SLT01_UBP02") && Check_Minus_SLT3_Working_LGV == 1)
                    {
                        Alloc_Work_Plus_SLT3 = 1;
                    }

                    //오버브릿지 상승리프트 만재 확인
                    if (Main.m_stCommand[WaitCommandCount].Dest_Port == "CA7FLF02_BBP01" && Check_UP_Lift_Carrier == 1)
                    {
                        Alloc_UP_Lift_Carrier = 1;
                    }
                    //3층 상승 리프트 물건이 다차있으면 막기
                    if (Main.m_stCommand[WaitCommandCount].Dest_Port == "CC8FLF01_BBP01" && Check_Floor3_Up_Lift == 1)
                    {
                        Alloc_Floor3_Up_Lift = 1;
                    }


                    //1084에서 다른차량이 양극1식 슬리터 작업중이면 막기
                    if (Main.m_stAGV[LGV_No].current == 1084 && Check_PLUS_SLT_1 == 1
                    && (Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP01"
                     || Main.m_stCommand[WaitCommandCount].Source_Port == "CC8SLT01_UBP02"))
                    {
                        Alloc_PLUS_SLT_1 = 1;
                    }
                    //포장실 탈출지에서는 할당 금지
                    //포장실 탈출지에서 양극 롤 리와인더 #1 출고대 작업이 없으면 할당 금지한다. lkw20190617
                    if (Main.m_stAGV[LGV_No].current == 1156 && 
                        Check_Plus_RRW1_Order == 0)
                    {
                        Alloc_1156_NoWork = 1;
                    }

                    if ((Main.m_stAGV[LGV_No].current == 1369 || Main.m_stAGV[LGV_No].current == 369) && (Main.m_stAGV[LGV_No].MCS_Source_Port == "" && Main.m_stAGV[LGV_No].MCS_Dest_Port == ""))
                    {
                        if (Main.m_stCommand[WaitCommandCount].Source_Port != "CA7FLF01_BBP01")
                        {
                            Alloc_1369_NoWork = 1;
                        }
                    }

                    //양극 롤 리와인더 #1 출고대 작업이 있고,
                    //양극 롤 리와인더 #1 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. 
                    //양극 롤 리와인더 #2 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다.
                    //포장실 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. lkw20190617
                    if ((Main.m_stAGV[LGV_No].current == 1085 || Main.m_stAGV[LGV_No].current == 1149 || Main.m_stAGV[LGV_No].current == 1156)
                        && Check_Plus_RRW1_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port == "CC6MRW01_UBP01")
                    {
                        Alloc_Work_Plus_RRW1 = 0;
                    }

                    //양극 롤 리와인더 #1 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    //양극 롤 리와인더 #2 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    if ((Main.m_stAGV[LGV_No].current == 1085 || Main.m_stAGV[LGV_No].current == 1149)
                        && Check_Plus_RRW1_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port != "CC6MRW01_UBP01")
                    {
                        Alloc_Work_Plus_RRW1 = 1;
                    }

                    //음극 롤 리와인더 #1 출고대 작업이 있고,
                    //음극 롤 리와인더 #1 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. 
                    //음극 롤 리와인더 #2 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. lkw20190617
                    if ((Main.m_stAGV[LGV_No].current == 271 || Main.m_stAGV[LGV_No].current == 289)
                        && Check_Minus_RRW1_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port == "CA7MRW01_UBP01")
                    {
                        Alloc_Work_Minus_RRW1 = 0;
                    }

                    //음극 롤 리와인더 #1 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    //음극 롤 리와인더 #2 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    if ((Main.m_stAGV[LGV_No].current == 271 || Main.m_stAGV[LGV_No].current == 289)
                        && Check_Minus_RRW1_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7MRW01_UBP01")
                    {
                        Alloc_Work_Minus_RRW1 = 1;
                    }

                    //음극 롤 리와인더 #2 출고대 작업이 있고,
                    //음극 롤 리와인더 #1 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. 
                    //음극 롤 리와인더 #2 입고대 작업이 끝난 상태이면 작업을 할당할 수 있게 한다. lkw20190617
                    if ((Main.m_stAGV[LGV_No].current == 271 || Main.m_stAGV[LGV_No].current == 289)
                        && Check_Minus_RRW2_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port == "CA7MRW02_UBP01")
                    {
                        Alloc_Work_Minus_RRW2 = 0;
                    }

                    //음극 롤 리와인더 #1 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    //음극 롤 리와인더 #2 입고대 작업이 끝난 상태에서 출고대 작업이 있으면 다른 작업은 무시한다.
                    if ((Main.m_stAGV[LGV_No].current == 271 || Main.m_stAGV[LGV_No].current == 289)
                        && Check_Minus_RRW2_Order == 1 && Main.m_stCommand[WaitCommandCount].Source_Port != "CA7MRW02_UBP01")
                    {
                        Alloc_Work_Minus_RRW2 = 1;
                    }

                    #endregion



                    for (int Work_Station_Count = 0; Work_Station_Count < Main.CS_Work_DB.Path_Count; Work_Station_Count++)
                    {
                        if (((Main.CS_Work_Path[Work_Station_Count].Work_Station == Main.m_stCommand[WaitCommandCount].Source_Port) && Main.m_stAGV[LGV_No].MCS_Carrier_ID == "")
                          || (Main.m_stCommand[WaitCommandCount].Source_Port == Main.m_stAGV[LGV_No].MCS_Vehicle_ID))
                        {
                            if ((Main.CS_AGV_C_Info[LGV_No].Type == Main.CS_Work_Path[Work_Station_Count].Type)
                            || (Main.m_stCommand[WaitCommandCount].Source_Port == Main.m_stAGV[LGV_No].MCS_Vehicle_ID)
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "기재" || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(음)" && (LGV_No == 5 || LGV_No == 6 || LGV_No == 7 || LGV_No == 8))
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(양)" && (LGV_No == 2 || LGV_No == 3 || LGV_No == 4 || LGV_No == 10))
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "REEL_공용" && (LGV_No == 0 || LGV_No == 1 || LGV_No == 9 || LGV_No == 11 || LGV_No == 12))))
                            {
                                Check_WorkType_Source = 1;
                            }
                        }
                        if ((Main.CS_Work_Path[Work_Station_Count].Work_Station == Main.m_stCommand[WaitCommandCount].Dest_Port))
                        {
                            if ((Main.CS_AGV_C_Info[LGV_No].Type == Main.CS_Work_Path[Work_Station_Count].Type)
                            || (Main.m_stCommand[WaitCommandCount].Source_Port == Main.m_stAGV[LGV_No].MCS_Vehicle_ID)
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "기재" || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(음)" && (LGV_No == 5 || LGV_No == 6 || LGV_No == 7 || LGV_No == 8))
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "ROLL_공용(양)" && (LGV_No == 2 || LGV_No == 3 || LGV_No == 4 || LGV_No == 10))
                            || (Main.CS_Work_Path[Work_Station_Count].Type == "REEL_공용" && (LGV_No == 0 || LGV_No == 1 || LGV_No == 9 || LGV_No == 11 || LGV_No == 12))))
                            {
                                Check_WorkType_Dest = 1;
                            }
                        }

                        if (Check_WorkType_Source == 1 && Check_WorkType_Dest == 1)
                        {
                            FLAG_Check_WorkType = 1;
                            break;
                        }
                    }

                    //출발지가 차량 번호일때는 팅구는 조건 무시
                    if (Main.m_stCommand[WaitCommandCount].Source_Port == Main.m_stAGV[LGV_No].MCS_Vehicle_ID && FLAG_Check_WorkType == 1
                     && Main.m_stCommand[WaitCommandCount].Alloc_State == "0"
                     && (Main.m_stAGV[LGV_No].state == 0 || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 5
                     || Main.m_stAGV[LGV_No].state == 6 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10))
                    {
                        Main.CS_AGV_Logic.Wait_Command_ID = Main.m_stCommand[WaitCommandCount].Command_ID;
                        Main.CS_AGV_Logic.Wait_Proiority = Main.m_stCommand[WaitCommandCount].Proiority;
                        Main.CS_AGV_Logic.Wait_Carrier_ID = Main.m_stCommand[WaitCommandCount].Carrier_ID;
                        Main.CS_AGV_Logic.Wait_Source_Port = Main.m_stCommand[WaitCommandCount].Source_Port;
                        Main.CS_AGV_Logic.Wait_Dest_Port = Main.m_stCommand[WaitCommandCount].Dest_Port;
                        Main.CS_AGV_Logic.Wait_Carrier_Type = Main.m_stCommand[WaitCommandCount].Carrier_Type;
                        Main.CS_AGV_Logic.Wait_Carrier_LOC = Main.m_stCommand[WaitCommandCount].Carrier_LOC;
                        Main.CS_AGV_Logic.Wait_Process_ID = Main.m_stCommand[WaitCommandCount].Process_ID;

                        Main.CS_AGV_Logic.Wait_Batch_ID = Main.m_stCommand[WaitCommandCount].Batch_ID;
                        Main.CS_AGV_Logic.Wait_LOT_ID = Main.m_stCommand[WaitCommandCount].LOT_ID;
                        Main.CS_AGV_Logic.Wait_Carrier_S_Count = Main.m_stCommand[WaitCommandCount].Carrier_S_Count;
                        Main.CS_AGV_Logic.Wait_Alloc_State = Main.m_stCommand[WaitCommandCount].Alloc_State;
                        Main.CS_AGV_Logic.Wait_Transfer_State = Main.m_stCommand[WaitCommandCount].Transfer_State;
                        Main.CS_AGV_Logic.Wait_LGV_No = Main.m_stCommand[WaitCommandCount].LGV_No;
                        Main.CS_AGV_Logic.Wait_Call_Time = Main.m_stCommand[WaitCommandCount].Call_Time;
                        Main.CS_AGV_Logic.Wait_Alloc_Time = Main.m_stCommand[WaitCommandCount].Alloc_Time;
                        Main.CS_AGV_Logic.Wait_Complete_Time = Main.m_stCommand[WaitCommandCount].Complete_Time;
                        Main.CS_AGV_Logic.Wait_Quantity = Main.m_stCommand[WaitCommandCount].Quantity;
                        break;
                    }
                    //일반
                    else if (FLAG_Check_WorkType == 1 && Alloc_AGV_166 == 0 && Alloc_AGV_163 == 0 && Alloc_AGV_Charge == 0 && Alloc_UP_Lift_Carrier == 0 && Alloc_Over_Lift_Order == 0 && Alloc_PackingRoom_Order == 0 && Alloc_Over_Buffer_Order == 0
                     && Alloc_Reel_Charge_Station_1438 == 0 && Alloc_Reel_Charge_Station_438 == 0 && Alloc_Reel_Charge_Station_496 == 0 && Alloc_Floor3_Down_Lift == 0 && Alloc_Plus_SLT3 == 0 && Alloc_3_Up_Lift == 0 && Alloc_3SLT_LGV == 0
                     && Alloc_Work_Plus_SLT1 == 0 && Alloc_Work_Plus_SLT2 == 0 && Alloc_Work_Plus_SLT3 == 0 && Alloc_Work_Minus_SLT1 == 0 && Alloc_Work_Minus_SLT2 == 0 && Alloc_Work_Minus_SLT3 == 0 && Alloc_1369_NoWork == 0
                     && Alloc_AGV_1237 == 0 && Alloc_AGV_1041 == 0 && Alloc_AGV_Charge_Plus == 0 && Alloc_Entry_LGV == 0 && Alloc_Entry_Plus_Roll == 0 && FLAG_Alloc_Lift_Order == 0 && Alloc_PLUS_SLT_1 == 0
                     && Alloc_Floor3_Up_Lift == 0 && Alloc_Entry_Minus_Roll == 0 && Alloc_23SLT_LGV == 0 && Alloc_1156_NoWork == 0 && Alloc_Plus_Roll_InPut1 == 0 && Alloc_Plus_Roll_InPut2 == 0 && Alloc_Minus_Roll_InPut1 == 0 && Alloc_Minus_Roll_InPut2 == 0
                     && Main.m_stCommand[WaitCommandCount].Alloc_State == "0" && Alloc_Plus_Work_Node == 0 && Alloc_Charge_Station_M_Roll == 0 && Alloc_Charge_Station_P_Roll == 0
                     && Alloc_Work_Plus_RRW1 == 0 && Alloc_Work_Minus_RRW1 == 0 && Alloc_Work_Minus_RRW2 == 0
                     && (Main.m_stAGV[LGV_No].state == 0 || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 5
                     || Main.m_stAGV[LGV_No].state == 6 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10) && Main.M_7_SLT_Normal == true)
                    {
                        Main.CS_AGV_Logic.Wait_Command_ID = Main.m_stCommand[WaitCommandCount].Command_ID;
                        Main.CS_AGV_Logic.Wait_Proiority = Main.m_stCommand[WaitCommandCount].Proiority;
                        Main.CS_AGV_Logic.Wait_Carrier_ID = Main.m_stCommand[WaitCommandCount].Carrier_ID;
                        Main.CS_AGV_Logic.Wait_Source_Port = Main.m_stCommand[WaitCommandCount].Source_Port;
                        Main.CS_AGV_Logic.Wait_Dest_Port = Main.m_stCommand[WaitCommandCount].Dest_Port;
                        Main.CS_AGV_Logic.Wait_Carrier_Type = Main.m_stCommand[WaitCommandCount].Carrier_Type;
                        Main.CS_AGV_Logic.Wait_Carrier_LOC = Main.m_stCommand[WaitCommandCount].Carrier_LOC;
                        Main.CS_AGV_Logic.Wait_Process_ID = Main.m_stCommand[WaitCommandCount].Process_ID;
                        Main.CS_AGV_Logic.Wait_Batch_ID = Main.m_stCommand[WaitCommandCount].Batch_ID;
                        Main.CS_AGV_Logic.Wait_LOT_ID = Main.m_stCommand[WaitCommandCount].LOT_ID;
                        Main.CS_AGV_Logic.Wait_Carrier_S_Count = Main.m_stCommand[WaitCommandCount].Carrier_S_Count;
                        Main.CS_AGV_Logic.Wait_Alloc_State = Main.m_stCommand[WaitCommandCount].Alloc_State;
                        Main.CS_AGV_Logic.Wait_Transfer_State = Main.m_stCommand[WaitCommandCount].Transfer_State;
                        Main.CS_AGV_Logic.Wait_LGV_No = Main.m_stCommand[WaitCommandCount].LGV_No;
                        Main.CS_AGV_Logic.Wait_Call_Time = Main.m_stCommand[WaitCommandCount].Call_Time;
                        Main.CS_AGV_Logic.Wait_Alloc_Time = Main.m_stCommand[WaitCommandCount].Alloc_Time;
                        Main.CS_AGV_Logic.Wait_Complete_Time = Main.m_stCommand[WaitCommandCount].Complete_Time;
                        Main.CS_AGV_Logic.Wait_Quantity = Main.m_stCommand[WaitCommandCount].Quantity;
                        break;
                    }
                    //슬리터 우선
                    else if (((Alloc_AGV_166 == 0 && Alloc_AGV_163 == 0 && Alloc_AGV_Charge == 0 && Alloc_UP_Lift_Carrier == 0 && Alloc_Over_Lift_Order == 0 && Alloc_PackingRoom_Order == 0 && Alloc_Over_Buffer_Order == 0
                     && Alloc_Reel_Charge_Station_1438 == 0 && Alloc_Reel_Charge_Station_438 == 0 && Alloc_Reel_Charge_Station_496 == 0 && Alloc_Floor3_Down_Lift == 0 && Alloc_Plus_SLT3 == 0 && Alloc_3_Up_Lift == 0 && Alloc_3SLT_LGV == 0
                     && Alloc_Work_Plus_SLT1 == 0 && Alloc_Work_Plus_SLT2 == 0 && Alloc_Work_Plus_SLT3 == 0 && Alloc_Work_Minus_SLT1 == 0 && Alloc_Work_Minus_SLT2 == 0 && Alloc_Work_Minus_SLT3 == 0 && Alloc_1369_NoWork == 0
                     && Alloc_AGV_1237 == 0 && Alloc_AGV_1041 == 0 && Alloc_AGV_Charge_Plus == 0 && Alloc_Entry_LGV == 0 && Alloc_Entry_Plus_Roll == 0 && FLAG_Alloc_Lift_Order == 0 && Alloc_PLUS_SLT_1 == 0
                     && Alloc_Floor3_Up_Lift == 0 && Alloc_Entry_Minus_Roll == 0 && Alloc_23SLT_LGV == 0 && Alloc_1156_NoWork == 0 && Alloc_Plus_Roll_InPut1 == 0 && Alloc_Plus_Roll_InPut2 == 0 && Alloc_Minus_Roll_InPut1 == 0 && Alloc_Minus_Roll_InPut2 == 0
                     && Alloc_Plus_Work_Node == 0 && Alloc_Charge_Station_M_Roll == 0 && Alloc_Charge_Station_P_Roll == 0
                     && Alloc_Work_Plus_RRW1 == 0 && Alloc_Work_Minus_RRW1 == 0 && Alloc_Work_Minus_RRW2 == 0 && Check_7SLT_ORDER == 0)

                     || (Check_7SLT_ORDER == 1 && Alloc_UP_Lift_Carrier == 0 && Alloc_Floor3_Up_Lift == 0 && Alloc_7SLT == 0))

                     && (Main.m_stAGV[LGV_No].state == 0 || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 5
                     || Main.m_stAGV[LGV_No].state == 6 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10) && Main.M_7_SLT_First_Mode == true
                     && FLAG_Check_WorkType == 1 && Main.m_stCommand[WaitCommandCount].Alloc_State == "0")
                    {
                        Main.CS_AGV_Logic.Wait_Command_ID = Main.m_stCommand[WaitCommandCount].Command_ID;
                        Main.CS_AGV_Logic.Wait_Proiority = Main.m_stCommand[WaitCommandCount].Proiority;
                        Main.CS_AGV_Logic.Wait_Carrier_ID = Main.m_stCommand[WaitCommandCount].Carrier_ID;
                        Main.CS_AGV_Logic.Wait_Source_Port = Main.m_stCommand[WaitCommandCount].Source_Port;
                        Main.CS_AGV_Logic.Wait_Dest_Port = Main.m_stCommand[WaitCommandCount].Dest_Port;
                        Main.CS_AGV_Logic.Wait_Carrier_Type = Main.m_stCommand[WaitCommandCount].Carrier_Type;
                        Main.CS_AGV_Logic.Wait_Carrier_LOC = Main.m_stCommand[WaitCommandCount].Carrier_LOC;
                        Main.CS_AGV_Logic.Wait_Process_ID = Main.m_stCommand[WaitCommandCount].Process_ID;
                        Main.CS_AGV_Logic.Wait_Batch_ID = Main.m_stCommand[WaitCommandCount].Batch_ID;
                        Main.CS_AGV_Logic.Wait_LOT_ID = Main.m_stCommand[WaitCommandCount].LOT_ID;
                        Main.CS_AGV_Logic.Wait_Carrier_S_Count = Main.m_stCommand[WaitCommandCount].Carrier_S_Count;
                        Main.CS_AGV_Logic.Wait_Alloc_State = Main.m_stCommand[WaitCommandCount].Alloc_State;
                        Main.CS_AGV_Logic.Wait_Transfer_State = Main.m_stCommand[WaitCommandCount].Transfer_State;
                        Main.CS_AGV_Logic.Wait_LGV_No = Main.m_stCommand[WaitCommandCount].LGV_No;
                        Main.CS_AGV_Logic.Wait_Call_Time = Main.m_stCommand[WaitCommandCount].Call_Time;
                        Main.CS_AGV_Logic.Wait_Alloc_Time = Main.m_stCommand[WaitCommandCount].Alloc_Time;
                        Main.CS_AGV_Logic.Wait_Complete_Time = Main.m_stCommand[WaitCommandCount].Complete_Time;
                        Main.CS_AGV_Logic.Wait_Quantity = Main.m_stCommand[WaitCommandCount].Quantity;
                        break;
                    }
                    //슬리터 우선(음극라인만)
                    else if (((Alloc_AGV_166 == 0 && Alloc_AGV_163 == 0 && Alloc_AGV_Charge == 0 && Alloc_UP_Lift_Carrier == 0 && Alloc_Over_Lift_Order == 0 && Alloc_PackingRoom_Order == 0 && Alloc_Over_Buffer_Order == 0
                     && Alloc_Reel_Charge_Station_1438 == 0 && Alloc_Reel_Charge_Station_438 == 0 && Alloc_Reel_Charge_Station_496 == 0 && Alloc_Floor3_Down_Lift == 0 && Alloc_Plus_SLT3 == 0 && Alloc_3_Up_Lift == 0 && Alloc_3SLT_LGV == 0
                     && Alloc_Work_Plus_SLT1 == 0 && Alloc_Work_Plus_SLT2 == 0 && Alloc_Work_Plus_SLT3 == 0 && Alloc_Work_Minus_SLT1 == 0 && Alloc_Work_Minus_SLT2 == 0 && Alloc_Work_Minus_SLT3 == 0 && Alloc_1369_NoWork == 0
                     && Alloc_AGV_1237 == 0 && Alloc_AGV_1041 == 0 && Alloc_AGV_Charge_Plus == 0 && Alloc_Entry_LGV == 0 && Alloc_Entry_Plus_Roll == 0 && FLAG_Alloc_Lift_Order == 0 && Alloc_PLUS_SLT_1 == 0
                     && Alloc_Floor3_Up_Lift == 0 && Alloc_Entry_Minus_Roll == 0 && Alloc_23SLT_LGV == 0 && Alloc_1156_NoWork == 0 && Alloc_Plus_Roll_InPut1 == 0 && Alloc_Plus_Roll_InPut2 == 0 && Alloc_Minus_Roll_InPut1 == 0 && Alloc_Minus_Roll_InPut2 == 0
                     && Alloc_Plus_Work_Node == 0 && Alloc_Charge_Station_M_Roll == 0 && Alloc_Charge_Station_P_Roll == 0
                     && Alloc_Work_Plus_RRW1 == 0 && Alloc_Work_Minus_RRW1 == 0 && Alloc_Work_Minus_RRW2 == 0 && Check_7SLT_ORDER == 0)

                     || (Check_7SLT_ORDER == 1 && Alloc_UP_Lift_Carrier == 0 && Alloc_Floor3_Up_Lift == 0 && Alloc_7SLT == 0))

                     && (Main.m_stAGV[LGV_No].state == 0 || Main.m_stAGV[LGV_No].state == 4 || Main.m_stAGV[LGV_No].state == 8 || Main.m_stAGV[LGV_No].state == 5
                     || Main.m_stAGV[LGV_No].state == 6 || Main.m_stAGV[LGV_No].state == 9 || Main.m_stAGV[LGV_No].state == 10) && Main.M_7_SLT_Minus_First_Mode == true
                     && FLAG_Check_WorkType == 1 && Main.m_stCommand[WaitCommandCount].Alloc_State == "0")
                    {
                        Main.CS_AGV_Logic.Wait_Command_ID = Main.m_stCommand[WaitCommandCount].Command_ID;
                        Main.CS_AGV_Logic.Wait_Proiority = Main.m_stCommand[WaitCommandCount].Proiority;
                        Main.CS_AGV_Logic.Wait_Carrier_ID = Main.m_stCommand[WaitCommandCount].Carrier_ID;
                        Main.CS_AGV_Logic.Wait_Source_Port = Main.m_stCommand[WaitCommandCount].Source_Port;
                        Main.CS_AGV_Logic.Wait_Dest_Port = Main.m_stCommand[WaitCommandCount].Dest_Port;
                        Main.CS_AGV_Logic.Wait_Carrier_Type = Main.m_stCommand[WaitCommandCount].Carrier_Type;
                        Main.CS_AGV_Logic.Wait_Carrier_LOC = Main.m_stCommand[WaitCommandCount].Carrier_LOC;
                        Main.CS_AGV_Logic.Wait_Process_ID = Main.m_stCommand[WaitCommandCount].Process_ID;
                        Main.CS_AGV_Logic.Wait_Batch_ID = Main.m_stCommand[WaitCommandCount].Batch_ID;
                        Main.CS_AGV_Logic.Wait_LOT_ID = Main.m_stCommand[WaitCommandCount].LOT_ID;
                        Main.CS_AGV_Logic.Wait_Carrier_S_Count = Main.m_stCommand[WaitCommandCount].Carrier_S_Count;
                        Main.CS_AGV_Logic.Wait_Alloc_State = Main.m_stCommand[WaitCommandCount].Alloc_State;
                        Main.CS_AGV_Logic.Wait_Transfer_State = Main.m_stCommand[WaitCommandCount].Transfer_State;
                        Main.CS_AGV_Logic.Wait_LGV_No = Main.m_stCommand[WaitCommandCount].LGV_No;
                        Main.CS_AGV_Logic.Wait_Call_Time = Main.m_stCommand[WaitCommandCount].Call_Time;
                        Main.CS_AGV_Logic.Wait_Alloc_Time = Main.m_stCommand[WaitCommandCount].Alloc_Time;
                        Main.CS_AGV_Logic.Wait_Complete_Time = Main.m_stCommand[WaitCommandCount].Complete_Time;
                        Main.CS_AGV_Logic.Wait_Quantity = Main.m_stCommand[WaitCommandCount].Quantity;
                        break;
                    }
                }
            }
        }
    }
}
