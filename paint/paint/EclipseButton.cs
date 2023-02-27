using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    internal class EclipseButton: Button
    {
        private Color borderColor = Color.Black;
        private int borderWidth = 3;

        public Color BorderColor { get => borderColor; set => borderColor = value; }
        public int BorderWidth { get => borderWidth; set => borderWidth = value; }

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
            
            //
            Pen pen = new Pen(BorderColor, borderWidth);
            
            GraphicsPath eclipse = new GraphicsPath();
            eclipse.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            e.Graphics.DrawPath(pen, eclipse);
            this.Region = new Region(eclipse);
            

            

        }
    }
}
