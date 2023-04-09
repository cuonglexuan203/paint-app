using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace paint.src.Layers.InterfaceLayer.Actions
{
    public class PaintAction
    {
        public enum PaintActionType
        {
            Draw,
            Fill,
            Remove,
            Resize,
            Moving,
            Untrack,
            Delete // remove object
        }
        //
        PaintActionType type = PaintActionType.Draw;
        GraphicObject curGObject = null;
        GraphicObject oldGObject = null;
        //
        internal PaintActionType Type { get => type; set => type = value; }
        internal GraphicObject CurGObject { get => curGObject; set => curGObject = value; }
        internal GraphicObject OldGObject { get => oldGObject; set => oldGObject = value; }

        //
        public PaintAction()
        {

        }
        public PaintAction(PaintActionType t, GraphicObject newGO, GraphicObject oldGO = null)
        {
            CurGObject = newGO;
            OldGObject = oldGO;
            Type = t;
        }
        public PaintAction(GraphicObject newGO, GraphicObject oldGO = null)
        {
            CurGObject = newGO;
            OldGObject = oldGO;
        }
        //

    }
}
