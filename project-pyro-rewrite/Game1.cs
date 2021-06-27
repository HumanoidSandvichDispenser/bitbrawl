using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;

namespace project_pyro_rewrite
{
    public class Game1 : Core
    {
        public Game1() : base(1600, 900, false, "Nez Engine")
        {
            //Screen.HardwareModeSwitch = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // load settings
            Utils.Configuration config = Utils.Settings.ReadFromFile();
            Screen.SetSize(config.Width, config.Height);
            Screen.IsFullscreen = config.Fullscreen;
            Screen.HardwareModeSwitch = config.HardwareModeSwitch;

            DebugRenderEnabled = true;
            Nez.Console.DebugConsole.RenderScale = 2;
            Batcher.UseFnaHalfPixelMatrix = true;  // render fonts correctly

            // Create default scene
            var loadingScene = new Scenes.LoadingScene();

            // set current scene to loading screen
            Scene = loadingScene;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Input.IsKeyPressed(Keys.Enter) && !(Scene is Scenes.GameScene))
            {
                Scene = new Scenes.GameScene();
            }
        }
    }
}
