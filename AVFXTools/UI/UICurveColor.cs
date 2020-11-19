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
    public class UICurveColor
    {
        public AVFXCurveColor Curve;
        public bool Assigned = false;
        public string Name;
        //=========================
        public UICurve RGB;
        public UICurve A;
        public UICurve SclR;
        public UICurve SclG;
        public UICurve SclB;
        public UICurve SclA;
        public UICurve Bri;
        public UICurve RanR;
        public UICurve RanG;
        public UICurve RanB;
        public UICurve RanA;
        public UICurve RBri;

        public UICurveColor(AVFXCurveColor curve, string name)
        {
            Curve = curve;
            if (!curve.Assigned) return;
            Assigned = true;
            Name = name;
            // ======================
            RGB = new UICurve(Curve.RGB, "RGB", color:true);
            A = new UICurve(Curve.A, "Alpha");
            SclR = new UICurve(Curve.SclR, "Scale R");
            SclG = new UICurve(Curve.SclG, "Scale G");
            SclB = new UICurve(Curve.SclB, "Scale B");
            SclA = new UICurve(Curve.SclA, "Scale Alpha");
            Bri = new UICurve(Curve.Bri, "Brightness");
            RanR = new UICurve(Curve.RanR, "Random R");
            RanG = new UICurve(Curve.RanG, "Random G");
            RanB = new UICurve(Curve.RanB, "Random B");
            RanA = new UICurve(Curve.RanA, "Random A");
            RBri = new UICurve(Curve.RBri, "Random Bri");
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode(Name + id))
            {
                RGB.Draw(id + "-RGB");
                A.Draw(id + "-A");
                SclR.Draw(id + "-SclR");
                SclG.Draw(id + "-SclG");
                SclB.Draw(id + "-SclB");
                SclA.Draw(id + "-SclA");
                Bri.Draw(id + "-Bri");
                RanR.Draw(id + "-RanR");
                RanG.Draw(id + "-RanG");
                RanB.Draw(id + "-RanB");
                RanA.Draw(id + "-RanA");
                RBri.Draw(id + "-RBri");

                ImGui.TreePop();
            }
        }
    }
}
