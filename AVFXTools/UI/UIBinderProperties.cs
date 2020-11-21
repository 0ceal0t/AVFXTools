using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UIBinderProperties
    {
        public AVFXBinderProperty Prop;
        //===================
        public static readonly string[] BindPointOptions = Enum.GetNames(typeof(BindPoint));
        public int BindPointIdx;
        public static readonly string[] BindPointTargetOptions = Enum.GetNames(typeof(BindTargetPoint));
        public int BindPointTargetIdx;
        // TODO: Name
        public int BindPointId;
        public int GenerateDelay;
        public int CoordUpdateFrame;
        public bool RingEnable;
        public int RingProgressTime;
        public Vector3 RingPosition;
        public float RingRadius;
        public UICurve3Axis Position;

        public UIBinderProperties(AVFXBinderProperty prop)
        {
            Prop = prop;
            //====================
            BindPointIdx = Array.IndexOf(BindPointOptions, Prop.BindPointType.Value);
            BindPointTargetIdx = Array.IndexOf(BindPointTargetOptions, Prop.BindTargetPointType.Value);
            //
            BindPointId = Prop.BindPointId.Value;
            GenerateDelay = Prop.GenerateDelay.Value;
            CoordUpdateFrame = Prop.CoordUpdateFrame.Value;
            RingEnable = (Prop.RingEnable.Value == true);
            RingProgressTime = Prop.RingProgressTime.Value;
            RingPosition = new Vector3(Prop.RingPositionX.Value, Prop.RingPositionY.Value, Prop.RingPositionZ.Value);
            RingRadius = Prop.RingRadius.Value;
            Position = new UICurve3Axis(Prop.Position, "Position");
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Properties" + id))
            {
                if (UIUtils.EnumComboBox("Bind Point Type" + id, BindPointOptions, ref BindPointIdx))
                {
                    Prop.BindPointType.GiveValue(BindPointOptions[BindPointIdx]);
                }
                if (UIUtils.EnumComboBox("Bind Target Point Type" + id, BindPointTargetOptions, ref BindPointTargetIdx))
                {
                    Prop.BindTargetPointType.GiveValue(BindPointTargetOptions[BindPointTargetIdx]);
                }
                //
                if (ImGui.InputInt("Bind Point Id" + id, ref BindPointId))
                {
                    Prop.BindPointId.GiveValue(BindPointId);
                }
                if (ImGui.InputInt("Generate Delay" + id, ref GenerateDelay))
                {
                    Prop.GenerateDelay.GiveValue(GenerateDelay);
                }
                if (ImGui.InputInt("Coord Update Frame" + id, ref CoordUpdateFrame))
                {
                    Prop.CoordUpdateFrame.GiveValue(CoordUpdateFrame);
                }
                if (ImGui.Checkbox("Enable Ring" + id, ref RingEnable))
                {
                    Prop.RingEnable.GiveValue(RingEnable);
                }
                if (ImGui.InputInt("Ring Progress Time" + id, ref RingProgressTime))
                {
                    Prop.RingProgressTime.GiveValue(RingProgressTime);
                }
                if (ImGui.InputFloat3("Ring Position" + id, ref RingPosition))
                {
                    Prop.RingPositionX.GiveValue(RingPosition.X);
                    Prop.RingPositionY.GiveValue(RingPosition.Y);
                    Prop.RingPositionZ.GiveValue(RingPosition.Z);
                }
                if (ImGui.InputFloat("Ring Radius" + id, ref RingRadius))
                {
                    Prop.RingRadius.GiveValue(RingRadius);
                }
                Position.Draw(id + "-position");
                ImGui.TreePop();
            }
        }
    }
}
