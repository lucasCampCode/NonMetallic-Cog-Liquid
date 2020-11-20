using MathLibrary;
using Raylib_cs;
using System;

namespace MathForGames
{
    /// <summary>
    /// This is the goal the player must reach to end the game. 
    /// </summary>
    class Goal : Actor
    {
        private Actor _player;
        private Sprite _sprite;

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Goal(float x, float y, float collisionRadius, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, collisionRadius, icon, color)
        {
            _player = player;
            _sprite = new Sprite("Images/goal.png");
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Goal(float x, float y, Color rayColor, float collisionRadius, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, collisionRadius, icon, color)
        {
            _player = player;
            _sprite = new Sprite("Images/goal.png");
        }

        public override void OnCollision(Actor other)
        {
            if (other is Player)
            {
                if (Engine.CurrentSceneIndex == Engine.SceneLength - 1)
                {
                    GameManager.GameOver = true;
                }
                Engine.SetCurrentScene(Engine.CurrentSceneIndex + 1);
                _player.LocalPosition = new Vector2(1, 26);
                LocalPosition = new Vector2(Engine.rand.Next(1, 30), Engine.rand.Next(1, 23));
            }
            base.OnCollision(other);
        }

        public override void Update(float deltaTime)
        {
            Rotate(0.1f);
            //If the player is in range of the goal, end the game
            base.Update(deltaTime);
        }
        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}
