using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public abstract class UIBase
    {
        public List<UIBase> Attributes = new List<UIBase>();
        public abstract void Draw(string parentId);
        public void DrawAttrs(string parentId)
        {
            foreach(UIBase attr in Attributes)
            {
                attr.Draw(parentId);
            }
        }
    }
}
