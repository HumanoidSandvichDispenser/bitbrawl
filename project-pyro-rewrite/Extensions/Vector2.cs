using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Extensions
{
    public static class Vector2Extensions
    {
        public static bool IsNaN(this Vector2 vector2)
        {
            return float.IsNaN(vector2.X) || float.IsNaN(vector2.Y);
        }
    }
}
