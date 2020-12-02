using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace MathForGames3D
{
    class Bullet : Actor
    {
        private Player _player;
        public Bullet(float x, float y, float z, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, z, collisionRadius, icon, color)
        {

        }
        public Bullet(float x, float y, float z, Color raycolor, Shape shape, float collisionRadius, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, z, raycolor, shape, collisionRadius, icon, color)
        {

        }
        public Bullet(float x, float y, float z, Color raycolor, Shape shape, float collisionRadius, Player holder, char icon = ' ', ConsoleColor color = ConsoleColor.White)
           : base(x, y, z, raycolor, shape, collisionRadius, icon, color)
        {
            _player = holder;
        }
        /// <summary>
        /// what the bullet does when it collides with another actor
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(Actor other)
        {
            Actor[] collected = _player.Rotations;

            if (other is Collectible)
            {
                //relocates the actor hit
                other.LocalPosition = (Game.Random.Next(-50, 50), Game.Random.Next(1, 5), Game.Random.Next(-50, 50));
                for (int i = 0; i < collected.Length; i++)
                {
                    // if player has child has more than one child on rotation then go to next one
                    if (collected[i].Children.Length < 1)
                    {
                        //addes a new cube to player on a new rotation actor
                        _player.CubesCollected += 1;
                        _player.AddObjectToPlayer(i, other);
                        break;
                    }
                }
            }


            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            //if the bullet hits the ground or goes to far it gets destroy
            if (WorldPosition.X < -51 || WorldPosition.X > 51
                || WorldPosition.Y < 0 || WorldPosition.Y > 51
                || WorldPosition.Z < -51 || WorldPosition.Z > 51)
                Destroy();
            //applies gravity to any nonGrounded actors
            if (!OnGround())
                Velocity += _gravity;

            base.Update(deltaTime);
        }
    }
}

