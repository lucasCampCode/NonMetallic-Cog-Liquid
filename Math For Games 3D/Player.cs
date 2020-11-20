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
        private float _bulletSpeed = 20;
        private Actor _tankBody;
        private Actor[] _leftTankTreads;
        private Actor[] _rightTankTreads;
        private Actor _turret;
        private Actor _barrel;

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
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y,float z, Color rayColor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y,z, rayColor,shape, collisionRadius, icon, color)
        {
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }
        public Player(Vector3 position, Color rayColor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(position.X, position.Y, position.Z, rayColor,shape, collisionRadius, icon, color)
        {
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet(_barrel.WorldPosition.X, _barrel.WorldPosition.Y, _barrel.WorldPosition.Z, Color.WHITE,Shape.SHPERE, 0.20f);
            Game.GetCurrentScene().AddActor(bullet);
            bullet.Velocity = Forward * (_bulletSpeed + (Speed/2));
        }
        public override void OnCollision(Actor other)
        {
            base.OnCollision(other);
        }

        public override void Start()
        {
            //initilizes body that the player moves
            _tankBody = new Actor(0, 1, 0, Color.LIME, Shape.TANKBODY, 1);

            //initilizes the left side of the tank treads
            float additive = 0;
            for(int i = 0; i < _leftTankTreads.Length; i++)
            {
                if (i == 0)
                    _leftTankTreads[i] = new Actor(-2 + additive, 0, -1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                else if (i == _leftTankTreads.Length - 1)
                    _leftTankTreads[i] = new Actor(-2 + additive, 0, -1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                else
                    _leftTankTreads[i] = new Actor(-2 + additive, -0.5f, -1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                additive += 1;
            }

            //initilize right side of the tank treads
            additive = 0;
            for (int i = 0; i < _rightTankTreads.Length; i++)
            {
                if (i == 0)
                    _rightTankTreads[i] = new Actor(-2 + additive, 0, 1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                else if (i == _rightTankTreads.Length - 1)
                    _rightTankTreads[i] = new Actor(-2 + additive, 0, 1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                else
                    _rightTankTreads[i] = new Actor(-2 + additive, -0.5f, 1.25f, Color.DARKGREEN, Shape.TIRES, 1);
                additive += 1;
            }

            _turret = new Actor(0, 0.25f, 0, Color.GREEN, Shape.TIRES, 1);
            _barrel = new Actor(0,0.5f,0,Color.GREEN,Shape.CYLINDER,1);

            _barrel.SetRotationZ((float)Math.PI / 2);
            _turret.SetScale((2,1,2));
            _barrel.SetScale((0.25f, 0.25f, 0.125f));

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].SetRotationX((float)Math.PI / 2);
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].SetRotationX(-(float)Math.PI / 2);

            this.AddChild(_tankBody);
            for (int i = 0; i < _leftTankTreads.Length; i++)
                _tankBody.AddChild(_leftTankTreads[i]);
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _tankBody.AddChild(_rightTankTreads[i]);
            _tankBody.AddChild(_turret);
            _turret.AddChild(_barrel);
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

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                Shoot();

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = Forward * xDirection;
            Velocity = Velocity.Normalized * Speed;
            //Acceleration = Forward * xDirection;

            if (rotate > 0)
                RotateY(0.1f);
            else if (rotate < 0)
                RotateY(-0.1f);
            if (xDirection > 0)
            {
                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(-0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(0.2f);
            }
            if (xDirection < 0)
            {
                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(-0.2f);
            }


            base.Update(deltaTime);
            _tankBody.Update(deltaTime);

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].Update(deltaTime);
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].Update(deltaTime);
            _turret.Update(deltaTime);
            _barrel.Update(deltaTime);
            UpdateFacing();
        }
        public override void Draw()
        {
            _tankBody.Draw();

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].Draw();
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].Draw();
            _turret.Draw();
            _barrel.Draw();
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
