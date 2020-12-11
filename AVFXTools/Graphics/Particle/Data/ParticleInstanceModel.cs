using AVFXLib.Models;
using System.Collections.Generic;
using System.Numerics;

namespace AVFXTools.Main
{
    public class ParticleInstanceModel : ParticleInstance
    {
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

        public CurveRandomGroup ColorR;
        public CurveRandomGroup ColorG;
        public CurveRandomGroup ColorB;
        public CurveRandomGroup ColorA;
        public CurveRandomGroup ColorBri;
        public CurveRandomGroup Color_ScaleR;
        public CurveRandomGroup Color_ScaleG;
        public CurveRandomGroup Color_ScaleB;
        public CurveRandomGroup Color_ScaleA;

        public CurveRandomGroup UV1_ScrollX;
        public CurveRandomGroup UV1_ScrollY;
        public CurveRandomGroup UV1_ScaleX;
        public CurveRandomGroup UV1_ScaleY;
        public CurveRandomGroup UV1_Rot;

        public CurveRandomGroup UV2_ScrollX;
        public CurveRandomGroup UV2_ScrollY;
        public CurveRandomGroup UV2_ScaleX;
        public CurveRandomGroup UV2_ScaleY;
        public CurveRandomGroup UV2_Rot;

        public CurveRandomGroup UV3_ScrollX;
        public CurveRandomGroup UV3_ScrollY;
        public CurveRandomGroup UV3_ScaleX;
        public CurveRandomGroup UV3_ScaleY;
        public CurveRandomGroup UV3_Rot;

        public CurveRandomGroup UV4_ScrollX;
        public CurveRandomGroup UV4_ScrollY;
        public CurveRandomGroup UV4_ScaleX;
        public CurveRandomGroup UV4_ScaleY;
        public CurveRandomGroup UV4_Rot;

        public CurveRandomGroup NormalPower;
        public CurveRandomGroup DistortPower;

        int UVLen;
        bool UV1_ = false;
        bool UV2_ = false;
        bool UV3_ = false;
        bool UV4_ = false;
        bool TN_ = false;
        bool TD_ = false;

