using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class AppPaint
    {

        private List<List<int>> penWidths = new List<List<int>>() {
        new List<int>{ 1, 2, 3 , 4 }, // pencil
        new List<int>{4, 6, 8, 10}, // eraser
        new List<int> { 1, 3, 5, 8 }, // shapes
        };
        int indexPenSize = 34;
        //
        public void CustomSizes(Panel pnls)
        {
            int i = 0;
            foreach (CustomWidthButton pnl in pnls.Controls)
            {
                
                if (indexPenSize == 34)
                {
                    pnl.BarHeight = penWidths[0][penWidths[0].Count - i - 1];
                }
                else if (indexPenSize == 35)
                {
                    pnl.BarHeight = penWidths[1][penWidths[1].Count - i - 1];
                }
                else if (indexPenSize >= 42 && indexPenSize <= 63)
                {
                    pnl.BarHeight = penWidths[2][penWidths[2].Count - i - 1];
                }
                i++;
            }
        }
        public void CustomizeUIs()
        {
            this.SuspendLayout();
            //
            DrawRectangleFrame(this);
            //
            InitColorsOfPen();
            CustomizeButtonImage(new List<Button>() { BtnSave, BtnUndo, BtnRedo, BtnPaste, BtnCut, BtnCopy, BtnSelect, BtnCrop, BtnResize,
                BtnRotate, BtnFlip, BtnPencil, BtnFill, BtnText, BtnEraser, BtnColorPicker, BtnMagnifier, BtnBrush,
                BtnShape1, BtnShape2, BtnShape3, BtnShape4, BtnShape5, BtnShape6, BtnShape7, BtnShape8, BtnShape9, BtnShape10,
                BtnShape11, BtnShape12, BtnShape13, BtnShape14, BtnShape15, BtnShape16, BtnShape17, BtnShape18, BtnShape19, BtnShape20,
                BtnShape21, BtnShape22, BtnShapeOutline, BtnSize, BtnEditColor, BtnPenDashStyle

            });
            CustomizeOptionButtonImage(new List<Button> { BtnFlipHor, BtnFlipVer, this.BtnRotateRight90, this.BtnRotateLeft90, this.BtnRotate180,
                this.BtnPenDashDotDot, this.BtnPenDashDot, this.BtnPenDash, this.BtnPenSolid, this.BtnPenDot, this.BtnBrushSolid, this.BtnLinearBrush,
                this.BtnSquareSelection, this.BtnObjectSelection});
            CustomizeBorderPanelColor(this.PnlControlDrawing, 1,0,1,0,Color.FromArgb(234,234,234));
            HideInitialControls(new List<Control> { this.PnlSize, this.PnlImageFlip, this.PnlRotateImage, this.PnlPenDashStyleOptions, this.PnlBrushOptions, this.PnlSelectOptions });
            DisableInitialControls(new List<Control> { BtnRedo, BtnUndo });
            //

            //
            this.ResumeLayout();

        }
        private void DrawRectangleFrame(Form app)
        {
            Bitmap bm = new Bitmap(app.Width, app.Height);
            Graphics g = Graphics.FromImage(bm);
            g.DrawRectangle(new Pen(Color.FromArgb(155, 164, 181)), new Rectangle(0, 0, app.Width - 1, app.Height - 1));
            app.BackgroundImage = bm;
        }
        public void InitColorsOfPen()
        {
            foreach (EclipseButton ebtn in FLBColorOptions.Controls)
            {
                int index = Convert.ToInt32(ebtn.Tag);
                if (index >= colors.Count)
                {
                    break;
                }
                ebtn.BackColor = this.colors[index];
            }
        }
        public void CustomizeButtonImage(List<Button> buttons)
        {
            int borderWidth = 5;
            foreach (Button btn in buttons)
            {
                Image img = btn.Image;
                btn.Image = (Image)(new Bitmap(img, new Size(btn.Width - borderWidth * 2, btn.Height - borderWidth * 2)));
            }
        }
        public void CustomizeOptionButtonImage(List<Button> buttons)
        {
            int borderWidth = 5;
            
            foreach (Button btn in buttons)
            {
                Image img = btn.Image;
                btn.Image = (Image)(new Bitmap(img, new Size((int)(btn.Width * 0.2 - borderWidth * 2), (int)(btn.Height * 0.6 - borderWidth * 2))));
            }
        }
        public void CustomizeBorderPanelColor(Panel pnl, int topWidth, int rightWidth, int bottomWidth, int leftWidth, Color color)
        {
            using (Graphics g = this.PnlControlDrawing.CreateGraphics())
            {
                Pen p = new Pen(color, topWidth);
                // top
                Point sp = pnl.Location;
                Point ep = new Point(sp.X + pnl.Width, sp.Y);
                g.DrawLine(p, sp, ep);
                // bottom
                p.Width = bottomWidth;
                sp.Y += pnl.Height;
                ep.Y += pnl.Height;
                g.DrawLine(p, sp, ep);
                // right
                p.Width = rightWidth;
                sp.X = ep.X;
                sp.Y = pnl.Location.Y;
                g.DrawLine(p, sp, ep);
                // left
                p.Width = leftWidth;
                sp.X = pnl.Location.X;
                ep.X = pnl.Location.X;
                g.DrawLine(p, sp, ep);

            }
        }
        public void DisableInitialControls(List <Control> controls)
        {
            foreach(Control control in controls)
            {
                control.Enabled = false;
            }
        }
        public void HideInitialControls(List<Control> ctls) // hide control when start
        {
            foreach (Control c in ctls)
            {
                c.Hide();
            }
        }
    }
}
