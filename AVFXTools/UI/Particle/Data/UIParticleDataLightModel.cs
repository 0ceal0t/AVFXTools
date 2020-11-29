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
    public class UIParticleDataLightModel : UIBase
    {
        public AVFXParticleDataLightModel Data;
        //==========================

        public UIParticleDataLightModel(AVFXParticleDataLightModel data)
        {
            Data = data;
            //=======================
            Attributes.Add(new UIInt("Model Index", Data.ModelIdx));
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
