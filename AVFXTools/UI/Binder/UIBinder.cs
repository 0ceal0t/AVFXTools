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
    public class UIBinder : UIBase
    {
        public AVFXBinder Binder;
        public int Idx;
        //==================
        public UIBinderProperties Prop;
        //====================
        public UIBase Data;

        public UIBinder(AVFXBinder binder)
        {
            Binder = binder;
            //=====================
            Attributes.Add(new UICheckbox("Start to Global Direction", Binder.StartToGlobalDirection));
            Attributes.Add(new UICheckbox("VFX Scale", Binder.VfxScaleEnabled));
            Attributes.Add(new UIFloat("VFX Scale Bias", Binder.VfxScaleBias));
            Attributes.Add(new UICheckbox("VFX Scale Depth Offset", Binder.VfxScaleDepthOffset));
            Attributes.Add(new UICheckbox("VFX Scale Interpolation", Binder.VfxScaleInterpolation));
            Attributes.Add(new UICheckbox("Transform Scale", Binder.TransformScale));
            Attributes.Add(new UICheckbox("Transform Scale Depth Offset", Binder.TransformScaleDepthOffset));
            Attributes.Add(new UICheckbox("Transform Scale Interpolation", Binder.TransformScaleInterpolation));
            Attributes.Add(new UICheckbox("Following Target Orientation", Binder.FollowingTargetOrientation));
            Attributes.Add(new UICheckbox("Document Scale Enabled", Binder.DocumentScaleEnabled));
            Attributes.Add(new UICheckbox("Adjust to Screen", Binder.AdjustToScreenEnabled));
            Attributes.Add(new UIInt("Life", Binder.Life));
            Attributes.Add(new UICombo<BinderRotation>("Binder Rotation Type", Binder.BinderRotationType));
            Attributes.Add(new UICombo<BinderType>("Binder Type", Binder.BType));
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

        public override void Draw(string parentId)
        {
            string id = parentId + "/Binder" + Idx;
            if (ImGui.CollapsingHeader("Binder " + Idx + "(" + Binder.BType.Value + ")" + id))
            {
                DrawAttrs(id);
                //=====================
                if (Prop != null)
                {
                    Prop.Draw(id);
                }
                //====================
                if (Data != null)
                {
                    Data.Draw(id);
                }
            }
        }
    }
}
