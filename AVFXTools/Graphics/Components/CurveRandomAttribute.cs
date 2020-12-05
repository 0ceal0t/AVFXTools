using System;
using System.Collections.Generic;
using System.Linq;
using static alglib;
using AVFXLib.Models;

namespace AVFXTools.Main
{
    public class CurveRandomGroup
    {
        public string Name;
        public bool Enabled = false;
        public float DEFAULT;

        public CurveRandomAttribute Main;
        public CurveRandomAttribute Random;

        public CurveRandomGroup(string name, AVFXCurve main, AVFXCurve random, float D = 0.0f)
        {
            Name = name;
            DEFAULT = D;

            if (main != null && main.Assigned)
            {
                Enabled = true;
                Main = new CurveRandomAttribute(main);
            }
            if (random != null && random.Assigned)
            {
                Enabled = true;
                Random = new CurveRandomAttribute(random, R: true);
            }
        }

        public float GetValue(float time)
        {
            float value = 0.0f;
            if (Main != null)
            {
                value += Main.GetValue(time);
            }
            else
            {
                value += DEFAULT;
            }

            if (Random != null)
            {
                value += Random.GetValue(time);
            }
            return value;
        }

        public void Reset()
        {
            if (Random != null)
            {
                Random.Reset();
            }
        }
    }

    public class CurveRandomAttribute
    {
        public static Random RandomGen = new Random();

        public spline1dinterpolant Spline;
        public AVFXCurve Curve;
        public bool isConstant = true;
        public float ConstantValue;
        public float MinTime;
        public float MaxTime;
        public bool isRandom;
        public RandomType RandomVariety;

        public CurveBehavior PreType;
        public CurveBehavior PostType;

        public CurveRandomAttribute(AVFXCurve curve, bool R = false)
        {
            Curve = curve;
            isRandom = R;
            RandomVariety = Curve.Random.Value;

            if (Curve.Keys.Count == 0)
            {
                ConstantValue = 0;
            }
            else if (Curve.Keys.Count == 1)
            {
                ConstantValue = Curve.Keys[0].Z;
            }
            else
            {
                isConstant = false;
                PreType = Curve.PreBehavior.Value;
                PostType = Curve.PostBehavior.Value;
                MinTime = Curve.Keys[0].Time;
                MaxTime = Curve.Keys[Curve.Keys.Count - 1].Time;
            }

            Reset(); // reset values
        }

        public float GetRandom(float R)
        {
            float Max = R;
            float Min = -1 * R;

            return (float)RandomGen.NextDouble() * (Max - Min) + Min;
        }

        public void Reset()
        {
            double[] x = new double[Curve.Keys.Count()];
            double[] y = new double[Curve.Keys.Count()];
            for (int idx = 0; idx < x.Length; idx++)
            {
                x[idx] = Curve.Keys[idx].Time;
                if (isRandom)
                {
                    y[idx] = GetRandom(Curve.Keys[idx].Z);
                }
                else
                {
                    y[idx] = Curve.Keys[idx].Z;
                }
            }
            //
            if (y.Length == 0)
            {
                ConstantValue = 0;
            }
            else if (isConstant)
            {
                ConstantValue = (float)y[0];
            }
            else
            {
                spline1dbuildlinear(x, y, out Spline);
            }
        }

        public float GetTime(float time)
        {
            // behavior = Add, keep time the same
            // behavior = Repeat, take modulo
            // behavior = Const, take the last value

            float Diff = MaxTime - MinTime;
            if (time > MaxTime)
            {
                if (PostType == CurveBehavior.Const)
                {
                    return MaxTime;
                }
                else if (PostType == CurveBehavior.Repeat)
                {
                    return (time - MinTime) % Diff;
                }
            }
            if (time < MinTime)
            {
                if (PreType == CurveBehavior.Const)
                {
                    return MinTime;
                }
                else if (PreType == CurveBehavior.Repeat)
                {
                    return (time - MinTime) % Diff;
                }
            }
            return time;
        }

        public float GetValue(float time)
        {
            if (isConstant)
            {
                return ConstantValue;
            }
            return (float)spline1dcalc(Spline, GetTime(time));
        }


        // =======================
        // STATIC 
        // ======================
        // AXIS CONNECT
        public static void ConnectAxis(AxisConnect axisConnect, ref CurveRandomGroup X, ref CurveRandomGroup Y, ref CurveRandomGroup Z)
        {
            switch (axisConnect)
            {
                case AxisConnect.X_YZ:
                    Y = X;
                    Z = X;
                    break;
                case AxisConnect.X_Y:
                    Y = X;
                    break;
                case AxisConnect.X_Z:
                    Z = X;
                    break;
                case AxisConnect.Y_XZ:
                    X = Y;
                    Z = Y;
                    break;
                case AxisConnect.Y_X:
                    X = Y;
                    break;
                case AxisConnect.Y_Z:
                    Z = Y;
                    break;
                case AxisConnect.Z_XY:
                    X = Z;
                    Y = Z;
                    break;
                case AxisConnect.Z_X:
                    X = Z;
                    break;
                case AxisConnect.Z_Y:
                    Y = Z;
                    break;
            }
        }
        public static void ConnectAxis(AxisConnect axisConnect, ref CurveRandomGroup X, ref CurveRandomGroup Y)
        {
            switch (axisConnect)
            {
                case AxisConnect.X_YZ:
                    Y = X;
                    break;
                case AxisConnect.X_Y:
                    Y = X;
                    break;
                case AxisConnect.Y_XZ:
                    X = Y;
                    break;
                case AxisConnect.Y_X:
                    X = Y;
                    break;
            }
        }

        public static AVFXCurve SplitCurve(AVFXCurve curve, int idx)
        {
            AVFXCurve NewCurve = new AVFXCurve(curve.JSONPath, curve.AVFXName);
            NewCurve.Assigned = curve.Assigned;
            NewCurve.Random = curve.Random;
            NewCurve.PreBehavior = curve.PreBehavior;
            NewCurve.PostBehavior = curve.PostBehavior;

            List<AVFXKey> Keys = new List<AVFXKey>();
            foreach (AVFXKey key in curve.Keys)
            {
                float Val = 1.0f;
                switch (idx)
                {
                    case 0:
                        Val = key.X;
                        break;
                    case 1:
                        Val = key.Y;
                        break;
                    case 2:
                        Val = key.Z;
                        break;
                }
                Keys.Add(new AVFXKey(key.Type, key.Time, 1.0f, 1.0f, Val));
            }
            NewCurve.Keys = Keys;

            return NewCurve;
        }
    }
}
