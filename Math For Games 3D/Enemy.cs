using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class Enemy : Actor
    {
        public Enemy(float x,float y,float z, Color rayColor,Shape shape,float collisionRadius) : base(x,y,z,rayColor,shape,collisionRadius)
        {
            
        }
    }
}
