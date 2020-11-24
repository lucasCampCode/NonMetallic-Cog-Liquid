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
        private float _turretRotationZ = 0;
        private float _turretRotationY = 0;
        private Actor _tankBody;
        private Actor[] _leftTankTreads;
        private Actor[] _rightTankTreads;
        private Actor _turretZ;
        private Actor _turretY;
        private Actor _barrel;
        private Actor _supressor;

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
            Bullet bullet = new Bullet(_supressor.WorldPosition.X, _supressor.WorldPosition.Y, _supressor.WorldPosition.Z, Color.WHITE,Shape.SHPERE, 0.20f);
            Game.GetCurrentScene().AddActor(bullet);
            bullet.Velocity = _turretZ.Forward * (_bulletSpeed + (Speed/2));
        }
        public override void OnCollision(Actor other)
        {
            base.OnCollision(other);
        }


        private void InitBody()
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
            _turretZ = new Actor(0, 2, 0, Raylib.Fade(Color.BLUE, 0),Shape.NULL, 0);
            _turretY = new Actor(0, 0, 0, Color.GREEN, Shape.CYLINDER, 1);
            _barrel = new Actor(.5f,0,0,Color.GREEN,Shape.CYLINDER,1);
            _supressor = new Actor(5,0,0,Color.GREEN,Shape.CUBE,0.5f);

            _barrel.SetRotationZ((float)Math.PI / 2);
            _turretY.SetScale((0.5f,0.25f,0.5f));
            _barrel.SetScale((.5f,1,.5f));
            _supressor.SetScale((2, 2, 2));
            


            this.AddChild(_tankBody);
            for (int i = 0; i < _leftTankTreads.Length; i++)
            {
                _leftTankTreads[i].SetRotationX((float)Math.PI / 2);
                _tankBody.AddChild(_leftTankTreads[i]);
            }
            for (int i = 0; i < _rightTankTreads.Length; i++)
            {
                _rightTankTreads[i].SetRotationX(-(float)Math.PI / 2);
                _tankBody.AddChild(_rightTankTreads[i]);
            }
            _tankBody.AddChild(_turretY);
            _turretY.AddChild(_turretZ);
            _turretZ.AddChild(_barrel);
            _turretZ.AddChild(_supressor);

        }

        private void UpdateBody(float deltaTime)
        {
            _tankBody.Update(deltaTime);

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].Update(deltaTime);
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].Update(deltaTime);
            _turretZ.Update(deltaTime);
            _turretY.Update(deltaTime);
            _barrel.Update(deltaTime);
            _supressor.Update(deltaTime);
        }
        private void DrawBody()
        {
            _tankBody.Draw();

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].Draw();
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].Draw();
            _turretY.Draw();
            _barrel.Draw();
            _supressor.Draw();
        }

        public override void Start()
        {
            InitBody();
            base.Start();
        }
        public override void Update(float deltaTime)
        {
            //Gets the player's input to determine which direction the actor will move in on each axis 
            int rotatePlayer = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
            int rotateTurretY = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_LEFT))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_RIGHT));
            int rotateTurretZ = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_UP))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_DOWN));
            int xDirection = Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_W))
                + -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_S));

            if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_CONTROL))
                Speed = 20;
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_SHIFT))
                Speed = 5;
            else { Speed = 10; }

            if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE))
                _bulletSpeed += 1;
            else if (Raylib.IsKeyReleased(KeyboardKey.KEY_SPACE))
            {
                Shoot();
                _bulletSpeed = 10;
            }
            _bulletSpeed = Math.Clamp(_bulletSpeed, 10, 100);
            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = Forward * xDirection;
            Velocity = Velocity.Normalized * Speed;
            //Acceleration = Forward * xDirection;

            if (rotateTurretY > 0)
                _turretRotationY += 0.1f;
            else if (rotateTurretY < 0)
                _turretRotationY -= 0.1f;

            if (rotateTurretZ > 0)
                _turretRotationZ += 0.1f;
            else if (rotateTurretZ < 0)
                _turretRotationZ -= 0.1f;
            
            
            if (rotatePlayer > 0)
                RotateY(0.1f);
            else if (rotatePlayer < 0)
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

            if (!OnGround())
                Velocity += _gravity;

            _turretRotationZ = Math.Clamp(_turretRotationZ, -(float)Math.PI / 2,0);
            _turretY.SetRotationY(_turretRotationY);
            _turretZ.SetRotationZ(_turretRotationZ);

            base.Update(deltaTime);
            UpdateBody(deltaTime);
        }
        public override void Draw()
        {
            DrawBody();
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
