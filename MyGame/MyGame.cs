using GameEngine;

namespace MyGame
{
    static class MyGame
    {
        //window width and height for fullscreen
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;

        private const string WindowTitle = "Explore";

        private static void Main(string[] args)
        {
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