using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class ParticleInstancePowderSpawner
    {
        public AVFXParticle Particle;
        public AVFXParticleSimple Simple;
        public ParticleInstanceModel Instance;
        // ========================
        public int CreateCount;
        public float CreateInterval;
        public float CreateLife;
        public int CreatePerInterval;

        public int NumCreated = 0;
        public float TimeUntilCreate = 0.0f;

        public ModelItem Model;

        public ParticleInstancePowderSpawner(AVFXParticle particle, ParticleInstanceModel instance)
        {
            Particle = particle;
            Simple = particle.Simple;
            Instance = instance;
            // ===================
            CreateCount = Simple.CreateCount.Value;
            CreateInterval = Simple.CreateIntervalVal.Value;
            CreateLife = Simple.CreateIntervalLife.Value;
            CreatePerInterval = Simple.CreateIntervalCount.Value;
            if(Simple.InjectionModelIdx.Value != -1)
            {
                Model = Instance.Item.C.Models[Simple.InjectionModelIdx.Value];
            }
        }

        public void UpdateTime(float dT)
        {
            if (Instance.Dead) return;
            TimeUntilCreate -= dT;
            if(TimeUntilCreate < 0)
            {
                TimeUntilCreate = CreateInterval;
                for(int idx = 0; idx < CreatePerInterval; idx++)
                {
                    if (NumCreated == CreateCount) return;
                    Vector3 StartingPoint = new Vector3();
                    if(Model != null)
                    {
                        var emitV = Model.VEmits[NumCreated % Model.VEmits.Length];
                        StartingPoint = emitV.Pos;
                    }
                    ParticleInstanceData Data = Instance.GetData();
                    Matrix4x4 startTransform = Matrix4x4.CreateTranslation(Vector3.Transform(StartingPoint, Data.TransformMatrix));

                    EmitterCreateStruct createData = new EmitterCreateStruct(-1, false, false, false, false, 1); // TODO: update this
                    Instance.Item.AddInstance(Instance, startTransform, createData, true, this);

                    NumCreated++;
                }
            }
        }
    }
}
