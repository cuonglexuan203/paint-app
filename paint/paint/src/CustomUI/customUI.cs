using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    partial class AppPaint
    {
        public void CustomizeButtonImage()
        {
            int borderWidth = 5;
            List<Button> buttons = new List<Button>() { BtnSave, BtnUndo, BtnRedo, BtnPaste, BtnCut, BtnCopy, BtnSelect, BtnCrop, BtnResize, BtnRotate, BtnFlip, BtnPencil, BtnFill, BtnText, BtnEraser, BtnColorPicker, BtnMagnifier, BtnBrush, BtnShape1, BtnShape2, BtnShape3, BtnShape4, BtnShape5, BtnShape6, BtnShape7, BtnShape8, BtnShape9, BtnShape10, BtnShape11, BtnShape12, BtnShape13, BtnShape14, BtnShape15, BtnShape16, BtnShape17, BtnShape18, BtnShape19, BtnShape20, BtnShape21, BtnShape22, BtnShapeOutline, BtnShapeFill, BtnSize, BtnEditColor };
            foreach (Button btn in buttons)
            {
                Image img = btn.Image;
                btn.Image = (Image)(new Bitmap(img, new Size(btn.Width - borderWidth * 2 , btn.Height - borderWidth * 2)));
            }
        }
    }
}
