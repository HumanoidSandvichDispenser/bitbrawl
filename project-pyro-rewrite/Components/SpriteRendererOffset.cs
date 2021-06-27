using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class SpriteRendererOffset : SpriteRenderer
    {
        public SpriteRendererOffset(Texture2D texture) : base(texture)
        {

        }

        public Vector2 Offset { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0;

        public override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(Sprite, Entity.Transform.Position + LocalOffset + Offset, Color,
                Entity.Transform.Rotation, Origin, Entity.Transform.Scale, SpriteEffects, _layerDepth);
        }
    }

    
    public class SpriteAnimatorOffset : SpriteAnimator
    {
        public SpriteAnimatorOffset(Texture2D texture) : base()
        {

        }

        public Vector2 Offset { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0;

        public override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(Sprite, Entity.Transform.Position + LocalOffset + Offset, Color,
                Entity.Transform.Rotation, Origin, Entity.Transform.Scale, SpriteEffects, _layerDepth);
        }
    }
    
}
