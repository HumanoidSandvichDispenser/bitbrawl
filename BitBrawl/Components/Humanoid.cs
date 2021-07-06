using BitBrawl.Extensions;
using Microsoft.Xna.Framework;
using Nez;

namespace BitBrawl.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class Humanoid : Component, IUpdatable
    {
        public Vector2 Velocity { get; internal set; }

        public float Speed { get; set; } = 256;

        public float Friction { get; set; } = 0;

        public Controllers.Controller Controller { get; set; }

        public Humanoid(Controllers.Controller controller = null)
        {
            if (controller == null)
            {
                Controller = new Controllers.Controller { Direction = new Vector2(0, 0) };
            }
        }

        public void Update()
        {
            Vector2 accel = Vector2.Zero;

            if (!Controller.Direction.IsNaN() && Controller.Direction != Vector2.Zero)
            {
                Vector2 dir = Controller.Direction;
                accel = dir.Clamp() * Speed * (1 - Friction) * Time.DeltaTime;
            }

            // decelerate by friction
            Velocity *= Friction;

            // set velocity to zero if it is extremely small
            if (Velocity.LengthSquared() < 1 / 1024f)
                Velocity = Vector2.Zero;

            // accelerate
            if (!accel.IsNaN() && accel.LengthSquared() > 0)
                Velocity += accel;

            // clamp velocity
            Vector2 velocity = Velocity;
            velocity.Clamp(0, Speed); // can't directly use property
            Velocity = velocity;

            Entity.Position += Velocity;
        }
    }
}