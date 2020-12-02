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
        public int Idx;
        public bool JustOneCreate;
        public int NumCreated;
        public int CreateCount;

        public bool InfluenceCoordScale;
        public bool InfluenceCoordPos;
        public bool InfluenceCoordRot;

        public Vector3 InjectionAngle;

        public EmitterCreateStruct(
            int idx,
            bool justOne,
            AVFXEmitterIterationItem subItem
            ) : this(
                idx,
                justOne,
                subItem.InfluenceCoordPos.Value == 1,
                subItem.InfluenceCoordScale.Value == 1,
                subItem.InfluenceCoordRot.Value == 1,
                subItem.CreateCount.Value,
                injectionAngle: new Vector3(subItem.ByInjectionAngleX.Value, subItem.ByInjectionAngleY.Value, subItem.ByInjectionAngleZ.Value)
                )
        {
        }
        public EmitterCreateStruct(
            int idx,
            bool justOne,
            bool pos,
            bool scale,
            bool rot,
            int createCount,
            Vector3 injectionAngle = new Vector3()
            )
        {
            Idx = idx;
            JustOneCreate = justOne;
            NumCreated = 0;
            CreateCount = Math.Max(1,createCount);

            InfluenceCoordPos = pos;
            InfluenceCoordRot = rot;
            InfluenceCoordScale = scale;

            InjectionAngle = injectionAngle;
        }
    }

    public class EmitterInstance : GenericInstance
    {
        public float Age;
        public float Life;
        public bool Dead = false;
        public EmitterItem Item;

        public float TimeUntilCreate = 0.0f;
        public float CreateInterval;
        public int CreateCount;
        public List<EmitterCreateStruct> CreateParticles = new List<EmitterCreateStruct>();
        public List<EmitterCreateStruct> CreateEmitters = new List<EmitterCreateStruct>();

        public RotationOrder _RotationOrder;
        public CoordComputeOrder _CoordOrder;
        public RotationDirectionBase _RotationBase;
        public EmitterType _Type;

        public CurveRandomGroup PosX;
        public CurveRandomGroup PosY;
        public CurveRandomGroup PosZ;
        public CurveRandomGroup RotX;
        public CurveRandomGroup RotY;
        public CurveRandomGroup RotZ;
        public CurveRandomGroup ScaleX;
        public CurveRandomGroup ScaleY;
        public CurveRandomGroup ScaleZ;



        // ======== CYLINDER / SPHERE ========
        public CurveRandomGroup Radius;
        public CurveRandomGroup Length;
        public int DivideX;
        public int DivideY;

        public EmitterInstance(
            AVFXEmitter emitter,
            EmitterItem item,
            GenericInstance parent,
            Matrix4x4 startTransform,
            EmitterCreateStruct createData
        )
        {
            Age = 0.0f;
            Life = emitter.Life.Value.Value;
            Item = item;

            Parent = parent;
            StartTransform = startTransform;
            CreateData = createData;
            CurrentTransform = Matrix4x4.Identity;
            // =======================

            CreateCount = (int)emitter.CreateCount.Keys[0].Z;
            CreateInterval = emitter.CreateInterval.Keys[0].Z;

            // WHICH PARTICLES TO CREATE? ===========
            if(emitter.ItPrs.Count > 0)
            {
                var lastItPr = emitter.ItPrs[emitter.ItPrs.Count - 1];
                for(int idx = 0; idx < lastItPr.Items.Count; idx++)
                {
                    var SubItem = lastItPr.Items[idx];
                    int targetIdx = SubItem.TargetIdx.Value;
                    if(targetIdx != -1 && SubItem.Enabled.Value == true && targetIdx < Item.C.Particles.Length)  // TODO: sometimes it's over. why?
                    {
                        var Target = Item.C.Particles[targetIdx];
                        float targetLife = Target.Life;
                        CreateParticles.Add(new EmitterCreateStruct(
                            targetIdx,
                            targetLife == -1,
                            SubItem
                        ));
                    }
                }
            }
            // WHICH EMITTERS TO CREATE? ==============
            if (emitter.ItEms.Count > 0)
            {
                var lastItEm = emitter.ItEms[emitter.ItEms.Count - 1];
                for (int idx = 0; idx < lastItEm.Items.Count; idx++)
                {
                    var SubItem = lastItEm.Items[idx];
                    int targetIdx = SubItem.TargetIdx.Value;
                    if (targetIdx != -1 && SubItem.Enabled.Value == true && targetIdx < Item.C.Emitters.Length) // TODO: sometimes it's over. why?
                    {
                        var Target = Item.C.Emitters[targetIdx];
                        float targetLife = Target.Life;
                        CreateEmitters.Add(new EmitterCreateStruct(
                            targetIdx,
                            targetLife == -1,
                            SubItem
                        ));
                    }
                }
            }

            _RotationOrder = emitter.RotationOrder.Value;
            _CoordOrder = emitter.CoordComputeOrder.Value;
            _RotationBase = emitter.RotationDirectionBase.Value;
            _Type = emitter.EmitterVariety.Value;
            switch (_Type)
            {
                case EmitterType.CylinderModel:
                    AVFXEmitterDataCylinderModel data = (AVFXEmitterDataCylinderModel)emitter.Data;
                    Radius = new CurveRandomGroup("Radius", data.Radius, null);
                    Length = new CurveRandomGroup("Length", data.Length, null);
                    DivideX = data.DivideX.Value;
                    DivideY = data.DivideY.Value;
                    break;
            }

            PosX = new CurveRandomGroup("PosX", emitter.Position.X, emitter.Position.RX);
            PosY = new CurveRandomGroup("PosY", emitter.Position.Y, emitter.Position.RY);
            PosZ = new CurveRandomGroup("PosZ", emitter.Position.Z, emitter.Position.RZ);
            RotX = new CurveRandomGroup("RotX", emitter.Rotation.X, emitter.Rotation.RX);
            RotY = new CurveRandomGroup("RotY", emitter.Rotation.Y, emitter.Rotation.RY);
            RotZ = new CurveRandomGroup("RotZ", emitter.Rotation.Z, emitter.Rotation.RZ);
            ScaleX = new CurveRandomGroup("ScaleX", emitter.Scale.X, emitter.Scale.RX, D: 1.0f);
            ScaleY = new CurveRandomGroup("ScaleY", emitter.Scale.Y, emitter.Scale.RY, D: 1.0f);
            ScaleZ = new CurveRandomGroup("ScaleZ", emitter.Scale.Z, emitter.Scale.RZ, D: 1.0f);

            CurveRandomAttribute.ConnectAxis(emitter.Position.AxisConnectType.Value, ref PosX, ref PosY, ref PosZ);
            CurveRandomAttribute.ConnectAxis(emitter.Rotation.AxisConnectType.Value, ref RotX, ref RotY, ref RotZ);
            CurveRandomAttribute.ConnectAxis(emitter.Scale.AxisConnectType.Value, ref ScaleX, ref ScaleY, ref ScaleZ);
        }

        public void UpdateTime(float dT)
        {
            Age += dT;
            if (Life != -1 && Age > Life)
            {
                Age = 0;
                Reset(); // TEMP? need to figure out how timed emitters are created
            }
            CurrentTransform = GetMatrix(Age) * GetCurrentTransform();
            // ===== TIME TO CREATE? =====
            TimeUntilCreate -= dT;
            if(TimeUntilCreate < 0 && !Dead)
            {
                TimeUntilCreate = CreateInterval;
                foreach(var createEmitter in CreateEmitters)
                {
                    if (createEmitter.JustOneCreate && createEmitter.NumCreated > 0) continue;
                    createEmitter.NumCreated++;

                    for (int idx = 0; idx < createEmitter.CreateCount; idx++)
                    {
                        Matrix4x4 injectionMatrix = GUtil.RotationMatrix(idx * createEmitter.InjectionAngle, RotationOrder.ZXY);
                        Matrix4x4 startMatrix = injectionMatrix * GetNewInstancePosition(idx);
                        Item.C.AddEmitterInstance(createEmitter.Idx, this, startMatrix, createEmitter); // TODO: generate point better here
                    }
                }
                foreach (var createParticle in CreateParticles)
                {
                    if (createParticle.JustOneCreate && createParticle.NumCreated > 0) continue;
                    createParticle.NumCreated++;

                    for (int idx = 0; idx < createParticle.CreateCount; idx++)
                    {
                        Matrix4x4 injectionMatrix = GUtil.RotationMatrix(idx * createParticle.InjectionAngle, RotationOrder.ZXY);
                        Matrix4x4 startMatrix = injectionMatrix * GetNewInstancePosition(idx);
                        Item.C.AddParticleInstance(createParticle.Idx, this, startMatrix, createParticle, num: CreateCount); // TODO: generate point better here
                    }
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

        public Matrix4x4 GetMatrix(float time)
        {
            return GUtil.TransformMatrix(
                new Vector3(RotX.GetValue(time), RotY.GetValue(time), RotZ.GetValue(time)),
                new Vector3(ScaleX.GetValue(time), ScaleY.GetValue(time), ScaleZ.GetValue(time)),
                new Vector3(PosX.GetValue(time), PosY.GetValue(time), PosZ.GetValue(time)),
                _RotationOrder,
                _CoordOrder
            );
        }

        public Matrix4x4 GetNewInstancePosition(int NumCreated)
        {
            switch (_Type)
            {
                case EmitterType.SphereModel:
                    return Matrix4x4.Identity;
                case EmitterType.CylinderModel:
                    float R = Radius.GetValue(Age);
                    float Rot = (2 * (float)Math.PI / DivideX) * NumCreated;
                    Vector3 Position = new Vector3(R * ScaleX.GetValue(Age) * (float)Math.Cos(Rot), 0, R * ScaleY.GetValue(Age) * (float)Math.Sin(Rot));
                    return Matrix4x4.CreateTranslation(Position);
                default:
                    return Matrix4x4.Identity;
            }
        }
    }
}
