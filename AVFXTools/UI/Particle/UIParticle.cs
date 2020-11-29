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
    public class UIParticle : UIBase
    {
        public AVFXParticle Particle;
        public int Idx;
        // =======================
        // TODO: particle type
        // TODO: UV Set Count
        // TODO: simple animations enabled
        //==========================
        public UILife Life;
        public UIParticleSimple Simple;
        public UICurve Gravity;
        public UICurve AirResistance;
        public UICurve3Axis Scale;
        public UICurve3Axis Rotation;
        public UICurve3Axis Position;
        public UICurveColor Color;
        //==========================
        public UIParticleUVSet[] UVSets;
        //==========================
        public UIBase Data;
        //========================
        public UITextureColor1 TC1;
        public UITextureColor2 TC2;
        public UITextureColor2 TC3;
        public UITextureColor2 TC4;
        public UITextureNormal TN;
        public UITextureReflection TR;
        public UITextureDistortion TD;
        public UITexturePalette TP;

        public UIParticle(AVFXParticle particle)
        {
            Particle = particle;
            //==========================
            Attributes.Add(new UIInt("Loop Start", Particle.LoopStart));
            Attributes.Add(new UIInt("Loop End", Particle.LoopEnd));
            Attributes.Add(new UICombo<RotationDirectionBase>("Rotation Direction Base", Particle.RotationDirectionBase));
            Attributes.Add(new UICombo<RotationOrder>("Rotation Compute Order", Particle.RotationOrder));
            Attributes.Add(new UICombo<CoordComputeOrder>("Coord Compute Order", Particle.CoordComputeOrder));
            Attributes.Add(new UICombo<DrawMode>("Draw Mode", Particle.DrawMode));
            Attributes.Add(new UICombo<CullingType>("Culling Type", Particle.CullingType));
            Attributes.Add(new UICombo<EnvLight>("Enviornmental Light", Particle.EnvLight));
            Attributes.Add(new UICombo<DirLight>("Directional Light", Particle.DirLight));
            Attributes.Add(new UICombo<UVPrecision>("UV Precision", Particle.UvPrecision));
            Attributes.Add(new UIInt("Draw Priority", Particle.DrawPriority));
            Attributes.Add(new UICheckbox("Depth Test", Particle.IsDepthTest));
            Attributes.Add(new UICheckbox("Depth Write", Particle.IsDepthWrite));
            Attributes.Add(new UICheckbox("Soft Particle", Particle.IsSoftParticle));
            Attributes.Add(new UIInt("Collision Type", Particle.CollisionType));
            Attributes.Add(new UICheckbox("BS11", Particle.Bs11));
            Attributes.Add(new UICheckbox("Apply Tone Map", Particle.IsApplyToneMap));
            Attributes.Add(new UICheckbox("Apply Fog", Particle.IsApplyFog));
            Attributes.Add(new UICheckbox("Clip Near", Particle.ClipNearEnable));
            Attributes.Add(new UICheckbox("Clip Far", Particle.ClipFarEnable));
            Attributes.Add(new UIFloat2("Clip Near", Particle.ClipNearStart, Particle.ClipNearEnd));
            Attributes.Add(new UIFloat2("Clip Far", Particle.ClipFarStart, Particle.ClipFarEnd));
            Attributes.Add(new UICombo<ClipBasePoint>("Clip Base Point", Particle.ClipBasePoint));
            Attributes.Add(new UIInt("Apply Rate Env", Particle.ApplyRateEnvironment));
            Attributes.Add(new UIInt("Apply Rate Directional", Particle.ApplyRateDirectional));
            Attributes.Add(new UIInt("Apply Rate Light Buffer", Particle.ApplyRateLightBuffer));
            Attributes.Add(new UICheckbox("DOTy", Particle.DOTy));
            Attributes.Add(new UIFloat("Apply Rate Light Buffer", Particle.DepthOffset));
            //==============================
            Life = new UILife(Particle.Life);
            Simple = new UIParticleSimple(Particle.Simple);
            Gravity = new UICurve(Particle.Gravity, "Gravity");
            AirResistance = new UICurve(Particle.AirResistance, "Air Resistance");
            Scale = new UICurve3Axis(Particle.Scale, "Scale");
            Rotation = new UICurve3Axis(Particle.Rotation, "Rotation");
            Position = new UICurve3Axis(Particle.Position, "Position");
            Color = new UICurveColor(Particle.Color, "Color");
            //===============================
            UVSets = new UIParticleUVSet[Particle.UVSets.Count];
            for(int i = 0; i < UVSets.Length; i++)
            {
                UVSets[i] = new UIParticleUVSet(Particle.UVSets[i]);
            }
            //===============================
            switch (Particle.ParticleType.Value)
            {
                case "Model":
                    Data = new UIParticleDataModel((AVFXParticleDataModel)Particle.Data);
                    break;
                case "LightModel":
                    Data = new UIParticleDataLightModel((AVFXParticleDataLightModel)Particle.Data);
                    break;
                case "Powder":
                    Data = new UIParticleDataPowder((AVFXParticleDataPowder)Particle.Data);
                    break;
                //
                case "Decal":
                    Data = new UIParticleDataDecal((AVFXParticleDataDecal)Particle.Data);
                    break;
                case "DecalRing":
                    Data = new UIParticleDataDecalRing((AVFXParticleDataDecalRing)Particle.Data);
                    break;
                case "Disc":
                    Data = new UIParticleDataDisc((AVFXParticleDataDisc)Particle.Data);
                    break;
                case "Laser":
                    Data = new UIParticleDataLaser((AVFXParticleDataLaser)Particle.Data);
                    break;
                case "Polygon":
                    Data = new UIParticleDataPolygon((AVFXParticleDataPolygon)Particle.Data);
                    break;
                case "Polyline":
                    Data = new UIParticleDataPolyline((AVFXParticleDataPolyline)Particle.Data);
                    break;
                case "Windmill":
                    Data = new UIParticleDataWindmill((AVFXParticleDataWindmill)Particle.Data);
                    break;
            }
            //============================
            TC1 = new UITextureColor1(Particle.TC1);
            TC2 = new UITextureColor2(Particle.TC2, "Texture Color 2");
            TC3 = new UITextureColor2(Particle.TC3, "Texture Color 3");
            TC4 = new UITextureColor2(Particle.TC4, "Texture Color 4");
            TN = new UITextureNormal(Particle.TN);
            TR = new UITextureReflection(Particle.TR);
            TD = new UITextureDistortion(Particle.TD);
            TP = new UITexturePalette(Particle.TP);
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/Particle" + Idx;
            if (ImGui.CollapsingHeader("Particle " + Idx + "(" + Particle.ParticleType.Value + ")" + id))
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
                    Simple.Draw(id);
                    Gravity.Draw(id);
                    AirResistance.Draw(id);
                    Scale.Draw(id);
                    Rotation.Draw(id);
                    Position.Draw(id);
                    Color.Draw(id);
                    ImGui.TreePop();
                }
                //=====================
                if (ImGui.TreeNode("UV Sets (" + UVSets.Length + ")" + id))
                {
                    int uvIdx = 0;
                    foreach (var uv in UVSets)
                    {
                        uv.Idx = uvIdx;
                        uv.Draw(id);
                        uvIdx++;
                    }
                    ImGui.TreePop();
                }
                //====================
                if(Data != null)
                {
                    Data.Draw(id);
                }
                //===================
                TC1.Draw(id);
                TC2.Draw(id);
                TC3.Draw(id);
                TC4.Draw(id);
                TN.Draw(id);
                TR.Draw(id);
                TD.Draw(id);
                TP.Draw(id);
            }
        }
    }
}
