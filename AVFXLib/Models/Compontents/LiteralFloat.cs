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
    public class LiteralFloat : LiteralBase
    {
        public float Value { get; set; }

        public LiteralFloat(string jsonPath, string avfxName, int size = 4, bool inJson = true, bool inAVFX = true) : base(jsonPath, avfxName, size, inJson, inAVFX)
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
            Value = (float)value;
            Assigned = true;
        }

        public override void read(AVFXLeaf leaf)
        {
            Value = Util.Bytes4ToFloat(leaf.Contents);
            Size = leaf.Size;
            Assigned = true;
        }

        public void GiveValue(float value)
        {
            Value = value;
            Assigned = true;
        }

        public override JToken toJSON()
        {
            return new JValue(Value);
        }

        public override AVFXNode toAVFX()
        {
            return new AVFXLeaf(AVFXName, Size, Util.FloatTo4Bytes(Value));
        }

        public override string stringValue()
        {
            return Value.ToString();
        }
    }
}
