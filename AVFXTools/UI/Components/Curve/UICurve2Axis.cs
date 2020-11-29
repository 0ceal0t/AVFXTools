using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UICurve2Axis : UIBase
    {
        public AVFXCurve2Axis Curve;
        public bool Assigned = false;
        public string Name;
        //=========================

        public UICurve2Axis(AVFXCurve2Axis curve, string name)
        {
            Curve = curve;
            if (!curve.Assigned) return;
            Assigned = true;
            Name = name;
            // ======================
            Attributes.Add(new UICombo<AxisConnect>("Axis Connect", Curve.AxisConnectType));
            Attributes.Add(new UICombo<RandomType>("Axis Connect Random", Curve.AxisConnectRandomType));
            Attributes.Add(new UICurve(Curve.X, "X"));
            Attributes.Add(new UICurve(Curve.Y, "Y"));
            Attributes.Add(new UICurve(Curve.RX, "RX"));
            Attributes.Add(new UICurve(Curve.RY, "RY"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/" + Name;
            if (!Assigned) return;
            if (ImGui.TreeNode(Name + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
