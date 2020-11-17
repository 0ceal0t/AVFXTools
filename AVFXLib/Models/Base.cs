using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;

namespace AVFXLib.Models
{
    public abstract class Base
    {
        public bool Assigned { get; set; } = false;
        public string JSONPath { get; set; }
        public string AVFXName { get; set; }

        // temp
        //public List<LiteralBase> Attributes { get; set; }

        public Base() // temp
        {
            JSONPath = "";
            AVFXName = "";
        }

        public Base(string jsonPath, string avfxName)
        {
            JSONPath = jsonPath;
            AVFXName = avfxName;
        }

        public abstract AVFXNode toAVFX();
        public abstract void Print(int level);

        // public abstract JToken toJSON();
        public virtual JToken toJSON() // TEMP
        {
            return new JValue("");
        }

        public abstract void read(JObject elem);
        public abstract void read(AVFXNode node);
        // TEMP
        //public void read(JObject elem) { }
        //public void read(AVFXNode node) { }

        // STATIC ===========================
        public static void ReadJSON(List<Base> attributes, JObject elem)
        {
            if (attributes == null || elem == null) return;
            foreach (Base attribute in attributes)
            {
                ReadJSON(attribute, elem);
            }
        }
        public static void ReadJSON(Base attribute, JObject elem)
        {
            if (attribute == null || elem == null) return;
            foreach (var item in elem)
            {
                if (item.Key == attribute.JSONPath)
                {
                    if(attribute is LiteralBase)
                    {
                        LiteralBase literal = (LiteralBase)attribute;
                        literal.read((JValue)item.Value);
                    }
                    else
                    {
                        attribute.read((JObject)item.Value);
                    }
                    break;
                }
            }
        }

        public static void ReadAVFX(List<Base> attributes, AVFXNode node)
        {
            if (attributes == null || node == null) return;
            foreach (Base attribute in attributes)
            {
                ReadAVFX(attribute, node);
            }
        }
        public static void ReadAVFX(Base attribute, AVFXNode node)
        {
            if (attribute == null || node == null) return;
            foreach (AVFXNode item in node.Children)
            {
                if(item.Name == attribute.AVFXName)
                {
                    if (attribute is LiteralBase)
                    {
                        LiteralBase literal = (LiteralBase)attribute;
                        literal.read((AVFXLeaf)item);
                    }
                    else
                    {
                        attribute.read(item);
                    }
                    break;
                }
            }
        }

        public static void PutJSON(JObject obj, List<Base> sources)
        {
            if (obj == null || sources == null) return;
            foreach (Base b in sources)
            {
                PutJSON(obj, b);
            }
        }
        public static void PutJSON(JObject obj, Base source)
        {
            if (obj == null || source == null) return;
            if (!source.Assigned) return;
            obj[source.JSONPath] = source.toJSON();
        }

        public static void PutAVFX(AVFXNode destination, List<Base> sources)
        {
            if (destination == null || sources == null) return;
            foreach (Base b in sources)
            {
                PutAVFX(destination, b);
            }
        }
        public static void PutAVFX(AVFXNode destination, Base source)
        {
            if (destination == null || source == null) return;
            destination.Children.Add(GetAVFX(source));
        }
        public static AVFXNode GetAVFX(Base b)
        {
            if (b == null) return null;
            if (b.Assigned)
            {
                return b.toAVFX();
            }
            return new AVFXBlank();
        }

        public static void Output(List<Base> source, int level)
        {
            if (source == null) return;
            foreach (Base b in source)
            {
                Output(b, level);
            }
        }

        public static void Output(Base b, int level)
        {
            if (b == null) return;
            if (b.Assigned)
            {
                if (b is LiteralBase){
                    b.Print(level);
                }
                else
                {
                    b.Print(level + 1);
                }
            }
        }


        // TEMP

        public static void Print(Base b, int level)
        {
            if (b.Assigned)
            {
                b.Print(level);
            }
        }
    }
}
