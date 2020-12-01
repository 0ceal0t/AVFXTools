using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;

namespace AVFXTools.UI
{
    public class UICombo<T> : UIBase
    {
        public string Id;
        public int Value;
        public LiteralEnum<T> Literal;

        public delegate int Init(LiteralEnum<T> literal, string[] options);
        public Init InitFunction;
        public delegate string Change(int value, string[] options);
        public Change ChangeFunction;

        public UICombo(string id, LiteralEnum<T> literal, Init initFunction = null, Change changeFunction = null)
        {
            Id = id;
            Literal = literal;
            if (initFunction != null)
                InitFunction = initFunction;
            if (changeFunction != null)
                ChangeFunction = changeFunction;
            // =====================
            if (InitFunction == null)
            {
                Value = (int)(object)Literal.Value;
            }
            else
            {
                Value = InitFunction(Literal, Literal.Options);
            }
        }

        public override void Draw(string id)
        {
            if (ImGui.BeginCombo(Id + id, Literal.Options[Value]))
            {
                for (int i = 0; i < Literal.Options.Length; i++)
                {
                    bool isSelected = (Value == i);
                    if (ImGui.Selectable(Literal.Options[i], isSelected))
                    {
                        Value = i;
                        if(ChangeFunction == null)
                        {
                            Literal.GiveValue(Literal.Options[Value]);
                        }
                        else
                        {
                            Literal.GiveValue(ChangeFunction(Value, Literal.Options));
                        }
                    }
                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }
    }
}
