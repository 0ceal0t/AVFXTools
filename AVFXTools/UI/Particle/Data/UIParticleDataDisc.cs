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
    public class UIParticleDataDisc : UIBase
    {
        public AVFXParticleDataDisc Data;
        //==========================

        public UIParticleDataDisc(AVFXParticleDataDisc data)
        {
            Data = data;
            Init();
        }
        public override void Init()
        {
            base.Init();
            //=======================
            Attributes.Add(new UIInt("Parts Count", Data.PartsCount));
            Attributes.Add(new UIInt("Parts Count U", Data.PartsCountU));
            Attributes.Add(new UIInt("Parts Count V", Data.PartsCountV));
            Attributes.Add(new UIFloat("Point Interval Factor V", Data.PointIntervalFactoryV));
            Attributes.Add(new UICurve(Data.Angle, "Angle"));
            Attributes.Add(new UICurve(Data.WidthBegin, "Width Begin"));
            Attributes.Add(new UICurve(Data.WidthEnd, "Width End"));
            Attributes.Add(new UICurve(Data.RadiusBegin, "Radius Begin"));
            Attributes.Add(new UICurve(Data.RadiusEnd, "Radius End"));
            Attributes.Add(new UICurveColor(Data.ColorEdgeInner, "Color Edge Inner"));
            Attributes.Add(new UICurveColor(Data.ColorEdgeOuter, "Color Edge Outer"));
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
