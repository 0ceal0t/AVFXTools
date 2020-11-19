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
    public class UIParticleDataModel : UIParticleDataBase
    {
        public AVFXParticleDataModel Data;
        //==========================
        public int ModelNumberRandomValue;
        public static readonly string[] ModelNumberRandomTypeOptions = Enum.GetNames(typeof(RandomType));
        public int ModelNumberRandomTypeIdx;
        public int ModelNumberRandomInterval;
        public static readonly string[] FresnelTypeOptions = Enum.GetNames(typeof(FresnelType));
        public int FresnelTypeIdx;
        public static readonly string[] DirectionLightTypeOptions = Enum.GetNames(typeof(DirectionalLightType));
        public int DirectionLightTypeIdx;
        public static readonly string[] PointLightTypeOptions = Enum.GetNames(typeof(PointLightType));
        public int PointLightTypeIdx;
        public bool IsLightning;
        public bool IsMorph;
        public int ModelIdx;

        public UICurve FresnelCurve;
        public UICurve3Axis FresnelRotation;
        public UICurveColor ColorBegin;
        public UICurveColor ColorEnd;

        public UIParticleDataModel(AVFXParticleDataModel data)
        {
            Data = data;
            //=======================
            ModelNumberRandomValue = Data.ModelNumberRandomValue.Value;
            ModelNumberRandomTypeIdx = Array.IndexOf(ModelNumberRandomTypeOptions, Data.ModelNumberRandomType.Value);
            ModelNumberRandomInterval = Data.ModelNumberRandomInterval.Value;
            FresnelTypeIdx = Array.IndexOf(FresnelTypeOptions, Data.FresnelType.Value);
            DirectionLightTypeIdx = Array.IndexOf(DirectionLightTypeOptions, Data.DirectionalLightType.Value);
            PointLightTypeIdx = Array.IndexOf(PointLightTypeOptions, Data.PointLightType.Value);
            IsLightning = (Data.IsLightning.Value == true);
            IsMorph = (Data.IsMorph.Value == true);
            ModelIdx = Data.ModelIdx.Value;
            //=======================
            FresnelCurve = new UICurve(Data.FresnelCurve, "Fresnel Curve");
            FresnelRotation = new UICurve3Axis(Data.FresnelRotation, "Fresnel Rotation");
            ColorBegin = new UICurveColor(Data.ColorBegin, "Color Begin");
            ColorEnd = new UICurveColor(Data.ColorEnd, "Color End");
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Data" + id))
            {
                if (ImGui.DragInt("Model Number Random" + id, ref ModelNumberRandomValue, 1, 0))
                {
                    Data.ModelNumberRandomValue.GiveValue(ModelNumberRandomValue);
                }
                if (UIUtils.EnumComboBox("Model Number Random" + id, ModelNumberRandomTypeOptions, ref ModelNumberRandomTypeIdx))
                {
                    Data.ModelNumberRandomType.GiveValue(ModelNumberRandomTypeOptions[ModelNumberRandomTypeIdx]);
                }
                if (ImGui.DragInt("Model Number Random Interval" + id, ref ModelNumberRandomInterval, 1, 0))
                {
                    Data.ModelNumberRandomInterval.GiveValue(ModelNumberRandomInterval);
                }
                if (UIUtils.EnumComboBox("Fresnel Type" + id, FresnelTypeOptions, ref FresnelTypeIdx))
                {
                    Data.FresnelType.GiveValue(FresnelTypeOptions[FresnelTypeIdx]);
                }
                if (UIUtils.EnumComboBox("Direction Light Type" + id, DirectionLightTypeOptions, ref DirectionLightTypeIdx))
                {
                    Data.DirectionalLightType.GiveValue(DirectionLightTypeOptions[DirectionLightTypeIdx]);
                }
                if (UIUtils.EnumComboBox("Point Light Type" + id, PointLightTypeOptions, ref PointLightTypeIdx))
                {
                    Data.PointLightType.GiveValue(PointLightTypeOptions[PointLightTypeIdx]);
                }
                if (ImGui.Checkbox("Is Lightning" + id, ref IsLightning))
                {
                    Data.IsLightning.GiveValue(IsLightning);
                }
                if (ImGui.Checkbox("Is Soft Particle" + id, ref IsMorph))
                {
                    Data.IsMorph.GiveValue(IsMorph);
                }
                if (ImGui.DragInt("Model Number" + id, ref ModelIdx, 1, 0))
                {
                    Data.ModelIdx.GiveValue(ModelIdx);
                }
                //=================
                FresnelCurve.Draw(id + "-fresnelcurve");
                FresnelRotation.Draw(id + "-fresnelrotation");
                ColorBegin.Draw(id + "-colorbegin");
                ColorEnd.Draw(id + "-colorend");

                ImGui.TreePop();
            }
        }
    }
}
