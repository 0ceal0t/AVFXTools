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
        public UITextureBindings Bindings;
        public int Idx;
        // =======================

        public UITexture(AVFXTexture texture, UITextureBindings bindings)
        {
            Texture = texture;
            Bindings = bindings;
            // ================
            UIString.Change bytesToPath = BytesToPath;
            Attributes.Add(new UIString("Path", Texture.Path, changeFunction: bytesToPath));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Texture" + Idx;
            if (ImGui.CollapsingHeader("Texture " + Idx + id))
            {
                DrawAttrs(id);
                TextureInfo info = Bindings.IndexToPointer[Idx];
                ImGui.Image(info.Pointer, new Vector2(info.Width, info.Height));
            }
        }

        public static string BytesToPath(byte[] value)
        {
            return Util.BytesToString(value).Trim('\0') + "\u0000";
        }
    }
}
