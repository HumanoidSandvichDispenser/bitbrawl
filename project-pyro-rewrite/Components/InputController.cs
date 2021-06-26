using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Components
{
    public class InputController : Component, IUpdatable
    {
        public Entities.Player Player => Entity as Entities.Player;

        /// <summary>
        /// The direction the character should move
        /// </summary>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// The point the character is looking at
        /// </summary>
        public Vector2 Target { get; set; }

        public bool IsBot { get; set; } = false;

        public virtual void Update()
        {
            // Set target
            var camera = Entity.GetComponent<FollowCamera>(true);
            if (camera != null && camera.Camera != null)
            {
                Target = camera.Camera.MouseToWorldPoint();
            }

            // Set direction
            Vector2 direction = Vector2.Zero;

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                direction.Y -= 1;
            }

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                direction.Y += 1;
            }

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                direction.X -= 1;
            }

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                direction.X += 1;
            }

            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
            {
                if (Player.IsAlive)
                {
                    Player.Kill();
                }
            }

            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                Player.Spawn();
            }

            if (Input.RightMouseButtonPressed)
            {
                Player.StartAbility();
            }

            if (Input.RightMouseButtonReleased)
            {
                Player.StopAbility();
            }

            Direction = direction;
        }

        private void BotInput()
        {
            
        }

        private void PlayerInput()
        {
            
        }
    }
}
