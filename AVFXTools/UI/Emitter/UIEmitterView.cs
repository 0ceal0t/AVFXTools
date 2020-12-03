using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace AVFXTools.UI
{
    public class UIEmitterView : UIBase
    {
        List<UIEmitter> Emitters = new List<UIEmitter>();

        public UIEmitterView(AVFXBase avfx)
        {
            foreach (var emitter in avfx.Emitters)
            {
                Emitters.Add(new UIEmitter(emitter));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##EMIT";
            int eIdx = 0;
            foreach (var emitter in Emitters)
            {
                emitter.Idx = eIdx;
                emitter.Draw(id);
                eIdx++;
            }
            if (ImGui.Button("+ Emitter" + id))
            {
                // TODO
            }
        }
    }
}
