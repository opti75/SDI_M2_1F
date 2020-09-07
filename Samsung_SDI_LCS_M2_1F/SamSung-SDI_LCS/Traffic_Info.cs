using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class Traffic_Info
    {
        Form1 Main;
        public string idx_Current_Use;
        public string idx_Start_Current;
        public string idx_End_Current;

        public string idx_Goal_Use;
        public string idx_Start_Goal;
        public string idx_End_Goal;

        public string idx_X_Use;
        public string idx_Start_X;
        public string idx_End_X;

        public string idx_Y_Use;
        public string idx_Start_Y;
        public string idx_End_Y;

        public string i_Current_Use;
        public string i_Start_Current;
        public string i_End_Current;

        public string i_Goal_Use;
        public string i_Start_Goal;
        public string i_End_Goal;

        public string i_X_Use;
        public string i_Start_X;
        public string i_End_X;

        public string i_Y_Use;
        public string i_Start_Y;
        public string i_End_Y;

        public string Use_Traffic_Area;

        public string Real_LeftTop_X;
        public string Real_LeftTop_Y;

        public string Real_Right_Bottom_X;
        public string Real_Right_Bottom_Y;

        public int Traffic_Area_No1;



        public Traffic_Info()
        {
            Init_Traffic_Info();
        }

        public Traffic_Info(Form1 CS_Main)
        {
            Main = CS_Main;
            Init_Traffic_Info();
        }
        public void Init_Traffic_Info()
        {
            idx_Start_Current = "";
            idx_End_Current = "";
            idx_Start_Goal = "";
            idx_End_Goal = "";

            i_Start_Current = "";
            i_End_Current = "";
            i_Start_Goal = "";
            i_End_Goal = "";
            Traffic_Area_No1 = -1;
        }

    }
}
