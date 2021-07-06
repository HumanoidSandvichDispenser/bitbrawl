using BitBrawl.Entities;
using Microsoft.Xna.Framework;
using RedGrin;

namespace BitBrawl.Network
{
    public struct PlayerInitState : IState
    {
        /// <summary>
        /// Network ID of the owner of the Player entity
        /// </summary>
        public byte OwnerId { get; set; }

        public object FromObject(object obj)
        {
            return this;
        }

        public void ToObject(object obj)
        {
            
        }

        public bool ShouldUpdate(IState previousState) => true;
    }
}
