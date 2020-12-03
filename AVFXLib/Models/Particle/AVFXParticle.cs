using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXParticle : Base
    {
        public const string NAME = "Ptcl";

        public LiteralInt LoopStart = new LiteralInt("loopStart", "LpSt");
        public LiteralInt LoopEnd = new LiteralInt("loopEnd", "LpEd");
        public LiteralEnum<ParticleType> ParticleVariety = new LiteralEnum<ParticleType>("particleType", "PrVT");
        public LiteralEnum<RotationDirectionBase> RotationDirectionBase = new LiteralEnum<RotationDirectionBase>("rotationDirectionBase", "RBDT");
        public LiteralEnum<RotationOrder> RotationOrder = new LiteralEnum<RotationOrder>("rotationOrder", "RoOT");
        public LiteralEnum<CoordComputeOrder> CoordComputeOrder = new LiteralEnum<CoordComputeOrder>("coordComputeOrder", "CCOT");
        public LiteralEnum<DrawMode> DrawMode = new LiteralEnum<DrawMode>("drawMode", "RMT");
        public LiteralEnum<CullingType> CullingType = new LiteralEnum<CullingType>("cullingType", "CulT");
        public LiteralEnum<EnvLight> EnvLight = new LiteralEnum<EnvLight>("envLight", "EnvT");
        public LiteralEnum<DirLight> DirLight = new LiteralEnum<DirLight>("dirLight", "DirT");
        public LiteralEnum<UVPrecision> UvPrecision = new LiteralEnum<UVPrecision>("uvPrecision", "UVPT");
        public LiteralInt DrawPriority = new LiteralInt("drawPriority", "DwPr");
        public LiteralBool IsDepthTest = new LiteralBool("isDepthTest", "DsDt");
        public LiteralBool IsDepthWrite = new LiteralBool("isDepthWrite", "DsDw");
        public LiteralBool IsSoftParticle = new LiteralBool("isSoftParticle", "DsSp");
        public LiteralInt CollisionType = new LiteralInt("collisionType", "Coll");
        public LiteralBool Bs11 = new LiteralBool("bs11", "bS11");
        public LiteralBool IsApplyToneMap = new LiteralBool("isApplyToneMap", "bATM");
        public LiteralBool IsApplyFog = new LiteralBool("isApplyFog", "bAFg");
        public LiteralBool ClipNearEnable = new LiteralBool("clipNearEnable", "bNea");
        public LiteralBool ClipFarEnable = new LiteralBool("clipFarEnable", "bFar");
        public LiteralFloat ClipNearStart = new LiteralFloat("clipNearStart", "NeSt");
        public LiteralFloat ClipNearEnd = new LiteralFloat("clipNearEnd", "NeEd");
        public LiteralFloat ClipFarStart = new LiteralFloat("clipFarStart", "FaSt");
        public LiteralFloat ClipFarEnd = new LiteralFloat("clipFarEnd", "FaEd");
        public LiteralEnum<ClipBasePoint> ClipBasePoint = new LiteralEnum<ClipBasePoint>("clipBasePoint", "FaBP");
        public LiteralInt UvSetCount = new LiteralInt("uvSetCount", "UvSN");
        public LiteralInt ApplyRateEnvironment = new LiteralInt("applyRateEnvironment", "EvAR");
        public LiteralInt ApplyRateDirectional = new LiteralInt("applyRateDirectional", "DlAR");
        public LiteralInt ApplyRateLightBuffer = new LiteralInt("applyRateLightBuffer", "LBAR");
        public LiteralBool DOTy = new LiteralBool("DOTy", "DOTy");
        public LiteralFloat DepthOffset = new LiteralFloat("depthOffset", "DpOf");
        public LiteralBool SimpleAnimEnable = new LiteralBool("simpleAnimEnabled", "bSCt");
        public AVFXLife Life = new AVFXLife("life");
        public AVFXParticleSimple Simple = new AVFXParticleSimple("simpleAnimations");

        public AVFXCurve Gravity = new AVFXCurve("gravity", "Gra");
        public AVFXCurve GravityRandom = new AVFXCurve("gravityRandom", "GraR");
        public AVFXCurve AirResistance = new AVFXCurve("airResistance", "ARs");
        public AVFXCurve3Axis Scale = new AVFXCurve3Axis("scale", "Scl");
        public AVFXCurve3Axis Rotation = new AVFXCurve3Axis("rotation", "Rot");
        public AVFXCurve3Axis Position = new AVFXCurve3Axis("position", "Pos");
        public AVFXCurveColor Color = new AVFXCurveColor("color");

        // UVSets
        //=====================//
        public List<AVFXParticleUVSet> UVSets = new List<AVFXParticleUVSet>();

        // Data
        //=====================//
        public ParticleType Type;
        public AVFXParticleData Data;

        // Texture Properties
        //====================//
        public AVFXTextureColor1 TC1 = new AVFXTextureColor1("textureColor1");
        public AVFXTextureColor2 TC2 = new AVFXTextureColor2("textureColor2", "TC2");
        public AVFXTextureColor2 TC3 = new AVFXTextureColor2("textureColor3", "TC3");
        public AVFXTextureColor2 TC4 = new AVFXTextureColor2("textureColor4", "TC4");
        public AVFXTextureNormal TN = new AVFXTextureNormal("textureNormal");
        public AVFXTextureReflection TR = new AVFXTextureReflection("textureReflection");
        public AVFXTextureDistortion TD = new AVFXTextureDistortion("textureDistortion");
        public AVFXTexturePalette TP = new AVFXTexturePalette("texturePalette");

        List<Base> Attributes;
        List<Base> Attributes2;

        public AVFXParticle() : base("particles", NAME)
        {
            Attributes = new List<Base>(new Base[]{
                LoopStart,
                LoopEnd,
                ParticleVariety,
                RotationDirectionBase,
                RotationOrder,
                CoordComputeOrder,
                DrawMode,
                CullingType,
                EnvLight,
                DirLight,
                UvPrecision,
                DrawPriority,
                IsDepthTest,
                IsDepthWrite,
                IsSoftParticle,
                CollisionType,
                Bs11,
                IsApplyToneMap,
                IsApplyFog,
                ClipNearEnable,
                ClipFarEnable,
                ClipNearStart,
                ClipNearEnd,
                ClipFarStart,
                ClipFarEnd,
                ClipBasePoint,
                UvSetCount,
                ApplyRateEnvironment,
                ApplyRateDirectional,
                ApplyRateLightBuffer,
                DOTy,
                DepthOffset,
                SimpleAnimEnable,
                Life,
                Simple,
                Gravity,
                GravityRandom,
                AirResistance,
                Scale,
                Rotation,
                Position,
                Color
            });

            Attributes2 = new List<Base>(new Base[]{
                TC1,
                TC2,
                TC3,
                TC4,
                TN,
                TR,
                TD,
                TP
            });
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
            Type = ParticleVariety.Value;

            foreach (AVFXNode item in node.Children) 
            {
                switch (item.Name) {
                    // UVSET =================================
                    case AVFXParticleUVSet.NAME:
                        AVFXParticleUVSet UVSet = new AVFXParticleUVSet();
                        UVSet.read(item);
                        UVSets.Add(UVSet);
                        break;

                    // DATA ==================================
                    case AVFXParticleData.NAME:
                        SetType(Type);
                        ReadAVFX(Data, node);
                        break;
                }
            }

            ReadAVFX(Attributes2, node);
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
            Type = ParticleVariety.Value;

            // UVSets
            //=======================//
            JArray uvElems = (JArray)elem.GetValue("uvSets");
            foreach (JToken u in uvElems)
            {
                AVFXParticleUVSet UVSet = new AVFXParticleUVSet();
                UVSet.read((JObject)u);
                UVSets.Add(UVSet);
            }

            // Data
            //========================//
            SetType(Type);
            ReadJSON(Data, elem);
            ReadJSON(Attributes2, elem);
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);

            JArray uvArray = new JArray();
            foreach(AVFXParticleUVSet uvSet in UVSets)
            {
                uvArray.Add(uvSet.toJSON());
            }

            elem["uvSets"] = uvArray;
            PutJSON(elem, Data);
            PutJSON(elem, Attributes2);
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode ptclAvfx = new AVFXNode("Ptcl");

            PutAVFX(ptclAvfx, Attributes);

            // UVSets
            //=======================//
            foreach (AVFXParticleUVSet uvElem in UVSets)
            {
                PutAVFX(ptclAvfx, uvElem);
            }

            PutAVFX(ptclAvfx, Data);
            PutAVFX(ptclAvfx, Attributes2);

            return ptclAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- PTCL --------", new String('\t', level));
            Output(Attributes, level);

            // UVSets
            //=======================//
            foreach (AVFXParticleUVSet uvElem in UVSets)
            {
                Output(uvElem, level);
            }

            Output(Data, level);
            Output(Attributes2, level);
        }

        public void SetType(ParticleType type)
        {
            switch (type)
            {
                case ParticleType.Parameter:
                    throw new System.InvalidOperationException("Parameter Particle Data!");
                    Data = new AVFXParticleDataParameter("data");
                    break;
                case ParticleType.Powder:
                    Data = new AVFXParticleDataPowder("data");
                    break;
                case ParticleType.Windmill:
                    Data = new AVFXParticleDataWindmill("data");
                    break;
                case ParticleType.Line:
                    throw new System.InvalidOperationException("Line Particle Data!");
                    Data = new AVFXParticleDataLine("data");
                    break;
                case ParticleType.Model:
                    Data = new AVFXParticleDataModel("data");
                    break;
                case ParticleType.Polyline:
                    Data = new AVFXParticleDataPolyline("data");
                    break;
                case ParticleType.Reserve0:
                    break;
                case ParticleType.Quad:
                    break;
                case ParticleType.Polygon:
                    Data = new AVFXParticleDataPolygon("data");
                    break;
                case ParticleType.Decal:
                    Data = new AVFXParticleDataDecal("data");
                    break;
                case ParticleType.DecalRing:
                    Data = new AVFXParticleDataDecalRing("data");
                    break;
                case ParticleType.Disc:
                    Data = new AVFXParticleDataDisc("data");
                    break;
                case ParticleType.LightModel:
                    Data = new AVFXParticleDataLightModel("data");
                    break;
            }
        }
    }
}
