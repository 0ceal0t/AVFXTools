using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITexturePalette : UIBase
    {
        public AVFXTexturePalette Tex;
        public string Name;
        public bool Assigned;
        //============================

        public UITexturePalette(AVFXTexturePalette tex)
        {
            Tex = tex;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Attributes.Add(new UICheckbox("Enabled", Tex.Enabled));
            Attributes.Add(new UIInt("Texture Index", Tex.TextureIdx));
            Attributes.Add(new UICombo<TextureFilterType>("Texture Filter", Tex.TextureFilter));
            Attributes.Add(new UICombo<TextureBorderType>("Texture Border", Tex.TextureBorder));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TP";
            if (!Assigned) return;
            if (ImGui.TreeNode("Palette" + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
