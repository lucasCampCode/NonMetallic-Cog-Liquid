using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class Bullet : Actor
    {
        public Bullet(float x, float y,float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y,z, collisionRadius, icon, color)
        {

        }
        public Bullet(float x, float y, float z, Color raycolor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, z,raycolor,shape, collisionRadius, icon, color)
        {

        }
        public override void OnCollision(Actor other)
        {
            //if (other is Enemy)
                //other.Destroy();

            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            if (WorldPosition.X < -100 || WorldPosition.X > 100
                || WorldPosition.Y < -100 || WorldPosition.Y > 100)
                Destroy();

            base.Update(deltaTime);
            UpdateFacing();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

