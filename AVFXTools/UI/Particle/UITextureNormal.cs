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
    public class UITextureNormal : UIBase
    {
        public AVFXTextureNormal Tex;
        public string Name;
        public bool Assigned;
        //============================

        public UITextureNormal(AVFXTextureNormal tex)
        {
            Tex = tex;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Attributes.Add(new UICheckbox("Enabled", Tex.Enabled));
            Attributes.Add(new UIInt("UV Set Index", Tex.UvSetIdx));
            Attributes.Add(new UIInt("Texture Index", Tex.TextureIdx));
            Attributes.Add(new UICombo<TextureFilterType>("Texture Filter", Tex.TextureFilter));
            Attributes.Add(new UICombo<TextureBorderType>("Texture Border U", Tex.TextureBorderU));
            Attributes.Add(new UICombo<TextureBorderType>("Texture Border V", Tex.TextureBorderV));
            Attributes.Add(new UICurve(Tex.NPow, "Power"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TN";
            if (!Assigned) return;
            if (ImGui.TreeNode("Normal" + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
