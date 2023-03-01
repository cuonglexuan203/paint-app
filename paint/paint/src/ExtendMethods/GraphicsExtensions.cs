using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    internal static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen,  int x, int y, int width, int height, int borderRadius)
        {
            
            float diameter = borderRadius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(x, y, size.Width, size.Height);
            //
            //top-left
            graphics.DrawArc(pen, arc, 180, 90);
            //top-right
            arc.X = x + width - diameter;
            graphics.DrawArc(pen,arc, 270, 90);
            //bottom-right
            arc.Y = y + height - diameter;
            graphics.DrawArc(pen, arc, 0, 90);
            // bottom-left
            arc.X = x;
            graphics.DrawArc(pen, arc, 90, 90);
            
            // draw edges
            // top
            graphics.DrawLine(pen, x + borderRadius, y, x + width - borderRadius, y);
            // bottom
            graphics.DrawLine(pen, x + borderRadius, y + height, x + width - borderRadius, y + height);
            // right
            graphics.DrawLine(pen, x + width, y + borderRadius, x + width , y + height - borderRadius);
            // left
            graphics.DrawLine(pen, x , y + borderRadius, x , y + height - borderRadius);


        }

    }
}
