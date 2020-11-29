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
    public class UIParticleDataWindmill : UIBase
    {
        public AVFXParticleDataWindmill Data;
        //==========================

        public UIParticleDataWindmill(AVFXParticleDataWindmill data)
        {
            Data = data;
            //=======================
            Attributes.Add(new UICombo<WindmillUVType>("Windmill UV Type", Data.WindmillUVType));
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
