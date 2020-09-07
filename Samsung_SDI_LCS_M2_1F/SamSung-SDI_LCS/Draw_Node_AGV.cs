using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System;

namespace SDI_LCS
{
    public class Draw_Node_AGV
    {
        public DoubleBufferedPictureBox[] ImageAGV = new DoubleBufferedPictureBox[Form1.LGV_NUM];

        public Bitmap BitNodePath;
        public Bitmap BitNodePath_Traffic;
        public bool Node_Edit = false;
        public Bitmap[] BitAGV = new Bitmap[Form1.LGV_NUM];
        Graphics[] temp = new Graphics[Form1.LGV_NUM];
        Graphics[] drawAGV = new Graphics[Form1.LGV_NUM];
        SolidBrush Brush = new SolidBrush(Color.Transparent);
        public Form1 Main;

        //노드 그리기(사각형)   //원점 OX, OY-------------------------------------------
        public int OX = -1;  //3층 : 370
        public int OY = 580; //3층 : 600
        public int PIXELSIZE = 150;//3층 : 100

        public int OX_Traffic = 0;//870
        public int OY_Traffic = 1180;//-50;             //500
        public int PIXELSIZE_Traffic = 50;//112,25

        public const int GRIDSIZE = 40;
        public int MAXNODE = 2000;
        public const int NODECOUNT = 2000;

        public const int AGVBOXW = 10;    // AGV가 그려지는 박스의 크기
        public const int AGVBOXH = 10;

        public const int AGV_VERTICAL = 1100;   // AGV 본체의 세로 길이
        public const int AGV_HORIZON = 600;    // AGV 본체의 가로 길이

        int[] PathArr = new int[2000];// 출력으로 나오는 경로 저장 배열
        int[] PathArr2 = new int[2000];// 출력으로 나오는 경로 저장 배열

        //-------------------------------------------------------------------------------
        public struct stNode
        {
            public int index;      // 노드 번호
            public int ID;         // 노드 ID
            public float x;        // x 좌표
            public float y;        // y 좌표
            public int NodeType;
            public int Area;
        };

        public struct stSubPath
        {
            public int linkedIndex;    // 연결된 노드 번호
            public int line;           // 0 = 직선, 1 = 곡선(위), 2 = 곡선(아래)
            public int vel;            // 속도. 0~5 속까지
        };
        public class stPath
        {
            public int targetIndex;    // 타겟 노드 번호
            public int pathCnt;        // 연결된 경로 개수
            public stSubPath[] path;

            public stPath()
            {
                path = new stSubPath[4];
            }
        };
        public struct AGVPosition
        {
            public float x;
            public float y;
            public float t;
        }

        public class DoubleBufferedPictureBox : PictureBox
        {
            public DoubleBufferedPictureBox()
            {
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            }
        }

        public AGVPosition stAGVPosition;
        public stPath[] dqPath;
        public stNode[] dqNode;
        public stSubPath[] dqSubPath;
        public stNode GoalNode;
        public stNode StartNode;
        public stNode t1Node, n1Node;      // t1Node : 현재 노드, n1Node : 다음 노드
        public stPath t1Path, n1Path;      // t1Path : 현재 경로, n1Path : 다음 경로


        public Draw_Node_AGV(Form1 CS_Main)
        {
            Main = CS_Main;
            Main.ImageNode.Location = new Point(0, 0);
            //Main.Pic_Map.Location = new Point(0, 0);
            /*
            Main.Form_Map_Setting.PB_Map.Location = new Point(7, 128);
            Main.Pic_Map.Location = new Point(7, 128);
            */
            for (int i = 0; i < Form1.LGV_NUM; i++)
            {
                ImageAGV[i] = new DoubleBufferedPictureBox();
                ImageAGV[i].Width = 45;
                ImageAGV[i].Height = 45;
                ImageAGV[i].Parent = Main.ImageNode;
                ImageAGV[i].BackColor = Color.Transparent;//투명 하게
                BitAGV[i] = new Bitmap(ImageAGV[i].Width, ImageAGV[i].Height);
                //BitAGV[i].MakeTransparent(Color.Transparent);
                temp[i] = Graphics.FromImage(BitAGV[i]);
                temp[i].FillRectangle(Brush, 0, 0, ImageAGV[i].Width, ImageAGV[i].Height);
            }

            //레이어 기능
            Main.ImageNode.BackColor = Color.Transparent;//투명 하게
            Main.ImageNode.Parent = Main.Pic_Map;
            //------------------------------------------------------------
            dqPath = new stPath[MAXNODE];
            dqNode = new stNode[MAXNODE];
            dqSubPath = new stSubPath[MAXNODE];
            stAGVPosition = new AGVPosition();

            for (int i = 0; i < MAXNODE; i++)
            {
                dqPath[i] = new stPath();
                dqNode[i] = new stNode();
                dqSubPath[i] = new stSubPath();
                t1Path = new stPath();
                n1Path = new stPath();
                t1Node = new stNode();
                n1Node = new stNode();
            }

            BitNodePath = new Bitmap(Main.ImageNode.Width, Main.ImageNode.Height);
            //BitNodePath.MakeTransparent(Color.Transparent);
            //BitNodePath_Traffic.MakeTransparent(Color.Transparent);
        }
        public void AGV_Location(int Current, int LGV_No)
        {
            ReceiveNodeInfo(Current, ref StartNode);
            stAGVPosition.x = StartNode.x;
            stAGVPosition.y = StartNode.y;
            stAGVPosition.t = 0;
            DrawAGV(stAGVPosition.x, stAGVPosition.y, 0, LGV_No);
        }
        public void AGV_Location_Nav(int x, int y, int t, int LGV_No)
        {
            DrawAGV(x, y, t, LGV_No);
        }


