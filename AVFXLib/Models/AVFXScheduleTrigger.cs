using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXScheduleTrigger : Base
    {
        public const string NAME = "Trgr";

        public List<AVFXScheduleSubItem> SubItems = new List<AVFXScheduleSubItem>();

        public AVFXScheduleTrigger() : base("trigger", NAME)
        {
        }

        public override void read(JObject elem)
        {
            Assigned = true;

            JArray itemElems = (JArray)elem.GetValue("parts");
            foreach (JToken i in itemElems)
            {
                AVFXScheduleSubItem Item = new AVFXScheduleSubItem();
                Item.read((JObject)i);
                SubItems.Add(Item);
            }
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;

            // split every 3 leafs, make dummy elements and insert
            int numItems = (int)Math.Floor((double)node.Children.Count / 3);
            for (int idx = 0; idx < numItems; idx++)
            {
                List<AVFXNode> subItem = node.Children.GetRange(idx * 3, 3);
                AVFXNode dummyNode = new AVFXNode("SubItem");
                dummyNode.Children = subItem;

                AVFXScheduleSubItem Item = new AVFXScheduleSubItem();
                Item.read(dummyNode);
                SubItems.Add(Item);
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();

            JArray partArray = new JArray();
            foreach (AVFXScheduleSubItem subItem in SubItems)
            {
                partArray.Add(subItem.toJSON());
            }
            elem["parts"] = partArray;

            return elem;
        }

        public override AVFXNode toAVFX()
        {
            // make ItPr by concatting elements of dummy elements
            AVFXNode itemAvfx = new AVFXNode("Trgr");
            foreach (AVFXScheduleSubItem Item in SubItems)
            {
                itemAvfx.Children.AddRange(Item.toAVFX().Children); // flatten
            }

            return itemAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- Item --------", new String('\t', level));
            foreach (AVFXScheduleSubItem Item in SubItems)
            {
                Output(Item, level);
            }
        }
    }

    public class AVFXScheduleSubItem : Base
    {
        LiteralBool Enabled = new LiteralBool("enabled", "bEna");
        LiteralInt StartTime = new LiteralInt("startTime", "StTm");
        LiteralInt TimelineIdx = new LiteralInt("timelineIdx", "TlNo");

        List<Base> Attributes;

        public AVFXScheduleSubItem() : base("subItem", "SubItem")
        {
            Attributes = new List<Base>(new Base[]{
                Enabled,
                StartTime,
                TimelineIdx
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode dataAvfx = new AVFXNode("SubItem");
            PutAVFX(dataAvfx, Attributes);
            return dataAvfx;
        }

        public override void Print(int level)
        {
            Output(Attributes, level);
        }
    }
}
