using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Particles
{
    public static class DefaultParticles
    {
        public static Nez.Particles.ParticleEmitterConfig MageWarpParticle()
        {
            return new Nez.Particles.ParticleEmitterConfig
            {
                StartColor = Color.FromNonPremultiplied(145, 255, 255, 205),
                FinishColor = Color.FromNonPremultiplied(105, 205, 255, 50),
                StartParticleSize = 4,
                FinishParticleSize = 8,
                ParticleLifespan = 4,
                ParticleLifespanVariance = 1,
                Duration = 0.125f,
                EmissionRate = 16,
                SourcePositionVariance = new Vector2(16, 1),
                Gravity = new Vector2(0, 8),
                MaxRadiusVariance = 8
            };
        }
    }
}
