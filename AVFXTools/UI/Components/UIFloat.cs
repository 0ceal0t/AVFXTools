using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;

namespace AVFXTools.UI
{
    public class UIFloat : UIBase
    {
        public string Id;
        public float Value;
        public LiteralFloat Literal;

        public delegate float Init(LiteralFloat literal);
        public Init InitFunction;
        public delegate float Change(float value);
        public Change ChangeFunction;

        public UIFloat(string id, LiteralFloat literal, Init initFunction = null, Change changeFunction = null)
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
                Value = Literal.Value;
            }
            else
            {
                Value = InitFunction(Literal);
            }
        }

        public override void Draw(string id)
        {
            if (ImGui.InputFloat(Id + id, ref Value))
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
