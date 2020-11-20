using AVFXLib.Main;
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
    public class UIParticleSimple
    {
        public AVFXParticleSimple Simple;
        //====================
        public int InjectionPositionType;
        public int InjectionDirectionType;
        public int BaseDirectionType;
        public int CreateCount;
        public Vector3 CreateArea; //X,Y,Z
        public Vector3 CoordAccuracy; //X,Y,Z
        public Vector3 CoordGra; // X,Y,Z
        public Vector2 ScaleStart; // X,Y
        public Vector2 ScaleEnd; // X,Y
        public float ScaleCurve;
        public Vector2 ScaleRandX; //X0, X1
        public Vector2 ScaleRandY; //Y0, Y1
        public Vector3 RotStart; //X,Y,Z
        public Vector3 RotAdd;
        public Vector3 RotBase;
        public Vector3 RotVel;
        public float VelMin;
        public float VelMax;
        public float VelFlatteryRate;
        public float VelFlatterySpeed;
        public int UvCellU;
        public int UvCellV;
        public int UvInterval;
        public int UvNoRandom;
        public int UvNoLoopCount;
        public int InjectionModelIdx; // -1
        public int InjectionVertexBindModelIdx; // -1
        public int InjectionRadialDir0;
        public int InjectionRadialDir1;
        public float PivotX;
        public float PivotY;
        public int BlockNum;
        public float LineLengthMin;
        public float LineLengthMax;
        public int CreateIntervalVal;
        public int CreateIntervalRandom;
        public int CreateIntervalCount;
        public int CreateIntervalLife;
        public int CreateNewAfterDelete;
        public int UvReverse;
        public int ScaleRandomLink;
        public int BindParent;
        public int ScaleByParent;
        public int PolyLineTag;
        //=============
        public Vector4[] Colors;
        public int[] Frames;

        public UIParticleSimple(AVFXParticleSimple simple)
        {
            if (simple.Assigned)
            {
                Simple = simple;
                //=======================
                InjectionPositionType = Simple.InjectionPositionType.Value;
                InjectionDirectionType = Simple.InjectionDirectionType.Value;
                BaseDirectionType = Simple.BaseDirectionType.Value;
                CreateCount = Simple.CreateCount.Value;
                CreateArea = new Vector3(Simple.CreateAreaX.Value, Simple.CreateAreaY.Value, Simple.CreateAreaZ.Value);
                CoordAccuracy = new Vector3(Simple.CoordAccuracyX.Value, Simple.CoordAccuracyY.Value, Simple.CoordAccuracyZ.Value);
                CoordGra = new Vector3(Simple.CoordGraX.Value, Simple.CoordGraY.Value, Simple.CoordGraZ.Value);
                ScaleStart = new Vector2(Simple.ScaleXStart.Value, Simple.ScaleYStart.Value);
                ScaleEnd = new Vector2(Simple.ScaleXEnd.Value, Simple.ScaleYEnd.Value);
                ScaleCurve = Simple.ScaleCurve.Value;
                ScaleRandX = new Vector2(Simple.ScaleRandX0.Value, Simple.ScaleRandX1.Value);
                ScaleRandY = new Vector2(Simple.ScaleRandY0.Value, Simple.ScaleRandY1.Value);
                RotStart = new Vector3(Simple.RotXStart.Value, Simple.RotYStart.Value, Simple.RotZStart.Value);
                RotAdd = new Vector3(Simple.RotXAdd.Value, Simple.RotYAdd.Value, Simple.RotZAdd.Value);
                RotBase = new Vector3(Simple.RotXBase.Value, Simple.RotYBase.Value, Simple.RotZBase.Value);
                RotVel = new Vector3(Simple.RotXVel.Value, Simple.RotYVel.Value, Simple.RotZVel.Value);
                VelMin = Simple.VelMin.Value;
                VelMax = Simple.VelMax.Value;
                VelFlatteryRate = Simple.VelFlatteryRate.Value;
                VelFlatterySpeed = Simple.VelFlatterySpeed.Value;
                UvCellU = Simple.UvCellU.Value;
                UvCellV = Simple.UvCellV.Value;
                UvInterval = Simple.UvInterval.Value;
                UvNoRandom = Simple.UvNoRandom.Value;
                UvNoLoopCount = Simple.UvNoLoopCount.Value;
                InjectionModelIdx = Simple.InjectionModelIdx.Value;
                InjectionVertexBindModelIdx = Simple.InjectionVertexBindModelIdx.Value;
                InjectionRadialDir0 = Simple.InjectionRadialDir0.Value;
                InjectionRadialDir1 = Simple.InjectionRadialDir1.Value;
                PivotX = Simple.PivotX.Value;
                PivotY = Simple.PivotY.Value;
                BlockNum = Simple.BlockNum.Value;
                LineLengthMin = Simple.LineLengthMin.Value;
                LineLengthMax = Simple.LineLengthMax.Value;
                CreateIntervalVal = Simple.CreateIntervalVal.Value;
                CreateIntervalRandom = Simple.CreateIntervalRandom.Value;
                CreateIntervalCount = Simple.CreateIntervalCount.Value;
                CreateIntervalLife = Simple.CreateIntervalLife.Value;
                CreateNewAfterDelete = Simple.CreateNewAfterDelete.Value;
                UvReverse = Simple.UvReverse.Value;
                ScaleRandomLink = Simple.ScaleRandomLink.Value;
                BindParent = Simple.BindParent.Value;
                ScaleByParent = Simple.ScaleByParent.Value;
                PolyLineTag = Simple.PolyLineTag.Value;
                //=================
                Colors = new Vector4[4];
                Frames = new int[4];
                for(int i = 0; i < 4; i++)
                {
                    Colors[i] = new Vector4(
                        (float)Util.Bytes1ToInt(new byte[] { Simple.Colors.colors[i * 4 + 0] }) / 255,
                        (float)Util.Bytes1ToInt(new byte[] { Simple.Colors.colors[i * 4 + 1] }) / 255,
                        (float)Util.Bytes1ToInt(new byte[] { Simple.Colors.colors[i * 4 + 2] }) / 255,
                        (float)Util.Bytes1ToInt(new byte[] { Simple.Colors.colors[i * 4 + 3] }) / 255
                    );
                    Frames[i] = Simple.Frames.frames[i];
                }
            }
        }

        public void Draw(string id)
        {
            if (Simple == null) return;
            if (ImGui.TreeNode("Simple Animation" + id))
            {
                if (ImGui.InputInt("Injection Position Type" + id, ref InjectionPositionType))
                {
                    Simple.InjectionPositionType.GiveValue(InjectionPositionType);
                }
                if (ImGui.InputInt("Injection Direction Type" + id, ref InjectionDirectionType))
                {
                    Simple.InjectionDirectionType.GiveValue(InjectionDirectionType);
                }
                if (ImGui.InputInt("Base Direction Type" + id, ref BaseDirectionType))
                {
                    Simple.BaseDirectionType.GiveValue(BaseDirectionType);
                }
                if (ImGui.InputInt("Create Count" + id, ref CreateCount))
                {
                    Simple.CreateCount.GiveValue(CreateCount);
                }
                if(ImGui.InputFloat3("Create Area" + id, ref CreateArea))
                {
                    Simple.CreateAreaX.GiveValue(CreateArea.X);
                    Simple.CreateAreaY.GiveValue(CreateArea.Y);
                    Simple.CreateAreaZ.GiveValue(CreateArea.Z);
                }
                if (ImGui.InputFloat3("Coord Accuracy" + id, ref CoordAccuracy))
                {
                    Simple.CoordAccuracyX.GiveValue(CoordAccuracy.X);
                    Simple.CoordAccuracyY.GiveValue(CoordAccuracy.Y);
                    Simple.CoordAccuracyZ.GiveValue(CoordAccuracy.Z);
                }
                if (ImGui.InputFloat3("Coord Gra" + id, ref CoordGra))
                {
                    Simple.CoordGraX.GiveValue(CoordGra.X);
                    Simple.CoordGraY.GiveValue(CoordGra.Y);
                    Simple.CoordGraZ.GiveValue(CoordGra.Z);
                }
                if (ImGui.InputFloat2("Scale Start" + id, ref ScaleStart))
                {
                    Simple.ScaleXStart.GiveValue(ScaleStart.X);
                    Simple.ScaleYStart.GiveValue(ScaleStart.Y);
                }
                if (ImGui.InputFloat2("Scale End" + id, ref ScaleEnd))
                {
                    Simple.ScaleXEnd.GiveValue(ScaleEnd.X);
                    Simple.ScaleYEnd.GiveValue(ScaleEnd.Y);
                }
                if (ImGui.InputFloat("Scale Curve" + id, ref ScaleCurve))
                {
                    Simple.ScaleCurve.GiveValue(ScaleCurve);
                }
                if (ImGui.InputFloat2("Scale Random X" + id, ref ScaleRandX))
                {
                    Simple.ScaleRandX0.GiveValue(ScaleRandX.X);
                    Simple.ScaleRandX1.GiveValue(ScaleRandX.Y);
                }
                if (ImGui.InputFloat2("Scale Random Y" + id, ref ScaleRandY))
                {
                    Simple.ScaleRandY0.GiveValue(ScaleRandY.X);
                    Simple.ScaleRandY1.GiveValue(ScaleRandY.Y);
                }
                if (ImGui.InputFloat3("Rotation Start" + id, ref RotStart))
                {
                    Simple.RotXStart.GiveValue(RotStart.X);
                    Simple.RotYStart.GiveValue(RotStart.Y);
                    Simple.RotZStart.GiveValue(RotStart.Z);
                }
                if (ImGui.InputFloat3("Rotation Add" + id, ref RotAdd))
                {
                    Simple.RotXAdd.GiveValue(RotAdd.X);
                    Simple.RotYAdd.GiveValue(RotAdd.Y);
                    Simple.RotZAdd.GiveValue(RotAdd.Z);
                }
                if (ImGui.InputFloat3("Rotation Base" + id, ref RotBase))
                {
                    Simple.RotXBase.GiveValue(RotBase.X);
                    Simple.RotYBase.GiveValue(RotBase.Y);
                    Simple.RotZBase.GiveValue(RotBase.Z);
                }
                if (ImGui.InputFloat3("Rotation Velocity" + id, ref RotVel))
                {
                    Simple.RotXVel.GiveValue(RotVel.X);
                    Simple.RotYVel.GiveValue(RotVel.Y);
                    Simple.RotZVel.GiveValue(RotVel.Z);
                }
                if (ImGui.InputFloat("Velocity Min" + id, ref VelMin))
                {
                    Simple.VelMin.GiveValue(CreateCount);
                }
                if (ImGui.InputFloat("Velocity Max" + id, ref VelMax))
                {
                    Simple.VelMax.GiveValue(CreateCount);
                }
                if (ImGui.InputFloat("Velocity Flattery Rate" + id, ref VelFlatteryRate))
                {
                    Simple.VelFlatteryRate.GiveValue(VelFlatteryRate);
                }
                if (ImGui.InputFloat("Velocity Flatter Speed" + id, ref VelFlatterySpeed))
                {
                    Simple.VelFlatterySpeed.GiveValue(VelFlatterySpeed);
                }
                if (ImGui.InputInt("UV Cell U" + id, ref UvCellU))
                {
                    Simple.UvCellU.GiveValue(UvCellU);
                }
                if (ImGui.InputInt("UV Cell V" + id, ref UvCellV))
                {
                    Simple.UvCellV.GiveValue(UvCellV);
                }
                if (ImGui.InputInt("UV Interval" + id, ref UvInterval))
                {
                    Simple.UvInterval.GiveValue(UvInterval);
                }
                if (ImGui.InputInt("UV Number Random" + id, ref UvNoRandom))
                {
                    Simple.UvNoRandom.GiveValue(UvNoRandom);
                }
                if (ImGui.InputInt("UV Number Loop Count" + id, ref UvNoLoopCount))
                {
                    Simple.UvNoLoopCount.GiveValue(UvNoLoopCount);
                }
                if (ImGui.InputInt("Injection Model Index" + id, ref InjectionModelIdx))
                {
                    Simple.InjectionModelIdx.GiveValue(InjectionModelIdx);
                }
                if (ImGui.InputInt("Injection Vertex Bind Model Idx" + id, ref InjectionVertexBindModelIdx))
                {
                    Simple.InjectionVertexBindModelIdx.GiveValue(InjectionVertexBindModelIdx);
                }
                if (ImGui.InputInt("Injection Radial Direction 0" + id, ref InjectionRadialDir0))
                {
                    Simple.InjectionRadialDir0.GiveValue(InjectionRadialDir0);
                }
                if (ImGui.InputInt("Injection Radial Direction 1" + id, ref InjectionRadialDir1))
                {
                    Simple.InjectionRadialDir1.GiveValue(InjectionRadialDir1);
                }
                if (ImGui.InputFloat("Pivot X" + id, ref PivotX))
                {
                    Simple.PivotX.GiveValue(PivotX);
                }
                if (ImGui.InputFloat("Pivot Y" + id, ref PivotY))
                {
                    Simple.PivotY.GiveValue(PivotY);
                }
                if (ImGui.InputInt("Block Number" + id, ref BlockNum))
                {
                    Simple.BlockNum.GiveValue(BlockNum);
                }
                if (ImGui.InputFloat("Min Line Length" + id, ref LineLengthMin))
                {
                    Simple.LineLengthMin.GiveValue(LineLengthMin);
                }
                if (ImGui.InputFloat("Max Line Length" + id, ref LineLengthMax))
                {
                    Simple.LineLengthMax.GiveValue(LineLengthMax);
                }
                if (ImGui.InputInt("Create Interval" + id, ref CreateIntervalVal))
                {
                    Simple.CreateIntervalVal.GiveValue(CreateIntervalVal);
                }
                if (ImGui.InputInt("Create Interval Random" + id, ref CreateIntervalRandom))
                {
                    Simple.CreateIntervalRandom.GiveValue(CreateIntervalRandom);
                }
                if (ImGui.InputInt("Create Interval Count" + id, ref CreateIntervalCount))
                {
                    Simple.CreateIntervalCount.GiveValue(CreateIntervalCount);
                }
                if (ImGui.InputInt("Create Interval Life" + id, ref CreateIntervalLife))
                {
                    Simple.CreateIntervalLife.GiveValue(CreateIntervalLife);
                }
                if (ImGui.InputInt("Create New After Delete" + id, ref CreateNewAfterDelete))
                {
                    Simple.CreateNewAfterDelete.GiveValue(CreateNewAfterDelete);
                }
                if (ImGui.InputInt("UV Reverse" + id, ref UvReverse))
                {
                    Simple.UvReverse.GiveValue(UvReverse);
                }
                if (ImGui.InputInt("Scale Random Link" + id, ref ScaleRandomLink))
                {
                    Simple.ScaleRandomLink.GiveValue(ScaleRandomLink);
                }
                if (ImGui.InputInt("Bind Parent" + id, ref BindParent))
                {
                    Simple.BindParent.GiveValue(BindParent);
                }
                if (ImGui.InputInt("Scale By Parent" + id, ref ScaleByParent))
                {
                    Simple.ScaleByParent.GiveValue(ScaleByParent);
                }
                if (ImGui.InputInt("Polyline Tag" + id, ref PolyLineTag))
                {
                    Simple.PolyLineTag.GiveValue(PolyLineTag);
                }
                //====================
                for(int i = 0; i < 4; i++)
                {
                    if(ImGui.InputInt("Frame#" + i + id, ref Frames[i]))
                    {
                        Simple.Frames.frames[i] = Frames[i];
                    }
                    if(ImGui.ColorEdit4("Color#" + i + id, ref Colors[i], ImGuiColorEditFlags.Float))
                    {
                        Simple.Colors.colors[i * 4 + 0] = Util.IntTo1Bytes((int)(Colors[i].X * 255f))[0];
                        Simple.Colors.colors[i * 4 + 1] = Util.IntTo1Bytes((int)(Colors[i].Y * 255f))[0];
                        Simple.Colors.colors[i * 4 + 2] = Util.IntTo1Bytes((int)(Colors[i].Z * 255f))[0];
                        Simple.Colors.colors[i * 4 + 3] = Util.IntTo1Bytes((int)(Colors[i].W * 255f))[0];
                    }
                }
                ImGui.TreePop();
            }
        }
    }
}
