using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;   //DllImport
using System.Windows.Forms;
using System.IO;

namespace SDI_LCS
{
    #region 설명
        /*////////////////////////////////////////////////////////////////////////////////
        
        ////////////////////////////////////////////////////////////////////////////////*/

    #endregion


    #region Control INI File

    //------------------------------------------------------------------------------
    // INI Class
    //------------------------------------------------------------------------------
    public class IniControl
    {
       
        static public string g_ini_file = @"C:\LCS_LOG\Config\\setup.ini";

        //------------------------------------------------------------------------------
        [DllImport("kernel32")]

        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]

        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]

        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        //------------------------------------------------------------------------------
        public static int ReadInteger(string fileName, string IpAppName, string IpKeyName, int Default)
        {
            try
            {
                string inifile = g_ini_file;    //Path + File

                StringBuilder result = new StringBuilder(255);
                IniControl.GetPrivateProfileString(IpAppName, IpKeyName, "error", result, 255, inifile);

                if (result.ToString() == "error")
                {
                    return Default;
                }
                else
                {
                    return Convert.ToInt16(result);
                }
            }
            catch
            {
                return Default;
            }
        }

        //------------------------------------------------------------------------------
        public static Boolean ReadBool(string fileName, string IpAppName, string IpKeyName)
        {
            string inifile = g_ini_file;    //Path + File
            StringBuilder result = new StringBuilder(255);
            IniControl.GetPrivateProfileString(IpAppName, IpKeyName, "error", result, 255, inifile);

            if (result.ToString() == "True" || result.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //------------------------------------------------------------------------------
        public static string ReadString(string fileName, string IpAppName, string IpKeyName, string Default)
        {
            string inifile = g_ini_file;    //Path + File

            StringBuilder result = new StringBuilder(255);
            IniControl.GetPrivateProfileString(IpAppName, IpKeyName, "error", result, 255, inifile);

            if (result.ToString() == "error")
            {
                return Default;
            }
            else
            {
                return result.ToString();
            }

        }

        //------------------------------------------------------------------------------
        public static string getIni(string fileName, string IpAppName, string IpKeyName)
        {
            string inifile = g_ini_file;    //Path + File
            if (fileName != "")
                inifile = fileName;

            StringBuilder result = new StringBuilder(255);
            IniControl.GetPrivateProfileString(IpAppName, IpKeyName, "error", result, 255, inifile);
            
           
            return result.ToString();

        }

        //------------------------------------------------------------------------------
        public static Boolean setIni(string fileName, string IpAppName, string IpKeyName, string IpValue)
        {
            string inifile = g_ini_file;    //Path + File
            if (fileName != "")
                inifile = fileName;
            IniControl.WritePrivateProfileString(IpAppName, IpKeyName, IpValue, inifile);

            return true;
        }

        //------------------------------------------------------------------------------
        public static Boolean setIni(string filePath, string fileName, string IpAppName, string IpKeyName, string IpValue)
        {
            string inifile = filePath + g_ini_file;
            IniControl.WritePrivateProfileString(IpAppName, IpKeyName, IpValue, inifile);

            return true;
        }
    }

    #endregion

}


