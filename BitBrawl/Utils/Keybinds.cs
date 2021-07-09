using Nez;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BitBrawl.Utils
{
    public static class Keybinds
    {
        private static Dictionary<string, Keys> _keymaps;

        static Keybinds()
        {
            _keymaps = new Dictionary<string, Keys>();
            _keymaps.Add("moveup", Keys.W);
            _keymaps.Add("movedown", Keys.S);
            _keymaps.Add("moveleft", Keys.A);
            _keymaps.Add("moveright", Keys.D);
        }

        /// <summary>
        /// Returns whether or not the specified action 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool IsKeybindDown(string action)
        {
            if (_keymaps.ContainsKey(action))
            {
                return Input.IsKeyDown(_keymaps[action]);
            }

            return false;
        }

        public static bool IsKeybindPressed(string action)
        {
            if (_keymaps.ContainsKey(action))
            {
                return Input.IsKeyPressed(_keymaps[action]);
            }

            return false;
        }
    }
}
