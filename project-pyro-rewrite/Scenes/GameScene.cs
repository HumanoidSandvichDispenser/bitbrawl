using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Console;
using Nez.Sprites;
using Nez.Tiled;
using project_pyro_rewrite.Entities;
using project_pyro_rewrite.Components;
using Nez.UI;

namespace project_pyro_rewrite.Scenes
{
    public class GameScene : Scene
    {
        public TmxMap TiledMap { get; private set; }
        public TiledMapRenderer TiledMapRenderer { get; private set; }

        private Player _localPlayer = null;
        private Entity _map = null;

        public GameScene()
        {
            ClearColor = Color.DarkGray;
            SetDesignResolution(1920, 1080, SceneResolutionPolicy.ShowAll);
        }

        public override void Initialize()
        {
            base.Initialize();

            TiledMap = Content.LoadTiledMap("Content/Maps/Pepega.tmx");
            _map = CreateEntity("map");
            var mapRenderer = new TiledMapRenderer(TiledMap, "Foreground");
            mapRenderer.SetLayersToRender(new string[] { "Details", "Foreground", "Background" });
            mapRenderer.SetRenderLayer((int)Utils.RenderLayer.TileMap);
            TiledMapRenderer = mapRenderer;
            _map.AddComponent(mapRenderer);

            _localPlayer = new Player("localplayer", mapRenderer.CollisionLayer);
            AddEntity(_localPlayer.MakeControllablePlayer());
        }

        public Vector2 FindSpawn(PlayerTeam team)
        {
            var spawns = TiledMapRenderer.TiledMap.GetObjectGroup("Spawns");

            if (spawns is null)
                return Vector2.Zero;

            if (team == PlayerTeam.Purple)
            {
                if (spawns.Objects.Contains("Purple"))
                {
                    var spawn = spawns.Objects["Purple"];
                    return new Vector2(spawn.X, spawn.Y);
                }
            }
            else if (team == PlayerTeam.Yellow)
            {
                if (spawns.Objects.Contains("Yellow"))
                {
                    var spawn = spawns.Objects["Yellow"];
                    return new Vector2(spawn.X, spawn.Y);
                }
            }

            return Vector2.Zero;
        }
    }

    public static class GameSceneCommands
    {
        [Command("bot", "adds a bot")]
        public static void Add(string name = "Bot")
        {
            if (Core.Scene is GameScene scene)
            {
                Player plr = new Player(name, scene.TiledMapRenderer.CollisionLayer);
                plr.InputController = plr.AddComponent(new BotController
                {
                    MapCollisionLayer = scene.TiledMapRenderer.CollisionLayer
                });

                plr.AttachToScene(scene);

                Core.Schedule(0.1f, (t) =>
                {
                    plr.Spawn();
                });
            }
        }

        [Command("rename-entity", "Renames an entity")]
        private static void RenameEntity(string name, string newName)
        {
            if (Core.Scene is GameScene scene)
            {
                Entity targetPlayer = scene.FindEntity(name);
                if (targetPlayer == null)
                {
                    DebugConsole.Instance.Log($"Target player \"{name}\" does not exist");
                    return;
                }
                targetPlayer.Name = newName;
            }
            else
            {
                DebugConsole.Instance.Log("You can not use this command while not in-game");
            }
        }

        [Command("bring-entity", "Places an entity (by name) to the local player's position")]
        private static void BringEntity(string name)
        {
            if (Core.Scene is GameScene scene)
            {
                Entity localPlayer = scene.FindEntity("localplayer");
                Entity targetPlayer = scene.FindEntity(name);
                if (localPlayer == null)
                {
                    DebugConsole.Instance.Log("Local player does not exist");
                    return;
                }
                if (targetPlayer == null)
                {
                    DebugConsole.Instance.Log($"Target player \"{name}\" does not exist");
                    return;
                }
                targetPlayer.Position = localPlayer.Position;
            }
            else
            {
                DebugConsole.Instance.Log("You can not use this command while not in-game");
            }
        }

        [Command("move-entity", "Teleports an entity to another entity's position")]
        private static void MoveEntity(string from, string to)
        {
            if (Core.Scene is GameScene scene)
            {
                Entity fromPlayer = scene.FindEntity(from);
                Entity targetPlayer = scene.FindEntity(to);
                if (fromPlayer == null)
                {
                    DebugConsole.Instance.Log($"Target player \"{from}\" does not exist");
                    return;
                }
                if (targetPlayer == null)
                {
                    DebugConsole.Instance.Log($"Target player \"{to}\" does not exist");
                    return;
                }
                fromPlayer.Position = targetPlayer.Position;
            }
            else
            {
                DebugConsole.Instance.Log("You can not use this command while not in-game");
            }
        }
    }
}
