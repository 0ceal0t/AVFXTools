using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXEffector : Base
    {
        public const string NAME = "Efct";

        public LiteralEnum EffectorType = new LiteralEnum(new EffectorType(), "effectorType", "EfVT");
        public LiteralEnum RotationOrder = new LiteralEnum(new RotationOrder(), "rotationOrder", "RoOT");
        public LiteralEnum CoordComputeOrder = new LiteralEnum(new CoordComputeOrder(), "coordComputeOrder", "CCOT");
        public LiteralBool AffectOtherVfx = new LiteralBool("affectOtherVfx", "bAOV");
        public LiteralBool AffectGame = new LiteralBool("affectGame", "bAGm");
        public LiteralInt LoopPointStart = new LiteralInt("loopPointStart", "LpSt");
        public LiteralInt LoopPointEnd = new LiteralInt("loopPointEnd", "LpEd");

        public string Type;
        public AVFXEffectorData Data;

        List<Base> Attributes;

        public AVFXEffector() : base("effectors", NAME)
        {
            Attributes = new List<Base>(new Base[]{
                EffectorType,
                RotationOrder,
                CoordComputeOrder,
                AffectOtherVfx,
                AffectGame,
                LoopPointStart,
                LoopPointEnd
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
            Type = EffectorType.Value;

            // Data
            //========================//
            SetType(Type);
            ReadJSON(Data, elem);
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
            Type = EffectorType.Value;

            foreach (AVFXNode item in node.Children){
                switch (item.Name){
                    // DATA ==================================
                    case AVFXEffectorData.NAME:
                            SetType(Type);
                        ReadAVFX(Data, node);
                        break;
                }
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);
            PutJSON(elem, Data);
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode effectorAvfx = new AVFXNode("Efct");
            PutAVFX(effectorAvfx, Attributes);

            PutAVFX(effectorAvfx, Data);
            return effectorAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- EFCT --------", new String('\t', level));
            Output(Attributes, level);

            Output(Data, level);
        }

        public void SetType(string type)
        {
            switch (Type)
            {
                case "PointLight":
                    Data = new AVFXEffectorDataPointLight("data");
                    break;
                case "DirectionalLight":
                    throw new System.InvalidOperationException("Directional Light Effector!");
                    break;
                case "RadialBlur":
                    Data = new AVFXEffectorDataRadialBlur("data");
                    break;
                case "BlackHole":
                    throw new System.InvalidOperationException("Black Hole Effector!");
                    break;
                case "CameraQuake":
                    Data = new AVFXEffectorDataCameraQuake("data");
                    break;
            }
        }
    }
}
