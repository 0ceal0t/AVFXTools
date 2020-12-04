using AVFXLib.Main;
using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AVFXTools.UI
{
    public class UITexture : UIBase
    {
        public AVFXTexture Texture;
        public UITextureView View;
        public UITextureBindings Bindings;
        public int Idx;
        public string lastValue;
        public UIString Path;
        // =======================

        public UITexture(AVFXTexture texture, UITextureBindings bindings, UITextureView view)
        {
            Texture = texture;
            Bindings = bindings;
            View = view;
            Init();
        }
        public override void Init()
        {
            base.Init();
            // ================
            UIString.Change bytesToPath = BytesToPath;
            Path = new UIString("Path", Texture.Path, changeFunction: bytesToPath);
            lastValue = Texture.Path.Value;
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Texture" + Idx;
            if (ImGui.CollapsingHeader("Texture " + Idx + id))
            {
                if (UIUtils.RemoveButton("Delete" + id))
                {
                    View.AVFX.removeTexture(Idx);
                    Bindings.removeTextureBinding(Idx);
                    View.Init();
                    return;
                }
                Path.Draw(id);

                // jank change detection
                var newValue = Path.Literal.Value;
                if(newValue != lastValue)
                {
                    Bindings.updateTextureBinding(newValue, Idx);
                    lastValue = newValue;
                }

                TextureInfo info = Bindings.IdxToPointer[Idx];
                ImGui.Image(info.Pointer, new Vector2(info.Width, info.Height));
            }
        }

        public static void BytesToPath(LiteralString literal)
        {
            literal.GiveValue(literal.Value + "\u0000");
        }
    }
}
