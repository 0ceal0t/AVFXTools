using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;
using System.Numerics;

namespace AVFXTools.UI
{
    public class UIFloat2 : UIBase
    {
        public string Id;
        public Vector2 Value;

        public LiteralFloat Literal1;
        public LiteralFloat Literal2;

        public delegate Vector2 Init(LiteralFloat literal1, LiteralFloat literal2);
        public Init InitFunction;
        public delegate Vector2 Change(Vector2 value);
        public Change ChangeFunction;

        public UIFloat2(string id, LiteralFloat literal1, LiteralFloat literal2, Init initFunction = null, Change changeFunction = null)
        {
            Id = id;
            Literal1 = literal1;
            Literal2 = literal2;
            if (initFunction != null)
                InitFunction = initFunction;
            if (changeFunction != null)
                ChangeFunction = changeFunction;
            // =====================
            if (InitFunction == null)
            {
                Value = new Vector2(Literal1.Value, Literal2.Value);
            }
            else
            {
                Value = InitFunction(Literal1, Literal2);
            }
        }

        public override void Draw(string id)
        {
            if (ImGui.InputFloat2(Id + id, ref Value))
            {
                if (ChangeFunction == null)
                {
                    Literal1.GiveValue(Value.X);
                    Literal2.GiveValue(Value.Y);
                }
                else
                {
                    Vector2 V = ChangeFunction(Value);
                    Literal1.GiveValue(V.X);
                    Literal2.GiveValue(V.Y);
                }
            }
        }
    }
}
