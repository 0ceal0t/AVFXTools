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
            Type = (ParticleType)Enum.Parse(typeof(ParticleType), particle.ParticleType.Value, true);
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
                    InitQuad(0.5f, 1.0f, 2.0f);
                    break;
                // ======= POWDER ===========
                case ParticleType.Powder:
                    InitQuad(1.0f, 1.0f, 1.0f);
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

        public void InitQuad(float size, float uvX, float uvY)
        {
            Verts = new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(-1.0f * size,-1.0f * size,0), new Vector2(0.0f,uvY)),
                new VertexPositionTexture(new Vector3(-1.0f * size, 1.0f * size,0), new Vector2(0.0f,0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f * size,-1.0f * size,0), new Vector2(uvX,uvY)),
                new VertexPositionTexture(new Vector3( 1.0f * size, 1.0f * size,0), new Vector2(uvX,0.0f)),
            };
            Indexes = new ushort[] {
                2, 0, 1,
                3, 2, 1
            };
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
