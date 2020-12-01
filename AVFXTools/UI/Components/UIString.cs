using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImGuiNET;
using AVFXLib.Models;
using AVFXLib.Main;

namespace AVFXTools.UI
{
    public class UIString : UIBase
    {
        public string Id;
        public LiteralString Literal;
        public byte[] Value;
        // ========================
        public delegate byte[] Init(LiteralString literal);
        public Init InitFunction;
        public delegate string Change(byte[] value);
        public Change ChangeFunction;

        public UIString(string id, LiteralString literal, Init initFunction = null, Change changeFunction = null, int maxSizeBytes = 164)
        {
            Id = id;
            Literal = literal;
            Value = new byte[maxSizeBytes];
            if (initFunction != null)
                InitFunction = initFunction;
            if (changeFunction != null)
                ChangeFunction = changeFunction;
            // =====================
            byte[] val;
            if (InitFunction == null)
            {
                val = Util.StringToBytes(Literal.Value);
            }
            else
            {
                val = InitFunction(Literal);
            }
            Buffer.BlockCopy(val, 0, Value, 0, val.Length);
        }

        public override void Draw(string id)
        {
            ImGui.InputText(Id + id, Value, (uint)Value.Length);
            ImGui.SameLine();
            if (ImGui.Button("Update" + id))
            {
                if (ChangeFunction == null)
                {
                    Literal.GiveValue(Util.BytesToString(Value).Trim('\0'));
                }
                else
                {
                    Literal.GiveValue(ChangeFunction(Value));
                }
            }
        }
    }
}
