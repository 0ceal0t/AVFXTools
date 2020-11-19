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
    public class UIParticleDataLightModel : UIParticleDataBase
    {
        public AVFXParticleDataLightModel Data;
        //==========================
        public int ModelIdx;

        public UIParticleDataLightModel(AVFXParticleDataLightModel data)
        {
            Data = data;
            //=======================
            ModelIdx = Data.ModelIdx.Value;
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Data" + id))
            {
                if (ImGui.DragInt("Model Number" + id, ref ModelIdx, 1, 0))
                {
                    Data.ModelIdx.GiveValue(ModelIdx);
                }
                ImGui.TreePop();
            }
        }
    }
}
