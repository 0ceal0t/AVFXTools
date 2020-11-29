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
    public class UIEmitter : UIBase
    {
        public AVFXEmitter Emitter;
        public int Idx;
        //========================
        // TODO: sound
        // TODO: emitter type
        // TODO: particle + emitter count
        //=======================
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
        public UIBase Data;

        public UIEmitter(AVFXEmitter emitter)
        {
            Emitter = emitter;
            //======================
            Attributes.Add(new UIInt("Sound Index", Emitter.SoundNumber));
            Attributes.Add(new UIInt("Loop Start", Emitter.LoopStart));
            Attributes.Add(new UIInt("Loop End", Emitter.LoopEnd));
            Attributes.Add(new UIInt("Child Limit", Emitter.ChildLimit));
            Attributes.Add(new UIInt("Effector Index", Emitter.EffectorIdx));
            Attributes.Add(new UICheckbox("Any Direction", Emitter.AnyDirection));
            Attributes.Add(new UICombo<RotationDirectionBase>("Rotation Direction Base", Emitter.RotationDirectionBase));
            Attributes.Add(new UICombo<CoordComputeOrder>("Coordinate Compute Order", Emitter.CoordComputeOrder));
            Attributes.Add(new UICombo<RotationOrder>("Rotation Order", Emitter.RotationOrder));
            // ==========================
            Life = new UILife(Emitter.Life);
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
                    Particles[i] = new UIEmitterItem(lastOne.Items[i], true);
                }
            }
            else { Particles = new UIEmitterItem[0]; }
            //============================
            if (Emitter.ItEms.Count > 0)
            {
                var lastOne = Emitter.ItEms[Emitter.ItEms.Count - 1];
                Emitters = new UIEmitterItem[lastOne.Items.Count];
                for (int i = 0; i < Emitters.Length; i++)
                {
                    Emitters[i] = new UIEmitterItem(lastOne.Items[i], false);
                }
            }
            else { Emitters = new UIEmitterItem[0]; }
            //=======================
            switch (Emitter.EmitterType.Value)
            {
                case "SphereModel":
                    Data = new UIEmitterDataSphereModel((AVFXEmitterDataSphereModel)Emitter.Data);
                    break;
                case "CylinderModel":
                    Data = new UIEmitterDataCylinderModel((AVFXEmitterDataCylinderModel)Emitter.Data);
                    break;
                case "Model":
                    Data = new UIEmitterDataModel((AVFXEmitterDataModel)Emitter.Data);
                    break;
            }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Emitter" + Idx;
            if (ImGui.CollapsingHeader("Emitter " + Idx + "(" + Emitter.EmitterType.Value + ")" + id))
            {
                if (ImGui.TreeNode("Parameters" + id))
                {
                    DrawAttrs(id);
                    ImGui.TreePop();
                }
                //======================
                if (ImGui.TreeNode("Animation" + id))
                {
                    Life.Draw(id);
                    CreateCount.Draw(id);
                    CreateInterval.Draw(id);
                    AirResistance.Draw(id);
                    Scale.Draw(id);
                    Rotation.Draw(id);
                    Position.Draw(id);
                    Color.Draw(id);
                }
                //=======================
                if (ImGui.TreeNode("Particles (" + Particles.Length + ")" + id))
                {
                    int pIdx = 0;
                    foreach (var particle in Particles)
                    {
                        particle.Idx = pIdx;
                        particle.Draw(id);
                        pIdx++;
                    }
                    ImGui.TreePop();
                }
                //=======================
                if (ImGui.TreeNode("Emitters (" + Emitters.Length + ")" + id))
                {
                    int eIdx = 0;
                    foreach (var emitter in Emitters)
                    {
                        emitter.Idx = eIdx;
                        emitter.Draw(id);
                        eIdx++;
                    }
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
