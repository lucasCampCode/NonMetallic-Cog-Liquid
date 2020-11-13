using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames3D
{
    class Player : Actor
    {
        private float _speed = 1;
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
        public Player(float x, float y,float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y,z, collisionRadius, icon, color)
        {
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y,float z, Color rayColor, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y,z, rayColor, collisionRadius, icon, color)
        {
        }

        public void Shoot()
        {

        }
        public override void OnCollision(Actor other)
        {
            base.OnCollision(other);
        }

        public override void Start()
        {
            base.Start();
        }
        public override void Update(float deltaTime)
        {
            //Gets the player's input to determine which direction the actor will move in on each axis 
            int xDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
            int yDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_W))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_S));

            int speed = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_CONTROL)) +
                Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_SHIFT));
            if (speed > 0 && Speed < 20)
            {
                Speed += 2;
            }
            else if (speed < 0 && Speed > 0)
            {
                Speed -= 2;
            }

            if (Game.GetKeyPressed((int)KeyboardKey.KEY_SPACE))
                Shoot();

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = new Vector3(xDirection,0, yDirection);
            Velocity = Velocity.Normalized * Speed;

            base.Update(deltaTime);

            UpdateFacing();
        }
        public override void Draw()
        {
            base.Draw();
        }
    }
}
