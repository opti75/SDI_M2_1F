using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class Traffic_Dest
    {
        Form1 Main;
        public string idx_Current_Use;
        public string idx_Start_Current;
        public string idx_End_Current;

        public string idx_Dest_Use;
        public string idx_Dest;

        public string i_Current_Use;
        public string i_Start_Current;
        public string i_End_Current;

        public string i_Goal_Use;
        public string i_Start_Goal;
        public string i_End_Goal;

        public Traffic_Dest()
        {
            Init_Traffic_Dest();
        }

        public Traffic_Dest(Form1 CS_Main)
        {
            Main = CS_Main;
            Init_Traffic_Dest();
        }
        public void Init_Traffic_Dest()
        {
            idx_Current_Use = "";
            idx_Start_Current = "";
            idx_End_Current = "";

            idx_Dest_Use = "";
            idx_Dest = "";

            i_Current_Use = "";
            i_Start_Current = "";
            i_End_Current = "";

            i_Goal_Use = "";
            i_Start_Goal = "";
            i_End_Goal = "";
        }
    }
}
