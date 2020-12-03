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
    public class UIScheduler : UIBase
    {
        public AVFXSchedule Scheduler;
        public int Idx;
        // =================
        // TODO: item count
        // TODO: trigger count
        // =================
        public UISchedulerItem[] Items;
        // ================
        public UISchedulerItem[] Triggers;

        public UIScheduler(AVFXSchedule scheduler)
        {
            Scheduler = scheduler;
            // =====================
            if(Scheduler.Items.Count > 0)
            {
                var LastItem = Scheduler.Items[Scheduler.Items.Count - 1];
                Items = new UISchedulerItem[LastItem.SubItems.Count];
                for(int i = 0; i < Items.Length; i++)
                {
                    Items[i] = new UISchedulerItem(LastItem.SubItems[i], "Item");
                }
            }
            else { Items = new UISchedulerItem[0]; }
            // =====================
            if (Scheduler.Triggers.Count > 0)
            {
                var LastTrigger = Scheduler.Triggers[Scheduler.Triggers.Count - 1];
                Triggers = new UISchedulerItem[LastTrigger.SubItems.Count - Items.Length];
                for (int i = 0; i < Triggers.Length; i++)
                {
                    Triggers[i] = new UISchedulerItem(LastTrigger.SubItems[i + Items.Length], "Trigger");
                }
            }
            else { Triggers = new UISchedulerItem[0]; }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Scheduler" + Idx;
            if (ImGui.CollapsingHeader("Scheduler " + Idx + id))
            {
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
                    if (ImGui.Button("+ Item" + id))
                    {
                        // TODO
                    }
                    ImGui.TreePop();
                }
                //=====================
                if (ImGui.TreeNode("Triggers (" + Triggers.Length + ")" + id))
                {
                    int tIdx = 0;
                    foreach (var trigger in Triggers)
                    {
                        trigger.Idx = tIdx;
                        trigger.Draw(id);
                        tIdx++;
                    }
                    ImGui.TreePop();
                }
            }
        }
    }
}
