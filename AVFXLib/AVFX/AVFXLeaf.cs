using AVFXLib.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AVFXLib.AVFX
{
    public class AVFXLeaf: AVFXNode
    {
        public byte[] Contents { get; set; }
        public int Size { get; set; }

        public AVFXLeaf(string n, int s, byte[] c): base(n)
        {
            Name = n;
            Size = s;
            Contents = c;
        }

        public override bool EqualsNode(AVFXNode node)
        {
            if(!(node is AVFXLeaf))
            {
                Console.WriteLine("Wrong Type 2 {0}", node.Name);
                return false;
            }
            AVFXLeaf leaf = (AVFXLeaf)node;
            if (Name != leaf.Name)
            {
                Console.WriteLine("Wrong Name {0} {1}", Name, leaf.Name);
                return false;
            }
            if(Size != leaf.Size)
            {
                Console.WriteLine("Wrong Leaf Size {0} : {1} / {2} : {3}", Name, Size.ToString(), leaf.Name, leaf.Size.ToString());
                return false;
            }
            if(Contents.Length != leaf.Contents.Length)
            {
                Console.WriteLine("Wrong Contents Size {0} : {1} / {2} : {3}", Name, Contents.Length.ToString(), leaf.Name, leaf.Contents.Length.ToString());
                return false;
            }
            for(int idx = 0; idx < Contents.Length; idx++)
            {
                if(Contents[idx] != leaf.Contents[idx])
                {
                    Console.WriteLine("Wrong Contents in {0} byte {1} : {2} / {3}", Name, idx, Contents[idx].ToString(), leaf.Contents[idx].ToString());
                    Util.PrintBytes(Contents);
                    Util.PrintBytes(leaf.Contents);
                    return false;
                }
            }
            return true;
        }
    }
}
