using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    /// <summary>
    /// An actor that moves based on input given by the user
    /// </summary>
    class Player : Actor
    {
        private float _speed = 1;
        private Sprite _sprite;
        private float _bulletSpeed = 10;

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Player(float x, float y, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, collisionRadius, icon, color)
        {
            _sprite = new Sprite("Images/player.png");
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y, Color rayColor, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, collisionRadius, icon, color)
        {
            _sprite = new Sprite("Images/player.png");
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet(WorldPosition.X, WorldPosition.Y, 1);
            Game.GetCurrentScene().AddActor(bullet);
            bullet.Velocity = Forward * _bulletSpeed;
        }
        public override void OnCollision(Actor other)
        {
            Bullet bullet = other as Bullet;
            if (bullet == null)
                Destroy();
            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            //Gets the player's input to determine which direction the actor will move in on each axis 
            int xDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
            int yDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_W))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_S));

            if (Game.GetKeyPressed((int)KeyboardKey.KEY_SPACE))
                Shoot();

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = new Vector2(xDirection, yDirection);
            Velocity = Velocity.Normalized * Speed;

            base.Update(deltaTime);

            UpdateFacing();
        }
        public override void Draw()
        {
            _sprite.Draw(_localTransform);
            base.Draw();
        }
    }
}