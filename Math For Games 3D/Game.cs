using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames3D
{
    class Game
    {
        private static bool _gameover = false;
        private static Scene[] _scenes;
        private Camera3D _camera = new Camera3D();
        private static int _currentSceneIndex;
        public static int CurrentSceneIndex { get { return _currentSceneIndex; } }
        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;
        public static bool GameOver { get { return _gameover; } set { _gameover = value; } }

        public static Scene GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Length)
                return new Scene();

            return _scenes[index];
        }

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

        private void Start()
        {
            Raylib.InitWindow(1600, 900, "Math For Games");
            Raylib.SetTargetFPS(60);
            _camera.position = new System.Numerics.Vector3(0.0f,20.0f,20.0f);
            _camera.target = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
            _camera.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
            _camera.fovy = 45.0f;
            _camera.type = CameraType.CAMERA_PERSPECTIVE;

            Scene scene1 = new Scene();
            Planet sun = new Planet((0, 0, 0), (0.025f, 0, 0), 2, Color.YELLOW);
            Planet earth = new Planet((5, 0, 0), (0, 0.025f,0), 1, Color.BLUE);
            Planet moon = new Planet((2, 0, 0), (0, 0, 0.025f), 0.5f, Color.GRAY);
            
            sun.AddChild(earth);
            earth.AddChild(moon);

            scene1.AddActor(sun);
            scene1.AddActor(moon);
            scene1.AddActor(earth);
            int startingSceneIndex = 0;
            startingSceneIndex = AddScene(scene1);
        }
        private void Update(float deltaTime)
        {
            if (!_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].Start();

            _scenes[_currentSceneIndex].Update(deltaTime);
        }
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode3D(_camera);
            Raylib.ClearBackground(Color.DARKGRAY);

            _scenes[_currentSceneIndex].Draw();
            Raylib.DrawGrid(50,1.0f);
            Raylib.EndMode3D();
            Raylib.EndDrawing();
        }
        private void End()
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        }
        
        public void Run()
        {
            Start();

            while (!_gameover && !Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                Update(deltaTime);
                Draw();
            }

            End();
        }
    }
}
