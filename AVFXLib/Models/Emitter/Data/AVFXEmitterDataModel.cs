using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXEmitterDataModel : AVFXEmitterData
    {
        public LiteralInt ModelIdx = new LiteralInt("modelIdx", "MdNo");
        public LiteralEnum RotationOrderType = new LiteralEnum(new RotationOrder(), "rotationOrder", "ROT");
        public LiteralEnum GenerateMethodType = new LiteralEnum(new GenerateMethod(), "generateMethod", "GeMT");
        public AVFXCurve AX = new AVFXCurve("angleX", "AX");
        public AVFXCurve AY = new AVFXCurve("angleY", "AY");
        public AVFXCurve AZ = new AVFXCurve("angleZ", "AZ");
        public AVFXCurve InjectionSpeed = new AVFXCurve("injectionSpeed", "IjS");

        List<Base> Attributes;

        public AVFXEmitterDataModel(string jsonPath) : base(jsonPath, "Data")
        {
            Attributes = new List<Base>(new Base[] {
                ModelIdx,
                RotationOrderType,
                GenerateMethodType,
                AX,
                AY,
                AZ,
                InjectionSpeed
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
