﻿using System;
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
        private Model _cube = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(1, 1, 1));
        private Model _cylinder = Raylib.LoadModelFromMesh(Raylib.GenMeshCylinder(2, 4, 10));
        private Model _tankBody = Raylib.LoadModelFromMesh(Raylib.GenMeshCube(4, 1, 3));
        private Model _tires = Raylib.LoadModelFromMesh(Raylib.GenMeshCylinder(0.5f, 1, 6));
        private Model _null = Raylib.LoadModelFromMesh(Raylib.GenMeshPlane(.5f, .5f, 0, 0));
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
        public Actor(float x, float y,float z, Color rayColor, Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x, y, z, collisionRadius, icon, color)
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
        public void LookAt(Vector3 position)
        {
            //Find the direction that the actor should look in 
            Vector3 direction = (position - LocalPosition).Normalized;

            //Use the dotproduct to find the angle the actor needs to rotate 
            float dotProd = Vector3.DotProduct(Forward, direction);
            if (Math.Abs(dotProd) > 1)
                return;
            float angle = (float)Math.Acos(dotProd);

            //Find a perpindicular vector to the direction 
            Vector3 perp = new Vector3(direction.Y, -direction.X, direction.Z);

            //Find the dot product of the perpindicular vector and the current forward 
            float perpDot = Vector3.DotProduct(perp, Forward);

            //If the result isn't 0, use it to change the sign of the angle to be either positive or negative 
            if (perpDot != 0)
                angle *= -perpDot / Math.Abs(perpDot);

            RotateZ(angle);
        }

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
            UpdateShape();
            UpdateTransform();
            UpdateGlobalTransform();
            LocalPosition += _velocity * deltaTime;
        }
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
        public void DrawShape()
        {
            switch (_shape)
            {
                case Shape.SHPERE:
                    Raylib.DrawSphere(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), _collisionRadius, _rayColor);
                    break;
                case Shape.CUBE:
                    Raylib.DrawModel(_cube, new System.Numerics.Vector3(0,0,0), 1.0f, _rayColor);
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
            //draws sprite and direction they are pointing
            DrawShape();
        }
        public virtual void End()
        {
            Started = false;
        }
    }
}
