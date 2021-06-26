using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class Decoration : Component
    {
        public Texture2D Texture { get; set; }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
        }
    }
}
