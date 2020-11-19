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
    public class UIParticleUVSet
    {
        public ParticleItem Item;
        public AVFXParticleUVSet UVSet;
        //=======================
        public static readonly string[] CalculateUVOptions = Enum.GetNames(typeof(TextureCalculateUV));
        public int CalculateUVIdx;
        public UICurve2Axis Scale;
        public UICurve2Axis Scroll;
        public UICurve Rot;
        public UICurve RotRandom;

        public UIParticleUVSet(ParticleItem item, AVFXParticleUVSet uvSet)
        {
            Item = item;
            UVSet = uvSet;
            //=================
            CalculateUVIdx = Array.IndexOf(CalculateUVOptions, UVSet.CalculateUVType.Value);
            Scale = new UICurve2Axis(UVSet.Scale, "Scale");
            Scroll = new UICurve2Axis(UVSet.Scroll, "Scroll");
            Rot = new UICurve(UVSet.Rot, "Rotation");
            RotRandom = new UICurve(UVSet.RotRandom, "Rotation Random");
        }

        public void Draw(string id, int idx)
        {
            if (ImGui.TreeNode("UV#" + idx + id))
            {
                if (UIUtils.EnumComboBox("Calculate UV" + id, CalculateUVOptions, ref CalculateUVIdx))
                {
                    UVSet.CalculateUVType.GiveValue(CalculateUVOptions[CalculateUVIdx]);
                }
                Scale.Draw(id + "-scale");
                Scroll.Draw(id + "-scroll");
                Rot.Draw(id + "-rot");
                RotRandom.Draw(id + "-rotRandom");
                ImGui.TreePop();
            }
        }
    }
}
