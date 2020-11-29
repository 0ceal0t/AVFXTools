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
    public class LiteralEnum : LiteralBase
    {
        public string Value { get; set; }
        public Enum EnumType { get; set; }

        public LiteralEnum(Enum e, string jsonPath, string avfxName, int size = 4, bool inJson = true, bool inAVFX = true) : base(jsonPath, avfxName, size, inJson, inAVFX)
        {
            EnumType = e;
        }

        public override void read(JObject json)
        {
        }

        public override void read(AVFXNode node)
        {
        }

        public override void read(JValue value)
        {
            if ((int)value != -1)
            {
                Value = (string)value;
            }
            Assigned = true;
        }

        public override void read(AVFXLeaf leaf)
        {
            int intValue = Util.Bytes4ToInt(leaf.Contents);
            if (intValue != -1) // means none
            {
                Value = Enum.GetName(EnumType.GetType(), intValue);

                if (Value == null)
                {
                    throw new System.InvalidOperationException(intValue.ToString() + " " + EnumType.GetType().ToString());
                }
            }

            Size = leaf.Size;
            Assigned = true;
        }

        public void GiveValue(string value)
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
            int enumValue = -1;
            if(Value != null)
            {
                enumValue = (int)Enum.Parse(EnumType.GetType(), Value, true);
            }
            return new AVFXLeaf(AVFXName, Size, Util.IntTo4Bytes(enumValue));
        }

        public override string stringValue()
        {
            return Value;
        }
    }
}
