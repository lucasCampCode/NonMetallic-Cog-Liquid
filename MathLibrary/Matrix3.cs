using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MathLibrary
{
    public class Matrix3
    {
        //            x    y    w
        public float m00, m01, m02;
        public float m10, m11, m12;
        public float m20, m21, m22;

        public Matrix3()
        {
            m00 = 1; m01 = 0; m02 = 0;
            m10 = 0; m11 = 1; m12 = 0;
            m20 = 0; m21 = 0; m22 = 1;
        }
        public Matrix3(float m00, float m01, float m02,
                       float m10, float m11, float m12,
                       float m20, float m21, float m22)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02;
            this.m10 = m10; this.m11 = m11; this.m12 = m12;
            this.m20 = m20; this.m21 = m21; this.m22 = m22;
        }
        public Matrix3(Vector2 position, float rotation, float scale)
        {
            m00 = scale; m01 = -rotation; m02 = position.X;
            m10 = rotation; m11 = scale; m12 = position.Y;
            m20 = 0; m21 = 0; m22 = 1;
        }
        public static Matrix3 operator +(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    left.m00 + right.m00, left.m01 + right.m01, left.m02 + right.m02,
                    left.m10 + right.m10, left.m11 + right.m11, left.m12 + right.m12,
                    left.m10 + right.m20, left.m21 + right.m21, left.m22 + right.m22
                );
        }
        public static Matrix3 operator -(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    left.m00 - right.m00, left.m01 - right.m01, left.m02 - right.m02,
                    left.m10 - right.m10, left.m11 - right.m11, left.m12 - right.m12,
                    left.m10 - right.m20, left.m21 - right.m21, left.m22 - right.m22
                );
        }
        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            return new Matrix3
                (
                    (left.m00 * left.m00) + (left.m01 * right.m10) + (left.m02 * right.m20),
                    (left.m00 * left.m01) + (left.m01 * right.m11) + (left.m02 * right.m21),
                    (left.m00 * left.m02) + (left.m01 * right.m12) + (left.m02 * right.m22),

                    (left.m10 * left.m00) + (left.m11 * right.m10) + (left.m12 * right.m20),
                    (left.m10 * left.m01) + (left.m11 * right.m11) + (left.m12 * right.m21),
                    (left.m10 * left.m02) + (left.m11 * right.m12) + (left.m12 * right.m22),

                    (left.m20 * left.m00) + (left.m21 * right.m10) + (left.m22 * right.m20),
                    (left.m20 * left.m01) + (left.m21 * right.m11) + (left.m22 * right.m21),
                    (left.m20 * left.m02) + (left.m21 * right.m12) + (left.m22 * right.m22)
                );
        }
        public Matrix3 Rotate(float degrees)
        {
            return new Matrix3((float)Math.Sin(degrees), (float)-Math.Cos(degrees), 0,
                               (float)Math.Cos(degrees), (float)Math.Sin(degrees), 0,
                               0, 0, 1);
        }
    } 
}
