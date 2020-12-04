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
        public delegate void Change(LiteralString literal);
        public Change ChangeFunction;

        public UIString(string id, LiteralString literal, Change changeFunction = null, int maxSizeBytes = 164)
        {
            Id = id;
            Literal = literal;
            Value = new byte[maxSizeBytes];
            if (changeFunction != null)
                ChangeFunction = changeFunction;
            else
                ChangeFunction = DoNothing;
            // =====================
            byte[] val = Util.StringToBytes(Literal.Value);
            Buffer.BlockCopy(val, 0, Value, 0, val.Length);
        }

        public override void Draw(string id)
        {
            ImGui.InputText(Id + id, Value, (uint)Value.Length);
            ImGui.SameLine();
            if (ImGui.Button("Update" + id))
            {
                Literal.GiveValue(Util.BytesToString(Value).Trim('\0'));
                ChangeFunction(Literal);
            }
        }

        public static void DoNothing(LiteralString literal) { }
    }
}
