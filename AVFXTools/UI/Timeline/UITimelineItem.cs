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
    public class UITimelineItem : UIBase
    {
        public AVFXTimelineSubItem Item;
        public int Idx;
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
            Attributes.Add(new UICheckbox("Enabled", Item.Enabled));
            Attributes.Add(new UIInt("Start Time", Item.StartTime));
            Attributes.Add(new UIInt("End Time", Item.EndTime));
            Attributes.Add(new UIInt("Binder Index", Item.BinderIdx));
            Attributes.Add(new UIInt("Effector Index", Item.EffectorIdx));
            Attributes.Add(new UIInt("Emitter Index", Item.EmitterIdx));
            Attributes.Add(new UIInt("Platform", Item.Platform));
            Attributes.Add(new UIInt("ClipNumber", Item.ClipNumber));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TLItem" + Idx;
            if (ImGui.TreeNode("Item " + Idx + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
