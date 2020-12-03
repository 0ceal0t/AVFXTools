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
            Name = name;
            if (!curve.Assigned) { Assigned = false; return; }
            Assigned = true;
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
            // ===== UNASSIGNED ====
            if (!Assigned)
            {
                if (ImGui.Button("+ " + Name + id))
                {
                    // TODO
                }
                return;
            }
            // ===== ASSIGNED ===
            if (ImGui.TreeNode(Name + id))
            {
                if (UIUtils.RemoveButton("Delete" + id))
                {
                    // TODO
                }
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
