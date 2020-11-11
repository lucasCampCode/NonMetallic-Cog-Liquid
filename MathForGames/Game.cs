using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Game
    {
        public static Random rand = new Random();
        private static bool _gameOver = false;
        private static Scene[] _scenes;
        private static int _currentSceneIndex;
        public static int CurrentSceneIndex
        {
            get
            {
                return _currentSceneIndex;
            }
        }
        public static int SceneLength { get { return _scenes.Length; } }
        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Used to set the value of game over.
        /// </summary>
        /// <param name="value">If this value is true, the game will end</param>
        public static void SetGameOver(bool value)
        {
            _gameOver = value;
        }

        /// <summary>
        /// Returns the scene at the index given.
        /// Returns an empty scene if the index is out of bounds
        /// </summary>
        /// <param name="index">The index of the desired scene</param>
        /// <returns></returns>
        public static Scene GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Length)
                return new Scene();

            return _scenes[index];
        }

        /// <summary>
        /// Returns the scene that is at the index of the 
        /// current scene index
        /// </summary>
        /// <returns></returns>
        public static Scene GetCurrentScene()
        {
            return _scenes[_currentSceneIndex];
        }

        /// <summary>
        /// Adds the given scene to the array of scenes.
        /// </summary>
        /// <param name="scene">The scene that will be added to the array</param>
        /// <returns>The index the scene was placed at. Returns -1 if
        /// the scene is null</returns>
        public static int AddScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return -1;

            //Create a new temporary array that one size larger than the original
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy values from old array into new array
            for (int i = 0; i < _scenes.Length; i++)
            {
                tempArray[i] = _scenes[i];
            }

            //Store the current index
            int index = _scenes.Length;

            //Sets the scene at the new index to be the scene passed in
            tempArray[index] = scene;

            //Set the old array to the tmeporary array
            _scenes = tempArray;

            return index;
        }

        /// <summary>
        /// Finds the instance of the scene given that inside of the array
        /// and removes it
        /// </summary>
        /// <param name="scene">The scene that will be removed</param>
        /// <returns>If the scene was successfully removed</returns>
        public static bool RemoveScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return false;

            bool sceneRemoved = false;

            //Create a new temporary array that is one less than our original array
            Scene[] tempArray = new Scene[_scenes.Length - 1];

            //Copy all scenes except the scene we don't want into the new array
            int j = 0;
            for (int i = 0; i < _scenes.Length; i++)
            {
                if (tempArray[i] != scene)
                {
                    tempArray[j] = _scenes[i];
                    j++;
                }
                else
                {
                    sceneRemoved = true;
                }
            }

            //If the scene was successfully removed set the old array to be the new array
            if (sceneRemoved)
                _scenes = tempArray;

            return sceneRemoved;
        }

        /// <summary>
        /// Sets the current scene in the game to be the scene at the given index
        /// </summary>
        /// <param name="index">The index of the scene to switch to</param>
        public static void SetCurrentScene(int index)
        {
            //If the index is not within the range of the the array return
            if (index < 0 || index >= _scenes.Length)
                return;

            //Call end for the previous scene before changing to the new one
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();

            //Update the current scene index
            _currentSceneIndex = index;
        }

        /// <summary>
        /// Returns true while a key is being pressed
        /// </summary>
        /// <param name="key">The ascii value of the key to check</param>
        /// <returns></returns>
        public static bool GetKeyDown(int key)
        {
            return Raylib.IsKeyDown((KeyboardKey)key);
        }

        /// <summary>
        /// Returns true while if key was pressed once
        /// </summary>
        /// <param name="key">The ascii value of the key to check</param>
        /// <returns></returns>
        public static bool GetKeyPressed(int key)
        {
            return Raylib.IsKeyPressed((KeyboardKey)key);
        }

        public Game()
        {
            _scenes = new Scene[0];
        }

        //Called when the game begins. Use this for initialization.
        public void Start()
        {
            //Creates a new window for raylib
            Raylib.InitWindow(1600, 900, "Math For Games");
            Raylib.SetTargetFPS(60);

            //Set up console window
            Console.CursorVisible = false;
            Console.Title = "Math For Games";

            //Create a new scene for our actors to exist in
            Scene scene1 = new Scene();
            Scene scene2 = new Scene();
            Scene scene3 = new Scene();
            Planet[] lines = new Planet[10];

            //Create the actors to add to our scene
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Planet(1, 0, 0.5f, 0.10f);
            }
            Enemy enemy1 = new Enemy(1, 11.5f, Color.GREEN, 0.5f, new Vector2(15.5f, 23), new Vector2(30, 11.5f), new Vector2(15.5f, 1), new Vector2(1, 11.5f), '■', ConsoleColor.Green);
            Enemy enemy2 = new Enemy(15.5f, 11.5f, Color.GREEN, 0.5f, new Vector2(1, 11.5f), new Vector2(30, 11.5f), '■', ConsoleColor.Green);
            Enemy enemy3 = new Enemy(30, 11.5f, Color.GREEN, 0.5f, new Vector2(15.5f, 1), new Vector2(1, 11.5f), new Vector2(15.5f, 23), new Vector2(30, 11.5f), '■', ConsoleColor.Green);
            Enemy enemy4 = new Enemy(15.5f, 1, Color.GREEN, 0.5f, new Vector2(1, 11.5f), new Vector2(15.5f, 23), new Vector2(30, 11.5f), new Vector2(15.5f, 1), '■', ConsoleColor.Green);
            Enemy enemy5 = new Enemy(15.5f, 23, Color.GREEN, 0.5f, new Vector2(30, 11.5f), new Vector2(15.5f, 1), new Vector2(1, 11.5f), new Vector2(15.5f, 23), '■', ConsoleColor.Green);

            Player player = new Player(1, 26, Color.BLUE, 0.5f, '@', ConsoleColor.Red);
            Goal goal = new Goal(20, 13, Color.GREEN, 0.5f, player, 'G', ConsoleColor.Green);

            //Initialize the enemies' starting values
            enemy1.Speed = 5;
            enemy2.Speed = 5;
            enemy3.Speed = 5;
            enemy4.Speed = 5;
            enemy5.Speed = 5;
            enemy1.Target = player;
            enemy2.Target = player;
            enemy3.Target = player;
            enemy4.Target = player;
            enemy5.Target = player;
            //Set player's starting speed
            player.Speed = 6;
            goal.AddChild(lines[0]);
            for(int i = 0; i < lines.Length-1; i++)
            {
                lines[i].AddChild(lines[i + 1]);
            }

            //Add actors to the scenes
            scene1.AddActor(player);
            for (int i = 0; i < lines.Length; i++)
            {
                scene1.AddActor(lines[i]);
            }
            //scene1.AddActor(enemy1);
            //scene1.AddActor(enemy2);
            //scene1.AddActor(enemy3);
            scene1.AddActor(goal);
            scene2.AddActor(player);
            scene2.AddActor(enemy4);
            scene2.AddActor(enemy2);
            scene2.AddActor(enemy5);
            scene2.AddActor(goal);
            scene3.AddActor(player);
            scene3.AddActor(enemy1);
            scene3.AddActor(enemy2);
            scene3.AddActor(enemy3);
            scene3.AddActor(enemy4);
            scene3.AddActor(enemy5);
            scene3.AddActor(goal);

            //Sets the starting scene index and adds the scenes to the scenes array
            int startingSceneIndex = 0;
            startingSceneIndex = AddScene(scene1);
            AddScene(scene2);
            AddScene(scene3);

            //Sets the current scene to be the starting scene index
            SetCurrentScene(startingSceneIndex);
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime">The time between each frame</param>
        public void Update(float deltaTime)
        {

            if (!_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].Start();

            _scenes[_currentSceneIndex].Update(deltaTime);
        }

        //Used to display objects and other info on the screen.
        public void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.DARKGRAY);
            Console.Clear();
            _scenes[_currentSceneIndex].Draw();

            Raylib.EndDrawing();
        }

        //Called when the game ends.
        public void End()
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        }


        //Handles all of the main game logic including the main game loop.
        public void Run()
        {
            //Call start for all objects in game
            Start();

            //Loops the game until either the game is set to be over or the window closes
            while (!_gameOver && !Raylib.WindowShouldClose())
            {
                //Stores the current time between frames
                float deltaTime = Raylib.GetFrameTime();
                //Call update for all objects in game
                Update(deltaTime);
                //Call draw for all objects in game
                Draw();
                //Clear the input stream for the console window
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            }
            End();

        }
    }
}
