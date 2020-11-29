using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXEffectorDataRadialBlur : AVFXEffectorData
    {
        public LiteralFloat FadeStartDistance = new LiteralFloat("fadeStartDistance", "FSDc");
        public LiteralFloat FadeEndDistance = new LiteralFloat("fadeEndDistance", "FEDc");
        public LiteralEnum FadeBasePointType = new LiteralEnum(new ClipBasePoint(), "fadeBasePoint", "FaBP");
        public AVFXCurve Length = new AVFXCurve("length", "Len");
        public AVFXCurve Strength = new AVFXCurve("strength", "Str");
        public AVFXCurve Gradation = new AVFXCurve("gradation", "Gra");
        public AVFXCurve InnerRadius = new AVFXCurve("innerRadius", "IRad");
        public AVFXCurve OuterRadius = new AVFXCurve("outerRadius", "ORad");

        List<Base> Attributes;

        public AVFXEffectorDataRadialBlur(string jsonPath) : base(jsonPath, "Data")
        {
            Attributes = new List<Base>(new Base[]{
                FadeStartDistance,
                FadeEndDistance,
                FadeBasePointType,
                Length,
                Strength,
                Gradation,
                InnerRadius,
                OuterRadius
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
            AVFXNode dataAvfx = new AVFXNode("Data");
            PutAVFX(dataAvfx, Attributes);
            return dataAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- DATA --------", new String('\t', level));
            Output(Attributes, level);
        }
    }
}
