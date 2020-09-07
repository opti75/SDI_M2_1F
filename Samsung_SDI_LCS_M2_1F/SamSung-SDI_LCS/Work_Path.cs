using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDI_LCS
{
    public class Work_Path
    {
        Form1 Main;
        public string Work_Station = "";
        public string Stop_Area_1 = "";
        public string Stop_Area_2 = "";
        public string Stop_Area_3 = "";
        public string Goal_Area = "";
        public string Exit_Area_1 = "";
        public string Exit_Area_2 = "";
        public string Exit_Area_3 = "";
        public string Type = "";

        public Work_Path()
        {

        }
        public Work_Path(Form1 CS_Main)
        {
            Main = CS_Main;
        }
      
    }
}
