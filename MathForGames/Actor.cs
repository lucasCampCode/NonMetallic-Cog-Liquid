using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{

    /// <summary>
    /// This is the base class for all objects that will 
    /// be moved or interacted with in the game
    /// </summary>
    class Actor
    {
        protected char _icon = ' ';
        private float _collisionRadius;
        protected Vector2 _velocity;
        protected Matrix3 _localTransform = new Matrix3();
        protected Matrix3 _globalTransform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _rotation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Actor[] _children = new Actor[0];

        public bool Started { get; private set; }
        public Actor Parent { get; private set; }
        public Vector2 Forward
        {
            get { return new Vector2(_globalTransform.m11, _globalTransform.m21); }
            set
            {
                Vector2 lookPosition = LocalPosition + value.Normalized;
                LookAt(lookPosition);
            }
        }

        public Vector2 WorldPosition
        {
            get { return new Vector2(_globalTransform.m13, _globalTransform.m23); }
        }

        public Vector2 LocalPosition
        {
            get
            {
                return new Vector2(_localTransform.m13, _localTransform.m23);
            }
            set
            {
                _translation.m13 = value.X;
                _translation.m23 = value.Y;
            }
        }

        public Vector2 Velocity
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
        public Actor(float x, float y, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            LocalPosition = new Vector2(x, y);
            _velocity = new Vector2();
            _color = color;
            _collisionRadius = collisionRadius;
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x, y, collisionRadius, icon, color)
        {
            _rayColor = rayColor;
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

        public void SetTranslate(Vector2 position)
        {
            _translation = Matrix3.CreateTraslation(position);
        }
        public void SetRotation(float radians)
        {
            _rotation = Matrix3.CreateRotation(radians);
        }
        public void Rotate(float radians)
        {
            _rotation *= Matrix3.CreateRotation(radians);
        }
        public void SetScale(float x, float y)
        {
            _scale = Matrix3.CreateScale(x, y);
        }

        /// <summary> 
        /// Rotates the actor to face the given position 
        /// </summary> 
        /// <param name="position">The position the actor should be facing</param> 
        public void LookAt(Vector2 position)
        {
            //Find the direction that the actor should look in 
            Vector2 direction = (position - LocalPosition).Normalized;

            //Use the dotproduct to find the angle the actor needs to rotate 
            float dotProd = Vector2.DotProduct(Forward, direction);
            if (Math.Abs(dotProd) > 1)
                return;
            float angle = (float)Math.Acos(dotProd);

            //Find a perpindicular vector to the direction 
            Vector2 perp = new Vector2(direction.Y, -direction.X);

            //Find the dot product of the perpindicular vector and the current forward 
            float perpDot = Vector2.DotProduct(perp, Forward);

            //If the result isn't 0, use it to change the sign of the angle to be either positive or negative 
            if (perpDot != 0)
                angle *= -perpDot / Math.Abs(perpDot);

            Rotate(angle);
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
            Forward = Velocity.Normalized;
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
            UpdateFacing();
            LocalPosition += _velocity * deltaTime;
        }
        public virtual void Draw()
        {
            //draws sprite and direction they are pointing
            Raylib.DrawLine(
                (int)(WorldPosition.X * 32),
                (int)(WorldPosition.Y * 32),
                (int)((WorldPosition.X + Forward.X) * 32),
                (int)((WorldPosition.Y + Forward.Y) * 32),
                Color.WHITE
            );

            Raylib.DrawCircleLines((int)(WorldPosition.X * 32),(int)(WorldPosition.Y * 32),_collisionRadius * 32,Color.GREEN);

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if (WorldPosition.X >= 0 && WorldPosition.X < Console.WindowWidth
                && WorldPosition.Y >= 0 && WorldPosition.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)WorldPosition.X, (int)WorldPosition.Y);
                Console.Write(_icon);
            }

            //Reset console text color to be default color
            Console.ForegroundColor = Game.DefaultColor;
        }
        public virtual void End()
        {
            Started = false;
        }

    }
}
