using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MathLibrary
{
    class matrix3
    {
        private float m00, m01, m02;
        private float m10, m11, m12;
        private float m20, m21, m22;

        public matrix3()
        {
            m00 = 1; m01 = 0; m02 = 0;
            m10 = 0; m11 = 1; m12 = 0;
            m20 = 0; m21 = 0; m22 = 1;
        }
        public matrix3(Vector2 position,float rotation,float scale)
        {
            m00 = scale; m01 = -rotation; m02 = position.X;
            m10 = rotation; m11 = scale; m12 = position.Y;
            m20 = 0; m21 = 0; m22 = 1;
        }

    }
}
