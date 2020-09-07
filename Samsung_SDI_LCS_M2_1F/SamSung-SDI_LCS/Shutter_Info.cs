using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDI_LCS
{
    public class Shutter_Info
    {
        Form1 Main;
        public int InPut_Open_Sensor = 0;
        public int InPut_Close_Sensor = 0;
        public int OutPut_Open_Sensor = 0;
        public int OutPut_Close_Sensor = 0;
        public int HeartBit = 0;
        public Shutter_Info()
        {

        }

        public Shutter_Info(Form1 CS_Main)
        {
            Main = CS_Main;
           
        }
        
    }
}
