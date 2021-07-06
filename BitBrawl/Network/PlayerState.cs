using BitBrawl.Entities;
using BitBrawl.Extensions;
using Microsoft.Xna.Framework;
using RedGrin;
using RedGrin.Interfaces;

namespace BitBrawl.Network
{
    public struct PlayerState : IState
    {
        /// <summary>
        /// Network ID of the owner of the Player entity
        /// </summary>
        public byte OwnerId;

        /// <summary>
        /// The X component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionX;

        /// <summary>
        /// The Y component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionY;

        /// <summary>
        /// The player's position's X component
        /// </summary>
        public float PositionX;

        /// <summary>
        /// The player's position's Y component
        /// </summary>
        public float PositionY;

        /// <summary>
        /// Determines whether the player's sprite is flipped
        /// </summary>
        public bool Flip;

        public Vector2 HumanoidDirection => new Vector2(HumanoidDirectionX, HumanoidDirectionY);

        public Vector2 Position => new Vector2(PositionX, PositionY);

        public object FromObject(object obj)
        {
            if (obj is Player player)
            {
                OwnerId = player.GetClientId();
                PositionX = player.Position.X;
                PositionY = player.Position.Y;
                HumanoidDirectionX = player.Humanoid.Controller.Direction.X;
                HumanoidDirectionY = player.Humanoid.Controller.Direction.Y;
                Flip = player.Renderer.FlipX;
            }

            return this;
        }

        public void ToObject(object obj)
        {
            if (obj is Player player)
            {
                Vector2 directionToRealPos = Position - player.Position;

                if (!player.IsOwned())
                    player.Humanoid.Controller.Direction = new Vector2(HumanoidDirectionX, HumanoidDirectionY);

                if ((player.Position - Position).LengthSquared() > 16)
                    player.Position = Position;
                //else if ((player.Position - Position).LengthSquared() > 16)
                    

                player.Renderer.FlipX = Flip;
            }
        }

        public bool ShouldUpdate(IState previousState)
        {
            if (previousState is PlayerState previousPlayerState)
            {
                if ((HumanoidDirection - previousPlayerState.HumanoidDirection).LengthSquared() > 0.25f)
                    return true;
            }

            return false;
        }
    }
}
