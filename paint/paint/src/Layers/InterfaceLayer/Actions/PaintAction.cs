using System;
using System.Collections.Generic;
using System.Drawing;
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
            Resize,
            Moving,
            Delete, // remove object
            Group,
            Ungroup,
            Untrack,
        }
        //
        PaintActionType type = PaintActionType.Draw;
        GraphicObject curGObject = null;
        GraphicObject oldGObject = null;
        private List<GraphicObject> objAfterGroup = null;
        private List<GraphicObject> objBeforeGroup = null;
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
        public List<GraphicObject> GetObjsBeforeGroup()
        {
            List<GraphicObject> res = null;
            if(this.type == PaintActionType.Group)
            {
                res = new List<GraphicObject>();
                foreach(GraphicObject obj in this.objBeforeGroup)
                {
                    res.Add(obj);
                }
            }
            return res;
        }
        public List<GraphicObject> GetObjsAfterGroup()
        {
            List<GraphicObject> res = null;
            if (this.type == PaintActionType.Group)
            {
                res = new List<GraphicObject>();
                foreach (GraphicObject obj in this.objAfterGroup)
                {
                    res.Add(obj);
                }
            }
            return res;
        }
        private bool SetObjsBeforeGroup(List<GraphicObject> ls)
        {
            bool res = false;
            if(this.type == PaintActionType.Group)
            {
                res = true;
                this.objBeforeGroup = new List<GraphicObject>();
                foreach (GraphicObject obj in ls)
                {
                    this.objBeforeGroup.Add(obj);
                }
            }
            return res;
        }
        public bool GroupObjects(List<GraphicObject> ls)
        {
            bool res = false;
            if (ls.Count >= 2)
            {
                res = true;
                this.type = PaintActionType.Group;
                SetObjsBeforeGroup(ls);
                GenerateObjsAfterGroup();
                //
                foreach (GraphicObject obj in objBeforeGroup)
                {
                    obj.IsDeleted = true;
                }
                // 
                this.curGObject = new GraphicObject();
                this.curGObject.Bound = objAfterGroup[0].Bound;
                foreach (GraphicObject obj in objAfterGroup) // redundant but it will be safe
                {
                    this.curGObject.Bound = Rectangle.Union(this.curGObject.Bound, obj.Bound);
                    obj.IsDeleted= false;
                }
                //
            }
            return res;
        }
        private bool GenerateObjsAfterGroup()
        {
            bool res = false;
            if(this.type == PaintActionType.Group)
            {
                res = true;
                objAfterGroup = new List<GraphicObject>();
                foreach (GraphicObject obj in this.objBeforeGroup)
                {
                    objAfterGroup.Add(obj.Clone());
                }
            }
            return res;
        }

    }
}
