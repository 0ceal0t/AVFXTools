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
    public class UITextureColor1
    {
        public AVFXTextureColor1 Tex;
        public ParticleItem Item;
        //============================
        public bool Enabled;
        public bool ColorToAlpha;
        public bool UseScreenCopy;
        public bool PreviousFrameCopy;
        public int UvSetIdx;
        public int MaskTextureIdx;

        public static readonly string[] TextureFilterOptions = Enum.GetNames(typeof(TextureFilterType));
        public int TextureFilterIdx;
        public static readonly string[] TextureBorderUOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderUIdx;
        public static readonly string[] TextureBorderVOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderVIdx;
        public static readonly string[] TextureCalculateColorOptions = Enum.GetNames(typeof(TextureCalculateColor));
        public int TextureCalculateColorIdx;
        public static readonly string[] TextureCalculateAlphaOptions = Enum.GetNames(typeof(TextureCalculateAlpha));
        public int TextureCalculateAlphaIdx;

        public UITextureColor1(AVFXTextureColor1 tex, ParticleItem item)
        {
            Tex = tex;
            Item = item;
            //====================
            Enabled = (Tex.Enabled.Value == true);
            ColorToAlpha = (Tex.ColorToAlpha.Value == true);
            UseScreenCopy = (Tex.UseScreenCopy.Value == true);
            PreviousFrameCopy = (Tex.PreviousFrameCopy.Value == true);
            UvSetIdx = Tex.UvSetIdx.Value;
            MaskTextureIdx = Tex.MaskTextureIdx.Value;

            TextureFilterIdx = Array.IndexOf(TextureFilterOptions, Tex.TextureFilter.Value);
            TextureBorderUIdx = Array.IndexOf(TextureBorderUOptions, Tex.TextureBorderU.Value);
            TextureBorderVIdx = Array.IndexOf(TextureBorderVOptions, Tex.TextureBorderV.Value);
            TextureCalculateColorIdx = Array.IndexOf(TextureCalculateColorOptions, Tex.TextureCalculateColor.Value);
            TextureCalculateAlphaIdx = Array.IndexOf(TextureCalculateAlphaOptions, Tex.TextureCalculateAlpha.Value);
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Texture Color 1" + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Tex.Enabled.GiveValue(Enabled);
                }
                if (ImGui.Checkbox("Color To Alpha" + id, ref ColorToAlpha))
                {
                    Tex.ColorToAlpha.GiveValue(ColorToAlpha);
                }
                if (ImGui.Checkbox("Use Screen Copy" + id, ref UseScreenCopy))
                {
                    Tex.UseScreenCopy.GiveValue(UseScreenCopy);
                }
                if (ImGui.Checkbox("Previous Frame Copy" + id, ref PreviousFrameCopy))
                {
                    Tex.PreviousFrameCopy.GiveValue(PreviousFrameCopy);
                }
                if (ImGui.DragInt("UVSet Index" + id, ref UvSetIdx, 1, 0))
                {
                    Tex.UvSetIdx.GiveValue(UvSetIdx);
                }
                if (ImGui.DragInt("Mask Texture Index" + id, ref MaskTextureIdx, 1, -1))
                {
                    Tex.MaskTextureIdx.GiveValue(MaskTextureIdx);
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
                if (UIUtils.EnumComboBox("Texture Calculate Color" + id, TextureCalculateColorOptions, ref TextureCalculateColorIdx))
                {
                    Tex.TextureCalculateColor.GiveValue(TextureCalculateColorOptions[TextureCalculateColorIdx]);
                }
                if (UIUtils.EnumComboBox("Texture Calculate Alpha" + id, TextureCalculateAlphaOptions, ref TextureCalculateAlphaIdx))
                {
                    Tex.TextureCalculateAlpha.GiveValue(TextureCalculateAlphaOptions[TextureCalculateAlphaIdx]);
                }

                ImGui.TreePop();
            }
        }
    }
}
