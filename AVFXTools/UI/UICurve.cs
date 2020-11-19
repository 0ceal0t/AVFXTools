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
    public class UICurve
    {
        public AVFXCurve Curve;
        public bool Assigned = false;
        public string Name;
        public bool Color = false;
        //=======================
        public static readonly string[] PreBehaviorOptions = Enum.GetNames(typeof(CurveBehavior));
        public int PreBehaviorIdx;
        public static readonly string[] PostBehaviorOptions = Enum.GetNames(typeof(CurveBehavior));
        public int PostBehaviorIdx;
        public static readonly string[] RandomTypeOptions = Enum.GetNames(typeof(RandomType));
        public int RandomTypeIdx;

        public UIKey[] Keys;

        public UICurve(AVFXCurve curve, string name, bool color=false)
        {
            Curve = curve;
            if (!curve.Assigned) return;
            Assigned = true;
            Name = name;
            Color = color;
            //=====================
            PreBehaviorIdx = Array.IndexOf(PreBehaviorOptions, Curve.PreBehavior.Value);
            PostBehaviorIdx = Array.IndexOf(PostBehaviorOptions, Curve.PostBehavior.Value);
            if (!color)
            {
                RandomTypeIdx = Array.IndexOf(RandomTypeOptions, Curve.Random.Value);
            }
            Keys = new UIKey[Curve.Keys.Count];
            for(int i = 0; i < Keys.Length; i++)
            {
                Keys[i] = new UIKey(Curve.Keys[i], color);
            }
        }

        public void Draw(string id)
        {
            if (!Assigned) return;
            if (ImGui.TreeNode(Name + id))
            {
                if (UIUtils.EnumComboBox("Pre Behavior" + id, PreBehaviorOptions, ref PreBehaviorIdx))
                {
                    Curve.PreBehavior.GiveValue(PreBehaviorOptions[PreBehaviorIdx]);
                }
                if (UIUtils.EnumComboBox("Post Behavior" + id, PostBehaviorOptions, ref PostBehaviorIdx))
                {
                    Curve.PostBehavior.GiveValue(PostBehaviorOptions[PostBehaviorIdx]);
                }
                if (!Color)
                {
                    if (UIUtils.EnumComboBox("Random Type" + id, RandomTypeOptions, ref RandomTypeIdx))
                    {
                        Curve.Random.GiveValue(RandomTypeOptions[RandomTypeIdx]);
                    }
                }
                //==============
                if(ImGui.TreeNode("Keys" + id))
                {
                    for(int i = 0; i < Keys.Length; i++)
                    {
                        Keys[i].Draw(id + "-key" + i);
                    }
                    ImGui.TreePop();
                }
                ImGui.TreePop();
            }
        }
    }

    public class UIKey
    {
        public AVFXKey Key;

        public int Time;
        public Vector3 Data;
        public bool Color;
        public static readonly string[] TypeOptions = Enum.GetNames(typeof(KeyType));
        public int TypeIdx;

        public UIKey(AVFXKey key, bool color=false)
        {
            Key = key;
            Time = key.Time;
            Color = color;
            Data = new Vector3(key.X, key.Y, key.Z);
            TypeIdx = Array.IndexOf(TypeOptions, Key.Type.ToString());
        }

        public void Draw(string id)
        {
            if (ImGui.DragInt("Time" + id, ref Time, 1, 0))
            {
                Key.Time = Time;
            }
            if (UIUtils.EnumComboBox("Type" + id, TypeOptions, ref TypeIdx))
            {
                Enum.TryParse(TypeOptions[TypeIdx], out KeyType newKeyType);
                Key.Type = newKeyType;
            }
            //=====================
            if (Color)
            {
                if (ImGui.ColorEdit3("Color" + id, ref Data, ImGuiColorEditFlags.Float))
                {
                    Key.X = Data.X;
                    Key.Y = Data.Y;
                    Key.Z = Data.Z;
                }
            }
            else
            {
                if (ImGui.DragFloat3("Value" + id, ref Data))
                {
                    Key.X = Data.X;
                    Key.Y = Data.Y;
                    Key.Z = Data.Z;
                }
            }
        }
    }
}
