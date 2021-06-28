using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Utils
{
    public static class VectorPrediction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displacement">Vector between the user and the target</param>
        /// <param name="vel">Velocity of the target</param>
        /// <param name="projectileSpeed">Speed of the projectile</param>
        /// <returns>Displacement vector</returns>
        public static Vector2 PredictVector(Vector2 displacement, Vector2 vel, float projectileSpeed)
        {
            if (vel.LengthSquared() == 0 || float.IsNaN(vel.LengthSquared()) || projectileSpeed == 0)
                return Vector2.Zero;

            float time = (displacement / projectileSpeed).Length();
            return vel * time;
        }
    }
}
