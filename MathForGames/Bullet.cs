using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    class Bullet : Actor
    {
        private Sprite _sprite;

        public Bullet(float x, float y, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, collisionRadius, icon, color)
        {
            _sprite = new Sprite("Images/bullet.png");
        }

        public Bullet(float x, float y, Color rayColor, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, collisionRadius, icon, color)
        {
            _sprite = new Sprite(Raylib.LoadTexture("Images/bullet.png"));
        }

        public override void OnCollision(Actor other)
        {
            Bullet bullet = other as Bullet;
            if ((other is Enemy || other is Planet) && bullet == null)
                other.Destroy();

            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            if (WorldPosition.X < 0 || WorldPosition.X > Raylib.GetScreenWidth()
                || WorldPosition.Y < 0 || WorldPosition.Y > Raylib.GetScreenHeight())
                Destroy();

            base.Update(deltaTime);
            UpdateFacing();
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}
