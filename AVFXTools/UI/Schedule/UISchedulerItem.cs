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
    public class UISchedulerItem : UIBase
    {
        public AVFXScheduleSubItem Item;
        public int Idx;
        public string Name;
        // ====================

        public UISchedulerItem(AVFXScheduleSubItem item, string name)
        {
            Item = item;
            Name = name;
            // ============================
            Attributes.Add(new UICheckbox("Enabled", Item.Enabled));
            Attributes.Add(new UIInt("Start Time", Item.StartTime));
            Attributes.Add(new UIInt("Timeline Index", Item.TimelineIdx));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/" + Name + Idx;
            if (ImGui.TreeNode(Name + " " + Idx + id))
            {
                if(Name == "Item")
                {
                    if (UIUtils.RemoveButton("Delete" + id))
                    {
                        // TODO
                    }
                }
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
