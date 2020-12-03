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
    public class UICurve : UIBase
    {
        public AVFXCurve Curve;
        public string Name;
        public bool Color = false;
        //=======================
        public UIKey[] Keys;

        public UICurve(AVFXCurve curve, string name, bool color=false)
        {
            Curve = curve;
            Name = name;
            Color = color;
            if (!curve.Assigned) { Assigned = false; return; }
            //=====================
            Attributes.Add(new UICombo<CurveBehavior>("Pre Behavior", Curve.PreBehavior));
            Attributes.Add(new UICombo<CurveBehavior>("Post Behavior", Curve.PostBehavior));
            if (!color)
            {
                Attributes.Add(new UICombo<RandomType>("Random Type", Curve.Random));
            }
            Keys = new UIKey[Curve.Keys.Count];
            for(int i = 0; i < Keys.Length; i++)
            {
                Keys[i] = new UIKey(Curve.Keys[i], color);
            }
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/" + Name;
            // === UNASSIGNED ===
            if (!Assigned)
            {
                if (ImGui.Button("+ " + Name + id))
                {
                    // TODO
                }
                return;
            }
            // ==== ASSIGNED ===
            if (ImGui.TreeNode(Name + id))
            {
                if(UIUtils.RemoveButton("Delete" + id))
                {
                    // TODO
                }
                DrawAttrs(id);
                //==============
                if(ImGui.TreeNode("Keys" + id))
                {
                    int keyIdx = 0;
                    foreach(var key in Keys)
                    {
                        key.Idx = keyIdx;
                        key.Draw(id);
                        keyIdx++;
                    }

                    if (ImGui.Button("+ Key" + id))
                    {
                        // TODO
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
        public int Idx;
        // ====================
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

        public void Draw(string parentId)
        {
            string id = parentId + "/Key" + Idx;

            if (UIUtils.RemoveButton("Delete Key" + id))
            {
                // TODO
            }
            if (ImGui.InputInt("Time" + id, ref Time))
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
                if (ImGui.InputFloat3("Value" + id, ref Data))
                {
                    Key.X = Data.X;
                    Key.Y = Data.Y;
                    Key.Z = Data.Z;
                }
            }
        }
    }
}
