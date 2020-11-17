using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class ParticleInstancePowderInstance : ParticleInstanceBase
    {
        public Vector2 ScaleStart;
        public Vector2 ScaleEnd;

        public Vector3 CurrentPos;
        public Vector3 CurrentVel;
        public float VMin;
        public float VMax;

        public int UVLoopMax;
        public int UVLoopCurrent;
        public float UVInterval;
        public int UVCellsX;
        public int UVCellsY;

        public ParticleInstancePowderInstance(AVFXParticle particle, ParticleItem item, Matrix4x4 prevTransform)
        {
            AVFXParticleSimple Simple = particle.Simple;
            PrevTransform = prevTransform;
            Age = 0.0f;
            Item = item;
            Life = Simple.CreateIntervalLife.Value;

            CurrentPos = new Vector3(0, 0, 0);
            VMin = Simple.VelMin.Value;
            VMax = Simple.VelMax.Value;
            CurrentVel = new Vector3(0, 0, 0);

            UVLoopMax = Simple.UvNoLoopCount.Value;
            UVLoopCurrent = 0;
            UVInterval = Simple.UvInterval.Value;
            UVCellsX = Simple.UvCellU.Value;
            UVCellsY = Simple.UvCellV.Value;

            ScaleStart = new Vector2(Simple.ScaleXStart.Value, Simple.ScaleYStart.Value);
            ScaleEnd = new Vector2(Simple.ScaleXEnd.Value, Simple.ScaleYEnd.Value);
        }

        public override void UpdateTime(float dT)
        {
            Age += dT;
            if (Age > Life && Life != -1)
            {
                Dead = true;
            }
            // UVs

        }

        public override void Reset()
        {
            Age = 0;
            Dead = false;
        }

        public override ParticleInstanceData GetData() { return GetData(Age); }
        public override ParticleInstanceData GetData(float time)
        {
            float Percent = Age / Life;
            Vector3 Scale = new Vector3(ScaleStart + (ScaleEnd - ScaleStart) * Percent, 1);
            int UVIterations = (int)Math.Floor(Age / UVInterval);
            int CellX = UVIterations % UVCellsX;
            int CellY = UVIterations % UVCellsY;

            Vector2 ScrollUV = new Vector2(-0.25f, -0.25f);
            Vector2 ScaleUV = new Vector2(0.5f, 0.5f);

            /* x scaled by 1/cells
             * y scaled by 1/2*cells
             * 
             * | ------------------- | -------------------------|
             *           |===========|=============|
             *   scale / 2                 scale / 2
             *          1/2                 1/2
             *          
             *          
             * |---------------|---------------------|----------------|
             *                 |==========|==========|
             */

            float UVScaleX = 1.0f / UVCellsX;
            float UVOverhangX = (0.5f / UVScaleX - 0.5f) * UVScaleX;
            float UVOffsetX = (float)CellX * UVScaleX;

            float UVScaleY = 1.0f / UVCellsY;
            float UVOverhangY = (0.5f / UVScaleY - 0.5f) * UVScaleY;
            float UVOffsetY = (float)CellY * UVScaleY;

            ScaleUV = new Vector2(UVScaleX, UVScaleY);
            ScrollUV = new Vector2(UVOffsetX - UVOverhangX, UVOffsetY - UVOverhangY);

            Matrix4x4 TranslationMatrix = GUtil.TransformMatrix(
                new Vector3(),
                new Vector3(1,1,1),
                CurrentPos,
                RotationOrder.ZYX,
                CoordComputeOrder.Scale_Rot_Translate
            ) * PrevTransform;

            return new ParticleInstanceData(
                TranslationMatrix,

                new Vector4(0,0,0,1),
                1,
                new Vector4(1,1,1,1),

                ScrollUV,
                ScaleUV,
                0,

                ScrollUV,
                ScaleUV,
                0,

                ScrollUV,
                ScaleUV,
                0,

                ScrollUV,
                ScaleUV,
                0,

                1,
                1
            );

            /*Matrix4x4 TranslationMatrix = GUtil.TransformMatrix(
                new Vector3(RotX.GetValue(time), RotY.GetValue(time), RotZ.GetValue(time)),
                new Vector3(ScaleX.GetValue(time), ScaleY.GetValue(time), ScaleZ.GetValue(time)),
                new Vector3(PosX.GetValue(time), PosY.GetValue(time), PosZ.GetValue(time)),
                _RotationOrder,
                _CoordOrder
            );
            if (_RotationBase == RotationDirectionBase.CameraBillboard)
            {
                TranslationMatrix = TranslationMatrix * Matrix4x4.CreateBillboard(new Vector3(0, 0, 0), Item.C.Cam.Position, Vector3.UnitY, Item.C.Cam.Forward);
            }
            else if (_RotationBase == RotationDirectionBase.TreeBillboard)
            {
                TranslationMatrix = TranslationMatrix * Matrix4x4.CreateConstrainedBillboard(new Vector3(0, 0, 0), Item.C.Cam.Position, Vector3.UnitY, Item.C.Cam.Forward, -1 * Vector3.UnitZ);
            }
            TranslationMatrix = TranslationMatrix * PrevTransform;

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
            );*/
        }
    }
}
