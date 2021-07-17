using BitBrawl.Components;
using BitBrawl.Extensions;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using RedGrin.Interfaces;
using System.Collections.Generic;

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

        internal Vector2 _lerpTo = default;
        private Network.PlayerInputState _previousState = default;
        internal Entity _debugServerPlayer = null;
        //internal SortedDictionary<int, Network.PlayerState> _stateBuffer = new SortedDictionary<int, Network.PlayerState>();
        internal Network.PlayerState[] _stateBuffer = new Network.PlayerState[1024];
        internal Network.PlayerInputState[] _inputBuffer = new Network.PlayerInputState[1024];
        

        public Player()
        {
            Humanoid = AddComponent(new Humanoid());
            Renderer = AddComponent(new SpriteAnimator());
            /*
            var effect = new Sprite();
            effect.LineColor = Color.Red;
            Renderer.Material = new Material(effect);
            */
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            _debugServerPlayer = Scene.CreateEntity("localpreview");
            _debugServerPlayer.AddComponent(new SpriteRenderer(Renderer.Sprite.Texture2D) { Color = Color.FromNonPremultiplied(255, 50, 255, 150) });
        }

        public object GetState()
        {
            object state = null;
            if (GameCore.IsServer)
            {
                // send the full state of the client (including position) as the server
                var playerState = (Network.PlayerState)new Network.PlayerState().FromObject(this);
                playerState.Tick = 0;
            }
            else if ((this as INetworkEntity).IsOwned())
            {
                // we only want to send player input as the client
                state = new Network.PlayerInputState().FromObject(this);
            }
            else
            {
                throw new System.Exception("This client does not have control over this entity.");
            }
            return state;
        }

        public void UpdateFromState(object entityState, double stateTime, bool isReckoning)
        {
            if (entityState is Network.IState state)
            {
                state.ToObject(this, RedGrin.NetworkManager.Self.ServerTime - stateTime);
            }
        }

        public override void Update()
        {
            base.Update();

            
            if (!RedGrin.NetworkManager.Self.IsConnected)
                return;

            var currentState = (Network.PlayerState)GetState();

            /*
            if ((this as INetworkEntity).IsOwned() || GameCore.IsServer)
            {
                uint bufferSlot = currentState.Tick % 1024;
                _stateBuffer[bufferSlot] = currentState;

                if (currentState.ShouldUpdate(_previousState))
                {
                    RedGrin.NetworkManager.Self.RequestUpdateEntity(this);
                    _previousState = (Network.PlayerState)GetState();

                    if (GameCore.IsServer)
                    {
                        DebugConsole.Logger.Instance.Info($"[{RedGrin.NetworkManager.Self.GetTick()}] Sending player info");
                    }
                }
            }
            */
            if ((this as INetworkEntity).IsOwned())
            {
                uint bufferSlot = currentState.Tick % 1024;
                _stateBuffer[bufferSlot] = (Network.PlayerState)new Network.PlayerState().FromObject(this);
                _inputBuffer[bufferSlot] = (Network.PlayerInputState)new Network.PlayerInputState().FromObject(this);

                if (_stateBuffer[bufferSlot].ShouldUpdate(_previousState))
                {
                    // on clients, updating the requested entity only sends PlayerInputStates
                    RedGrin.NetworkManager.Self.RequestUpdateEntity(this);
                }
            }
            if (GameCore.IsClient)
            {
                if (_lerpTo != default)
                {
                    float desyncDistance = (Position - _lerpTo).LengthSquared();

                    if (desyncDistance > 16)
                        Position = Vector2.Lerp(Position, _lerpTo, 1/8f);
                    else if (desyncDistance > 4)
                        Position = Vector2.Lerp(Position, _lerpTo, 1/16f);
                    else if (desyncDistance > 1)
                        Position = Vector2.Lerp(Position, _lerpTo, 1/32f);

                    Debug.DrawPixel(_lerpTo, 4, Color.Purple);
                    //Debug.DrawPixel(Position, in)
                }
            }
        }
    }
}
