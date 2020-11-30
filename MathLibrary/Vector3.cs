using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class Vector3
    {
        private float _x;
        private float _y;
        private float _z;

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

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
            }
        }

        public Vector3 Normalized
        {
            get
            {
                return Normalize(this);
            }
        }



        public Vector3()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Vector3(float x, float y,float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Returns the normalized version of a the vector passed in.
        /// </summary>
        /// <param name="vector">The vector that will be normalized</param>
        /// <returns></returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            if (vector.Magnitude == 0)
                return new Vector3();
            return vector / vector.Magnitude;
        }

        /// <summary>
        /// Returns the dot product of the two vectors given.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float DotProduct(Vector3 left, Vector3 right)
        {

            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }
        /// <summary>
        /// returns teh cross product of the two vectors given
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector3 CrossProduct(Vector3 lhs,Vector3 rhs)
        {
            return new Vector3
                (
                    lhs.Y * rhs.Z - lhs.Z * rhs.Y,
                    lhs.Z * rhs.X - lhs.X * rhs.Z,
                    lhs.X * rhs.Y - lhs.Y * rhs.X
                );
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator *(Vector3 left, float scalar)
        {

            return new Vector3(left.X * scalar, left.Y * scalar, left.Z * scalar);
        }
        public static Vector3 operator *( float scalar,Vector3 right)
        {

            return new Vector3(right.X * scalar, right.Y * scalar, right.Z * scalar);
        }

        public static Vector3 operator /(Vector3 left, float scalar)
        {
            return new Vector3(left.X / scalar, left.Y / scalar, left.Z / scalar);
        }
        public static Vector3 operator /( float scalar, Vector3 right)
        {
            return new Vector3(right.X / scalar, right.Y / scalar, right.Z / scalar);
        }


        public static implicit operator Vector3((float, float, float) tuple)
        {
            return new Vector3(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        
    }
}
