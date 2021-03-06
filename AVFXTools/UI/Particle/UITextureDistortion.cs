﻿using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITextureDistortion : UIBase
    {
        public AVFXTextureDistortion Tex;
        public string Name;
        //============================

        public UITextureDistortion(AVFXTextureDistortion tex)
        {
            Tex = tex;
            Init();
        }
        public override void Init()
        {
            base.Init();
            if (!Tex.Assigned) { Assigned = false; return; }
            //====================
            Attributes.Add(new UICheckbox("Enabled", Tex.Enabled));
            Attributes.Add(new UICheckbox("Distort UV1", Tex.TargetUV1));
            Attributes.Add(new UICheckbox("Distort UV2", Tex.TargetUV2));
            Attributes.Add(new UICheckbox("Distort UV3", Tex.TargetUV3));
            Attributes.Add(new UICheckbox("Distort UV4", Tex.TargetUV4));
            Attributes.Add(new UIInt("UV Set Index", Tex.UvSetIdx));
            Attributes.Add(new UIInt("Texture Index", Tex.TextureIdx));
            Attributes.Add(new UICombo<TextureFilterType>("Texture Filter", Tex.TextureFilter));
            Attributes.Add(new UICombo<TextureBorderType>("Texture Border U", Tex.TextureBorderU));
            Attributes.Add(new UICombo<TextureBorderType>("Texture Border V", Tex.TextureBorderV));
            Attributes.Add(new UICurve(Tex.DPow, "Power"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TD";
            // === UNASSIGNED ===
            if (!Assigned)
            {
                if (ImGui.Button("+ Texture Distortion" + id))
                {
                    Tex.toDefault();
                    Init();
                }
                return;
            }
            // ==== ASSIGNED ===
            if (ImGui.TreeNode("Distortion" + id))
            {
                if (UIUtils.RemoveButton("Delete " + id))
                {
                    Tex.Assigned = false;
                    Init();
                }
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