        public void DrawAGV(double x, double y, double t, int LGV_No)
        {
            drawAGV[LGV_No] = Graphics.FromImage(BitAGV[LGV_No]);

            //그리기 선, 글씨체
            Pen blackPen = new Pen(Color.Black, 5);
            SolidBrush LimePen = new SolidBrush(Color.Lime);
            SolidBrush RedPen = new SolidBrush(Color.Red);
            SolidBrush YellowPen = new SolidBrush(Color.Yellow);
            SolidBrush VioletPen = new SolidBrush(Color.Violet);
            SolidBrush Red_Pen = new SolidBrush(Color.Red);

            Font drawFont = new Font("맑은 고딕", 13, FontStyle.Bold);

            SolidBrush drawBrush = new SolidBrush(Color.Black);

            SolidBrush RedBrush = new SolidBrush(Color.Black);
            long x1, y1, x2, y2, x3, y3, x4, y4, x5, y5;
            int bX, bY, nX, nY;
            float a, b;


            x /= PIXELSIZE;
            y /= PIXELSIZE;

            // 맵상에서의 AGV박스 공간의 시작위치( bX : 원점+현재x좌표,  bY : 원점-현재y좌표)
            bX = (int)(OX + x);
            bY = (int)(OY - y);

            nX = bX - ImageAGV[LGV_No].Width / 2;
            nY = bY - ImageAGV[LGV_No].Height / 2;

            a = 20;//  AGV_VERTICAL / (2 * PIXELSIZE);
            b = 10;// AGV_HORIZON / (2 * PIXELSIZE);         // 차체 너비, 높이. a=10, b=5.3
            t = (-t - 270) * 3.14 / 180;

            x1 = (int)(-b * Math.Cos(t)) + (int)(a * Math.Sin(t)) + ImageAGV[LGV_No].Width / 2;
            y1 = (int)(-b * Math.Sin(t)) - (int)(a * Math.Cos(t)) + ImageAGV[LGV_No].Height / 2;
            x2 = (int)(b * Math.Cos(t)) + (int)(a * Math.Sin(t)) + ImageAGV[LGV_No].Width / 2;
            y2 = (int)(b * Math.Sin(t)) - (int)(a * Math.Cos(t)) + ImageAGV[LGV_No].Height / 2;
            x3 = (int)(b * Math.Cos(t)) - (int)(a * Math.Sin(t)) + ImageAGV[LGV_No].Width / 2;
            y3 = (int)(b * Math.Sin(t)) + (int)(a * Math.Cos(t)) + ImageAGV[LGV_No].Height / 2;
            x4 = (int)(-b * Math.Cos(t)) - (int)(a * Math.Sin(t)) + ImageAGV[LGV_No].Width / 2;
            y4 = (int)(-b * Math.Sin(t)) + (int)(a * Math.Cos(t)) + ImageAGV[LGV_No].Height / 2;
            x5 = x1;
            y5 = y1;

            Point P_xy1 = new Point((int)x1, (int)y1);
            Point P_xy2 = new Point((int)x2, (int)y2);
            Point P_xy3 = new Point((int)x3, (int)y3);
            Point P_xy4 = new Point((int)x4, (int)y4);
            Point P_xy5 = new Point((int)x5, (int)y5);

            Point[] Showagv = { P_xy1, P_xy2, P_xy3, P_xy4, P_xy5 };

            // AGV 그리기
            drawAGV[LGV_No].Clear(Color.Transparent);
            drawAGV[LGV_No].DrawPolygon(blackPen, Showagv);

            //트래픽
            if (Main.m_stAGV[LGV_No].state == 7)
            {
                drawAGV[LGV_No].FillPolygon(VioletPen, Showagv);
            }
            //수동
            else if (Main.m_stAGV[LGV_No].mode == 0)
            {
                drawAGV[LGV_No].FillPolygon(YellowPen, Showagv);
            }
            //자동
            else if (Main.m_stAGV[LGV_No].mode == 1)
            {
                drawAGV[LGV_No].FillPolygon(LimePen, Showagv);
            }
            //에러
            if (Main.m_stAGV[LGV_No].Error > 0)
            {
                drawAGV[LGV_No].FillPolygon(Red_Pen, Showagv);
            }

            drawAGV[LGV_No].DrawString(Convert.ToString(LGV_No + 1), drawFont, drawBrush, 14, 9);


            // AGV 이동
            ImageAGV[LGV_No].Location = new Point(nX, nY);

            ImageAGV[LGV_No].Image = BitAGV[LGV_No];
        }

