using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class Collectible: Actor
    {
        public Collectible(float x, float y, float z, Color rayColor, Shape shape, float collisionRadius) : base(x, y, z, rayColor, shape, collisionRadius)
        {
            Rotate((float)Math.PI/4,0, (float)Math.PI / 4);
        }
        public override void OnCollision(Actor other)
        {
            if (other is Bullet)

            base.OnCollision(other);
        }
        public override void Update(float deltaTime)
        {
            RotateY(0.1f);
            base.Update(deltaTime);
        }
    }
}
