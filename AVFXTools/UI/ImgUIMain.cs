using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AVFXTools.Main;
using ImGuiNET;
using Veldrid;

namespace AVFXTools.UI
{
    public class ImgUIMain
    {
        public Core C;
        public ImGuiRenderer I;
        public GraphicsDevice GD;
        public CommandList CL;

        public List<UIParticle> Particles = new List<UIParticle>();
        public List<UIEmitter> Emitters = new List<UIEmitter>();

        public ImgUIMain(Core core, ImGuiRenderer imgui, GraphicsDevice gd, CommandList cl)
        {
            C = core;
            I = imgui;
            GD = gd;
            CL = cl;

            ImGui.SetNextWindowPos(new Vector2(10, 10));
            ImGui.SetNextWindowSize(new Vector2(400, 600));

            foreach(var particle in C.Particles)
            {
                Particles.Add(new UIParticle(particle));
            }
            foreach(var emitter in C.Emitters)
            {
                Emitters.Add(new UIEmitter(emitter));
            }
        }

        public void Draw()
        {
            // ================================
            ImGui.Begin("AVFX");
            if (ImGui.BeginTabBar("##MainTabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton | ImGuiTabBarFlags.TabListPopupButton)) {
                if (ImGui.BeginTabItem("Parameters"))
                {
                    DrawParameters();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Scheduler"))
                {
                    DrawScheduler();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Timelines"))
                {
                    DrawTimelines();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Emitters"))
                {
                    DrawEmitters();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Particles"))
                {
                    DrawParticles();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Effectors"))
                {
                    DrawEffectors();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Binders"))
                {
                    DrawBinders();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Textures"))
                {
                    DrawTextures();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Models"))
                {
                    DrawModels();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }

            I.Render(GD, CL);
        }

        public void DrawParameters() { }
        public void DrawScheduler() { }
        public void DrawTimelines() { }

        // ====== EMITTERS ========
        public void DrawEmitters() {
            int idx = 0;
            foreach (var emitter in Emitters)
            {
                if (ImGui.CollapsingHeader("Emitter #" + idx + "(" + emitter.Emitter.EmitterType.Value + ")"))
                {
                    emitter.Draw("##emitter-" + idx);
                }
                idx++;
            }
        }
        // ====== PARTICLES ===========
        public void DrawParticles() {
            int idx = 0;
            foreach(var particle in Particles)
            {
                if(ImGui.CollapsingHeader("Particle #" + idx + "(" + particle.Particle.ParticleType.Value + ")"))
                {
                    particle.Draw("##particle-" + idx);
                }
                idx++;
            }
        }

        public void DrawEffectors() { }
        public void DrawBinders() { }
        public void DrawTextures() { }
        public void DrawModels() { }
    }
}
