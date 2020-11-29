using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UIBinderView : UIBase
    {
        List<UIBinder> Binders = new List<UIBinder>();

        public UIBinderView(AVFXBase avfx)
        {
            foreach (var binder in avfx.Binders)
            {
                Binders.Add(new UIBinder(binder));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##BIND";
            int bIdx = 0;
            foreach (var binder in Binders)
            {
                binder.Idx = bIdx;
                binder.Draw(id);
                bIdx++;
            }
        }
    }
}
