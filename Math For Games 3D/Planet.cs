using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames3D
{
    class Planet : Actor
    {
        private Vector3 _rotate = new Vector3();

        public Planet(Vector3 position,Vector3 rotation,float collisionRadius,Color color) : base(position.X,position.Y,position.Z,color,collisionRadius)
        {
            _rotate.X = rotation.X;
            _rotate.Y = rotation.Y;
            _rotate.Z = rotation.Z;
        }
        public override void Update(float deltaTime)
        {
            Rotate(_rotate.X,_rotate.Y,_rotate.Z);
            base.Update(deltaTime);
        }
        public override void Draw()
        {

            Raylib.DrawSphere(new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z), _collisionRadius, _rayColor);
            base.Draw();
        }
    }
}
