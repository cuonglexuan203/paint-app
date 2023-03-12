using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public static void FillRoundedRectangle(this Graphics graphics,Brush brush, Rectangle rect, int borderRadius)
        {
            int x = rect.X;
            int y = rect.Y;
            int width = rect.Width;
            int height = rect.Height;
            float diameter = borderRadius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(x, y, size.Width, size.Height);
            using (GraphicsPath path = new GraphicsPath())
            {
                 //
                 //top-left
                path.AddArc(arc, 180, 90);

                //top-right
                arc.X = x + width - diameter;
                path.AddArc(arc, 270, 90);

                //bottom-right
                arc.Y = y + height - diameter;
                path.AddArc(arc, 0, 90);

                // bottom-left
                arc.X = x;
                path.AddArc(arc, 90, 90);

                //
                // fill the rounded rectangle
                path.CloseFigure();
                graphics.FillPath(brush, path);
            }
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

        public static void FillTriangle(this Graphics g, Brush brush, Point topLeft, Point rightBottom)
        {
            Point pointX = topLeft;
            int x = rightBottom.X;
            int y = rightBottom.Y;
            //
            Point bottomPoint1 = new Point(pointX.X, y);
            Point bottomPoint2 = new Point(x, y);
            Point vertexPoint = new Point((x - pointX.X) / 2 + pointX.X, pointX.Y);
            Point[] trianglePoints = { bottomPoint1, bottomPoint2, vertexPoint };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(trianglePoints);
            g.FillPath(brush, path);
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
        public static void FillRightTriangle(this Graphics g, Brush brush, Point topLeft, Point rightBottom)
        {
            Point pointX = topLeft;
            int x = rightBottom.X;
            int y = rightBottom.Y;
            //
            Point bottomPoint1 = new Point(pointX.X, y);
            Point bottomPoint2 = new Point(x, y);
            Point vertexPoint = pointX;
            Point[] trianglePoints = { bottomPoint1, bottomPoint2, vertexPoint };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(trianglePoints);
            g.FillPath(brush, path);
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

        public static void FillDiamond(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //
            Point top = new Point((pointX.X + x) / 2, pointX.Y);
            Point right = new Point(x, (pointX.Y + y) / 2);
            Point bottom = new Point((pointX.X + x) / 2, y);
            Point left = new Point(pointX.X, (pointX.Y + y) / 2);
            Point[] diamondPoints = { top, right, bottom, left };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(diamondPoints);
            g.FillPath(brush, path);

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

        public static void FillPentagon(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            PointF centerPoint = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float convertRatio = (float)Math.PI / 180;
            float x = centerPoint.X;
            float y = centerPoint.Y;
            float q = 1.61803398875f;
            float d = (float)Math.Sqrt(Math.Pow(topLeft.X - bottomRight.X, 2) + Math.Pow(topLeft.Y - bottomRight.Y, 2));
            float s = d / (1 + q);
            PointF p1 = new PointF(x, y - s);
            PointF p2 = new PointF((float)(x + s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio)));
            PointF p3 = new PointF((float)(x + s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio)));
            PointF p4 = new PointF((float)(x - s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio)));
            PointF p5 = new PointF((float)(x - s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio)));
            PointF[] pentagonPoints = { p1, p2, p3, p4, p5 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(pentagonPoints);
            g.FillPath(brush, path);
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
            PointF[] hexagonPoints = { p1, p2, p3, p4, p5, p6 };
            g.DrawPolygon(p, hexagonPoints);
        }
        public static void FillHexagon(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
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
            PointF[] hexagonPoints = { p1, p2, p3, p4, p5, p6 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(hexagonPoints);
            g.FillPath(brush, path);
        }
        //
        //
        public static void DrawRightArrow(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF( centerP.X, centerP.Y - height / 4 );
            PointF p2 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p3 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p4 = new PointF(centerP.X, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X, centerP.Y + height / 4);
            PointF p6 = new PointF(centerP.X - width / 2, centerP.Y + height / 4);
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y - height / 4);

            PointF[] rightArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            g.DrawPolygon(p, rightArrowPoints);
        }
        public static void FillRightArrow(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 4);
            PointF p2 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p3 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p4 = new PointF(centerP.X, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X, centerP.Y + height / 4);
            PointF p6 = new PointF(centerP.X - width / 2, centerP.Y + height / 4);
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y - height / 4);

            PointF[] rightArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(rightArrowPoints);
            g.FillPath(brush, path);
        }

        public static void DrawLeftArrow(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 4);
            PointF p2 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p3 = new PointF(centerP.X - width / 2, centerP.Y);
            PointF p4 = new PointF(centerP.X, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X, centerP.Y + height / 4);
            PointF p6 = new PointF(centerP.X + width / 2, centerP.Y + height / 4);
            PointF p7 = new PointF(centerP.X + width / 2, centerP.Y - height / 4);

            PointF[] leftArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            g.DrawPolygon(p, leftArrowPoints);
        }
        public static void FillLeftArrow(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 4);
            PointF p2 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p3 = new PointF(centerP.X - width / 2, centerP.Y);
            PointF p4 = new PointF(centerP.X, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X, centerP.Y + height / 4);
            PointF p6 = new PointF(centerP.X + width / 2, centerP.Y + height / 4);
            PointF p7 = new PointF(centerP.X + width / 2, centerP.Y - height / 4);

            PointF[] leftArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(leftArrowPoints);
            g.FillPath(brush, path);
        }
        public static void DrawUpArrow(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p2 = new PointF(centerP.X + width / 2, centerP.Y );
            PointF p3 = new PointF(centerP.X + width / 4, centerP.Y);
            PointF p4 = new PointF(centerP.X + width / 4, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X - width / 4, centerP.Y + height / 2);
            PointF p6 = new PointF(centerP.X - width / 4, centerP.Y );
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y );

            PointF[] upArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            g.DrawPolygon(p, upArrowPoints);
        }
        public static void FillUpArrow(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p2 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p3 = new PointF(centerP.X + width / 4, centerP.Y);
            PointF p4 = new PointF(centerP.X + width / 4, centerP.Y + height / 2);

            PointF p5 = new PointF(centerP.X - width / 4, centerP.Y + height / 2);
            PointF p6 = new PointF(centerP.X - width / 4, centerP.Y);
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y);

            PointF[] upArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(upArrowPoints);
            g.FillPath(brush, path);
        }
        public static void DrawDownArrow(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y + height / 2);
            PointF p2 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p3 = new PointF(centerP.X + width / 4, centerP.Y);
            PointF p4 = new PointF(centerP.X + width / 4, centerP.Y  - height / 2);

            PointF p5 = new PointF(centerP.X - width / 4, centerP.Y - height / 2);
            PointF p6 = new PointF(centerP.X - width / 4, centerP.Y);
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y);

            PointF[] downArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            g.DrawPolygon(p, downArrowPoints);
        }
        public static void FillDownArrow(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            PointF p1 = new PointF(centerP.X, centerP.Y + height / 2);
            PointF p2 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p3 = new PointF(centerP.X + width / 4, centerP.Y);
            PointF p4 = new PointF(centerP.X + width / 4, centerP.Y - height / 2);

            PointF p5 = new PointF(centerP.X - width / 4, centerP.Y - height / 2);
            PointF p6 = new PointF(centerP.X - width / 4, centerP.Y);
            PointF p7 = new PointF(centerP.X - width / 2, centerP.Y);

            PointF[] downArrowPoints = { p1, p2, p3, p4, p5, p6, p7 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(downArrowPoints);
            g.FillPath(brush, path);
        }
        public static void DrawFourPointStar(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            //
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p2 = new PointF( centerP.X + width / 2, centerP.Y);
            PointF p3 = new PointF(centerP.X , centerP.Y + height / 2);
            PointF p4 = new PointF(centerP.X - width / 2, centerP.Y);
            //
            PointF p5 = new PointF((p4.X + p1.X + 2 * centerP.X) / 4 , (p4.Y + p1.Y + 2 * centerP.Y) / 4);
            PointF p6 = new PointF((p1.X + p2.X + 2 * centerP.X) / 4, (p1.Y + p2.Y + 2 * centerP.Y) / 4);
            PointF p7 = new PointF((p2.X + p3.X + 2 * centerP.X) / 4, (p2.Y + p3.Y + 2 * centerP.Y) / 4);
            PointF p8 = new PointF((p3.X + p4.X + 2 * centerP.X) / 4, (p3.Y + p4.Y + 2 * centerP.Y) / 4);

            PointF[] fourPointStarPoints = { p1, p6, p2, p7, p3, p8, p4, p5 };
            g.DrawPolygon(p, fourPointStarPoints);
        }
        public static void FillFourPointStar(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            Point pointX = topLeft;
            int x = bottomRight.X;
            int y = bottomRight.Y;
            //

            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            //
            PointF p1 = new PointF(centerP.X, centerP.Y - height / 2);
            PointF p2 = new PointF(centerP.X + width / 2, centerP.Y);
            PointF p3 = new PointF(centerP.X, centerP.Y + height / 2);
            PointF p4 = new PointF(centerP.X - width / 2, centerP.Y);
            //
            PointF p5 = new PointF((p4.X + p1.X + 2 * centerP.X) / 4, (p4.Y + p1.Y + 2 * centerP.Y) / 4);
            PointF p6 = new PointF((p1.X + p2.X + 2 * centerP.X) / 4, (p1.Y + p2.Y + 2 * centerP.Y) / 4);
            PointF p7 = new PointF((p2.X + p3.X + 2 * centerP.X) / 4, (p2.Y + p3.Y + 2 * centerP.Y) / 4);
            PointF p8 = new PointF((p3.X + p4.X + 2 * centerP.X) / 4, (p3.Y + p4.Y + 2 * centerP.Y) / 4);

            PointF[] fourPointStarPoints = { p1, p6, p2, p7, p3, p8, p4, p5 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(fourPointStarPoints);
            g.FillPath(brush, path);
        }
        public static void DrawFivePointStar(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            PointF centerPoint = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float convertRatio = (float)Math.PI / 180;
            float x = centerPoint.X;
            float y = centerPoint.Y;
            float q = 1.61803398875f;
            float d = (float)Math.Sqrt(Math.Pow(topLeft.X - bottomRight.X, 2) + Math.Pow(topLeft.Y - bottomRight.Y, 2));
            float s = d / (1 + q);
            PointF p1 = new PointF(x, y - s); // A
            PointF p2 = new PointF((float)(x + s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio))); // B
            PointF p3 = new PointF((float)(x + s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio))); // C
            PointF p4 = new PointF((float)(x - s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio))); // D
            PointF p5 = new PointF((float)(x - s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio))); // E
            PointF[] outerPoints = {p1,p2, p3, p4, p5};  // outer points: A B C D E
            //
            //                          A
            //
            //          E          K         F         B
            //
            //                  I               G
            //                          H            
            //
            //              D                       C
            float a = 0f;
            float b = 0f;
            // calculate parameter of AC, BD, CE, DA, EB
            List <Tuple<float, float>> parameters = new List<Tuple<float, float>>();
            for(int i = 0; i  < outerPoints.Length; i++)
            {
                int ii = ( i + 2 < outerPoints.Length ?  i + 2 : i + 2 - outerPoints.Length);
                a = (outerPoints[i].Y - outerPoints[ii].Y) / (outerPoints[i].X - outerPoints[ii].X);
                b = outerPoints[i].Y - a * outerPoints[i].X;
                parameters.Add(new Tuple<float, float>(a, b)); 
            }
            // calculate the coordinate of intersections ( inner points )
            // in order: G H I K F
            List <PointF> innerPoints = new List<PointF>();
            float pointX = 0f;
            float pointY = 0f;
            for (int i = 0; i < parameters.Count; i++)
            {
                int ii = (i + 1 < parameters.Count ? i + 1 : i + 1 -  parameters.Count);
                pointX = (parameters[ii].Item2 - parameters[i].Item2) / (parameters[i].Item1 - parameters[ii].Item1);
                pointY = parameters[i].Item1 * pointX + parameters[i].Item2;
                innerPoints.Add(new PointF(pointX, pointY));
            }
            //
            List <PointF> fivePointStarPoints = new List<PointF>() { outerPoints[0], innerPoints[4], outerPoints[1], innerPoints[0], outerPoints[2],
                                                                     innerPoints[1], outerPoints[3], innerPoints[2], outerPoints[4], innerPoints[3] };
            g.DrawPolygon(p, fivePointStarPoints.ToArray());
        }
        public static void FillFivePointStar(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            PointF centerPoint = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float convertRatio = (float)Math.PI / 180;
            float x = centerPoint.X;
            float y = centerPoint.Y;
            float q = 1.61803398875f;
            float d = (float)Math.Sqrt(Math.Pow(topLeft.X - bottomRight.X, 2) + Math.Pow(topLeft.Y - bottomRight.Y, 2));
            float s = d / (1 + q);
            PointF p1 = new PointF(x, y - s); // A
            PointF p2 = new PointF((float)(x + s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio))); // B
            PointF p3 = new PointF((float)(x + s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio))); // C
            PointF p4 = new PointF((float)(x - s * Math.Cos(54 * convertRatio)), (float)(y + s * Math.Sin(54 * convertRatio))); // D
            PointF p5 = new PointF((float)(x - s * Math.Cos(18 * convertRatio)), (float)(y - s * Math.Sin(18 * convertRatio))); // E
            PointF[] outerPoints = { p1, p2, p3, p4, p5 };  // outer points: A B C D E
            //
            //                          A
            //
            //          E          K         F         B
            //
            //                  I               G
            //                          H            
            //
            //              D                       C
            float a = 0f;
            float b = 0f;
            // calculate parameter of AC, BD, CE, DA, EB
            List<Tuple<float, float>> parameters = new List<Tuple<float, float>>();
            for (int i = 0; i < outerPoints.Length; i++)
            {
                int ii = (i + 2 < outerPoints.Length ? i + 2 : i + 2 - outerPoints.Length);
                a = (outerPoints[i].Y - outerPoints[ii].Y) / (outerPoints[i].X - outerPoints[ii].X);
                b = outerPoints[i].Y - a * outerPoints[i].X;
                parameters.Add(new Tuple<float, float>(a, b));
            }
            // calculate the coordinate of intersections ( inner points )
            // in order: G H I K F
            List<PointF> innerPoints = new List<PointF>();
            float pointX = 0f;
            float pointY = 0f;
            for (int i = 0; i < parameters.Count; i++)
            {
                int ii = (i + 1 < parameters.Count ? i + 1 : i + 1 - parameters.Count);
                pointX = (parameters[ii].Item2 - parameters[i].Item2) / (parameters[i].Item1 - parameters[ii].Item1);
                pointY = parameters[i].Item1 * pointX + parameters[i].Item2;
                innerPoints.Add(new PointF(pointX, pointY));
            }
            //
            List<PointF> fivePointStarPoints = new List<PointF>() { outerPoints[0], innerPoints[4], outerPoints[1], innerPoints[0], outerPoints[2],
                                                                     innerPoints[1], outerPoints[3], innerPoints[2], outerPoints[4], innerPoints[3] };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(fivePointStarPoints.ToArray());
            g.FillPath(brush, path);
        }
        public static void DrawSixPointStar(this Graphics g, Pen p, Point topLeft, Point bottomRight)
        {
            //
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float x = centerP.X;
            float y = centerP.Y;
            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            //
            PointF p1 = new PointF(x , y - height / 2);
            PointF p2 = new PointF(x  + width / 6, y - height / 4);
            PointF p3 = new PointF(x + width / 2, y - height / 4);
            PointF p4 = new PointF(x + width / 3, y);
            
            //
            PointF p5 = new PointF(x + width / 2, y + height / 4);
            PointF p6 = new PointF(x + width / 6, y + height / 4);
            PointF p7 = new PointF(x, y + height / 2);
            PointF p8 = new PointF(x - width / 6, y + height / 4);
            PointF p9 = new PointF(x - width / 2, y + height / 4);
            PointF p10 = new PointF(x - width / 3, y);
            PointF p11 = new PointF(x - width / 2, y - height / 4);
            PointF p12 = new PointF(x - width / 6, y - height / 4);
            //
            PointF[] sixPointStarPoints = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };
            g.DrawPolygon(p, sixPointStarPoints);
        }

        public static void FillSixPointStar(this Graphics g, Brush brush, Point topLeft, Point bottomRight)
        {
            //
            PointF centerP = new PointF(1f * (topLeft.X + bottomRight.X) / 2, 1f * (topLeft.Y + bottomRight.Y) / 2);
            float x = centerP.X;
            float y = centerP.Y;
            float height = Math.Abs(topLeft.Y - bottomRight.Y);
            float width = Math.Abs(topLeft.X - bottomRight.X);
            //
            PointF p1 = new PointF(x, y - height / 2);
            PointF p2 = new PointF(x + width / 6, y - height / 4);
            PointF p3 = new PointF(x + width / 2, y - height / 4);
            PointF p4 = new PointF(x + width / 3, y);

            //
            PointF p5 = new PointF(x + width / 2, y + height / 4);
            PointF p6 = new PointF(x + width / 6, y + height / 4);
            PointF p7 = new PointF(x, y + height / 2);
            PointF p8 = new PointF(x - width / 6, y + height / 4);
            PointF p9 = new PointF(x - width / 2, y + height / 4);
            PointF p10 = new PointF(x - width / 3, y);
            PointF p11 = new PointF(x - width / 2, y - height / 4);
            PointF p12 = new PointF(x - width / 6, y - height / 4);
            //
            PointF[] sixPointStarPoints = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(sixPointStarPoints);
            g.FillPath(brush, path);
        }
    }
}
