using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITimeline
    {
        public AVFXTimeline Timeline;
        //=====================
        public int LoopStart;
        public int LoopEnd;
        public int BinderIdx;
        // TODO: timeline cound
        // TODO clip count
        //=====================
        public UITimelineItem[] Items;
        //=====================
        public UITimelineClip[] Clips;

        public UITimeline(AVFXTimeline timeline)
        {
            Timeline = timeline;
            //========================
            LoopStart = Timeline.LoopStart.Value;
            LoopEnd = Timeline.LoopEnd.Value;
            BinderIdx = Timeline.BinderIdx.Value;
            //========================
            if(Timeline.Items.Count > 0)
            {
                var LastItem = Timeline.Items[Timeline.Items.Count - 1];
                Items = new UITimelineItem[LastItem.SubItems.Count];
                for(int i = 0; i < Items.Length; i++)
                {
                    Items[i] = new UITimelineItem(LastItem.SubItems[i]);
                }
            }
            else
            {
                Items = new UITimelineItem[0];
            }
            //==========================
            Clips = new UITimelineClip[Timeline.Clips.Count];
            for (int i = 0; i < Timeline.Clips.Count; i++)
            {
                Clips[i] = new UITimelineClip(Timeline.Clips[i]);
            }
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Parameters" + id))
            {
                if (ImGui.InputInt("Loop Start" + id, ref LoopStart))
                {
                    Timeline.LoopStart.GiveValue(LoopStart);
                }
                if (ImGui.InputInt("Loop End" + id, ref LoopEnd))
                {
                    Timeline.LoopEnd.GiveValue(LoopEnd);
                }
                if (ImGui.InputInt("Binder Index" + id, ref BinderIdx))
                {
                    Timeline.BinderIdx.GiveValue(BinderIdx);
                }
                ImGui.TreePop();
            }
            //==========================
            if (ImGui.TreeNode("Items (" + Items.Length + ")" + id))
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    Items[i].Draw(id + "-item" + i, i);
                }
                ImGui.TreePop();
            }
            //==========================
            if (ImGui.TreeNode("Clips (" + Clips.Length + ")" + id))
            {
                for (int i = 0; i < Clips.Length; i++)
                {
                    Clips[i].Draw(id + "-clip" + i, i);
                }
                ImGui.TreePop();
            }
        }
    }
}
