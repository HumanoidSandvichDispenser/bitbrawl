using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using RedGrin;
using RedGrin.Interfaces;

namespace BitBrawl.Scenes
{
    public class GameScene : Scene, INetworkArena
    {
        public Entities.Player LocalPlayer => _localPlayer;

        private Entities.Player _localPlayer = null;
        private Components.Controllers.Controller _localPlayerController = null;

        public GameScene()
        {
            if (GameCore.IsClient)
            {
                _localPlayerController = new Components.Controllers.InputController();

                NetworkManager.Self.RequestCreateEntity(new Network.PlayerInitState
                {
                    OwnerId = NetworkManager.Self.ClientId
                });
            }
        }

        public INetworkEntity HandleCreateEntity(ulong uniqueId, object entityData)
        {
            INetworkEntity ent = null;

            if (entityData is Network.PlayerInitState playerInitState)
            {
                var sadegTex = Content.Load<Texture2D>("Sprites/Debug/Sadeg");

                var player = new Entities.Player();
                player.SetEntityId(playerInitState.OwnerId, player.Id & uint.MaxValue);
                Network.Logger.Instance.Info($"Creating entity owned by {player.GetClientId()}");
                player.Renderer = player.AddComponent(new SpriteRenderer(sadegTex));
                player.Position = new Vector2(250, 250); // TODO: replace this a player spawn function
                AddEntity(player);

                // if we own this entity, this entity should be our local player
                if (player.IsOwned())
                {
                    player.Humanoid.Controller = player.AddComponent(_localPlayerController);

                    _localPlayer = player;
                }
                else
                {
                    player.Humanoid.Controller = new Components.Controllers.Controller();
                }

                ent = player;
            }
            else if (entityData is Network.PlayerState playerState)
            {
                var sadegTex = Content.Load<Texture2D>("Sprites/Debug/Sadeg");

                var player = new Entities.Player();
                player.SetEntityId(playerState.OwnerId, player.Id & uint.MaxValue);
                System.Diagnostics.Debug.WriteLine($"Creating entity owned by {player.GetClientId()}");
                player.Renderer = player.AddComponent(new SpriteRenderer(sadegTex));
                player.Position = playerState.Position;
                AddEntity(player);

                // if we own this entity, this entity should be our local player
                if (player.IsOwned())
                {
                    player.Humanoid.Controller = player.AddComponent(_localPlayerController);

                    _localPlayer = player;
                }
                else
                {
                    player.Humanoid.Controller = new Components.Controllers.Controller();
                }

                ent = player;
            }

            return ent;
        }

        public void HandleDestroyEntity(INetworkEntity entity)
        {
            if (!(entity is Entity ent))
            {
                throw new System.Exception($"Invalid networked object.");
            }

            ent.Destroy();
        }

        public void HandleGenericMessage(ulong messageId, object message, double messageTime)
        {
            
        }
    }
}
