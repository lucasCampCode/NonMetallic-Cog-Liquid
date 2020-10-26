using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames
{
    /// <summary>
    /// This is the goal the player must reach to end the game. 
    /// </summary>
    class Goal : Actor
    {
        private Actor _player;

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Goal(float x, float y, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {
            _player = player;
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
        }

        /// <summary>
        /// Checks to see if the player is in range of the goal.
        /// </summary>
        /// <returns></returns>
        private bool CheckPlayerDistance()
        {
            float distance = (_player.Position - Position).Magnitude;
            return distance <= 1;
        }

        public override void Update(float deltaTime)
        {
            //If the player is in range of the goal, end the game
            if (CheckPlayerDistance())
                Game.SetCurrentScene(Game.CurrentSceneIndex + 1);
            

            base.Update(deltaTime);
        }
    }
}
