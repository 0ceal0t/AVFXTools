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
                if (ImGui.DragInt("Target Index" + id, ref TargetIdx, 1, 0))
                {
                    Iteration.TargetIdx.GiveValue(TargetIdx);
                }
                if (ImGui.DragInt("Local Direction" + id, ref LocalDirection, 1, 0))
                {
                    Iteration.LocalDirection.GiveValue(LocalDirection);
                }
                if (ImGui.DragInt("Create Time" + id, ref CreateTime, 1, 0))
                {
                    Iteration.CreateTime.GiveValue(CreateTime);
                }
                if (ImGui.DragInt("Create Count" + id, ref CreateCount, 1, 0))
                {
                    Iteration.CreateCount.GiveValue(CreateCount);
                }
                if (ImGui.DragInt("Create Probability" + id, ref CreateProbability, 1, 0))
                {
                    Iteration.CreateProbability.GiveValue(CreateProbability);
                }
                if (ImGui.DragInt("Parent Influence Coordinates" + id, ref ParentInfluenceCoord, 1, 0))
                {
                    Iteration.ParentInfluenceCoord.GiveValue(ParentInfluenceCoord);
                }
                if (ImGui.DragInt("Parent Influence Color" + id, ref ParentInfluenceColor, 1, 0))
                {
                    Iteration.ParentInfluenceColor.GiveValue(ParentInfluenceColor);
                }
                if (ImGui.DragInt("Influence Coordinates Scale" + id, ref InfluenceCoordScale, 1, 0))
                {
                    Iteration.InfluenceCoordScale.GiveValue(InfluenceCoordScale);
                }
                if (ImGui.DragInt("Influence Coordinates Rotation" + id, ref InfluenceCoordRot, 1, 0))
                {
                    Iteration.InfluenceCoordRot.GiveValue(InfluenceCoordRot);
                }
                if (ImGui.DragInt("Influence Coordinates Position" + id, ref InfluenceCoordPos, 1, 0))
                {
                    Iteration.InfluenceCoordPos.GiveValue(InfluenceCoordPos);
                }
                if (ImGui.DragInt("Influence Coords. Binder Position" + id, ref InfluenceCoordBinderPosition, 1, 0))
                {
                    Iteration.InfluenceCoordBinderPosition.GiveValue(InfluenceCoordBinderPosition);
                }
                if (ImGui.DragInt("Influence Coords. Unstickiness" + id, ref InfluenceCoordUnstickiness, 1, 0))
                {
                    Iteration.InfluenceCoordUnstickiness.GiveValue(InfluenceCoordUnstickiness);
                }
                if (ImGui.DragInt("Inherit Parent Velocity" + id, ref InheritParentVelocity, 1, 0))
                {
                    Iteration.InheritParentVelocity.GiveValue(InheritParentVelocity);
                }
                if (ImGui.DragInt("Inherit Parent Life" + id, ref InheritParentLife, 1, 0))
                {
                    Iteration.InheritParentLife.GiveValue(InheritParentLife);
                }
                if (ImGui.Checkbox("Override Life" + id, ref OverrideLife))
                {
                    Iteration.OverrideLife.GiveValue(OverrideLife);
                }
                if (ImGui.DragInt("Override Life Value" + id, ref OverrideLifeValue, 1, 0))
                {
                    Iteration.OverrideLifeValue.GiveValue(OverrideLifeValue);
                }
                if (ImGui.DragInt("Override Life Random" + id, ref OverrideLifeRandom, 1, 0))
                {
                    Iteration.OverrideLifeRandom.GiveValue(OverrideLifeRandom);
                }
                if (ImGui.DragInt("Parameter Link" + id, ref ParameterLink, 1, -1))
                {
                    Iteration.ParameterLink.GiveValue(ParameterLink);
                }
                if (ImGui.DragInt("Start Frame" + id, ref StartFrame, 1, 0))
                {
                    Iteration.StartFrame.GiveValue(StartFrame);
                }
                if (ImGui.Checkbox("Start Frame Null Update" + id, ref StartFrameNullUpdate))
                {
                    Iteration.StartFrameNullUpdate.GiveValue(StartFrameNullUpdate);
                }
                if (ImGui.DragFloat3("By Injection Angle" + id, ref ByInjectionAngle))
                {
                    Iteration.ByInjectionAngleX.GiveValue(ByInjectionAngle.X);
                    Iteration.ByInjectionAngleY.GiveValue(ByInjectionAngle.Y);
                    Iteration.ByInjectionAngleZ.GiveValue(ByInjectionAngle.Z);
                }
                if (ImGui.DragInt("Generate Delay" + id, ref GenerateDelay, 1, -1))
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
