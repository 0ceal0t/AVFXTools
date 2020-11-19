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
    public class UIParticle
    {
        public ParticleItem Item;
        public AVFXParticle Particle;
        // =======================
        public int LoopStart;
        public int LoopEnd;
        // TODO: particle type
        public static readonly string[] RotationDirectionBaseOptions = Enum.GetNames(typeof(RotationDirectionBase));
        public int RotationDirectionBaseIdx;
        public static readonly string[] RotationOrderOptions = Enum.GetNames(typeof(RotationOrder));
        public int RotationOrderIdx;
        public static readonly string[] CoordComputerOptions = Enum.GetNames(typeof(CoordComputeOrder));
        public int CoordComputeIdx;
        public static readonly string[] DrawModeOptions = Enum.GetNames(typeof(DrawMode));
        public int DrawModeIdx;
        public static readonly string[] CullingTypeOptions = Enum.GetNames(typeof(CullingType));
        public int CullingTypeIdx;
        public static readonly string[] EnvLightOptions = Enum.GetNames(typeof(EnvLight));
        public int EnvLightIdx;
        public static readonly string[] DirLightOptions = Enum.GetNames(typeof(DirLight));
        public int DirLightIdx;
        public static readonly string[] UVPrecisionOptions = Enum.GetNames(typeof(UVPrecision));
        public int UVPrecisionIdx;
        public int DrawPriority;
        public bool IsDepthTest;
        public bool IsDepthWrite;
        public bool IsSoftParticle;
        public int CollisionType;
        public bool Bs11;
        public bool IsApplyToneMap;
        public bool IsApplyFog;
        public bool ClipNearEnable;
        public bool ClipFarEnable;
        public float ClipNearStart;
        public float ClipNearEnd;
        public float ClipFarStart;
        public float ClipFarEnd;
        public static readonly string[] ClipBasePointOptions = Enum.GetNames(typeof(ClipBasePoint));
        public int ClipBasePointIdx;
        // TODO: UV Set Count
        public int ApplyRateEnvironment;
        public int ApplyRateDirectional;
        public int ApplyRateLightBuffer;
        public bool DOTy;
        public float DepthOffset;
        public bool SimpleAnimEnable;
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
        public UIParticleDataBase Data;
        //========================
        public UITextureColor1 TC1;
        public UITextureColor2 TC2;
        public UITextureColor2 TC3;
        public UITextureColor2 TC4;
        public UITextureNormal TN;
        public UITextureReflection TR;
        public UITextureDistortion TD;
        public UITexturePalette TP;

        public UIParticle(ParticleItem item)
        {
            Item = item;
            Particle = item.Particle;
            //==========================
            LoopStart = Particle.LoopStart.Value;
            LoopEnd = Particle.LoopEnd.Value;
            RotationDirectionBaseIdx = Array.IndexOf(RotationDirectionBaseOptions, Particle.RotationDirectionBase.Value);
            RotationOrderIdx = Array.IndexOf(RotationOrderOptions, Particle.RotationOrder.Value);
            CoordComputeIdx = Array.IndexOf(CoordComputerOptions, Particle.CoordComputeOrder.Value);
            DrawModeIdx = Array.IndexOf(DrawModeOptions, Particle.DrawMode.Value);
            CullingTypeIdx = Array.IndexOf(CullingTypeOptions, Particle.CullingType.Value);
            EnvLightIdx = Array.IndexOf(EnvLightOptions, Particle.EnvLight.Value);
            DirLightIdx = Array.IndexOf(DirLightOptions, Particle.DirLight.Value);
            UVPrecisionIdx = Array.IndexOf(UVPrecisionOptions, Particle.UvPrecision.Value);
            DrawPriority = Particle.DrawPriority.Value;
            IsDepthTest = (Particle.IsDepthTest.Value == true);
            IsDepthWrite = (Particle.IsDepthWrite.Value == true);
            IsSoftParticle = (Particle.IsSoftParticle.Value == true);
            CollisionType = Particle.CollisionType.Value;
            Bs11 = (Particle.Bs11.Value == true);
            IsApplyToneMap = (Particle.IsApplyToneMap.Value == true);
            IsApplyFog = (Particle.IsApplyFog.Value == true);
            ClipNearEnable = (Particle.ClipNearEnable.Value == true);
            ClipFarEnable = (Particle.ClipFarEnable.Value == true);
            ClipNearStart = Particle.ClipNearStart.Value;
            ClipNearEnd = Particle.ClipNearEnd.Value;
            ClipFarStart = Particle.ClipFarStart.Value;
            ClipFarEnd = Particle.ClipFarEnd.Value;
            ClipBasePointIdx = Array.IndexOf(ClipBasePointOptions, Particle.ClipBasePoint.Value);
            ApplyRateEnvironment = Particle.ApplyRateEnvironment.Value;
            ApplyRateDirectional = Particle.ApplyRateDirectional.Value;
            ApplyRateLightBuffer = Particle.ApplyRateLightBuffer.Value;
            DOTy = (Particle.DOTy.Value == true);
            DepthOffset = Particle.DepthOffset.Value;
            SimpleAnimEnable = (Particle.SimpleAnimEnable.Value == true);
            //==============================
            Life = new UILife(item);
            Simple = new UIParticleSimple(item);
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
                UVSets[i] = new UIParticleUVSet(Item, Particle.UVSets[i]);
            }
            //===============================
            switch (Particle.ParticleType.Value)
            {
                case "Model":
                    Data = new UIParticleDataModel((AVFXParticleDataModel)Particle.Data, Item);
                    break;
                case "LightModel":
                    Data = new UIParticleDataLightModel((AVFXParticleDataLightModel)Particle.Data, Item);
                    break;
                case "Powder":
                    Data = new UIParticleDataPowder((AVFXParticleDataPowder)Particle.Data, Item);
                    break;
            }
            //============================
            TC1 = new UITextureColor1(Particle.TC1, item);
            TC2 = new UITextureColor2(Particle.TC2, item, "Texture Color 2");
            TC3 = new UITextureColor2(Particle.TC3, item, "Texture Color 3");
            TC4 = new UITextureColor2(Particle.TC4, item, "Texture Color 4");

            TN = new UITextureNormal(Particle.TN, item);
            TR = new UITextureReflection(Particle.TR, item);
            TD = new UITextureDistortion(Particle.TD, item);
            TP = new UITexturePalette(Particle.TP, item);
        }

        public void Draw(string id)
        {
            if (ImGui.TreeNode("Parameters" + id))
            {
                if (ImGui.DragInt("Loop Start" + id, ref LoopStart, 1, 0))
                {
                    Particle.LoopStart.GiveValue(LoopStart);
                }
                if (ImGui.DragInt("Loop End" + id, ref LoopEnd, 1, 0))
                {
                    Particle.LoopEnd.GiveValue(LoopEnd);
                }
                if (UIUtils.EnumComboBox("Rotation Direction Base" + id, RotationDirectionBaseOptions, ref RotationDirectionBaseIdx))
                {
                    Particle.RotationDirectionBase.GiveValue(RotationDirectionBaseOptions[RotationDirectionBaseIdx]);
                }
                if (UIUtils.EnumComboBox("Rotation Compute Order" + id, RotationOrderOptions, ref RotationOrderIdx))
                {
                    Particle.RotationOrder.GiveValue(RotationOrderOptions[RotationOrderIdx]);
                }
                if (UIUtils.EnumComboBox("Coord Compute Order" + id, CoordComputerOptions, ref CoordComputeIdx))
                {
                    Particle.CoordComputeOrder.GiveValue(CoordComputerOptions[CoordComputeIdx]);
                }
                if (UIUtils.EnumComboBox("Draw Mode" + id, DrawModeOptions, ref DrawModeIdx))
                {
                    Particle.DrawMode.GiveValue(DrawModeOptions[DrawModeIdx]);
                }
                if (UIUtils.EnumComboBox("Culling Type" + id, CullingTypeOptions, ref CullingTypeIdx))
                {
                    Particle.CullingType.GiveValue(CullingTypeOptions[CullingTypeIdx]);
                }
                if (UIUtils.EnumComboBox("Env Light" + id, EnvLightOptions, ref EnvLightIdx))
                {
                    Particle.EnvLight.GiveValue(EnvLightOptions[EnvLightIdx]);
                }
                if (UIUtils.EnumComboBox("Dir Light" + id, DirLightOptions, ref DirLightIdx))
                {
                    Particle.DirLight.GiveValue(DirLightOptions[DirLightIdx]);
                }
                if (UIUtils.EnumComboBox("UV Precision" + id, UVPrecisionOptions, ref UVPrecisionIdx))
                {
                    Particle.UvPrecision.GiveValue(UVPrecisionOptions[UVPrecisionIdx]);
                }
                if (ImGui.DragInt("Draw Priority" + id, ref DrawPriority, 1, 0))
                {
                    Particle.DrawPriority.GiveValue(DrawPriority);
                }
                if (ImGui.Checkbox("Depth Test" + id, ref IsDepthTest))
                {
                    Particle.IsDepthTest.GiveValue(IsDepthTest);
                }
                if (ImGui.Checkbox("Depth Write" + id, ref IsDepthWrite))
                {
                    Particle.IsDepthWrite.GiveValue(IsDepthWrite);
                }
                if (ImGui.Checkbox("Soft Particle" + id, ref IsSoftParticle))
                {
                    Particle.IsSoftParticle.GiveValue(IsSoftParticle);
                }
                if (ImGui.DragInt("Collision Type" + id, ref CollisionType, 1, -1))
                {
                    Particle.CollisionType.GiveValue(CollisionType);
                }
                if (ImGui.Checkbox("Bs11" + id, ref Bs11))
                {
                    Particle.Bs11.GiveValue(Bs11);
                }
                if (ImGui.Checkbox("Apply Tone Map" + id, ref IsApplyToneMap))
                {
                    Particle.IsApplyToneMap.GiveValue(IsApplyToneMap);
                }
                if (ImGui.Checkbox("Apply Fog" + id, ref IsApplyFog))
                {
                    Particle.IsApplyFog.GiveValue(IsApplyFog);
                }
                if (ImGui.Checkbox("Clip Near" + id, ref ClipNearEnable))
                {
                    Particle.ClipNearEnable.GiveValue(ClipNearEnable);
                }
                if (ImGui.Checkbox("Clip Far" + id, ref ClipFarEnable))
                {
                    Particle.ClipFarEnable.GiveValue(ClipFarEnable);
                }
                if (ImGui.DragFloat("Clip Near Start" + id, ref ClipNearStart, 1, 0))
                {
                    Particle.ClipNearStart.GiveValue(ClipNearStart);
                }
                if (ImGui.DragFloat("Clip Near End" + id, ref ClipNearEnd, 1, 0))
                {
                    Particle.ClipNearEnd.GiveValue(ClipNearEnd);
                }
                if (ImGui.DragFloat("Clip Far Start" + id, ref ClipFarStart, 1, 0))
                {
                    Particle.ClipFarStart.GiveValue(ClipFarStart);
                }
                if (ImGui.DragFloat("Clip Far End" + id, ref ClipFarEnd, 1, 0))
                {
                    Particle.ClipFarEnd.GiveValue(ClipFarEnd);
                }
                if (UIUtils.EnumComboBox("Clip Base Point" + id, ClipBasePointOptions, ref ClipBasePointIdx))
                {
                    Particle.ClipBasePoint.GiveValue(ClipBasePointOptions[ClipBasePointIdx]);
                }
                if (ImGui.DragInt("Apply Rate Environment" + id, ref ApplyRateEnvironment, 1, 0))
                {
                    Particle.ApplyRateEnvironment.GiveValue(ApplyRateEnvironment);
                }
                if (ImGui.DragInt("Apply Rate Directional" + id, ref ApplyRateDirectional, 1, 0))
                {
                    Particle.ApplyRateDirectional.GiveValue(ApplyRateDirectional);
                }
                if (ImGui.DragInt("Apply Rate Light Buffer" + id, ref ApplyRateLightBuffer, 1, 0))
                {
                    Particle.ApplyRateLightBuffer.GiveValue(ApplyRateLightBuffer);
                }
                if (ImGui.Checkbox("DOTy" + id, ref DOTy))
                {
                    Particle.DOTy.GiveValue(DOTy);
                }
                if (ImGui.DragFloat("Depth Offset" + id, ref DepthOffset, 1, 0))
                {
                    Particle.DepthOffset.GiveValue(DepthOffset);
                }
                if (ImGui.Checkbox("Simple Animations" + id, ref SimpleAnimEnable))
                {
                    Particle.SimpleAnimEnable.GiveValue(SimpleAnimEnable);
                }
                ImGui.TreePop();
            }
            //======================
            if (ImGui.TreeNode("Animation" + id)) {
                Life.Draw(id + "-life");
                Simple.Draw(id + "-simple");
                Gravity.Draw(id + "-grav");
                AirResistance.Draw(id + "-ar");
                Scale.Draw(id + "-scale");
                Rotation.Draw(id + "-rotation");
                Position.Draw(id + "-position");
                Color.Draw(id + "-color");
                ImGui.TreePop();
            }
            //=====================
            if (ImGui.TreeNode("UV Sets (" + UVSets.Length + ")" + id))
            {
                for (int i = 0; i < UVSets.Length; i++)
                {
                    UVSets[i].Draw(id + "-uv" + i, i);
                }

                ImGui.TreePop();
            }
            //====================
            if(Data != null)
            {
                Data.Draw(id + "-data");
            }
            //===================
            TC1.Draw(id + "-tc1");
            TC2.Draw(id + "-tc2");
            TC3.Draw(id + "-tc3");
            TC4.Draw(id + "-tc4");

            TN.Draw(id + "-tn");
            TR.Draw(id + "-tr");
            TD.Draw(id + "-td");
            TP.Draw(id + "-tp");
        }
    }
}
