using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    internal class CustomWidthButton: Button
    {
        private int barHeight = 10;
        private int barWidth = 80;
        public int BarHeight { get => barHeight; set => barHeight = value; }

        protected override void OnPaint( PaintEventArgs e)
        {
            base.OnPaint(e);
            this.Text = BarHeight.ToString() + " px";
            e.Graphics.FillRectangle(Brushes.Black, this.Width / 3 * 2 - barWidth / 2, this.Height / 9 * 5 - barHeight / 2, barWidth, barHeight); // 2/3 , 9/5 : appropriate ratio to align with text of button
        }
        
    }
}
