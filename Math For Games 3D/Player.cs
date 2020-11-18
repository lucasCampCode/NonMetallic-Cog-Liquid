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
        private Actor _tankBody;
        private Actor _tankTire1;
        private Actor _tankTire2;
        private Actor _tankTire3;
        private Actor _tankTire4;
        private Actor _turrent;

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
        public Player(float x, float y,float z, Color rayColor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y,z, rayColor,shape, collisionRadius, icon, color)
        {
        }
        public Player(Vector3 position, Color rayColor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(position.X, position.Y, position.Z, rayColor,shape, collisionRadius, icon, color)
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
            _tankBody = new Actor(0, 1, 0, Color.LIME, Shape.TANKBODY, 1);
            _tankTire1 = new Actor(1.5f, -0.5f, 1.5f, Color.BROWN, Shape.TIRES, 1);
            _tankTire2 = new Actor(1.5f,-0.5f,-1.5f,Color.BROWN, Shape.TIRES,1);
            _tankTire3 = new Actor(-1.5f, -0.5f, 1.5f, Color.BROWN, Shape.TIRES, 1);
            _tankTire4 = new Actor(-1.5f, -0.5f, -1.5f, Color.BROWN, Shape.TIRES, 1);
            _turrent = new Actor(0, 0.25f, 0, Color.GREEN, Shape.TIRES, 1);
            _turrent.SetScale((2,1,2));
            _tankTire1.SetRotationX(-(float)Math.PI / 2);
            _tankTire2.SetRotationX((float)Math.PI / 2);
            _tankTire3.SetRotationX(-(float)Math.PI / 2);
            _tankTire4.SetRotationX((float)Math.PI / 2);

            this.AddChild(_tankBody);
            _tankBody.AddChild(_tankTire1);
            _tankBody.AddChild(_tankTire2);
            _tankBody.AddChild(_tankTire3);
            _tankBody.AddChild(_tankTire4);
            _tankBody.AddChild(_turrent);
            base.Start();
        }
        public override void Update(float deltaTime)
        {
            //Gets the player's input to determine which direction the actor will move in on each axis 
            int rotate = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
            int xDirection = Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_W))
                + -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_S));

            if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_CONTROL))
                Speed = 20;
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_SHIFT))
                Speed = 5;
            else { Speed = 10; }

            if (Game.GetKeyPressed((int)KeyboardKey.KEY_SPACE))
                Shoot();

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = Forward * xDirection;
            Velocity = Velocity.Normalized * Speed;
            if (rotate > 0)
                RotateY(0.1f);
            else if (rotate < 0)
                RotateY(-0.1f);

            base.Update(deltaTime);
            _tankBody.Update(deltaTime);
            _tankTire1.Update(deltaTime);
            _tankTire2.Update(deltaTime);
            _tankTire3.Update(deltaTime);
            _tankTire4.Update(deltaTime);
            _turrent.Update(deltaTime);
            UpdateFacing();
        }
        public override void Draw()
        {
            _tankBody.Draw();
            _tankTire1.Draw();
            _tankTire2.Draw();
            _tankTire3.Draw();
            _tankTire4.Draw();
            _turrent.Draw();
            DrawShape();
            Raylib.DrawLine3D
                (
                new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z),
                new System.Numerics.Vector3(WorldPosition.X + (Forward.X * 5f), WorldPosition.Y + (Forward.Y * 5f), WorldPosition.Z + (Forward.Z * 5f)),
                Color.GREEN
                );
        }
        public override void End()
        {

            base.End();
        }
    }
}
