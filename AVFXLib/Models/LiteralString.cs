using AVFXLib.AVFX;
using AVFXLib.Main;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class LiteralString : LiteralBase
    {
        public string Value { get; set; }

        public LiteralString(string jsonPath, string avfxName, int size = 4, bool inJson = true, bool inAVFX = true) : base(jsonPath, avfxName, size, inJson, inAVFX)
        {
        }

        public override void read(JObject json)
        {
        }

        public override void read(AVFXNode node)
        {
        }

        public override void read(JValue value)
        {
            Value = (string)value;
            Size = Value.Length;
            Assigned = true;
        }

        public override void read(AVFXLeaf leaf)
        {
            Value = Util.BytesToString(leaf.Contents);
            Size = leaf.Size;
            Assigned = true;
        }

        public void GiveValue(string value)
        {
            Value = value;
            Size = Value.Length;
            Assigned = true;
        }

        public override JToken toJSON()
        {
            return new JValue(Value);
        }

        public override AVFXNode toAVFX()
        {
            return new AVFXLeaf(AVFXName, Size, Util.StringToBytes(Value));
        }

        public override string stringValue()
        {
            return Value.ToString();
        }
    }
}
