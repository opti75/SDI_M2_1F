using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class AGV_Info
    {
        Form1 Main;
        public DateTime Work_Start_Time;
        public string IP;
        public int x;
        public int y;
        public int t;
        public int SendData;
        public int connect;
        public int mode;
        public int Error;
        public int Charge;
        public int Battery;
        public int Working;
        public int current = 0;
        public int state = 0;
        public int target = 0;
        public int Goal = 0;
        public int Product_Sensor = 0;
        public int Working_Error = 0;
        public int Flag_Wait_Station = 0;
        public int Ask_Abort = 0;
        //1031추가 프로토콜
        public int Traffic_Skip = 0;
        public int Lift_Height = 0;
        public int FW_Driving_Motor_Angle = 0;
        public int BW_Driving_Motor_Angle = 0;
        public int LGV_Way = 0;
        public int Error_1 = 0;
        public int Error_2 = 0;
        public int Error_3 = 0;

        //배터리가 없어서 들어간 차량이 바로 튀어 나오는거 방지
        public int FLAG_LGV_Charge = 0;
        //중간 배터리 플래그
        public int FLAG_Middle_Battery_LGV = 0;
        public ArrayList dqGoal = new ArrayList();
        public ArrayList dqWork = new ArrayList();
        //MCS보고
        public string MCS_Vehicle_ID = "";
        public string MCS_Vehicle_State = "";
        public string MCS_Vehicle_Current_Position = "";
        public string MCS_Vehicle_Next_Postion = "";
        public string MCS_Vehicle_Goal = "";
        public string MCS_Carrier_ID = "";
        public string MCS_Install_Time = "";
        public string MCS_Source_Port = "";
        public string MCS_Dest_Port = "";
        public string MCS_Carrier_LOC = "";
        public string MCS_Carrier_Type = "";
        public string MCS_Alarm_Text = "";
        public string MCS_Alarm_ID = "";
        public string MCS_Vehicle_Command_ID = "";

        public string Source_Port_Num = "";
        public string Dest_Port_Num = "";
        public string MCS_Vehicle_Priority = "";
        public int Before_State = 0;
        public int Before_Current = 0;
        public string Before_State_611 = "";
        public int FLAG_Prevent_Double_Charge = 0;
        public int Setting_Charge_Value = 0;
        public int Setting_Middle_Charge_Value = 0;
        public int Setting_Low_Battery_Job_Count = 0;
        public AGV_Info()
        {
            Init_AGV_Info();
        }

        public AGV_Info(Form1 CS_Main)
        {
            Main = CS_Main;
            Init_AGV_Info();
        }
        public void Init_AGV_Info()
        {
            SendData = 0;
            IP = "";
            x = 0; ;
            y = 0;
            t = 0;
            connect = 0;
            state = 0;
            current = 0;
            target = 0;
            mode = 0;
            Error = 0;
            Charge = 0;
            Battery = 0;
            Working = 0;      
            Work_Start_Time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}
