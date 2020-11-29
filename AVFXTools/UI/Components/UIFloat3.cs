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
    public class UIFloat3 : UIBase
    {
        public string Id;
        public Vector3 Value;

        public LiteralFloat Literal1;
        public LiteralFloat Literal2;
        public LiteralFloat Literal3;

        public delegate Vector3 Init(LiteralFloat literal1, LiteralFloat literal2, LiteralFloat literal3);
        public Init InitFunction;
        public delegate Vector3 Change(Vector3 value);
        public Change ChangeFunction;

        public UIFloat3(string id, LiteralFloat literal1, LiteralFloat literal2, LiteralFloat literal3, Init initFunction = null, Change changeFunction = null)
        {
            Id = id;
            Literal1 = literal1;
            Literal2 = literal2;
            Literal3 = literal3;
            if (initFunction != null)
                InitFunction = initFunction;
            if (changeFunction != null)
                ChangeFunction = changeFunction;
            // =====================
            if (InitFunction == null)
            {
                Value = new Vector3(Literal1.Value, Literal2.Value, Literal3.Value);
            }
            else
            {
                Value = InitFunction(Literal1, Literal2, Literal3);
            }
        }

        public override void Draw(string id)
        {
            if (ImGui.InputFloat3(Id + id, ref Value))
            {
                if (ChangeFunction == null)
                {
                    Literal1.GiveValue(Value.X);
                    Literal2.GiveValue(Value.Y);
                    Literal3.GiveValue(Value.Z);
                }
                else
                {
                    Vector3 V = ChangeFunction(Value);
                    Literal1.GiveValue(V.X);
                    Literal2.GiveValue(V.Y);
                    Literal3.GiveValue(V.Z);
                }
            }
        }
    }
}
