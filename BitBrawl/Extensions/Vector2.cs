using Microsoft.Xna.Framework;
using System;

namespace BitBrawl.Extensions
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Inverts the X component of the vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 InvertX(this ref Vector2 vector)
        {
            vector.X *= -1;
            return vector;
        }

        /// <summary>
        /// Inverts the Y component of the vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 InvertY(this ref Vector2 vector)
        {
            vector.Y *= -1;
            return vector;
        }

        /// <summary>
        /// Returns whether or not the X or Y component is not a number
        /// </summary>
        /// <param name="vec"></param>
        /// <returns><see langword="true"/> if any component of the Vector2 is not a number (<see cref="float.NaN"/>); otherwise <see langword="false"/></returns>
        public static bool IsNaN(this Vector2 vec)
        {
            return float.IsNaN(vec.X) || float.IsNaN(vec.Y);
        }

        /// <summary>
        /// Returns <paramref name="vector"/> where its length is clamped to the specified scalar range
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="minLength">Minimum scalar length</param>
        /// <param name="maxLength">Maximum scalar length</param>
        /// <returns></returns>
        public static Vector2 Clamp(this ref Vector2 vector, float minLength = 0f, float maxLength = 1f)
        {
            System.Diagnostics.Contracts.Contract.Requires(minLength >= 0f);
            System.Diagnostics.Contracts.Contract.Requires(maxLength >= 0f);

            if (vector.IsNaN())
                return vector;
            
            float lengthSquared = vector.LengthSquared();
            float minLengthSquared = (float)Math.Pow(minLength, 2);
            float maxLengthSquared = (float)Math.Pow(maxLength, 2);

            if (lengthSquared > maxLengthSquared)
            {
                vector.Normalize();
                vector *= maxLength;
            }
            else if (lengthSquared < minLengthSquared)
            {
                vector.Normalize();
                vector *= minLength;
            }

            return vector;
        }

        /// <summary>
        /// Returns the signed angle between two unit vectors
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static float AngleBetween(this Vector2 vectorA, Vector2 vectorB)
        {
            // must be unit vectors
            System.Diagnostics.Contracts.Contract.Requires(vectorA.LengthSquared().IsApproximately(1f));
            System.Diagnostics.Contracts.Contract.Requires(vectorB.LengthSquared().IsApproximately(1f));

            return ((float)Math.Acos(Vector2.Dot(vectorA, vectorB))).ToDegrees();

            // using law of cosines, theta = arccos((a^2 + b^2 - c^2)/(2ab))

            // simplified given a and b = 1,
            // and the vector of c is the magnitude difference between a and b
            //return (float)Math.Acos((double)(2 - (vectorA - vectorB).LengthSquared()) / 2);
        }
    }
}
