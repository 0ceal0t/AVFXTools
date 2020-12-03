using AVFXLib.Main;
using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITimelineClip : UIBase
    {
        public AVFXTimelineClip Clip;
        public int Idx;
        //===============================
        public static uint UniqueIdBytesSize = 164;
        public byte[] UniqueIdBytes = new byte[UniqueIdBytesSize];

        public Vector4 UnknownInts;
        public Vector4 UnknownFloats;

        public UITimelineClip(AVFXTimelineClip clip)
        {
            Clip = clip;
            //=====================
            byte[] uniqueId = Util.StringToBytes(clip.UniqueId);
            Buffer.BlockCopy(uniqueId, 0, UniqueIdBytes, 0, uniqueId.Length);
            UnknownInts = new Vector4(clip.UnknownInts[0], clip.UnknownFloats[1], clip.UnknownInts[2], clip.UnknownInts[3]);
            UnknownFloats = new Vector4(clip.UnknownFloats[0], clip.UnknownFloats[1], clip.UnknownFloats[2], clip.UnknownFloats[3]);
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/TLClip" + Idx;
            if (ImGui.TreeNode("Clip " + Idx + id))
            {
                if (UIUtils.RemoveButton("Delete" + id))
                {
                    // TODO
                }
                if (ImGui.InputFloat4("Unknown Ints" + id, ref UnknownInts))
                {
                    Clip.UnknownInts[0] = (int)UnknownInts.X;
                    Clip.UnknownInts[1] = (int)UnknownInts.Y;
                    Clip.UnknownInts[2] = (int)UnknownInts.Z;
                    Clip.UnknownInts[3] = (int)UnknownInts.W;
                }
                if (ImGui.InputFloat4("Unknown Floats" + id, ref UnknownFloats))
                {
                    Clip.UnknownFloats[0] = UnknownFloats.X;
                    Clip.UnknownFloats[1] = UnknownFloats.Y;
                    Clip.UnknownFloats[2] = UnknownFloats.Z;
                    Clip.UnknownFloats[3] = UnknownFloats.W;
                }
                if (ImGui.InputText("Unique Id", UniqueIdBytes, UniqueIdBytesSize))
                {
                    Clip.UniqueId = Util.BytesToString(UniqueIdBytes).Trim('\0');
                }
                ImGui.TreePop();
            }
        }
    }
}
