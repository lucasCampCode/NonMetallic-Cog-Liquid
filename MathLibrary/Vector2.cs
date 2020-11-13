using System;

namespace MathLibrary
{
    public class Vector2
    {
        private float _x;
        private float _y;

        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt((X * X) + (Y * Y));
            }
        }

        public Vector2 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        

        public Vector2()
        {
            _x = 0;
            _y = 0;
        }

        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Returns the normalized version of a the vector passed in.
        /// </summary>
        /// <param name="vector">The vector that will be normalized</param>
        /// <returns></returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            if (vector.Magnitude == 0)
                return new Vector2();
            return vector / vector.Magnitude;
        }

        /// <summary>
        /// Returns the dot product of the two vectors given.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float DotProduct(Vector2 left, Vector2 right)
        {

            return (left.X * right.X) + (left.Y * right.Y);
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2 operator *(Vector2 left, float scalar)
        {

            return new Vector2(left.X * scalar, left.Y * scalar);
        }
        public static Vector2 operator *(float scalar,Vector2 right)
        {

            return new Vector2(right.X * scalar, right.Y * scalar);
        }

        public static Vector2 operator /(Vector2 left, float scalar)
        {
            return new Vector2(left.X / scalar, left.Y / scalar);
        }
        public static Vector2 operator /( float scalar, Vector2 right)
        {
            return new Vector2(right.X / scalar, right.Y / scalar);
        }

        public static implicit operator Vector2((float, float) tuple)
        {
            return new Vector2(tuple.Item1, tuple.Item2);
        }
    }
}
