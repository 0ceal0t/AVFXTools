using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;

namespace AVFXTools.UI
{
    public class UICheckbox : UIBase
    {
        public string Id;
        public bool Value;
        public LiteralBool Literal;

        public delegate bool Init(LiteralBool literal);
        public Init InitFunction;
        public delegate bool Change(bool value);
        public Change ChangeFunction;

        public int SL;

        public UICheckbox(string id, LiteralBool literal, Delegate initFunction = null, Delegate changeFunction = null, int sl = 0)
        {
            Id = id;
            Literal = literal;
            SL = sl;
            if (initFunction != null)
                InitFunction = (Init)initFunction;
            if (changeFunction != null)
                ChangeFunction = (Change)changeFunction;
            // =====================
            if (InitFunction == null)
            {
                Value = (Literal.Value == true);
            }
            else
            {
                Value = InitFunction(Literal);
            }
        }

        public override void Draw(string id)
        {
            if (SL > 0) ImGui.SameLine(SL);
            if (ImGui.Checkbox(Id + id, ref Value))
            {
                if (ChangeFunction == null)
                {
                    Literal.GiveValue(Value);
                }
                else
                {
                    Literal.GiveValue(ChangeFunction(Value));
                }
            }
        }
    }
}
