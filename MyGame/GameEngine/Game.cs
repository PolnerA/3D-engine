using System;
using System.Collections.Generic;
using MyGame;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine
{
    // The Game manages scenes and runs the main game loop.
    static class Game
    {
        // The number of frames that will be drawn to the screen in one second.
        private const int FramesPerSecond = 60;

        // We keep a current and next scene so the scene can be changed mid-frame.
        private static Scene _currentScene;
        private static Scene _nextScene;

        // Cached textures
        private static readonly Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        // Cached sounds
        private static readonly Dictionary<string, SoundBuffer> Sounds = new Dictionary<string, SoundBuffer>();

        // Cached fonts
        private static readonly Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

        // The window we will draw to.
        private static RenderWindow _window;

        // A flag to prevent being initialized twice.
        private static bool _initialized;

        // A Random number generator we can use throughout the game. Seeded with a constant so 
        // the game plays the same every time for easy debugging.
        // @TODO: provide a method to randomize this for when they want variety.
        public static Random Random = new Random(42);

        // Creates our render window. Must be called once at startup.
        public static void Initialize(uint windowWidth, uint windowHeight, string windowTitle)
        {
            // Only initialize once.
            if (_initialized) return;
            _initialized = true;
            Mesh meshcube = new Mesh();
            //south
            Triangle triangle1  = new Triangle(new Vector3f(0, 0, 0), new Vector3f(0, 1, 0), new Vector3f(1, 1, 0));
            Triangle triangle2  = new Triangle(new Vector3f(0, 0, 0), new Vector3f(1, 1, 0), new Vector3f(1, 0, 0));
            //east
            Triangle triangle3  = new Triangle(new Vector3f(1, 0, 0), new Vector3f(1, 1, 0), new Vector3f(1, 1, 1));
            Triangle triangle4  = new Triangle(new Vector3f(1, 0, 0), new Vector3f(1, 1, 1), new Vector3f(1, 0, 1));
            //north
            Triangle triangle5  = new Triangle(new Vector3f(1, 0, 1), new Vector3f(1, 1, 1), new Vector3f(0, 1, 1));
            Triangle triangle6  = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 1, 1), new Vector3f(0, 0, 1));
            //west
            Triangle triangle7  = new Triangle(new Vector3f(0, 0, 1), new Vector3f(0, 1, 1), new Vector3f(0, 1, 0));
            Triangle triangle8  = new Triangle(new Vector3f(0, 0, 1), new Vector3f(0, 1, 0), new Vector3f(0, 0, 0));
            //top
            Triangle triangle9  = new Triangle(new Vector3f(0, 1, 0), new Vector3f(0, 1, 1), new Vector3f(1, 1, 1));
            Triangle triangle10 = new Triangle(new Vector3f(0, 1, 0), new Vector3f(1, 1, 1), new Vector3f(1, 1, 0));
            //bottom
            Triangle triangle11 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 0, 1), new Vector3f(0, 0, 0));
            Triangle triangle12 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 0, 0), new Vector3f(1, 0, 0));
            meshcube.AddTriangle(triangle1);
            meshcube.AddTriangle(triangle2);
            meshcube.AddTriangle(triangle3);
            meshcube.AddTriangle(triangle4);
            meshcube.AddTriangle(triangle5);
            meshcube.AddTriangle(triangle6);
            meshcube.AddTriangle(triangle7);
            meshcube.AddTriangle(triangle8);
            meshcube.AddTriangle(triangle9);
            meshcube.AddTriangle(triangle10);
            meshcube.AddTriangle(triangle11);
            meshcube.AddTriangle(triangle12);
            // Create the render window.
            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio = (float)windowHeight() / (float)windowWidth();
            float fFovRad = (float)1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);
            _window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            _window.Position = new Vector2i(-11,-45);
            _window.SetMouseCursorGrabbed(true);
            _window.SetFramerateLimit(FramesPerSecond);

            // Add a method to be called whenever the "Closed" event fires.
            _window.Closed += ClosedEventHandler;
        }

        // Called whenever you try to close the game window.
        private static void ClosedEventHandler(object sender, EventArgs e)
        {
            // This indicates we should close the window, so just do that.
            _window.Close();
        }

        // Returns a reference to the game's RenderWindow.
        public static RenderWindow RenderWindow
        {
            get { return _window; }
        }

        // Get a texture (pixels) from a file
        public static Texture GetTexture(string fileName)
        {
            Texture texture;

            if (Textures.TryGetValue(fileName, out texture)) return texture;

            texture = new Texture(fileName);
            Textures[fileName] = texture;
            return texture;
        }

        // Get a sound from a file
        public static SoundBuffer GetSoundBuffer(string fileName)
        {
            SoundBuffer soundBuffer;

            if (Sounds.TryGetValue(fileName, out soundBuffer)) return soundBuffer;

            soundBuffer = new SoundBuffer(fileName);
            Sounds[fileName] = soundBuffer;
            return soundBuffer;
        }

        // Get a font from a file
        public static Font GetFont(string fileName)
        {
            Font font;

            if (Fonts.TryGetValue(fileName, out font)) return font;

            font = new Font(fileName);
            Fonts[fileName] = font;
            return font;
        }

        // Returns the active running scene.
        public static Scene CurrentScene
        {
            get { return _currentScene; }
        }

        // Specifies the next Scene to run.
        public static void SetScene(Scene scene)
        {
            // If we don't have a current scene, set it.
            // Otherwise, note the next scene.
            if (_currentScene == null)
                _currentScene = scene;
            else
                _nextScene = scene;
        }

        // Begins the main game loop with the initial scene.
        public static void Run()
        {
            Clock clock = new Clock();

            // Keep looping until the window closes.
            while (_window.IsOpen)
            {
                // If the next scene has been set, swap it with the current scene.
                if (_nextScene != null)
                {
                    _currentScene = _nextScene;
                    _nextScene = null;
                    clock.Restart();
                }

                // Get the time since the last frame, then have the scene update itself.
                Time time = clock.Restart();
                _currentScene.Update(time);
            }
        }
    }
}