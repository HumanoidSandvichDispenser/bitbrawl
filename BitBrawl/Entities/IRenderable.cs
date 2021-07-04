using System;
using System.Collections.Generic;
using System.Text;

namespace BitBrawl.Entities
{
    public interface IRenderable
    {
        public Nez.Sprites.SpriteRenderer Renderer { get; set; }
    }
}
