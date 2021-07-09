using BitBrawl.Entities;
using BitBrawl.Extensions;
using Microsoft.Xna.Framework;
using Nez;
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
        /// The server's current tick when this packet was sent
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

        public Vector2 Position
        {
            get
            {
                return new Vector2(PositionX, PositionY);
            }
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
            }
        }

        public object FromObject(object obj)
        {
            if (obj is Player player)
            {
                Tick = NetworkManager.Self.GetTick();
                OwnerId = player.GetClientId();
                PositionX = player.Position.X;
                PositionY = player.Position.Y;
                HumanoidDirectionX = player.Humanoid.Controller.Direction.X;
                HumanoidDirectionY = player.Humanoid.Controller.Direction.Y;
                Flip = player.Renderer.FlipX;
            }

            return this;
        }

        public void ToObject(object obj, double latency)
        {
            if (obj is Player player)
            {
                if (GameCore.IsServer || !player.IsOwned())
                {
                    DebugConsole.Logger.Instance.Log($"[Server] [{NetworkManager.Self.GetTick()}] Players's server position is {player.Position}");
                    player.Humanoid.Controller.Direction = HumanoidDirection;
                }

                // all clients may experience divergence
                // server reconciliation code
                if (!GameCore.IsServer)
                {
                    float divergence = (player.Position - Position).LengthSquared();

                    // for debug purposes to show where the player was on the server
                    if (player._debugServerPlayer != null)
                        player._debugServerPlayer.Position = Position;

                    if (player.IsOwned())
                    {
                        DebugConsole.Logger.Instance.Log($"[Client: {NetworkManager.Self.GetTick()}, Server: {Tick}] Players's server position is {Position}");

                        var previousState = player._stateBuffer[Tick % 1024];

                        if (previousState.Tick != Tick)
                            return;

                        divergence = (Position - previousState.Position).LengthSquared();

                        //DebugConsole.Logger.Instance.Debug($"Old position: {player.Position}");
                        //DebugConsole.Logger.Instance.Debug($"Rewind position: {player._stateBuffer[Tick % 1024].Position}");

                        if (!divergence.IsApproximately(0))
                        {
                            // reconciliation:
                            // rewind the client back to its state when the original state was received
                            // resimulate the player's movement as it loops through its state history
                            // until it reaches the present

                            //player.Position = player._stateBuffer[Tick % 1024].Position;
                            
                            uint rewindTick = Tick;
                            uint currentTick = NetworkManager.Self.GetTick();
                            uint deltaTicks = 1;

                            while (rewindTick < currentTick)
                            {
                                uint bufferSlot = rewindTick % 1024;

                                // some ticks may be skipped due to low client fps
                                // ensure that the packet's state exists in the state buffer (by checking if the ticks are recent)
                                if (rewindTick == player._stateBuffer[bufferSlot].Tick)
                                {
                                    if (rewindTick == Tick)
                                    {
                                        DebugConsole.Logger.Instance.Debug($"At tick {Tick} I was at {Position}");
                                        player.Position = Position;
                                    }

                                    player.Humanoid.Controller.Direction = player._stateBuffer[bufferSlot].HumanoidDirection;
                                    Vector2 oldPos = player._stateBuffer[bufferSlot].Position;
                                    Vector2 newPos = player.Humanoid.Simulate(deltaTicks / (float)GameCore.ServerTickRate);
                                    player._stateBuffer[bufferSlot].Position = newPos;

                                    if (rewindTick == currentTick - 1)
                                    {
                                        DebugConsole.Logger.Instance.Debug($"[{Tick}] Projected position from ticks {rewindTick} to {currentTick}: {oldPos} to {newPos}");
                                    }
                                    deltaTicks = 1;
                                }
                                else
                                {
                                    deltaTicks++;
                                }

                                rewindTick++;
                            }
                            //DebugConsole.Logger.Instance.Debug($"New position: {player.Position}");
                        }
                    }
                    else
                    {
                        // snap player to its server position if their local position is too far
                        if (divergence > 1024)
                        {
                            player._lerpTo = default; // don't interpolate if we are far. otherwise bugs will occur
                            player.Position = Position;
                        }
                        // but if we are a tiny bit off, interpolate to its correct position
                        else if (divergence > 1)
                        {
                            player._lerpTo = Position;
                        }
                    }
                }

                player.Renderer.FlipX = Flip;
            }
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
    }
}
