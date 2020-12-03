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
        List<UIBase> Animation = new List<UIBase>();
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
            Animation.Add(new UILife(Emitter.Life));
            Animation.Add(new UICurve(Emitter.CreateCount, "Create Count"));
            Animation.Add(new UICurve(Emitter.CreateInterval, "Create Interval"));
            Animation.Add(new UICurve(Emitter.CreateIntervalRandom, "Create Interval Random"));
            Animation.Add(new UICurve(Emitter.Gravity, "Gravity"));
            Animation.Add(new UICurve(Emitter.GravityRandom, "Gravity Random"));
            Animation.Add(new UICurve(Emitter.AirResistance, "Air Resistance"));
            Animation.Add(new UICurve(Emitter.AirResistanceRandom, "Air Resistance Random"));
            Animation.Add(new UICurveColor(Emitter.Color, "Color"));
            Animation.Add(new UICurve3Axis(Emitter.Position, "Position"));
            Animation.Add(new UICurve3Axis(Emitter.Rotation, "Rotation"));
            Animation.Add(new UICurve3Axis(Emitter.Scale, "Scale"));
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
            switch (Emitter.EmitterVariety.Value)
            {
                case EmitterType.SphereModel:
                    Data = new UIEmitterDataSphereModel((AVFXEmitterDataSphereModel)Emitter.Data);
                    break;
                case EmitterType.CylinderModel:
                    Data = new UIEmitterDataCylinderModel((AVFXEmitterDataCylinderModel)Emitter.Data);
                    break;
                case EmitterType.Model:
                    Data = new UIEmitterDataModel((AVFXEmitterDataModel)Emitter.Data);
                    break;
            }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Emitter" + Idx;
            if (ImGui.CollapsingHeader("Emitter " + Idx + "(" + Emitter.EmitterVariety.stringValue() + ")" + id))
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
                //======================
                if (ImGui.TreeNode("Animation" + id))
                {
                    DrawList(Animation, id);
                    ImGui.TreePop();
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
                    if (ImGui.Button("+ Particle" + id))
                    {
                        // TODO
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
                    if (ImGui.Button("+ Emitter" + id))
                    {
                        // TODO
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
