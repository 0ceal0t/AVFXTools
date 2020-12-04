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
    public class UILife : UIBase
    {
        public AVFXLife Life;

        public UILife(AVFXLife life)
        {
            Life = life;
            Init();
        }
        public override void Init()
        {
            base.Init();
            //==========================
            Attributes.Add(new UIFloat("Value", Life.Value));
            Attributes.Add(new UIFloat("Random Value", Life.ValRandom));
            Attributes.Add(new UICombo<RandomType>("Random Type", Life.ValRandomType));
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Life" + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
