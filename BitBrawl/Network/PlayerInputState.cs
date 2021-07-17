using BitBrawl.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBrawl.Network
{
    /// <summary>
    /// Determines the state of the player's input
    /// </summary>
    public struct PlayerInputState : IState
    {
        /// <summary>
        /// Network ID of the owner of the Player entity
        /// </summary>
        public byte OwnerId;

        /// <summary>
        /// The client's current tick when this packet was sent
        /// </summary>
        public uint Tick;

        /// <summary>
        /// The X component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionX;

        /// <summary>
        /// The Y component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionY;

        public Vector2 HumanoidDirection
        {
            get
            {
                return new Vector2(HumanoidDirectionX, HumanoidDirectionY);
            }
            set
            {
                HumanoidDirectionX = value.X;
                HumanoidDirectionY = value.Y;
            }
        }

        public object FromObject(object obj)
        {
            if (obj is Entities.Player player)
            {
                HumanoidDirection = player.Humanoid.Controller.Direction;
            }

            return this;
        }

        public bool ShouldUpdate(IState previousState)
        {
            if (previousState is PlayerState previousPlayerState)
            {
                // if our direction changes more than 1/8 units, then this state should be updated on the server
                if (!(HumanoidDirection - previousPlayerState.HumanoidDirection).LengthSquared()
                    .IsApproximately(0, 1/64f))
                    return true;
            }

            return false;
        }

        public void ToObject(object obj, double time)
        {
            throw new NotImplementedException();
        }
    }
}