        //-------------------------------------------------------------------------------

        //선 그리기------------------------------------------------------------------------
        public void DrawLine_Traffic(int x1, int y1, int x2, int y2, int line, int ps)
        {
            Graphics drawLine = Graphics.FromImage(BitNodePath_Traffic);
            Pen blackPen = new Pen(Color.Yellow, 2);

            x1 /= PIXELSIZE_Traffic;
            y1 /= PIXELSIZE_Traffic;
            x2 /= PIXELSIZE_Traffic;
            y2 /= PIXELSIZE_Traffic;

            Rectangle rect = new Rectangle(OX + x1, OY - y1, OX + x2, OY - y2);
            if ((x1 != 0 && y1 != 0) && (x2 != 0 && y2 != 0))
            {
                drawLine.DrawLine(blackPen, OX_Traffic + x1, OY_Traffic - y1, OX_Traffic + x2, OY_Traffic - y2);
            }

        }


        //-------------------------------------------------------------------------------
        //경로 그리기===============================================================================
        public void Draw_Path()
        {

            for (int i = 0; i < MAXNODE; i++)
            {
                ReadyToDrawLine(i);
            }

            Main.ImageNode.Image = BitNodePath;
        }
        //=======================================================================================================
        public void ReadyToDrawLine(int sindex)
        {   // 연결선 그리기 전 세팅 작업

            float x1, y1, x2, y2;
            int line = 9, ps = 0;               // 양방향(선) = 1, 단방향(점선) = 2
//            int nindex, temp;

            ReceiveNodeInfo(sindex, ref t1Node);           // 출발 노드 정보
            ReceivePathInfo(sindex, ref t1Path);           // 출발 경로 정보

            for (int i = 0; i < t1Path.pathCnt; i++)         // 출발 노드의 연결 노드 순환
            {
                ReceiveNodeInfo(t1Path.path[i].linkedIndex, ref n1Node);       // 이전 노드도 읽고
                ReceivePathInfo(t1Path.path[i].linkedIndex, ref n1Path);       // 이전 노드 경로도 읽고

                //for (int j = 0; j < n1Path.pathCnt; j++)
                {
                    x1 = t1Node.x;
                    y1 = t1Node.y;
                    x2 = n1Node.x;
                    y2 = n1Node.y;
                    line = t1Path.path[i].line; ps = 2;
                    if (x2 != 0 && y2 != 0)
                    {
                        DrawLine((int)x1, (int)y1, (int)x2, (int)y2, line, ps);
                        DrawLine_Traffic((int)x1, (int)y1, (int)x2, (int)y2, line, ps);
                    }
                }
            }
        }
        //--------------------------------------------------------------------------------------
        //선 그리기------------------------------------------------------------------------
        public void DrawLine(int x1, int y1, int x2, int y2, int line, int ps)
        {
            Graphics drawLine = Graphics.FromImage(BitNodePath);
            Pen blackPen = new Pen(Color.Yellow, 2);

            x1 /= PIXELSIZE;
            y1 /= PIXELSIZE;
            x2 /= PIXELSIZE;
            y2 /= PIXELSIZE;

            Rectangle rect = new Rectangle(OX + x1, OY - y1, OX + x2, OY - y2);
            if (line == 0 || line == 5 || line == 6 || line == 7 || line == 8 || line == 9)           // 직선 그리기
            {
                // 양방향 경로
                //
                if ((x1 != 0 && y1 != 0) && (x2 != 0 && y2 != 0))
                {
                    drawLine.DrawLine(blackPen, OX + x1, OY - y1, OX + x2, OY - y2);

                }

            }
            else
            {
                // Draw curve to screen.
                // Draw arc to screen.

                if ((x1 != 0 && y1 != 0) && (x2 != 0 && y2 != 0))
                {
                    drawLine.DrawLine(blackPen, OX + x1, OY - y1, OX + x2, OY - y2);
                }
            }

        }

