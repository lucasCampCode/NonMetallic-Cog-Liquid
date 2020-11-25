using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames3D
{
    class Game
    {
        private static Random _random = new Random();
        private int _time = 0;
        private int _seconds = 0;
        private int _minutes = 0;
        private static bool _gameover = false;
        private static bool _debug = false;
        private static bool _showControls = false;
        private static bool _playerInfo = false;
        private static Scene[] _scenes;
        public Scene scene1 = new Scene();
        private Scene scene2 = new Scene();
        private Collectible _target;
        private Player _player1;
        private Camera3D _camera = new Camera3D();
        private static int _currentSceneIndex;

        public static int CurrentSceneIndex { get { return _currentSceneIndex; } }
        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;

        public static bool GameOver { get { return _gameover; } set { _gameover = value; } }
        public static Random Random { get => _random; set => _random = value; }

        public static bool Debug { get => _debug; set => _debug = value; }
        public static bool ShowControls { get => _showControls; set => _showControls = value; }
        public static bool PlayerInfo { get => _playerInfo; set => _playerInfo = value; }

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
            _camera.position = new System.Numerics.Vector3(0.0f, 20.0f, 20.0f);
            _camera.target = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
            _camera.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
            _camera.fovy = 45.0f;
            _camera.type = CameraType.CAMERA_PERSPECTIVE;

            _target = new Collectible(10, 1, 0, Color.BROWN, Shape.CUBE, 1);

            _player1 = new Player((0, 0, 0), Color.BEIGE, Shape.NULL, 4);
            _player1.Speed = 5;

            scene1.AddActor(_player1);
            scene1.AddActor(_target);
            scene2.AddActor(_player1);
            int startingSceneIndex = 0;
            startingSceneIndex = AddScene(scene1);
            AddScene(scene2);
        }
        private void Update(float deltaTime)
        {
            _time = (int)Raylib.GetTime();
            
            if(_time % 1 == 0)
            {
                _seconds += 1;
            }

            if (_seconds > 60)
            {
                _minutes += 1;
                _seconds = 0;
            }

            _camera.position = new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 20.0f, _player1.WorldPosition.Z + 20.0f);
            _camera.target = new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y, _player1.WorldPosition.Z);

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
            Raylib.DrawGrid(100, 1);
            Raylib.DrawPlane(new System.Numerics.Vector3(0, 0, 0), new System.Numerics.Vector2(100,100), Raylib.Fade(Color.GRAY, 0.75f));

            Raylib.EndMode3D();

            Raylib.DrawText("Cubes collected: " + _player1.CubesCollected, (int)Raylib.GetWorldToScreen(new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 5, _player1.WorldPosition.Z), _camera).X - Raylib.MeasureText("Cubes collected: " + _player1.CubesCollected, 20) / 2, (int)Raylib.GetWorldToScreen(new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 5, _player1.WorldPosition.Z), _camera).Y, 20, Color.BLACK);

            Raylib.DrawText("seconds:"+ _time, Raylib.GetScreenWidth()/2 - Raylib.MeasureText("seconds:" + _time, 20)/2, 5, 20, Color.BLACK);
            Raylib.DrawText("minutes:" + _minutes, Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("minutes:" + _minutes, 20) / 2, 25, 20, Color.BLACK);
            if (ShowControls)
            {
                Raylib.DrawText("press F1 to not show controls", Raylib.GetScreenWidth() - Raylib.MeasureText("press F1 to not show controls", 20), 5, 20, Color.BLACK);
                Raylib.DrawText("w to move tank forward", 3, 5, 20, Color.BLACK);
                Raylib.DrawText("s to move tank backwards", 3, 25, 20, Color.BLACK);
                Raylib.DrawText("a to rotate tank left", 3, 45, 20, Color.BLACK);
                Raylib.DrawText("d to rotate tank right", 3, 65, 20, Color.BLACK);

                Raylib.DrawText("up arrow to raise cannon", 3, 85 + 10, 20, Color.BLACK);
                Raylib.DrawText("down arrow to lower cannon", 3, 105 + 10, 20, Color.BLACK);
                Raylib.DrawText("left arrow to rotate cannon left", 3, 125 + 10, 20, Color.BLACK);
                Raylib.DrawText("right arrow to rotate cannon right", 3, 145 + 10, 20, Color.BLACK);


                Raylib.DrawText("shift to move slowly", 3, 165 + 20, 20, Color.BLACK);
                Raylib.DrawText("control to move quickly", 3, 185 + 20, 20, Color.BLACK);

                Raylib.DrawText("hold space to charge shot", 3, 205 + 30, 20, Color.BLACK);
                Raylib.DrawText("realese space to shoot", 3, 225 + 30, 20, Color.BLACK);
            }
            else
                Raylib.DrawText("press F1 to show controls", Raylib.GetScreenWidth() - Raylib.MeasureText("press F1 to show controls", 20), 5, 20, Color.BLACK);
            if (Debug)
            {
                Raylib.DrawText("press F2 to not show collisionSpheres", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to not show collisionSpheres", 20), 25, 20, Color.BLACK);
            }
            else
                Raylib.DrawText("press F2 to show collisionSpheres", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to show collisionSpheres", 20), 25, 20, Color.BLACK);
            if (PlayerInfo)
            {
                Raylib.DrawText("press F3 to not show Player Info", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to not show Player Info", 20), 45, 20, Color.BLACK);
                
                Raylib.DrawText("player position x:" + _player1.WorldPosition.X, 3, Raylib.GetScreenHeight() - 140, 20, Color.BLACK);
                Raylib.DrawText("player position y:" + _player1.WorldPosition.Y, 3, Raylib.GetScreenHeight() - 120, 20, Color.BLACK);
                Raylib.DrawText("player position z:" + _player1.WorldPosition.Z, 3, Raylib.GetScreenHeight() - 100, 20, Color.BLACK);
                Raylib.DrawText("collectible position x:" + _target.WorldPosition.X, 3, Raylib.GetScreenHeight() - 80, 20, Color.BLACK);
                Raylib.DrawText("collectible position x:" + _target.WorldPosition.Y, 3, Raylib.GetScreenHeight() - 60, 20, Color.BLACK);
                Raylib.DrawText("collectible position x:" + _target.WorldPosition.Z, 3, Raylib.GetScreenHeight() - 40, 20, Color.BLACK);
                Raylib.DrawText("bullets launch speed:" + _player1.BulletSpeed, 3, Raylib.GetScreenHeight() - 20, 20, Color.BLACK);
            }
            else
                Raylib.DrawText("press F3 to show Player Info", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to show Player Info", 20), 45, 20, Color.BLACK);

            Raylib.EndDrawing();
        }
        private void End()
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        }
        public void DebugGame()
        {

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