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
            CustomizeButtonImage();
            CustomizeBorderPanelColor(this.PnlControlDrawing, 1,0,1,0,Color.FromArgb(234,234,234));
            InitColorsOfPen();
            HideControls(new List<Control> { this.PnlSize });
            this.ResumeLayout();
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
        public void CustomizeButtonImage()
        {
            int borderWidth = 5;
            List<Button> buttons = new List<Button>() { BtnSave, BtnUndo, BtnRedo, BtnPaste, BtnCut, BtnCopy, BtnSelect, BtnCrop, BtnResize, BtnRotate, BtnFlip, BtnPencil, BtnFill, BtnText, BtnEraser, BtnColorPicker, BtnMagnifier, BtnBrush, BtnShape1, BtnShape2, BtnShape3, BtnShape4, BtnShape5, BtnShape6, BtnShape7, BtnShape8, BtnShape9, BtnShape10, BtnShape11, BtnShape12, BtnShape13, BtnShape14, BtnShape15, BtnShape16, BtnShape17, BtnShape18, BtnShape19, BtnShape20, BtnShape21, BtnShape22, BtnShapeOutline, BtnShapeFill, BtnSize, BtnEditColor };
            foreach (Button btn in buttons)
            {
                Image img = btn.Image;
                btn.Image = (Image)(new Bitmap(img, new Size(btn.Width - borderWidth * 2, btn.Height - borderWidth * 2)));
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
        public void HideControls(List<Control> ctls)
        {
            foreach (Control c in ctls)
            {
                c.Hide();
            }
        }
    }
}
