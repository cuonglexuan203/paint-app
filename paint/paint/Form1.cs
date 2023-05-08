
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
using static paint.src.Layers.InterfaceLayer.Actions.PaintAction;
using paint.src.Layers.BussinessLayer;
using paint.src.Layers.InterfaceLayer.Actions;
using System.Windows.Documents;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Cursor = System.Windows.Forms.Cursor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using Cursors = System.Windows.Forms.Cursors;
using System.Windows;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Application = System.Windows.Forms.Application;

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
        List<int> savedPenWidths = new List<int> { 1, 1, 4, 1 }; // 0: mainPen, 1: subPen, 2: eraser, 3: shapes // mem storage
        Size mainPcbSize = new Size(1200, 500);
        // point
        Point pointX; // begining point when mouse down
        Point pointY; // ending point when mouse up
        int x, y; // current coordinate of mouse in mouse move event
        int sx, sy, ix, iy; // s: scale (set in mouse move event) , i: initial ( set in mouse down event )
        List<Point> points = new List<Point>();
        // point: shape47
        bool hasRoot = false;
        Point rootPoint, endPoint; // root coordinate of the polygon, endPoint: the end point of previous line
        int countLine = 0;
        //
        // control var
        bool isBrush = false;
        Point mouseOffset;  // var to control the drag-drop form
        int preIndex = 34;
        int index = 34;  // var to set the feature to client
        bool painted = false;
        string fileExt = "png"; // file extension
        // object storage
        List<PaintAction> mainActions; // reserve 
        List<PaintAction> untrackActions; // prepare for tracking actions and commit to mainActions list
        List<GraphicObject> mainGraphicObjects; // change; reference to graphic object in mainaction
        List<Tuple<PaintAction, List<Point>>> mainGroupActions; // reserve ( store group actions and offset of graphic objects in that group )
        //
        //
        List<Panel> autoHideControls;  // auto hide controls in this var list when click on the optionalPanels
        //
        // selected selection
        bool isClearSelectedGraphicObject = true; // handler for clearing the graphic of graphic objects
        bool isSelectingGraphicObjects = false; // select one or many graphic objects , control select
        bool isClickOnSelectedGraphicObject = false; // control click on selected graphic object to do something
        int selectedSelectionIndex; // seperately clone
        List<GraphicObject> selectedGraphicObjs;
        List<Point> clickedSelectedGraphicObjectOffsets = new List<Point>(); // offset of currently mouse position and upper-left of bounds
        // historic action
        int indexAction = -1; // init ( from 0 to list.Count - 1)
        // Key control
        bool ctrlKeypressed = false; //
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
        // indexes <= 20 use for handling logic , they can be the same for separate controls
        // 21 and afterward use for painting


        //
        private void InitData()
        {
            mainBitmap = new Bitmap(PcBMainDrawing.Width, PcBMainDrawing.Height);
            mainGraphic = Graphics.FromImage(mainBitmap);
            mainGraphic.Clear(SystemVariable.sysMainBitmapColor);
            mainGraphic.SmoothingMode = SmoothingMode.AntiAlias;
            mainColor = Color.Black;
            subColor = Color.White;
            mainPen = new Pen(mainColor, savedPenWidths[0]);
            subPen = new Pen(subColor, savedPenWidths[1]);
            mainEraser = new Pen(Color.White, savedPenWidths[2]);
            //
            mainSolidBrush = new SolidBrush(mainColor);
            // init for main linear gradient brush
            mainLinearGradientBrush = new LinearGradientBrush(new PointF(0, 0), new PointF(1, 1), mainColor, subColor);

            //
            PcBMainDrawing.Image = mainBitmap;
            PcBMainDrawing.Size = mainPcbSize;
            //
            //this.PnlDrawing.AutoScrollMinSize = this.PcBMainDrawing.Size;

            //
            autoHideControls = new List<Panel> { this.PnlSize, this.PnlImageFlip, this.PnlRotateImage, this.PnlPenDashStyleOptions
                , this.PnlBrushOptions, this.PnlSelectOptions }; // auto hide controls when click on the optional panels
            // init data storage
            mainActions = new List<PaintAction>();
            mainGraphicObjects = new List<GraphicObject>();
            untrackActions = new List<PaintAction>();
            mainGroupActions = new List<Tuple<PaintAction, List<Point>>>();
            // selection index 
            selectedSelectionIndex = 2;
            selectedGraphicObjs = new List<GraphicObject>();
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
            // add addintional events
            AddEvent_Controls_OnClick(new List<Control> { this.PnlContainer, this.PnlControlPaint }
            , new List<Action<object, EventArgs>> { HideControls }); // add on optional panel

            AddEvent_Controls_OnClick(new List<Control> { BtnPaste, BtnCut, BtnSelect, PnlSelectOptions, BtnPencil, BtnFill
                , BtnText, BtnEraser, BtnColorPicker, BtnMagnifier, PnlBrushOptions, FLPShape }
            , new List<Action<object, EventArgs>> { CancelSelectedObjects });
            //
            if (Keyboard.IsKeyDown(Key.A))
            {

            }
        }
        // process logic
        public void CancelSelectedObjects(object sender, EventArgs e) // The same Handler_CancelSelectingGraphicObject but it use to assign event for controls
        {
            if (isSelectingGraphicObjects)
            {
                Handler_CancelSelectingGraphicObject();
            }
            //
            PcBMainDrawing.Refresh(); // must be placed last
        }
        public void HideControls(object sender, EventArgs e)
        {

            foreach (Panel pnl in autoHideControls)
            {
                pnl.Hide();
            }
        }
        public void AddEvents_ForClick(Control c, List<Action<Object, EventArgs>> elist) // event list
        {
            foreach (Action<Object, EventArgs> ev in elist)
            {
                c.Click += new System.EventHandler(ev);
            }
        }

        public void AddEventHandlerForFullControls(Control parent, List<Action<object, EventArgs>> elist) // add event handler for the control include its child (full tree)
        {
            AddEvents_ForClick(parent, elist);
            foreach (Control c in parent.Controls)
            {
                AddEvents_ForClick(c, elist); // add all event handler in list for the control
                if (c.HasChildren)
                {
                    AddEventHandlerForFullControls(c, elist);
                }
            }
        }
        public void AddEvent_Controls_OnClick(List<Control> optionalControls, List<Action<object, EventArgs>> elist) // traverse all panel need to add event handler
        {
            foreach (Control c in optionalControls)
            {
                AddEventHandlerForFullControls(c, elist);
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
            switch (selectedOption)
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

        private Point GetStartPoint() // get top-left corner of the bound rectangle
        {
            Point point = pointX;
            Point epoint = new Point(x, y);
            if (sx < 0 && sy < 0)
            {
                point = epoint;
            }
            else if (sx < 0)
            {
                point.X += sx; // plus because sx now is negative
            }
            else if (sy < 0)
            {
                point.Y += sy; // // plus because sy now is negative
            }
            return point;
        }

        private void Handler_DrawShape(Graphics g, int index)
        {
            Pen selectedPen = new Pen(Color.Black, 1);
            if (selectedColor == 0)
            {
                selectedPen = mainPen;
            }
            else if (selectedColor == 1)
            {
                selectedPen = subPen;
            }
            switch (index) // local index
            {
                case 42:
                    {
                        Point sp = pointX;
                        Point ep = new Point(x, y);
                        g.DrawLine(selectedPen, sp, ep);
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(sp, ep, index, (Pen)GetSelectedPen().Clone(), isBrush, (SolidBrush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
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
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillEllipse(selectedBrush, rect);
                        }
                        else
                        {
                            g.DrawEllipse(selectedPen, rect);
                        }
                        if (g == mainGraphic)
                        {

                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
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
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillRectangle(selectedBrush, rect);

                        }
                        else
                        {
                            g.DrawRectangle(selectedPen, rect);
                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
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
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillRoundedRectangle(selectedBrush, rect, 10);

                        }
                        else
                        {
                            g.DrawRoundedRectangle(selectedPen, rect, 10);
                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 47: //
                    {
                        Point currentPoint = new Point(x, y);
                        g.DrawPolygon(selectedPen, rootPoint, pointX, pointY, endPoint, currentPoint, ref countLine, ref hasRoot);
                        //if(g == mainGraphic)
                        //{
                        //    GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush(SolidBrush), GetSelectedBrush().Clone());
                        //    PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                        //    AddPaintAction(temp);
                        //}
                        break;
                    }
                case 48:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            //
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillTriangle(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawTriangle(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            //
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 49:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillRightTriangle(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawRightTriangle(selectedPen, originalPoints[0], originalPoints[1]);


                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 50:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillDiamond(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawDiamond(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 51:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillPentagon(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawPentagon(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 52:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillHexagon(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawHexagon(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 53:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillRightArrow(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawRightArrow(selectedPen, originalPoints[0], originalPoints[1]);


                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 54:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillLeftArrow(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawLeftArrow(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 55:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillUpArrow(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawUpArrow(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 56:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillDownArrow(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawDownArrow(selectedPen, originalPoints[0], originalPoints[1]);


                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 57:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillFourPointStar(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawFourPointStar(selectedPen, originalPoints[0], originalPoints[1]);


                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 58:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillFivePointStar(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawFivePointStar(selectedPen, originalPoints[0], originalPoints[1]);

                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
                        }
                        break;
                    }
                case 59:
                    {
                        Point currentPoint = SetPoint(PcBMainDrawing, new Point(x, y));
                        Point[] originalPoints = CalcTopleftBottomright(pointX, currentPoint);
                        Rectangle rect = GenerateRectangle(originalPoints[0], originalPoints[1]);
                        if (isBrush)
                        {
                            if (selectedBrushIndex == 1)
                            {
                            }
                            else if (selectedBrushIndex == 2)
                            {
                                SetMainLinearGradientBrush(rect);
                            }
                            Brush selectedBrush = GetSelectedBrush();
                            g.FillSixPointStar(selectedBrush, originalPoints[0], originalPoints[1]);

                        }
                        else
                        {
                            g.DrawSixPointStar(selectedPen, originalPoints[0], originalPoints[1]);
                        }
                        if (g == mainGraphic)
                        {
                            GraphicObject curObj = new GraphicObject(rect, index, (Pen)GetSelectedPen().Clone(), isBrush, (Brush)GetSelectedBrush().Clone());
                            PaintAction temp = new PaintAction(PaintActionType.Draw, curObj);
                            AddPaintAction(temp);
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

        //
        /// <summary>
        /// Clear graphic
        /// </summary>
        private void ClearGraphicObjectGraphics(List<GraphicObject> clearedGraphicBbjects)
        {
            foreach (GraphicObject gobj in clearedGraphicBbjects)
            {
                if (gobj.Bound != null)
                {

                    using (SolidBrush sbr = new SolidBrush(SystemVariable.sysMainBitmapColor))
                    {
                        int width = (int)(gobj.MainPen?.Width != null ? gobj.MainPen?.Width : 2);
                        Rectangle temprect = new Rectangle(gobj.Bound.Location.X - width, gobj.Bound.Y - width, gobj.Bound.Size.Width + width * 2, gobj.Bound.Size.Height + width * 2);
                        mainGraphic.FillRectangle(sbr, temprect); // change 
                    }
                }

            }
        }

        // PCB mouse control
        private void PcBMainDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            // get coordinate
            painted = true;
            pointX = e.Location;
            // reset
            ix = e.X;
            iy = e.Y;
            sx = 0;
            sy = 0;
            //
            if (isSelectingGraphicObjects)
            {
                if (this.selectedSelectionIndex == 2)
                {

                    int clickedPosition = 0; // 4 edge
                    clickedSelectedGraphicObjectOffsets.Clear(); // clear only one time for each mouse down
                    for (int i = 0; i < selectedGraphicObjs.Count; i++)
                    {
                        if (!isClickOnSelectedGraphicObject) // clear only one time
                        {
                            if (selectedGraphicObjs[i].Bound != null && selectedGraphicObjs[i].Bound.Contains(pointX)) // click on the seletected objs 
                            {
                                isClickOnSelectedGraphicObject = true;
                            }
                        }
                        if (selectedGraphicObjs[i].Bound != null)
                        {
                            clickedSelectedGraphicObjectOffsets.Add(new Point(-(e.X - selectedGraphicObjs[i].Bound.X), -(e.Y - selectedGraphicObjs[i].Bound.Y)));
                        }
                    }
                    if (isClearSelectedGraphicObject && isClickOnSelectedGraphicObject) // clear only one time when click on the selected graphic object
                    {
                        ClearGraphicObjectGraphics(this.selectedGraphicObjs);
                        isClearSelectedGraphicObject = false;
                    }
                    //
                }
            }
            // draw
            else if (this.index == 34)
            {
                points.Clear(); // dispose for free-line
                points.Add(e.Location);
            }
            else if (index == 47)
            {
                if (!hasRoot)
                {
                    rootPoint = e.Location; // the root of polygon
                    hasRoot = true;
                }
            }
        }
        private void PcBMainDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            pointY = e.Location;
            if (painted)
            {
                // endPoint now contain the coordinate of the end point of the pre line
                if (!isSelectingGraphicObjects)
                {
                    Handler_DrawShape(mainGraphic, this.index);
                }
                //
                if (this.index == 28) // selection
                {
                    if (this.selectedSelectionIndex == 2)
                    {
                        Point curPoint = e.Location;
                        bool hasSelectedObject = AddSelectectedGraphicObject(curPoint);
                        if (hasSelectedObject)
                        {
                            isSelectingGraphicObjects = true;
                            // reserve preindex
                            SetIndex(preIndex);
                            SetIndex(-1000);
                        }
                    }

                }
                if (isSelectingGraphicObjects) // cancel selection
                {
                    bool outSelection = true;
                    foreach (GraphicObject gobj in selectedGraphicObjs)
                    {
                        if (gobj.Bound.Contains(e.Location))
                        {
                            outSelection = false;
                            break;
                        }
                    }
                    if (outSelection)
                    {
                        Handler_CancelSelectingGraphicObject();
                    }
                }
                //

                isClickOnSelectedGraphicObject = false;

                // Store additional information
                if (index == 34)
                {
                    GraphicObject newFreeLine = new GraphicObject(points.ToArray(), this.index, (Pen)GetSelectedPen().Clone(), true);
                    PaintAction paintAction = new PaintAction(PaintActionType.Draw, newFreeLine);
                    AddPaintAction(paintAction);
                }
                else if (index == 47)
                {
                    endPoint = e.Location; // save the current coordinate of the end point of the current line
                    countLine++;
                }

            }
            points.Clear();
            painted = false;

        }

        private void PcBMainDrawing_MouseMove(object sender, MouseEventArgs e)
        {

            if (painted)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);

                //
                if (this.index == 34) // free-line
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
                        //AddFilledPoint(p, mainColor);
                    }
                    else if (selectedColor == 1)
                    {
                        FillUp(mainBitmap, p.X, p.Y, subColor);
                        //AddFilledPoint(p, subColor);
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
                    else if (index == 35)
                    {
                        mainGraphic.DrawCurve(mainEraser, points.ToArray());
                    }

                }
                Handler_DrawShape(e.Graphics, this.index);
            }
            // selection logic
            // moving selected object

            if (isClickOnSelectedGraphicObject) // modify later: redundant
            {
                if (selectedSelectionIndex == 2)
                {
                    Handler_MovingGraphicObject_Click(e.Graphics, this.selectedGraphicObjs);
                }
            }
            else if (isSelectingGraphicObjects)
            {
                foreach (GraphicObject gobj in selectedGraphicObjs)
                {
                    if (gobj.Type == 0)
                    {
                        Handler_ReDrawGroupGraphicObject(e.Graphics, gobj);
                    }
                    else
                    {
                        Handler_ReDrawGraphicObject(e.Graphics, gobj);
                    }
                }
            }
            // draw frame of selected objects
            if (isSelectingGraphicObjects)
            {
                foreach (GraphicObject gobj in selectedGraphicObjs)
                {
                    Pen tempPen = new Pen(SystemVariable.sysSelectedGraphicObjectPenColor, SystemVariable.sysSelectedGraphicObjectPenWidth);
                    tempPen.DashStyle = SystemVariable.sysSelectedGraphicObjectPenDashStyle;
                    e.Graphics.DrawRectangle(tempPen, gobj.Bound);
                }
            }
        }

        private void Handler_ReDrawGraphicObject(Graphics g, GraphicObject gobj)
        {
            if (gobj.IsFreeObject)
            {

            }
            else
            {
                int type = gobj.Type;
                //
                bool isBrushed = gobj.IsBrush;
                if (gobj.MainBrush is LinearGradientBrush)
                {
                    LinearGradientBrush linearGradientBrush = (LinearGradientBrush)gobj.MainBrush;
                    gobj.MainBrush = new LinearGradientBrush(gobj.Bound, linearGradientBrush.LinearColors[0], linearGradientBrush.LinearColors[1], 0, true); // change the rectangle of brush to the gobj.bound 
                    // because this function is called when redrawing graphic object ( moving object )
                }
                Brush brush = gobj.MainBrush;
                //
                Point[] originalPoints = CalcTopleftBottomright(gobj.Bound);
                switch (type) // type === this.index
                {
                    case 42:
                        {
                            g.DrawLine(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            break;
                        }
                    case 43:
                        {
                            break;
                        }
                    case 44:
                        {
                            if (isBrushed)
                            {
                                g.FillEllipse(brush, gobj.Bound);
                            }
                            else
                            {
                                g.DrawEllipse(gobj.MainPen, gobj.Bound);
                            }
                            break;
                        }
                    case 45:
                        {
                            if (isBrushed)
                            {
                                g.FillRectangle(brush, gobj.Bound);
                            }
                            else
                            {
                                g.DrawRectangle(gobj.MainPen, gobj.Bound);
                            }
                            break;
                        }
                    case 46:
                        {
                            if (isBrushed)
                            {
                                g.FillRoundedRectangle(brush, gobj.Bound, 10);
                            }
                            else
                            {
                                g.DrawRoundedRectangle(gobj.MainPen, gobj.Bound, 10); // 10: virtual value
                            }
                            break;
                        }
                    case 47:
                        {

                            break;
                        }
                    case 48:
                        {
                            if (isBrushed)
                            {
                                g.FillTriangle(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawTriangle(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 49:
                        {
                            if (isBrushed)
                            {
                                g.FillRightTriangle(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawRightTriangle(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 50:
                        {
                            if (isBrushed)
                            {
                                g.FillDiamond(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawDiamond(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 51:
                        {
                            if (isBrushed)
                            {
                                g.FillPentagon(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawPentagon(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 52:
                        {
                            if (isBrushed)
                            {
                                g.FillHexagon(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawHexagon(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 53:
                        {
                            if (isBrushed)
                            {
                                g.FillRightArrow(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawRightArrow(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 54:
                        {
                            if (isBrushed)
                            {
                                g.FillLeftArrow(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawLeftArrow(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 55:
                        {
                            if (isBrushed)
                            {
                                g.FillUpArrow(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawUpArrow(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 56:
                        {
                            if (isBrushed)
                            {
                                g.FillDownArrow(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawDownArrow(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 57:
                        {
                            if (isBrushed)
                            {
                                g.FillFourPointStar(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawFourPointStar(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 58:
                        {
                            if (isBrushed)
                            {
                                g.FillFivePointStar(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawFivePointStar(gobj.MainPen, originalPoints[0], originalPoints[1]);
                            }
                            break;
                        }
                    case 59:
                        {
                            if (isBrushed)
                            {
                                g.FillSixPointStar(brush, originalPoints[0], originalPoints[1]);
                            }
                            else
                            {
                                g.DrawSixPointStar(gobj.MainPen, originalPoints[0], originalPoints[1]);
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
        }
        private Tuple<PaintAction, List<Point>> GetGroupAction(GraphicObject gobj)
        {
            Tuple<PaintAction, List<Point>> res = null;
            foreach (Tuple<PaintAction, List<Point>> tup in mainGroupActions)
            {
                if (tup.Item1.CurGObject.Bound.Size.Equals(gobj.Bound.Size))
                {
                    res = tup;
                    break;
                }
            }
            return res;
        }
        private void Handler_MovingGroupGraphicObject(Graphics g, GraphicObject boundContainer, Point offset)
        {
            Tuple<PaintAction, List<Point>> groupActionData = GetGroupAction(boundContainer);
            if (groupActionData != null)
            {
                List<GraphicObject> group = groupActionData.Item1.GetObjsAfterGroup();
                Point newPos;
                for (int i = 0; i < group.Count; i++)
                {
                    newPos = new Point(x, y);
                    newPos.Offset(offset);
                    newPos.Offset(groupActionData.Item2[i]);
                    group[i].Bound = new Rectangle(newPos, group[i].Bound.Size);
                    Handler_ReDrawGraphicObject(g, group[i]);
                }
                newPos = new Point(x, y);
                newPos.Offset(offset);
                //groupActionData.Item1.CurGObject.Bound = new Rectangle(newPos, groupActionData.Item1.CurGObject.Bound.Size);
                boundContainer.Bound = new Rectangle(newPos, boundContainer.Bound.Size);
            }
        }
        /// <summary>
        /// Change the coordinate of selected graphic object and redrawing them
        /// </summary>
        /// <param name="g"></param>
        private void Handler_MovingGraphicObject_Click(Graphics g, List<GraphicObject> ls)
        {
            for (int i = 0; i < ls.Count; i++)
            {
                Point newPos = new Point(x, y); // current position
                if (clickedSelectedGraphicObjectOffsets.Count == 0)
                {
                    return;
                }
                if (ls[i].Type == 0)
                {
                    Handler_MovingGroupGraphicObject(g, ls[i], clickedSelectedGraphicObjectOffsets[i]);
                }
                else
                {
                    newPos.Offset(clickedSelectedGraphicObjectOffsets[i]);
                    ls[i].Bound = new Rectangle(newPos, ls[i].Bound.Size);
                    Handler_ReDrawGraphicObject(g, ls[i]);
                }
            }
        }

        private void ResetDataSelectionMode()
        {
            untrackActions.Clear();
            // quite selection mode
            selectedGraphicObjs.Clear();
            clickedSelectedGraphicObjectOffsets.Clear();
            mainGraphicObjects.Clear();
            isSelectingGraphicObjects = false;
            isClearSelectedGraphicObject = true;
            SetIndex(preIndex);
        }
        private void Handler_ReDrawGroupGraphicObject(Graphics g, GraphicObject boundContainer)
        {
            Tuple<PaintAction, List<Point>> groupActionData = GetGroupAction(boundContainer);
            if (groupActionData != null)
            {
                List<GraphicObject> group = groupActionData.Item1.GetObjsAfterGroup();
                for (int i = 0; i < group.Count; i++)
                {
                    Handler_ReDrawGraphicObject(g, group[i]);
                }
            }
        }
        private bool CheckActionChange()
        {
            bool isNewActions = false; // only need to check the one action in list but we check all for completion
            foreach (PaintAction pac in untrackActions)
            {
                if (pac.CurGObject.Bound != null && pac.OldGObject.Bound != null && !pac.CurGObject.Bound.Location.Equals(pac.OldGObject.Bound.Location))
                {
                    pac.Type = PaintActionType.Moving;
                    isNewActions = true;
                }
                if (pac.CurGObject.Bound != null && pac.OldGObject.Bound != null && !pac.CurGObject.Bound.Size.Equals(pac.OldGObject.Bound.Size))
                {
                    pac.Type = PaintActionType.Resize;
                    isNewActions = true;
                }
                if (!isNewActions && pac.OldGObject != null && !pac.CurGObject.IsDeleted)
                {
                    pac.OldGObject.IsDeleted = false;
                }
            }
            return isNewActions;
        }
        /// <summary>
        /// Cancel selecting and redraw
        /// return true if add new actions, else false 
        /// </summary>
        private bool Handler_CancelSelectingGraphicObject()
        {
            // redraw selected graphic object
            if (isSelectingGraphicObjects)
            {
                foreach (GraphicObject gobj in selectedGraphicObjs)
                {
                    if (gobj.Type == 0)
                    {
                        Handler_ReDrawGroupGraphicObject(mainGraphic, gobj);
                    }
                    else
                    {
                        Handler_ReDrawGraphicObject(mainGraphic, gobj);
                    }
                }
                // add new paint actions
                bool isNewActions = CheckActionChange();

                if (isNewActions) // only add actions when there are something change
                {
                    AddRangePaintAction(untrackActions);
                }
                ResetDataSelectionMode();
                return isNewActions;
            }
            return false;
        }
        private void PcBMainDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            //
            if (this.index == 36)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);
                Color curColor = Color.Empty;
                if (selectedColor == 0)
                {
                    curColor = mainColor;
                }
                else if (selectedColor == 1)
                {
                    curColor = subColor;
                }
                FillUp(mainBitmap, p.X, p.Y, curColor);
                // Store additional information
                GraphicObject curObject = new GraphicObject(new Point[] { e.Location }, this.index, (Pen)GetSelectedPen().Clone());
                PaintAction paintAction = new PaintAction(PaintActionType.Fill, curObject);
                AddPaintAction(paintAction);
            }
            else if (index == 37)
            {
                Point p = SetPoint(PcBMainDrawing, e.Location);
                Color cor = (Color)(mainBitmap.GetPixel(p.X, p.Y));
                SetMainColor(cor); // need to modify to fit the requirment
            }

        }

        // Btn event

        private void Handler_ColorChoice_Click(object sender, EventArgs e)
        {
            EclipseButton esBtn = (EclipseButton)sender;
            this.selectedColor = Convert.ToInt32(esBtn.Tag);
            if (isBrush)
            {
                SetBrushColor(mainColor, subColor);
            }
        }

        private void Handler_ColorOptions_Click(object sender, EventArgs e)
        {
            EclipseButton esBtn = (EclipseButton)(sender);
            //
            if (selectedColor == 0)
            {
                SetMainColor(esBtn.BackColor);
            }
            else if (selectedColor == 1)
            {
                SetSubColor(esBtn.BackColor);
            }
            //
            if (isBrush)
            {
                SetBrushColor(mainColor, subColor);
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
        private void Handler_ChangeCursorIcon(int tag)
        {
            if (tag == 34)
            {
                this.PcBMainDrawing.Cursor = new Cursor(SystemVariable.sysAppPath + "\\src\\CustomUI\\Cursors\\freeLineCur.cur");
            }
            else if (tag == 35)
            {
                this.PcBMainDrawing.Cursor = new Cursor(SystemVariable.sysAppPath + "\\src\\CustomUI\\Cursors\\eraserCur.cur");
            }
            else if (tag == 36)
            {

                Bitmap bm = new Bitmap(Image.FromFile(SystemVariable.sysAppPath + "\\src\\CustomUI\\Cursors\\fillCur.png"), new Size(30, 30));
                Cursor cs = new Cursor(bm.GetHicon());
                this.PcBMainDrawing.Cursor = cs;
            }
            else if (tag == 37)
            {
                this.PcBMainDrawing.Cursor = new Cursor(SystemVariable.sysAppPath + "\\src\\CustomUI\\Cursors\\colorPickerCur.cur");
            }
            else if (tag >= 42 && tag <= 63)
            {
                Bitmap bm = new Bitmap(Image.FromFile(SystemVariable.sysAppPath + "\\src\\CustomUI\\Cursors\\shapeCur.png"), new Size(30, 30));
                Cursor cs = new Cursor(bm.GetHicon());
                this.PcBMainDrawing.Cursor = cs;
            }
            else
            {
                this.PcBMainDrawing.Cursor = Cursors.Cross;
            }
        }
        private void Handler_Tools_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int tag = Convert.ToInt32(btn.Tag);
            //
            this.indexPenSize = tag;
            if (this.index == -1000)
            {
                SetIndex(preIndex); // reserve preindex
            }
            SetIndex(tag);
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
        private void Handler_ImageControls_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int tag = Convert.ToInt32(btn.Tag);
            //
            this.indexPenSize = tag;
            if (this.index == -1000)
            {
                SetIndex(preIndex); // reserve preindex
            }
            SetIndex(tag);
            if (this.index == 28)
            {
                mainGraphicObjects = Get_GraphicObject_Selection();
            }
        }
        private void Handler_Shapes_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int tag = Convert.ToInt32(btn.Tag);
            indexPenSize = tag;
            if (this.index == -1000)
            {
                SetIndex(preIndex); // reserve preindex
            }
            SetIndex(tag);
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
            foreach (CustomWidthButton c in pnl.Controls)
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
                    this.mainPen.Width = savedPenWidths[0] = btn.BarHeight;
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
            Bitmap flipedBm = new Bitmap(newBm);
            mainGraphic = Graphics.FromImage(flipedBm);
            mainBitmap = flipedBm;
            PcBMainDrawing.Image = flipedBm;
        }

        private void BtnFlipHor_Click(object sender, EventArgs e)
        {
            Bitmap newBm = new Bitmap(PcBMainDrawing.Image);
            newBm.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //
            Bitmap flipedBm = new Bitmap(newBm);
            mainGraphic = Graphics.FromImage(flipedBm);
            mainBitmap = flipedBm;
            PcBMainDrawing.Image = flipedBm;
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
            //
            SetBrushColor(mainColor, subColor);
        }

        private void PnlBrush_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlBrushOptions.Show();
            this.PnlBrushOptions.Focus();
            this.PnlBrushOptions.Refresh();
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
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
            if (this.WindowState == FormWindowState.Minimized)
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

        private void RunPaintActions(int ifrom, int iend)
        {
            for (; ifrom <= iend; ifrom++)
            {

            }
        }
        private void Handler_UndoPaintAction(object sender, MouseEventArgs e)
        {
            indexAction--;
            this.BtnRedo.Enabled = indexAction + 1 < mainActions.Count; // of course but still check
            this.BtnUndo.Enabled = indexAction >= 0;
            //
            if (indexAction < 0)
            {
                return;
            }
            // RunPaintActions(0, indexAction);

        }

        // additional method
        private void SetMainColor(Color color)
        {
            this.BtnMainColor1.BackColor = mainColor = mainPen.Color = color;
            //

        }
        private void SetSubColor(Color color)
        {
            this.BtnMainColor2.BackColor = subColor = subPen.Color = color;
        }
        private void SetMainSolidBrushColor(Color color)
        {
            this.mainSolidBrush.Color = color;
        }
        private void SetMainLinearGradientBrushColor(Color start, Color end)
        {
            this.mainLinearGradientBrush.LinearColors = new Color[] { start, end };
        }
        private void SetBrushColor(Color main, Color sub)
        {
            if (selectedBrushIndex == 1)
            {
                if (selectedColor == 0)
                {
                    SetMainSolidBrushColor(main);
                }
                else if (selectedColor == 1)
                {
                    SetMainSolidBrushColor(sub);
                }
            }
            else if (selectedBrushIndex == 2)
            {
                SetMainLinearGradientBrushColor(main, sub);
            }
        }
        private void BtnSelectOptions_MouseClick(object sender, MouseEventArgs e)
        {
            this.PnlSelectOptions.Show();
            this.PnlSelectOptions.Focus();
            this.PnlSelectOptions.Refresh();
        }

        private void BtnObjectSelection_MouseClick(object sender, MouseEventArgs e)
        {


        }
        private List<GraphicObject> Get_GraphicObject_Selection()
        {
            List<GraphicObject> res = new List<GraphicObject>();
            for (int i = 0; i < mainActions.Count; i++) // get moved shape , not init shape
            {
                bool isAdd = true;
                if (mainActions[i].Type == PaintActionType.Delete)
                {
                    isAdd = false;
                }
                else if (mainActions[i].CurGObject == null) // no object to add ( or that is the delete action )
                {
                    isAdd = false;
                }
                else if (mainActions[i].CurGObject != null && mainActions[i].CurGObject.IsDeleted) // curgobj != null and isdelete : object was been changed
                {
                    isAdd = false;
                }
                //
                if (isAdd)
                {
                    res.Add(mainActions[i].CurGObject); // must be reference type to add historic changes afterward
                }
            }
            return res;
        }
        private void Handler_SelectionOptions_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            selectedSelectionIndex = Convert.ToInt32(btn.Tag);
            if (this.index == -1000)
            {
                SetIndex(preIndex); // reserve preindex
            }
            SetIndex(Convert.ToInt32(this.BtnSelect.Tag)); // 28 set for selection
            mainGraphicObjects = Get_GraphicObject_Selection();
        }



        private void PnlDrawing_MouseClick(object sender, MouseEventArgs e) // cancel selected object
        {
            if (isSelectingGraphicObjects)
            {
                Handler_CancelSelectingGraphicObject();
            }
        }

        private Pen GetSelectedPen()
        {
            Pen selectedPen = null;
            if (selectedColor == 0)
            {
                selectedPen = mainPen;
            }
            else if (selectedColor == 1)
            {
                selectedPen = subPen;
            }
            return selectedPen;
        }
        private void SetMainLinearGradientBrush(Rectangle rect)
        {
            if (rect.Width == 0 || rect.Height == 0)
            {
                rect.Width = 1;
                rect.Height = 1;
            }
            mainLinearGradientBrush = new LinearGradientBrush(rect, mainLinearGradientBrush.LinearColors[0], mainLinearGradientBrush.LinearColors[1], 0, true);
        }
        private void SetMainLinearGradientBrush(Rectangle rect, Color color1, Color color2, int angle)
        {
            mainLinearGradientBrush = new LinearGradientBrush(rect, color1, color2, angle, true);
        }

        private Brush GetSelectedBrush()
        {
            Brush selectedBrush = null;
            if (selectedBrushIndex == 1)
            {
                selectedBrush = mainSolidBrush;
            }
            else if (selectedBrushIndex == 2)
            {
                selectedBrush = mainLinearGradientBrush;
            }
            return selectedBrush;
        }
        /// <summary>
        /// Submit paint action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool AddPaintAction(PaintAction action)
        {
            mainActions.Add(action);
            indexAction++;
            //
            this.BtnUndo.Enabled = indexAction >= 0; // if == 0, a work has been done 
            //
            return mainActions.Count > 0;
        }
        /// <summary>
        /// Sumit paint actions
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        private bool AddRangePaintAction(List<PaintAction> ls)
        {
            mainActions.AddRange(ls);
            indexAction += ls.Count;
            //
            this.BtnUndo.Enabled = indexAction >= 0;
            //
            return mainActions.Count > 0;
        }

        private bool RemoveUnusePaintActions(int currentActionTag) // when user back to the previous step and add additional actions
        // then we remove unuse paint actions from that step to the end list
        {
            mainActions.RemoveRange(currentActionTag, mainActions.Count - currentActionTag);
            this.BtnRedo.Enabled = false;
            return mainActions.Count > 0;
        }

        private void BtnRedo_MouseClick(object sender, MouseEventArgs e)
        {
            indexAction++;
            this.BtnUndo.Enabled = this.indexAction >= 0;
            this.BtnRedo.Enabled = indexAction + 1 < mainActions.Count;
        }
        private void Handler_DeleteSelectedGraphicObjects()
        {
            if (isSelectingGraphicObjects)
            {
                //
                List<PaintAction> tempUntrackPaintAction = new List<PaintAction>(); // submit after untrackPaintAction
                foreach (PaintAction pac in untrackActions)
                {
                    // pac now has : curobj and oldobj but we don't need curobj because the delete action, so we get rid of curobj
                    pac.CurGObject.IsDeleted = true;
                    pac.OldGObject.IsDeleted = true; // ensure the oldgobj is set to deleted
                    //
                    PaintAction newpac = new PaintAction(pac.CurGObject, pac.OldGObject); // currentObj = null, but we store for the later purpose ( determine the old object )
                    newpac.Type = PaintActionType.Delete;
                    tempUntrackPaintAction.Add(newpac);
                }
                //
                bool graphicObjectChanged = Handler_CancelSelectingGraphicObject();
                foreach (PaintAction pac in tempUntrackPaintAction)
                {
                    if (graphicObjectChanged)
                    {
                        pac.OldGObject = pac.CurGObject;
                    }
                    pac.CurGObject = null; // release current graphic object
                }
                //
                List<GraphicObject> deletedGraphicObject = new List<GraphicObject>();
                foreach (PaintAction pac in tempUntrackPaintAction)
                {
                    deletedGraphicObject.Add(pac.OldGObject);
                }
                //Handler_CancelSelectingGraphicObject(); // submit selected graphic object to main action (reserve reference link among sibling graphic objects)
                ClearGraphicObjectGraphics(deletedGraphicObject);
                AddRangePaintAction(tempUntrackPaintAction); // submit delete action
            }
        }

        private void Handler_GroupingGraphicObjects(List<GraphicObject> ls)
        {
            if (isSelectingGraphicObjects)
            {
                if (ls.Count >= 2)
                {
                    PaintAction newPaintAction = new PaintAction();
                    newPaintAction.GroupObjects(ls);
                    ResetDataSelectionMode();
                    //
                    AddPaintAction(newPaintAction);
                    //
                    List<GraphicObject> objAfterGroup = newPaintAction.GetObjsAfterGroup();
                    List<Point> groupOffsets = new List<Point>(); // offset of graphic object within group compare to the common bound
                    foreach (GraphicObject gobj in objAfterGroup)
                    {
                        groupOffsets.Add(new Point(gobj.Bound.Location.X - newPaintAction.CurGObject.Bound.Location.X, gobj.Bound.Location.Y - newPaintAction.CurGObject.Bound.Location.Y));
                    }
                    mainGroupActions.Add(new Tuple<PaintAction, List<Point>>(newPaintAction, groupOffsets));
                }
            }
        }
        private void AppPaint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G && ctrlKeypressed) // group mode
            {
                if (selectedGraphicObjs.Count >= 2)
                {
                    List<GraphicObject> groupedGObjs = new List<GraphicObject>();
                    foreach (GraphicObject gobj in selectedGraphicObjs)
                    {
                        if (gobj.Type == 0)
                        {
                            Tuple<PaintAction, List<Point>> temp = GetGroupAction(gobj);
                            if (temp != null)
                            {
                                groupedGObjs.AddRange(temp.Item1.GetObjsAfterGroup());
                            }
                        }
                        else
                        {
                            groupedGObjs.Add(gobj);
                        }
                    }
                    Handler_GroupingGraphicObjects(groupedGObjs);
                    mainGraphicObjects = Get_GraphicObject_Selection();

                }
            }
            else if (e.KeyCode == Keys.ControlKey) // selection mode
            {
                ctrlKeypressed = true;
                //
                if (this.index != 28)
                {
                    if (this.index == -1000)
                    {
                        SetIndex(preIndex);
                    }
                    SetIndex(28);
                }
                mainGraphicObjects = Get_GraphicObject_Selection();
            }
            else if (e.KeyCode == Keys.Delete) //
            {
                Handler_DeleteSelectedGraphicObjects();
            }
        }
        private void AppPaint_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) //modify
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                if (preIndex == 28)
                {
                    if (isSelectingGraphicObjects)
                    {
                        SetIndex(-1000);
                    }
                    else
                    {
                        SetIndex(34);
                    }
                }
                else
                {
                    if (isSelectingGraphicObjects)
                    {
                        // reserve preindex
                        SetIndex(preIndex);
                        SetIndex(-1000);
                    }
                    else
                    {
                        SetIndex(preIndex);
                    }
                }
                ctrlKeypressed = false;
            }
        }
        private PaintAction GetCurrentPaintAction()
        {
            return (mainActions.Count > 0) ? mainActions[mainActions.Count - 1] : null;
        }

        private bool AddSelectectedGraphicObject(Point p)
        {

            foreach (GraphicObject gobj in selectedGraphicObjs)
            {
                if (gobj.Bound != null && gobj.Bound.Contains(p))
                {
                    return false;
                }
            }
            //
            foreach (GraphicObject gobj in mainGraphicObjects)
            {
                if (gobj.Bound != null && gobj.Bound.Contains(p))
                {
                    GraphicObject newGobj = gobj.Clone();
                    gobj.IsDeleted = true;
                    PaintAction newPaintAction = new PaintAction(newGobj, gobj);
                    newPaintAction.Type = PaintActionType.Untrack;
                    //
                    untrackActions.Add(newPaintAction);
                    selectedGraphicObjs.Add(newGobj); // after lose focus on moving graphic object, add this action to the mainActions
                    //ClearGraphicObjectGraphics(this.selectedGraphicObjs);
                    return true;
                }
            }
            return false; // no graphic object found for this selection
        }
        private Rectangle GenerateRectangle(Point upperLeft, Point bottomRight)
        => new Rectangle(upperLeft, new Size(Math.Abs(bottomRight.X - upperLeft.X), Math.Abs(bottomRight.Y - upperLeft.Y)));
        private Point[] CalcTopleftBottomright(Point p1, Point p2) // [0] topleft,[1] bottomright
        => new Point[] {new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
        ,new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y))};
        private Point[] CalcTopleftBottomright(Rectangle rect) // use new Point: not reference
        => new Point[] { new Point(rect.Location.X, rect.Location.Y), new Point(rect.Location.X + rect.Width, rect.Location.Y + rect.Height) };
        /// <summary>
        /// Set pre-index and index
        /// </summary>
        /// <param name="index"></param>
        private void SetIndex(int index)
        {
            // reset variable of old index
            if (this.index == 47)
            {
                hasRoot = false;
                countLine = 0;
            }
            // change to new index
            this.preIndex = this.index;
            this.index = index;
            // change cursor
            Handler_ChangeCursorIcon(this.index);

        }
    }
}


// fix: clear graphic when add more graphic object ( clear only the new added obj)