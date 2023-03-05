
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace paint
{
    internal class RoundButton: Button
    {
        private static Color borderColor = Color.Black;
        private int borderWidth = 3;
        private int borderRadius = 3;
        private Pen pen = new Pen(borderColor, 2);
        public Color BorderColor { get => borderColor; set => borderColor = value; }
        public int BorderWidth { get => borderWidth; set => borderWidth = value; }
        public int BorderRadius { get => borderRadius; set => borderRadius = value; }
        


        //
        protected override void InitLayout()
        {
            base.InitLayout();
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Text = "";
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //int x = this.Location.X;
            //int y = this.Location.Y;
            ////
            //float diameter = BorderRadius * 2;
            //SizeF size = new SizeF(diameter, diameter);
            //RectangleF arc = new RectangleF(x, y, size.Width, size.Height);
            //GraphicsPath roundPath = new GraphicsPath();
            ////
            ////top-left
            //roundPath.AddArc( arc, 180, 90);
            ////top-right
            //arc.X = x + this.Width - diameter;
            //roundPath.AddArc( arc, 270, 90);
            ////bottom-right
            //arc.Y = y + this.Height - diameter;
            //roundPath.AddArc( arc, 0, 90);
            //// bottom-left
            //arc.X = x;
            //roundPath.AddArc( arc, 90, 90);

            //// draw edges
            //// top
            //roundPath.AddLine( x + BorderRadius, y, x + this.Width - BorderRadius, y);
            //// bottom
            //roundPath.AddLine( x + BorderRadius, y + this.Height, x + this.Width - BorderRadius, y + this.Height);
            //// right
            //roundPath.AddLine( x + this.Width, y + BorderRadius, x + this.Width, y + this.Height - BorderRadius);
            //// left
            //roundPath.AddLine( x, y + BorderRadius, x, y + this.Height - BorderRadius);
            ////
            //e.Graphics.DrawPath(pen, roundPath);
            //this.Region  = new Region(roundPath);
        }
    }
}
