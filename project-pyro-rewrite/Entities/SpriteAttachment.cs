using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Entities
{
    public enum AttachmentType
    {
        None,
        MageWarpPreview,
        MageSpell,
    }

    public class SpriteAttachment : Entity
    {
        /*
        public SpriteRenderer SpriteRenderer { get; set; }

        public SpriteAnimator SpriteAnimator => SpriteRenderer as SpriteAnimator;

        /// <summary>
        /// Creates a new sprite attachment (attached to a parent)
        /// </summary>
        /// <param name="parent">Parent entity</param>
        /// <param name="renderer">Sprite renderer (can be a normal SpriteRenderer or SpriteAnimator)</param>
        public SpriteAttachment(Entity parent, SpriteRenderer renderer)
        {
            Parent = parent.Transform;
            SpriteRenderer = AddComponent(renderer);
        }
        */
    }
}
