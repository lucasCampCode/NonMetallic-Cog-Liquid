using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class Collectible : Actor
    {

        public Collectible(float x, float y, float z, Color rayColor, Shape shape, float collisionRadius) : base(x, y, z, rayColor, shape, collisionRadius)
        {
            RotateXYZ((float)Math.PI / 4, 0, (float)Math.PI / 4);
        }
        public override void Update(float deltaTime)
        {
            RotateXYZ(0,0.1f,0);
            base.Update(deltaTime);
        }
    }
}
