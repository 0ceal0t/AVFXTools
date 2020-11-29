using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITextureView : UIBase
    {
        List<UITexture> Textures = new List<UITexture>();
        public UITextureBindings Bindings;

        public UITextureView(AVFXBase avfx, UITextureBindings bindings)
        {
            Bindings = bindings;
            // ======================
            foreach (var texture in avfx.Textures)
            {
                Textures.Add(new UITexture(texture, Bindings));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##TEX";
            int tIdx = 0;
            foreach (var texture in Textures)
            {
                texture.Idx = tIdx;
                texture.Draw(id);
                tIdx++;
            }
        }
    }
}