        //노드 그리기===================================================================================
        public void Draw_Node_Traffic()
        {
            for (int i = 0; i < MAXNODE; i++)
            {
                if ((int)dqNode[i].x != 0 && (int)dqNode[i].y != 0)
                {
                    DrawNode_Traffic(dqNode[i].index, (int)dqNode[i].x, (int)dqNode[i].y);
                }

            }

        }

        //노드 그리기-------------------------------------------------------------------------
        public void DrawNode_Traffic(int index, int x, int y)
        {
            Graphics drawNode = Graphics.FromImage(BitNodePath_Traffic);
            Pen myPen;
            SolidBrush myBrush;

            // 노드 지점 그리기
            x /= PIXELSIZE_Traffic;
            y /= PIXELSIZE_Traffic;

            Font drawFont = new Font("맑은 고딕", 10, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            myBrush = new SolidBrush(Color.FromArgb(255, 0, 255,0));//.Lime);
            myPen = new Pen(Color.Black, 1);
            myPen.Width = 2;

            drawNode.DrawEllipse(myPen, new Rectangle(OX_Traffic + x - (5 / 2), OY_Traffic - y - (5 / 2), 7, 7));
            drawNode.FillEllipse(myBrush, new Rectangle(OX_Traffic + x - (5 / 2), OY_Traffic - y - (5 / 2), 7, 7));
            //drawNode.DrawString(Convert.ToString(index), drawFont, drawBrush, OX_Traffic + x + 6, OY_Traffic - y - 18);
            myPen.Dispose();
            drawNode.Dispose();
        }
        //--------------------------------------------------------------------------------------

        //노드 그리기===================================================================================
        public void Draw_Node()
        {
            for (int i = 0; i < MAXNODE; i++)
            {
                if ((int)dqNode[i].x != 0 && (int)dqNode[i].y != 0)
                {
                    DrawNode(dqNode[i].index, (int)dqNode[i].x, (int)dqNode[i].y);
                }

            }
            Main.ImageNode.Image = BitNodePath;
        }

        //노드 그리기-------------------------------------------------------------------------
        public void DrawNode(int index, int x, int y)
        {
            Graphics drawNode = Graphics.FromImage(BitNodePath);
            Pen myPen;
            SolidBrush myBrush;

            // 노드 지점 그리기
            x /= PIXELSIZE;
            y /= PIXELSIZE;

            Font drawFont = new Font("맑은 고딕", 10, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            myBrush = new SolidBrush(Color.Lime);
            myPen = new Pen(Color.Black, 1);
            myPen.Width = 2;

            drawNode.DrawEllipse(myPen, new Rectangle(OX + x - (5 / 2), OY - y - (5 / 2), 7, 7));
            drawNode.FillEllipse(myBrush, new Rectangle(OX + x - (5 / 2), OY - y - (5 / 2), 7, 7));
            //drawNode.DrawString(Convert.ToString(index), drawFont, drawBrush, OX + x + 6, OY - y - 18);
            myPen.Dispose();
            drawNode.Dispose();
        }



        //----------------------------------------------------------------------------
        public void AddNode(stNode tempNode)
        {
            // 노드 정보를 구조체 배열에 저장
            int index = tempNode.index;

            dqNode[index].index = index;
            dqNode[index].ID = tempNode.ID;
            dqNode[index].x = tempNode.x;
            dqNode[index].y = tempNode.y;
        }


        //---------------------------------------------------------------------------
        public void ReceiveNodeInfo(int index, ref stNode tempNode)
        {
            // 가지고 있는 index번의 노드 구조체 정보를 tempPath에 복사

            tempNode.index = dqNode[index].index;       // 해당 index의 노드 값 받아가기
            tempNode.ID = dqNode[index].ID;
            tempNode.x = dqNode[index].x;
            tempNode.y = dqNode[index].y;
        }
        public void ReceivePathInfo(int index, ref stPath tempPath)
        {   // 가지고 있는 index번의 경로 구조체 정보를 tempPath에 복사

            tempPath.targetIndex = dqPath[index].targetIndex;
            tempPath.pathCnt = dqPath[index].pathCnt;
            for (int i = 0; i < dqPath[index].pathCnt; i++)
            {
                tempPath.path[i].linkedIndex = dqPath[index].path[i].linkedIndex;
                tempPath.path[i].line = dqPath[index].path[i].line;
                tempPath.path[i].vel = dqPath[index].path[i].vel;
            }
        }

    }
}
