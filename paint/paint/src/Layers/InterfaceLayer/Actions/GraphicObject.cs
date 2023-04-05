using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint.src.Layers.InterfaceLayer.Actions
{
    public class GraphicObject
    {
        Rectangle bound = Rectangle.Empty;
        int type = 0;
        Pen mainPen; 
        bool isBrush = false;
        SolidBrush solidBrush = null;
        //
        public Rectangle Bound { get => bound; set => bound = value; }
        public int Type { get => type; set => type = value; }
        public Pen MainPen { get => mainPen; set => mainPen = value; }
        public bool IsBrush { get => isBrush; set => isBrush = value; }
        public SolidBrush SolidBrush { get => solidBrush; set => solidBrush = value; }

        //
        public GraphicObject()
        {

        }
        public GraphicObject(Point topLeft, Point bottomRight, int tag, Pen mainPen, bool isBrush, SolidBrush mainSolidBrush)
        {
            Rectangle newObject = new Rectangle(topLeft, new Size(Math.Abs(topLeft.X - bottomRight.X), Math.Abs(topLeft.Y - bottomRight.Y)));
            bound = newObject;
            type = tag;
            this.MainPen = mainPen;
            this.IsBrush = isBrush;
            SolidBrush = mainSolidBrush;
        }
        public GraphicObject(Rectangle rect, int tag, Pen mainPen, bool isBrush, SolidBrush mainSolidBrush)
        {
            bound = rect;
            type = tag;
            this.MainPen = mainPen;
            this.IsBrush = isBrush;
            SolidBrush = mainSolidBrush;
        }

        public GraphicObject(Point topLeft, int width, int height, int tag, Pen mainPen, bool isBrush, SolidBrush mainSolidBrush)
        {
            Rectangle newObject = new Rectangle(topLeft, new Size(width, height));
            bound = newObject;
            type = tag;
            this.MainPen = mainPen;
            this.IsBrush = isBrush;
            SolidBrush = mainSolidBrush;
        }
        
    }
}
