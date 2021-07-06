using Nez;
using Nez.Sprites;

namespace BitBrawl.Entities
{
    /// <summary>
    /// Interface that can be added to an Entity that requires a SpriteRenderer component
    /// </summary>
    public interface IRenderable
    {
        public SpriteRenderer Renderer { get; set; }
    }
}
