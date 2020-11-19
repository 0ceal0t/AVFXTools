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
    public class UILife
    {
        public AVFXLife Life;
        //====================
        public float Value;
        public float ValRandom;
        public static readonly string[] RandomTypeOptions = Enum.GetNames(typeof(RandomType));
        public int RandomTypeIdx;

        public UILife(AVFXLife life)
        {
            Life = life;
            //==========================
            Value = Life.Value.Value;
            ValRandom = Life.ValRandom.Value;
            RandomTypeIdx = Array.IndexOf(RandomTypeOptions, Life.ValRandomType.Value);
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Life" + id))
            {
                if (ImGui.DragFloat("Value" + id, ref Value, 1, -1))
                {
                    Life.Value.GiveValue(Value);
                }
                if (ImGui.DragFloat("Random Value" + id, ref ValRandom, 1, 0))
                {
                    Life.ValRandom.GiveValue(ValRandom);
                }
                if (UIUtils.EnumComboBox("Random Type" + id, RandomTypeOptions, ref RandomTypeIdx))
                {
                    Life.ValRandomType.GiveValue(RandomTypeOptions[RandomTypeIdx]);
                }
                ImGui.TreePop();
            }
        }
    }
}
