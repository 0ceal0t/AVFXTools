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
    public class UIParticleDataDecal : UIBase
    {
        public AVFXParticleDataDecal Data;
        //==========================

        public UIParticleDataDecal(AVFXParticleDataDecal data)
        {
            Data = data;
            //=======================
            Attributes.Add(new UIFloat("Scaling Scale", Data.ScalingScale));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Data";
            if (ImGui.TreeNode("Data" + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
