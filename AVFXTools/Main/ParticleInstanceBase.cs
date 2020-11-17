using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public abstract class ParticleInstanceBase
    {
        public float Age;
        public float Life;
        public bool Dead = false;
        public bool IsSpawner = false;
        public ParticleInstancePowderSpawner Spawner;
        public ParticleItem Item;
        public Matrix4x4 PrevTransform;

        public abstract void UpdateTime(float dT);
        public abstract void Reset();
        public abstract ParticleInstanceData GetData();
        public abstract ParticleInstanceData GetData(float time);
    }
}
