using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace AVFXTools.UI
{
    public class UIEffectorView : UIBase
    {
        List<UIEffector> Effectors = new List<UIEffector>();

        public UIEffectorView(AVFXBase avfx)
        {
            foreach (var effector in avfx.Effectors)
            {
                Effectors.Add(new UIEffector(effector));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##EFFECT";
            int eIdx = 0;
            foreach (var effector in Effectors)
            {
                effector.Idx = eIdx;
                effector.Draw(id);
                eIdx++;
            }
            if (ImGui.Button("+ Effector" + id))
            {
                // TODO
            }
        }
    }
}
