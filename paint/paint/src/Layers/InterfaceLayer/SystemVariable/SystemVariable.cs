using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public static class SystemVariable
    {
        // Color.FromArgb(85, 85, 85)
        // object selection pen
        public static Color sysSelectedGraphicObjectPenColor = Color.Red;
        public static DashStyle sysSelectedGraphicObjectPenDashStyle = DashStyle.Dot;
        public static int sysSelectedGraphicObjectPenWidth = 2;
        // main bitmap variable
        public static Color sysMainBitmapColor = Color.White;
        // application path
        public static string sysAppPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString();

    }
}
