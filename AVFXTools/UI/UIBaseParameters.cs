using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AVFXTools.UI
{
    public class UIBaseParameters
    {
        public AVFXBase AVFX;
        //=====================
        public bool IsDelayFastParticle;
        public bool IsFitGround;
        public bool IsTranformSkip;
        public bool IsAllStopOnHide;
        public bool CanBeClippedOut;
        public bool ClipBoxenabled;
        public Vector3 ClipBox;
        public Vector3 ClipBoxSize;
        public float BiasZmaxScale;
        public float BiasZmaxDistance;
        public bool IsCameraSpace;
        public bool IsFullEnvLight;
        public bool IsClipOwnSetting;
        public float SoftParticleFadeRange;
        public float SoftKeyOffset;
        public static readonly string[] DrawLayerOptions = Enum.GetNames(typeof(DrawLayer));
        public int DrawLayerIdx;
        public static readonly string[] DrawOrderOptions = Enum.GetNames(typeof(DrawOrder));
        public int DrawOrderIdx;
        public static readonly string[] DirectionalLightSourceOptions = Enum.GetNames(typeof(DirectionalLightSource));
        public int DirectionalLightSourceIdx;
        public static readonly string[] PointLightsOptions = Enum.GetNames(typeof(PointLightSouce));
        public int PointLightsIdx1;
        public int PointLightsIdx2;
        public Vector3 RevisedValuesPos;
        public Vector3 RevisedValuesRot;
        public Vector3 RevisedValuesScale;
        public Vector3 RevisedValuesColor;
        public bool FadeXenabled;
        public bool FadeYenabled;
        public bool FadeZenabled;
        public Vector3 FadeInner;
        public Vector3 FadeOuter;
        public bool GlobalFogEnabled;
        public float GlobalFogInfluence;
        public bool LTSEnabled;

        public UIBaseParameters(AVFXBase avfx){
            AVFX = avfx;
            //================
            IsDelayFastParticle = (AVFX.IsDelayFastParticle.Value == true);
            IsFitGround = (AVFX.IsFitGround.Value == true);
            IsTranformSkip = (AVFX.IsTranformSkip.Value == true);
            IsAllStopOnHide = (AVFX.IsAllStopOnHide.Value == true);
            CanBeClippedOut = (AVFX.CanBeClippedOut.Value == true);
            ClipBoxenabled = (AVFX.ClipBoxenabled.Value == true);
            ClipBox = new Vector3(AVFX.ClipBoxX.Value, AVFX.ClipBoxY.Value, AVFX.ClipBoxZ.Value);
            ClipBoxSize = new Vector3(AVFX.ClipBoxsizeX.Value, AVFX.ClipBoxsizeY.Value, AVFX.ClipBoxsizeZ.Value);
            BiasZmaxScale = AVFX.BiasZmaxScale.Value;
            BiasZmaxDistance = AVFX.BiasZmaxDistance.Value;
            IsCameraSpace = (AVFX.IsCameraSpace.Value == true);
            IsFullEnvLight = (AVFX.IsFullEnvLight.Value == true);
            IsClipOwnSetting = (AVFX.IsClipOwnSetting.Value == true);
            SoftParticleFadeRange = AVFX.SoftParticleFadeRange.Value;
            SoftKeyOffset = AVFX.SoftKeyOffset.Value;
            DrawLayerIdx = Array.IndexOf(DrawLayerOptions, AVFX.DrawLayerType.Value);
            DrawOrderIdx = Array.IndexOf(DrawOrderOptions, AVFX.DrawOrderType.Value);
            DirectionalLightSourceIdx = Array.IndexOf(DirectionalLightSourceOptions, AVFX.DirectionalLightSourceType.Value);
            PointLightsIdx1 = Array.IndexOf(PointLightsOptions, AVFX.PointLightsType1.Value);
            PointLightsIdx2 = Array.IndexOf(PointLightsOptions, AVFX.PointLightsType2.Value);
            RevisedValuesPos = new Vector3(AVFX.RevisedValuesPosX.Value, AVFX.RevisedValuesPosY.Value, AVFX.RevisedValuesPosZ.Value);
            RevisedValuesRot = new Vector3(AVFX.RevisedValuesRotX.Value, AVFX.RevisedValuesRotY.Value, AVFX.RevisedValuesRotZ.Value);
            RevisedValuesScale = new Vector3(AVFX.RevisedValuesScaleX.Value, AVFX.RevisedValuesScaleY.Value, AVFX.RevisedValuesScaleZ.Value);
            RevisedValuesColor = new Vector3(AVFX.RevisedValuesR.Value, AVFX.RevisedValuesG.Value, AVFX.RevisedValuesB.Value);
            FadeXenabled = (AVFX.FadeXenabled.Value == true);
            FadeYenabled = (AVFX.FadeYenabled.Value == true);
            FadeZenabled = (AVFX.FadeZenabled.Value == true);
            FadeInner = new Vector3(AVFX.FadeXinner.Value, AVFX.FadeYinner.Value, AVFX.FadeZinner.Value);
            FadeOuter = new Vector3(AVFX.FadeXouter.Value, AVFX.FadeYouter.Value, AVFX.FadeZouter.Value);
            GlobalFogEnabled = (AVFX.GlobalFogEnabled.Value == true);
            GlobalFogInfluence = AVFX.GlobalFogInfluence.Value;
            LTSEnabled = (AVFX.LTSEnabled.Value == true);
        }

        public void Draw(string id)
        {
            if (ImGui.Checkbox("Delay Fast Particle" + id, ref IsDelayFastParticle))
            {
                AVFX.IsDelayFastParticle.GiveValue(IsDelayFastParticle);
            }
            if (ImGui.Checkbox("Fit Ground" + id, ref IsFitGround))
            {
                AVFX.IsFitGround.GiveValue(IsFitGround);
            }
            if (ImGui.Checkbox("Transform Skip" + id, ref IsTranformSkip))
            {
                AVFX.IsTranformSkip.GiveValue(IsTranformSkip);
            }
            if (ImGui.Checkbox("Stop on Hide" + id, ref IsAllStopOnHide))
            {
                AVFX.IsAllStopOnHide.GiveValue(IsAllStopOnHide);
            }
            if (ImGui.Checkbox("Can be Clipped Out" + id, ref CanBeClippedOut))
            {
                AVFX.CanBeClippedOut.GiveValue(CanBeClippedOut);
            }
            if (ImGui.Checkbox("Clip Box Enabled" + id, ref ClipBoxenabled))
            {
                AVFX.ClipBoxenabled.GiveValue(ClipBoxenabled);
            }
            if (ImGui.InputFloat3("Clip Box Position", ref ClipBox))
            {
                AVFX.ClipBoxX.GiveValue(ClipBox.X);
                AVFX.ClipBoxY.GiveValue(ClipBox.Y);
                AVFX.ClipBoxZ.GiveValue(ClipBox.Z);
            }
            if (ImGui.InputFloat3("Clip Box Size", ref ClipBoxSize))
            {
                AVFX.ClipBoxsizeX.GiveValue(ClipBoxSize.X);
                AVFX.ClipBoxsizeY.GiveValue(ClipBoxSize.Y);
                AVFX.ClipBoxsizeZ.GiveValue(ClipBoxSize.Z);
            }
            if(ImGui.InputFloat("Bias Z Max Scale", ref BiasZmaxScale))
            {
                AVFX.BiasZmaxScale.GiveValue(BiasZmaxScale);
            }
            if (ImGui.InputFloat("Bias Z Max Distance", ref BiasZmaxDistance))
            {
                AVFX.BiasZmaxDistance.GiveValue(BiasZmaxDistance);
            }
            if (ImGui.Checkbox("Camera Space" + id, ref IsCameraSpace))
            {
                AVFX.IsCameraSpace.GiveValue(IsDelayFastParticle);
            }
            if (ImGui.Checkbox("Full Env Light" + id, ref IsFullEnvLight))
            {
                AVFX.IsFullEnvLight.GiveValue(IsFullEnvLight);
            }
            if (ImGui.Checkbox("Clip Own Setting" + id, ref IsClipOwnSetting))
            {
                AVFX.IsClipOwnSetting.GiveValue(IsClipOwnSetting);
            }
            if (ImGui.InputFloat("Soft Particle Fade Range", ref SoftParticleFadeRange))
            {
                AVFX.SoftParticleFadeRange.GiveValue(SoftParticleFadeRange);
            }
            if (ImGui.InputFloat("Soft Key Offset", ref SoftKeyOffset))
            {
                AVFX.SoftKeyOffset.GiveValue(SoftKeyOffset);
            }
            if (UIUtils.EnumComboBox("Draw Layer" + id, DrawLayerOptions, ref DrawLayerIdx))
            {
                AVFX.DrawLayerType.GiveValue(DrawLayerOptions[DrawLayerIdx]);
            }
            if (UIUtils.EnumComboBox("Draw Order" + id, DrawOrderOptions, ref DrawOrderIdx))
            {
                AVFX.DrawOrderType.GiveValue(DrawOrderOptions[DrawOrderIdx]);
            }
            if (UIUtils.EnumComboBox("Directional Light Source" + id, DirectionalLightSourceOptions, ref DirectionalLightSourceIdx))
            {
                AVFX.DirectionalLightSourceType.GiveValue(DirectionalLightSourceOptions[DirectionalLightSourceIdx]);
            }
            if (UIUtils.EnumComboBox("Point Light 1" + id, PointLightsOptions, ref PointLightsIdx1))
            {
                AVFX.PointLightsType1.GiveValue(PointLightsOptions[PointLightsIdx1]);
            }
            if (UIUtils.EnumComboBox("Point Light 2" + id, PointLightsOptions, ref PointLightsIdx2))
            {
                AVFX.PointLightsType2.GiveValue(PointLightsOptions[PointLightsIdx2]);
            }
            if (ImGui.InputFloat3("Revised Position", ref RevisedValuesPos))
            {
                AVFX.RevisedValuesPosX.GiveValue(RevisedValuesPos.X);
                AVFX.RevisedValuesPosY.GiveValue(RevisedValuesPos.Y);
                AVFX.RevisedValuesPosZ.GiveValue(RevisedValuesPos.Z);
            }
            if (ImGui.InputFloat3("Revised Rotation", ref RevisedValuesRot))
            {
                AVFX.RevisedValuesRotX.GiveValue(RevisedValuesRot.X);
                AVFX.RevisedValuesRotY.GiveValue(RevisedValuesRot.Y);
                AVFX.RevisedValuesRotZ.GiveValue(RevisedValuesRot.Z);
            }
            if (ImGui.InputFloat3("Revised Scale", ref RevisedValuesScale))
            {
                AVFX.RevisedValuesScaleX.GiveValue(RevisedValuesScale.X);
                AVFX.RevisedValuesScaleY.GiveValue(RevisedValuesScale.Y);
                AVFX.RevisedValuesScaleZ.GiveValue(RevisedValuesScale.Z);
            }
            if (ImGui.ColorEdit3("Revised Color", ref RevisedValuesColor, ImGuiColorEditFlags.Float))
            {
                AVFX.RevisedValuesR.GiveValue(RevisedValuesColor.X);
                AVFX.RevisedValuesG.GiveValue(RevisedValuesColor.Y);
                AVFX.RevisedValuesB.GiveValue(RevisedValuesColor.Z);
            }
            if (ImGui.Checkbox("Fade X Enabled" + id, ref FadeXenabled))
            {
                AVFX.FadeXenabled.GiveValue(FadeXenabled);
            }
            if (ImGui.Checkbox("Fade Y Enabled" + id, ref FadeYenabled))
            {
                AVFX.FadeYenabled.GiveValue(FadeYenabled);
            }
            if (ImGui.Checkbox("Fade Z Enabled" + id, ref FadeZenabled))
            {
                AVFX.FadeZenabled.GiveValue(FadeZenabled);
            }
            if(ImGui.InputFloat3("Fade Inner", ref FadeInner))
            {
                AVFX.FadeXinner.GiveValue(FadeInner.X);
                AVFX.FadeYinner.GiveValue(FadeInner.Y);
                AVFX.FadeZinner.GiveValue(FadeInner.Z);
            }
            if (ImGui.InputFloat3("Fade Outer", ref FadeOuter))
            {
                AVFX.FadeXouter.GiveValue(FadeOuter.X);
                AVFX.FadeYouter.GiveValue(FadeOuter.Y);
                AVFX.FadeZouter.GiveValue(FadeOuter.Z);
            }
            if (ImGui.Checkbox("Global Fog Enabled" + id, ref GlobalFogEnabled))
            {
                AVFX.GlobalFogEnabled.GiveValue(GlobalFogEnabled);
            }
            if (ImGui.InputFloat("Global Fog Influence", ref GlobalFogInfluence))
            {
                AVFX.GlobalFogInfluence.GiveValue(GlobalFogInfluence);
            }
            if (ImGui.Checkbox("LTS Enabled" + id, ref LTSEnabled))
            {
                AVFX.LTSEnabled.GiveValue(LTSEnabled);
            }
        }
    }
}
