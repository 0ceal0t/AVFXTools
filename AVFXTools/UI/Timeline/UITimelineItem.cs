﻿using AVFXLib.Models;
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
        public UITimeline Timeline;
        public int Idx;
        //===========================

        public UITimelineItem(AVFXTimelineSubItem item, UITimeline timeline)
        {
            Item = item;
            Timeline = timeline;
            Init();
        }
        public override void Init()
        {
            base.Init();
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
                if (UIUtils.RemoveButton("Delete" + id))
                {
                    Timeline.Timeline.removeItem(Idx);
                    Timeline.Init();
                }
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
