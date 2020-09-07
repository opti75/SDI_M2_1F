using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public class Shutter_C_Info
    {
        Form1 Main;
        public string IP = "";
        public int Port = 0;
        public int Start_Station_InPut = 0;
        public int End_Station_InPut = 0;
        public int Start_Goal_InPut = 0;
        public int End_Goal_InPut = 0;

        public int Start_Station_OutPut = 0;
        public int End_Station_OutPut = 0;
        public int Start_Goal_OutPut = 0;
        public int End_Goal_OutPut = 0;

        public string Machine_Name = "";

        public string Name = "";

        public Shutter_C_Info()
        {

        }
        public Shutter_C_Info(Form1 CS_Main)
        {
            Main = CS_Main;
        }
    }
}
