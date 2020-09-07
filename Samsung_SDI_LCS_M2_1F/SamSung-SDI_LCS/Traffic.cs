using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class Traffic
    {
        Form1 Main;
        int[] Ather_AGV_Count = new int[Form1.LGV_NUM];
        int[] Ather_ROLL_AGV_Count = new int[Form1.LGV_NUM];
        public int[] otherAGV_Exist = new int[Form1.LGV_NUM];
        int GO = 1;
        int STOP = 2;
        public int Traffic_Area_No1 = -1;
        public int[] Area_Exist = new int[Form1.LGV_NUM];

        int[] I_C_USE_Ok = new int[Form1.LGV_NUM];
        int[] I_G_USE_Ok = new int[Form1.LGV_NUM];


        int[] U_C_USE_Ok = new int[Form1.LGV_NUM];
        int[] U_G_USE_Ok = new int[Form1.LGV_NUM];

        //11.15 최종목적지 트래픽 추가
        int[] I_C_USE_Ok_Dest = new int[Form1.LGV_NUM];
        int[] I_G_USE_Ok_Dest = new int[Form1.LGV_NUM];

        int[] U_C_USE_Ok_Dest = new int[Form1.LGV_NUM];
        int[] U_G_USE_Ok_Dest = new int[Form1.LGV_NUM];

        int[] Cell_On = new int[Form1.LGV_NUM];

        public bool[] Use_Traffic = new bool[300];

        public int My_Current = 0;
        public int Other_Current = 0;
        int Traffic_Node_Count = 300;

        //생성자
        public Traffic()
        {

        }
        //생성자
        public Traffic(Form1 CS_Main)
        {
            Main = CS_Main;
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                Ather_ROLL_AGV_Count[i] = 0;
                Ather_AGV_Count[i] = 0;
                Area_Exist[i] = 0;
                otherAGV_Exist[i] = 0;
                I_C_USE_Ok[i] = 0;
                I_G_USE_Ok[i] = 0;
                U_C_USE_Ok[i] = 0;
                U_G_USE_Ok[i] = 0;

                I_C_USE_Ok_Dest[i] = 0;
                I_G_USE_Ok_Dest[i] = 0;
                U_C_USE_Ok_Dest[i] = 0;
                U_G_USE_Ok_Dest[i] = 0;
                Cell_On[i] = 0;
            }
            for (int i = 0; i < Traffic_Node_Count; i++)
            {
                Use_Traffic[i] = new bool();
            }
        }

        //트래픽 조건 사용 유무 찾아내기
        public void Check_Use_Traffic(int idx)
        {
            I_C_USE_Ok[idx] = 0;
            I_G_USE_Ok[idx] = 0;

            U_C_USE_Ok[idx] = 0;
            U_G_USE_Ok[idx] = 0;

            I_C_USE_Ok_Dest[idx] = 0;
            I_G_USE_Ok_Dest[idx] = 0;

            U_C_USE_Ok_Dest[idx] = 0;
            U_G_USE_Ok_Dest[idx] = 0;

            Cell_On[idx] = 0;

            for (int Traffic = 0; Traffic < Main.Form_Traffic_Cell.Traffic_Cell.Count; Traffic++)
            {
                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    //if (Main.m_stAGV[idx].connect == 0) continue; //접속 안된놈 
                    if (idx == i) continue;
                    if (Main.m_stAGV[idx].current == Main.m_stAGV[i].current) continue;

                    //정방향 칸트래픽
                    if (Main.m_stTraffic_Cell[Traffic].Way == "정방향" && Main.m_stAGV[idx].current >= Main.m_stTraffic_Cell[Traffic].Start_Index && Main.m_stAGV[idx].current <= Main.m_stTraffic_Cell[Traffic].End_Index)
                    {
                        if (Main.m_stAGV[i].current >= Main.m_stAGV[idx].current && Main.m_stAGV[i].current <= Main.m_stAGV[idx].current + Main.m_stTraffic_Cell[Traffic].Cell_Count)
                        {
                            Cell_On[idx] = 1;
                        }
                    }

                    //역방향 칸트래픽
                    if (Main.m_stTraffic_Cell[Traffic].Way == "역방향" && Main.m_stAGV[idx].current <= Main.m_stTraffic_Cell[Traffic].Start_Index && Main.m_stAGV[idx].current >= Main.m_stTraffic_Cell[Traffic].End_Index)
                    {
                        if (Main.m_stAGV[i].current <= Main.m_stAGV[idx].current && Main.m_stAGV[i].current >= Main.m_stAGV[idx].current - Main.m_stTraffic_Cell[Traffic].Cell_Count)
                        {
                            Cell_On[idx] = 1;
                        }
                    }

                }

            }
            int ChkTraffic = 0;
            //현재차량
            for (int i = 0; i < Main.Form_Traffic_Basic.Traffic_Node.Count; i++)
            {
                ChkTraffic = 0;
                 
                //if (Main.m_stAGV[idx].connect == 0) continue; //접속 안된놈 

                #region

                //현재 차량 위치 트래픽 사용 할때
                if (Main.m_stTraffic[i].idx_Current_Use == "사용")
                {
                    if (Main.m_stTraffic[i].idx_Start_Current != "" && Main.m_stTraffic[i].idx_End_Current != ""
                    && (Main.m_stAGV[idx].current >= Convert.ToInt32(Main.m_stTraffic[i].idx_Start_Current))
                    && (Main.m_stAGV[idx].current <= Convert.ToInt32(Main.m_stTraffic[i].idx_End_Current)))
                    {
                        //  I_C_USE_Ok[idx] = 1;

                        ChkTraffic++;
                    }
                }

                //현재 차량 목적지 트래픽 사용 할때
                if (Main.m_stTraffic[i].idx_Current_Use == "사용" && Main.m_stTraffic[i].idx_Goal_Use == "사용")
                {
                    if (Main.m_stTraffic[i].idx_Start_Goal != "" && Main.m_stTraffic[i].idx_End_Goal != ""
                    && (Main.m_stAGV[idx].Goal >= Convert.ToInt32(Main.m_stTraffic[i].idx_Start_Goal))
                    && (Main.m_stAGV[idx].Goal <= Convert.ToInt32(Main.m_stTraffic[i].idx_End_Goal)))
                    {
                        //   I_G_USE_Ok[idx] = 1;
                        ChkTraffic++;

                    }
                }

                else if (Main.m_stTraffic[i].idx_Current_Use == "사용" && Main.m_stTraffic[i].idx_Goal_Use == "미사용")
                {
                    // I_G_USE_Ok[idx] = 1;
                    ChkTraffic++;

                }

                int ChkOtherAGV = 0;
                for (int other_LGV = 0; other_LGV < Form1.LGV_NUM; other_LGV++)
                {
                    ChkOtherAGV = 0;

                    if (idx == other_LGV) continue;

                    //다른 차량 위치 트래픽 사용 할때
                    if (Main.m_stTraffic[i].i_Current_Use == "사용")
                    {
                        if (Main.m_stTraffic[i].i_Start_Current != "" && Main.m_stTraffic[i].i_End_Current != ""
                        && (Main.m_stAGV[other_LGV].current >= Convert.ToInt32(Main.m_stTraffic[i].i_Start_Current))
                        && (Main.m_stAGV[other_LGV].current <= Convert.ToInt32(Main.m_stTraffic[i].i_End_Current)))
                        {
                            // U_C_USE_Ok[idx] = 1;
                            //ChkTraffic++;
                            ChkOtherAGV++;
                        }
                    }

                    //다른 차량 목적지 트래픽 사용 할때
                    if (Main.m_stTraffic[i].i_Current_Use == "사용" && Main.m_stTraffic[i].i_Goal_Use == "사용")
                    {
                        if (Main.m_stTraffic[i].i_Start_Goal != "" && Main.m_stTraffic[i].i_End_Goal != ""
                        && (Main.m_stAGV[other_LGV].Goal >= Convert.ToInt32(Main.m_stTraffic[i].i_Start_Goal))
                        && (Main.m_stAGV[other_LGV].Goal <= Convert.ToInt32(Main.m_stTraffic[i].i_End_Goal)))
                        {
                            //  U_G_USE_Ok[idx] = 1;
                            ChkOtherAGV++;
                        }
                    }
                    else if (Main.m_stTraffic[i].i_Current_Use == "사용" && Main.m_stTraffic[i].i_Goal_Use == "미사용")
                    {
                        // U_G_USE_Ok[idx] = 1;
                        ChkOtherAGV++;
                    }

                    if (ChkOtherAGV >= 2)
                    {
                        ChkTraffic++;
                        break;
                    }

                    ChkOtherAGV = 0;


                }
                #endregion



                if (ChkTraffic >= 3)
                {
                    I_C_USE_Ok[idx] = 1;
                    I_G_USE_Ok[idx] = 1;

                    U_C_USE_Ok[idx] = 1;
                    U_G_USE_Ok[idx] = 1;


                }

                ChkTraffic = 0;
            }

            int ChkTraffic_Dest = 0;
            //최종 목적지 트래픽
            for (int i = 0; i < Main.Form_Traffic_Dest.Traffic_Node.Count; i++)
            {
                ChkTraffic_Dest = 0;

                //if (Main.m_stAGV[idx].connect == 0) continue; //접속 안된놈 

                #region

                //현재 차량 위치 트래픽 사용 할때
                if (Main.m_stTraffic_Dest[i].idx_Current_Use == "사용")
                {
                    if (Main.m_stTraffic_Dest[i].idx_Start_Current != "" && Main.m_stTraffic_Dest[i].idx_End_Current != ""
                    && (Main.m_stAGV[idx].current >= Convert.ToInt32(Main.m_stTraffic_Dest[i].idx_Start_Current))
                    && (Main.m_stAGV[idx].current <= Convert.ToInt32(Main.m_stTraffic_Dest[i].idx_End_Current)))
                    {
                        //  I_C_USE_Ok[idx] = 1;

                        ChkTraffic_Dest++;
                    }
                }

                //현재 차량 목적지 트래픽 사용 할때
                if (Main.m_stTraffic_Dest[i].idx_Current_Use == "사용" && Main.m_stTraffic_Dest[i].idx_Dest_Use == "사용")
                {
                    if (Main.m_stAGV[idx].MCS_Carrier_ID == "")
                    {
                        if (Main.m_stTraffic_Dest[i].idx_Dest != "" && Main.m_stTraffic_Dest[i].idx_Dest != ""
                        && (Main.m_stAGV[idx].Source_Port_Num == Main.m_stTraffic_Dest[i].idx_Dest))
                        {
                            //I_G_USE_Ok[idx] = 1;
                            ChkTraffic_Dest++;

                        }
                    }
                    else if (Main.m_stAGV[idx].MCS_Carrier_ID != "")
                    {
                        if (Main.m_stTraffic_Dest[i].idx_Dest != "" && Main.m_stTraffic_Dest[i].idx_Dest != ""
                        && (Main.m_stAGV[idx].Dest_Port_Num == Main.m_stTraffic_Dest[i].idx_Dest))
                        {
                            //   I_G_USE_Ok[idx] = 1;
                            ChkTraffic_Dest++;

                        }
                    }

                }
                else if (Main.m_stTraffic_Dest[i].idx_Current_Use == "사용" && Main.m_stTraffic_Dest[i].idx_Dest_Use == "미사용")
                {
                    // I_G_USE_Ok[idx] = 1;
                    ChkTraffic_Dest++;

                }

                int ChkOtherAGV = 0;
                for (int other_LGV = 0; other_LGV < Form1.LGV_NUM; other_LGV++)
                {
                    ChkOtherAGV = 0;

                    if (idx == other_LGV) continue;
                    //if (Main.m_stAGV[other_LGV].connect == 0) continue; //접속 안된놈 제외

                    //다른 차량 위치 트래픽 사용 할때
                    if (Main.m_stTraffic_Dest[i].i_Current_Use == "사용")
                    {
                        if (Main.m_stTraffic_Dest[i].i_Start_Current != "" && Main.m_stTraffic_Dest[i].i_End_Current != ""
                        && (Main.m_stAGV[other_LGV].current >= Convert.ToInt32(Main.m_stTraffic_Dest[i].i_Start_Current))
                        && (Main.m_stAGV[other_LGV].current <= Convert.ToInt32(Main.m_stTraffic_Dest[i].i_End_Current)))
                        {
                            // U_C_USE_Ok[idx] = 1;
                            //ChkTraffic_Dest++;
                            ChkOtherAGV++;
                        }
                    }

                    //다른 차량 목적지 트래픽 사용 할때
                    if (Main.m_stTraffic_Dest[i].i_Current_Use == "사용" && Main.m_stTraffic_Dest[i].i_Goal_Use == "사용")
                    {
                        if (Main.m_stTraffic_Dest[i].i_Start_Goal != "" && Main.m_stTraffic_Dest[i].i_End_Goal != ""
                        && (Main.m_stAGV[other_LGV].Goal >= Convert.ToInt32(Main.m_stTraffic_Dest[i].i_Start_Goal))
                        && (Main.m_stAGV[other_LGV].Goal <= Convert.ToInt32(Main.m_stTraffic_Dest[i].i_End_Goal)))
                        {
                            //  U_G_USE_Ok[idx] = 1;
                            ChkOtherAGV++;
                        }
                    }
                    else if (Main.m_stTraffic_Dest[i].i_Current_Use == "사용" && Main.m_stTraffic_Dest[i].i_Goal_Use == "미사용")
                    {
                        // U_G_USE_Ok[idx] = 1;
                        ChkOtherAGV++;
                    }

                    if (ChkOtherAGV >= 2)
                    {
                        ChkTraffic_Dest++;
                        break;
                    }

                    ChkOtherAGV = 0;


                }
                #endregion


                if (ChkTraffic_Dest >= 3)
                {
                    I_C_USE_Ok_Dest[idx] = 1;
                    I_G_USE_Ok_Dest[idx] = 1;

                    U_C_USE_Ok_Dest[idx] = 1;
                    U_G_USE_Ok_Dest[idx] = 1;


                }

                ChkTraffic_Dest = 0;
            }
        }

        //20200804 MDH 충전소 이설
        private void Check_ChargeDepot_163(int idx, int i)
        {
            int y_bound = 52000; //충전소에서 트래픽정지 가능한 범위지정 : y경계

            //충전소 163 차량
            if ((Main.m_stAGV[idx].current >= 162 && Main.m_stAGV[idx].current <= 163) && Main.m_stAGV[idx].Goal != 163)
            {
                if (Main.m_stAGV[i].current >= 46 && Main.m_stAGV[i].current <= 52)
                {
                    if (Main.m_stAGV[idx].y >= y_bound)
                    {
                        otherAGV_Exist[idx] = 1; //대상차량 : 트래픽
                        //zzotherAGV_Exist[i] = 0; //상대차량 : 주행
                    }
                    else
                    {
                        otherAGV_Exist[idx] = 0; //대상차량 : 주행
                        //otherAGV_Exist[i] = 1; //상대차량 : 트래픽
                    }
                }  
            } 

            //46-50 트래픽구간 차량
            if (Main.m_stAGV[idx].current >= 46 && Main.m_stAGV[idx].current <= 52)
            {
                if ((Main.m_stAGV[i].current >= 162 && Main.m_stAGV[i].current <= 163) && Main.m_stAGV[i].Goal != 163)
                {
                    if (Main.m_stAGV[i].y >= y_bound)
                    {
                        otherAGV_Exist[idx] = 0; //대상차량 : 주행
                    }
                    else
                    {
                        otherAGV_Exist[idx] = 1; //대상차량 : 트래픽
                    }
                }
            }
        }

        //20200804 MDH 충전소 이설
        private void Check_ChargeDepot_166(int idx, int i)
        {
            int y_bound = 53000; //충전소에서 트래픽정지 가능한 범위지정 : y경계

            //충전소 166 차량
            if ((Main.m_stAGV[idx].current >= 166 && Main.m_stAGV[idx].current <= 167) && Main.m_stAGV[idx].Goal != 166)
            {
                if (Main.m_stAGV[i].current >= 67 && Main.m_stAGV[i].current <= 75)
                {
                    if (Main.m_stAGV[idx].y <= y_bound)
                    {
                        otherAGV_Exist[idx] = 1; //대상차량 : 트래픽
                        //otherAGV_Exist[i] = 0; //상대차량 : 주행  (i로 지정할 경우 <-출발,정지 반복함)
                    }
                    else
                    {
                        otherAGV_Exist[idx] = 0; //대상차량 : 주행
                        //otherAGV_Exist[i] = 1; //상대차량 : 트래픽
                    }
                }     
            }

            //67-70 트래픽구간 차량
            if (Main.m_stAGV[idx].current >= 67 && Main.m_stAGV[idx].current <= 70)
            {
                if ((Main.m_stAGV[i].current >= 166 && Main.m_stAGV[i].current <= 167) && Main.m_stAGV[i].Goal != 166)
                {
                    if (Main.m_stAGV[i].y <= y_bound)
                    {
                        otherAGV_Exist[idx] = 0; //대상차량 : 주행
                    }
                    else
                    {
                        otherAGV_Exist[idx] = 1; //대상차량 : 트래픽
                    }
                }
            }
        }

        public void Move_Traffic(int idx)
        {
            try
            {
                otherAGV_Exist[idx] = 0;

                I_C_USE_Ok[idx] = 0;
                I_G_USE_Ok[idx] = 0;

                U_C_USE_Ok[idx] = 0;
                U_G_USE_Ok[idx] = 0;

                Cell_On[idx] = 0;
                Check_Use_Traffic(idx);
                int FLAG_SKIP_TRAFFIC = 0;

                //최종 목적지 트래픽 추가
                if (I_C_USE_Ok_Dest[idx] == 1 && I_G_USE_Ok_Dest[idx] == 1
                    && U_C_USE_Ok_Dest[idx] == 1 && U_G_USE_Ok_Dest[idx] == 1) // 엑셀 트레픽
                {
                    otherAGV_Exist[idx] = 1;
                }

                //기본 트래픽
                if (I_C_USE_Ok[idx] == 1 && I_G_USE_Ok[idx] == 1
                    && U_C_USE_Ok[idx] == 1 && U_G_USE_Ok[idx] == 1) // 엑셀 트레픽
                {
                    #region 트래픽 스킵
                    if ((Main.m_stAGV[idx].current == 1075 || Main.m_stAGV[idx].current == 1074)
                   && (Main.m_stAGV[idx].Goal == 1075 || Main.m_stAGV[idx].Goal == 1451 || Main.m_stAGV[idx].Goal == 1238 || Main.m_stAGV[idx].Goal == 1237
                    || Main.m_stAGV[idx].Goal == 1231 || Main.m_stAGV[idx].Goal == 1460))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }
                    if ((Main.m_stAGV[idx].current >= 141 && Main.m_stAGV[idx].current <= 142)
                   && (Main.m_stAGV[idx].Goal >= 197 && Main.m_stAGV[idx].Goal <= 221))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }
                    if ((Main.m_stAGV[idx].current >= 1174 && Main.m_stAGV[idx].current <= 1175)
                   && (Main.m_stAGV[idx].Goal >= 197 && Main.m_stAGV[idx].Goal <= 221))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1053 && Main.m_stAGV[idx].current <= 1054)
                   && (Main.m_stAGV[idx].Goal >= 1424 && Main.m_stAGV[idx].Goal <= 1427))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }
                    if ((Main.m_stAGV[idx].current >= 1284 && Main.m_stAGV[idx].current <= 1285)
                   && (Main.m_stAGV[idx].Goal >= 1286 && Main.m_stAGV[idx].Goal <= 1286))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1053 && Main.m_stAGV[idx].current <= 1054)
                   && ((Main.m_stAGV[idx].Goal >= 1 && Main.m_stAGV[idx].Goal <= 999) || Main.m_stAGV[idx].Goal == 1054))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1174 && Main.m_stAGV[idx].current <= 1175)
                   && ((Main.m_stAGV[idx].Goal >= 1208 && Main.m_stAGV[idx].Goal <= 1221)
                    || (Main.m_stAGV[idx].Goal >= 1178 && Main.m_stAGV[idx].Goal <= 1178)))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1080 && Main.m_stAGV[idx].current <= 1080)
                   && ((Main.m_stAGV[idx].Goal >= 1054 && Main.m_stAGV[idx].Goal <= 1054)
                    || (Main.m_stAGV[idx].Goal >= 1 && Main.m_stAGV[idx].Goal <= 999)))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1060 && Main.m_stAGV[idx].current <= 1060) && (idx == 2 || idx == 3 || idx == 4))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1080 && Main.m_stAGV[idx].current <= 1080)
                   && ((Main.m_stAGV[idx].Goal >= 1054 && Main.m_stAGV[idx].Goal <= 1054)
                    || (Main.m_stAGV[idx].Goal >= 1 && Main.m_stAGV[idx].Goal <= 999)))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1235 && Main.m_stAGV[idx].current <= 1236)
                   && (Main.m_stAGV[idx].Goal >= 1469 && Main.m_stAGV[idx].Goal <= 1469)
                   && (Main.m_stAGV[idx].Goal >= 1475 && Main.m_stAGV[idx].Goal <= 1475)
                   && (Main.m_stAGV[idx].Goal >= 1236 && Main.m_stAGV[idx].Goal <= 1238))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1026 && Main.m_stAGV[idx].current <= 1028)
                   && (Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1026))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 173)
                   && (Main.m_stAGV[idx].Goal <= 1000 && Main.m_stAGV[idx].Goal != 183))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 233 && Main.m_stAGV[idx].current <= 235)
                    && ((Main.m_stAGV[idx].Goal >= 236 && Main.m_stAGV[idx].Goal <= 237) || (Main.m_stAGV[idx].Goal >= 300 && Main.m_stAGV[idx].Goal <= 378)))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }

                    if ((Main.m_stAGV[idx].current >= 1025 && Main.m_stAGV[idx].current <= 1027)
                    && ((Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1023) || (Main.m_stAGV[idx].Goal >= 200 && Main.m_stAGV[idx].Goal <= 221)
                    || Main.m_stAGV[idx].Goal == 1178))
                    {
                        FLAG_SKIP_TRAFFIC = 1;
                    }
                    #endregion

                    //오버브릿지 하강리프트 작업후 탈출시, 상승리프트때문에 1277~1279 사이에 대기 차량이 있으면
                    //트래픽이 걸린다. 1277~1279 사이에 차량이 트래픽 중이면 엑셀 트래픽을 무시하고 보내준다. lkw20190601
                    int Check_Node_1278 = 0;
                    if ((Main.m_stAGV[idx].current >= 1504 && Main.m_stAGV[idx].current <= 1505)
                    && (Main.m_stAGV[idx].Goal >= 1351 && Main.m_stAGV[idx].Goal <= 1351))
                    {
                        for (int i = 0; i < Form1.LGV_NUM; i++)
                        {
                            //자기 자신은 제외
                            if (idx == i) continue;


                            //20200904 MDH 롤Roll 양극슬리터#10 우선(작전명:모세의기적)
                            /*if (((Main.m_stAGV[i].current >= 1258 && Main.m_stAGV[i].current <= 1276)
                                && (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395)
                                && (Main.m_stAGV[i].current >= 1319 && Main.m_stAGV[i].current <= 1321)
                                && (Main.m_stAGV[i].current >= 1479 && Main.m_stAGV[i].current <= 1481)) && !(i >= 2 && i <= 4)) continue;*/


                            //다른 차량이 오버브릿지 상승리프트로 인해 1277~1279에 트래픽 중이면...
                            if ((Main.m_stAGV[i].current >= 1277 && Main.m_stAGV[i].current <= 1279) &&
                                (Main.m_stAGV[idx].Goal >= 1393 && Main.m_stAGV[idx].Goal <= 1393) &&
                                Main.m_stAGV[i].state == 7)
                                Check_Node_1278 = 1;
                        }

                        if (Check_Node_1278 == 1)
                            FLAG_SKIP_TRAFFIC = 1;
                    }

                    if (FLAG_SKIP_TRAFFIC == 0)
                    {
                        otherAGV_Exist[idx] = 1;
                    }
                }
                for (int i = 0; i < Main.Form_Traffic_Cell.Traffic_Cell.Count; i++)
                {
                    if (Cell_On[idx] == 1 && Main.m_stAGV[idx].current >= Main.m_stTraffic_Cell[i].Start_Index && Main.m_stAGV[idx].current <= Main.m_stTraffic_Cell[i].End_Index)
                    {
                        otherAGV_Exist[idx] = 1;
                    }
                }


                Ather_AGV_Count[idx] = 0;
                Ather_ROLL_AGV_Count[idx] = 0;

                for (int i = 0; i < Form1.LGV_NUM; i++)
                {
                    if (idx == i) continue;

                    //20200612 MDH 속도로 인한 트래픽 뚫림으로 범위 1356에서 1355로 조정 
                    if (Main.m_stAGV[idx].current >= 1355 && Main.m_stAGV[idx].current <= 1358)
                    {
                        if (i == 0 || i == 1 || i == 9 || i == 11 || i == 12)
                        {
                            //오버브릿지 영역에 몇대 있는지 확인
                            if ((Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1318)
                                || (Main.m_stAGV[i].current >= 1500 && Main.m_stAGV[i].current <= 1505)
                                || (Main.m_stAGV[i].current >= 1359 && Main.m_stAGV[i].current <= 1360)
                                || (Main.m_stAGV[i].current >= 1319 && Main.m_stAGV[i].current <= 1321)
                                || (Main.m_stAGV[i].current >= 1268 && Main.m_stAGV[i].current <= 1292)
                                || (Main.m_stAGV[i].current >= 1488 && Main.m_stAGV[i].current <= 1493)
                                || (Main.m_stAGV[i].current >= 1479 && Main.m_stAGV[i].current <= 1481)
                                || (Main.m_stAGV[i].current >= 1578 && Main.m_stAGV[i].current <= 1589)
                                || (Main.m_stAGV[i].current >= 1476 && Main.m_stAGV[i].current <= 1478))
                            {
                                Ather_AGV_Count[idx]++;
                            }
                        }
                    }
                    //양극 롤창고에 몇대 있는지 확인
                    if (((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077) && Main.m_stAGV[idx].x <= 100000))
                    {
                        //양극 롤 영역에 몇대 있는지 확인
                        if ((Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1033)
                            || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1412)
                            || (Main.m_stAGV[i].current >= 1455 && Main.m_stAGV[i].current <= 1457)
                            || (Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                            || (Main.m_stAGV[i].current >= 1137 && Main.m_stAGV[i].current <= 1141))
                        {
                            Ather_ROLL_AGV_Count[idx]++;
                        }
                    }

                    //20200722 MDH, 1356 > 1355로 확장
                    //20200722 MDH, (idx != 1393) 조건 제외조치, 해당조건으로 인해 충돌 발생
                    if ((Main.m_stAGV[idx].current >= 1355 && Main.m_stAGV[idx].current <= 1358)
                     && Main.m_stAGV[idx].Goal != 1286 
                     //&& Main.m_stAGV[idx].Goal != 1393 
                     && Main.m_stAGV[idx].Goal != 1357 
                     && Main.m_stAGV[idx].Goal != 1356)
                    {
                        if ((Main.m_stAGV[i].current >= 1261 && Main.m_stAGV[i].current <= 1283)
                        || (Main.m_stAGV[i].current >= 1359 && Main.m_stAGV[i].current <= 1360)
                        || (Main.m_stAGV[i].current >= 1578 && Main.m_stAGV[i].current <= 1589)
                        || (Main.m_stAGV[i].current >= 1503 && Main.m_stAGV[i].current <= 1505)
                        || (Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1320)
                        || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //20200722 MDH, 1356 > 1355로 확장
                    if ((Main.m_stAGV[idx].current >= 1355 && Main.m_stAGV[idx].current <= 1358)
                     && Main.m_stAGV[idx].Goal == 1268)
                    {
                        if ((Main.m_stAGV[i].current >= 1261 && Main.m_stAGV[i].current <= 1283)
                        || (Main.m_stAGV[i].current >= 1359 && Main.m_stAGV[i].current <= 1360)
                        || (Main.m_stAGV[i].current >= 1578 && Main.m_stAGV[i].current <= 1589)
                        || (Main.m_stAGV[i].current >= 1500 && Main.m_stAGV[i].current <= 1505)
                        || (Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1320)
                        || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }


                    if ((Main.m_stAGV[idx].current >= 1277 && Main.m_stAGV[idx].current <= 1279)
                     && (Main.m_stAGV[idx].Goal != 1286
                     && ((Main.m_stAGV[idx].Goal == 1393 && Main.m_stAGV[idx].Dest_Port_Num == "1502")
                     || Main.m_stAGV[idx].Goal == 1061 || Main.m_stAGV[idx].Goal == 1062 || Main.m_stAGV[idx].Goal == 1270
                     || Main.m_stAGV[idx].Goal == 1064 || Main.m_stAGV[idx].Goal == 1369)))
                    {
                        if ((Main.m_stAGV[i].current >= 1578 && Main.m_stAGV[i].current <= 1589)
                        || (Main.m_stAGV[i].current >= 1269 && Main.m_stAGV[i].current <= 1276)
                        || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395)
                        || (Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1318)
                        || (Main.m_stAGV[i].current >= 1476 && Main.m_stAGV[i].current <= 1478)) //MDH 20200904 1478에서 1481로 확장
                        //|| (Main.m_stAGV[i].current >= 1476 && Main.m_stAGV[i].current <= 1481)) //MDH 20200904 1478에서 1481로 확장
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }


                    // 대상차량의 목적지가 버퍼일 때, 양극프레스9호기 출고 작업이 있으면 오버브릿지 진입전 트래픽
                    if ((Main.m_stAGV[idx].current >= 1277 && Main.m_stAGV[idx].current <= 1279)
                     && (Main.m_stAGV[idx].Goal != 1286
                     && ((Main.m_stAGV[idx].Goal >= 1391 && Main.m_stAGV[idx].Goal <= 1395) || Main.m_stAGV[idx].Goal == 1268)  // 버퍼1,2 경유지 1268 추가 (MDH 20190702)
                     && Main.m_stAGV[idx].Dest_Port_Num != "1502"))
                    {
                        if ((Main.m_stAGV[i].current >= 1269 && Main.m_stAGV[i].current <= 1269)    // 1268을 1269로 변경 (MDH 20190702)
                            || (Main.m_stAGV[i].current >= 1476 && Main.m_stAGV[i].current <= 1481)  // 양극프레스9호기 출고대 1476-1478 조건 추가 (MDH 20190702), 20200904 MDH 1478에서 1481로 확장
                            || ((Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1313 && Main.m_stAGV[i].Goal != 1268))   // 양극프레스9호기 출고대 1311-1313 진입일때 조건 추가 (MDH 20190702)
                            || (Main.m_stAGV[i].current == 1395 && Main.m_stAGV[i].Goal != 1268))   // 양극프레스9호기 출고대 1311-1313 진입일때 조건 추가 (MDH 20190702)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1277 && Main.m_stAGV[idx].current <= 1279)
                     && (Main.m_stAGV[idx].Goal != 1286
                     && (Main.m_stAGV[idx].Goal == 1393 && Main.m_stAGV[idx].Dest_Port_Num == "1502")))
                    {
                        if (Main.m_stAGV[i].current >= 1500 && Main.m_stAGV[i].current <= 1502)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1277 && Main.m_stAGV[idx].current <= 1279)
                     && Main.m_stAGV[idx].Goal != 1286)
                    {
                        if ((Main.m_stAGV[i].current >= 1578 && Main.m_stAGV[i].current <= 1589)
                        || (Main.m_stAGV[i].current >= 1269 && Main.m_stAGV[i].current <= 1276)
                        || (Main.m_stAGV[i].current >= 1391 && Main.m_stAGV[i].current <= 1395)
                        || (Main.m_stAGV[i].current >= 1311 && Main.m_stAGV[i].current <= 1318))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //20200722 MDH, 상대차량 1356 > 1355로 확장
                    if ((Main.m_stAGV[idx].current >= 1500 && Main.m_stAGV[idx].current <= 1501)
                     && (Main.m_stAGV[idx].Goal >= 1313 && Main.m_stAGV[idx].Goal <= 1313))
                    {
                        if (((Main.m_stAGV[i].current >= 1355 && Main.m_stAGV[i].current <= 1360)
                        || (Main.m_stAGV[i].current >= 1276 && Main.m_stAGV[i].current <= 1280))
                        && (Main.m_stAGV[i].Goal >= 1313 && Main.m_stAGV[i].Goal <= 1313))
                        {
                            if ((Main.m_stAGV[i].current >= 1355 && Main.m_stAGV[i].current <= 1360) && Main.m_stAGV[i].state == 7)
                                continue;

                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1054 && Main.m_stAGV[idx].current <= 1055)
                        && Main.m_stAGV[idx].Goal != 1054 && Main.m_stAGV[idx].Goal != 1427 && Main.m_stAGV[idx].Goal != 1424)
                    {
                        if ((Main.m_stAGV[i].current >= 1103 && Main.m_stAGV[i].current <= 1105)
                         || (Main.m_stAGV[i].current >= 1428 && Main.m_stAGV[i].current <= 1428)
                         || (Main.m_stAGV[i].current >= 1056 && Main.m_stAGV[i].current <= 1059))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1284 && Main.m_stAGV[idx].current <= 1285) && Main.m_stAGV[idx].Goal != 1286)
                    {
                        //20200417 MDH 1356을 1355로 확장
                        if ((Main.m_stAGV[i].current >= 1355 && Main.m_stAGV[i].current <= 1360)
                         && !(Main.m_stAGV[i].Goal >= 1286 && Main.m_stAGV[i].Goal <= 1286))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    /*
                    if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 173) && Main.m_stAGV[idx].Goal != 1286)
                    {
                        if ((Main.m_stAGV[i].current >= 1356 && Main.m_stAGV[i].current <= 1360)
                         && !(Main.m_stAGV[i].Goal >= 1286 && Main.m_stAGV[i].Goal <= 1286))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    */
                    //11.07 포장실 트래픽 추가
                    if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 173)
                       && (Main.m_stAGV[idx].Goal >= 1000 || Main.m_stAGV[idx].Goal == 183))
                    {
                        if ((Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1132)
                         && !(Main.m_stAGV[i].Goal >= 1156 && Main.m_stAGV[i].Goal <= 1161))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1437 && Main.m_stAGV[idx].current <= 1438)
                       && (Main.m_stAGV[idx].Goal >= 1091 && Main.m_stAGV[idx].Goal <= 1102))
                    {
                        if ((Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1132)
                         && !(Main.m_stAGV[i].Goal >= 1156 && Main.m_stAGV[i].Goal <= 1161))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if (Main.m_stAGV[idx].current >= 1082 && Main.m_stAGV[idx].current <= 1083)
                    {
                        if ((Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1132)
                         && !(Main.m_stAGV[i].Goal >= 1156 && Main.m_stAGV[i].Goal <= 1161))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1187 && Main.m_stAGV[idx].current <= 1189)
                       && (Main.m_stAGV[idx].Goal >= 1084 && Main.m_stAGV[idx].Goal <= 1187))
                    {
                        if ((Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1131)
                         && !(Main.m_stAGV[i].Goal >= 1156 && Main.m_stAGV[i].Goal <= 1161))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1132 && Main.m_stAGV[idx].current <= 1134)
                       && (!(Main.m_stAGV[idx].Goal >= 1156 && Main.m_stAGV[idx].Goal <= 1161)))
                    {
                        if ((Main.m_stAGV[i].current >= 1084 && Main.m_stAGV[i].current <= 1102)
                         || (Main.m_stAGV[i].current >= 1431 && Main.m_stAGV[i].current <= 1436)
                         || (Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1131)
                         || (Main.m_stAGV[i].current >= 1336 && Main.m_stAGV[i].current <= 1338)
                         || (Main.m_stAGV[i].current >= 1354 && Main.m_stAGV[i].current <= 1354)
                         || (Main.m_stAGV[i].current >= 1186 && Main.m_stAGV[i].current <= 1190)
                         || (Main.m_stAGV[i].current >= 1482 && Main.m_stAGV[i].current <= 1487)
                         || (Main.m_stAGV[i].current >= 1354 && Main.m_stAGV[i].current <= 1354)
                         || (Main.m_stAGV[i].current >= 174 && Main.m_stAGV[i].current <= 183)
                         || (Main.m_stAGV[i].current >= 1183 && Main.m_stAGV[i].current <= 1183)
                         || (Main.m_stAGV[i].current >= 1506 && Main.m_stAGV[i].current <= 1508)    //20200515 MDH 양극RW 구간 추가
                         )
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //음극 광폭구간 트래픽
                    if ((Main.m_stAGV[idx].current == 451 || Main.m_stAGV[idx].current == 460) && Main.m_stAGV[idx].Goal == 78)
                    {
                        if ((Main.m_stAGV[i].current >= 74 && Main.m_stAGV[i].current <= 78)
                         && (Main.m_stAGV[i].Dest_Port_Num != "451" && Main.m_stAGV[i].Dest_Port_Num != "460"
                          && Main.m_stAGV[i].Source_Port_Num != "451" && Main.m_stAGV[i].Source_Port_Num != "460"))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[8].current >= 153 && Main.m_stAGV[8].current <= 155) && Main.m_stAGV[8].Goal == 228)
                    {
                        if ((Main.m_stAGV[i].current >= 226 && Main.m_stAGV[i].current <= 237) // 음극 ROLL 차량 출고대 작업으로 인해 수정함. lkw20190411
                        || (Main.m_stAGV[i].current >= 310 && Main.m_stAGV[i].current <= 314)
                        || (Main.m_stAGV[i].current >= 467 && Main.m_stAGV[i].current <= 469)
                        || (Main.m_stAGV[i].current >= 473 && Main.m_stAGV[i].current <= 475))
                        {
                            otherAGV_Exist[8] = 1;
                        }
                    }


                    if ((Main.m_stAGV[idx].current >= 153 && Main.m_stAGV[idx].current <= 155) && Main.m_stAGV[idx].Goal == 228)
                    {
                        if ((Main.m_stAGV[i].current >= 74 && Main.m_stAGV[i].current <= 78)
                         && (Main.m_stAGV[i].Dest_Port_Num != "472" && Main.m_stAGV[i].Dest_Port_Num != "466"
                          && Main.m_stAGV[i].Source_Port_Num != "472" && Main.m_stAGV[i].Source_Port_Num != "466"))
                        {

                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 233 && Main.m_stAGV[idx].current <= 235)
                    && !(Main.m_stAGV[idx].Goal >= 236 && Main.m_stAGV[idx].Goal <= 237)
                    && !(Main.m_stAGV[idx].Goal >= 300 && Main.m_stAGV[idx].Goal <= 378))
                    {
                        if ((Main.m_stAGV[i].current >= 74 && Main.m_stAGV[i].current <= 78)
                         && (Main.m_stAGV[i].Dest_Port_Num != "469" && Main.m_stAGV[i].Dest_Port_Num != "475"
                          && Main.m_stAGV[i].Source_Port_Num != "469" && Main.m_stAGV[i].Source_Port_Num != "475")
                         && !(Main.m_stAGV[i].Goal >= 300 && Main.m_stAGV[i].Goal <= 378))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //양극 광폭구간 트래픽
                    if ((Main.m_stAGV[idx].current == 1451 || Main.m_stAGV[idx].current == 1460) && Main.m_stAGV[idx].Goal == 1075)
                    {
                        if ((Main.m_stAGV[i].current >= 1070 && Main.m_stAGV[i].current <= 1075)
                         && (Main.m_stAGV[i].Dest_Port_Num != "1451" && Main.m_stAGV[i].Dest_Port_Num != "1460" && Main.m_stAGV[i].Dest_Port_Num != "1466"  //20200612 MDH 맞트래픽으로 인해 1466추가
                          && Main.m_stAGV[i].Source_Port_Num != "1451" && Main.m_stAGV[i].Source_Port_Num != "1460" && Main.m_stAGV[i].Source_Port_Num != "1466")) //20200612 MDH 맞트래픽으로 인해 1466추가
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1323 && Main.m_stAGV[idx].current <= 1325) && Main.m_stAGV[idx].Goal == 1231)
                    {
                        if ((Main.m_stAGV[i].current >= 1070 && Main.m_stAGV[i].current <= 1075)
                         && Main.m_stAGV[i].Dest_Port_Num != "1472" && Main.m_stAGV[i].Dest_Port_Num != "1466"
                         && Main.m_stAGV[i].Source_Port_Num != "1472" && Main.m_stAGV[i].Source_Port_Num != "1466")
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1235 && Main.m_stAGV[idx].current <= 1237)
                    && !(Main.m_stAGV[idx].Goal >= 1237 && Main.m_stAGV[idx].Goal <= 1239)
                    && !(Main.m_stAGV[idx].Goal >= 1469 && Main.m_stAGV[idx].Goal <= 1475))
                    {
                        if ((Main.m_stAGV[i].current >= 1070 && Main.m_stAGV[i].current <= 1075)
                         && Main.m_stAGV[i].Dest_Port_Num != "1475" && Main.m_stAGV[i].Dest_Port_Num != "1469"
                         && Main.m_stAGV[i].Source_Port_Num != "1475" && Main.m_stAGV[i].Source_Port_Num != "1469")
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }


                    //양극 리프트 구간 트리팩
                    if ((Main.m_stAGV[idx].current >= 1110 && Main.m_stAGV[idx].current <= 1112)
                    && (Main.m_stAGV[idx].Goal >= 1054 && Main.m_stAGV[idx].Goal <= 1054))
                    {
                        if ((Main.m_stAGV[i].current >= 1053 && Main.m_stAGV[i].current <= 1054)  //MDH 20200716 (1050 > 1053), 맞트래픽 발생으로 인한 수정
                         && Main.m_stAGV[i].Dest_Port_Num != "1424" && Main.m_stAGV[i].Dest_Port_Num != "1427"
                         && Main.m_stAGV[i].Source_Port_Num != "1424" && Main.m_stAGV[i].Source_Port_Num != "1427")
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    if ((Main.m_stAGV[idx].current >= 1424 && Main.m_stAGV[idx].current <= 1424)
                    && (Main.m_stAGV[idx].Goal >= 1054 && Main.m_stAGV[idx].Goal <= 1054))
                    {
                        if ((Main.m_stAGV[i].current >= 1050 && Main.m_stAGV[i].current <= 1054)
                         && Main.m_stAGV[i].Dest_Port_Num != "1424" && Main.m_stAGV[i].Dest_Port_Num != "1427"
                         && Main.m_stAGV[i].Source_Port_Num != "1424" && Main.m_stAGV[i].Source_Port_Num != "1427")
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }


                    //양극 프레스 입고대 트래픽
                    if ((Main.m_stAGV[idx].current >= 1416 && Main.m_stAGV[idx].current <= 1421)
                    && (Main.m_stAGV[idx].Goal >= 1040 && Main.m_stAGV[idx].Goal <= 1040))
                    {
                        if ((Main.m_stAGV[i].current >= 1035 && Main.m_stAGV[i].current <= 1040)
                         && (Main.m_stAGV[i].Goal != 1040 && Main.m_stAGV[i].Goal != 1035))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //음극 코터 입고대 트래픽
                    if (((Main.m_stAGV[idx].current >= 413 && Main.m_stAGV[idx].current <= 415)
                      || (Main.m_stAGV[idx].current >= 135 && Main.m_stAGV[idx].current <= 135))
                    && (Main.m_stAGV[idx].Goal >= 33 && Main.m_stAGV[idx].Goal <= 33))
                    {
                        if (((Main.m_stAGV[i].current >= 30 && Main.m_stAGV[i].current <= 36) && Main.m_stAGV[i].Goal != 33)
                          || (Main.m_stAGV[i].current >= 81 && Main.m_stAGV[i].current <= 83))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //음극 프레스 입고대 트래픽
                    //MDH 191002 원본은 아래에, 44에서 프레스나 코터 진입하는 경우도 고려
                    //MDH 20200612 범위 40>39로 확장
                    if ((Main.m_stAGV[idx].current >= 416 && Main.m_stAGV[idx].current <= 421)
                    && (Main.m_stAGV[idx].Goal >= 43 && Main.m_stAGV[idx].Goal <= 43))
                    {
                        if (Main.m_stAGV[i].current >= 39 && Main.m_stAGV[i].current <= 44 && !(Main.m_stAGV[i].state == 8 || Main.m_stAGV[i].state == 9 || Main.m_stAGV[i].state == 10)
                            || Main.m_stAGV[i].current >= 136 && Main.m_stAGV[i].current <= 139)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    /*
                    if ((Main.m_stAGV[idx].current >= 416 && Main.m_stAGV[idx].current <= 421)
                    && (Main.m_stAGV[idx].Goal >= 43 && Main.m_stAGV[idx].Goal <= 43))
                    {
                        if ((Main.m_stAGV[i].current >= 37 && Main.m_stAGV[i].current <= 43)
                         && Main.m_stAGV[i].Goal != 43)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    */

                    //11.23 양극 후공정 트래픽 개선
                    if (((Main.m_stAGV[idx].current >= 1485 && Main.m_stAGV[idx].current <= 1487)
                     || (Main.m_stAGV[idx].current >= 1482 && Main.m_stAGV[idx].current <= 1484))
                      && Main.m_stAGV[idx].Goal != 1487 && Main.m_stAGV[idx].Goal != 1484)
                    {
                        if ((Main.m_stAGV[i].current >= 1082 && Main.m_stAGV[i].current <= 1102)
                         || (Main.m_stAGV[i].current >= 1336 && Main.m_stAGV[i].current <= 1339)
                         || (Main.m_stAGV[i].current >= 1354 && Main.m_stAGV[i].current <= 1360)
                         || (Main.m_stAGV[i].current >= 1361 && Main.m_stAGV[i].current <= 1364)
                         || (Main.m_stAGV[i].current >= 1186 && Main.m_stAGV[i].current <= 1190))
                        {
                            // 양극 9호기 출고대 1082 차량과 맞트래픽 발생. 엑셀에 트래픽이 있으므로, 목적지 해제함. lkw20190325
                            if (((Main.m_stAGV[i].current >= 1082 && Main.m_stAGV[i].current <= 1083) && Main.m_stAGV[i].Goal == 1084)
                             || ((Main.m_stAGV[i].current >= 1087 && Main.m_stAGV[i].current <= 1102)
                              && (Main.m_stAGV[idx].Goal == 1097 || Main.m_stAGV[idx].Goal == 1438))) //0114 pgb 충전소 들어가는 놈이면 풀어주기
                            {
                                continue;
                            }
                            if ((Main.m_stAGV[idx].Goal == 1356 || Main.m_stAGV[idx].Goal == 1357)
                            && ((Main.m_stAGV[i].current >= 1086 && Main.m_stAGV[i].current <= 1102)
                            && ((Main.m_stAGV[i].Goal >= 1097 && Main.m_stAGV[i].Goal <= 1102) || Main.m_stAGV[i].Goal == 1438)))
                            {
                                continue;
                            }

                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 176) && Main.m_stAGV[idx].Goal != 438)
                    {
                        if ((Main.m_stAGV[i].current == 1436 || Main.m_stAGV[i].current == 1433) && Main.m_stAGV[i].x <= 42000)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    #region 0331 트래픽 추가(4, 11호기 관련)
                    /*if (Main.m_stAGV[i].dqGoal.Count > 0)
                    {
                        if ((Main.m_stAGV[idx].current >= 1213 && Main.m_stAGV[idx].current <= 1215) && Main.m_stAGV[idx].Goal == 1002)
                        {
                            if ((Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) >= 1200 && Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) <= 1210))
                            {
                                otherAGV_Exist[idx] = 1;
                            }
                        }
                    }*/
                    #endregion


                    //MDH 20200720 기재반입고 입구에서 11호기 감지조건
                    if (((Main.m_stAGV[idx].current >= 213 && Main.m_stAGV[idx].current <= 215) || (Main.m_stAGV[idx].current >= 1213 && Main.m_stAGV[idx].current <= 1215))
                        && (idx == 2 || idx == 3 || idx == 4) //양극 Roll
                        && (Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1005))
                    {
                        if ((i == 10) && //광폭11호기
                            (((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079) && ((Main.m_stAGV[i].Goal >= 1001 && Main.m_stAGV[i].Goal <= 1005) || (Main.m_stAGV[i].Goal >= 200 && Main.m_stAGV[i].Goal <= 212) || (Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1212) || Main.m_stAGV[i].Goal == 1178))
                            || ((Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033) && ((Main.m_stAGV[i].Goal >= 1001 && Main.m_stAGV[i].Goal <= 1005) || (Main.m_stAGV[i].Goal >= 200 && Main.m_stAGV[i].Goal <= 212) || (Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1212) || Main.m_stAGV[i].Goal == 1178))
                            || (Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1008)
                            || (Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1185)
                            || (Main.m_stAGV[i].current >= 200 && Main.m_stAGV[i].current <= 212)
                            || (Main.m_stAGV[i].current >= 1200 && Main.m_stAGV[i].current <= 1212)
                            || (Main.m_stAGV[i].current >= 1524 && Main.m_stAGV[i].current <= 1547)))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //충전소 갈때는 트래픽
                    if (Main.m_stAGV[idx].dqGoal.Count > 0)
                    {
                        if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 176) && Convert.ToInt32(Main.m_stAGV[idx].dqGoal[0]) == 1097)
                        {
                            if (Main.m_stAGV[i].current == 1436 || Main.m_stAGV[i].current == 1433)
                            {
                                otherAGV_Exist[idx] = 1;
                            }
                        }
                    }

                    if ((Main.m_stAGV[idx].current == 1436 || Main.m_stAGV[idx].current == 1433)
                     && (Main.m_stAGV[idx].Goal >= 1090 && Main.m_stAGV[idx].Goal <= 1102) && Main.m_stAGV[idx].x >= 42000)
                    {
                        if ((Main.m_stAGV[i].current >= 1082 && Main.m_stAGV[i].current <= 1099)
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183)
                         || (Main.m_stAGV[i].current >= 1183 && Main.m_stAGV[i].current <= 1183)
                         || (Main.m_stAGV[i].current >= 1186 && Main.m_stAGV[i].current <= 1190)
                         || (Main.m_stAGV[i].current >= 1129 && Main.m_stAGV[i].current <= 1130))
                        {
                            if ((Main.m_stAGV[i].current >= 1082 && Main.m_stAGV[i].current <= 1083)
                              && ((Main.m_stAGV[i].Goal >= 1091 && Main.m_stAGV[i].Goal <= 1097) || (Main.m_stAGV[i].Goal == 1161) || (Main.m_stAGV[i].Goal == 1087) || (Main.m_stAGV[i].Goal == 1089)))    //20200518 MDH ROLL 포장실가는 경우 예외조건 추가, 20200706 MDH Reel 오버브릿지 중간목적지 예외조건 추가
                            {
                                continue;
                            }
                            if ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183)
                              && (Main.m_stAGV[i].Source_Port_Num == "1436" || (Main.m_stAGV[i].Dest_Port_Num == "1436" && Main.m_stAGV[i].MCS_Carrier_ID != "")
                               || Main.m_stAGV[i].Source_Port_Num == "1433" || (Main.m_stAGV[i].Dest_Port_Num == "1433" && Main.m_stAGV[i].MCS_Carrier_ID != "")))
                            {
                                continue;
                            }

                            if (Main.m_stAGV[i].dqGoal.Count > 0)
                            {
                                if ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183) && (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 1097 || Convert.ToInt32(Main.m_stAGV[i].dqGoal[1]) == 1097))   //20200518 MDH dqGoal[1]일때에도 조건추가
                                if ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183) && (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 1097 || Convert.ToInt32(Main.m_stAGV[i].dqGoal[1]) == 1097))   //20200518 MDH dqGoal[1]일때에도 조건추가
                                {
                                    continue;
                                }
                            }
                            
                            //173 <> 1436 맞트래픽 발생
                            //20200624 MDH, 상대차 목적지 183이면서 dqGoal이 없을때 예외조건
                            if (Main.m_stAGV[i].dqGoal.Count == 0)
                            {
                                if ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 183) && Main.m_stAGV[i].Goal == 183)
                                {
                                    continue;
                                }
                            }
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //11.27 양극 후공정 개선
                    if ((Main.m_stAGV[idx].current >= 1053 && Main.m_stAGV[idx].current <= 1054)
                     && Main.m_stAGV[idx].Goal != 1064 && Main.m_stAGV[idx].Goal != 1054 && Main.m_stAGV[idx].Goal != 1427
                     && Main.m_stAGV[idx].Goal != 1424 && Main.m_stAGV[idx].Goal != 1369)
                    {
                        if ((Main.m_stAGV[i].current >= 1055 && Main.m_stAGV[i].current <= 1062)
                        || (Main.m_stAGV[i].current >= 1080 && Main.m_stAGV[i].current <= 1080)
                        || (Main.m_stAGV[i].current >= 1345 && Main.m_stAGV[i].current <= 1346)
                        || (Main.m_stAGV[i].current >= 1461 && Main.m_stAGV[i].current <= 1462)
                        || (Main.m_stAGV[i].current >= 1250 && Main.m_stAGV[i].current <= 1257)
                        || (Main.m_stAGV[i].current >= 1452 && Main.m_stAGV[i].current <= 1453))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //11.28광폭 트래픽 추가
                    if (Main.m_stAGV[idx].current >= 317 && Main.m_stAGV[idx].current <= 319
                    && ((Main.m_stAGV[idx].Goal >= 200 && Main.m_stAGV[idx].Goal <= 221)
                    || (Main.m_stAGV[idx].Goal >= 178 && Main.m_stAGV[idx].Goal <= 178)))
                    {
                        if (Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 151
                         || Main.m_stAGV[i].current >= 186 && Main.m_stAGV[i].current <= 200)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //12.05 음극 하강 리프트 구간 트래픽
                    if ((Main.m_stAGV[idx].current >= 422 && Main.m_stAGV[idx].current <= 427)
                        && Main.m_stAGV[idx].Goal == 58)
                    {
                        if ((Main.m_stAGV[i].current >= 55 && Main.m_stAGV[i].current <= 59)
                         && Main.m_stAGV[i].Goal != 58)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 428 && Main.m_stAGV[idx].current <= 430)
                        && Main.m_stAGV[idx].Goal == 61)
                    {
                        if ((Main.m_stAGV[i].current >= 55 && Main.m_stAGV[i].current <= 65)
                         && Main.m_stAGV[i].Goal != 61)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    /*
                    //12.10 음극 후공정 구간 트래픽 추가
                    if ((Main.m_stAGV[idx].current >= 104 && Main.m_stAGV[idx].current <= 105)
                        && (Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 278))
                    {
                        if ((Main.m_stAGV[i].current >= 262 && Main.m_stAGV[i].current <= 282)
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 493)
                         || (Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 109)
                         || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172)
                         || ((Main.m_stAGV[i].current >= 494 && Main.m_stAGV[i].current <= 496) && Main.m_stAGV[i].Goal == 277)

                         || ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 120) && (Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 278))

                         || ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173) && (Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 278)))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 104 && Main.m_stAGV[idx].current <= 105)
                        && !(Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 278))
                    {
                        if ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 118)
                         || (Main.m_stAGV[i].current >= 431 && Main.m_stAGV[i].current <= 436)
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 484)
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173)
                         || (Main.m_stAGV[i].current >= 276 && Main.m_stAGV[i].current <= 282)
                         || (((Main.m_stAGV[i].current >= 437 && Main.m_stAGV[i].current <= 439) 
                           || (Main.m_stAGV[i].current >= 119 && Main.m_stAGV[i].current <= 120)) 
                            && Main.m_stAGV[i].Goal == 173))

                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 276 && Main.m_stAGV[idx].current <= 278)
                        && !(Main.m_stAGV[idx].Goal >= 482 && Main.m_stAGV[idx].Goal <= 487)
                        && !(Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 278))
                    {
                        if ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 118)
                         || (Main.m_stAGV[i].current >= 431 && Main.m_stAGV[i].current <= 436)
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173)
                         || (Main.m_stAGV[i].current >= 279 && Main.m_stAGV[i].current <= 282))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    */
                            #region 음극 슬리터 2~3식 트래픽 추가 수정.lkw20190104 
                            //음극 슬리터 2식 트레픽 OR 충전소 트래픽
                            if ((Main.m_stAGV[idx].current >= 104 && Main.m_stAGV[idx].current <= 105)
                     && (Main.m_stAGV[idx].Goal >= 274 && Main.m_stAGV[idx].Goal <= 282))           //음극 슬리터 2식 / 충전소 방향
                    {
                        if ((Main.m_stAGV[i].current >= 274 && Main.m_stAGV[i].current <= 282)      //음극 슬리터 2~3식 대로
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 487)      //음극 슬리터 2식 출고대
                         || (Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 109)      //음극 슬리터 1식 방향. 차량 회전에 걸리기 때문에...
                         || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172)      //롤 리와인더#1
                         || (Main.m_stAGV[i].current >= 506 && Main.m_stAGV[i].current <= 508)      //롤 리와인더#1. lkw20190618
                                                                                                    //충전소에서 나오는 차량
                         || ((Main.m_stAGV[i].current >= 494 && Main.m_stAGV[i].current <= 496) && Main.m_stAGV[i].Goal == 277)
                         || ((Main.m_stAGV[i].current >= 437 && Main.m_stAGV[i].current <= 439) && Main.m_stAGV[i].Goal == 173)
                         || (Main.m_stAGV[i].current >= 116 && Main.m_stAGV[i].current <= 120)
                         // 음극 슬리터 3식에서 아래로 내려오는 차량확인. lkw20190113
                         || ((Main.m_stAGV[i].current >= 262 && Main.m_stAGV[i].current <= 273)
                         && !(Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 282))
                         //양극에서 올라오는 차량
                         || ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 120)
                          && ((Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 280) || (Main.m_stAGV[i].Goal >= 173 && Main.m_stAGV[i].Goal <= 173) || (Main.m_stAGV[i].Goal >= 1369 && Main.m_stAGV[i].Goal <= 1369)))
                         //충전소에서 올라오는 차량
                         || ((Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173)
                          && ((Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 280) || (Main.m_stAGV[i].Goal >= 1369 && Main.m_stAGV[i].Goal <= 1369))))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    //음극 슬리터 3식 트레픽
                    if ((Main.m_stAGV[idx].current >= 104 && Main.m_stAGV[idx].current <= 105)
                     && (Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 273))           //음극 슬리터 3식 방향
                    {
                        if ((Main.m_stAGV[i].current >= 262 && Main.m_stAGV[i].current <= 282)      //음극 슬리터 2~3식 대로
                         || (Main.m_stAGV[i].current >= 617 && Main.m_stAGV[i].current <= 619)      //릴코어회수 음극#2 추가. lkw20190508
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 493)      //음극 슬리터 2~3식 출고대
                         || (Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 109)      //음극 슬리터 1식 방향. 차량 회전에 걸리기 때문에...
                         || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172)      //롤 리와인더#1
                         || (Main.m_stAGV[i].current >= 506 && Main.m_stAGV[i].current <= 508)      //롤 리와인더#1. lkw20190618
                         || (Main.m_stAGV[i].current >= 512 && Main.m_stAGV[i].current <= 514)      //롤 리와인더#2. lkw20190618
                                                                                                    //충전소에서 나오는 차량
                         || ((Main.m_stAGV[i].current >= 494 && Main.m_stAGV[i].current <= 496) && Main.m_stAGV[i].Goal == 277)
                         || ((Main.m_stAGV[i].current >= 437 && Main.m_stAGV[i].current <= 438) && Main.m_stAGV[i].Goal == 173)
                         || (Main.m_stAGV[i].current >= 116 && Main.m_stAGV[i].current <= 120)
                         //양극에서 올라오는 차량
                         || ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 120)
                          && ((Main.m_stAGV[i].Goal >= 262 && Main.m_stAGV[i].Goal <= 280) || (Main.m_stAGV[i].Goal >= 173 && Main.m_stAGV[i].Goal <= 173) || (Main.m_stAGV[i].Goal >= 1369 && Main.m_stAGV[i].Goal <= 1369)))
                         //충전소에서 올라오는 차량 
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //음극 슬리터 3식 -> 양극 방향 트래픽. 음극 슬리터 2식이 작업 진행할 경우
                    if ((Main.m_stAGV[idx].current >= 269 && Main.m_stAGV[idx].current <= 271)
                    && !(Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 273)           //음극 슬리터 3식 방향이 아닐때...
                    && !(Main.m_stAGV[idx].Goal == 289))    //20200423 MDH, 음극 RW출고대 방향이 아닐경우
                    {
                        if ((Main.m_stAGV[i].current >= 274 && Main.m_stAGV[i].current <= 282)      //음극 슬리터 2식 대로
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 487)      //음극 슬리터 2식 출고대
                         || (Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 109)      //음극 슬리터 1식 방향. 차량 회전에 걸리기 때문에...
                         || (Main.m_stAGV[i].current >= 170 && Main.m_stAGV[i].current <= 172)      //롤 리와인더
                         || (Main.m_stAGV[i].current >= 110 && Main.m_stAGV[i].current <= 115)      //음극 슬리터 1식 대로
                         || (Main.m_stAGV[i].current >= 431 && Main.m_stAGV[i].current <= 436))      //음극 슬리터 1식 출고대
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    #endregion


                    #region M동 하강리프트 트래픽 수정. lkw20190110
                    // 384~386(리프트 뒷편) 에서 M동 하강리프트 OR 프레스 1식 작업 트래픽
                    if (((Main.m_stAGV[idx].current >= 384 && Main.m_stAGV[idx].current <= 386)
                        || (Main.m_stAGV[idx].current >= 1384 && Main.m_stAGV[idx].current <= 1386))
                        && (Main.m_stAGV[idx].Goal >= 1369 && Main.m_stAGV[idx].Goal <= 1369))
                    {
                        if ((Main.m_stAGV[i].current >= 1369 && Main.m_stAGV[i].current <= 1374)  // 신규 노드 라인(M동 하강리프트 OR 프레스 1 방향)
                         || (Main.m_stAGV[i].current >= 422 && Main.m_stAGV[i].current <= 427)  // 프레스 1, M동 하강 리프트 작업 위치
                         || (Main.m_stAGV[i].current >= 90 && Main.m_stAGV[i].current <= 95)    // 프레스 1, M동 하강 리프트 탈출 라인
                         || ((Main.m_stAGV[i].current >= 388 && Main.m_stAGV[i].current <= 390)
                         || (Main.m_stAGV[i].current >= 1388 && Main.m_stAGV[i].current <= 1390))// 진행 방향
                         || (Main.m_stAGV[i].current >= 96 && Main.m_stAGV[i].current <= 98)    // 음극 슬리터 1식 입고대 방향
                         || (Main.m_stAGV[i].current >= 428 && Main.m_stAGV[i].current <= 430) // 음극 슬리터 1식 입고대 작업 위치
                         || (Main.m_stAGV[i].current >= 397 && Main.m_stAGV[i].current <= 399))  //음극 충전소 신경로 트래픽
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 384~386(리프트 뒷편) 에서 M동 하강리프트 OR 프레스 1식 작업이 아닐때 트래픽 
                    if (((Main.m_stAGV[idx].current >= 384 && Main.m_stAGV[idx].current <= 388)
                        || (Main.m_stAGV[idx].current >= 1384 && Main.m_stAGV[idx].current <= 1388))
                        && !(Main.m_stAGV[idx].Goal >= 1369 && Main.m_stAGV[idx].Goal <= 1369))
                    {
                        if ((Main.m_stAGV[i].current >= 1369 && Main.m_stAGV[i].current <= 1374)  // 신규 노드 라인(M동 하강리프트 OR 프레스 1 방향)
                         || (Main.m_stAGV[i].current >= 96 && Main.m_stAGV[i].current <= 99)    // 음극 슬리터 1식 입고대 방향
                         || (Main.m_stAGV[i].current >= 428 && Main.m_stAGV[i].current <= 430) // 음극 슬리터 1식 입고대 작업 위치
                         || ((Main.m_stAGV[i].current >= 55 && Main.m_stAGV[i].current <= 65)    // 회전 노드 라인
                         && !(Main.m_stAGV[i].Goal == 58))                                   // ROLL LGV가 음극 슬리터 1식 작업이 아니면 트래픽. mdh20190614
                         || (Main.m_stAGV[i].current >= 121 && Main.m_stAGV[i].current <= 123)  // 회전 노드 라인
                         || ((Main.m_stAGV[i].current >= 388 && Main.m_stAGV[i].current <= 390)
                         || (Main.m_stAGV[i].current >= 1388 && Main.m_stAGV[i].current <= 1390))// 진행 방향
                         || (Main.m_stAGV[i].current >= 301 && Main.m_stAGV[i].current <= 303) // 프레스 2식 탈출지 라인
                         || (Main.m_stAGV[i].current >= 397 && Main.m_stAGV[i].current <= 399)  //음극 충전소 신경로 트래픽
                                                                                                // 하강 리프트 탈출지 58과 1388에서 트래픽이 잡히는 경우 정적 장애물이 발생하기 때문에
                                                                                                // 작업 후, 탈출지로 이동하는 작업이 있으면 트래픽을 잡을 수 있게 추가함. lkw20190326
                                                                                                // 하강 리프트 작업 후, 탈출지 58로 이동시 1388에 트래픽이 잡히면 정적 장애물이 발생함.
                                                                                                // 이로 인해, 92~94의 위치에 있을 경우, 엑셀 트래픽으로 잡음. lkw20190409
                         || (((Main.m_stAGV[i].current >= 422 && Main.m_stAGV[i].current <= 424) || // 프레스 1, M동 하강 리프트 작업 위치, 20200622 MDH 422-424로 조정, 425-427존재시 맞트래픽 발생 (1384 <> 427)
                              (Main.m_stAGV[i].current >= 90 && Main.m_stAGV[i].current <= 91) ||
                              (Main.m_stAGV[i].current >= 95 && Main.m_stAGV[i].current <= 95))  // 프레스 1, M동 하강 리프트 탈출 라인
                            && Main.m_stAGV[i].Goal == 58))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 92~94 탈출 시, 프레스 1, M동 하강 리프트 작업을 제외하고 트래픽. lkw20190418
                    // 프레스 1, M동 하강 리프트 작업일 경우, 엑셀 53~55에서 트래픽이 잡힘.
                    if ((Main.m_stAGV[idx].current >= 92 && Main.m_stAGV[idx].current <= 94)
                        && (Main.m_stAGV[idx].Goal >= 58 && Main.m_stAGV[idx].Goal <= 58))
                    {
                        if ((Main.m_stAGV[i].current >= 55 && Main.m_stAGV[i].current <= 65)  // 프레스 1, M동 하강 리프트 진입 노드
                            // 20200408 MDH 상대차량 현재위치 54에서 55로 변경, 54이면 3중 맞트래픽 발생
                            && !(Main.m_stAGV[i].Goal == 58))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    #endregion

                    // 음극 1식 슬리터 OR 양극 방향의 LGV 대차인 경우...
                    if ((Main.m_stAGV[idx].current >= 104 && Main.m_stAGV[idx].current <= 105)
                        && !(Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 278))
                    {
                        if ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 118)
                         || (Main.m_stAGV[i].current >= 431 && Main.m_stAGV[i].current <= 437)      // 음극 1식 슬리터 출고대, MDH 20200422 간섭으로 인해 436을 437로 확장
                                                                                                    // 턴 구간인 280에서 음극 2식 슬리터 출고대 1번과 부딪칠 가능성이 있기 때문에...
                         || (Main.m_stAGV[i].current >= 482 && Main.m_stAGV[i].current <= 487)      // 음극 2식 슬리터 출고대 1번
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 176)      // 음극 충전소 #2 탈출지
                                                                                                    // 기존 : 음극 2식 슬리터 앞 구간에 차량이 있으면 트래픽.
                                                                                                    // 신규 : 음극 2식 슬리터 출고대 작업이 있으면 트래픽. lkw20190103
                                                                                                    //|| (Main.m_stAGV[i].current >= 276 && Main.m_stAGV[i].current <= 282)  
                         || (Main.m_stAGV[i].current >= 116 && Main.m_stAGV[i].current <= 120)
                         || (Main.m_stAGV[i].current >= 276 && Main.m_stAGV[i].current <= 282)
                         || (Main.m_stAGV[i].current >= 391 && Main.m_stAGV[i].current <= 392)      //20200706 MDH 상대차량 391-392 멈춰있을때, 트래픽 뚫림 
                         // 음극 충전소 #2 -> 충전소 탈출지
                         || ((Main.m_stAGV[i].current >= 437 && Main.m_stAGV[i].current <= 439) //충전소 탈출지 범위 확장 pgb 0114
                            && Main.m_stAGV[i].Goal == 173))

                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //음극 2식 앞 -> 양극 진행 방향 트래픽
                    if ((Main.m_stAGV[idx].current >= 276 && Main.m_stAGV[idx].current <= 279)  //음극 2식 슬리터 출고대 앞
                        && !(Main.m_stAGV[idx].Goal >= 482 && Main.m_stAGV[idx].Goal <= 487)    //음극 2식 슬리터 출고대 아니고
                        && !(Main.m_stAGV[idx].Goal >= 262 && Main.m_stAGV[idx].Goal <= 278)    //음극 2~3식 목적지가 아니면...
                        && Main.m_stAGV[idx].Goal != 496                                        //충전소 아닐때
                        && Main.m_stAGV[idx].Goal != 508)                                       //20190822 mdh 목적지가 R/W 아닐때
                    {
                        if ((Main.m_stAGV[i].current >= 106 && Main.m_stAGV[i].current <= 118)  //양극 방향 차량이 있는지
                         || (Main.m_stAGV[i].current >= 431 && Main.m_stAGV[i].current <= 437)  //음극 1식 슬리터 출고대 앞, MDH 20200422 간섭으로 인해 436에서 437로 확장
                         || (Main.m_stAGV[i].current >= 173 && Main.m_stAGV[i].current <= 173)  //음극 충전소 #2 탈출지
                         || (Main.m_stAGV[i].current >= 279 && Main.m_stAGV[i].current <= 282)) //진행 방향에 차량이 있는지
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    /*//20190507 MDH 
                    // 슬리터출입구쪽 자동문사이에 LGV가 트래픽 상태일 때
                    if ((Main.m_stAGV[idx].current >= 173 && Main.m_stAGV[idx].current <= 176)  // 대상 현위치 - 자동문 사이
                    && !(Main.m_stAGV[idx].Goal == 1369)    // 대상 목적지 - 음극 슬리터7호기 입고대 근처가 아니면
                    {
                        if ((Main.m_stAGV[i].current >= 1100 && Main.m_stAGV[i].current <= 1102)  // 상대 현위치 - 양극 릴충전소
                         || (Main.m_stAGV[i].current >= 1431 && Main.m_stAGV[i].current <= 1432)  // 상대 현위치 - 양극 슬리터8호기 출고대#1
                         || (Main.m_stAGV[i].current >= 1434 && Main.m_stAGV[i].current <= 1435)  // 상대 현위치 - 양극 슬리터8호기 출고대#2
                         || (Main.m_stAGV[i].current >= 1084 && Main.m_stAGV[i].current <= 1099)  // 상대 현위치 - 양극 슬리터8호기 출고대로
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    /*/

                    //12.13 음극 롤창고 트래픽
                    //if ((Main.m_stAGV[idx].current >= 86 && Main.m_stAGV[idx].current <= 87)
                    //    && !(Main.m_stAGV[idx].Goal >= 33 && Main.m_stAGV[idx].Goal <= 100))
                    //{
                    //    if ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 498)
                    //     || (Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 27)
                    //     || (Main.m_stAGV[i].current >= 401 && Main.m_stAGV[i].current <= 412)
                    //     || (Main.m_stAGV[i].current >= 455 && Main.m_stAGV[i].current <= 457)
                    //     || (Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 140)
                    //     || ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 499) && Main.m_stAGV[i].Goal == 8))
                    //    {
                    //        otherAGV_Exist[idx] = 1;
                    //    }
                    //}

                    #region 음극 롤창고 트래픽 개선. lkw20190628
                    // 음극 롤창고#1 가는 차량 or 광폭 가는 차량 or 음극 프레스 9호기 가는 차량일 경우...
                    // 창고#1 방향으로 올라갈때
                    if ((Main.m_stAGV[idx].current >= 86 && Main.m_stAGV[idx].current <= 88)
                        && ((Main.m_stAGV[idx].Goal >= 23 && Main.m_stAGV[idx].Goal <= 27) ||
                            (Main.m_stAGV[idx].Goal >= 15 && Main.m_stAGV[idx].Goal <= 15) ||
                            (Main.m_stAGV[idx].Goal >= 221 && Main.m_stAGV[idx].Goal <= 221)))
                    {
                        if ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 498)  //충전소
                         || (Main.m_stAGV[i].current >= 10 && Main.m_stAGV[i].current <= 27)    //가는길
                         || (Main.m_stAGV[i].current >= 401 && Main.m_stAGV[i].current <= 406)  //롤창고#1 입출고대
                         || (Main.m_stAGV[i].current >= 455 && Main.m_stAGV[i].current <= 457)  //프레스#1 입고대
                         || (Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 140)  //광폭 가는 길
                         || ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 499) && Main.m_stAGV[i].Goal == 8)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && Main.m_stAGV[i].Goal == 8)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && Main.m_stAGV[i].Goal == 15)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && Main.m_stAGV[i].Goal == 221)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && (Main.m_stAGV[i].Goal >= 23 && Main.m_stAGV[i].Goal <= 27)))
                        {
                            otherAGV_Exist[idx] = 1;
                        }

                        if (Main.m_stAGV[i].dqGoal.Count > 0)
                        {
                            if (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 8 && Main.m_stAGV[i].current == 499)
                            {
                                otherAGV_Exist[idx] = 1;
                            }
                        }
                    }

                    // 음극 롤창고#2 가는 차량 or 다른 곳 작업하는 차량일 경우...
                    // 창고#2 방향으로 내려갈때
                    if ((Main.m_stAGV[idx].current >= 86 && Main.m_stAGV[idx].current <= 88)
                        && ((Main.m_stAGV[idx].Goal >= 1 && Main.m_stAGV[idx].Goal <= 4)
                        || (Main.m_stAGV[idx].Goal >= 33 && Main.m_stAGV[idx].Goal <= 100)   // 목적지 44, 67번등의 충전소 작업대비, 33-100 조건 추가 MDH 190701
                        || !((Main.m_stAGV[idx].Goal >= 23 && Main.m_stAGV[idx].Goal <= 27) ||
                             (Main.m_stAGV[idx].Goal >= 15 && Main.m_stAGV[idx].Goal <= 15) ||
                             (Main.m_stAGV[idx].Goal >= 221 && Main.m_stAGV[idx].Goal <= 221))))
                    {
                        if ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 498)  //충전소
                         || (Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 17)    //가는길
                         || ((Main.m_stAGV[i].current >= 18 && Main.m_stAGV[i].current <= 27) && !(Main.m_stAGV[i].Goal == 221 || (Main.m_stAGV[i].Goal >= 401 && Main.m_stAGV[i].Goal <= 406)))// 음극 창고 1을 거쳐서 올라가는 작업을 제외한 전부 20200522 KOK
                         || (Main.m_stAGV[i].current >= 407 && Main.m_stAGV[i].current <= 412)  //롤창고#2 입출고대
                         || (Main.m_stAGV[i].current >= 455 && Main.m_stAGV[i].current <= 457)  //프레스#1 입고대
                         || ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 499) && Main.m_stAGV[i].Goal == 8)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && Main.m_stAGV[i].Goal == 8)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && !(Main.m_stAGV[i].Goal == 8))
                         || ((Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 24) && (Main.m_stAGV[i].Goal >= 1 && Main.m_stAGV[i].Goal <= 4))   //20200515 kok 상대차 기재반입소에서 오는경우 고려 조건추가
                         || ((Main.m_stAGV[i].current >= 140 && Main.m_stAGV[i].current <= 140) && (Main.m_stAGV[i].Goal >= 1 && Main.m_stAGV[i].Goal <= 4)))   //20200515 kok 상대차 기재반입소에서 오는경우 고려 조건추가

                        {
                            otherAGV_Exist[idx] = 1;
                        }

                        if (Main.m_stAGV[i].dqGoal.Count > 0)
                        {
                            if (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 8 && Main.m_stAGV[i].current == 499)
                            {
                                otherAGV_Exist[idx] = 1;
                            }
                        }
                    }

                    // 음극 롤창고#1에서 내려 가는 차량일 경우...
                    if ((Main.m_stAGV[idx].current >= 18 && Main.m_stAGV[idx].current <= 20)
                        && !((Main.m_stAGV[idx].Goal >= 23 && Main.m_stAGV[idx].Goal <= 27) ||
                             (Main.m_stAGV[idx].Goal >= 15 && Main.m_stAGV[idx].Goal <= 15) ||
                             (Main.m_stAGV[idx].Goal >= 221 && Main.m_stAGV[idx].Goal <= 221)))
                    {
                        if ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 498)  //충전소
                         || (Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 17)    //가는길
                         || (Main.m_stAGV[i].current >= 89 && Main.m_stAGV[i].current <= 89)    //가는길 (충돌위험 노드)
                         || (Main.m_stAGV[i].current >= 455 && Main.m_stAGV[i].current <= 457)  //프레스#1 입고대
                         || ((Main.m_stAGV[i].current >= 497 && Main.m_stAGV[i].current <= 499) && Main.m_stAGV[i].Goal == 8 && Main.m_stAGV[i].state != 7)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && Main.m_stAGV[i].Goal == 8)
                         || ((Main.m_stAGV[i].current >= 8 && Main.m_stAGV[i].current <= 9) && !(Main.m_stAGV[i].Goal == 8)))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 음극 롤창고#1에서 내려 가는 차량일 경우...
                    if ((Main.m_stAGV[idx].current >= 7 && Main.m_stAGV[idx].current <= 9)
                        && !((Main.m_stAGV[idx].Goal >= 23 && Main.m_stAGV[idx].Goal <= 27) ||
                             (Main.m_stAGV[idx].Goal >= 15 && Main.m_stAGV[idx].Goal <= 15) ||
                             (Main.m_stAGV[idx].Goal >= 221 && Main.m_stAGV[idx].Goal <= 221)))
                    {
                        if ((Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 6)    //가는길
                         || (Main.m_stAGV[i].current >= 28 && Main.m_stAGV[i].current <= 28)    //가는길 (충돌위험 노드)
                         || (Main.m_stAGV[i].current >= 407 && Main.m_stAGV[i].current <= 412))  //롤창고#2 입출고대
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    #endregion

                    //12.13 음극 롤창고 트래픽
                    if ((Main.m_stAGV[idx].current >= 18 && Main.m_stAGV[idx].current <= 20)
                        && !(Main.m_stAGV[idx].Goal >= 21 && Main.m_stAGV[idx].Goal <= 27)
                        && !(Main.m_stAGV[idx].Goal >= 200 && Main.m_stAGV[idx].Goal <= 221))
                    {
                        if (((Main.m_stAGV[i].current >= 89 && Main.m_stAGV[i].current <= 89)
                            && (Main.m_stAGV[i].Goal >= 33 && Main.m_stAGV[i].Goal <= 100))
                         || (Main.m_stAGV[i].current >= 1 && Main.m_stAGV[i].current <= 17)
                         || (Main.m_stAGV[i].current >= 28 && Main.m_stAGV[i].current <= 28))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 후공정 음극 슬리터 하단 충전소 트래픽 추가. lkw20190227
                    // 상단 충전소에서 명령을 받은 상태이면, 하단 충전소에서 나오는 차량을 트래픽으로 정지시킨다.
                    if (Main.m_stAGV[idx].current == 438)
                    {
                        if (Main.m_stAGV[i].dqGoal.Count > 0)
                        {
                            if (Convert.ToInt32(Main.m_stAGV[i].dqGoal[0]) == 277 && Main.m_stAGV[i].current == 496)
                            {
                                otherAGV_Exist[idx] = 1;
                            }
                        }
                    }

                    //01.15 양극 롤LGV 트래픽 개선 

                    //양극 롤 영역에 2대 이상 있으면 트래픽
                    if ((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077)
                      && Main.m_stAGV[idx].x <= 100000)
                    {
                        if (Ather_ROLL_AGV_Count[idx] >= 2)
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    //롤창고 #1번,기재 라인 으로 갈때
                    if (((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077)
                      && Main.m_stAGV[idx].x <= 100000)
                      && ((Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1005)    // 롤창고 #1
                       || (Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)    // 기재 반입구
                       || (Main.m_stAGV[idx].Goal >= 1178 && Main.m_stAGV[idx].Goal <= 1178)))  // LGV 11 충전소
                    {
                        if ((Main.m_stAGV[i].current >= 1137 && Main.m_stAGV[i].current <= 1141)
                         || (Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1014)
                         || (Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033)
                         || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1406)
                         || (Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1173))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    //롤창고 #2번으로 갈때

                    if (((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077)
                      && Main.m_stAGV[idx].x <= 100000)
                      && Main.m_stAGV[idx].Goal >= 1011 && Main.m_stAGV[idx].Goal <= 1023)
                    {
                        if ((Main.m_stAGV[i].current >= 1137 && Main.m_stAGV[i].current <= 1141)
                         || (Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1014)
                         || (Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033)
                         || (Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1173)
                         || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1406) // 창고 #2로 갈때 입출고대 작업중이면 트래픽. lkw20190418
                         )
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if (((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077)
                      && Main.m_stAGV[idx].x <= 100000))
                    {
                        if ((Main.m_stAGV[i].current >= 1026 && Main.m_stAGV[i].current <= 1033)
                         || (Main.m_stAGV[i].current >= 1117 && Main.m_stAGV[i].current <= 1119)
                         || (Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    
                    //20200408 MDH LGV 11호기, 기재랙, 반입고 가는 경우
                    if (((Main.m_stAGV[idx].current >= 1076 && Main.m_stAGV[idx].current <= 1077)
                      && Main.m_stAGV[idx].x <= 100000) && (idx == 10)  //LGV 11호기
                      && ((Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1005)    // 롤창고 #1, 롤창고는 가지 않으나, 중간목적지로 찍혀있는 경우 다수
                       || (Main.m_stAGV[idx].Goal >= 200 && Main.m_stAGV[idx].Goal <= 221)    // 기재 반입구
                       || (Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)    // 기재 반입구
                       || (Main.m_stAGV[idx].Goal == 448)    // 기재 반입구
                       || (Main.m_stAGV[idx].Goal == 1178)))  // LGV 11 충전소
                    {
                        if ((Main.m_stAGV[i].current >= 1171 && Main.m_stAGV[i].current <= 1185)
                         || (Main.m_stAGV[i].current >= 208 && Main.m_stAGV[i].current <= 212)
                         || (Main.m_stAGV[i].current >= 1208 && Main.m_stAGV[i].current <= 1212))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 프레스 3 작업(455~457) 후, 라인으로 복귀하는 차량(1041)의 경우, 1012에서 1138로 이동하는데
                    // 양극 입출고대 작업하던 차량과 충돌 및 정적 장애물 에러를 방지하기 위해 목적지를 확인하지 않도록 수정함. lkw20190415
                    if ((Main.m_stAGV[idx].current >= 1010 && Main.m_stAGV[idx].current <= 1012))// && (Main.m_stAGV[idx].Goal >= 1020 && Main.m_stAGV[idx].Goal <= 1023))
                    {
                        if ((Main.m_stAGV[i].current >= 1407 && Main.m_stAGV[i].current <= 1412)
                         || (Main.m_stAGV[i].current >= 1009 && Main.m_stAGV[i].current <= 1023)
                         || (Main.m_stAGV[i].current >= 1137 && Main.m_stAGV[i].current <= 1140))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1001 && Main.m_stAGV[idx].current <= 1004)
                    && Main.m_stAGV[idx].Goal != 1403 && Main.m_stAGV[idx].Goal != 1406 && Main.m_stAGV[idx].Goal != 1178
                    && !(Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)
                    && !(Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1004))
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000)
                         || (Main.m_stAGV[i].current >= 1005 && Main.m_stAGV[i].current <= 1009)
                         || ((Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033)
                         && ((Main.m_stAGV[i].Goal >= 1002 && Main.m_stAGV[i].Goal <= 1023)
                         || (Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1221)
                         || (Main.m_stAGV[i].Goal >= 1178 && Main.m_stAGV[i].Goal <= 1178)))
                         //|| ((Main.m_stAGV[i].current >= 1137 && Main.m_stAGV[i].current <= 1141) // 20200409 MDH 주석처리 (1137-1141에서 트래픽 부여)
                         && !(Main.m_stAGV[i].Goal >= 1001 && Main.m_stAGV[i].Goal <= 1005)
                         && !(Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1221))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    // 양극 롤창고#1에서 프레스 3 작업(455~457) / 충전소 일 경우. 트래픽.lkw20190427
                    if ((Main.m_stAGV[idx].current >= 1001 && Main.m_stAGV[idx].current <= 1004)
                    && (Main.m_stAGV[i].Goal >= 1011 && Main.m_stAGV[i].Goal <= 1012))
                    {
                        if ((Main.m_stAGV[i].current >= 1455 && Main.m_stAGV[i].current <= 1477)    //프레스 3 작업
                         || (Main.m_stAGV[i].current == 1005 && Main.m_stAGV[i].current <= 1013)
                         || ((Main.m_stAGV[i].current == 1497 && Main.m_stAGV[i].current <= 1499)   //충전소에서 나오는 차량
                         && !(Main.m_stAGV[i].Goal == 1499)))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1139 && Main.m_stAGV[idx].current <= 1140)        //20200409 MDH 1137을 1139로 수정
                    && ((Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1005)
                       || (Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)))
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000)
                         || (Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1009)
                         || ((Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033)
                         && ((Main.m_stAGV[i].Goal >= 1002 && Main.m_stAGV[i].Goal <= 1023)
                         || (Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1221)
                         || (Main.m_stAGV[i].Goal >= 1178 && Main.m_stAGV[i].Goal <= 1178)))
                         || (Main.m_stAGV[i].current >= 1141 && Main.m_stAGV[i].current <= 1141)
                         || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1406))
                        {
                            otherAGV_Exist[idx] = 1;
                        }

                    }

                    if ((Main.m_stAGV[idx].current >= 1139 && Main.m_stAGV[idx].current <= 1140)    //20200409 MDH 1137을 1139로 수정
                    && !(Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1005) && !(Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221))
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000)
                         || (Main.m_stAGV[i].current >= 1001 && Main.m_stAGV[i].current <= 1009) // 1005를 1001로 변경 MDH (20200407)
                         || (Main.m_stAGV[i].current >= 1401 && Main.m_stAGV[i].current <= 1406) // 양극롤창고 작업중인 차량이 있으면 트래픽 MDH (20200407)
                         || ((Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1033)
                         && ((Main.m_stAGV[i].Goal >= 1002 && Main.m_stAGV[i].Goal <= 1023)
                         || (Main.m_stAGV[i].Goal >= 1200 && Main.m_stAGV[i].Goal <= 1221)
                         || (Main.m_stAGV[i].Goal >= 1178 && Main.m_stAGV[i].Goal <= 1178)))
                         || (Main.m_stAGV[i].current >= 1141 && Main.m_stAGV[i].current <= 1141))
                        {
                            if (Main.m_stAGV[idx].current == 1138 && Main.m_stAGV[idx].x < 123900) continue;
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1025 && Main.m_stAGV[idx].current <= 1026)
                    && Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1023 && Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)
                    {
                        if ((Main.m_stAGV[i].current >= 1024 && Main.m_stAGV[i].current <= 1024)
                         || (Main.m_stAGV[i].current >= 1006 && Main.m_stAGV[i].current <= 1013)
                         || (Main.m_stAGV[i].current >= 1455 && Main.m_stAGV[i].current <= 1457)
                         || ((Main.m_stAGV[i].current >= 1497 && Main.m_stAGV[i].current <= 1499) && (Main.m_stAGV[i].Goal >= 1011 && Main.m_stAGV[i].Goal <= 1012))
                         || (Main.m_stAGV[i].current >= 1497 && Main.m_stAGV[i].current <= 1498))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1026 && Main.m_stAGV[idx].current <= 1028)
                    && (!(Main.m_stAGV[idx].Goal >= 1001 && Main.m_stAGV[idx].Goal <= 1026)
                    && !(Main.m_stAGV[idx].Goal >= 1200 && Main.m_stAGV[idx].Goal <= 1221)
                    && !(Main.m_stAGV[idx].Goal >= 1178 && Main.m_stAGV[idx].Goal <= 1178)))
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1118 && Main.m_stAGV[idx].current <= 1119)
                    && Main.m_stAGV[idx].Goal >= 1029 && Main.m_stAGV[idx].Goal <= 1029)
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)

                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    /*
                    if ((Main.m_stAGV[idx].current >= 1497 && Main.m_stAGV[idx].current <= 1499)
                    && Main.m_stAGV[idx].Goal >= 1011 && Main.m_stAGV[idx].Goal <= 1012)
                    {
                        if ((Main.m_stAGV[i].current >= 1078 && Main.m_stAGV[i].current <= 1079)
                         || (Main.m_stAGV[i].current == 1077 && Main.m_stAGV[i].x >= 100000))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }
                    */
                    if ((Main.m_stAGV[idx].current >= 1174 && Main.m_stAGV[idx].current <= 1175)
                    && (!(Main.m_stAGV[idx].Goal >= 1208 && Main.m_stAGV[idx].Goal <= 1221)
                    && !(Main.m_stAGV[idx].Goal >= 1178 && Main.m_stAGV[idx].Goal <= 1178)))
                    {
                        if ((Main.m_stAGV[i].current >= 1074 && Main.m_stAGV[i].current <= 1079) && idx != 10) //1074-1079에 있는 차량이 LGV11호기 일경우 예외, MDH 20200415
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    if ((Main.m_stAGV[idx].current >= 1072 && Main.m_stAGV[idx].current <= 1075)
                        && Main.m_stAGV[idx].Goal != 1075 && Main.m_stAGV[idx].Goal != 1238 && Main.m_stAGV[idx].Goal != 1451 && Main.m_stAGV[idx].Goal != 1460
                        && Main.m_stAGV[idx].Goal != 1231 && Main.m_stAGV[idx].Goal != 1237)
                    {

                        if ((Main.m_stAGV[i].current >= 1076 && Main.m_stAGV[i].current <= 1077))
                        {
                            otherAGV_Exist[idx] = 1;
                        }
                    }

                    Check_ChargeDepot_163(idx, i);
                    Check_ChargeDepot_166(idx, i);
                }

                //20200722 MDH, 1356 > 1355로 확장
                //오버브릿지 영역에 2대 이상 있으면 트래픽
                if (Main.m_stAGV[idx].current >= 1355 && Main.m_stAGV[idx].current <= 1358)
                {
                    if (Ather_AGV_Count[idx] >= 2)
                    {
                        otherAGV_Exist[idx] = 1;
                    }
                }


                //글로벌텍 차량 트래픽
                if (((Main.m_stAGV[idx].current >= 1182 && Main.m_stAGV[idx].current <= 1184)
                || (Main.m_stAGV[idx].current >= 147 && Main.m_stAGV[idx].current <= 149)
                || (Main.m_stAGV[idx].current >= 659 && Main.m_stAGV[idx].current <= 661))
                && Main.CS_Connect_PLC.Area_Using == 1)
                {
                    otherAGV_Exist[idx] = 1;
                }

                if (otherAGV_Exist[idx] == 0)
                {
                    if (Main.m_stAGV[idx].state == 7)
                    {
                        Main.CS_AGV_Logic.LGV_SendData_Traffic(idx, GO);
                        Main.Log("LGV_0" + (idx + 1) + "---- Traffic_Log", "출발~~ 위치 = " + Main.m_stAGV[idx].current);
                    }
                }
                else if (otherAGV_Exist[idx] == 1)
                {
                    if (Main.m_stAGV[idx].state == 1)
                    {
                        Main.CS_AGV_Logic.LGV_SendData_Traffic(idx, STOP);
                        Main.Log("LGV_0" + (idx + 1) + "---- Traffic_Log", "정지!! 위치 = " + Main.m_stAGV[idx].current);
                    }

                }

            }
            catch (Exception ex)
            {
                Main.Log("Try Catch Traffic", "호기 :" + (idx + 1) + Convert.ToString(ex));
            }
        }
    }
}
