using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class EmitterCreateStruct
    {

        /*
        * timed emitter -> -1 particle, just once
        * -1 emitter -> -1 particle, just once
        * -1 emitter -> timed particle, repeat
        * timed emitter -> timed particle, repeat
        */

        public int Idx;
        public bool JustOneCreate;
        public bool AlreadyCreated;

        public EmitterCreateStruct(int idx, bool justOne)
        {
            Idx = idx;
            JustOneCreate = justOne;
            AlreadyCreated = false;
        }
    }

    public class EmitterInstance
    {
        public float Age;
        public float Life;
        public bool Dead = false;
        public EmitterItem Item;
        public Matrix4x4 PrevTransform;

        public float TimeUntilCreate = 0.0f;
        public float CreateInterval;
        public int CreateCount;
        public List<EmitterCreateStruct> CreateParticles = new List<EmitterCreateStruct>();
        public List<EmitterCreateStruct> CreateEmitters = new List<EmitterCreateStruct>();

        public RotationOrder _RotationOrder;
        public CoordComputeOrder _CoordOrder;
        public RotationDirectionBase _RotationBase;

        public CurveRandomGroup PosX;
        public CurveRandomGroup PosY;
        public CurveRandomGroup PosZ;
        public CurveRandomGroup RotX;
        public CurveRandomGroup RotY;
        public CurveRandomGroup RotZ;
        public CurveRandomGroup ScaleX;
        public CurveRandomGroup ScaleY;
        public CurveRandomGroup ScaleZ;

        public EmitterInstance(AVFXEmitter emitter, EmitterItem item, Matrix4x4 prevTransform)
        {
            PrevTransform = prevTransform;
            Age = 0.0f;
            Life = emitter.Life.Value.Value;
            Item = item;

            CreateCount = (int)emitter.CreateCount.Keys[0].Z;
            CreateInterval = emitter.CreateInterval.Keys[0].Z;
            // WHICH PARTICLES TO CREATE? ===========
            if(emitter.ItPrs.Count > 0)
            {
                var lastItPr = emitter.ItPrs[emitter.ItPrs.Count - 1];
                for(int idx = 0; idx < lastItPr.Items.Count; idx++)
                {
                    int targetIdx = lastItPr.Items[idx].TargetIdx.Value;
                    if(targetIdx != -1)
                    {
                        float targetLife = Item.C.Particles[targetIdx].Life;
                        CreateParticles.Add(new EmitterCreateStruct(targetIdx, targetLife == -1));
                    }
                }
            }
            // WHICH EMITTERS TO CREATE? ==============
            if (emitter.ItEms.Count > 0)
            {
                var lastItEm = emitter.ItEms[emitter.ItEms.Count - 1];
                for (int idx = 0; idx < lastItEm.Items.Count; idx++)
                {
                    int targetIdx = lastItEm.Items[idx].TargetIdx.Value;
                    if (targetIdx != -1)
                    {
                        float targetLife = Item.C.Emitters[targetIdx].Life;
                        CreateEmitters.Add(new EmitterCreateStruct(targetIdx, targetLife == -1));
                    }
                }
            }

            _RotationOrder = (RotationOrder)Enum.Parse(typeof(RotationOrder), emitter.RotationOrder.Value, true);
            _CoordOrder = (CoordComputeOrder)Enum.Parse(typeof(CoordComputeOrder), emitter.CoordComputeOrder.Value, true);
            _RotationBase = (RotationDirectionBase)Enum.Parse(typeof(RotationDirectionBase), emitter.RotationDirectionBase.Value, true);

            PosX = new CurveRandomGroup("PosX", emitter.Position.X, emitter.Position.RX);
            PosY = new CurveRandomGroup("PosY", emitter.Position.Y, emitter.Position.RY);
            PosZ = new CurveRandomGroup("PosZ", emitter.Position.Z, emitter.Position.RZ);
            RotX = new CurveRandomGroup("RotX", emitter.Rotation.X, emitter.Rotation.RX);
            RotY = new CurveRandomGroup("RotY", emitter.Rotation.Y, emitter.Rotation.RY);
            RotZ = new CurveRandomGroup("RotZ", emitter.Rotation.Z, emitter.Rotation.RZ);
            ScaleX = new CurveRandomGroup("ScaleX", emitter.Scale.X, emitter.Scale.RX, D: 1.0f);
            ScaleY = new CurveRandomGroup("ScaleY", emitter.Scale.Y, emitter.Scale.RY, D: 1.0f);
            ScaleZ = new CurveRandomGroup("ScaleZ", emitter.Scale.Z, emitter.Scale.RZ, D: 1.0f);

            CurveRandomAttribute.AxisConnect(emitter.Position.AxisConnectType.Value, ref PosX, ref PosY, ref PosZ);
            CurveRandomAttribute.AxisConnect(emitter.Rotation.AxisConnectType.Value, ref RotX, ref RotY, ref RotZ);
            CurveRandomAttribute.AxisConnect(emitter.Scale.AxisConnectType.Value, ref ScaleX, ref ScaleY, ref ScaleZ);
        }

        public void UpdateTime(float dT)
        {
            Age += dT;
            if (Life != -1 && Age > Life)
            {
                Age = 0;
                Reset(); // TEMP? need to figure out how timed emitters are created
            }
            // ===== TIME TO CREATE? =====
            TimeUntilCreate -= dT;
            if(TimeUntilCreate < 0 && !Dead)
            {
                TimeUntilCreate = CreateInterval;
                foreach(var createEmitter in CreateEmitters)
                {
                    if (createEmitter.JustOneCreate && createEmitter.AlreadyCreated) continue;
                    createEmitter.AlreadyCreated = true;

                    Item.C.AddEmitterInstance(createEmitter.Idx, CreateCount, GetData());
                }
                foreach (var createParticle in CreateParticles)
                {
                    if (createParticle.JustOneCreate && createParticle.AlreadyCreated) continue;
                    createParticle.AlreadyCreated = true;

                    Item.C.AddParticleInstance(createParticle.Idx, CreateCount, GetData());
                }
            }
        }

        public void Reset()
        {
            TimeUntilCreate = CreateInterval;

            PosX.Reset();
            PosY.Reset();
            PosZ.Reset();
            ScaleX.Reset();
            ScaleY.Reset();
            ScaleZ.Reset();
            RotX.Reset();
            RotY.Reset();
            RotZ.Reset();
        }

        public Matrix4x4 GetData(){ return GetData(Age); }
        public Matrix4x4 GetData(float time)
        {
            // TODO: sphere / cylinder
            return PrevTransform * GUtil.TransformMatrix(
                new Vector3(RotX.GetValue(time), RotY.GetValue(time), RotZ.GetValue(time)),
                new Vector3(ScaleX.GetValue(time), ScaleY.GetValue(time), ScaleZ.GetValue(time)),
                new Vector3(PosX.GetValue(time), PosY.GetValue(time), PosZ.GetValue(time)),
                _RotationOrder,
                _CoordOrder
            );
        }
    }
}
