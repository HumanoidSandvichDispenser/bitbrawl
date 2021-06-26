using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Scenes
{
    public class LoadingScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.FromNonPremultiplied(new Vector4(0.5f, 0.5f, 0.5f, 1));

            Texture2D texture = Content.Load<Texture2D>("Sprites/Debug/PepegaCoin");
            Entity ent = CreateEntity("icon");
            ent.AddComponent(new SpriteRenderer(texture));
            ent.Transform.LocalPosition = new Vector2(100);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
