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
    public class UIBinderDataPoint : UIBinderDataBase
    {
        public AVFXBinderDataPoint Data;
        //=======================
        public UICurve SpringStrength;

        public UIBinderDataPoint(AVFXBinderDataPoint data)
        {
            Data = data;
            //==================
            SpringStrength = new UICurve(data.SpringStrength, "Spring Strength");
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Data" + id))
            {
                SpringStrength.Draw(id + "-springstrength");
                ImGui.TreePop();
            }
        }
    }
}
