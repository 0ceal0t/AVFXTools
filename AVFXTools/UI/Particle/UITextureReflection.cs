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
    public class UITextureReflection : UIBase
    {
        public AVFXTextureReflection Tex;
        //============================

        public UITextureReflection(AVFXTextureReflection tex)
        {
            Tex = tex;
            if (!tex.Assigned) { Assigned = false; return; }
            //====================
            Attributes.Add(new UICheckbox("Enabled", Tex.Enabled));
            Attributes.Add(new UICheckbox("Use Screen Copy", Tex.UseScreenCopy));
            Attributes.Add(new UIInt("Texture Index", Tex.TextureIdx));
            Attributes.Add(new UICombo<TextureFilterType>("Texture Filter", Tex.TextureFilter));
            Attributes.Add(new UICombo<TextureCalculateColor>("Calculate Color", Tex.TextureCalculateColor));
            Attributes.Add(new UICurve(Tex.RPow, "Power"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TR";
            // === UNASSIGNED ===
            if (!Assigned)
            {
                if (ImGui.Button("+ Texture Reflection" + id))
                {
                    // TODO
                }
                return;
            }
            // ==== ASSIGNED ===
            if (ImGui.TreeNode("Reflection" + id))
            {
                if (UIUtils.RemoveButton("Delete " + id))
                {
                    // TODO
                }
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
