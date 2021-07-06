using BitBrawl.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using RedGrin.Interfaces;

namespace BitBrawl.Entities
{
    /// <summary>
    /// Players are special, rigid entities that contain all the information about a player.
    /// </summary>
    public class Player : Entity, IRenderable, INetworkEntity
    {
        ulong INetworkEntity.Id { get; set; }

        public string Username { get; set; }

        public Humanoid Humanoid { get; set; }

        public SpriteRenderer Renderer { get; set; }

        private Network.PlayerState _previousState = default;
        private Vector2 _lerpTo = default;
        

        public Player()
        {
            Humanoid = AddComponent(new Humanoid());
            Renderer = AddComponent(new SpriteAnimator());
        }

        public object GetState()
        {
            object state = new Network.PlayerState().FromObject(this);
            if (GameCore.IsServer)
            {
                System.Console.WriteLine(((Network.PlayerState)state).Position);
            }    
            return state;
        }

        public void UpdateFromState(object entityState, double stateTime, bool isReckoning)
        {
            if (entityState is Network.PlayerState playerState)
            {
                if (GameCore.IsServer || !(this as INetworkEntity).IsOwned())
                {
                    Humanoid.Controller.Direction = playerState.HumanoidDirection;
                }

                if (!GameCore.IsServer)
                {
                    float desyncDistance = (playerState.Position - Position).LengthSquared();

                    // snap player to its server position if their local position is too far
                    if (desyncDistance > 1024)
                    {
                        _lerpTo = default; // don't interpolate if we are far. otherwise bugs will occur
                        Debug.DrawPixel(Position, 4, Color.Red, 2);
                        Debug.DrawPixel(playerState.Position, 4, Color.Blue, 2);
                        Position = playerState.Position;
                    }
                    // but if we are a tiny bit off, interpolate to its correct position
                    else if (desyncDistance > 1)
                    {
                        _lerpTo = playerState.Position;
                    }
                }

                Renderer.FlipX = playerState.Flip;
            }
        }

        public override void Update()
        {
            base.Update();

            
            if (!RedGrin.NetworkManager.Self.IsConnected)
                return;

            if ((this as INetworkEntity).IsOwned())
            {
                var currentState = (Network.PlayerState)GetState();

                if (currentState.ShouldUpdate(_previousState))
                {
                    RedGrin.NetworkManager.Self.RequestUpdateEntity(this);
                    _previousState = (Network.PlayerState)GetState();
                }
            }
            else if (GameCore.IsClient)
            {
                if (_lerpTo != default)
                {
                    Position = Vector2.Lerp(Position, _lerpTo, 0.4f);
                }
            }
        }
    }
}
