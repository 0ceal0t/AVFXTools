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
    public class UIBinderDataCamera : UIBase
    {
        public AVFXBinderDataCamera Data;
        //=======================

        public UIBinderDataCamera(AVFXBinderDataCamera data)
        {
            Data = data;
            //==================
            Attributes.Add(new UICurve(data.Distance, "Distance"));
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
