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
        public Goal(float x, float y, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {
            _player = player;
            _sprite = new Sprite("Images/goal.png");
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Goal(float x, float y, Color rayColor, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _player = player;
            _sprite = new Sprite("Images/goal.png");
        }

        /// <summary>
        /// Checks to see if the player is in range of the goal.
        /// </summary>
        /// <returns></returns>
        private bool CheckPlayerDistance()
        {
            float distance = (_player.LocalPosition - LocalPosition).Magnitude;
            return distance <= 1;
        }

        public override void Update(float deltaTime)
        {
            Rotate(0.25f);
            //If the player is in range of the goal, end the game
            if (CheckPlayerDistance())
            {
                if (Game.CurrentSceneIndex == Game.SceneLength - 1)
                {
                    Game.SetGameOver(true);
                }
                Game.SetCurrentScene(Game.CurrentSceneIndex + 1);
                _player.LocalPosition = new Vector2(1, 26);
                LocalPosition = new Vector2(Game.rand.Next(1, 30), Game.rand.Next(1, 23));
            }
            base.Update(deltaTime);
        }
        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}
