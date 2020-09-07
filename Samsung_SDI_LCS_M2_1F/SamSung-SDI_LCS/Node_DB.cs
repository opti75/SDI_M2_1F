using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDI_LCS
{
    
    public class Node_DB
    {
        Form1 Main;
        OleDbConnection conn;
        DataTable Grid_Table;
        public int Node_No;
        int Max_Count = 2000;
        int[] Node_Count;
    
        //생성자
        public Node_DB(Form1 CS_Main)
        {
            Main = CS_Main;

            Node_Count = new int[Max_Count];
            for (int i = 0; i < Max_Count; i++)
            {
                Node_Count[i] = 0;
            }
        }
        //엑셀 데이터 불러오기
        public void Select_Excel_Data()
        {
//            string test;
            double Min_X = 1e10;
            double Max_X = 1e-10;
            double Min_Y = 1e10;
            double Max_Y = 1e-10;

            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\\SDI_DATA\\Node.xlsx;Extended Properties=""Excel 12.0;HDR=YES;""";
            OleDbConnection excelConnection = new OleDbConnection(conStr);
            excelConnection.Open();

            Grid_Table = new DataTable();
            string strSQL = "SELECT * FROM [Sheet$]";
            OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);


            dataAdapter.Fill(Grid_Table);
            
            Node_No = Grid_Table.Rows.Count;
            for (int i = 0; i < Grid_Table.Rows.Count; i++)
            {
                Main.CS_Draw_Node_AGV.dqNode[i].index = Convert.ToInt32(Grid_Table.Rows[i][0]);
                Main.CS_Draw_Node_AGV.dqNode[i].x = Convert.ToInt32(Grid_Table.Rows[i][1]);
                Main.CS_Draw_Node_AGV.dqNode[i].y = Convert.ToInt32(Grid_Table.Rows[i][2]);

                Main.CS_Draw_Node_AGV.t1Node.index = Convert.ToInt32(Grid_Table.Rows[i][0]);
                Main.CS_Draw_Node_AGV.t1Node.x = Convert.ToInt32(Grid_Table.Rows[i][1]);
                Main.CS_Draw_Node_AGV.t1Node.y = Convert.ToInt32(Grid_Table.Rows[i][2]);
                Main.CS_Draw_Node_AGV.AddNode(Main.CS_Draw_Node_AGV.dqNode[i]);

                if(Main.CS_Draw_Node_AGV.dqNode[i].x < Min_X)
                {
                    Min_X = Main.CS_Draw_Node_AGV.dqNode[i].x;
                }

                if (Main.CS_Draw_Node_AGV.dqNode[i].x > Max_X)
                {
                    Max_X = Main.CS_Draw_Node_AGV.dqNode[i].x;
                }

                if (Main.CS_Draw_Node_AGV.dqNode[i].y < Min_Y)
                {
                    Min_Y = Main.CS_Draw_Node_AGV.dqNode[i].y;
                }

                if (Main.CS_Draw_Node_AGV.dqNode[i].y > Max_Y)
                {
                    Max_Y = Main.CS_Draw_Node_AGV.dqNode[i].y;
                }

            }
            /*
            double X_Ratio = (Max_X - Min_X) / Main.CS_Draw_Node_AGV.NODEWIDTH;
            double Y_Ratio = (Max_Y - Min_Y) / Main.CS_Draw_Node_AGV.NODEHEIGHT;

            double M_Ratio = X_Ratio > Y_Ratio ? X_Ratio : Y_Ratio;

            double Cen_X = ((Min_X + Max_X) / 2.0) / M_Ratio;
            double Cen_Y = ((Min_Y + Max_Y) / 2.0) / M_Ratio;
            */
            //Main.CS_Draw_Node_AGV.OX = (int)Cen_X;
            //Main.CS_Draw_Node_AGV.OY = (int)Cen_Y;
            //Main.CS_Draw_Node_AGV.PIXELSIZE = (int)M_Ratio;


            //dispose used objects
            Grid_Table.Dispose();
            dataAdapter.Dispose();
            dbCommand.Dispose();

            excelConnection.Close();
            excelConnection.Dispose();
        }
        public void Select_Excel_Path()
        {
//            string test;
            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\\SDI_DATA\\Path.xlsx;Extended Properties=""Excel 12.0;HDR=YES;""";
            OleDbConnection excelConnection = new OleDbConnection(conStr);
            excelConnection.Open();

            Grid_Table = new DataTable();
            string strSQL = "SELECT * FROM [Sheet$]";
            OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);

            dataAdapter.Fill(Grid_Table);

            for (int i = 0; i < Grid_Table.Rows.Count; i++)
            {
                Node_Count[i] = Convert.ToInt32(Grid_Table.Rows[i][1]);
                for (int j = 0; j < Grid_Table.Rows.Count; j++)
                {
                    int k = 0;
                    if (Convert.ToString(Grid_Table.Rows[i][j]) == "e" || Convert.ToString(Grid_Table.Rows[i][j]) == "" || Convert.ToString(Grid_Table.Rows[i][j]) == "{}")
                    {
                        break;
                    }
                    if (j == 0)
                    {
                        Main.CS_Draw_Node_AGV.dqPath[i].targetIndex = Convert.ToInt32(Grid_Table.Rows[i][j]);
                        Main.CS_Draw_Node_AGV.t1Path.targetIndex = Convert.ToInt32(Grid_Table.Rows[i][j]);
                    }
                    else if (j == 1)
                    {
                        Main.CS_Draw_Node_AGV.dqPath[i].pathCnt = Convert.ToInt32(Grid_Table.Rows[i][j]);
                        Main.CS_Draw_Node_AGV.t1Path.pathCnt = Convert.ToInt32(Grid_Table.Rows[i][j]);
                    }

                    else if (j == 2 || j == 5 || j == 8 || j == 11)
                    {
                        Main.CS_Draw_Node_AGV.dqPath[i].path[k].linkedIndex = Convert.ToInt32(Grid_Table.Rows[i][j]);
                        Main.CS_Draw_Node_AGV.t1Path.path[k].linkedIndex = Convert.ToInt32(Grid_Table.Rows[i][j]);
                    }
                    else if (j == 3 || j == 6 || j == 9 || j == 12)
                    {
                        Main.CS_Draw_Node_AGV.dqPath[i].path[k].line = Convert.ToInt32(Grid_Table.Rows[i][j]);
                        Main.CS_Draw_Node_AGV.t1Path.path[k].line = Convert.ToInt32(Grid_Table.Rows[i][j]);
                    }
                    else if (j == 4 || j == 7 || j == 10 || j == 13)
                    {
                        Main.CS_Draw_Node_AGV.dqPath[i].path[k].vel = Convert.ToInt32(Grid_Table.Rows[i][j]);
                        Main.CS_Draw_Node_AGV.t1Path.path[k].vel = Main.CS_Draw_Node_AGV.dqPath[i].path[k].vel;
                        k++;
                    }
                    Main.CS_Draw_Node_AGV.Draw_Path();
                }


            }
            //dispose used objects
            Grid_Table.Dispose();
            dataAdapter.Dispose();
            dbCommand.Dispose();

            excelConnection.Close();
            excelConnection.Dispose();
        }

    }
 }


