using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXParticleDataPolyline : AVFXParticleData
    {
        public LiteralEnum<LineCreateType> CreateLineType = new LiteralEnum<LineCreateType>("createLineType", "LnCT");
        public LiteralEnum<NotBillboardBaseAxisType> NotBillBoardBaseAxisType = new LiteralEnum<NotBillboardBaseAxisType>("notBillBoardBaseAxisType", "NBBA");
        public LiteralInt BindWeaponType = new LiteralInt("bindWeaponType", "BWpT");
        public LiteralInt PointCount = new LiteralInt("pointCount", "PnC");
        public LiteralInt PointCountCenter = new LiteralInt("pointCountCenter", "PnCC");
        public LiteralInt PointCountEndDistortion = new LiteralInt("pointCountEndDistortion", "PnED");
        public LiteralBool UseEdge = new LiteralBool("useEdge", "bEdg");
        public LiteralBool NotBillboard = new LiteralBool("notBillboard", "bNtB");
        public LiteralBool BindWeapon = new LiteralBool("bindWeapon", "BdWp");
        public LiteralBool ConnectTarget = new LiteralBool("connectTarget", "bCtg");
        public LiteralBool ConnectTargetReverse = new LiteralBool("connectTargetReverse", "bCtr");
        public LiteralInt TagNumber = new LiteralInt("tagNumber", "TagN");
        public LiteralBool IsSpline = new LiteralBool("isSpline", "bSpl");
        public LiteralBool IsLocal = new LiteralBool("isLocal", "bLcl");

        public AVFXCurve Width = new AVFXCurve("width", "Wd");
        public AVFXCurve WidthBegin = new AVFXCurve("widthBegin", "WdB");
        public AVFXCurve WidthCenter = new AVFXCurve("widthCenter", "WdC");
        public AVFXCurve WidthEnd = new AVFXCurve("widthEnd", "WdE");
        public AVFXCurve Length = new AVFXCurve("length", "Len");
        public AVFXCurve Softness = new AVFXCurve("softness", "Sft");
        public AVFXCurveColor ColorBegin = new AVFXCurveColor("colorBegin", name: "ColB");
        public AVFXCurveColor ColorCenter = new AVFXCurveColor("colorCenter", name: "ColC");
        public AVFXCurveColor ColorEnd = new AVFXCurveColor("colorEnd", name: "ColE");
        public AVFXCurveColor ColorEdgeBegin = new AVFXCurveColor("colorEdgeBegin", name: "CoEB");
        public AVFXCurveColor ColorEdgeCenter = new AVFXCurveColor("colorEdgeCenter", name: "CoEC");
        public AVFXCurveColor ColorEdgeEnd = new AVFXCurveColor("colorEdgeEnd", name: "CoEE");

        List<Base> Attributes;

        public AVFXParticleDataPolyline(string jsonPath) : base(jsonPath, "Data")
        {
            Attributes = new List<Base>(new Base[]{
                CreateLineType,
                NotBillBoardBaseAxisType,
                BindWeaponType,
                PointCount,
                PointCountCenter,
                PointCountEndDistortion,
                UseEdge,
                NotBillboard,
                BindWeapon,
                ConnectTarget,
                ConnectTargetReverse,
                TagNumber,
                IsSpline,
                IsLocal,
                Width,
                WidthBegin,
                WidthCenter,
                WidthEnd,
                Length,
                Softness,
                ColorBegin,
                ColorCenter,
                ColorEnd,
                ColorEdgeBegin,
                ColorEdgeCenter,
                ColorEdgeEnd
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
