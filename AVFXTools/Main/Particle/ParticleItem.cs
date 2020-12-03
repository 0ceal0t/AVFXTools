using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class ParticleItem
    {
        public AVFXParticle Particle;
        public Core C;
        // =================
        public float Life;
        public int ParticleIdx;
        public bool DoDraw = false;
        public ParticleType Type;
        public VertexPositionTexture[] Verts;
        public ushort[] Indexes;
        public ParticlePipeline Pipe;
        public int InstanceNum = 0;
        public ParticleInstance[] Instances;

        public ParticleItem(AVFXParticle particle, Core core, int pIdx)
        {
            ParticleIdx = pIdx;
            Particle = particle;
            C = core;
            // ================
            Life = particle.Life.Value.Value;
            Type = particle.ParticleVariety.Value;

            switch (Type)
            {
                // ======= MODEL ========
                case ParticleType.Model:
                    var modelData = (AVFXParticleDataModel)particle.Data;
                    if (!InitModel(modelData.ModelIdx.Value)) return;
                    break;
                // ======= LIGHT MODEL =======
                case ParticleType.LightModel:
                    var lightModelData = (AVFXParticleDataLightModel)particle.Data;
                    if (!InitModel(lightModelData.ModelIdx.Value)) return;
                    break;
                // ======== QUAD ============
                case ParticleType.Quad:
                    InitQuad(0.5f);
                    break;
                // ======= POWDER ===========
                case ParticleType.Powder:
                    InitQuad(1.0f);
                    break;
                // ======= POLYLINE ===========
                case ParticleType.Polyline:
                    var polyLineData = (AVFXParticleDataPolyline)particle.Data;
                    InitPolyline(polyLineData, 1.0f);
                    break;
                default:
                    return;
            }
            DoDraw = true;
            InstanceNum = 20;
            Instances = new ParticleInstance[InstanceNum];
            Pipe = new ParticlePipeline(particle, this, core);
        }

        public void AddInstance(
            GenericInstance parent, Matrix4x4 startTransform, EmitterCreateStruct createData,
            bool powder = false, ParticleInstancePowderSpawner spawner = null
        ) {
            AddInstance(
                parent, startTransform, createData,
                1,
                powder:powder, spawner:spawner
           );
        }

        public void AddInstance(
            GenericInstance parent, Matrix4x4 startTransform, EmitterCreateStruct createData,
            int num,
            bool powder = false, ParticleInstancePowderSpawner spawner = null
        )
        {
            if (!DoDraw) return;
            // ====================
            int NumToAdd = num;
            for(int instanceIdx = 0; instanceIdx < Instances.Length; instanceIdx++)
            {
                if (NumToAdd == 0) { return; }
                // ==== RESET DEAD ONES ====
                if (Instances[instanceIdx] == null || Instances[instanceIdx].Dead)
                {
                    if (!powder)
                    {
                        Instances[instanceIdx] = new ParticleInstanceModel(Particle, this, parent, startTransform, createData);
                    }
                    else
                    {
                        // powder instance
                        Instances[instanceIdx] = new ParticleInstancePowderInstance(Particle, this, parent, startTransform, createData, spawner);
                    }
                    NumToAdd--;
                }
            }
        }

        public void Update(float dT)
        {
            if (!DoDraw) return;
            // update instances here
            // TODO: check if make powder particles
            for(int instanceIdx = 0; instanceIdx < Instances.Length; instanceIdx++)
            {
                if(Instances[instanceIdx] != null && Instances[instanceIdx].Dead != true)
                {
                    Instances[instanceIdx].UpdateTime(dT);
                }
            }
        }

        public void Draw()
        {
            if (!DoDraw) return;
            Pipe.Draw();
        }

        // ======== PARTICLE MODELS =========
        public bool InitModel(int modelIdx)
        {
            if (modelIdx == -1) return false;
            Verts = C.Models[modelIdx].Verts;
            Indexes = C.Models[modelIdx].Indexes;

            return true;
        }

        public void InitQuad(float size)
        {
            Verts = new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(-1.0f * size,-1.0f * size,0), new Vector4(1,0,1,0), new Vector4(1)),
                new VertexPositionTexture(new Vector3(-1.0f * size, 1.0f * size,0), new Vector4(1,1,1,1), new Vector4(1)),
                new VertexPositionTexture(new Vector3( 1.0f * size,-1.0f * size,0), new Vector4(0,0,0,0), new Vector4(1)),
                new VertexPositionTexture(new Vector3( 1.0f * size, 1.0f * size,0), new Vector4(0,1,0,1), new Vector4(1)),
            };
            Indexes = new ushort[] {
                2, 0, 1,
                3, 2, 1
            };
        }

        public void InitPolyline(AVFXParticleDataPolyline polyLineData, float size)
        {
            int points = polyLineData.PointCount.Value;
            int center = polyLineData.PointCountCenter.Value;


            float width = 1.0f / (float)points;
            /*    x --- 1 --- 3
             *    |  \  |  \  |
             *    x --- 0 --- 2
             */
            float XOffset = 0; //center * width;
            int flip = polyLineData.IsLocal.Value == true ? -1 : 1;

            Verts = new VertexPositionTexture[(points + 1) * 2];
            Indexes = new ushort[points * 6];
            for(int i = 0; i <= points; i++)
            {
                float x = (i * width - XOffset) * size;
                Verts[i * 2 ] = new VertexPositionTexture(
                        new Vector3(flip * x, 1.0f * size, 0),
                        new Vector4(0, 1 - i * width, 0, 1 - i * width),
                        new Vector4(1)
                );
                Verts[i * 2 + 1] = new VertexPositionTexture(
                        new Vector3(flip * x, -1.0f * size, 0),
                        new Vector4(1, 1 - i * width, 0, 1 - i * width),
                        new Vector4(1)
                );

                if(i != 0)
                {
                    int i0 = (i - 1) * 2;
                    int i1 = i0 + 1;
                    int i2 = i * 2;
                    int i3 = i2 + 1;

                    int indexRoot = (i - 1) * 6;
                    Indexes[indexRoot + 0] = (ushort)i2;
                    Indexes[indexRoot + 1] = (ushort)i0;
                    Indexes[indexRoot + 2] = (ushort)i1;
                    Indexes[indexRoot + 3] = (ushort)i3;
                    Indexes[indexRoot + 4] = (ushort)i2;
                    Indexes[indexRoot + 5] = (ushort)i1;
                }
            }
        }

        // ======== ARRAY =============
        public static ParticleItem[] GetArray(List<AVFXParticle> particles, Core core)
        {
            ParticleItem[] ret = new ParticleItem[particles.Count];
            for (int idx = 0; idx < particles.Count; idx++)
            {
                ret[idx] = new ParticleItem(particles[idx], core, idx);
            }
            return ret;
        }
    }
}
