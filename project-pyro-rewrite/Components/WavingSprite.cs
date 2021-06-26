using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class WavingSprite : Component, IUpdatable
    {
        public bool WaveX { get; set; } = false;

        public bool WaveY { get; set; } = false;

        public Vector2 Period { get; set; } = new Vector2(2, 1);

        public Vector2 PhaseOffset { get; set; } = new Vector2((float)Math.PI, 0);

        public Vector2 Amplitude { get; set; } = Vector2.One;

        private SpriteRenderer _renderer;

        public override void OnAddedToEntity()
        {
            _renderer = Entity.GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            Vector2 offset = Vector2.Zero;
            if (WaveX)
            {
                // sine function with x components
                offset.X = (float)(Amplitude.X * Math.Sin(Time.TotalTime *
                    (float)(2 * Math.PI / Period.X) + PhaseOffset.X));
            }
            if (WaveY)
            {
                // sine function with y components
                offset.Y = (float)(Amplitude.Y * Math.Sin(Time.TotalTime *
                    (float)(2 * Math.PI / Period.Y) + PhaseOffset.Y));
            }
            _renderer.RenderOffset = offset;
        }
    }
}
