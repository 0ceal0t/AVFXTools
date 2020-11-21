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
    public class UIBinder
    {
        public AVFXBinder Binder;
        //==================
        public bool StartToGlobalDirection;
        public bool VfxScaleEnabled;
        public float VfxScaleBias;
        public bool VfxScaleDepthOffset;
        public bool VfxScaleInterpolation;
        public bool TransformScale;
        public bool TransformScaleDepthOffset;
        public bool TransformScaleInterpolation;
        public bool FollowingTargetOrientation;
        public bool DocumentScaleEnabled;
        public bool AdjustToScreenEnabled;
        public int Life;
        public static readonly string[] BinderRotationOptions = Enum.GetNames(typeof(BinderRotation));
        public int BinderRotationIdx;
        public static readonly string[] BTypeOptions = Enum.GetNames(typeof(BinderType));
        public int BTypeIdx;
        //===================
        public UIBinderProperties Prop;
        //====================
        public UIBinderDataBase Data;

        public UIBinder(AVFXBinder binder)
        {
            Binder = binder;
            //=====================
            StartToGlobalDirection = (Binder.StartToGlobalDirection.Value == true);
            VfxScaleEnabled = (Binder.VfxScaleEnabled.Value == true);
            VfxScaleBias = Binder.VfxScaleBias.Value;
            VfxScaleDepthOffset = (Binder.VfxScaleDepthOffset.Value == true);
            VfxScaleInterpolation = (Binder.VfxScaleInterpolation.Value == true);
            TransformScale = (Binder.TransformScale.Value == true);
            TransformScaleDepthOffset = (Binder.TransformScaleDepthOffset.Value == true);
            TransformScaleInterpolation = (Binder.TransformScaleInterpolation.Value == true);
            FollowingTargetOrientation = (Binder.FollowingTargetOrientation.Value == true);
            DocumentScaleEnabled = (Binder.DocumentScaleEnabled.Value == true);
            AdjustToScreenEnabled = (Binder.AdjustToScreenEnabled.Value == true);
            Life = Binder.Life.Value;
            BinderRotationIdx = Array.IndexOf(BinderRotationOptions, Binder.BinderRotationType.Value);
            BTypeIdx = Array.IndexOf(BTypeOptions, Binder.BType.Value);
            //=====================
            if (Binder.Prop.Assigned)
            {
                Prop = new UIBinderProperties(Binder.Prop);
            }
            //======================
            switch (Binder.BType.Value)
            {
                case "Point":
                    Data = new UIBinderDataPoint((AVFXBinderDataPoint)Binder.Data);
                    break;
            }
        }

        public void Draw(string id)
        {
            if (ImGui.Checkbox("Start to Global Direction" + id, ref StartToGlobalDirection))
            {
                Binder.StartToGlobalDirection.GiveValue(StartToGlobalDirection);
            }
            if (ImGui.Checkbox("VFX Scale Enabled" + id, ref VfxScaleEnabled))
            {
                Binder.VfxScaleEnabled.GiveValue(VfxScaleEnabled);
            }
            if (ImGui.InputFloat("VFX Scale Bias" + id, ref VfxScaleBias))
            {
                Binder.VfxScaleBias.GiveValue(VfxScaleBias);
            }
            if (ImGui.Checkbox("VFX Scale Depth Offset" + id, ref VfxScaleDepthOffset))
            {
                Binder.VfxScaleDepthOffset.GiveValue(VfxScaleDepthOffset);
            }
            if (ImGui.Checkbox("VFX Scale Interpolation" + id, ref VfxScaleInterpolation))
            {
                Binder.VfxScaleInterpolation.GiveValue(VfxScaleInterpolation);
            }
            if (ImGui.Checkbox("Transform Scale" + id, ref TransformScale))
            {
                Binder.TransformScale.GiveValue(TransformScale);
            }
            if (ImGui.Checkbox("Transform Scale Depth Offset" + id, ref TransformScaleDepthOffset))
            {
                Binder.TransformScaleDepthOffset.GiveValue(TransformScaleDepthOffset);
            }
            if (ImGui.Checkbox("Transform Scale Interpolation" + id, ref TransformScaleInterpolation))
            {
                Binder.TransformScaleInterpolation.GiveValue(TransformScaleInterpolation);
            }
            if (ImGui.Checkbox("Following Target Orientation" + id, ref FollowingTargetOrientation))
            {
                Binder.FollowingTargetOrientation.GiveValue(FollowingTargetOrientation);
            }
            if (ImGui.Checkbox("Document Scale" + id, ref DocumentScaleEnabled))
            {
                Binder.DocumentScaleEnabled.GiveValue(DocumentScaleEnabled);
            }
            if (ImGui.Checkbox("Adjust to Screen" + id, ref AdjustToScreenEnabled))
            {
                Binder.AdjustToScreenEnabled.GiveValue(AdjustToScreenEnabled);
            }
            if (ImGui.InputInt("Life" + id, ref Life))
            {
                Binder.Life.GiveValue(Life);
            }
            if (UIUtils.EnumComboBox("Rotation Type" + id, BinderRotationOptions, ref BinderRotationIdx))
            {
                Binder.BinderRotationType.GiveValue(BinderRotationOptions[BinderRotationIdx]);
            }
            if (UIUtils.EnumComboBox("Binder Type" + id, BTypeOptions, ref BTypeIdx))
            {
                Binder.BType.GiveValue(BTypeOptions[BTypeIdx]);
            }
            //=====================
            if (Prop != null)
            {
                Prop.Draw(id + "-prop");
            }
            //====================
            if (Data != null)
            {
                Data.Draw(id + "-data");
            }
        }
    }
}
