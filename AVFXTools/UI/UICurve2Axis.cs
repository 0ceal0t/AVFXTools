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
    public class UICurve2Axis
    {
        public AVFXCurve2Axis Curve;
        public bool Assigned = false;
        public string Name;
        //=========================
        public static readonly string[] AxisConnectOptions = Enum.GetNames(typeof(AxisConnect));
        public int AxisConnectIdx;
        public static readonly string[] AxisConnectRandomOptions = Enum.GetNames(typeof(RandomType));
        public int AxisConnectRandomIdx;

        public UICurve X;
        public UICurve Y;
        public UICurve RX;
        public UICurve RY;

        public UICurve2Axis(AVFXCurve2Axis curve, string name)
        {
            Curve = curve;
            if (!curve.Assigned) return;
            Assigned = true;
            Name = name;
            // ======================
            AxisConnectIdx = Array.IndexOf(AxisConnectOptions, Curve.AxisConnectType.Value);
            AxisConnectRandomIdx = Array.IndexOf(AxisConnectRandomOptions, Curve.AxisConnectRandomType.Value);

            X = new UICurve(Curve.X, "X");
            Y = new UICurve(Curve.Y, "Y");
            RX = new UICurve(Curve.RX, "RX");
            RY = new UICurve(Curve.RY, "RY");
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode(Name + id))
            {
                if (UIUtils.EnumComboBox("Axis Connect" + id, AxisConnectOptions, ref AxisConnectIdx))
                {
                    Curve.AxisConnectType.GiveValue(AxisConnectOptions[AxisConnectIdx]);
                }
                if (UIUtils.EnumComboBox("Axis Connect Random" + id, AxisConnectRandomOptions, ref AxisConnectRandomIdx))
                {
                    Curve.AxisConnectRandomType.GiveValue(AxisConnectRandomOptions[AxisConnectRandomIdx]);
                }
                //===========================
                X.Draw(id + "-X");
                Y.Draw(id + "-Y");
                RX.Draw(id + "-RX");
                RY.Draw(id + "-RY");
                ImGui.TreePop();
            }
        }
    }
}
