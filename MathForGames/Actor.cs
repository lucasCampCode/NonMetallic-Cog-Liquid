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
        private float _rotate = 0;
        protected Vector2 _velocity;
        protected Matrix3 _lolcalTransform = new Matrix3();
        protected Matrix3 _globalTransform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _rotation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Sprite _sprite;
        protected Actor[] _children = new Actor[0];

        public bool Started { get; private set; }
        public Actor Parent { get; private set; }
        public Vector2 Forward
        {
            get { return new Vector2(_globalTransform.m11,_globalTransform.m21); }
            
        }

        public Vector2 WorldPosition
        {
            get { return new Vector2(_globalTransform.m13, _globalTransform.m23); }
        }

        public Vector2 LocalPosition
        {
            get
            {
                return new Vector2(_lolcalTransform.m13,_lolcalTransform.m23);
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
        public Actor( float x, float y,char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            LocalPosition = new Vector2(x, y);
            _velocity = new Vector2();
            _color = color;
            _sprite = new Sprite("Images/enemy.png");
        }
        
        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this(x,y,icon,color)
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
            if (index < 0 || index >_children.Length)
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

        public virtual void Start()
        {
            Started = true;
        }

        public void SetTranslate(Vector2 position)
        {
            _translation.m13 = position.X;
            _translation.m23 = position.Y;
        }


        public void SetRotation(float radians)
        {
            _rotation.m11 = (float)Math.Cos(radians);
            _rotation.m12 = (float)Math.Sin(radians);
            _rotation.m21 = -(float)Math.Sin(radians);
            _rotation.m22 = (float)Math.Cos(radians);
        }
        public void Rotate(float radians)
        {
            _rotate += radians;
            SetRotation(_rotate);
        }
        
        public void SetScale(float x,float y)
        {
            _scale.m11 = x;
            _scale.m22 = y;
        }
        private void UpdateTransform()
        {
            _lolcalTransform =  _translation * _rotation * _scale;
        }

        public void UpdateGlobalTransform()
        {
            if (Parent != null)
            {
                _globalTransform = Parent._globalTransform * _lolcalTransform;

            }
            else
            {
                _globalTransform = _lolcalTransform;
            }
        }
        public virtual void Update(float deltaTime)
        {
            //Increase position by the current velocity
            
            UpdateTransform();
            UpdateGlobalTransform();
            Rotate(0.15f);
            LocalPosition += _velocity * deltaTime;
        }


        public virtual void Draw()
        {
            //draws sprite and direction they are pointing
            _sprite.Draw(_globalTransform);
            Raylib.DrawLine(
                (int)(WorldPosition.X * 32),
                (int)(WorldPosition.Y * 32),
                (int)((WorldPosition.X + Forward.X) * 32),
                (int)((WorldPosition.Y + Forward.Y) * 32),
                Color.WHITE
            );

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(LocalPosition.X >= 0 && LocalPosition.X < Console.WindowWidth 
                && LocalPosition.Y >= 0  && LocalPosition.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)LocalPosition.X, (int)LocalPosition.Y);
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
