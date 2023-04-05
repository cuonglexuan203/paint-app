using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace paint.src.Layers.InterfaceLayer.Actions
{
    public class PaintAction
    {
        public enum PaintActionType
        {
            Draw,
            Remove,
            Resize,
            Moving
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
        public PaintAction( PaintActionType t, GraphicObject newGO, GraphicObject oldGO = null)
        {
            CurGObject = newGO;
            OldGObject = oldGO;
            Type = t;
        }
        //
        public string ConvertToXMLString()
        {
            XmlSerializer serializer = new XmlSerializer( typeof( PaintAction ) );
            string serializerPaintAction = string.Empty;
            using(StringWriter writer = new StringWriter())
            {
                serializer.Serialize( writer, this );
                serializerPaintAction = writer.ToString();
            }
            return serializerPaintAction;
        }
        public static PaintAction PaintActionFromString( string str)
        {
            XmlSerializer xmlSerializer = new XmlSerializer( typeof( PaintAction ) ) ;
            PaintAction result = null;
            using(StringReader reader = new StringReader( str ))
            {
                result = (PaintAction)xmlSerializer.Deserialize( reader );
            }
            return result;
        }
    }
}
