using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UITextureDistortion
    {
        public AVFXTextureDistortion Tex;
        public ParticleItem Item;
        public string Name;
        public bool Assigned;
        //============================
        public bool Enabled;
        public bool TargetUV1;
        public bool TargetUV2;
        public bool TargetUV3;
        public bool TargetUV4;
        public int UvSetIdx;
        public int TextureIdx;

        public static readonly string[] TextureFilterOptions = Enum.GetNames(typeof(TextureFilterType));
        public int TextureFilterIdx;
        public static readonly string[] TextureBorderUOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderUIdx;
        public static readonly string[] TextureBorderVOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderVIdx;

        public UICurve DPow;

        public UITextureDistortion(AVFXTextureDistortion tex, ParticleItem item)
        {
            Tex = tex;
            Item = item;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Enabled = (Tex.Enabled.Value == true);
            TargetUV1 = (Tex.TargetUV1.Value == true);
            TargetUV2 = (Tex.TargetUV2.Value == true);
            TargetUV3 = (Tex.TargetUV3.Value == true);
            TargetUV4 = (Tex.TargetUV4.Value == true);
            UvSetIdx = Tex.UvSetIdx.Value;
            TextureIdx = Tex.TextureIdx.Value;

            TextureFilterIdx = Array.IndexOf(TextureFilterOptions, Tex.TextureFilter.Value);
            TextureBorderUIdx = Array.IndexOf(TextureBorderUOptions, Tex.TextureBorderU.Value);
            TextureBorderVIdx = Array.IndexOf(TextureBorderVOptions, Tex.TextureBorderV.Value);

            DPow = new UICurve(Tex.DPow, "Power");
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode("Distortion" + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Tex.Enabled.GiveValue(Enabled);
                }
                if (ImGui.Checkbox("Distort UV1" + id, ref TargetUV1))
                {
                    Tex.TargetUV1.GiveValue(TargetUV1);
                }
                if (ImGui.Checkbox("Distort UV2" + id, ref TargetUV2))
                {
                    Tex.TargetUV2.GiveValue(TargetUV2);
                }
                if (ImGui.Checkbox("Distort UV3" + id, ref TargetUV3))
                {
                    Tex.TargetUV3.GiveValue(TargetUV3);
                }
                if (ImGui.Checkbox("Distort UV4" + id, ref TargetUV4))
                {
                    Tex.TargetUV4.GiveValue(TargetUV4);
                }
                if (ImGui.DragInt("UVSet Index" + id, ref UvSetIdx, 1, 0))
                {
                    Tex.UvSetIdx.GiveValue(UvSetIdx);
                }
                if (ImGui.DragInt("Texture Index" + id, ref TextureIdx, 1, -1))
                {
                    Tex.TextureIdx.GiveValue(TextureIdx);
                }
                if (UIUtils.EnumComboBox("Texture Filter" + id, TextureFilterOptions, ref TextureFilterIdx))
                {
                    Tex.TextureFilter.GiveValue(TextureFilterOptions[TextureFilterIdx]);
                }
                if (UIUtils.EnumComboBox("Texture Border U" + id, TextureBorderUOptions, ref TextureBorderUIdx))
                {
                    Tex.TextureBorderU.GiveValue(TextureBorderUOptions[TextureBorderUIdx]);
                }
                if (UIUtils.EnumComboBox("Texture Border V" + id, TextureBorderVOptions, ref TextureBorderVIdx))
                {
                    Tex.TextureBorderV.GiveValue(TextureBorderVOptions[TextureBorderVIdx]);
                }
                DPow.Draw(id + "-dpow");

                ImGui.TreePop();
            }
        }
    }
}
