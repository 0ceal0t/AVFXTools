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
    public class UIBinderDataSpline : UIBase
    {
        public AVFXBinderDataSpline Data;
        //=======================

        public UIBinderDataSpline(AVFXBinderDataSpline data)
        {
            Data = data;
            //==================
            Attributes.Add(new UICurve(data.CarryOverFactor, "Carry Over Factor"));
            Attributes.Add(new UICurve(data.CarryOverFactorRandom, "Carry Over Factor Random"));
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
