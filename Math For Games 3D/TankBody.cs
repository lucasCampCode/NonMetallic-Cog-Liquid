using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class TankBody:Actor
    {

        private Mesh _tankBody;
        private float _rotateBody;
        public TankBody(float x,float y,float z, float collisionRadius) : base(x,y,z,collisionRadius)
        {
            _tankBody = Raylib.GenMeshCube(4,1,3);
        }
        public override void Update(float deltaTime)
        {
            _rotateBody = -(float)Math.Atan2(Forward.Z, Forward.X) * (float)(180/Math.PI);
            base.Update(deltaTime);
        }
        public override void Draw()
        {
            
            Raylib.DrawModelEx(
                                Raylib.LoadModelFromMesh(_tankBody),
                                new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y + 1.2f, WorldPosition.Z),
                                new System.Numerics.Vector3(0, 1, 0),
                                _rotateBody,
                                new System.Numerics.Vector3(1,1,1),
                                Color.LIME
                              ) ;
            base.Draw();
        }
    }
}
