using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    //명령 데이터 클래스
    public class Command_Info
    {
        Form1 Main;

        public string Command_ID = "";
        public string Proiority = "";
        public string Carrier_ID = "";
        public string Source_Port = "";
        public string Dest_Port = "";
        public string Carrier_Type = "";
        public string Carrier_LOC = "";
        public string Process_ID = "";
        public string Batch_ID = "";
        public string LOT_ID = "";
        public string Carrier_S_Count = "";
        public string[] Carrier_S_ID = new string[10];
        public string Alloc_State = "";
        public string LGV_No = "";
        public string Call_Time = "";
        public string Alloc_Time = "";
        public string Complete_Time = "";
        public string Transfer_State = "";
        public string Quantity = "";

        public string Work_Name = "";
        public string Work_Type = "";
        public string Alloc_Ok = "";
        public string Start_Loc = "";
        public string End_Loc = "";

        public string Load_Move_Time = "";
        public string Load_Time = "";
        public string UnLoad_Move_Time = "";
        public string UnLoad_Time = "";

        public Command_Info()
        {

        }

        public Command_Info(Form1 CS_Main)
        {
            Main = CS_Main;
            for(int i = 0; i < 10; i++)
            {
                Carrier_S_ID[i] = "";
            }
        }
        
    }
}
      
       
