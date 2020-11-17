using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXParticleUVSet : Base
    {
        public const string NAME = "UvSt";

        public LiteralEnum CalculateUVType = new LiteralEnum(new TextureCalculateUV(), "calculateUV", "CUvT");
        public AVFXCurve2Axis Scale = new AVFXCurve2Axis("scale", "Scl");
        public AVFXCurve2Axis Scroll = new AVFXCurve2Axis("scroll", "Scr");
        public AVFXCurve Rot = new AVFXCurve("rotation", "Rot");
        public AVFXCurve RotRandom = new AVFXCurve("rotationRandom", "RotR");

        List<Base> Attributes;

        public AVFXParticleUVSet() : base("uvSet", NAME)
        {
            Attributes = new List<Base>(new Base[] {
                CalculateUVType,
                Scale,
                Scroll,
                Rot,
                RotRandom
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode uvstAvfx = new AVFXNode("UvSt");
            PutAVFX(uvstAvfx, Attributes);
            return uvstAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- UVSt --------", new String('\t', level));
            Output(Attributes, level);
        }
    }
}
