using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public class AGV_C_Info
    {
        Form1 Main;
        public string IP = "";
        public int Port = 0;
        public string Type = "";
        public AGV_C_Info()
        {
              
        }
        public AGV_C_Info(Form1 CS_Main)
        {
            Main = CS_Main;
        }
    }
}
