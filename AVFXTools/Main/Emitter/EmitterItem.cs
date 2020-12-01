using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class EmitterItem
    {
        public AVFXEmitter Emitter;
        public Core C;
        // ==================
        public float Life;
        public int InstanceNum = 0;
        public EmitterInstance[] Instances;
        public EmitterType Type;

        public EmitterItem(AVFXEmitter emitter, Core core)
        {
            Emitter = emitter;
            C = core;
            // ==================
            // TODO: set up cylinder / sphere
            Life = emitter.Life.Value.Value;
            Type = emitter.EmitterVariety.Value;
            InstanceNum = 5;
            Instances = new EmitterInstance[InstanceNum];
        }

        public void AddInstance(GenericInstance parent, Matrix4x4 startTransform, EmitterCreateStruct createData) { AddInstance(parent, startTransform, createData, 1); }
        public void AddInstance(GenericInstance parent, Matrix4x4 startTransform, EmitterCreateStruct createData, int num)
        {
            int NumToAdd = num;
            for (int instanceIdx = 0; instanceIdx < Instances.Length; instanceIdx++)
            {
                if (NumToAdd == 0) { return; }
                // ==== RESET DEAD ONES ====
                if (Instances[instanceIdx] == null || Instances[instanceIdx].Dead)
                {
                    Instances[instanceIdx] = new EmitterInstance(Emitter, this, parent, startTransform, createData);
                    NumToAdd--;
                }
            }
        }

        public void Update(float dT)
        {
            // update instances here
            for (int instanceIdx = 0; instanceIdx < Instances.Length; instanceIdx++)
            {
                if (Instances[instanceIdx] != null && Instances[instanceIdx].Dead != true)
                {
                    Instances[instanceIdx].UpdateTime(dT);
                }
            }
        }

        // ======== ARRAY =============
        public static EmitterItem[] GetArray(List<AVFXEmitter> emitters, Core core)
        {
            EmitterItem[] ret = new EmitterItem[emitters.Count];
            for (int idx = 0; idx < emitters.Count; idx++)
            {
                ret[idx] = new EmitterItem(emitters[idx], core);
            }
            return ret;
        }
    }
}
