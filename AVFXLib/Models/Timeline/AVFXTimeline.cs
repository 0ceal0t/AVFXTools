using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXTimeline : Base
    {
        public const string NAME = "TmLn";

        public LiteralInt LoopStart = new LiteralInt("loopStart", "LpSt");
        public LiteralInt LoopEnd = new LiteralInt("loopEnd", "LpEd");
        public LiteralInt BinderIdx = new LiteralInt("binderIdx", "BnNo");
        public LiteralInt TimelineCount = new LiteralInt("timelineCount", "TICn");
        public LiteralInt ClipCount = new LiteralInt("clipCount", "CpCn");

        List<Base> Attributes;

        public List<AVFXTimelineItem> Items = new List<AVFXTimelineItem>();
        public List<AVFXTimelineClip> Clips = new List<AVFXTimelineClip>();

        public AVFXTimeline() : base("timelines", NAME)
        {
            Attributes = new List<Base>(new Base[] {
                LoopStart,
                LoopEnd,
                BinderIdx,
                TimelineCount,
                ClipCount
            });
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);

            foreach (AVFXNode item in node.Children)
            {
                switch (item.Name)
                {
                    // ITEMS =================================
                    case AVFXTimelineItem.NAME:
                        AVFXTimelineItem Item = new AVFXTimelineItem();
                        Item.read(item);
                        Items.Add(Item);
                        break;
                    // CLIPS =================================
                    case AVFXTimelineClip.NAME:
                        AVFXTimelineClip Clip = new AVFXTimelineClip();
                        Clip.read(item);
                        Clips.Add(Clip);
                        break;
                }
            }
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);

            // ITEMS
            //=======================//
            JArray itemElems = (JArray)elem.GetValue("items");
            foreach (JToken i in itemElems)
            {
                AVFXTimelineItem Item = new AVFXTimelineItem();
                Item.read((JObject)i);
                Items.Add(Item);
            }

            // CLIPS
            //=======================//
            JArray clipElems = (JArray)elem.GetValue("clips");
            foreach (JToken c in clipElems)
            {
                AVFXTimelineClip Clip = new AVFXTimelineClip();
                Clip.read((JObject)c);
                Clips.Add(Clip);
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);

            JArray itemArray = new JArray();
            foreach(AVFXTimelineItem item in Items)
            {
                itemArray.Add(item.toJSON());
            }
            elem["items"] = itemArray;

            JArray clipArray = new JArray();
            foreach (AVFXTimelineClip clip in Clips)
            {
                clipArray.Add(clip.toJSON());
            }
            elem["clips"] = clipArray;

            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode tmlnAvfx = new AVFXNode("TmLn");

            PutAVFX(tmlnAvfx, Attributes);

            // Items
            //=======================//
            /*foreach (AVFXTimelineItem itemElem in Items)
            {
                PutAVFX(tmlnAvfx, itemElem);
            }*/
            if (Items.Count > 0)
            {
                var lastItem = Items[Items.Count - 1];
                var subItemCount = lastItem.SubItems.Count;
                for (int i = 1; i <= subItemCount; i++)
                {
                    tmlnAvfx.Children.Add(lastItem.toAVFXRange(i));
                }
            }

            // Clips
            //=======================//
            foreach (AVFXTimelineClip clipElem in Clips)
            {
                PutAVFX(tmlnAvfx, clipElem);
            }

            return tmlnAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- TMLN --------", new String('\t', level));
            Output(Attributes, level);

            // Items
            //=======================//
            foreach (AVFXTimelineItem itemElem in Items)
            {
                Output(itemElem, level);
            }

            // Clips
            //=======================//
            foreach (AVFXTimelineClip clipElem in Clips)
            {
                Output(clipElem, level);
            }
        }
    }
}
