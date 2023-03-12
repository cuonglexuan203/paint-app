using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
        // brush
        int selectedBrushIndex = 1;
        SolidBrush mainSolidBrush;
        LinearGradientBrush mainLinearGradientBrush;
        //
        ColorDialog mainColorDialog = new ColorDialog();
        List<int> savedPenWidths = new List<int> { 1, 1, 4, 1 }; // 0: mainPen, 1: subPen, 2: eraser, 3: shapes
        Size mainPcbSize = new Size(1200, 500);
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
        bool isBrush = false;
        Point mouseOffset;  // var to control the drag-drop form
        int index = 34;  // var to set the feature to client
        bool painted = false;
        string fileExt = "png";
        //
        List<Panel> optionalPanels ;  // option for autoHideControls
        List<Panel> autoHideControls;  // auto hide controls in this var list when click on the optionalPanels
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
            mainBitmap = new Bitmap(PcBMainDrawing.Width, PcBMainDrawing.Height);
            mainGraphic = Graphics.FromImage(mainBitmap);
            mainGraphic.Clear(Color.White);
            mainGraphic.SmoothingMode = SmoothingMode.AntiAlias;
            mainColor = Color.Black;
            subColor = Color.White;
            mainPen = new Pen(mainColor, savedPenWidths[0]);
            subPen = new Pen(subColor, savedPenWidths[1]);
            mainEraser = new Pen(Color.White, savedPenWidths[2]);
            //
            mainSolidBrush = new SolidBrush(mainColor);
            // init for main linear gradient brush
            //
            PcBMainDrawing.Image = mainBitmap;
            PcBMainDrawing.Size = mainPcbSize;
            //
            //this.PnlDrawing.AutoScrollMinSize = this.PcBMainDrawing.Size;

            //
            autoHideControls = new List<Panel> { this.PnlSize, this.PnlImageFlip, this.PnlRotateImage, this.PnlPenDashStyleOptions, this.PnlBrushOptions }; // auto hide controls when click on the optional panels
            optionalPanels = new List<Panel> { this.PnlContainer, this.PnlControlPaint};
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
            AddEvent_Controls_OnClick();

            //

        }
        // process logic
        public void HideControls(object sender, EventArgs e)
        {
            
            foreach (Panel pnl in autoHideControls)
            {
                pnl.Hide();
            }
        }
        public void AddEventHandlerForAllControls(Control parent) // add event handler for the control include its child
        {
            foreach (Control c in parent.Controls)
            {
                c.Click += new System.EventHandler(HideControls);
                if (c.HasChildren)
                {
                    AddEventHandlerForAllControls(c);
                }
            }
        }
        public void AddEvent_Controls_OnClick() // traverse all panel need to add event handler
        {
            foreach (Panel p in optionalPanels)
            {
              AddEventHandlerForAllControls(p);
            }
        }

        private void Handler_PenDashStyleOptions_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int selectedOption = int.Parse(button.Tag.ToString());
            Pen pen = mainPen;
            isBrush = false;
            if (selectedColor == 1)
            {
                pen = subPen;
            }
            switch(selectedOption)
            {
                case 1:
                    {
                        pen.DashStyle = DashStyle.DashDotDot;
                        break;
                    }
                case 2:
                    {
                        pen.DashStyle = DashStyle.DashDot;
                        break;
                    }
                case 3:
                    {
                        pen.DashStyle = DashStyle.Dash;
                        break;
                    }
                case 4:
                    {
                        pen.DashStyle = DashStyle.Solid;
                        break;
                    }
                case 5:
                    {
                        pen.DashStyle = DashStyle.Dot;
                        break;
                    }
            }
            
        }

        private void Handler_SaveAsImage(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            fileExt = toolStripMenuItem.Tag as string;
            Handler_SaveImage(sender, e);
        }
        private void Handler_SaveImage(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Bitmap (*.bmp;*dib) | *.bmp;*dib | JPEG (*.jpg;*.jpeg;*.jpe;*.jfif) | *.jpg;*.jpeg;*.jpe;*.jfif | GIF (*.gif) | *.gif | TIFF (*.tif;*.tiff) | *.tif;*.tiff | PNG (*.png) | *.png";
            saveFile.RestoreDirectory = true;
            saveFile.DefaultExt = fileExt;   // it do not set the default ext
            saveFile.AddExtension = true;  
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Bitmap savedBm = mainBitmap.Clone(new Rectangle(0, 0, PcBMainDrawing.Width, PcBMainDrawing.Height), mainBitmap.PixelFormat);
                string extension = Path.GetExtension(saveFile.FileName);
                ImageFormat imageFormat = ImageFormat.Png;
                if (extension.Equals(".bmp", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Bmp;
                }
                else if (extension.Equals(".dib", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Bmp;
                }
                else if (extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (extension.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (extension.Equals(".jpe", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (extension.Equals(".jfif", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Gif;
                }
                else if (extension.Equals(".tif", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Tiff;
                }
                else if (extension.Equals(".tiff", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Tiff;
                }
                else if (extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageFormat = ImageFormat.Png;
                }
                savedBm.Save(saveFile.FileName, imageFormat);
            }
        }
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
            if (oldColor.ToArgb() == newColor.ToArgb()) // * to argb
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
            Pen selectedPen = new Pen(Color.Black, 1);
            if (selectedColor == 0)
            {
                selectedPen = mainPen;
            }
            else if(selectedColor == 1)
            {
                selectedPen = subPen;
            }
            switch (this.index)
            {
                case 42:
                    {
                        Point sp = pointX;
                        Point ep = new Point(x, y);
                        g.DrawLine(selectedPen, sp, ep);
                        break;
                    }                    
                case 43:
                    {

                        break;
                    }
                case 44:
                    {

                        Size size = new Size(sx, sy);
                        Rectangle rect = new Rectangle(pointX, size);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillEllipse(mainSolidBrush, rect);
                            }
                            
                        }
                        else
                        {
                            g.DrawEllipse(selectedPen, rect);
                        }
                        break;
                    }
                case 45:
                    {

                        Point startP = GetStartPoint();
                        Size size = new Size(Math.Abs(sx), Math.Abs(sy));
                        Rectangle rect = new Rectangle(startP, size);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillRectangle(mainSolidBrush, rect);
                            }
                            
                        }
                        else
                        {
                            g.DrawRectangle(selectedPen, rect);
                        }
                        break;
                    }
                case 46:
                    {
                        Point startP = GetStartPoint();
                        Size size = new Size(Math.Abs(sx), Math.Abs(sy));
                        Rectangle rect = new Rectangle(startP, size);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillRoundedRectangle(mainSolidBrush, rect, 10);
                                
                            }
                            
                            
                        }
                        else
                        {
                            g.DrawRoundedRectangle(selectedPen, rect, 10);
                        }
                        break;
                    }
                case 47:
                    {
                        Point currentPoint = new Point(x, y);
                        g.DrawPolygon(selectedPen, rootPoint, pointX, pointY, endPoint, currentPoint, ref countLine, ref hasRoot);
                        break;
                    }
                case 48:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillTriangle(mainSolidBrush, pointX, currentPoint);
                            }


                        }
                        else
                        {
                            g.DrawTriangle(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 49:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillRightTriangle(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawRightTriangle(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 50:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillDiamond(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawDiamond(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 51:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillPentagon(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawPentagon(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 52:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillHexagon(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawHexagon(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 53:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillRightArrow(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawRightArrow(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 54:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillLeftArrow(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawLeftArrow(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 55:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillUpArrow(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawUpArrow(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 56:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillDownArrow(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawDownArrow(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 57:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillFourPointStar(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawFourPointStar(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 58:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillFivePointStar(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawFivePointStar(selectedPen, pointX, currentPoint);
                        }
                        break;
                    }
                case 59:
                    {
                        Point currentPoint = new Point((int)x, (int)y);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                                g.FillSixPointStar(mainSolidBrush, pointX, currentPoint);
                            }

                        }
                        else
                        {
                            g.DrawSixPointStar(selectedPen, pointX, currentPoint);
                        }
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
            this.index = this.indexPenSize = tag;
            //
            if (tag == 34)
            {
                if (selectedColor == 0)
                {
                    this.mainPen.Width = savedPenWidths[0];
                }
                else if (selectedColor == 1)
                {
                    this.subPen.Width = savedPenWidths[1];
                }
            }
            else if (tag == 35)
            {
                this.mainEraser.Width = savedPenWidths[2];
            }
        }
        private void Handler_Shapes_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            this.index = indexPenSize = Convert.ToInt32(btn.Tag);
            //
            if (selectedColor == 0)
            {
            this.mainPen.Width = savedPenWidths[3];
            }
            else if (selectedColor == 1)
            {
                this.subPen.Width = savedPenWidths[3];

            }
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
        private void SetSelectedCustomWidthButton(int size, Panel pnl)
        {
            foreach(CustomWidthButton c in pnl.Controls)
            {
                if (c.BarHeight == size)
                {
                    c.BackColor = Color.FromArgb(238, 240, 243);
                }
                else
                {
                    c.BackColor = Color.White;
                }
            }
        }
        private void PnlControlPenWidth_MouseClick(object sender, MouseEventArgs e)
        {
            isBrush = false;
            CustomSizes(PnlSize);
            if (index == 34)
            {
                if (selectedColor == 0)
                {
                    SetSelectedCustomWidthButton((int)this.mainPen.Width, PnlSize);

                }
                else if (selectedColor == 1)
                {
                    SetSelectedCustomWidthButton((int)this.subPen.Width, PnlSize);
                }
            }
            else if (index == 35)
            {
                SetSelectedCustomWidthButton((int)this.mainEraser.Width, PnlSize);
            }
            this.PnlSize.Refresh();
            this.PnlSize.Show();
            this.PnlSize.Focus();
            
        }

        private void BtnPenSizes_Click(object sender, EventArgs e)
        {
            CustomWidthButton btn = ((CustomWidthButton)sender);
            if (indexPenSize == 34)
            {
                if (selectedColor == 0)
                {
                    this.mainPen.Width = savedPenWidths[0] =btn.BarHeight;
                }
                else if (selectedColor == 1)
                {
                    this.subPen.Width = savedPenWidths[1] = btn.BarHeight;
                }
            }
            else if (indexPenSize == 35)
            {
                this.mainEraser.Width = savedPenWidths[2] = btn.BarHeight;

            }
            else if (indexPenSize >= 42 && indexPenSize <= 63)
            {
                if (selectedColor == 0)
                {
                    this.mainPen.Width = savedPenWidths[3] = btn.BarHeight;
                }
                else if (selectedColor == 1)
                {
                    this.subPen.Width = savedPenWidths[3] = btn.BarHeight;

                }
            }
        }

        private void PnlControlPenWidth_Paint(object sender, PaintEventArgs e)
        {

        }

       
       
        private void PnlFlip_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlImageFlip.Show();
            this.PnlImageFlip.Focus();
            
        }

        private void BtnFlipVer_Click(object sender, EventArgs e)
        {
            Bitmap newBm = new Bitmap(PcBMainDrawing.Image);
            newBm.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //
            mainGraphic = Graphics.FromImage(newBm);
            PcBMainDrawing.Image = newBm;
        }

        private void BtnFlipHor_Click(object sender, EventArgs e)
        {
            Bitmap newBm = new Bitmap(PcBMainDrawing.Image);
            newBm.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //
            mainGraphic = Graphics.FromImage(newBm);
            PcBMainDrawing.Image = newBm;
        }

        private void PnlRotate_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlRotateImage.Show();
            this.PnlRotateImage.Focus();
        }

        private void BtnRotateRight90_Click(object sender, EventArgs e)
        {
            Bitmap rotatedBm = new Bitmap(PcBMainDrawing.Image);
            rotatedBm.RotateFlip(RotateFlipType.Rotate90FlipNone); // rotated bitmap will be swap the dimension ( width <-> height , x <-> y) 
            //
            Bitmap newBm = new Bitmap(rotatedBm); // back to the normal dimension
            PcBMainDrawing.Image = newBm;
            PcBMainDrawing.Location = new Point(0, 0);
            mainBitmap = newBm;
            mainGraphic = Graphics.FromImage(mainBitmap);
        }

        private void BtnRotateLeft90_Click(object sender, EventArgs e)
        {

            Bitmap rotatedBm = new Bitmap(PcBMainDrawing.Image);
            rotatedBm.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //
            Bitmap newBm = new Bitmap(rotatedBm); // back to the normal dimension
            PcBMainDrawing.Image = newBm;
            PcBMainDrawing.Location = new Point(0, 0);
            mainBitmap = newBm;
            mainGraphic = Graphics.FromImage(mainBitmap);
            
        }

        private void BtnRotate180_Click(object sender, EventArgs e)
        {
            Bitmap newBm = new Bitmap(PcBMainDrawing.Image);
            newBm.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //
            PcBMainDrawing.Image = newBm;
            PcBMainDrawing.Location = new Point(0, 0);
            mainBitmap = newBm;
            mainGraphic = Graphics.FromImage(mainBitmap);
            
        }

        private void BtnResize_MouseClick(object sender, MouseEventArgs e)
        {
            ResizeAndSkewForm resizeAndSkewForm = new ResizeAndSkewForm(PcBMainDrawing.Size);
            resizeAndSkewForm.Owner = this;
            resizeAndSkewForm.StartPosition = FormStartPosition.CenterParent;
            //
            resizeAndSkewForm.FormClosed += ResizeAndSkewForm_FormClosed;
            resizeAndSkewForm.ShowDialog();
            
        }

        private void ResizeAndSkewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResizeAndSkewForm resizeAndSkewForm = sender as ResizeAndSkewForm;
            int horResize = resizeAndSkewForm.HorResize;
            int verResize = resizeAndSkewForm.VertResize;
            int horSkew = resizeAndSkewForm.HorSkew;
            int verSkew = resizeAndSkewForm.VertSkew;
            //
            this.PcBMainDrawing.SizeMode = PictureBoxSizeMode.Zoom;
            Size newSize = new Size(horResize, verResize);
            this.PcBMainDrawing.Size = newSize;
            //
            mainBitmap = new Bitmap(mainBitmap, newSize);
            mainGraphic = Graphics.FromImage(mainBitmap);
            this.PcBMainDrawing.Image = mainBitmap;
            //
            //
            if (horSkew != 0 || verSkew != 0)
            {

            }
            PcBMainDrawing.Refresh();

        }

        private void MenuItemNew_Click(object sender, EventArgs e)
        {
            mainGraphic.Clear(Color.White);
            PcBMainDrawing.Refresh();
        }

        private void PnlPenDashStyle_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlPenDashStyleOptions.Show();
            this.PnlPenDashStyleOptions.Focus();
            this.PnlPenDashStyleOptions.Refresh();
        }
        private void Handler_BrushOptionsButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            selectedBrushIndex = int.Parse(button.Tag.ToString());
            isBrush = true;
            
            
        }

        private void PnlBrush_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlBrushOptions.Show();
            this.PnlBrushOptions.Focus();
            this.PnlBrushOptions.Refresh();
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
            this.BtnMainColor1.BackColor = mainColor = mainPen.Color = mainSolidBrush.Color = color;
        }
        private void SetSubColor(Color color)
        {
            this.BtnMainColor2.BackColor = subColor = subPen.Color = color;
        }

    }
}
