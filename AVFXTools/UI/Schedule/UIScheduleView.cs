using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UIScheduleView : UIBase
    {
        List<UIScheduler> Schedulers = new List<UIScheduler>();

        public UIScheduleView(AVFXBase avfx)
        {
            foreach (var sched in avfx.Schedulers)
            {
                Schedulers.Add(new UIScheduler(sched));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##SCHED";
            int sIdx = 0;
            foreach (var sched in Schedulers)
            {
                sched.Idx = sIdx;
                sched.Draw(id);
                sIdx++;
            }
        }
    }
}
