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
    public class UIEffector : UIBase
    {
        public AVFXEffector Effector;
        public int Idx;
        //========================
        // TODO: effector type
        //=======================
        public UIBase Data;

        public UIEffector(AVFXEffector effector)
        {
            Effector = effector;
            //======================
            Attributes.Add(new UICombo<RotationOrder>("Rotation Order", Effector.RotationOrder));
            Attributes.Add(new UICombo<CoordComputeOrder>("Coordinate Compute Order", Effector.CoordComputeOrder));
            Attributes.Add(new UICheckbox("Affect Other VFX", Effector.AffectOtherVfx));
            Attributes.Add(new UICheckbox("Affect Game", Effector.AffectGame));
            Attributes.Add(new UIInt("Loop Start", Effector.LoopPointStart));
            Attributes.Add(new UIInt("Loop End", Effector.LoopPointEnd));
            //=======================
            switch (Effector.EffectorVariety.Value)
            {
                case EffectorType.PointLight:
                    Data = new UIEffectorDataPointLight((AVFXEffectorDataPointLight)Effector.Data);
                    break;
                case EffectorType.RadialBlur:
                    Data = new UIEffectorDataRadialBlur((AVFXEffectorDataRadialBlur)Effector.Data);
                    break;
                case EffectorType.CameraQuake:
                    Data = new UIEffectorDataCameraQuake((AVFXEffectorDataCameraQuake)Effector.Data);
                    break;
            }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Effector" + Idx;
            if (ImGui.CollapsingHeader("Effector " + Idx + "(" + Effector.EffectorVariety.stringValue() + ")" + id))
            {
                if (UIUtils.RemoveButton("Delete" + id))
                {
                    // TODO
                }
                //==========================
                if (ImGui.TreeNode("Parameters" + id))
                {
                    DrawAttrs(id);
                    ImGui.TreePop();
                }
                //=============================
                if (Data != null)
                {
                    Data.Draw(id);
                }
            }
        }
    }
}
