using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXBinder : Base
    {
        public const string NAME = "Bind";

        public LiteralBool StartToGlobalDirection = new LiteralBool("startToGlobalDirection", "bStG");
        public LiteralBool VfxScaleEnabled = new LiteralBool("vfxScaleEnabled", "bVSc");
        public LiteralFloat VfxScaleBias = new LiteralFloat("vfxScaleBias", "bVSb");
        public LiteralBool VfxScaleDepthOffset = new LiteralBool("vfxScaleDepthOffset", "bVSd");
        public LiteralBool VfxScaleInterpolation = new LiteralBool("vfxScaleInterpolation", "bVSi");
        public LiteralBool TransformScale = new LiteralBool("transformScale", "bTSc");
        public LiteralBool TransformScaleDepthOffset = new LiteralBool("transformScaleDepthOffset", "bTSd");
        public LiteralBool TransformScaleInterpolation = new LiteralBool("transformScaleInterpolation", "bTSi");
        public LiteralBool FollowingTargetOrientation = new LiteralBool("followingTargetOrientation", "bFTO");
        public LiteralBool DocumentScaleEnabled = new LiteralBool("documentScaleEnabled", "bDSE");
        public LiteralBool AdjustToScreenEnabled = new LiteralBool("adjustToScreenEnabled", "bATS");
        public LiteralInt Life = new LiteralInt("life", "Life");
        public LiteralEnum<BinderRotation> BinderRotationType = new LiteralEnum<BinderRotation>("rotationtype", "RoTp");
        public LiteralEnum<BinderType> BinderVariety = new LiteralEnum<BinderType>("binderType", "BnVr");
        public AVFXBinderProperty PropStart = new AVFXBinderProperty("propertiesStart", "PrpS");
        public AVFXBinderProperty PropGoal = new AVFXBinderProperty("propertiesGoal", "PrpG");

        public BinderType Type;
        public AVFXBinderData Data;

        List<Base> Attributes;

        public AVFXBinder() : base("binder", NAME)
        {
            Attributes = new List<Base>(new Base[]{
                StartToGlobalDirection,
                VfxScaleEnabled,
                VfxScaleBias,
                VfxScaleDepthOffset,
                VfxScaleInterpolation,
                TransformScale,
                TransformScaleDepthOffset,
                TransformScaleInterpolation,
                FollowingTargetOrientation,
                DocumentScaleEnabled,
                AdjustToScreenEnabled,
                Life,
                BinderRotationType,
                BinderVariety,
                PropStart,
                PropGoal
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
            Type = BinderVariety.Value;

            // Data
            //========================//
            SetType(Type);
            ReadJSON(Data, elem);
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
            Type = BinderVariety.Value;

            foreach (AVFXNode item in node.Children)
            {
                switch (item.Name)
                {
                    // DATA =========================
                    case AVFXParticleData.NAME:
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
            AVFXNode bindAvfx = new AVFXNode("Bind");
            PutAVFX(bindAvfx, Attributes);
            PutAVFX(bindAvfx, Data);

            return bindAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- BIND --------", new String('\t', level));
            Output(Attributes, level);
            Output(Data, level);
        }

        public void SetType(BinderType type)
        {
            switch (type)
            {
                case BinderType.Point:
                    Data = new AVFXBinderDataPoint("data");
                    break;
                case BinderType.Linear:
                    Data = new AVFXBinderDataLinear("data");
                    break;
                case BinderType.Spline:
                    Data = new AVFXBinderDataSpline("data");
                    break;
                case BinderType.Camera:
                    Data = new AVFXBinderDataCamera("data");
                    break;
            }
        }
    }
}
