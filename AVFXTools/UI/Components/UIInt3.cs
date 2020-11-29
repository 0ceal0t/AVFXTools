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
    public class UIInt3 : UIBase
    {
        public string Id;
        public Vector3 Value;

        public LiteralInt Literal1;
        public LiteralInt Literal2;
        public LiteralInt Literal3;

        public delegate Vector3 Init(LiteralInt literal1, LiteralInt literal2, LiteralInt literal3);
        public Init InitFunction;
        public delegate Vector3 Change(Vector3 value);
        public Change ChangeFunction;

        public UIInt3(string id, LiteralInt literal1, LiteralInt literal2, LiteralInt literal3, Init initFunction = null, Change changeFunction = null)
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
                    Literal1.GiveValue((int)Value.X);
                    Literal2.GiveValue((int)Value.Y);
                    Literal3.GiveValue((int)Value.Z);
                }
                else
                {
                    Vector3 V = ChangeFunction(Value);
                    Literal1.GiveValue((int)V.X);
                    Literal2.GiveValue((int)V.Y);
                    Literal3.GiveValue((int)V.Z);
                }
            }
        }
    }
}
