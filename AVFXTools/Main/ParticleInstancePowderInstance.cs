using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class ParticleInstancePowderInstance : ParticleInstance
    {
        public ParticleInstancePowderSpawner PowerSpawner;

        public Vector2 ScaleStart;
        public Vector2 ScaleEnd;
        public Vector2 ScaleRandom;

        public Vector3 CurrentPos;
        public Vector3 CurrentVel;
        public float VMin;
        public float VMax;

        public int UVLoopMax;
        public int UVLoopCurrent;
        public float UVInterval;
        public int UVCellsX;
        public int UVCellsY;

        public Vector4 Color;

        public ParticleInstancePowderInstance(
            AVFXParticle particle,
            ParticleItem item,
            GenericInstance parent,
            Matrix4x4 startTransform,
            EmitterCreateStruct createData,
            ParticleInstancePowderSpawner spawner
            )
        {
            PowerSpawner = spawner;
            AVFXParticleSimple Simple = particle.Simple;

            Parent = parent;
            StartTransform = startTransform;
            CreateData = createData;
            Age = 0.0f;
            Item = item;
            Life = Simple.CreateIntervalLife.Value;
            // ===================================
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
            ScaleRandom = new Vector2(GUtil.GetRandom(Simple.ScaleRandX0.Value, Simple.ScaleRandX1.Value), GUtil.GetRandom(Simple.ScaleRandY0.Value, Simple.ScaleRandY1.Value));

            var data = PowerSpawner.Instance.GetData();
            Color = data.Color;
        }

        public override void UpdateTime(float dT)
        {
            Age += dT;
            if (Age > Life && Life != -1)
            {
                Dead = true;
            }
            // UVs
            CurrentTransform = GetCurrentTransform();
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
            Vector3 Scale = new Vector3(ScaleStart + (ScaleEnd - ScaleStart) * Percent * ScaleRandom, 1);
            int UVIterations = (int)Math.Floor(Age / UVInterval);
            int CellX = UVIterations % UVCellsX;
            int CellY = UVIterations % UVCellsY;

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

            Vector2  ScaleUV = new Vector2(UVScaleX, UVScaleY);
            Vector2  ScrollUV = new Vector2(UVOffsetX - UVOverhangX, UVOffsetY - UVOverhangY);

            Matrix4x4 TranslationMatrix = GUtil.TransformMatrix(
                new Vector3(),
                Scale,
                CurrentPos,
                RotationOrder.ZYX,
                CoordComputeOrder.Scale_Rot_Translate
            ) * CurrentTransform;

            return new ParticleInstanceData(
                TranslationMatrix,
                Color,
                1,
                new Vector4(1,1,1,1),
                ScrollUV, ScaleUV, 0,
                ScrollUV, ScaleUV, 0,
                ScrollUV, ScaleUV, 0,
                ScrollUV, ScaleUV, 0,
                1, 1
            );
        }
    }
}
