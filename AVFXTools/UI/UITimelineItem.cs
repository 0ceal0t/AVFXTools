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
    public class UITimelineItem
    {
        public AVFXTimelineSubItem Item;
        //===========================
        public bool Enabled;
        public int StartTime;
        public int EndTime;
        public int BinderIdx;
        public int EffectorIdx;
        public int EmitterIdx;
        public int Platform;
        public int ClipNumber;

        public UITimelineItem(AVFXTimelineSubItem item)
        {
            Item = item;
            //==================
            Enabled = (Item.Enabled.Value == true);
            StartTime = Item.StartTime.Value;
            EndTime = Item.EndTime.Value;
            BinderIdx = Item.BinderIdx.Value;
            EffectorIdx = Item.EffectorIdx.Value;
            EmitterIdx = Item.EmitterIdx.Value;
            Platform = Item.Platform.Value;
            ClipNumber = Item.ClipNumber.Value;
        }

        public void Draw(string id, int idx)
        {
            if (ImGui.TreeNode("Item#" + idx + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Item.Enabled.GiveValue(Enabled);
                }
                if (ImGui.InputInt("Start Time" + id, ref StartTime))
                {
                    Item.StartTime.GiveValue(StartTime);
                }
                if (ImGui.InputInt("End Time" + id, ref StartTime))
                {
                    Item.StartTime.GiveValue(StartTime);
                }
                if (ImGui.InputInt("Binder Index" + id, ref BinderIdx))
                {
                    Item.BinderIdx.GiveValue(BinderIdx);
                }
                if (ImGui.InputInt("Effector Index" + id, ref EffectorIdx))
                {
                    Item.EffectorIdx.GiveValue(EffectorIdx);
                }
                if (ImGui.InputInt("Emitter Index" + id, ref EmitterIdx))
                {
                    Item.EmitterIdx.GiveValue(EmitterIdx);
                }
                if (ImGui.InputInt("Platform" + id, ref Platform))
                {
                    Item.Platform.GiveValue(Platform);
                }
                if (ImGui.InputInt("Clip Number" + id, ref ClipNumber))
                {
                    Item.ClipNumber.GiveValue(ClipNumber);
                }
                ImGui.TreePop();
            }
        }
    }
}
