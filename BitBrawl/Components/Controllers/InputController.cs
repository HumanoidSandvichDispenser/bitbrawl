using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace BitBrawl.Components.Controllers
{
    public class InputController : Controller, IUpdatable
    {
        public void Update()
        {
            Direction = Vector2.Zero;

            if (Input.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, -1);
            }

            if (Input.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, 1);
            }

            if (Input.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-1, 0);
            }

            if (Input.IsKeyDown(Keys.D))
            {
                Direction += new Vector2(1, 0);
            }

            if (Input.GamePads.Length > 0)
            {
                Direction += Input.GamePads[0].GetLeftStick();
            }
        }
    }
}
