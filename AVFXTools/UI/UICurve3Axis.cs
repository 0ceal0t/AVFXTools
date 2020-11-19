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
    public class UICurve3Axis
    {
        public AVFXCurve3Axis Curve;
        public bool Assigned = false;
        public string Name;
        //=========================
        public static readonly string[] AxisConnectOptions = Enum.GetNames(typeof(AxisConnect));
        public int AxisConnectIdx;
        public static readonly string[] AxisConnectRandomOptions = Enum.GetNames(typeof(RandomType));
        public int AxisConnectRandomIdx;

        public UICurve X;
        public UICurve Y;
        public UICurve Z;
        public UICurve RX;
        public UICurve RY;
        public UICurve RZ;

        public UICurve3Axis(AVFXCurve3Axis curve, string name)
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
            Z = new UICurve(Curve.Z, "Z");
            RX = new UICurve(Curve.RX, "RX");
            RY = new UICurve(Curve.RY, "RY");
            RZ = new UICurve(Curve.RZ, "RZ");
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
                Z.Draw(id + "-Z");
                RX.Draw(id + "-RX");
                RY.Draw(id + "-RY");
                RZ.Draw(id + "-RZ");
                ImGui.TreePop();
            }
        }
    }
}
