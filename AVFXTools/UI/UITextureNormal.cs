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
    public class UITextureNormal
    {
        public AVFXTextureNormal Tex;
        public string Name;
        public bool Assigned;
        //============================
        public bool Enabled;
        public int UvSetIdx;
        public int TextureIdx;

        public static readonly string[] TextureFilterOptions = Enum.GetNames(typeof(TextureFilterType));
        public int TextureFilterIdx;
        public static readonly string[] TextureBorderUOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderUIdx;
        public static readonly string[] TextureBorderVOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderVIdx;

        public UICurve NPow;

        public UITextureNormal(AVFXTextureNormal tex)
        {
            Tex = tex;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Enabled = (Tex.Enabled.Value == true);
            UvSetIdx = Tex.UvSetIdx.Value;
            TextureIdx = Tex.TextureIdx.Value;

            TextureFilterIdx = Array.IndexOf(TextureFilterOptions, Tex.TextureFilter.Value);
            TextureBorderUIdx = Array.IndexOf(TextureBorderUOptions, Tex.TextureBorderU.Value);
            TextureBorderVIdx = Array.IndexOf(TextureBorderVOptions, Tex.TextureBorderV.Value);

            NPow = new UICurve(Tex.NPow, "Power");
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode("Normal" + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Tex.Enabled.GiveValue(Enabled);
                }
                if (ImGui.InputInt("UVSet Index" + id, ref UvSetIdx))
                {
                    Tex.UvSetIdx.GiveValue(UvSetIdx);
                }
                if (ImGui.InputInt("Texture Index" + id, ref TextureIdx))
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
                NPow.Draw(id + "-npow");

                ImGui.TreePop();
            }
        }
    }
}
