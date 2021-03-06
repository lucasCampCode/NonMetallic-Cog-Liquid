﻿using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

namespace MathForGames3D
{
    class Scene
    {
        private Actor[] _actors;
        private Matrix4 _transform = new Matrix4();
        public Matrix4 World { get { return _transform; } }
        public bool Started { get; private set; }
        public Scene()
        {
            _actors = new Actor[0];
        }

        public void AddActor(Actor actor)
        {
            //Create a new array with a size one greater than our old array
            Actor[] appendedArray = new Actor[_actors.Length + 1];
            //Copy the values from the old array to the new array
            for (int i = 0; i < _actors.Length; i++)
            {
                appendedArray[i] = _actors[i];
            }
            //Set the last value in the new array to be the actor we want to add
            appendedArray[_actors.Length] = actor;
            //Set old array to hold the values of the new array
            _actors = appendedArray;
        }
        public bool RemoveActor(int index)
        {
            //Check to see if the index is outside the bounds of our array
            if (index < 0 || index >= _actors.Length)
            {
                return false;
            }

            bool actorRemoved = false;

            //Create a new array with a size one less than our old array 
            Actor[] newArray = new Actor[_actors.Length - 1];
            //Create variable to access tempArray index
            int j = 0;
            //Copy values from the old array to the new array
            for (int i = 0; i < _actors.Length; i++)
            {
                //If the current index is not the index that needs to be removed,
                //add the value into the old array and increment j
                if (i != index)
                {
                    if (j < newArray.Length)
                    {
                        newArray[j] = _actors[i];
                        j++;
                    }
                }
                else
                {
                    actorRemoved = true;
                    if (_actors[i].Started)
                        _actors[i].End();
                }
            }
            //Set the old array to be the tempArray
            _actors = newArray;

            return actorRemoved;
        }
        public bool RemoveActor(Actor actor)
        {
            //Check to see if the actor was null
            if (actor == null || _actors.Length == 0)
            {
                return false;
            }

            bool actorRemoved = false;
            //Create a new array with a size one less than our old array
            Actor[] newArray = new Actor[_actors.Length - 1];
            //Create variable to access tempArray index
            int j = 0;
            //Copy values from the old array to the new array
            for (int i = 0; i < _actors.Length; i++)
            {
                if (actor != _actors[i])
                {
                    if (j < newArray.Length)
                    {
                        newArray[j] = _actors[i];
                        j++;
                    }
                }
                else
                {
                    actorRemoved = true;
                    if (actor.Started)
                        actor.End();
                }
            }

            //Set the old array to the new array
            _actors = newArray;
            //Return whether or not the removal was successful
            return actorRemoved;
        }


        public virtual void Start()
        {
            Started = true;
        }
        /// <summary>
        /// checks the collisions bettween actors
        /// </summary>
        public virtual void CheckCollision()
        {
            for (int i = 0; i < _actors.Length; i++)
            {
                for (int j = 0; j < _actors.Length; j++)
                {
                    //breaks the objects is out of bounds error
                    if (i >= _actors.Length)
                        break;

                    if (_actors[i].CheckCollision(_actors[j]) && i != j)
                        _actors[i].OnCollision(_actors[j]);
                }
            }
        }
        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < _actors.Length; i++)
            {
                if (!_actors[i].Started)
                    _actors[i].Start();

                _actors[i].Update(deltaTime);
            }
            CheckCollision();
        }
        public virtual void Draw()
        {
            for (int i = 0; i < _actors.Length; i++)
            {
                _actors[i].Draw();
            }
        }
        public virtual void End()
        {
            for (int i = 0; i < _actors.Length; i++)
            {
                if (_actors[i].Started)
                    _actors[i].End();
            }

            Started = false;
        }
    }
}