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
    public class UITextureReflection
    {
        public AVFXTextureReflection Tex;
        public bool Assigned;
        //============================
        public bool Enabled;
        public bool UseScreenCopy;
        public int TextureIdx;

        public static readonly string[] TextureFilterOptions = Enum.GetNames(typeof(TextureFilterType));
        public int TextureFilterIdx;
        public static readonly string[] TextureCalculateColorOptions = Enum.GetNames(typeof(TextureCalculateColor));
        public int TextureCalculateColorIdx;
        public UICurve RPow;

        public UITextureReflection(AVFXTextureReflection tex)
        {
            Tex = tex;
            if (!tex.Assigned) return;
            Assigned = true;
            //====================
            Enabled = (Tex.Enabled.Value == true);
            UseScreenCopy = (Tex.UseScreenCopy.Value == true);
            TextureIdx = Tex.TextureIdx.Value;

            TextureFilterIdx = Array.IndexOf(TextureFilterOptions, Tex.TextureFilter.Value);
            TextureCalculateColorIdx = Array.IndexOf(TextureCalculateColorOptions, Tex.TextureCalculateColor.Value);
            RPow = new UICurve(Tex.RPow, "Power");
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode("Reflection" + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Tex.Enabled.GiveValue(Enabled);
                }
                if (ImGui.Checkbox("Use Screen Copy" + id, ref UseScreenCopy))
                {
                    Tex.UseScreenCopy.GiveValue(UseScreenCopy);
                }
                if (ImGui.DragInt("Texture Index" + id, ref TextureIdx, 1, -1))
                {
                    Tex.TextureIdx.GiveValue(TextureIdx);
                }
                if (UIUtils.EnumComboBox("Texture Filter" + id, TextureFilterOptions, ref TextureFilterIdx))
                {
                    Tex.TextureFilter.GiveValue(TextureFilterOptions[TextureFilterIdx]);
                }
                if (UIUtils.EnumComboBox("Texture Calculate Color" + id, TextureCalculateColorOptions, ref TextureCalculateColorIdx))
                {
                    Tex.TextureCalculateColor.GiveValue(TextureCalculateColorOptions[TextureCalculateColorIdx]);
                }
                RPow.Draw(id + "-rpow");

                ImGui.TreePop();
            }
        }
    }
}
