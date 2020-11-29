using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITimelineView : UIBase
    {
        List<UITimeline> Timelines = new List<UITimeline>();

        public UITimelineView(AVFXBase avfx)
        {
            foreach (var timeline in avfx.Timelines)
            {
                Timelines.Add(new UITimeline(timeline));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##TIME";
            int tIdx = 0;
            foreach (var timeline in Timelines)
            {
                timeline.Idx = tIdx;
                timeline.Draw(id);
                tIdx++;
            }
        }
    }
}
