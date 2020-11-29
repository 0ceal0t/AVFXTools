using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;

namespace AVFXTools.UI
{
    public class UIInt : UIBase
    {
        public string Id;
        public int Value;
        public LiteralInt Literal;

        public delegate int Init(LiteralInt literal);
        public Init InitFunction;
        public delegate int Change(int value);
        public Change ChangeFunction;

        public UIInt(string id, LiteralInt literal, Init initFunction = null, Change changeFunction = null)
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
            if (ImGui.InputInt(Id + id, ref Value))
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
