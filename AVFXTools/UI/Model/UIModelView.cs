using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UIModelView : UIBase
    {
        List<UIModel> Models = new List<UIModel>();

        public UIModelView(AVFXBase avfx)
        {
            foreach (var model in avfx.Models)
            {
                Models.Add(new UIModel(model));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##MDL";
            int mIdx = 0;
            foreach (var model in Models)
            {
                model.Idx = mIdx;
                model.Draw(id);
                mIdx++;
            }
        }
    }
}
