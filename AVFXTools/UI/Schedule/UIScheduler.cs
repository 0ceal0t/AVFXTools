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
        public UIScheduler(AVFXSchedule scheduler)
        {
            Scheduler = scheduler;
            // =====================
        }

        public override void Draw(string parentId)
        {
        }
    }
}
