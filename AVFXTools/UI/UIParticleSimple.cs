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
                if (ImGui.DragInt("Injection Position Type" + id, ref InjectionPositionType, 1, 0))
                {
                    Simple.InjectionPositionType.GiveValue(InjectionPositionType);
                }
                if (ImGui.DragInt("Injection Direction Type" + id, ref InjectionDirectionType, 1, 0))
                {
                    Simple.InjectionDirectionType.GiveValue(InjectionDirectionType);
                }
                if (ImGui.DragInt("Base Direction Type" + id, ref BaseDirectionType, 1, 0))
                {
                    Simple.BaseDirectionType.GiveValue(BaseDirectionType);
                }
                if (ImGui.DragInt("Create Count" + id, ref CreateCount, 1, 0))
                {
                    Simple.CreateCount.GiveValue(CreateCount);
                }
                if(ImGui.DragFloat3("Create Area" + id, ref CreateArea, 1))
                {
                    Simple.CreateAreaX.GiveValue(CreateArea.X);
                    Simple.CreateAreaY.GiveValue(CreateArea.Y);
                    Simple.CreateAreaZ.GiveValue(CreateArea.Z);
                }
                if (ImGui.DragFloat3("Coord Accuracy" + id, ref CoordAccuracy, 1, 0))
                {
                    Simple.CoordAccuracyX.GiveValue(CoordAccuracy.X);
                    Simple.CoordAccuracyY.GiveValue(CoordAccuracy.Y);
                    Simple.CoordAccuracyZ.GiveValue(CoordAccuracy.Z);
                }
                if (ImGui.DragFloat3("Coord Gra" + id, ref CoordGra, 1, 0))
                {
                    Simple.CoordGraX.GiveValue(CoordGra.X);
                    Simple.CoordGraY.GiveValue(CoordGra.Y);
                    Simple.CoordGraZ.GiveValue(CoordGra.Z);
                }
                if (ImGui.DragFloat2("Scale Start" + id, ref ScaleStart, 1))
                {
                    Simple.ScaleXStart.GiveValue(ScaleStart.X);
                    Simple.ScaleYStart.GiveValue(ScaleStart.Y);
                }
                if (ImGui.DragFloat2("Scale End" + id, ref ScaleEnd, 1))
                {
                    Simple.ScaleXEnd.GiveValue(ScaleEnd.X);
                    Simple.ScaleYEnd.GiveValue(ScaleEnd.Y);
                }
                if (ImGui.DragFloat("Scale Curve" + id, ref ScaleCurve, 1, 0))
                {
                    Simple.ScaleCurve.GiveValue(ScaleCurve);
                }
                if (ImGui.DragFloat2("Scale Random X" + id, ref ScaleRandX, 1))
                {
                    Simple.ScaleRandX0.GiveValue(ScaleRandX.X);
                    Simple.ScaleRandX1.GiveValue(ScaleRandX.Y);
                }
                if (ImGui.DragFloat2("Scale Random Y" + id, ref ScaleRandY, 1))
                {
                    Simple.ScaleRandY0.GiveValue(ScaleRandY.X);
                    Simple.ScaleRandY1.GiveValue(ScaleRandY.Y);
                }
                if (ImGui.DragFloat3("Rotation Start" + id, ref RotStart, 1))
                {
                    Simple.RotXStart.GiveValue(RotStart.X);
                    Simple.RotYStart.GiveValue(RotStart.Y);
                    Simple.RotZStart.GiveValue(RotStart.Z);
                }
                if (ImGui.DragFloat3("Rotation Add" + id, ref RotAdd, 1))
                {
                    Simple.RotXAdd.GiveValue(RotAdd.X);
                    Simple.RotYAdd.GiveValue(RotAdd.Y);
                    Simple.RotZAdd.GiveValue(RotAdd.Z);
                }
                if (ImGui.DragFloat3("Rotation Base" + id, ref RotBase, 1))
                {
                    Simple.RotXBase.GiveValue(RotBase.X);
                    Simple.RotYBase.GiveValue(RotBase.Y);
                    Simple.RotZBase.GiveValue(RotBase.Z);
                }
                if (ImGui.DragFloat3("Rotation Velocity" + id, ref RotVel, 1))
                {
                    Simple.RotXVel.GiveValue(RotVel.X);
                    Simple.RotYVel.GiveValue(RotVel.Y);
                    Simple.RotZVel.GiveValue(RotVel.Z);
                }
                if (ImGui.DragFloat("Velocity Min" + id, ref VelMin, 1, 0))
                {
                    Simple.VelMin.GiveValue(CreateCount);
                }
                if (ImGui.DragFloat("Velocity Max" + id, ref VelMax, 1, 0))
                {
                    Simple.VelMax.GiveValue(CreateCount);
                }
                if (ImGui.DragFloat("Velocity Flattery Rate" + id, ref VelFlatteryRate, 1, 0))
                {
                    Simple.VelFlatteryRate.GiveValue(VelFlatteryRate);
                }
                if (ImGui.DragFloat("Velocity Flatter Speed" + id, ref VelFlatterySpeed, 1, 0))
                {
                    Simple.VelFlatterySpeed.GiveValue(VelFlatterySpeed);
                }
                if (ImGui.DragInt("UV Cell U" + id, ref UvCellU, 1, 0))
                {
                    Simple.UvCellU.GiveValue(UvCellU);
                }
                if (ImGui.DragInt("UV Cell V" + id, ref UvCellV, 1, 0))
                {
                    Simple.UvCellV.GiveValue(UvCellV);
                }
                if (ImGui.DragInt("UV Interval" + id, ref UvInterval, 1, 0))
                {
                    Simple.UvInterval.GiveValue(UvInterval);
                }
                if (ImGui.DragInt("UV Number Random" + id, ref UvNoRandom, 1, 0))
                {
                    Simple.UvNoRandom.GiveValue(UvNoRandom);
                }
                if (ImGui.DragInt("UV Number Loop Count" + id, ref UvNoLoopCount, 1, -1))
                {
                    Simple.UvNoLoopCount.GiveValue(UvNoLoopCount);
                }
                if (ImGui.DragInt("Injection Model Index" + id, ref InjectionModelIdx, 1, -1))
                {
                    Simple.InjectionModelIdx.GiveValue(InjectionModelIdx);
                }
                if (ImGui.DragInt("Injection Vertex Bind Model Idx" + id, ref InjectionVertexBindModelIdx, 1, 0))
                {
                    Simple.InjectionVertexBindModelIdx.GiveValue(InjectionVertexBindModelIdx);
                }
                if (ImGui.DragInt("Injection Radial Direction 0" + id, ref InjectionRadialDir0, 1, 0))
                {
                    Simple.InjectionRadialDir0.GiveValue(InjectionRadialDir0);
                }
                if (ImGui.DragInt("Injection Radial Direction 1" + id, ref InjectionRadialDir1, 1, 0))
                {
                    Simple.InjectionRadialDir1.GiveValue(InjectionRadialDir1);
                }
                if (ImGui.DragFloat("Pivot X" + id, ref PivotX, 1, 0))
                {
                    Simple.PivotX.GiveValue(PivotX);
                }
                if (ImGui.DragFloat("Pivot Y" + id, ref PivotY, 1, 0))
                {
                    Simple.PivotY.GiveValue(PivotY);
                }
                if (ImGui.DragInt("Block Number" + id, ref BlockNum, 1, 0))
                {
                    Simple.BlockNum.GiveValue(BlockNum);
                }
                if (ImGui.DragFloat("Min Line Length" + id, ref LineLengthMin, 1, 0))
                {
                    Simple.LineLengthMin.GiveValue(LineLengthMin);
                }
                if (ImGui.DragFloat("Max Line Length" + id, ref LineLengthMax, 1, 0))
                {
                    Simple.LineLengthMax.GiveValue(LineLengthMax);
                }
                if (ImGui.DragInt("Create Interval" + id, ref CreateIntervalVal, 1, 0))
                {
                    Simple.CreateIntervalVal.GiveValue(CreateIntervalVal);
                }
                if (ImGui.DragInt("Create Interval Random" + id, ref CreateIntervalRandom, 1, 0))
                {
                    Simple.CreateIntervalRandom.GiveValue(CreateIntervalRandom);
                }
                if (ImGui.DragInt("Create Interval Count" + id, ref CreateIntervalCount, 1, 0))
                {
                    Simple.CreateIntervalCount.GiveValue(CreateIntervalCount);
                }
                if (ImGui.DragInt("Create Interval Life" + id, ref CreateIntervalLife, 1, 0))
                {
                    Simple.CreateIntervalLife.GiveValue(CreateIntervalLife);
                }
                if (ImGui.DragInt("Create New After Delete" + id, ref CreateNewAfterDelete, 1, 0))
                {
                    Simple.CreateNewAfterDelete.GiveValue(CreateNewAfterDelete);
                }
                if (ImGui.DragInt("UV Reverse" + id, ref UvReverse, 1, 0))
                {
                    Simple.UvReverse.GiveValue(UvReverse);
                }
                if (ImGui.DragInt("Scale Random Link" + id, ref ScaleRandomLink, 1, 0))
                {
                    Simple.ScaleRandomLink.GiveValue(ScaleRandomLink);
                }
                if (ImGui.DragInt("Bind Parent" + id, ref BindParent, 1, 0))
                {
                    Simple.BindParent.GiveValue(BindParent);
                }
                if (ImGui.DragInt("Scale By Parent" + id, ref ScaleByParent, 1, 0))
                {
                    Simple.ScaleByParent.GiveValue(ScaleByParent);
                }
                if (ImGui.DragInt("Polyline Tag" + id, ref PolyLineTag, 1, 0))
                {
                    Simple.PolyLineTag.GiveValue(PolyLineTag);
                }
                //====================
                for(int i = 0; i < 4; i++)
                {
                    if(ImGui.DragInt("Frame#" + i + id, ref Frames[i], 1, 0))
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
