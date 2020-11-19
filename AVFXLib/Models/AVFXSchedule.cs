using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXSchedule : Base
    {
        public const string NAME = "Schd";

        public LiteralInt ItemCount = new LiteralInt("itemCount", "ItCn");
        public LiteralInt TriggerCount = new LiteralInt("triggerCount", "TrCn");
        public List<AVFXScheduleItem> Items = new List<AVFXScheduleItem>();
        public List<AVFXScheduleTrigger> Triggers = new List<AVFXScheduleTrigger>();

        List<Base> Attributes;

        public AVFXSchedule() : base("schedules", NAME)
        {
            Attributes = new List<Base>(new Base[] {
                ItemCount,
                TriggerCount
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
                    case AVFXScheduleItem.NAME:
                        AVFXScheduleItem Item = new AVFXScheduleItem();
                        Item.read(item);
                        Items.Add(Item);
                        break;
                    // TRIGGERS =================================
                    case AVFXScheduleTrigger.NAME:
                        AVFXScheduleTrigger Trigger = new AVFXScheduleTrigger();
                        Trigger.read(item);
                        Triggers.Add(Trigger);
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
                AVFXScheduleItem Item = new AVFXScheduleItem();
                Item.read((JObject)i);
                Items.Add(Item);
            }

            // TRIGGERS
            //=======================//
            JArray triggerElems = (JArray)elem.GetValue("triggers");
            foreach (JToken t in triggerElems)
            {
                AVFXScheduleTrigger Trigger = new AVFXScheduleTrigger();
                Trigger.read((JObject)t);
                Triggers.Add(Trigger);
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);

            JArray itemArray = new JArray();
            foreach (AVFXScheduleItem item in Items)
            {
                itemArray.Add(item.toJSON());
            }
            elem["items"] = itemArray;

            JArray triggerArray = new JArray();
            foreach (AVFXScheduleTrigger trigger in Triggers)
            {
                triggerArray.Add(trigger.toJSON());
            }
            elem["triggers"] = triggerArray;

            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode schdAvfx = new AVFXNode("Schd");

            PutAVFX(schdAvfx, Attributes);

            // Items
            //=======================//
            foreach (AVFXScheduleItem itemElem in Items)
            {
                PutAVFX(schdAvfx, itemElem);
            }

            // Triggers
            //=======================//
            /*foreach (AVFXScheduleTrigger triggerElem in Triggers)
            {
                PutAVFX(schdAvfx, triggerElem);
            }*/
            if (Triggers.Count > 0)
            {
                var lastTrigger = Triggers[Triggers.Count - 1];
                var subItemCount = lastTrigger.SubItems.Count;
                for (int i = 1; i <= subItemCount; i++)
                {
                    schdAvfx.Children.Add(lastTrigger.toAVFXRange(i));
                }
            }

            return schdAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- SCHD --------", new String('\t', level));
            Output(Attributes, level);

            // Items
            //=======================//
            foreach (AVFXScheduleItem itemElem in Items)
            {
                Output(itemElem, level);
            }

            // Triggers
            //=======================//
            foreach (AVFXScheduleTrigger triggerElem in Triggers)
            {
                Output(triggerElem, level);
            }
        }
    }
}
