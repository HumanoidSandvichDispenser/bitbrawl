using BitBrawl.Components;
using Nez;

namespace BitBrawl.Entities
{
    /// <summary>
    /// Players are special, rigid entities that contain all the information about a player.
    /// </summary>
    public class Player : Entity
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public Humanoid Humanoid { get; set; }
    }
}
