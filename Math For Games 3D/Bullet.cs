﻿using System;
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
            Bullet bullet = other as Bullet;
            if ((other is Enemy || other is Goal) && bullet == null)
                other.Destroy();

            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            if (WorldPosition.X < -100 || WorldPosition.X > 100
                || WorldPosition.Y < 0 || WorldPosition.Y > 100)
                Destroy();

            if (!OnGround())
                Velocity += _gravity;

            base.Update(deltaTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

