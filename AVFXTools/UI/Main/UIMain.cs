using AVFXLib.Models;
using AVFXTools.Main;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

namespace AVFXTools.UI
{
    public class UIMain
    {
        public AVFXBase AVFX;
        public MainViewer Main;
        public ImGuiRenderer I;
        public GraphicsDevice GD;
        public CommandList CL;

        public UIParameterView ParameterView;
        public UIEffectorView EffectorView;
        public UIEmitterView EmitterView;
        public UIModelView ModelView;
        public UIParticleView ParticleView;
        public UITextureView TextureView;
        public UITimelineView TimelineView;
        public UIScheduleView ScheduleView;
        public UIBinderView BinderView;

        public UITextureBindings TextureBindings;
        public UIControls Controls;

        public UIMain(AVFXBase avfx, MainViewer main, ImGuiRenderer imgui, GraphicsDevice gd, CommandList cl)
        {
            AVFX = avfx;
            Main = main;
            I = imgui;
            GD = gd;
            CL = cl;
            // =========================
            TextureBindings = new UITextureBindings(this);
            // =========================
            ParticleView = new UIParticleView(avfx);
            ParameterView = new UIParameterView(avfx);
            BinderView = new UIBinderView(avfx);
            EmitterView = new UIEmitterView(avfx);
            TimelineView = new UITimelineView(avfx);
            TextureView = new UITextureView(avfx, TextureBindings);
            ModelView = new UIModelView(avfx);
            ScheduleView = new UIScheduleView(avfx);

            Controls = new UIControls(this);
            ImGui.SetNextWindowPos(new Vector2(10, 10));
            ImGui.SetNextWindowSize(new Vector2(400, 600));
        }

        public void Draw()
        {
            ImGui.Begin("AVFX");
            //================================
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.8f, 0, 0, 1));
            if (ImGui.Button("UPDATE"))
            {
                Main.refreshGraphics();
            }
            ImGui.PopStyleColor();
            //================================
            if (ImGui.BeginTabBar("##MainTabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton))
            {
                if (ImGui.BeginTabItem("Parameters"))
                {
                    ParameterView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Scheduler"))
                {
                    ScheduleView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Timelines"))
                {
                    TimelineView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Emitters"))
                {
                    EmitterView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Particles"))
                {
                    ParticleView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Effectors"))
                {
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Binders"))
                {
                    BinderView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Textures"))
                {
                    TextureView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Models"))
                {
                    ModelView.Draw();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
            Controls.Draw();
            I.Render(GD, CL);
        }
    }
}
