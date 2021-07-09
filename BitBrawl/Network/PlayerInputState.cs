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
        /// The X component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionX;

        /// <summary>
        /// The Y component of the direction the player's humanoid is traveling in
        /// </summary>
        public float HumanoidDirectionY;

        public object FromObject(object obj)
        {
            throw new NotImplementedException();
        }

        public bool ShouldUpdate(IState previousState)
        {
            throw new NotImplementedException();
        }

        public void ToObject(object obj, double time)
        {
            throw new NotImplementedException();
        }
    }
}
