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
            EffectorView = new UIEffectorView(avfx);
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
            SetupTheme();
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
                if (ImGui.BeginTabItem("Parameters##Main"))
                {
                    ParameterView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Scheduler##Main"))
                {
                    ScheduleView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Timelines##Main"))
                {
                    TimelineView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Emitters##Main"))
                {
                    EmitterView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Particles##Main"))
                {
                    ParticleView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Effectors##Main"))
                {
                    EffectorView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Binders##Main"))
                {
                    BinderView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Textures##Main"))
                {
                    TextureView.Draw();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Models##Main"))
                {
                    ModelView.Draw();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
            Controls.Draw();
            I.Render(GD, CL);
        }

        public void SetupTheme()
        {
            ImGui.GetStyle().GrabRounding = 3f;
            ImGui.GetStyle().FrameRounding = 4f;
            ImGui.GetStyle().WindowRounding = 4f;
            ImGui.GetStyle().WindowBorderSize = 0f;
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.Right;
            ImGui.GetStyle().ScrollbarSize = 16f;

            ImGui.GetStyle().Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.40f, 0.40f, 0.40f, 0.87f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.29f, 0.29f, 0.29f, 0.54f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.54f, 0.54f, 0.54f, 0.40f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.64f, 0.64f, 0.64f, 0.67f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.29f, 0.29f, 0.29f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.86f, 0.86f, 0.86f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.54f, 0.54f, 0.54f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.67f, 0.67f, 0.67f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.Button] = new Vector4(0.71f, 0.71f, 0.71f, 0.40f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.47f, 0.47f, 0.47f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.74f, 0.74f, 0.74f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.Header] = new Vector4(0.59f, 0.59f, 0.59f, 0.31f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.50f, 0.50f, 0.50f, 0.80f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.60f, 0.60f, 0.60f, 1.00f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.79f, 0.79f, 0.79f, 0.25f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.78f, 0.78f, 0.78f, 0.67f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.88f, 0.88f, 0.88f, 0.95f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.Tab] = new Vector4(0.23f, 0.23f, 0.23f, 0.86f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.71f, 0.71f, 0.71f, 0.80f);
            ImGui.GetStyle().Colors[(int)ImGuiCol.TabActive] = new Vector4(0.36f, 0.36f, 0.36f, 1.00f);
        }
    }
}
