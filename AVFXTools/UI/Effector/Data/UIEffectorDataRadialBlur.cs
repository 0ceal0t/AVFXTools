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
    public class UIEffectorDataRadialBlur : UIBase
    {
        public AVFXEffectorDataRadialBlur Data;
        //==========================

        public UIEffectorDataRadialBlur(AVFXEffectorDataRadialBlur data)
        {
            Data = data;
            //=======================
            Attributes.Add(new UIFloat("Fade Start Distance", Data.FadeStartDistance));
            Attributes.Add(new UIFloat("Fade End Distance", Data.FadeEndDistance));
            Attributes.Add(new UICombo<ClipBasePoint>("Fade Base Point", Data.FadeBasePointType));
            Attributes.Add(new UICurve(Data.Length, "Length"));
            Attributes.Add(new UICurve(Data.Strength, "Strength"));
            Attributes.Add(new UICurve(Data.Gradation, "Gradation"));
            Attributes.Add(new UICurve(Data.InnerRadius, "Inner Radius"));
            Attributes.Add(new UICurve(Data.OuterRadius, "Outer Radius"));
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
