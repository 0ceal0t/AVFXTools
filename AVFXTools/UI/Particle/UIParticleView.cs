using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.UI
{
    public class UIParticleView : UIBase
    {
        List<UIParticle> Particles = new List<UIParticle>();

        public UIParticleView(AVFXBase avfx)
        {
            foreach (var particle in avfx.Particles)
            {
                Particles.Add(new UIParticle(particle));
            }
        }

        public override void Draw(string parentId = "")
        {
            string id = "##PTCL";
            int pIdx = 0;
            foreach(var particle in Particles)
            {
                particle.Idx = pIdx;
                particle.Draw(id);
                pIdx++;
            }
        }
    }
}
