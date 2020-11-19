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
    public class UIParticleDataPowder: UIParticleDataBase
    {
        public AVFXParticleDataPowder Data;
        public ParticleItem Item;
        //==========================
        public static readonly string[] DirectionLightTypeOptions = Enum.GetNames(typeof(DirectionalLightType));
        public int DirectionLightTypeIdx;
        public bool IsLightning;
        public float CenterOffset;

        public UIParticleDataPowder(AVFXParticleDataPowder data, ParticleItem item)
        {
            Data = data;
            Item = item;
            //=======================
            DirectionLightTypeIdx = Array.IndexOf(DirectionLightTypeOptions, Data.DirectionalLightType.Value);
            IsLightning = (Data.IsLightning.Value == true);
            CenterOffset = Data.CenterOffset.Value;
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Data" + id))
            {
                if (UIUtils.EnumComboBox("Direction Light Type" + id, DirectionLightTypeOptions, ref DirectionLightTypeIdx))
                {
                    Data.DirectionalLightType.GiveValue(DirectionLightTypeOptions[DirectionLightTypeIdx]);
                }
                if (ImGui.Checkbox("Is Lightning" + id, ref IsLightning))
                {
                    Data.IsLightning.GiveValue(IsLightning);
                }
                if (ImGui.DragFloat("Center Offset" + id, ref CenterOffset, 1, 0))
                {
                    Data.CenterOffset.GiveValue(CenterOffset);
                }

                ImGui.TreePop();
            }
        }
    }
}
