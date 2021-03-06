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
    public class UIParticleDataDecalRing : UIBase
    {
        public AVFXParticleDataDecalRing Data;
        //==========================

        public UIParticleDataDecalRing(AVFXParticleDataDecalRing data)
        {
            Data = data;
            Init();
        }
        public override void Init()
        {
            base.Init();
            //=======================
            Attributes.Add(new UICurve(Data.Width, "Width"));
            Attributes.Add(new UIFloat("Scaling Scale", Data.ScalingScale));
            Attributes.Add(new UIFloat("Ring Fan", Data.RingFan));
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
