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
    public class UIEmitterDataSphereModel : UIEmitterDataBase
    {
        public AVFXEmitterDataSphereModel Data;
        //==========================
        public static readonly string[] RotationOrderOptions = Enum.GetNames(typeof(RotationOrder));
        public int RotationOrderIdx;
        public static readonly string[] GenerateMethodOptions = Enum.GetNames(typeof(GenerateMethod));
        public int GenerateMethodIdx;
        public int DivideX;
        public int DivideY;

        public UICurve Radius;

        public UIEmitterDataSphereModel(AVFXEmitterDataSphereModel data)
        {
            Data = data;
            //=======================
            RotationOrderIdx = Array.IndexOf(RotationOrderOptions, Data.RotationOrderType.Value);
            GenerateMethodIdx = Array.IndexOf(GenerateMethodOptions, Data.GenerateMethodType.Value);

            DivideX = Data.DivideX.Value;
            DivideY = Data.DivideY.Value;
            //=======================
            Radius = new UICurve(Data.Radius, "Radius");
        }

        public override void Draw(string id)
        {
            if (ImGui.TreeNode("Data" + id))
            {
                if (UIUtils.EnumComboBox("Rotation Orrder" + id, RotationOrderOptions, ref RotationOrderIdx))
                {
                    Data.RotationOrderType.GiveValue(RotationOrderOptions[RotationOrderIdx]);
                }
                if (UIUtils.EnumComboBox("Generate Method" + id, GenerateMethodOptions, ref GenerateMethodIdx))
                {
                    Data.GenerateMethodType.GiveValue(GenerateMethodOptions[GenerateMethodIdx]);
                }
                if (ImGui.DragInt("Divide X" + id, ref DivideX, 1, 0))
                {
                    Data.DivideX.GiveValue(DivideX);
                }
                if (ImGui.DragInt("Divide Y" + id, ref DivideY, 1, 0))
                {
                    Data.DivideY.GiveValue(DivideY);
                }
                //=================
                Radius.Draw(id + "-radius");

                ImGui.TreePop();
            }
        }
    }
}
