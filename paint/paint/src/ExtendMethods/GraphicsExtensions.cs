using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace paint
{
    internal static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, int x, int y, int width, int height, int borderRadius)
        {

            float diameter = borderRadius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(x, y, size.Width, size.Height);
            //
            //top-left
            graphics.DrawArc(pen, arc, 180, 90);
            //top-right
            arc.X = x + width - diameter;
            graphics.DrawArc(pen, arc, 270, 90);
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
            graphics.DrawLine(pen, x + width, y + borderRadius, x + width, y + height - borderRadius);
            // left
            graphics.DrawLine(pen, x, y + borderRadius, x, y + height - borderRadius);


        }
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rect, int borderRadius)
        {
            int x = rect.X;
            int y = rect.Y;
            int width = rect.Width;
            int height = rect.Height;
            float diameter = borderRadius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(x, y, size.Width, size.Height);
            //
            //top-left
            graphics.DrawArc(pen, arc, 180, 90);
            //top-right
            arc.X = x + width - diameter;
            graphics.DrawArc(pen, arc, 270, 90);
            //bottom-right
            arc.Y = y + height - diameter;
            graphics.DrawArc(pen, arc, 0, 90);
            // bottom-left
            arc.X = x;
            graphics.DrawArc(pen, arc, 90, 90);

            // draw edges
            // top
            graphics.DrawLine(pen, x + borderRadius - 1, y, x + width - borderRadius + 1, y);
            // bottom
            graphics.DrawLine(pen, x + borderRadius - 1, y + height, x + width - borderRadius + 1, y + height);
            // right
            graphics.DrawLine(pen, x + width, y + borderRadius - 1, x + width, y + height - borderRadius + 1);
            // left
            graphics.DrawLine(pen, x, y + borderRadius - 1, x, y + height - borderRadius + 1);


        }
        //
        private static bool IsRoughlyPoints(Point p1, Point p2)
        {
            bool isRoughly = true;
            double radius = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            if (radius > 4)
            {
                isRoughly = false;
            }
            return isRoughly;
        }
        // 
        /// <summary>
        /// This method is automatically operate. Only finish the shape when the first point is the same as final point.
        /// Provide variable to the method can store data in them, not to get data from them.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="rootPoint"></param>
        /// <param name="firstPointEachLine"></param>
        /// <param name="endPointEachLine"></param>
        /// <param name="endPointOfPreLine"></param>
        /// <param name="currentPoint"></param>
        /// <param name="countLine">the number of line of the polygon</param>
        /// <param name="isDrawingFlag"></param>
        public static void DrawPolygon(this Graphics g, Pen p, Point rootPoint, Point firstPointEachLine, Point endPointEachLine, Point endPointOfPreLine, Point currentPoint,ref int countLine,ref bool isDrawingFlag)
        {
            Point pointX = firstPointEachLine;
            Point pointY = endPointEachLine;
            Point endPoint = endPointOfPreLine;
            int x = currentPoint.X;
            int y = currentPoint.Y;
            bool hasRoot = isDrawingFlag;
            //
            Point sp = pointX;
            Point ep = new Point(x, y); // end point at each period
            if (hasRoot)
            {
                if (countLine != 0)
                {
                    sp = endPoint;
                    if (IsRoughlyPoints(rootPoint, pointY))
                    {
                        g.DrawLine(p, sp, rootPoint); // the final line
                        countLine = -1; // finish shape
                        hasRoot = false;
                    }
                    else
                    {
                        g.DrawLine(p, sp, ep); // the middle lines
                    }
                }
                else
                {
                    g.DrawLine(p, sp, ep); // first line
                }
            }

        }
        public static void DrawTriangle(this Graphics g, Pen p, Point topLeft, Point rightBottom)
        {
            Point pointX = topLeft;
            int x = rightBottom.X;
            int y = rightBottom.Y;
            //
            Point bottomPoint1 = new Point(pointX.X, y);
            Point bottomPoint2 = new Point(x, y);
            Point vertexPoint = new Point((x - pointX.X) / 2 + pointX.X, pointX.Y);
            Point[] trianglePoints = { bottomPoint1, bottomPoint2, vertexPoint };
            g.DrawPolygon(p, trianglePoints);
        }
        public static void DrawRightTriangle(this Graphics g, Pen p, Point topLeft, Point rightBottom)
        {
            Point pointX = topLeft;
            int x = rightBottom.X;
            int y = rightBottom.Y;
            //
            Point bottomPoint1 = new Point(pointX.X, y);
            Point bottomPoint2 = new Point(x, y);
            Point vertexPoint = pointX;
            Point[] trianglePoints = { bottomPoint1, bottomPoint2, vertexPoint };
            g.DrawPolygon(p, trianglePoints);
        }
        public static void DrawDiamond(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //
            Point top = new Point((pointX.X + x) / 2, pointX.Y);
            Point right = new Point(x , (pointX.Y + y) / 2);
            Point bottom = new Point((pointX.X + x) / 2, y);
            Point left = new Point(pointX.X, (pointX.Y + y) / 2);
            Point[] diamondPoints = { top, right, bottom, left };
            g.DrawPolygon(p, diamondPoints);
            
        }
        public static void DrawPentagon(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            PointF centerPoint = new PointF ( 1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float convertRatio = (float)Math.PI / 180;
            float x = centerPoint.X;
            float y = centerPoint.Y;
            float q = 1.61803398875f;
            float d = (float)Math.Sqrt(Math.Pow(topLeft.X - bottomRight.X,2) + Math.Pow(topLeft.Y - bottomRight.Y,2));
            float s = d / (1 + q);
            PointF p1 = new PointF(x, y - s);
            PointF p2 = new PointF((float)(x + s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio)));
            PointF p3 = new PointF((float)(x + s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio)));
            PointF p4 = new PointF((float)(x - s * Math.Cos(54 * convertRatio)),(float) (y + s * Math.Sin(54 * convertRatio)));
            PointF p5 = new PointF((float)(x - s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio)));
            PointF[] pentagonPoints = { p1, p2, p3, p4, p5 };
            g.DrawPolygon(p, pentagonPoints);
        }
        public static void DrawHexagon(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            PointF centerPoint = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float x = centerPoint.X;
            float y = centerPoint.Y;
            float d = (float)Math.Sqrt(Math.Pow(topLeft.X - bottomRight.X, 2) + Math.Pow(topLeft.Y - bottomRight.Y, 2));
            float s = d / 2;
            PointF p1 = new PointF(x, y - s);
            PointF p2 = new PointF((float)(x + s * Math.Sqrt(3) / 2), y - s / 2);
            PointF p3 = new PointF((float)(x + s * Math.Sqrt(3) / 2), y + s / 2);
            PointF p4 = new PointF(x, y + s);
            PointF p5 = new PointF((float)(x - s * Math.Sqrt(3) / 2), y + s / 2);
            PointF p6 = new PointF((float)(x - s * Math.Sqrt(3) / 2), y - s / 2);
            PointF[] pentagonPoints = { p1, p2, p3, p4, p5, p6 };
            g.DrawPolygon(p, pentagonPoints);
        }
    }
}
