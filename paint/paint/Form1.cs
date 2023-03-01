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

namespace paint
{
    
    public partial class AppPaint : Form
    {
        // data
        int index = 1;
        Bitmap mainBitmap;
        Graphics mainGraphic;
        Color mainColor;
        Pen mainPen;
        Pen mainEraser;
        ColorDialog mainColorDialog = new ColorDialog();
        bool painted = false;
        // point
        Point pointX, pointY;
        int x, y, sx, sy, ix, iy; // s: scale, i: initial
        //



        //
        



        //
        public AppPaint()
        {
            InitializeComponent();
            int defaultWidth = 2;
            mainBitmap = new Bitmap(PcBMainDrawing.Width, PcBMainDrawing.Height);
            mainGraphic  = Graphics.FromImage(mainBitmap);
            mainGraphic.Clear(Color.White);
            mainPen = new Pen(Color.Black, defaultWidth);
            mainEraser = new Pen(Color.White, defaultWidth);
            PcBMainDrawing.Image = mainBitmap;
            //

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
            Button btn = ( Button )sender;
            btn.BackColor = btn.Parent.BackColor;

        }

        private void PcBMainDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            painted = true;
            pointY = e.Location;
            ix = e.X;
            iy = e.Y;
        }

        private void PcBMainDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            if (painted)
            {
                
            }
            painted = false;

        }

        private void PcBMainDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (painted)
            {

                if (index == 1)
                {
                    pointX = e.Location;
                    mainGraphic.DrawLine(mainPen, pointX, pointY);
                    pointY = pointX;
                }
                if (index == 2)
                {
                    pointX = e.Location;
                    mainGraphic.DrawLine(mainEraser, pointX, pointY);
                    pointY = pointX;
                }
            }
            PcBMainDrawing.Refresh();
            x = e.X;
            y = e.Y;
            sx = e.X - x;
            sy = e.Y - y;
        }

        private void PcBMainDrawing_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PcBMainDrawing_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            index = 2; 
        }

        private void BtnPencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void Btn_Click(object sender, EventArgs e)
        {

        }

        private void AppPaint_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.MaximumSize = new System.Drawing.Size(workingArea.Width, workingArea.Height);
            //
            
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

        
    }
}
