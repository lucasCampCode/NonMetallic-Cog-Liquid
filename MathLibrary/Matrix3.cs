using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MathLibrary
{
    public class Matrix3
    {
        //            x    y    w
        public float m11, m12, m13;
        public float m21, m22, m23;
        public float m31, m32, m33;

        public Matrix3()
        {
            m11 = 1; m12 = 0; m13 = 0;
            m21 = 0; m22 = 1; m23 = 0;
            m31 = 0; m32 = 0; m33 = 1;
        }
        public Matrix3(float m00, float m01, float m02,
                       float m10, float m11, float m12,
                       float m20, float m21, float m22)
        {
            this.m11 = m00; this.m12 = m01; this.m13 = m02;
            this.m21 = m10; this.m22 = m11; this.m23 = m12;
            this.m31 = m20; this.m32 = m21; this.m33 = m22;
        }
        public static Matrix3 operator +(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    left.m11 + right.m11, left.m12 + right.m12, left.m13 + right.m13,
                    left.m21 + right.m21, left.m22 + right.m22, left.m23 + right.m23,
                    left.m21 + right.m31, left.m32 + right.m32, left.m33 + right.m33
                );
        }
        public static Matrix3 operator -(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    left.m11 - right.m11, left.m12 - right.m12, left.m13 - right.m13,
                    left.m21 - right.m21, left.m22 - right.m22, left.m23 - right.m23,
                    left.m21 - right.m31, left.m32 - right.m32, left.m33 - right.m33
                );
        }
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    left.m11 * right.m11 + left.m12 * right.m21 + left.m13 * right.m31,
                    left.m11 * right.m12 + left.m12 * right.m22 + left.m13 * right.m32,
                    left.m11 * right.m13 + left.m12 * right.m23 + left.m13 * right.m33,

                    left.m21 * right.m11 + left.m22 * right.m21 + left.m23 * right.m31,
                    left.m21 * right.m12 + left.m22 * right.m22 + left.m23 * right.m32,
                    left.m21 * right.m13 + left.m22 * right.m23 + left.m23 * right.m33,

                    left.m31 * right.m11 + left.m32 * right.m21 + left.m33 * right.m31,
                    left.m31 * right.m12 + left.m32 * right.m22 + left.m33 * right.m32,
                    left.m31 * right.m13 + left.m32 * right.m23 + left.m33 * right.m33
                );
        }
        public static Matrix3 CreateRotation(float radians)
        {
            return new Matrix3((float)Math.Cos(radians),(float)Math.Sin(radians),0,
                               -(float)Math.Sin(radians),(float)Math.Cos(radians),0,
                               0,0,1);
        }

        public static Matrix3 CreateTranslation(Vector2 position)
        {
            return new Matrix3(1,0,position.X,
                               0,1,position.Y,
                               0,0,1);
        }
        public static Matrix3 CreateTranslation(float x,float y)
        {
            return new Matrix3(1, 0, x,
                               0, 1, y,
                               0, 0, 1);
        }
        
        public static Matrix3 CreateScale(Vector2 scale)
        {
            return new Matrix3(scale.X, 0, 0,
                               0, scale.Y, 0,
                               0, 0, 1);
        }
        public static Matrix3 CreateScale(float x,float y)
        {
            return new Matrix3(x,0,0,
                               0,y,0,
                               0,0,1);
        }
        public static Vector3 operator *(Matrix3 left, Vector3 right)
        {
            return new Vector3
                (
                    (left.m11 * right.X) + (left.m12 * right.Y) + (left.m13 * right.Z),
                    (left.m21 * right.X) + (left.m22 * right.Y) + (left.m23 * right.Z),
                    (left.m31 * right.X) + (left.m32 * right.Y) + (left.m33 * right.Z)
                );
        }
    } 
}
