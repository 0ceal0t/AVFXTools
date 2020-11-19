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
    public class UIEmitter
    {
        public EmitterItem Item;
        public AVFXEmitter Emitter;
        //========================
        // TODO: sound
        public int SoundNumber;
        public int LoopStart;
        public int LoopEnd;
        public int ChildLimit;
        public int EffectorIdx;
        public bool AnyDirection;
        // TODO: emitter type
        public static readonly string[] RotationDirectionBaseOptions = Enum.GetNames(typeof(RotationDirectionBase));
        public int RotationDirectionBaseIdx;
        public static readonly string[] CoordComputeOrderOptions = Enum.GetNames(typeof(CoordComputeOrder));
        public int CoordComputeOrderIdx;
        public static readonly string[] RotationOrderOptions = Enum.GetNames(typeof(RotationOrder));
        public int RotationOrderIdx;
        // TODO: particle + emitter count
        public UILife Life;
        public UICurve CreateCount;
        public UICurve CreateInterval;
        public UICurve AirResistance;
        public UICurveColor Color;
        public UICurve3Axis Position;
        public UICurve3Axis Rotation;
        public UICurve3Axis Scale;
        //========================
        public UIEmitterItem[] Particles;
        public UIEmitterItem[] Emitters;
        //========================
        public UIEmitterDataBase Data;

        public UIEmitter(EmitterItem item)
        {
            Item = item;
            Emitter = item.Emitter;
            //======================
            SoundNumber = Emitter.SoundNumber.Value;
            LoopStart = Emitter.LoopStart.Value;
            LoopEnd = Emitter.LoopEnd.Value;
            ChildLimit = Emitter.ChildLimit.Value;
            EffectorIdx = Emitter.EffectorIdx.Value;
            AnyDirection = (Emitter.AnyDirection.Value == true);
            RotationDirectionBaseIdx = Array.IndexOf(RotationDirectionBaseOptions, Emitter.RotationDirectionBase.Value);
            CoordComputeOrderIdx = Array.IndexOf(CoordComputeOrderOptions, Emitter.CoordComputeOrder.Value);
            RotationOrderIdx = Array.IndexOf(RotationOrderOptions, Emitter.RotationOrder.Value);
            Life = new UILife(Item);
            //========================
            CreateCount = new UICurve(Emitter.CreateCount, "Create Count");
            CreateInterval = new UICurve(Emitter.CreateInterval, "Create Interval");
            AirResistance = new UICurve(Emitter.AirResistance, "Air Resistance");
            Color = new UICurveColor(Emitter.Color, "Color");
            Position = new UICurve3Axis(Emitter.Position, "Position");
            Rotation = new UICurve3Axis(Emitter.Rotation, "Rotation");
            Scale = new UICurve3Axis(Emitter.Scale, "Scale");
            //========================
            if(Emitter.ItPrs.Count > 0)
            {
                var lastOne = Emitter.ItPrs[Emitter.ItPrs.Count - 1];
                Particles = new UIEmitterItem[lastOne.Items.Count];
                for(int i = 0; i < Particles.Length; i++)
                {
                    Particles[i] = new UIEmitterItem(Item, lastOne.Items[i], true);
                }
            }
            else { Particles = new UIEmitterItem[0]; }

            if (Emitter.ItEms.Count > 0)
            {
                var lastOne = Emitter.ItEms[Emitter.ItEms.Count - 1];
                Emitters = new UIEmitterItem[lastOne.Items.Count];
                for (int i = 0; i < Emitters.Length; i++)
                {
                    Emitters[i] = new UIEmitterItem(Item, lastOne.Items[i], false);
                }
            }
            else { Emitters = new UIEmitterItem[0]; }
            //=======================
            switch (Emitter.EmitterType.Value)
            {
                case "SphereModel":
                    Data = new UIEmitterDataSphereModel((AVFXEmitterDataSphereModel)Emitter.Data, Item);
                    break;
                case "CylinderModel":
                    Data = new UIEmitterDataCylinderModel((AVFXEmitterDataCylinderModel)Emitter.Data, Item);
                    break;
            }
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Parameters" + id))
            {
                if (ImGui.DragInt("Sound Number" + id, ref SoundNumber, 1, -1))
                {
                    Emitter.SoundNumber.GiveValue(SoundNumber);
                }
                if (ImGui.DragInt("Loop Start" + id, ref LoopStart, 1, 0))
                {
                    Emitter.LoopStart.GiveValue(LoopStart);
                }
                if (ImGui.DragInt("Loop End" + id, ref LoopEnd, 1, 0))
                {
                    Emitter.LoopEnd.GiveValue(LoopEnd);
                }
                if (ImGui.DragInt("Child Limit" + id, ref ChildLimit, 1, -1))
                {
                    Emitter.ChildLimit.GiveValue(ChildLimit);
                }
                if (ImGui.DragInt("Effector Index" + id, ref EffectorIdx, 1, -1))
                {
                    Emitter.EffectorIdx.GiveValue(EffectorIdx);
                }
                if (ImGui.Checkbox("Any Direction" + id, ref AnyDirection))
                {
                    Emitter.AnyDirection.GiveValue(AnyDirection);
                }
                if (UIUtils.EnumComboBox("Rotation Direction Base" + id, RotationDirectionBaseOptions, ref RotationDirectionBaseIdx))
                {
                    Emitter.RotationDirectionBase.GiveValue(RotationDirectionBaseOptions[RotationDirectionBaseIdx]);
                }
                if (UIUtils.EnumComboBox("Coordinate Computer Order" + id, CoordComputeOrderOptions, ref CoordComputeOrderIdx))
                {
                    Emitter.CoordComputeOrder.GiveValue(CoordComputeOrderOptions[CoordComputeOrderIdx]);
                }
                if (UIUtils.EnumComboBox("Rotation Order" + id, RotationOrderOptions, ref RotationOrderIdx))
                {
                    Emitter.RotationOrder.GiveValue(RotationOrderOptions[RotationOrderIdx]);
                }
                ImGui.TreePop();
            }
            //======================
            if (ImGui.TreeNode("Animation" + id))
            {
                Life.Draw(id + "-life");
                CreateCount.Draw(id + "-createcount");
                CreateInterval.Draw(id + "-createinterval");
                AirResistance.Draw(id + "-ar");
                Scale.Draw(id + "-scale");
                Rotation.Draw(id + "-rotation");
                Position.Draw(id + "-position");
                Color.Draw(id + "-color");
                ImGui.TreePop();
            }
            //===========================
            if (ImGui.TreeNode("Particles (" + Particles.Length + ")" + id))
            {
                for (int i = 0; i < Particles.Length; i++)
                {
                    Particles[i].Draw(id + "-particle" + i, i);
                }
                ImGui.TreePop();
            }
            if (ImGui.TreeNode("Emitters (" + Emitters.Length + ")" + id))
            {
                for (int i = 0; i < Emitters.Length; i++)
                {
                    Emitters[i].Draw(id + "-particle" + i, i);
                }
                ImGui.TreePop();
            }
            //=============================
            if (Data != null)
            {
                Data.Draw(id + "-data");
            }
        }
    }
}
