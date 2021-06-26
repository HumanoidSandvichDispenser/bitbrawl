using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;

namespace project_pyro_rewrite
{
    public class Game1 : Core
    {
        public static Nez.Timers.TimerManager TimerManager = new Nez.Timers.TimerManager();

        public Game1() : base(1600, 900, false, "Nez Engine")
        {
            Screen.HardwareModeSwitch = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            DebugRenderEnabled = true;
            Nez.Console.DebugConsole.RenderScale = 2;
            Batcher.UseFnaHalfPixelMatrix = true;

            Screen.SetSize(1600, 900);

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
