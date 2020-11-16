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
        CUBE
    }
    class Actor
    {
        protected char _icon = ' ';
        protected float _collisionRadius;
        protected Vector3 _velocity;
        protected Matrix4 _localTransform = new Matrix4();
        protected Matrix4 _globalTransform = new Matrix4();
        private Matrix4 _translation = new Matrix4();
        private Matrix4 _rotation = new Matrix4();
        private Matrix4 _scale = new Matrix4();
        protected ConsoleColor _color;
        protected Color _rayColor;
        private Shape _shape;
        protected Actor[] _children = new Actor[0];

        public bool Started { get; private set; }
        public Actor Parent { get; private set; }
        public Vector3 Forward
        {
            get { return new Vector3(_globalTransform.m11, _globalTransform.m21,_globalTransform.m31); }
            //set
            //{
            //    Vector3 lookPosition = LocalPosition + value.Normalized;
            //    LookAt(lookPosition);
            //}
        }

        public Vector3 WorldPosition
        {
            get { return new Vector3(_globalTransform.m14, _globalTransform.m24,_globalTransform.m34); }
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


        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Actor(float x, float y,float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            LocalPosition = new Vector3(x, y,z);
            _velocity = new Vector3();
            _color = color;
            _collisionRadius = collisionRadius;
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y,float z, Color rayColor,Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x, y,z, collisionRadius, icon, color)
        {
            _rayColor = rayColor;
            _shape = shape;
        }

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length + 1];
            //Copy the values from the old array to the new array
            for (int i = 0; i < _children.Length; i++)
            {
                tempArray[i] = _children[i];
            }
            //Set the last value in the new array to be the actor we want to add
            tempArray[_children.Length] = child;

            //Set old array to hold the values of the new array
            _children = tempArray;

            child.Parent = this;
        }
        public bool RemoveChild(Actor child)
        {
            if (child == null)
                return false;

            bool childRemoved = false;

            Actor[] tempArray = new Actor[_children.Length - 1];
            int j = 0;
            //Copy the values from the old array to the new array
            for (int i = 0; i < _children.Length; i++)
            {
                if (child != _children[i])
                {
                    tempArray[j] = _children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }

            //Set old array to hold the values of the new array
            _children = tempArray;
            child.Parent = null;
            return childRemoved;
        }
        public bool RemoveChild(int index)
        {
            if (index < 0 || index > _children.Length)
                return false;

            bool childRemoved = false;

            Actor[] tempArray = new Actor[_children.Length - 1];
            int j = 0;
            //Copy the values from the old array to the new array
            for (int i = 0; i < _children.Length; i++)
            {
                if (index != i)
                {
                    tempArray[j] = _children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }
            //Set old array to hold the values of the new array
            _children = tempArray;

            return childRemoved;
        }

        public void SetTranslate(Vector3 position)
        {
            _translation = Matrix4.CreateTraslation(position);
        }
        public void SetRotationX(float radians)
        {
            _rotation = Matrix4.CreateRotationX(radians);
        }
        public void SetRotationY(float radians)
        {
            _rotation = Matrix4.CreateRotationY(radians);
        }
        public void SetRotationZ(float radians)
        {
            _rotation = Matrix4.CreateRotationZ(radians);
        }
        public void Rotate(float radiansX,float radiansY,float radiansZ)
        {
            _rotation = _rotation * (Matrix4.CreateRotationX(radiansX) * Matrix4.CreateRotationY(radiansY) * Matrix4.CreateRotationZ(radiansZ)); 
        }
        public void RotateX(float radians)
        {
            _rotation *= Matrix4.CreateRotationX(radians);
        }
        public void RotateY(float radians)
        {
            _rotation *= Matrix4.CreateRotationY(radians);
        }
        public void RotateZ(float radians)
        {
            _rotation *= Matrix4.CreateRotationZ(radians);
        }
        
        public void SetScale(Vector3 scale)
        {
            _scale = Matrix4.CreateScale(scale);
        }

        /// <summary> 
        /// Rotates the actor to face the given position 
        /// </summary> 
        /// <param name="position">The position the actor should be facing</param> 
        //public void LookAt(Vector3 position)
        //{
        //    //Find the direction that the actor should look in 
        //    Vector3 direction = (position - LocalPosition).Normalized;

        //    //Use the dotproduct to find the angle the actor needs to rotate 
        //    float dotProd = Vector3.DotProduct(Forward, direction);
        //    if (Math.Abs(dotProd) > 1)
        //        return;
        //    float angle = (float)Math.Acos(dotProd);

        //    //Find a perpindicular vector to the direction 
        //    Vector3 perp = new Vector3(direction.Y, -direction.X, direction.Z);

        //    //Find the dot product of the perpindicular vector and the current forward 
        //    float perpDot = Vector3.DotProduct(perp, Forward);

        //    //If the result isn't 0, use it to change the sign of the angle to be either positive or negative 
        //    if (perpDot != 0)
        //        angle *= -perpDot / Math.Abs(perpDot);

        //    RotateZ(angle);
        //}

        public bool CheckCollision(Actor other)
        {
            float distance = (other.WorldPosition - WorldPosition).Magnitude;
            return distance <= _collisionRadius + other._collisionRadius;
        }
        public virtual void OnCollision(Actor other)
        {

        }

        private void UpdateTransform()
        {
            _localTransform = _translation * _rotation * _scale;
        }
        public void UpdateFacing()
        {
            if (_velocity.Magnitude <= 0)
                return;
            //Forward = Velocity.Normalized;
        }
        public void UpdateGlobalTransform()
        {
            if (Parent != null)
                _globalTransform = Parent._globalTransform * _localTransform;
            else
                _globalTransform = Game.GetCurrentScene().World * _localTransform;

            for (int i = 0; i < _children.Length; i++)
            {
                _children[i].UpdateGlobalTransform();
            }
        }

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
            //Increase position by the current velocity
            UpdateTransform();
            UpdateGlobalTransform();
            LocalPosition += _velocity * deltaTime;
        }
        public void DrawShape()
        {
            switch (_shape)
            {
                case Shape.SHPERE:
                    Raylib.DrawSphere(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), _collisionRadius, _rayColor);
                    break;
                case Shape.CUBE:

                    Raylib.DrawCube(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), 1,1,1, _rayColor);
                    //Raylib.DrawModelEx(
                    //            Raylib.LoadModelFromMesh(_tankBody),
                    //            new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y + 1.2f, WorldPosition.Z),
                    //            new System.Numerics.Vector3(0, 1, 0),
                    //            _rotateBody * (float)(180 / Math.PI),
                    //            new System.Numerics.Vector3(1, 1, 1),
                    //            Color.LIME
                    //          );
                    break;
            }
        }
        public virtual void Draw()
        {
            //draws sprite and direction they are pointing
            DrawShape();
            Raylib.DrawLine3D
                (
                new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z),
                new System.Numerics.Vector3(WorldPosition.X + (Forward.X * 2.5f), WorldPosition.Y + (Forward.Y * 2.5f), WorldPosition.Z + (Forward.Z * 2.5f)),
                Color.GREEN
                );
        }
        public virtual void End()
        {
            Started = false;
        }
    }
}
