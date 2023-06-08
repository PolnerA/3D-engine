using GameEngine;

namespace MyGame
{
    static class MyGame
    {
        //does what we want it to 100%
        //window width and height for fullscreen
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;

        private const string WindowTitle = "Explore";

        private static void Main(string[] args)
        {//organize code (figure out what it has to do)
            // Initialize the game.
            Game.Initialize(WindowWidth, WindowHeight, WindowTitle);

            // Create our scene.
            GameScene scene = new GameScene();
            Game.SetScene(scene);

            // Run the game loop.
            Game.Run();
        }
    }
}