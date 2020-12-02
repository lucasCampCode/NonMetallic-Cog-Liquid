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
        private int _cubesCollected;
        private float _bulletSpeed = 10;
        private float _turretRotationX = 0;
        private float _turretRotationY = 0;
        private Actor _tankBody;
        private Actor _tempActor;
        private Actor[] _rotations = new Actor[100];
        private Actor[] _leftTankTreads;
        private Actor[] _rightTankTreads;
        private Actor _turretX;
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
        public Actor[] Rotations { get => _rotations; set => _rotations = value; }
        public int CubesCollected { get => _cubesCollected; set => _cubesCollected = value; }
        public float BulletSpeed { get => _bulletSpeed; }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Player(float x, float y, float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, z, collisionRadius, icon, color)
        {
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y, float z, Color rayColor, Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, z, rayColor, shape, collisionRadius, icon, color)
        {
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }
        public Player(Vector3 position, Color rayColor, Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(position.X, position.Y, position.Z, rayColor, shape, collisionRadius, icon, color)
        {
            _leftTankTreads = new Actor[5];
            _rightTankTreads = new Actor[5];
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet(_supressor.WorldPosition.X, _supressor.WorldPosition.Y, _supressor.WorldPosition.Z, Color.WHITE, Shape.SHPERE, 0.2f, this);
            Game.GetCurrentScene().AddActor(bullet);
            bullet.Velocity = _turretX.Forward * (_bulletSpeed + (Speed / 2));
        }

        public void AddObjectToPlayer(int i, Actor actor)
        {
            _tempActor = new Collectible(WorldPosition.X - actor.WorldPosition.X,
                                         WorldPosition.Y - actor.WorldPosition.Y,
                                         WorldPosition.Z - actor.WorldPosition.Z,
                                         actor.RayColor, actor.Shape, 0);
            _rotations[i].AddChild(_tempActor);
        }

        private void InitBody()
        {
            //initilizes body that the player moves
            _tankBody = new Actor(0, 1, 0, Color.LIME, Shape.TANKBODY, 1);

            //initilizes the left side of the tank treads
            float additive = 0;
            for (int i = 0; i < _leftTankTreads.Length; i++)
            {
                if (i == 0)
                    _leftTankTreads[i] = new Actor(-1.25f, 0, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                else if (i == _leftTankTreads.Length - 1)
                    _leftTankTreads[i] = new Actor(-1.25f, 0, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                else
                    _leftTankTreads[i] = new Actor(-1.25f, -0.5f, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                additive += 1;

                //rotates each tire to look connected to the tankBody
                _leftTankTreads[i].SetRotationZ(-(float)Math.PI / 2);
                _tankBody.AddChild(_leftTankTreads[i]);
            }

            //initilize right side of the tank treads
            additive = 0;
            for (int i = 0; i < _rightTankTreads.Length; i++)
            {
                if (i == 0)
                    _rightTankTreads[i] = new Actor(1.25f, 0, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                else if (i == _rightTankTreads.Length - 1)
                    _rightTankTreads[i] = new Actor(1.25f, 0, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                else
                    _rightTankTreads[i] = new Actor(1.25f, -0.5f, -2 + additive, Color.DARKGREEN, Shape.TIRES, 0);
                additive += 1;

                //rotates each tire to look connected to the tankBody
                _rightTankTreads[i].SetRotationZ((float)Math.PI / 2);
                _tankBody.AddChild(_rightTankTreads[i]);
            }

            //initilize individual cube rotation paths
            for (int i = 0; i < _rotations.Length; i++)
            {
                _rotations[i] = new Actor(0, 0, 0, 0);

                _rotations[i].RotateXYZ(Game.Random.Next(-6, 6), 0, Game.Random.Next(-6, 6));

                AddChild(_rotations[i]);
            }

            //initilize turrent 
            _turretX = new Actor(0, 2, 0, 0);
            _turretY = new Actor(0, 0, 0, Color.GREEN, Shape.CYLINDER, 0);
            _barrel = new Actor(0, 0, .5f, Raylib.Fade(Color.GREEN, 0.75f), Shape.CYLINDER, 0);
            _supressor = new Actor(0, 0, 5, Color.GREEN, Shape.CUBE, 0);

            //changes to make it look right
            _barrel.SetRotationX(-(float)Math.PI / 2);
            _turretY.SetScale((0.5f, 0.25f, 0.5f));
            _barrel.SetScale((.5f, 1, .5f));
            _supressor.SetScale((2, 2, 2));

            //connects the turret to the tank
            AddChild(_tankBody);
            _tankBody.AddChild(_turretY);
            _turretY.AddChild(_turretX);
            _turretX.AddChild(_barrel);
            _turretX.AddChild(_supressor);

        }

        private void UpdateBody(float deltaTime)
        {
            _tankBody.Update(deltaTime);

            for (int i = 0; i < _leftTankTreads.Length; i++)
                _leftTankTreads[i].Update(deltaTime);
            for (int i = 0; i < _rightTankTreads.Length; i++)
                _rightTankTreads[i].Update(deltaTime);
            for (int i = 0; i < _rotations.Length; i++)
            {
                _rotations[i].Update(deltaTime);
                for (int j = 0; j < _rotations[i].Children.Length; j++)
                    _rotations[i].Children[j].Update(deltaTime);

            }
            _turretX.Update(deltaTime);
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
            for (int i = 0; i < _rotations.Length; i++)
            {
                _rotations[i].Draw();
                for (int j = 0; j < _rotations[i].Children.Length; j++)
                    _rotations[i].Children[j].Draw();
            }
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
            int rotatePlayer = 0;
            int rotateTurretY = 0;
            int rotateTurretZ = 0;
            int xDirection = 0;

            if (Game.CurrentSceneIndex == 0)
            {
                //Gets the player's input to determine which direction the actor will move in on each axis 
                rotatePlayer = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                    + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
                rotateTurretY = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_LEFT))
                    + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_RIGHT));
                rotateTurretZ = Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_UP))
                    - Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_DOWN));
                xDirection = Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_W))
                    + -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_S));

                if (Raylib.IsKeyReleased(KeyboardKey.KEY_F1))
                {
                    if (Game.ShowControls)
                        Game.ShowControls = false;
                    else
                        Game.ShowControls = true;
                }
                if (Raylib.IsKeyReleased(KeyboardKey.KEY_F2))
                {
                    if (Game.Debug)
                        Game.Debug = false;
                    else
                        Game.Debug = true;
                }
                if (Raylib.IsKeyReleased(KeyboardKey.KEY_F3))
                {
                    if (Game.PlayerInfo)
                        Game.PlayerInfo = false;
                    else
                        Game.PlayerInfo = true;
                }

                if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_CONTROL))
                    Speed = 20;
                else if (Game.GetKeyDown((int)KeyboardKey.KEY_LEFT_SHIFT))
                    Speed = 5;
                else { Speed = 10; }

                //summons a bullet to shoot after a set speed
                if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE))
                    _bulletSpeed += 1;
                else if (Raylib.IsKeyReleased(KeyboardKey.KEY_SPACE))
                {
                    Shoot();
                    //resets bullet speed so it doesn't get stuck at clamped value
                    _bulletSpeed = 10;
                }
            }
            // player rotates the turrent left and right
            if (rotateTurretY > 0)
                _turretRotationY += 0.05f;
            else if (rotateTurretY < 0)
                _turretRotationY -= 0.05f;

            //rotates the turrent up and down
            if (rotateTurretZ > 0)
                _turretRotationX += 0.1f;
            else if (rotateTurretZ < 0)
                _turretRotationX -= 0.1f;

            //rotates player and connecting parts
            if (rotatePlayer > 0)
            {
                RotateY(0.1f);

                for (int i = 0; i < _rotations.Length; i++)
                    _rotations[i].RotateY(0.1f);

                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(-0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(-0.2f);
            }
            else if (rotatePlayer < 0)
            {
                RotateY(-0.1f);

                for (int i = 0; i < _rotations.Length; i++)
                    _rotations[i].RotateY(-0.05f);

                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(0.2f);
            }

            //animations for how the tank should move forward and backwards
            if (xDirection > 0)
            {
                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(-0.2f);
            }
            if (xDirection < 0)
            {
                for (int i = 0; i < _leftTankTreads.Length; i++)
                    _leftTankTreads[i].RotateY(-0.2f);
                for (int i = 0; i < _rightTankTreads.Length; i++)
                    _rightTankTreads[i].RotateY(0.2f);
            }

            if (!OnGround())
                Velocity += _gravity;

            //provides a seemingly random roations around player
            for (int i = 0; i < _rotations.Length; i++)
            {
                if (i % 5 == 0)
                    _rotations[i].RotateY(0.05f);
                else if (i % 5 == 1)
                    _rotations[i].RotateY(0.025f);
                else if (i % 5 == 2)
                    _rotations[i].RotateY(0.1f);
                else if (i % 5 == 3)
                    _rotations[i].RotateY(0.035f);
                else if (i % 5 == 4)
                    _rotations[i].RotateY(0.075f);
            }
            
            //clamps the movements of the turrent and bullet speed 
            _turretRotationX = Math.Clamp(_turretRotationX, 0, (float)Math.PI / 2);
            _turretY.SetRotationY(_turretRotationY);
            _turretX.SetRotationX(_turretRotationX);
            _bulletSpeed = Math.Clamp(_bulletSpeed, 10, 50);

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            Velocity = Forward * xDirection;
            Velocity = Velocity.Normalized * Speed;
            //Acceleration = Forward * xDirection;

            //keeps player inbounds of play area
            if (WorldPosition.X > 47)
                Velocity.X = -1;
            else if (WorldPosition.X < -47)
                Velocity.X = 1;
            if (WorldPosition.Z > 47)
                Velocity.Z = -1;
            else if (WorldPosition.Z < -47)
                Velocity.Z = 1;

            base.Update(deltaTime);
            UpdateBody(deltaTime);
        }

        public override void Draw()
        {
            DrawBody();
            Raylib.DrawLine3D
                (
                new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z),
                new System.Numerics.Vector3(WorldPosition.X + (Forward.X * 5f), WorldPosition.Y + (Forward.Y * 5f), WorldPosition.Z + (Forward.Z * 5f)),
                Color.GREEN
                );
            Debug();
        }
    }
}