        public ParticleInstanceModel(
            AVFXParticle particle,
            ParticleItem item,
            GenericInstance parent,
            Matrix4x4 startTransform,
            EmitterCreateStruct createData
            )
        {
            Age = 0.0f;
            Life = particle.Life.Value.Value;
            Item = item;

            Parent = parent;
            StartTransform = startTransform;
            CreateData = createData;

            CurrentTransform = Matrix4x4.Identity;
            //======================================
            _RotationOrder = particle.RotationOrder.Value;
            _CoordOrder = particle.CoordComputeOrder.Value;
            _RotationBase = particle.RotationDirectionBase.Value;

            if(item.Type == ParticleType.Powder)
            {
                IsSpawner = true;
                Spawner = new ParticleInstancePowderSpawner(particle, this);
            }

            List<AVFXParticleUVSet> UVS = particle.UVSets;
            UVLen = UVS.Count;

            PosX = new CurveRandomGroup("PosX", particle.Position.X, particle.Position.RX);
            PosY = new CurveRandomGroup("PosY", particle.Position.Y, particle.Position.RY);
            PosZ = new CurveRandomGroup("PosZ", particle.Position.Z, particle.Position.RZ);
            RotX = new CurveRandomGroup("RotX", particle.Rotation.X, particle.Rotation.RX);
            RotY = new CurveRandomGroup("RotY", particle.Rotation.Y, particle.Rotation.RY);
            RotZ = new CurveRandomGroup("RotZ", particle.Rotation.Z, particle.Rotation.RZ);
            ScaleX = new CurveRandomGroup("ScaleX", particle.Scale.X, particle.Scale.RX, D: 1.0f);
            ScaleY = new CurveRandomGroup("ScaleY", particle.Scale.Y, particle.Scale.RY, D: 1.0f);
            ScaleZ = new CurveRandomGroup("ScaleZ", particle.Scale.Z, particle.Scale.RZ, D: 1.0f);

            CurveRandomAttribute.ConnectAxis(particle.Position.AxisConnectType.Value, ref PosX, ref PosY, ref PosZ);
            CurveRandomAttribute.ConnectAxis(particle.Rotation.AxisConnectType.Value, ref RotX, ref RotY, ref RotZ);
            CurveRandomAttribute.ConnectAxis(particle.Scale.AxisConnectType.Value, ref ScaleX, ref ScaleY, ref ScaleZ);

            ColorR = new CurveRandomGroup("ColorR", CurveRandomAttribute.SplitCurve(particle.Color.RGB, 0), particle.Color.RanR);
            ColorG = new CurveRandomGroup("ColorG", CurveRandomAttribute.SplitCurve(particle.Color.RGB, 1), particle.Color.RanG);
            ColorB = new CurveRandomGroup("ColorB", CurveRandomAttribute.SplitCurve(particle.Color.RGB, 2), particle.Color.RanB);
            ColorA = new CurveRandomGroup("ColorA", particle.Color.A, particle.Color.RanA, D: 1.0f);
            ColorBri = new CurveRandomGroup("ColorBri", particle.Color.Bri, particle.Color.RBri, D: 1.0f);
            Color_ScaleR = new CurveRandomGroup("Color_ScaleR", particle.Color.SclR, null, D: 1.0f);
            Color_ScaleG = new CurveRandomGroup("Color_ScaleG", particle.Color.SclG, null, D: 1.0f);
            Color_ScaleB = new CurveRandomGroup("Color_ScaleB", particle.Color.SclB, null, D: 1.0f);
            Color_ScaleA = new CurveRandomGroup("Color_ScaleA", particle.Color.SclA, null, D: 1.0f);
            // UV1
            if (UVLen > 0)
            {
                var UV = UVS[0];
                UV1_ = true;
                UV1_ScrollX = new CurveRandomGroup("UV1_ScrollX", UV.Scroll.X, UV.Scroll.RX);
                UV1_ScrollY = new CurveRandomGroup("UV1_ScrollY", UV.Scroll.Y, UV.Scroll.RY);
                UV1_ScaleX = new CurveRandomGroup("UV1_ScaleX", UV.Scale.X, UV.Scale.RX, D: 1.0f);
                UV1_ScaleY = new CurveRandomGroup("UV1_ScaleY", UV.Scale.Y, UV.Scale.RY, D: 1.0f);
                UV1_Rot = new CurveRandomGroup("UV1_Rot", UV.Rot, UV.RotRandom);

                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV1_ScrollX, ref UV1_ScrollY);
                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV1_ScaleX, ref UV1_ScaleY);
            }
            // UV2
            if (UVLen > 1)
            {
                var UV = UVS[1];
                UV2_ = true;
                UV2_ScrollX = new CurveRandomGroup("UV2_ScrollX", UV.Scroll.X, UV.Scroll.RX);
                UV2_ScrollY = new CurveRandomGroup("UV2_ScrollY", UV.Scroll.Y, UV.Scroll.RY);
                UV2_ScaleX = new CurveRandomGroup("UV2_ScaleX", UV.Scale.X, UV.Scale.RX, D: 1.0f);
                UV2_ScaleY = new CurveRandomGroup("UV2_ScaleY", UV.Scale.Y, UV.Scale.RY, D: 1.0f);
                UV2_Rot = new CurveRandomGroup("UV2_Rot", UV.Rot, UV.RotRandom);

                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV2_ScrollX, ref UV2_ScrollY);
                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV2_ScaleX, ref UV2_ScaleY);
            }
            // UV3
            if (UVLen > 2)
            {
                var UV = UVS[2];
                UV3_ = true;
                UV3_ScrollX = new CurveRandomGroup("UV3_ScrollX", UV.Scroll.X, UV.Scroll.RX);
                UV3_ScrollY = new CurveRandomGroup("UV3_ScrollY", UV.Scroll.Y, UV.Scroll.RY);
                UV3_ScaleX = new CurveRandomGroup("UV3_ScaleX", UV.Scale.X, UV.Scale.RX, D: 1.0f);
                UV3_ScaleY = new CurveRandomGroup("UV3_ScaleY", UV.Scale.Y, UV.Scale.RY, D: 1.0f);
                UV3_Rot = new CurveRandomGroup("UV3_Rot", UV.Rot, UV.RotRandom);

                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV3_ScrollX, ref UV3_ScrollY);
                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV3_ScaleX, ref UV3_ScaleY);
            }
            // UV4
            if (UVLen > 3)
            {
                var UV = UVS[3];
                UV4_ = true;
                UV4_ScrollX = new CurveRandomGroup("UV4_ScrollX", UV.Scroll.X, UV.Scroll.RX);
                UV4_ScrollY = new CurveRandomGroup("UV4_ScrollY", UV.Scroll.Y, UV.Scroll.RY);
                UV4_ScaleX = new CurveRandomGroup("UV4_ScaleX", UV.Scale.X, UV.Scale.RX, D: 1.0f);
                UV4_ScaleY = new CurveRandomGroup("UV4_ScaleY", UV.Scale.Y, UV.Scale.RY, D: 1.0f);
                UV4_Rot = new CurveRandomGroup("UV4_Rot", UV.Rot, UV.RotRandom);

                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV4_ScrollX, ref UV4_ScrollY);
                CurveRandomAttribute.ConnectAxis(UV.Scale.AxisConnectType.Value, ref UV4_ScaleX, ref UV4_ScaleY);
            }
            // NPow
            var TN = particle.TN.UvSetIdx;
            if (TN.Assigned && TN.Value != -1 && TN.Value < UVLen)
            {
                TN_ = true;
                NormalPower = new CurveRandomGroup("N_Pow", particle.TN.NPow, null);
            }
            // DPow
            var TD = particle.TD.UvSetIdx;
            if (TD.Assigned && TD.Value != -1 && TD.Value < UVLen)
            {
                TD_ = true;
                DistortPower = new CurveRandomGroup("D_Pow", particle.TD.DPow, null);
            }
        }

        public override void UpdateTime(float dT)
        {
            Age += dT;
            if (Age > Life && Life != -1)
            {
                Dead = true;
            }
            CurrentTransform = GetMatrix(Age) * GetCurrentTransform();

            if(Item.ParticleIdx == 3)
            {
                //CurrentTransform = Matrix4x4.CreateRotationZ((float)Math.PI);//* Matrix4x4.CreateRotationZ((float)Math.PI * 0);
                CurrentTransform = GetMatrix(Age);
            }

            // CHECK TO SEE IF POWDER NEEDS TO BE SPAWNED
            if (Spawner != null)
            {
                Spawner.UpdateTime(dT);
            }
        }

        public override void Reset()
        {
            Age = 0;
            Dead = false;

            PosX.Reset();
            PosY.Reset();
            PosZ.Reset();
            ScaleX.Reset();
            ScaleY.Reset();
            ScaleZ.Reset();
            RotX.Reset();
            RotY.Reset();
            RotZ.Reset();

            UV1_ScrollX.Reset();
            UV1_ScrollY.Reset();
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

        public override ParticleInstanceData GetData() { return GetData(Age); }
        public override ParticleInstanceData GetData(float time)
        {

            Matrix4x4 TranslationMatrix = CurrentTransform;
            if (_RotationBase == RotationDirectionBase.CameraBillboard)
            {
                TranslationMatrix = TranslationMatrix * Matrix4x4.CreateBillboard(new Vector3(0, 0, 0), Item.C.Cam.Position, Vector3.UnitY, Item.C.Cam.Forward);
            }
            else if (_RotationBase == RotationDirectionBase.TreeBillboard)
            {
                TranslationMatrix = TranslationMatrix * Matrix4x4.CreateConstrainedBillboard(new Vector3(0, 0, 0), Item.C.Cam.Position, Vector3.UnitY, Item.C.Cam.Forward, -1 * Vector3.UnitZ);
            }

            return new ParticleInstanceData(
                TranslationMatrix,

                new Vector4(ColorR.GetValue(time), ColorG.GetValue(time), ColorB.GetValue(time), ColorA.GetValue(time)),
                ColorBri.GetValue(time),
                new Vector4(Color_ScaleR.GetValue(time), Color_ScaleG.GetValue(time), Color_ScaleB.GetValue(time), Color_ScaleA.GetValue(time)),

                UV1_ ? new Vector2(UV1_ScrollX.GetValue(time), UV1_ScrollY.GetValue(time)) : new Vector2(),
                UV1_ ? new Vector2(UV1_ScaleX.GetValue(time), UV1_ScaleY.GetValue(time)) : new Vector2(1.0f, 1.0f),
                UV1_ ? UV1_Rot.GetValue(time) : 0.0f,

                UV2_ ? new Vector2(UV2_ScrollX.GetValue(time), UV2_ScrollY.GetValue(time)) : new Vector2(),
                UV2_ ? new Vector2(UV2_ScaleX.GetValue(time), UV2_ScaleY.GetValue(time)) : new Vector2(1.0f, 1.0f),
                UV2_ ? UV2_Rot.GetValue(time) : 0.0f,

                UV3_ ? new Vector2(UV3_ScrollX.GetValue(time), UV3_ScrollY.GetValue(time)) : new Vector2(),
                UV3_ ? new Vector2(UV3_ScaleX.GetValue(time), UV3_ScaleY.GetValue(time)) : new Vector2(1.0f, 1.0f),
                UV3_ ? UV3_Rot.GetValue(time) : 0.0f,

                UV4_ ? new Vector2(UV4_ScrollX.GetValue(time), UV4_ScrollY.GetValue(time)) : new Vector2(),
                UV4_ ? new Vector2(UV4_ScaleX.GetValue(time), UV4_ScaleY.GetValue(time)) : new Vector2(1.0f, 1.0f),
                UV4_ ? UV4_Rot.GetValue(time) : 0.0f,

                TN_ ? NormalPower.GetValue(time) : 0.0f,
                TD_ ? DistortPower.GetValue(time) : 0.0f
            );
        }
    }
}
