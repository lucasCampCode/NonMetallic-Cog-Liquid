using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames3D
{
    enum Shape
    {
        SHPERE,
        CUBE,
        CYLINDER,
        TANKBODY,
        TIRES,
        NULL
    }
    class Actor
    {
        //loads models from generated mesh
        private Model _cube = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(1, 1, 1));
        private Model _cylinder = Raylib.LoadModelFromMesh(Raylib.GenMeshCylinder(2, 4, 10));
        private Model _tankBody = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(3, 1, 4));
        private Model _tires = Raylib.LoadModelFromMesh(Raylib.GenMeshCylinder(0.5f, 1, 6));
        private Model _null = Raylib.LoadModelFromMesh(Raylib.GenMeshPlane(.5f, .5f, 1, 1));

        protected char _icon = ' ';
        protected float _collisionRadius;
        private float _maxSpeed = 40;
        protected Vector3 _gravity = new Vector3(0, -0.2f, 0);
        private Vector3 _acceleration = new Vector3();
        private Vector3 _velocity = new Vector3();
        protected Matrix4 _localTransform = new Matrix4();
        protected Matrix4 _globalTransform = new Matrix4();
        private Matrix4 _translation = new Matrix4();
        private Matrix4 _rotation = new Matrix4();
        private Matrix4 _scale = new Matrix4();
        protected ConsoleColor _color;
        protected Color _rayColor;
        private Shape _shape;
        private Actor[] _children = new Actor[0];

        public bool Started { get; private set; }
        public Actor Parent { get; private set; }
        public Vector3 Forward
        {
            get { return new Vector3(_globalTransform.m13, _globalTransform.m23, _globalTransform.m33); }
        }

        public Vector3 WorldPosition
        {
            get { return new Vector3(_globalTransform.m14, _globalTransform.m24, _globalTransform.m34); }
        }

        public Vector3 LocalPosition
        {
            get
            {
                return new Vector3(_localTransform.m14, _localTransform.m24, _localTransform.m34);
            }
            set
            {
                _translation.m14 = value.X;
                _translation.m24 = value.Y;
                _translation.m34 = value.Z;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return _acceleration;
            }
            set
            {
                _acceleration = value;
            }
        }

        public Actor[] Children { get => _children; set => _children = value; }
        public Shape Shape { get => _shape; }
        public Color RayColor { get => _rayColor; }


        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Actor(float x, float y, float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            LocalPosition = new Vector3(x, y, z);
            _velocity = new Vector3();
            _color = color;
            _collisionRadius = collisionRadius;
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, float z, Color rayColor, Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x, y, z, collisionRadius, icon, color)
        {
            _rayColor = rayColor;
            _shape = shape;
        }
        public Actor(float x, float y, float z, Color rayColor, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x, y, z, collisionRadius, icon, color)
        {
            _rayColor = rayColor;
        }

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[Children.Length + 1];
            //Copy the values from the old array to the new array
            for (int i = 0; i < Children.Length; i++)
            {
                tempArray[i] = Children[i];
            }
            //Set the last value in the new array to be the actor we want to add
            tempArray[Children.Length] = child;

            //Set old array to hold the values of the new array
            Children = tempArray;

            child.Parent = this;
        }
        public bool RemoveChild(Actor child)
        {
            if (child == null)
                return false;

            bool childRemoved = false;

            Actor[] tempArray = new Actor[Children.Length - 1];
            int j = 0;
            //Copy the values from the old array to the new array
            for (int i = 0; i < Children.Length; i++)
            {
                if (child != Children[i])
                {
                    tempArray[j] = Children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }

            //Set old array to hold the values of the new array
            Children = tempArray;
            child.Parent = null;
            return childRemoved;
        }
        public bool RemoveChild(int index)
        {
            if (index < 0 || index > Children.Length)
                return false;

            bool childRemoved = false;

            Actor[] tempArray = new Actor[Children.Length - 1];
            int j = 0;
            //Copy the values from the old array to the new array
            for (int i = 0; i < Children.Length; i++)
            {
                if (index != i)
                {
                    tempArray[j] = Children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }
            //Set old array to hold the values of the new array
            Children = tempArray;

            return childRemoved;
        }
        /// <summary>
        /// set the posistion of the actor
        /// </summary>
        /// <param name="position"></param>
        public void SetTranslate(Vector3 position)
        {
            _translation = Matrix4.CreateTraslation(position);
        }
        /// <summary>
        /// set the x-axis of rotation of an actor
        /// </summary>
        /// <param name="radians"></param>
        public void SetRotationX(float radians)
        {
            _rotation = Matrix4.CreateRotationX(radians);
        }
        /// <summary>
        /// set the y-axis of rotation of an actor
        /// </summary>
        /// <param name="radians"></param>
        public void SetRotationY(float radians)
        {
            _rotation = Matrix4.CreateRotationY(radians);
        }
        /// <summary>
        /// set the xZ-axis of rotation of an actor
        /// </summary>
        /// <param name="radians"></param>
        public void SetRotationZ(float radians)
        {
            _rotation = Matrix4.CreateRotationZ(radians);
        }
        /// <summary>
        /// rotates actor using all axis
        /// </summary>
        /// <param name="radiansX"></param>
        /// <param name="radiansY"></param>
        /// <param name="radiansZ"></param>
        public void RotateXYZ(float radiansX, float radiansY, float radiansZ)
        {
            _rotation *= (Matrix4.CreateRotationX(radiansX) * Matrix4.CreateRotationY(radiansY) * Matrix4.CreateRotationZ(radiansZ));
        }
        /// <summary>
        /// rotates the actor of the X-axis
        /// </summary>
        /// <param name="radians"></param>
        public void RotateX(float radians)
        {
            _rotation *= Matrix4.CreateRotationX(radians);
        }
        /// <summary>
        /// rotates the Actor of the Y-axis
        /// </summary>
        /// <param name="radians"></param>
        public void RotateY(float radians)
        {
            _rotation *= Matrix4.CreateRotationY(radians);
        }
        /// <summary>
        /// rotates the Actor of the Z-axis
        /// </summary>
        /// <param name="radians"></param>
        public void RotateZ(float radians)
        {
            _rotation *= Matrix4.CreateRotationZ(radians);
        }
        /// <summary>
        /// set the scale of the Actor
        /// </summary>
        /// <param name="scale"></param>
        public void SetScale(Vector3 scale)
        {
            _scale = Matrix4.CreateScale(scale);
        }
        /// <summary>
        /// checks to see if the distance of two actors are greater than each collision radius
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool CheckCollision(Actor other)
        {

            float distance = (other.WorldPosition - WorldPosition).Magnitude;
            return distance <= _collisionRadius + other._collisionRadius;
        }
        /// <summary>
        /// Actor base collision 
        /// </summary>
        /// <param name="other"></param>
        public virtual void OnCollision(Actor other)
        {

        }
        /// <summary>
        /// updates the local transform with the concatinations of translation, rotation, and scale
        /// </summary>
        private void UpdateTransform()
        {
            _localTransform = _translation * _rotation * _scale;
        }
        /// <summary>
        /// updates the gloabal transform with a concatination of parent or world with the local transform
        /// </summary>
        public void UpdateGlobalTransform()
        {
            if (Parent != null)
                _globalTransform = Parent._globalTransform * _localTransform;
            else
                _globalTransform = Game.GetCurrentScene().World * _localTransform;

            for (int i = 0; i < Children.Length; i++)
            {
                Children[i].UpdateGlobalTransform();
            }
        }
        /// <summary>
        /// checks to see if an actor hit ground level
        /// </summary>
        /// <returns></returns>
        public bool OnGround()
        {
            if (WorldPosition.Y <= Game.GetCurrentScene().World.m24)
                return true;

            return false;
        }
        /// <summary>
        /// destroys the current actor
        /// </summary>
        public void Destroy()
        {
            Game.GetCurrentScene().RemoveActor(this);
            if (Parent != null)
                Parent.RemoveChild(this);
            End();
        }

        public virtual void Start()
        {
            Started = true;
        }
        public virtual void Update(float deltaTime)
        {
            UpdateShape();
            UpdateTransform();
            UpdateGlobalTransform();
            //Increase position by the current velocity
            Velocity += Acceleration;

            //caps the speed of all actors
            if (Velocity.Magnitude > _maxSpeed)
                Velocity = Velocity.Normalized * _maxSpeed;

            LocalPosition += _velocity * deltaTime;
        }
        /// <summary>
        /// updates the models of the shape chosen by the actor
        /// </summary>
        public void UpdateShape()
        {
            switch (_shape)
            {
                case Shape.SHPERE:
                    break;
                case Shape.CUBE:
                    _cube.transform = new System.Numerics.Matrix4x4(_globalTransform.m11, _globalTransform.m12, _globalTransform.m13, _globalTransform.m14,
                                                                    _globalTransform.m21, _globalTransform.m22, _globalTransform.m23, _globalTransform.m24,
                                                                    _globalTransform.m31, _globalTransform.m32, _globalTransform.m33, _globalTransform.m34,
                                                                    _globalTransform.m41, _globalTransform.m42, _globalTransform.m43, _globalTransform.m44);
                    break;
                case Shape.CYLINDER:
                    _cylinder.transform = new System.Numerics.Matrix4x4(_globalTransform.m11, _globalTransform.m12, _globalTransform.m13, _globalTransform.m14,
                                                                    _globalTransform.m21, _globalTransform.m22, _globalTransform.m23, _globalTransform.m24,
                                                                    _globalTransform.m31, _globalTransform.m32, _globalTransform.m33, _globalTransform.m34,
                                                                    _globalTransform.m41, _globalTransform.m42, _globalTransform.m43, _globalTransform.m44);
                    break;
                case Shape.TANKBODY:
                    _tankBody.transform = new System.Numerics.Matrix4x4(_globalTransform.m11, _globalTransform.m12, _globalTransform.m13, _globalTransform.m14,
                                                                    _globalTransform.m21, _globalTransform.m22, _globalTransform.m23, _globalTransform.m24,
                                                                    _globalTransform.m31, _globalTransform.m32, _globalTransform.m33, _globalTransform.m34,
                                                                    _globalTransform.m41, _globalTransform.m42, _globalTransform.m43, _globalTransform.m44);
                    break;
                case Shape.TIRES:
                    _tires.transform = new System.Numerics.Matrix4x4(_globalTransform.m11, _globalTransform.m12, _globalTransform.m13, _globalTransform.m14,
                                                                    _globalTransform.m21, _globalTransform.m22, _globalTransform.m23, _globalTransform.m24,
                                                                    _globalTransform.m31, _globalTransform.m32, _globalTransform.m33, _globalTransform.m34,
                                                                    _globalTransform.m41, _globalTransform.m42, _globalTransform.m43, _globalTransform.m44);
                    break;
                case Shape.NULL:
                    _null.transform = new System.Numerics.Matrix4x4(_globalTransform.m11, _globalTransform.m12, _globalTransform.m13, _globalTransform.m14,
                                                                    _globalTransform.m21, _globalTransform.m22, _globalTransform.m23, _globalTransform.m24,
                                                                    _globalTransform.m31, _globalTransform.m32, _globalTransform.m33, _globalTransform.m34,
                                                                    _globalTransform.m41, _globalTransform.m42, _globalTransform.m43, _globalTransform.m44);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// draws a new shape for what the actor needs
        /// </summary>
        public void DrawShape()
        {
            switch (_shape)
            {
                case Shape.SHPERE:
                    Raylib.DrawSphere(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), _collisionRadius, Raylib.Fade(_rayColor, 50));
                    break;
                case Shape.CUBE:
                    Raylib.DrawModel(_cube, new System.Numerics.Vector3(0, 0, 0), 1.0f, _rayColor);
                    break;
                case Shape.CYLINDER:
                    Raylib.DrawModel(_cylinder, new System.Numerics.Vector3(0, 0, 0), 1.0f, _rayColor);
                    break;
                case Shape.TANKBODY:
                    Raylib.DrawModel(_tankBody, new System.Numerics.Vector3(0, 0, 0), 1.0f, _rayColor);
                    break;
                case Shape.TIRES:
                    Raylib.DrawModel(_tires, new System.Numerics.Vector3(0, 0, 0), 1.0f, _rayColor);
                    break;
                case Shape.NULL:
                    Raylib.DrawModel(_null, new System.Numerics.Vector3(0, 0, 0), 1.0f, _rayColor);
                    break;
                default:
                    break;
            }
        }
        public virtual void Draw()
        {
            DrawShape();
            Debug();
        }
        public virtual void End()
        {
            Started = false;
        }
        public virtual void Debug()
        {
            if (Game.Debug)
            {
                Raylib.DrawSphere(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), _collisionRadius, Raylib.Fade(Color.BLUE, 0.5f));
                Raylib.DrawLine3D(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z),
                                  new System.Numerics.Vector3(WorldPosition.X + (Forward.X * 5), WorldPosition.Y + (Forward.Y * 5), WorldPosition.Z + (Forward.Z * 5)),
                                  Color.PURPLE);
            }
        }
    }
}
