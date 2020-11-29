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
    public class UIBinderProperties : UIBase
    {
        public AVFXBinderProperty Prop;
        //===================
        // TODO: Name

        public UIBinderProperties(AVFXBinderProperty prop)
        {
            Prop = prop;
            //====================
            Attributes.Add(new UICombo<BindPoint>("Bind Point Type", Prop.BindPointType));
            Attributes.Add(new UICombo<BindTargetPoint>("Bind Target Point Type", Prop.BindTargetPointType));
            Attributes.Add(new UIInt("Bind Point Id", Prop.BindPointId));
            Attributes.Add(new UIInt("Generate Delay", Prop.GenerateDelay));
            Attributes.Add(new UIInt("Coord Update Frame", Prop.CoordUpdateFrame));
            Attributes.Add(new UICheckbox("Ring Enabled", Prop.RingEnable));
            Attributes.Add(new UIInt("Ring Progress Time", Prop.RingProgressTime));
            Attributes.Add(new UIFloat3("Ring Position", Prop.RingPositionX, Prop.RingPositionY, Prop.RingPositionZ));
            Attributes.Add(new UIFloat("Ring Radius", Prop.RingRadius));
            Attributes.Add(new UICurve3Axis(Prop.Position, "Position"));
        }

        public override void Draw(string parentId)
        {
            string id = parentId + "/PROP";
            if (ImGui.TreeNode("Properties" + id))
            {
                DrawAttrs(id);
                ImGui.TreePop();
            }
        }
    }
}
