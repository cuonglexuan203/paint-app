using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint.src.Layers.InterfaceLayer.Actions
{
    public class GraphicObject
    {
        Rectangle bound = Rectangle.Empty;
        int type = 0; // index (0: grouped object container)
        Pen mainPen = null;
        bool isBrush = false;
        SolidBrush solidBrush = null;
        //
        bool isFreeObject = false;
        Point[] points = null;
        //
        bool isDeleted = false; // check change object (replace by another)

        //
        public Rectangle Bound { get => bound; set => bound = value; }
        public int Type { get => type; set => type = value; }
        public Pen MainPen { get => mainPen; set => mainPen = value; }
        public bool IsBrush { get => isBrush; set => isBrush = value; }
        public SolidBrush SolidBrush { get => solidBrush; set => solidBrush = value; }
        public bool IsFreeObject { get => isFreeObject; set => isFreeObject = value; }
        public Point[] Points { get => points; set => points = value; }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }

        //
        public GraphicObject()
        {

        }
        // Shape
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
        public GraphicObject(Rectangle rect, int tag, Pen mainPen, bool isBrush, SolidBrush mainSolidBrush, bool isFreeObject, Point[] points, bool isDeleted)
        {
            bound = rect;
            type = tag;
            this.MainPen = mainPen;
            this.IsBrush = isBrush;
            SolidBrush = mainSolidBrush;
            //
            this.isFreeObject = isFreeObject;
            this.points = points;
            this.isDeleted = isDeleted;
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
        // free line: 34 // fill: 36
        public GraphicObject(Point[] points, int tag, Pen mainPen, bool isFreeObject = true) 
        {
            if (isFreeObject)
            {
                this.Points = points;
                this.type = tag;
                this.mainPen = mainPen;
                this.IsFreeObject = isFreeObject;
            }
        }
        public GraphicObject Clone()
        => new GraphicObject(new Rectangle(new Point(this.Bound.X, this.Bound.Y), this.Bound.Size)
            , this.Type, (Pen)MainPen?.Clone(), IsBrush, (SolidBrush)SolidBrush?.Clone(), this.isFreeObject,(Point[])this.points?.Clone(), this.isDeleted);
        
    }
}
