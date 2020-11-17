using AVFXLib.AVFX;
using AVFXLib.Main;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXModel : Base
    {
        public const string NAME = "Modl";

        public List<VNum> VNums = new List<VNum>();
        public List<Vertex> Vertices = new List<Vertex>();
        public List<Index> Indexes = new List<Index>();
        public List<EmitVertex> EmitVertices = new List<EmitVertex>();

        public AVFXModel() : base("model", NAME)
        {
        }

        public override void read(JObject elem)
        {
            Assigned = true;

            // VNum ===============
            if (elem.ContainsKey("num"))
            {
                JArray vnumElems = (JArray)elem.GetValue("num");
                foreach (JToken n in vnumElems)
                {
                    VNums.Add(new VNum((int)n));
                }
            }

            // VDRW ===============
            if (elem.ContainsKey("vertices"))
            {
                JArray vdrwElems = (JArray)elem.GetValue("vertices");
                foreach(JToken v in vdrwElems)
                {
                    Vertices.Add(new Vertex((JObject)v));
                }
            }

            // VEmt ===============
            if (elem.ContainsKey("emit"))
            {
                JArray vemtElems = (JArray)elem.GetValue("emit");
                foreach (JToken e in vemtElems)
                {
                    EmitVertices.Add(new EmitVertex((JObject)e));
                }
            }

            // VIDX ===============
            if (elem.ContainsKey("indexes"))
            {
                JArray vidxElems = (JArray)elem.GetValue("indexes");
                foreach (JToken v in vidxElems)
                {
                    Indexes.Add(new Index((JArray)v));
                }
            }
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            foreach(AVFXNode n in node.Children)
            {
                AVFXLeaf leaf;
                switch (n.Name)
                {
                    case "VNum": // VNum ===========
                        // 
                        leaf = (AVFXLeaf)n;
                        foreach (byte[] bytes in Util.SplitBytes(leaf.Contents, VNum.SIZE))
                        {
                            VNums.Add(new VNum(bytes));
                        }
                        break;

                    case "VDrw": // VDRW ===========
                        // 
                        AVFXLeaf vleaf = (AVFXLeaf)n;
                        foreach (byte[] bytes in Util.SplitBytes(vleaf.Contents, Vertex.SIZE))
                        {
                            Vertices.Add(new Vertex(bytes));
                        }
                        break;

                    case "VEmt": // VEMT ===========
                        // 
                        leaf = (AVFXLeaf)n;
                        foreach (byte[] bytes in Util.SplitBytes(leaf.Contents, EmitVertex.SIZE))
                        {
                            EmitVertices.Add(new EmitVertex(bytes));
                        }
                        break;

                    case "VIdx": // VIDX ===========
                        // 
                        leaf = (AVFXLeaf)n;
                        foreach(byte[] bytes in Util.SplitBytes(leaf.Contents, Index.SIZE))
                        {
                            Indexes.Add(new Index(bytes));
                        }
                        break;
                }
            }
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- PTCL --------", new String('\t', level));
            Console.WriteLine("{0} VDrw {1}, Idx {2}, Emit {3}, VNum {4}", new String('\t', level), Vertices.Count.ToString(), Indexes.Count.ToString(), EmitVertices.Count.ToString(), VNums.Count.ToString());
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode modelNode =  new AVFXNode("Modl");

            // VNUM ==================
            if (VNums.Count > 0)
            {
                byte[][] vnumBytes = new byte[VNums.Count][];
                int nIdx = 0;
                foreach (VNum n in VNums)
                {
                    vnumBytes[nIdx] = n.toBytes();
                    nIdx++;
                }
                byte[] nBytes = Util.JoinBytes(vnumBytes, VNum.SIZE);
                AVFXLeaf vnum = new AVFXLeaf("VNum", nBytes.Length, nBytes);
                modelNode.Children.Add(vnum);
            }

            // VEMT ==================
            if (EmitVertices.Count > 0)
            {
                byte[][] emtBytes = new byte[EmitVertices.Count][];
                int eIdx = 0;
                foreach (EmitVertex e in EmitVertices)
                {
                    emtBytes[eIdx] = e.toBytes();
                    eIdx++;
                }
                byte[] eBytes = Util.JoinBytes(emtBytes, EmitVertex.SIZE);
                AVFXLeaf vemt = new AVFXLeaf("VEmt", eBytes.Length, eBytes);
                modelNode.Children.Add(vemt);
            }

            // VDRW ==================
            if (Vertices.Count > 0)
            {
                byte[][] vertBytes = new byte[Vertices.Count][];
                int vIdx = 0;
                foreach (Vertex v in Vertices)
                {
                    vertBytes[vIdx] = v.toBytes();
                    vIdx++;
                }
                byte[] vBytes = Util.JoinBytes(vertBytes, Vertex.SIZE);
                AVFXLeaf vdrw = new AVFXLeaf("VDrw", vBytes.Length, vBytes);
                modelNode.Children.Add(vdrw);
            }

            // VIDX ==================
            if (Indexes.Count > 0)
            {
                byte[][] indexBytes = new byte[Indexes.Count][];
                int iIdx = 0;
                foreach (Index i in Indexes)
                {
                    indexBytes[iIdx] = i.toBytes();
                    iIdx++;
                }
                byte[] iBytes = Util.JoinBytes(indexBytes, Index.SIZE);
                AVFXLeaf vidx = new AVFXLeaf("VIdx", iBytes.Length, iBytes);
                modelNode.Children.Add(vidx);
            }


            return modelNode;
        }
    }
}
