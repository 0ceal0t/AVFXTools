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
        public LiteralEnum Literal;
        public string[] Options = Enum.GetNames(typeof(T));

        public delegate int Init(LiteralEnum literal, string[] options);
        public Init InitFunction;
        public delegate string Change(int value, string[] options);
        public Change ChangeFunction;

        public UICombo(string id, LiteralEnum literal, Init initFunction = null, Change changeFunction = null)
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
                Value = Array.IndexOf(Options, Literal.Value);
            }
            else
            {
                Value = InitFunction(Literal, Options);
            }
        }

        public override void Draw(string id)
        {
            if (ImGui.BeginCombo(Id + id, Options[Value]))
            {
                for (int i = 0; i < Options.Length; i++)
                {
                    bool isSelected = (Value == i);
                    if (ImGui.Selectable(Options[i], isSelected))
                    {
                        Value = i;
                        if(ChangeFunction == null)
                        {
                            Literal.GiveValue(Options[Value]);
                        }
                        else
                        {
                            Literal.GiveValue(ChangeFunction(Value, Options));
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
