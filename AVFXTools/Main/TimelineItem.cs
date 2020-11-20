using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class TimelineSubItem
    {
        public int BinderIdx;
        public int EmitterIdx;
        public TimelineSubItem(int binderIdx, int emitterIdx)
        {
            BinderIdx = binderIdx;
            EmitterIdx = emitterIdx;
        }
    }

    public class TimelineItem
    {
        public AVFXTimeline Timeline;
        public Core C;
        public List<TimelineSubItem> Items = new List<TimelineSubItem>();
        //===============

        public TimelineItem(AVFXTimeline timeline, Core core)
        {
            Timeline = timeline;
            C = core;
            //==============
            if(timeline.Items.Count > 0)
            {
                var lastItem = timeline.Items[timeline.Items.Count - 1];
                foreach(var subItem in lastItem.SubItems)
                {
                    var emitterIdx = subItem.EmitterIdx.Value;
                    if(emitterIdx != -1 && (subItem.Enabled.Value == true))
                    {
                        Items.Add(new TimelineSubItem(subItem.BinderIdx.Value, emitterIdx));
                    }
                }
            }
        }

        public void Start()
        {
            foreach(var subItem in Items)
            {
                BinderItem binder = null;
                Console.WriteLine("emitter {0} binder {1}", subItem.EmitterIdx, subItem.BinderIdx);
                if (subItem.BinderIdx != -1 && subItem.BinderIdx < C.Binders.Length)
                {
                    binder = C.Binders[subItem.BinderIdx];
                }

                C.AddEmitterInstance(subItem.EmitterIdx, binder, Matrix4x4.Identity, null);
            }
        }

        // ======== ARRAY =============
        public static TimelineItem[] GetArray(List<AVFXTimeline> timelines, Core core)
        {
            TimelineItem[] ret = new TimelineItem[timelines.Count];
            for (int idx = 0; idx < timelines.Count; idx++)
            {
                ret[idx] = new TimelineItem(timelines[idx], core);
            }
            return ret;
        }
    }
}
