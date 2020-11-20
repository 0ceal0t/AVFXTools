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
    public class UITexturePalette
    {
        public AVFXTexturePalette Tex;
        public string Name;
        public bool Assigned;
        //============================
        public bool Enabled;
        public int TextureIdx;

        public static readonly string[] TextureFilterOptions = Enum.GetNames(typeof(TextureFilterType));
        public int TextureFilterIdx;
        public static readonly string[] TextureBorderOptions = Enum.GetNames(typeof(TextureBorderType));
        public int TextureBorderIdx;

        public UITexturePalette(AVFXTexturePalette tex)
        {
            Tex = tex;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Enabled = (Tex.Enabled.Value == true);
            TextureIdx = Tex.TextureIdx.Value;

            TextureFilterIdx = Array.IndexOf(TextureFilterOptions, Tex.TextureFilter.Value);
            TextureBorderIdx = Array.IndexOf(TextureBorderOptions, Tex.TextureBorder.Value);
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode("Palette" + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Tex.Enabled.GiveValue(Enabled);
                }
                if (ImGui.InputInt("Texture Index" + id, ref TextureIdx))
                {
                    Tex.TextureIdx.GiveValue(TextureIdx);
                }
                if (UIUtils.EnumComboBox("Texture Filter" + id, TextureFilterOptions, ref TextureFilterIdx))
                {
                    Tex.TextureFilter.GiveValue(TextureFilterOptions[TextureFilterIdx]);
                }
                if (UIUtils.EnumComboBox("Texture Border" + id, TextureBorderOptions, ref TextureBorderIdx))
                {
                    Tex.TextureBorder.GiveValue(TextureBorderOptions[TextureBorderIdx]);
                }

                ImGui.TreePop();
            }
        }
    }
}
