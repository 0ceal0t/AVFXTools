using AVFXLib.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AVFXLib.AVFX
{
    public class AVFXNode
    {
        public string Name { get; set; }
        // calculate size on the fly

        public List<AVFXNode> Children { get; set; }

        public AVFXNode(string n)
        {
            Name = n;
            Children = new List<AVFXNode>();
        }

        public virtual byte[] toBytes()
        {
            int totalSize = 0;
            byte[][] byteArrays = new byte[Children.Count][];
            for(int i = 0; i < Children.Count; i++)
            {
                byteArrays[i] = Children[i].toBytes();
                totalSize += byteArrays[i].Length;
            }
            byte[] bytes = new byte[8 + Util.RoundUp(totalSize)];
            byte[] name = Util.NameTo4Bytes(Name);
            byte[] size = Util.IntTo4Bytes(totalSize);
            Buffer.BlockCopy(name, 0, bytes, 0, 4);
            Buffer.BlockCopy(size, 0, bytes, 4, 4);
            int bytesSoFar = 8;
            for(int i = 0; i < byteArrays.Length; i++)
            {
                Buffer.BlockCopy(byteArrays[i], 0, bytes, bytesSoFar, byteArrays[i].Length);
                bytesSoFar += byteArrays[i].Length;
            }
            return bytes;
        }

        public virtual bool EqualsNode(AVFXNode node)
        {
            if((node is AVFXLeaf) || (node is AVFXBlank))
            {
                Console.WriteLine("Wrong Type 1");

                return false;
            }
            if (Name != node.Name)
            {
                Console.WriteLine("Wrong Name {0} {1}", Name, node.Name);

                return false;
            }

            List<AVFXNode> notBlank = new List<AVFXNode>();
            List<AVFXNode> notBlank2 = new List<AVFXNode>();
            foreach (AVFXNode n in Children)
            {

                if(!(n is AVFXBlank))
                {
                    notBlank.Add(n);
                }
            }
            foreach (AVFXNode n in node.Children)
            {

                if (!(n is AVFXBlank))
                {
                    notBlank2.Add(n);
                }
            }

            if(notBlank.Count != notBlank2.Count)
            {
                Console.WriteLine("Wrong Node Size {0} : {1} / {2} : {3}", Name, notBlank.Count.ToString(), node.Name, notBlank2.Count.ToString());
                foreach(AVFXNode n in notBlank)
                {
                    Console.WriteLine(n.Name);
                }
                Console.WriteLine("---------------");
                foreach (AVFXNode n in notBlank2)
                {
                    Console.WriteLine(n.Name);
                }

                return false;
            }
            for(int idx = 0; idx < notBlank.Count; idx++)
            {
                //if (notBlank[idx].Name == "Modl") continue; // temp
                bool e = notBlank[idx].EqualsNode(notBlank2[idx]);
                if (!e)
                {
                    Console.WriteLine("{0} : {1}", Name, idx.ToString());

                    return false;
                }
            }

            return true;
        }
    }
}
