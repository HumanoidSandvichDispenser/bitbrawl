using BitBrawl.Extensions;
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

            if (Utils.Keybinds.IsKeybindDown("moveup"))
            {
                Direction += new Vector2(0, -1);
            }

            if (Utils.Keybinds.IsKeybindDown("movedown"))
            {
                Direction += new Vector2(0, 1);
            }

            if (Utils.Keybinds.IsKeybindDown("moveleft"))
            {
                Direction += new Vector2(-1, 0);
            }

            if (Utils.Keybinds.IsKeybindDown("moveright"))
            {
                Direction += new Vector2(1, 0);
            }

            Vector2 stickDir = Input.GamePads[0].GetLeftStick();

            Direction += stickDir.InvertY();
        }
    }
}
