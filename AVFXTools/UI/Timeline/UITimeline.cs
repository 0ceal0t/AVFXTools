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
    public class UITimeline : UIBase
    {
        public AVFXTimeline Timeline;
        public int Idx;
        //=====================
        // TODO: timeline count
        // TODO clip count
        //=====================
        public UITimelineItem[] Items;
        //=====================
        public UITimelineClip[] Clips;

        public UITimeline(AVFXTimeline timeline)
        {
            Timeline = timeline;
            //========================
            Attributes.Add(new UIInt("Loop Start", Timeline.LoopStart));
            Attributes.Add(new UIInt("Loop End", Timeline.LoopEnd));
            Attributes.Add(new UIInt("Binder Index", Timeline.BinderIdx));
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
            else { Items = new UITimelineItem[0]; }
            //==========================
            Clips = new UITimelineClip[Timeline.Clips.Count];
            for (int i = 0; i < Timeline.Clips.Count; i++)
            {
                Clips[i] = new UITimelineClip(Timeline.Clips[i]);
            }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Timeline" + Idx;
            if (ImGui.CollapsingHeader("Timeline " + Idx + id))
            {
                if (ImGui.TreeNode("Parameters" + id))
                {
                    DrawAttrs(id);
                    ImGui.TreePop();
                }
                //=====================
                if (ImGui.TreeNode("Items (" + Items.Length + ")" + id))
                {
                    int iIdx = 0;
                    foreach (var item in Items)
                    {
                        item.Idx = iIdx;
                        item.Draw(id);
                        iIdx++;
                    }
                    ImGui.TreePop();
                }
                //=====================
                if (ImGui.TreeNode("Clips (" + Clips.Length + ")" + id))
                {
                    int cIdx = 0;
                    foreach (var clip in Clips)
                    {
                        clip.Idx = cIdx;
                        clip.Draw(id);
                        cIdx++;
                    }
                    ImGui.TreePop();
                }
            }
        }
    }
}
