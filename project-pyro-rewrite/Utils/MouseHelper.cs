using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Utils
{
    [Flags]
    public enum MouseQuadrant
    {
        Right = 0b1,
        Bottom = 0b10,
    }

    public static class MouseHelper
    {
        public static byte GetMouseQuadrant()
        {
            byte flags = 0;
            Point mousePosition = Input.RawMousePosition;
            if (mousePosition.X > (Screen.Size.X / 2))
            {
                flags |= (byte)MouseQuadrant.Right;
            }
            if (mousePosition.Y > (Screen.Size.Y / 2))
            {
                flags |= (byte)MouseQuadrant.Bottom;
            }
            return flags;
        }
    }
}
