using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace paint
{
    
    public partial class AppPaint : Form
    {
        
        // data
        Bitmap mainBitmap;
        Graphics mainGraphic;
        Color mainColor;
        Color subColor;
        int selectedColor = 0; // 0: main , 1: sub
        Pen mainPen;
        Pen subPen;
        Pen mainEraser;
        ColorDialog mainColorDialog = new ColorDialog();
        // point
        Point pointX, pointY;
        int x, y, sx, sy, ix, iy; // s: scale, i: initial
        List<Point> points = new List<Point>();
        // point: shape
        bool hasRoot = false;
        Point rootPoint, endPoint; // root coordinate of the polygon, endPoint: the end point of previous line
        int countLine = 0;
        //
        // control var
        Point mouseOffset;
        int index = 34;
        bool painted = false;
        //
        List<Color> colors = new List<Color>() { 
        Color.FromArgb(0,0,0),
        Color.FromArgb(127,127,127),
        Color.FromArgb(136,0,21),
        Color.FromArgb(237,28,36),
        Color.FromArgb(255,137,29),
        Color.FromArgb(255,242,0),
        Color.FromArgb(34,177,76),
        Color.FromArgb(0,162,232),
        Color.FromArgb(63,72,204),
        Color.FromArgb(163,73,164),
        Color.FromArgb(249,249,249),
        Color.FromArgb(195,195,195),
        Color.FromArgb(185,122,87),
        Color.FromArgb(255, 174, 201),
        Color.FromArgb(255,201,14),
        Color.FromArgb(239,228,176),
        Color.FromArgb(181,230,29),
        Color.FromArgb(157,217,34),
        Color.FromArgb(112,145,189),
        Color.FromArgb(200,191,231),
        };



        //
        private void InitData()
        {
            int defaultWidth = 2;
            mainBitmap = new Bitmap(PcBMainDrawing.Width, PcBMainDrawing.Height);
            mainGraphic = Graphics.FromImage(mainBitmap);
            mainGraphic.Clear(Color.White);
            mainGraphic.SmoothingMode = SmoothingMode.AntiAlias;
            mainColor = Color.Black;
            subColor = Color.White;
            mainPen = new Pen(mainColor, defaultWidth);
            subPen = new Pen(subColor, defaultWidth);
            mainEraser = new Pen(Color.White, defaultWidth);
            PcBMainDrawing.Image = mainBitmap;
            
        }
        private void AppPaint_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.MaximumSize = new System.Drawing.Size(workingArea.Width, workingArea.Height);
            
            //
        }
        public AppPaint()
        {
            InitializeComponent();
            InitData();
            CustomizeUIs();
            //
            
        }
        // process logic
        // get the coordinate of mouse click follow by the ratio bitmap/picturebox
        public Point SetPoint(PictureBox pictureBox, Point point)
        {
            float xr = 1f * pictureBox.Image.Width / pictureBox.Width;
            float yr = 1f * pictureBox.Image.Height / pictureBox.Height;
            return new Point((int)(point.X * xr), (int)(point.Y * yr));
        }
        // Check fill pixel
        public void ValidateFillColor(Bitmap bm, Stack<Point> pixel, int x, int y, Color oldColor, Color newColor)
        {
            Color curColor = bm.GetPixel(x, y);
            if (curColor.ToArgb() == newColor.ToArgb())
            {
                return;
            }
            if (curColor.ToArgb() == oldColor.ToArgb())
            {
                pixel.Push(new Point(x, y));
                bm.SetPixel(x, y, newColor);
            }
        }
        // Fill
        public void FillUp(Bitmap bm, int x, int y, Color newColor)
        {
            Color oldColor = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, newColor);
            if (oldColor.ToArgb() == newColor.ToArgb())
            {
                return;
            }

            while (pixel.Count > 0)
            {
                Point p = pixel.Pop();
                if (p.X > 0 && p.Y > 0 && p.X < bm.Width - 1 && p.Y < bm.Height - 1)
                {
                    ValidateFillColor(bm, pixel, p.X, p.Y - 1, oldColor, newColor); // top
                    ValidateFillColor(bm, pixel, p.X + 1, p.Y, oldColor, newColor); // right
                    ValidateFillColor(bm, pixel, p.X, p.Y + 1, oldColor, newColor); // bottom
                    ValidateFillColor(bm, pixel, p.X - 1, p.Y, oldColor, newColor); // left
                }
            }
        }
        
        // PictureBox event
        private void PcBMainDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            painted = true;
            pointX = e.Location;
            ix = e.X;
            iy = e.Y;
            //
            //
            points.Clear(); // dispose for free-line
            points.Add(e.Location);
            //
            if (index == 47)
            {
                if (!hasRoot)
                {
                    rootPoint = e.Location; // the root of polygon
                    hasRoot = true;
                }
            }
        }
        private Point GetStartPoint()
        {
            Point point = pointX;
            Point epoint = new Point(x, y);
            if (sx < 0 && sy < 0)
            {
                point = epoint ;
            }
            else if (sx < 0)
            {
                point.X += sx; // sx now is negative
            }
            else if (sy < 0)
            {
                point.Y += sy; // sy now is negative
            }
            return point;
        }
        
        private void Handler_DrawShape(Graphics g)
        {
            switch (this.index)
            {
                case 42:
                    {
                        Point sp = pointX;
                        Point ep = new Point(x, y);
                        g.DrawLine(mainPen, sp, ep);
                        break;
                    }
                case 43:
                    {

                        break;
                    }
                case 44:
                    {

                        Size size = new Size(sx, sy);
                        g.DrawEllipse(mainPen, new Rectangle(pointX, size));
                        break;
                    }
                case 45:
                    {

                        Point startP = GetStartPoint();
                        Size size = new Size(Math.Abs(sx), Math.Abs(sy));
                        Rectangle rect = new Rectangle(startP, size);
                        g.DrawRectangle(mainPen, rect);
                        break;
                    }
                case 46:
                    {
                        Point startP = GetStartPoint();
                        Size size = new Size(Math.Abs(sx), Math.Abs(sy));
                        Rectangle rect = new Rectangle(startP, size);
                        g.DrawRoundedRectangle(mainPen, rect, 10);
                        break;
                    }
                case 47:
                    {
                        Point currentPoint = new Point(x, y);
                        g.DrawPolygon(mainPen, rootPoint, pointX, pointY, endPoint, currentPoint,ref countLine,ref hasRoot);
                        break;
                    }
                case 48:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawTriangle(mainPen, pointX, currentPoint);
                        break;
                    }
                case 49:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawRightTriangle(mainPen, pointX, currentPoint);
                        break;
                    }
                case 50:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawDiamond(mainPen, pointX, currentPoint);
                        break;
                    }
                case 51:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawPentagon(mainPen, pointX, currentPoint);
                        break;
                    }
                case 52:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawHexagon(mainPen, pointX, currentPoint);
                        break;
                    }
                case 53:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawRightArrow(mainPen, pointX, currentPoint);
                        break;
                    }
                case 54:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawLeftArrow(mainPen, pointX, currentPoint);
                        break;
                    }
                case 55:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawUpArrow(mainPen, pointX, currentPoint);
                        break;
                    }
                case 56:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawDownArrow(mainPen, pointX, currentPoint);
                        break;
                    }
                case 57:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawFourPointStar(mainPen, pointX, currentPoint);
                        break;
                    }
                case 58:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawFivePointStar(mainPen, pointX, currentPoint);
                        break;
                    }
                case 59:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        g.DrawSixPointStar(mainPen, pointX, currentPoint);
                        break;
                    }
                case 60:
                    {
                        
                        break;
                    }
                case 61:
                    {
                        break;
                    }
                case 62:
                    {
                        break;
                    }
                case 63:
                    {
                        break;
                    }
            }
        }
        private void PcBMainDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            pointY = e.Location;
            if (painted)
            {
                // endPoint now contain the coordinate of the end point of the pre line
                Handler_DrawShape(mainGraphic);
                if (index == 47)
                {
                    endPoint = e.Location; // save the current coordinate of the end point of the current line
                    countLine++;
                }
            }
            //points.Clear();
            painted = false;

        }

        private void PcBMainDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (painted)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);
                if (index == 34) // free-line
                {
                    points.Add(p);
                }
                else if (index == 35) // eraser
                {
                    points.Add(p);
                }
                else if (index == 36) // fill
                {
                    if (selectedColor == 0)
                    {
                        FillUp(mainBitmap, p.X, p.Y, mainColor);
                    }
                    else if (selectedColor == 1)
                    {
                        FillUp(mainBitmap, p.X, p.Y, subColor);
                    }
                }
            }

            PcBMainDrawing.Refresh();
            x = e.X;
            y = e.Y;
            sx = e.X - ix;
            sy = e.Y - iy;
        }

        private void PcBMainDrawing_Paint(object sender, PaintEventArgs e)
        {
            
            if (painted)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (points.Count > 1)
                {
                    if (index == 34)
                    {
                        if (selectedColor == 0)
                        {
                            mainGraphic.DrawCurve(mainPen, points.ToArray());
                        }
                        else if (selectedColor == 1)
                        {
                            mainGraphic.DrawCurve(subPen, points.ToArray());
                        }
                    }
                    if (index == 35)
                    {
                        mainGraphic.DrawCurve(mainEraser, points.ToArray());
                    }
                }
                
                Handler_DrawShape(e.Graphics);
            }
        }
        
        private void PcBMainDrawing_MouseClick(object sender, MouseEventArgs e)
        {

            if (this.index == 36)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);
                if(selectedColor == 0)
                {
                    FillUp(mainBitmap, p.X, p.Y, mainColor);
                }
                else if (selectedColor == 1)
                {
                    FillUp(mainBitmap, p.X, p.Y, subColor);
                }
            }
            else if (index == 37)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);
                Color cor = (Color)(mainBitmap.GetPixel(p.X, p.Y));
                SetMainColor(cor);
            }

        }

        // Btn event
       
        private void Handler_ColorChoice_Click(object sender, EventArgs e)
        {
            EclipseButton esBtn = (EclipseButton)sender;
            this.selectedColor = Convert.ToInt32(esBtn.Tag);
        }

        private void Handler_ColorOptions_Click(object sender, EventArgs e)
        {
            EclipseButton esBtn = (EclipseButton)(sender);
            if (selectedColor == 0)
            {
                SetMainColor(esBtn.BackColor);
            }
            else if (selectedColor == 1)
            {
                SetSubColor(esBtn.BackColor);
            }

        }

        private void Handler_ColorWheel_Click(object sender, EventArgs e)
        {
            mainColorDialog.ShowDialog();
            Color color = mainColorDialog.Color;
            if (selectedColor == 0)
            {
                SetMainColor(color);
            }
            else if (selectedColor == 1)
            {
                SetSubColor(color);
            }
        }
        private void Handler_Tools_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int tag = Convert.ToInt32(btn.Tag);
            //
            this.index = tag;
        }
        private void Handler_Shapes_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            this.index = Convert.ToInt32(btn.Tag);

        }
        private void Btn_Enter(object sender, EventArgs e)
        {
            int borderWidth = 5;
            Pen pen = new Pen(Color.FromArgb(195, 195, 195), 1);

            if (sender is Panel)
            {
                Panel panel = (Panel)sender;
                panel.BackColor = Color.FromArgb(240, 240, 240);
                panel.Refresh();
                Graphics graphic = panel.CreateGraphics();
                graphic.DrawRoundedRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1, borderWidth);
                return;
            }

            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(240, 240, 240);
            btn.Refresh();
            Graphics g = btn.CreateGraphics();
            g.DrawRoundedRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1, borderWidth);

        }

        private void Btn_Leave(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                Panel pnl = (Panel)sender;
                pnl.BackColor = pnl.Parent.BackColor;
                return;
            }
            Button btn = (Button)sender;
            btn.BackColor = btn.Parent.BackColor;

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        

        private void BtnExit_MouseEnter(object sender, EventArgs e)
        {
            this.BtnExit.ForeColor = Color.White;
            
        }

        

        private void BtnExit_MouseLeave(object sender, EventArgs e)
        {
            this.BtnExit.ForeColor = Color.Black;
        }

        private void PnlControlPenWidth_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlSize.Show();
            
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                return;
            }
            this.WindowState = FormWindowState.Minimized;
        }
        // Panel event
        private void PnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        private void PnlTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.WindowState = FormWindowState.Normal;
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset);
                this.Location = mousePos;
            }
        }
        private void PnlControlDrawing_Paint(object sender, PaintEventArgs e)
        {
            CustomizeBorderPanelColor(this.PnlControlDrawing, 1, 0, 1, 0, Color.FromArgb(234, 234, 234));

        }
        // additional method
        private void SetMainColor(Color color)
        {
            this.BtnMainColor1.BackColor = mainColor = mainPen.Color = color;
        }
        private void SetSubColor(Color color)
        {
            this.BtnMainColor2.BackColor = subColor = subPen.Color = color;
        }

    }
}
