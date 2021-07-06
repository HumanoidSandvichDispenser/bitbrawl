using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using RedGrin;

namespace BitBrawl
{
    /// <summary>
    /// The core of the game. Handles metadata and global content
    /// </summary>
    public class GameCore : Core
    {
        /// <summary>
        /// <see langword="true"/> if the game is running as a server
        /// </summary>
        public static bool IsServer { get; private set; }

        /// <summary>
        /// Tickrate of the server (applies to BitBrawl.Server instances only)
        /// </summary>
        public static int ServerTickRate { get; } = 32;

        /// <summary>
        /// More readable way to express if the game is not running as a server (i.e. as a client). See <see cref="IsServer"/>
        /// </summary>
        public static bool IsClient => !IsServer;

        public static NetworkConfiguration NetworkConfiguration { get; set; }

        public GameCore(bool isServer) : base()
        {
            IsServer = isServer;

            Batcher.UseFnaHalfPixelMatrix = true;

            NetworkConfiguration = new NetworkConfiguration();
            NetworkConfiguration.ApplicationName = "BitBrawl";
            NetworkConfiguration.ApplicationPort = 7777;

            // we should only be able to reckon if we are the server
            // otherwise we are trusting other clients (which we don't want)
            if (IsServer)
                NetworkConfiguration.DeadReckonSeconds = 1 / 20f;

            NetworkConfiguration.EntityStateTypes = new System.Collections.Generic.List<System.Type>
            {
                typeof(Network.PlayerState),
                typeof(Network.PlayerInitState)
            };

            NetworkManager.Self.Connected += OnConnectedToServer;

            NetworkManager.Self.Initialize(NetworkConfiguration, Network.Logger.Instance);

            if (IsServer)
            {
                WindowTitle = "BitBrawl Server";
                NetworkManager.Self.Start(NetworkRole.Server);
                PauseOnFocusLost = false;
            }
            else
            {
                NetworkManager.Self.Start(NetworkRole.Client);
            }
        }

        private void OnConnectedToServer(long id, object data = null)
        {
            Scene = new Scenes.GameScene();
            NetworkManager.Self.GameArena = (RedGrin.Interfaces.INetworkArena)Scene;
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (IsServer)
            {
                Scene = new Scenes.GameScene();
                NetworkManager.Self.GameArena = (RedGrin.Interfaces.INetworkArena)Scene;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            NetworkManager.Self.Update();
        }

        [Nez.Console.Command("connect", "Connects to an address")]
        public static void ConnectToAddress(string address)
        {
            NetworkManager.Self.Connect(address);
        }

        [Nez.Console.Command("disconnect", "Disconnects from the current connection or drops all connection if host")]
        public static void Disconnect()
        {
            NetworkManager.Self.Disconnect();
        }

#if DEBUG
        [Nez.Console.Command("test", "Connects to localhost test server")]
        public static void Test()
        {
            NetworkManager.Self.Connect("localhost");
        }
#endif
    }
}
