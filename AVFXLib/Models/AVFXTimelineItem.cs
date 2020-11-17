using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXTimelineItem : Base
    {
        public const string NAME = "Item";

        List<Base> Attributes;

        public List<AVFXTimelineSubItem> SubItems = new List<AVFXTimelineSubItem>();

        public AVFXTimelineItem() : base("item",NAME)
        {
            Attributes = new List<Base>(new Base[]{
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);

            JArray itemElems = (JArray)elem.GetValue("items");
            foreach (JToken i in itemElems)
            {
                AVFXTimelineSubItem Item = new AVFXTimelineSubItem();
                Item.read((JObject)i);
                SubItems.Add(Item);
            }
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);

            // split on every bEna
            List<AVFXNode> split = new List<AVFXNode>();
            int idx = 0;
            foreach (AVFXNode child in node.Children)
            {
                split.Add(child);

                if (idx == (node.Children.Count - 1) || node.Children[idx + 1].Name == "bEna") // split before bEna
                {
                    AVFXNode dummyNode = new AVFXNode("SubItem");
                    dummyNode.Children = split;
                    AVFXTimelineSubItem Item = new AVFXTimelineSubItem();
                    Item.read(dummyNode);
                    SubItems.Add(Item);
                    split = new List<AVFXNode>();
                }

                idx++;
            }
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            JArray itemArray = new JArray();
            foreach (AVFXTimelineSubItem item in SubItems)
            {
                itemArray.Add(item.toJSON());
            }
            elem["items"] = itemArray;
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            // make ItPr by concatting elements of dummy elements
            AVFXNode itemAvfx = new AVFXNode("Item");
            foreach (AVFXTimelineSubItem Item in SubItems)
            {
                itemAvfx.Children.AddRange(Item.toAVFX().Children); // flatten
            }

            PutAVFX(itemAvfx, Attributes);
            return itemAvfx;
        }

        public override void Print(int level)
        {
            Output(Attributes, level);

            Console.WriteLine("{0}------- Item --------", new String('\t', level));
            foreach (AVFXTimelineSubItem Item in SubItems)
            {
                Output(Item, level);
            }
        }
    }

    public class AVFXTimelineSubItem : Base
    {
        public LiteralBool Enabled = new LiteralBool("enabled", "bEna");
        public LiteralInt StartTime = new LiteralInt("startTime", "StTm");
        public LiteralInt EndTime = new LiteralInt("endTime", "EdTm");
        public LiteralInt BinderIdx = new LiteralInt("binderIdx", "BdNo");
        public LiteralInt EffectorIdx = new LiteralInt("effectorIdx", "EfNo");
        public LiteralInt EmitterIdx = new LiteralInt("emitterIdx", "EmNo");
        public LiteralInt Platform = new LiteralInt("platform", "Plfm");
        public LiteralInt ClipNumber = new LiteralInt("clipIdx", "ClNo");

        List<Base> Attributes;

        public AVFXTimelineSubItem() : base("subItem", "SubItem")
        {
            Attributes = new List<Base>(new Base[]{
                Enabled,
                StartTime,
                EndTime,
                BinderIdx,
                EffectorIdx,
                EmitterIdx,
                Platform,
                ClipNumber
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
