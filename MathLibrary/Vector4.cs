using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class Vector4
    {
        private float _x;
        private float _y;
        private float _z;
        private float _w;

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
        public float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }
        public float W
        {
            get
            {
                return _w;
            }
            set
            {
                _w = value;
            }
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
            }
        }

        public Vector4 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }



        public Vector4()
        {
            _x = 0;
            _y = 0;
            _z = 0;
            _w = 0;
        }

        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>
        /// Returns the normalized version of a the vector passed in.
        /// </summary>
        /// <param name="vector">The vector that will be normalized</param>
        /// <returns></returns>
        public static Vector4 Normalize(Vector4 vector)
        {
            if (vector.Magnitude == 0)
                return new Vector4();
            return vector / vector.Magnitude;
        }

        /// <summary>
        /// Returns the dot product of the two vectors given.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float DotProduct(Vector4 left, Vector4 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }
        /// <summary>
        /// returns teh cross product of the two vectors given
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector4 CrossProduct(Vector4 lhs, Vector4 rhs)
        {
            return new Vector4
                (
                    lhs.Y * rhs.Z - lhs.Z * rhs.Y,
                    lhs.Z * rhs.X - lhs.X * rhs.Z,
                    lhs.X * rhs.Y - lhs.Y * rhs.X,
                    0
                );
        }

        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        public static Vector4 operator *(Vector4 left, float scalar)
        {

            return new Vector4(left.X * scalar, left.Y * scalar, left.Z * scalar, left.W * scalar);
        }
        public static Vector4 operator *(float scalar, Vector4 left)
        {

            return new Vector4(left.X * scalar, left.Y * scalar, left.Z * scalar, left.W * scalar);
        }

        public static Vector4 operator /(Vector4 left, float scalar)
        {
            return new Vector4(left.X / scalar, left.Y / scalar, left.Z / scalar, left.W / scalar);
        }
        public static Vector4 operator /(float scalar, Vector4 right)
        {
            return new Vector4(right.X / scalar, right.Y / scalar, right.Z / scalar, right.W / scalar);
        }


        public static implicit operator Vector4((float, float, float, float) tuple)
        {
            return new Vector4(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

    }
}