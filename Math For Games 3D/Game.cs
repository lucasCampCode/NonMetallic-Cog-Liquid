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
        private float _seconds = 0;
        private int _minutes = 0;
        private static bool _gameover = false;
        private static bool _debug = false;
        private static bool _showControls = false;
        private static bool _playerInfo = false;
        private static Scene[] _scenes;
        private Scene _scene1 = new Scene();
        private Scene _scene2 = new Scene();
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
        /// <summary>
        /// gets the current scene in game
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

        private void Start()
        {
            //initilization of the raylib window
            Raylib.InitWindow(1600, 900, "Math For Games");
            Raylib.SetTargetFPS(60);
            _camera.position = new System.Numerics.Vector3(0.0f, 20.0f, 20.0f);
            _camera.target = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
            _camera.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
            _camera.fovy = 45.0f;
            _camera.type = CameraType.CAMERA_PERSPECTIVE;


            //initilization of the actors in game
            _target = new Collectible(10, 1, 0, Color.BROWN, Shape.CUBE, 1);
            _player1 = new Player(0, 0, 0, 4);
            _player1.Speed = 5;

            //adds player and target to the playable scenes
            _scene1.AddActor(_player1);
            _scene1.AddActor(_target);
            _scene2.AddActor(_player1);

            //add scenes to game 
            int startingSceneIndex = AddScene(_scene1);
             AddScene(_scene2);

            //Sets the current scene to be the starting scene index
            SetCurrentScene(startingSceneIndex);
        }
        private void Update(float deltaTime)
        {
            //timer for the players
            _seconds += deltaTime;
            //resets seconds and transfers to minutes
            if (_seconds > 60)
            {
                _minutes += 1;
                _seconds = 0;
            }
            //move camera along with the player
            _camera.position = new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 20.0f, _player1.WorldPosition.Z + 20.0f);
            _camera.target = new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y, _player1.WorldPosition.Z);

            if (!_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].Start();

            _scenes[_currentSceneIndex].Update(deltaTime);

            //when game reaches a certain time switches the game to the end screen
            if (_minutes >= 4)
            {
                _currentSceneIndex = 1;
                _minutes = 4;
                _seconds = 0;
            }
        }
        private void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.BeginMode3D(_camera);

            Raylib.ClearBackground(Color.DARKGRAY);
            //draws whatever is in scene
            _scenes[_currentSceneIndex].Draw();
            //draws the ground and walls
            Raylib.DrawGrid(100, 1);
            Raylib.DrawPlane(new System.Numerics.Vector3(0, 0, 0), new System.Numerics.Vector2(100, 100), Raylib.Fade(Color.GRAY, 0.75f));
            Raylib.DrawCube(new System.Numerics.Vector3(50.5f, 2.5f, 0), 1, 5, 102, Raylib.Fade(Color.GRAY, 0.5f));
            Raylib.DrawCube(new System.Numerics.Vector3(-50.5f, 2.5f, 0), 1, 5, 102, Raylib.Fade(Color.GRAY, 0.5f));
            Raylib.DrawCube(new System.Numerics.Vector3(0, 2.5f, 50.5f), 102, 5, 1, Raylib.Fade(Color.GRAY, 0.5f));
            Raylib.DrawCube(new System.Numerics.Vector3(0, 2.5f, -50.5f), 102, 5, 1, Raylib.Fade(Color.GRAY, 0.5f));
            Raylib.EndMode3D();

            
            if (CurrentSceneIndex != _scenes.Length - 1) 
            {
                Raylib.DrawText("Cubes collected: " + _player1.CubesCollected, (int)Raylib.GetWorldToScreen(new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 5, _player1.WorldPosition.Z), _camera).X - Raylib.MeasureText("Cubes collected: " + _player1.CubesCollected, 20) / 2, (int)Raylib.GetWorldToScreen(new System.Numerics.Vector3(_player1.WorldPosition.X, _player1.WorldPosition.Y + 5, _player1.WorldPosition.Z), _camera).Y, 20, Color.BLACK);
                //draws timer for player refrence
                Raylib.DrawText("seconds:" + (int)_seconds, Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("seconds:" + (int)_seconds, 20) / 2, 5, 20, Color.BLACK);
                Raylib.DrawText("minutes:" + _minutes, Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("minutes:" + _minutes, 20) / 2, 25, 20, Color.BLACK);
                DrawOptions(); 
            }
            else
            {
                Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), Raylib.Fade(Color.WHITE , 0.75f));
                Raylib.DrawText("GameOver", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("GameOver",50) / 2, 100, 50, Color.BLACK);
                Raylib.DrawText("you collected a total of " + _player1.CubesCollected +" cubes!", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("you collected a total of " + _player1.CubesCollected + " cubes!", 50) / 2, 150, 50, Color.BLACK);
                Raylib.DrawText("press space to exit game", Raylib.GetScreenWidth() / 2 - Raylib.MeasureText("press space to exit game", 50) / 2, 200, 50, Color.BLACK);
                
                    GameOver = Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE);
            }
            Raylib.EndDrawing();
        }
        private void DrawOptions()
        {
            //draws debug controls
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
            //draw debug for collision shperes
            if (Debug)
            {
                Raylib.DrawText("press F2 to not show collisionSpheres", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to not show collisionSpheres", 20), 25, 20, Color.BLACK);
            }
            else
                Raylib.DrawText("press F2 to show collisionSpheres", Raylib.GetScreenWidth() - Raylib.MeasureText("press F3 to show collisionSpheres", 20), 25, 20, Color.BLACK);
            //draws player info
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