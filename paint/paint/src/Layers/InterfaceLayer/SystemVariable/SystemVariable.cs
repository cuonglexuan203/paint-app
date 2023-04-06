using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public static class SystemVariable
    {
        // Color.FromArgb(85, 85, 85)
        public static Color sysSelectedGraphicObjectPenColor = Color.Red;
        public static DashStyle sysSelectedGraphicObjectPenDashStyle = DashStyle.Dot;
        public static int sysSelectedGraphicObjectPenWidth = 2;
    }
}
