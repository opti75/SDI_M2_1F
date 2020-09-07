using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class Traffic_Cell
    {
        Form1 Main;
        public int Start_Index;
        public int End_Index;
        public string Way;
        public int Cell_Count;

        public Traffic_Cell()
        {
            Init_Traffic_Cell();
        }

        public Traffic_Cell(Form1 CS_Main)
        {
            Main = CS_Main;
            Init_Traffic_Cell();
        }
        public void Init_Traffic_Cell()
        {
            Start_Index = 0;
            End_Index = 0;
            Cell_Count = 0;
            Way = "";
        }
    }
}
