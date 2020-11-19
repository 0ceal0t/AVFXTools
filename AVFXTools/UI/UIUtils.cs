﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace AVFXTools.UI
{
    public class UIUtils
    {
        public static bool EnumComboBox(string label, string[] options, ref int choiceIdx)
        {
            bool ret = false;
            if (ImGui.BeginCombo(label, options[choiceIdx]))
            {
                for (int idx = 0; idx < options.Length; idx++)
                {
                    bool is_selected = (choiceIdx == idx);
                    if(ImGui.Selectable(options[idx], is_selected))
                    {
                        choiceIdx = idx;
                        ret = true;
                    }

                    if (is_selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
            return ret;
        }
    }
}
