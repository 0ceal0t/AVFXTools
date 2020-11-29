using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXBinderProperty : Base
    {
        public const string NAME = "PrpS";

        public LiteralEnum BindPointType = new LiteralEnum(new BindPoint(), "bindPointType", "BPT");
        public LiteralEnum BindTargetPointType = new LiteralEnum(new BindTargetPoint(), "bindTargetPointType", "BPTP");
        public LiteralString Name = new LiteralString("name", "Name");
        public LiteralInt BindPointId = new LiteralInt("bindPointId", "BPID");
        public LiteralInt GenerateDelay = new LiteralInt("generateDelay", "GenD");
        public LiteralInt CoordUpdateFrame = new LiteralInt("coordUpdateFrame", "CoUF");
        public LiteralBool RingEnable = new LiteralBool("ringEnable", "bRng");
        public LiteralInt RingProgressTime = new LiteralInt("ringProgressTime", "RnPT");
        public LiteralFloat RingPositionX = new LiteralFloat("ringPositionX", "RnPX");
        public LiteralFloat RingPositionY = new LiteralFloat("ringPositionY", "RnPY");
        public LiteralFloat RingPositionZ = new LiteralFloat("ringPositionZ", "RnPZ");
        public LiteralFloat RingRadius = new LiteralFloat("ringRadius", "RnRd");
        public AVFXCurve3Axis Position = new AVFXCurve3Axis("position", "Pos");

        List<Base> Attributes;

        public AVFXBinderProperty(string jsonPath) : base(jsonPath, NAME)
        {
            Attributes = new List<Base>(new Base[]{
                BindPointType,
                BindTargetPointType,
                Name,
                BindPointId,
                GenerateDelay,
                CoordUpdateFrame,
                RingEnable,
                RingProgressTime,
                RingPositionX,
                RingPositionY,
                RingPositionZ,
                RingRadius,
                Position
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
            AVFXNode dataAvfx = new AVFXNode("PrpS");
            PutAVFX(dataAvfx, Attributes);
            return dataAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- PrPs --------", new String('\t', level));
            Output(Attributes, level);
        }
    }
}
