using AVFXLib.AVFX;
using AVFXLib.Main;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXBase : Base
    {
        public LiteralBool IsDelayFastParticle = new LiteralBool("isDelayFastParticle", "bDFP");
        public LiteralBool IsFitGround = new LiteralBool("isFitGround", "bFG");
        public LiteralBool IsTranformSkip = new LiteralBool("isTranformSkip", "bTS");
        public LiteralBool IsAllStopOnHide = new LiteralBool("isAllStopOnHide", "bASH");
        public LiteralBool CanBeClippedOut = new LiteralBool("canBeClippedOut", "bCBC");
        public LiteralBool ClipBoxenabled = new LiteralBool("clipBoxenabled", "bCul");
        public LiteralFloat ClipBoxX = new LiteralFloat("clipBoxX", "CBPx");
        public LiteralFloat ClipBoxY = new LiteralFloat("clipBoxY", "CBPy");
        public LiteralFloat ClipBoxZ = new LiteralFloat("clipBoxZ", "CBPz");
        public LiteralFloat ClipBoxsizeX = new LiteralFloat("clipBoxsizeX", "CBSx");
        public LiteralFloat ClipBoxsizeY = new LiteralFloat("clipBoxsizeY", "CBSy");
        public LiteralFloat ClipBoxsizeZ = new LiteralFloat("clipBoxsizeZ", "CBSz");
        public LiteralFloat BiasZmaxScale = new LiteralFloat("biasZmaxScale", "ZBMs");
        public LiteralFloat BiasZmaxDistance = new LiteralFloat("biasZmaxDistance", "ZBMd");
        public LiteralBool IsCameraSpace = new LiteralBool("isCameraSpace", "bCmS");
        public LiteralBool IsFullEnvLight = new LiteralBool("isFullEnvLight", "bFEL");
        public LiteralBool IsClipOwnSetting = new LiteralBool("isClipOwnSetting", "bOSt");
        public LiteralFloat SoftParticleFadeRange = new LiteralFloat("softParticleFadeRange", "SPFR");
        public LiteralFloat SoftKeyOffset = new LiteralFloat("softKeyOffset", "SKO");
        public LiteralEnum<DrawLayer> DrawLayerType = new LiteralEnum<DrawLayer>("drawLayerType", "DwLy");
        public LiteralEnum<DrawOrder> DrawOrderType = new LiteralEnum<DrawOrder>("drawOrderType", "DwOT");
        public LiteralEnum<DirectionalLightSource> DirectionalLightSourceType = new LiteralEnum<DirectionalLightSource>("directionalLightSourceType", "DLST");
        public LiteralEnum<PointLightSouce> PointLightsType1 = new LiteralEnum<PointLightSouce>("pointLightsType1", "PL1S");
        public LiteralEnum<PointLightSouce> PointLightsType2 = new LiteralEnum<PointLightSouce>("pointLightsType2", "PL2S");
        public LiteralFloat RevisedValuesPosX = new LiteralFloat("revisedValuesPosX", "RvPx");
        public LiteralFloat RevisedValuesPosY = new LiteralFloat("revisedValuesPosY", "RvPy");
        public LiteralFloat RevisedValuesPosZ = new LiteralFloat("revisedValuesPosZ", "RvPz");
        public LiteralFloat RevisedValuesRotX = new LiteralFloat("revisedValuesRotX", "RvRx");
        public LiteralFloat RevisedValuesRotY = new LiteralFloat("revisedValuesRotY", "RvRy");
        public LiteralFloat RevisedValuesRotZ = new LiteralFloat("revisedValuesRotZ", "RvRz");
        public LiteralFloat RevisedValuesScaleX = new LiteralFloat("revisedValuesScaleX", "RvSx");
        public LiteralFloat RevisedValuesScaleY = new LiteralFloat("revisedValuesScaleY", "RvSy");
        public LiteralFloat RevisedValuesScaleZ = new LiteralFloat("revisedValuesScaleZ", "RvSz");
        public LiteralFloat RevisedValuesR = new LiteralFloat("revisedValuesR", "RvR");
        public LiteralFloat RevisedValuesG = new LiteralFloat("revisedValuesG", "RvG");
        public LiteralFloat RevisedValuesB = new LiteralFloat("revisedValuesB", "RvB");
        public LiteralBool FadeXenabled = new LiteralBool("fadeXenabled", "AFXe");
        public LiteralFloat FadeXinner = new LiteralFloat("fadeXinner", "AFXi");
        public LiteralFloat FadeXouter = new LiteralFloat("fadeXouter", "AFXo");
        public LiteralBool FadeYenabled = new LiteralBool("fadeYenabled", "AFYe");
        public LiteralFloat FadeYinner = new LiteralFloat("fadeYinner", "AFYi");
        public LiteralFloat FadeYouter = new LiteralFloat("fadeYouter", "AFYo");
        public LiteralBool FadeZenabled = new LiteralBool("fadeZenabled", "AFZe");
        public LiteralFloat FadeZinner = new LiteralFloat("fadeZinner", "AFZi");
        public LiteralFloat FadeZouter = new LiteralFloat("fadeZouter", "AFZo");
        public LiteralBool GlobalFogEnabled = new LiteralBool("globalFogEnabled", "bGFE");
        public LiteralFloat GlobalFogInfluence = new LiteralFloat("globalFogInfluence", "GFIM");
        public LiteralBool LTSEnabled = new LiteralBool("globalFogEnabled", "bLTS");

        public List<AVFXSchedule> Schedulers = new List<AVFXSchedule>();
        public List<AVFXTimeline> Timelines = new List<AVFXTimeline>();
        public List<AVFXEmitter> Emitters = new List<AVFXEmitter>();
        public List<AVFXParticle> Particles = new List<AVFXParticle>();
        public List<AVFXEffector> Effectors = new List<AVFXEffector>();
        public List<AVFXBinder> Binders = new List<AVFXBinder>();
        public List<AVFXTexture> Textures = new List<AVFXTexture>();
        public List<AVFXModel> Models = new List<AVFXModel>();

        List<Base> Attributes;

        public AVFXBase() : base("", "AVFX")
        {
            Attributes = new List<Base>(new Base[]{
                IsDelayFastParticle,
                IsFitGround,
                IsTranformSkip,
                IsAllStopOnHide,
                CanBeClippedOut,
                ClipBoxenabled,
                ClipBoxX,
                ClipBoxY,
                ClipBoxZ,
                ClipBoxsizeX,
                ClipBoxsizeY,
                ClipBoxsizeZ,
                BiasZmaxScale,
                BiasZmaxDistance,
                IsCameraSpace,
                IsFullEnvLight,
                IsClipOwnSetting,
                SoftParticleFadeRange,
                SoftKeyOffset,
                DrawLayerType,
                DrawOrderType,
                DirectionalLightSourceType,
                PointLightsType1,
                PointLightsType2,
                RevisedValuesPosX,
                RevisedValuesPosY,
                RevisedValuesPosZ,
                RevisedValuesRotX,
                RevisedValuesRotY,
                RevisedValuesRotZ,
                RevisedValuesScaleX,
                RevisedValuesScaleY,
                RevisedValuesScaleZ,
                RevisedValuesR,
                RevisedValuesG,
                RevisedValuesB,
                FadeXenabled,
                FadeXinner,
                FadeXouter,
                FadeYenabled,
                FadeYinner,
                FadeYouter,
                FadeZenabled,
                FadeZinner,
                FadeZouter,
                GlobalFogEnabled,
                GlobalFogInfluence,
                LTSEnabled
            });
        }

        public override void read(JObject elem) {
            Assigned = true;
            JObject avfxElem = (JObject)elem.GetValue("params");
            ReadJSON(Attributes, avfxElem);

            // SCHEDULE
            JArray schdElems = (JArray)elem.GetValue("schedulers");
            foreach (JToken s in schdElems)
            {
                AVFXSchedule Scheduler = new AVFXSchedule();
                Scheduler.read((JObject)s);
                Schedulers.Add(Scheduler);
            }
            // TIMELINES
            JArray tmlnElems = (JArray)elem.GetValue("timelines");
            foreach (JToken t in tmlnElems)
            {
                AVFXTimeline Timeline = new AVFXTimeline();
                Timeline.read((JObject)t);
                Timelines.Add(Timeline);
            }
            // EMITTERS
            JArray emitElems = (JArray)elem.GetValue("emitters");
            foreach (JToken e in emitElems)
            {
                AVFXEmitter Emitter = new AVFXEmitter();
                Emitter.read((JObject)e);
                Emitters.Add(Emitter);
            }
            // PARTICLES
            JArray ptclElems = (JArray)elem.GetValue("particles");
            foreach (JToken p in ptclElems)
            {
                AVFXParticle Particle = new AVFXParticle();
                Particle.read((JObject)p);
                Particles.Add(Particle);
            }
            // EFFECTORS
            JArray effctorElems = (JArray)elem.GetValue("effectors");
            foreach (JToken e in effctorElems)
            {
                AVFXEffector Effector = new AVFXEffector();
                Effector.read((JObject)e);
                Effectors.Add(Effector);
            }
            // BINDERS
            JArray bndElems = (JArray)elem.GetValue("binders");
            foreach (JToken b in bndElems)
            {
                AVFXBinder Binder = new AVFXBinder();
                Binder.read((JObject)b);
                Binders.Add(Binder);
            }
            // TEXTURES
            JArray texElems = (JArray)elem.GetValue("textures");
            foreach (JToken t in texElems)
            {
                AVFXTexture Texture = new AVFXTexture();
                Texture.read((JObject)t);
                Textures.Add(Texture);
            }
            // MODELS
            JArray mdlElems = (JArray)elem.GetValue("models");
            foreach (JToken m in mdlElems)
            {
                AVFXModel Model = new AVFXModel();
                Model.read((JObject)m);
                Models.Add(Model);
            }
        }

        public override void read(AVFXNode node) {
            Assigned = true;
            ReadAVFX(Attributes, node);

            foreach (AVFXNode item in node.Children)
            {
                switch (item.Name)
                {
                    case AVFXSchedule.NAME:
                        AVFXSchedule Scheduler = new AVFXSchedule();
                        Scheduler.read(item);
                        Schedulers.Add(Scheduler);
                        break;
                    case AVFXTimeline.NAME:
                        AVFXTimeline Timeline = new AVFXTimeline();
                        Timeline.read(item);
                        Timelines.Add(Timeline);
                        break;
                    case AVFXEmitter.NAME:
                        AVFXEmitter Emitter = new AVFXEmitter();
                        Emitter.read(item);
                        Emitters.Add(Emitter);
                        break;
                    case AVFXParticle.NAME:
                        AVFXParticle Particle = new AVFXParticle();
                        Particle.read(item);
                        Particles.Add(Particle);
                        break;
                    case AVFXEffector.NAME:
                        AVFXEffector Effector = new AVFXEffector();
                        Effector.read(item);
                        Effectors.Add(Effector);
                        break;
                    case AVFXBinder.NAME:
                        AVFXBinder Binder = new AVFXBinder();
                        Binder.read(item);
                        Binders.Add(Binder);
                        break;
                    case AVFXTexture.NAME:
                        AVFXTexture Texture = new AVFXTexture();
                        Texture.read(item);
                        Textures.Add(Texture);
                        break;
                    case AVFXModel.NAME:
                        AVFXModel Model = new AVFXModel();
                        Model.read(item);
                        Models.Add(Model);
                        break;
                }
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();

            JObject paramElem = new JObject();
            PutJSON(paramElem, Attributes);
            elem["params"] = paramElem;

            // SCHEDULE
            JArray schedArray = new JArray();
            foreach (AVFXSchedule schedElem in Schedulers)
            {
                schedArray.Add(schedElem.toJSON());
            }
            elem["schedulers"] = schedArray;
            // TIMELINES
            JArray timeArray = new JArray();
            foreach (AVFXTimeline tmlnElem in Timelines)
            {
                timeArray.Add(tmlnElem.toJSON());
            }
            elem["timelines"] = timeArray;
            // EMITTERS
            JArray emitArray = new JArray();
            foreach (AVFXEmitter emitterElem in Emitters)
            {
                emitArray.Add(emitterElem.toJSON());
            }
            elem["emitters"] = emitArray;
            // PARTICLES
            JArray particleArray = new JArray();
            foreach (AVFXParticle particleElement in Particles)
            {
                particleArray.Add(particleElement.toJSON());
            }
            elem["particles"] = particleArray;
            // EFFECTORS
            JArray effectArray = new JArray();
            foreach (AVFXEffector effectorElem in Effectors)
            {
                effectArray.Add(effectorElem.toJSON());
            }
            elem["effectors"] = effectArray;
            // BINDERS
            JArray bindArray = new JArray();
            foreach (AVFXBinder bindElem in Binders)
            {
                bindArray.Add(bindElem.toJSON());
            }
            elem["binders"] = bindArray;
            // TEXTURES
            JArray texArray = new JArray();
            foreach (AVFXTexture texElem in Textures)
            {
                texArray.Add(texElem.toJSON());
            }
            elem["textures"] = texArray;
            // MODELS
            JArray modelArray = new JArray();
            foreach (AVFXModel modelElem in Models)
            {
                modelArray.Add(modelElem.toJSON());
            }
            elem["models"] = modelArray;

            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode baseAVFX = new AVFXNode("AVFX");

            baseAVFX.Children.Add(new AVFXLeaf("Ver", 4, new byte[] { 0x13, 0x09, 0x11, 0x20 })); //?
            PutAVFX(baseAVFX, Attributes);

            baseAVFX.Children.Add(new AVFXLeaf("ScCn", 4, BitConverter.GetBytes(Schedulers.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("TlCn", 4, BitConverter.GetBytes(Timelines.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("EmCn", 4, BitConverter.GetBytes(Emitters.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("PrCn", 4, BitConverter.GetBytes(Particles.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("EfCn", 4, BitConverter.GetBytes(Effectors.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("BdCn", 4, BitConverter.GetBytes(Binders.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("TxCn", 4, BitConverter.GetBytes(Textures.Count())));
            baseAVFX.Children.Add(new AVFXLeaf("MdCn", 4, BitConverter.GetBytes(Models.Count())));

            // SCHEDULE
            foreach (AVFXSchedule schedElem in Schedulers)
            {
                baseAVFX.Children.Add(schedElem.toAVFX());
            }
            // TIMELINES
            foreach (AVFXTimeline tmlnElem in Timelines)
            {
                baseAVFX.Children.Add(tmlnElem.toAVFX());
            }
            // EMITTERS
            foreach (AVFXEmitter emitterElem in Emitters)
            {
                baseAVFX.Children.Add(emitterElem.toAVFX());
            }
            // PARTICLES
            foreach (AVFXParticle particleElement in Particles)
            {
                baseAVFX.Children.Add(particleElement.toAVFX());
            }
            // EFFECTORS
            foreach (AVFXEffector effectorElem in Effectors)
            {
                baseAVFX.Children.Add(effectorElem.toAVFX());
            }
            // BINDERS
            foreach (AVFXBinder bindElem in Binders)
            {
                baseAVFX.Children.Add(bindElem.toAVFX());
            }
            // TEXTURES
            foreach (AVFXTexture texElem in Textures)
            {
                baseAVFX.Children.Add(texElem.toAVFX());
            }
            // MODELS
            foreach (AVFXModel modelElem in Models)
            {
                baseAVFX.Children.Add(GetAVFX(modelElem));
            }
            return baseAVFX;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- AVFX --------", new String('\t',level));
            Output(Attributes, level);

            // SCHEDULE
            foreach (AVFXSchedule schedElem in Schedulers)
            {
                Output(schedElem, level);
            }
            // TIMELINES
            foreach (AVFXTimeline tmlnElem in Timelines)
            {
                Output(tmlnElem, level);
            }
            // EMITTERS
            foreach (AVFXEmitter emitterElem in Emitters)
            {
                Output(emitterElem, level);
            }
            // PARTICLES
            foreach (AVFXParticle particleElement in Particles)
            {
                Output(particleElement, level);
            }
            // EFFECTORS
            foreach (AVFXEffector effectorElem in Effectors)
            {
                Output(effectorElem, level);
            }
            // BINDERS
            foreach (AVFXBinder bindElem in Binders)
            {
                Output(bindElem, level);
            }
            // TEXTURES
            foreach (AVFXTexture texElem in Textures)
            {
                Output(texElem, level);
            }
            // MODELS
            foreach (AVFXModel modelElem in Models)
            {
                Output(modelElem, level);
            }
        }

        public AVFXBase clone()
        {
            AVFXNode node = toAVFX();
            AVFXBase ret = new AVFXBase();
            ret.read(node);
            return ret;
        }
    }
}
