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
    public class UIParticleUVSet : UIBase
    {
        public AVFXParticleUVSet UVSet;
        public int Idx;
        //=======================

        public UIParticleUVSet(AVFXParticleUVSet uvSet)
        {
            UVSet = uvSet;
            //=================
            Attributes.Add(new UICombo<TextureCalculateUV>("Rotation Direction Base", UVSet.CalculateUVType));
            Attributes.Add(new UICurve2Axis(UVSet.Scale, "Scale"));
            Attributes.Add(new UICurve2Axis(UVSet.Scroll, "Scroll"));
            Attributes.Add(new UICurve(UVSet.Rot, "Rotation"));
            Attributes.Add(new UICurve(UVSet.RotRandom, "Rotation Random"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/UV" + Idx;
            if (ImGui.TreeNode("UV " + Idx + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
