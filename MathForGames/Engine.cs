using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MathLibrary;

namespace MathForGames
{
    class Engine
    {

        private static bool _applicationShouldClose = false;
        private static int _currentSceneIndex;
        private Scene[] _scenes = new Scene[0];
        private static Icon[,] _buffer;

        /// <summary>
        /// Called to begin the application
        /// </summary>
        public void Run()
        {
            //Call start for the entire application
            Start();

            //Loop until the application is told to close
            while (!_applicationShouldClose)
            {
                Update();
                Draw();

                Thread.Sleep(50);
            }

            //Call end for the entire application
            End();

        }

        /// <summary>
        /// Called when the application starts
        /// </summary>
        private void Start()
        {
            Scene scene = new Scene();

            Player player = new Player('@', 3, 10, 1, "Player", ConsoleColor.Cyan);

            Enemy enemy = new Enemy('O', 8, 0, 1, "Enemy", ConsoleColor.Red);

            Enemy enemy1 = new Enemy('O', 16, 0, 2, "Enemy", ConsoleColor.Red);

            Enemy enemy2 = new Enemy('O', 24, 0, 4, "Enemy", ConsoleColor.Red);

            Enemy enemy3 = new Enemy('O', 32, 0, 5, "Enemy", ConsoleColor.Red);

            Enemy enemy4 = new Enemy('O', 40, 0, 10, "Enemy", ConsoleColor.Red);

            Actor end = new Actor('O', 48, 10, "End", ConsoleColor.Green);

            UIText healthText = new UIText(53, 3, "Health", ConsoleColor.White, 20, 10, " Hello. \n\n Get to the GREEN     circle to survive.");

            UIText livesText = new UIText(54, 1, "Lives", ConsoleColor.White, 10, 10);

            UIText zone = new UIText(20, 23, "Throw Zone", ConsoleColor.Gray, 10, 10, "Throw Zone");

            PlayerHud playerHud = new PlayerHud(player, livesText);

            for (int i = 2; i < 50; i++)
            {
                Actor wall = new Actor('_', i, 0, "Wall");
                scene.AddActor(wall);
            }

            for (int i = 1; i < 26; i++)
            {
                Actor wall = new Actor('▌', 1, i, "Wall");
                scene.AddActor(wall);
            }

            for (int i = 2; i < 50; i++)
            {
                Actor wall = new Actor('_', i, 25, "Wall");
                scene.AddActor(wall);
            }

            for (int i = 1; i < 26; i++)
            {
                Actor wall = new Actor('▌', 50, i, "Wall");
                scene.AddActor(wall);
            }

            for (int i = 2; i < 50; i++)
            {
                Actor throwZone = new Actor('_', i, 20, "ThrowZone");
                scene.AddActor(throwZone);
            }

            scene.AddUIElement(healthText);
            scene.AddUIElement(playerHud);
            scene.AddUIElement(zone);
            scene.AddActor(player);
            scene.AddActor(enemy);
            scene.AddActor(enemy1);
            scene.AddActor(enemy2);
            scene.AddActor(enemy3);
            scene.AddActor(enemy4);
            scene.AddActor(end);

            _currentSceneIndex = AddScene(scene);

            _scenes[_currentSceneIndex].Start();
        }

        /// <summary>
        /// Called everytime the game loops
        /// </summary>
        private void Update()
        {
            _scenes[_currentSceneIndex].Update();
            _scenes[_currentSceneIndex].UpdateUI();

            while (Console.KeyAvailable)
                Console.ReadKey(true);

        }

        /// <summary>
        /// Called everytime the game loops to update visuals
        /// </summary>
        private void Draw()
        {
            Console.CursorVisible = false;

            //Clear the stuff that was on the screen in the last frame
            _buffer = new Icon[Console.WindowWidth, Console.WindowHeight - 1];

            //Reset the cursor position to the top so the previous screen is drawn over
            Console.SetCursorPosition(0, 0);

            //Adds all actor icons to buffer
            _scenes[_currentSceneIndex].Draw();
            _scenes[_currentSceneIndex].DrawUI();


            //Iterate through buffer
            for (int y = 0; y < _buffer.GetLength(1); y++)
            {
                for (int x = 0; x < _buffer.GetLength(0); x++)
                {
                    if (_buffer[x, y].Symbol == '\0')
                        _buffer[x, y].Symbol = ' ';
                    //Set console text color to be the color of item at buffer
                    Console.ForegroundColor = _buffer[x, y].color;
                    //Print the symbol of the item in the buffer
                    Console.Write(_buffer[x, y].Symbol);
                }

                //Skip a line once the end of a row has been reached
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
        }

        /// <summary>
        /// Adds a scene to the engine's scene array
        /// </summary>
        /// <param name="scene">The scene that will be added to the scene array</param>
        /// <returns>The index where the new scene is located</returns>
        public int AddScene(Scene scene)
        {
            //Create a new temporary array
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy all values from old array into the new array
            for (int i = 0; i < _scenes.Length; i++)
            {
                tempArray[i] = _scenes[i];
            }

            //Set the last indec to be the new scene
            tempArray[_scenes.Length] = scene;

            //Set the old array to be the new array
            _scenes = tempArray;

            //Return the last index
            return _scenes.Length - 1;
        }

        /// <summary>
        /// Gets the next key in the input stream
        /// </summary>
        /// <returns>The key that was pressed</returns>
        public static ConsoleKey GetNextKey()
        {
            //If there is no key being pressed...
            if (!Console.KeyAvailable)
                //...return
                return 0;

            //Return the current key being pressed
            return Console.ReadKey(true).Key;
        }


        /// <summary>
        /// Adds the icon to the buffer to print to the screen in the next draw call.
        /// Prints the icon at the given position in the buffer
        /// </summary>
        /// <param name="icon">The icon to draw</param>
        /// <param name="position">The position of the icon in the buffer</param>
        /// <returns>False if the position is outside the bounds of the buffer</returns>
        public static bool Render(Icon icon, Vector2 position)
        {
            //If the position id out of bounds...
            if (position.X < 0 || position.X >= _buffer.GetLength(0) || position.Y < 0 || position.Y >= _buffer.GetLength(1))
                //...return false
                return false;

            //Set the buffer at the index of the given position
            _buffer[(int)position.X, (int)position.Y] = icon;
            return true;
        }

        /// <summary>
        /// Ends the application
        /// </summary>
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }

        public static void ChangeScene(int currentSceneIndex)
        {
            _currentSceneIndex = currentSceneIndex;
        }
    }
}
