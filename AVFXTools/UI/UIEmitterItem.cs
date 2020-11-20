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
    public class UIEmitterItem
    {
        public AVFXEmitterIterationItem Iteration;
        public bool IsParticle;
        //=============================
        public bool Enabled;
        public int TargetIdx;
        public int LocalDirection;
        public int CreateTime;
        public int CreateCount;
        public int CreateProbability;
        public int ParentInfluenceCoord;
        public int ParentInfluenceColor;
        public int InfluenceCoordScale;
        public int InfluenceCoordRot;
        public int InfluenceCoordPos;
        public int InfluenceCoordBinderPosition;
        public int InfluenceCoordUnstickiness;
        public int InheritParentVelocity;
        public int InheritParentLife;
        public bool OverrideLife;
        public int OverrideLifeValue;
        public int OverrideLifeRandom;
        public int ParameterLink; // -1
        public int StartFrame;
        public bool StartFrameNullUpdate;
        public Vector3 ByInjectionAngle;
        public int GenerateDelay;
        public bool GenerateDelayByOne;
        //==========================
        public UIEmitterDataBase Data;

        public UIEmitterItem(AVFXEmitterIterationItem iteration, bool isParticle)
        {
            Iteration = iteration;
            IsParticle = isParticle;
            //=========================
            Enabled = (iteration.Enabled.Value == true);
            TargetIdx = iteration.TargetIdx.Value;
            LocalDirection = iteration.LocalDirection.Value;
            CreateTime = iteration.CreateTime.Value;
            CreateCount = iteration.CreateCount.Value;
            CreateProbability = iteration.CreateProbability.Value;
            ParentInfluenceCoord = iteration.ParentInfluenceCoord.Value;
            ParentInfluenceColor = iteration.ParentInfluenceColor.Value;
            InfluenceCoordScale = iteration.InfluenceCoordScale.Value;
            InfluenceCoordRot = iteration.InfluenceCoordRot.Value;
            InfluenceCoordPos = iteration.InfluenceCoordPos.Value;
            InfluenceCoordBinderPosition = iteration.InfluenceCoordBinderPosition.Value;
            InfluenceCoordUnstickiness = iteration.InfluenceCoordUnstickiness.Value;
            InheritParentVelocity = iteration.InheritParentVelocity.Value;
            InheritParentLife = iteration.InheritParentLife.Value;
            OverrideLife = (iteration.OverrideLife.Value == true);
            OverrideLifeValue = iteration.OverrideLifeValue.Value;
            OverrideLifeRandom = iteration.OverrideLifeRandom.Value;
            ParameterLink = iteration.ParameterLink.Value;
            StartFrame = iteration.StartFrame.Value;
            StartFrameNullUpdate = (iteration.StartFrameNullUpdate.Value == true);
            ByInjectionAngle = new Vector3(iteration.ByInjectionAngleX.Value, iteration.ByInjectionAngleY.Value, iteration.ByInjectionAngleZ.Value);
            GenerateDelay = iteration.GenerateDelay.Value;
            GenerateDelayByOne = (iteration.GenerateDelayByOne.Value == true);
        }

        public void Draw(string id, int idx)
        {
            if (ImGui.TreeNode((IsParticle ? "Particle" : "Emitter") + "#" + idx + id))
            {
                if (ImGui.Checkbox("Enabled" + id, ref Enabled))
                {
                    Iteration.Enabled.GiveValue(Enabled);
                }
                if (ImGui.InputInt("Target Index" + id, ref TargetIdx))
                {
                    Iteration.TargetIdx.GiveValue(TargetIdx);
                }
                if (ImGui.InputInt("Local Direction" + id, ref LocalDirection))
                {
                    Iteration.LocalDirection.GiveValue(LocalDirection);
                }
                if (ImGui.InputInt("Create Time" + id, ref CreateTime))
                {
                    Iteration.CreateTime.GiveValue(CreateTime);
                }
                if (ImGui.InputInt("Create Count" + id, ref CreateCount))
                {
                    Iteration.CreateCount.GiveValue(CreateCount);
                }
                if (ImGui.InputInt("Create Probability" + id, ref CreateProbability))
                {
                    Iteration.CreateProbability.GiveValue(CreateProbability);
                }
                if (ImGui.InputInt("Parent Influence Coordinates" + id, ref ParentInfluenceCoord))
                {
                    Iteration.ParentInfluenceCoord.GiveValue(ParentInfluenceCoord);
                }
                if (ImGui.InputInt("Parent Influence Color" + id, ref ParentInfluenceColor))
                {
                    Iteration.ParentInfluenceColor.GiveValue(ParentInfluenceColor);
                }
                if (ImGui.InputInt("Influence Coordinates Scale" + id, ref InfluenceCoordScale))
                {
                    Iteration.InfluenceCoordScale.GiveValue(InfluenceCoordScale);
                }
                if (ImGui.InputInt("Influence Coordinates Rotation" + id, ref InfluenceCoordRot))
                {
                    Iteration.InfluenceCoordRot.GiveValue(InfluenceCoordRot);
                }
                if (ImGui.InputInt("Influence Coordinates Position" + id, ref InfluenceCoordPos))
                {
                    Iteration.InfluenceCoordPos.GiveValue(InfluenceCoordPos);
                }
                if (ImGui.InputInt("Influence Coords. Binder Position" + id, ref InfluenceCoordBinderPosition))
                {
                    Iteration.InfluenceCoordBinderPosition.GiveValue(InfluenceCoordBinderPosition);
                }
                if (ImGui.InputInt("Influence Coords. Unstickiness" + id, ref InfluenceCoordUnstickiness))
                {
                    Iteration.InfluenceCoordUnstickiness.GiveValue(InfluenceCoordUnstickiness);
                }
                if (ImGui.InputInt("Inherit Parent Velocity" + id, ref InheritParentVelocity))
                {
                    Iteration.InheritParentVelocity.GiveValue(InheritParentVelocity);
                }
                if (ImGui.InputInt("Inherit Parent Life" + id, ref InheritParentLife))
                {
                    Iteration.InheritParentLife.GiveValue(InheritParentLife);
                }
                if (ImGui.Checkbox("Override Life" + id, ref OverrideLife))
                {
                    Iteration.OverrideLife.GiveValue(OverrideLife);
                }
                if (ImGui.InputInt("Override Life Value" + id, ref OverrideLifeValue))
                {
                    Iteration.OverrideLifeValue.GiveValue(OverrideLifeValue);
                }
                if (ImGui.InputInt("Override Life Random" + id, ref OverrideLifeRandom))
                {
                    Iteration.OverrideLifeRandom.GiveValue(OverrideLifeRandom);
                }
                if (ImGui.InputInt("Parameter Link" + id, ref ParameterLink))
                {
                    Iteration.ParameterLink.GiveValue(ParameterLink);
                }
                if (ImGui.InputInt("Start Frame" + id, ref StartFrame))
                {
                    Iteration.StartFrame.GiveValue(StartFrame);
                }
                if (ImGui.Checkbox("Start Frame Null Update" + id, ref StartFrameNullUpdate))
                {
                    Iteration.StartFrameNullUpdate.GiveValue(StartFrameNullUpdate);
                }
                if (ImGui.InputFloat3("By Injection Angle" + id, ref ByInjectionAngle))
                {
                    Iteration.ByInjectionAngleX.GiveValue(ByInjectionAngle.X);
                    Iteration.ByInjectionAngleY.GiveValue(ByInjectionAngle.Y);
                    Iteration.ByInjectionAngleZ.GiveValue(ByInjectionAngle.Z);
                }
                if (ImGui.InputInt("Generate Delay" + id, ref GenerateDelay))
                {
                    Iteration.GenerateDelay.GiveValue(GenerateDelay);
                }
                if (ImGui.Checkbox("Generate Delay By One" + id, ref GenerateDelayByOne))
                {
                    Iteration.GenerateDelayByOne.GiveValue(GenerateDelayByOne);
                }

                ImGui.TreePop();
            }
        }
    }
}
