using System;
using System.Collections.Generic;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace GameEngine
{
    // The Scene manages all the GameObjects currently in the game.
    class Scene
    {
        // These lists hold various game objects -AP
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        //_gameObjects renders for the differing y values giving us true isometric 3d (2.5d) -AP
        private readonly List<GameObject> _tiles = new List<GameObject>();
        //_tiles gameobject list allows for us to alway render the tiles inbetween the character and the background -AP
        private readonly List<GameObject> _background = new List<GameObject>();
        //_background allows for a certain game objects that are in the background to be rendered first -AP
        private readonly List<GameObject> _cloud = new List<GameObject>();
        //_cloud gives game objects in between the main game object and the UI-AP
        private readonly List<GameObject> _userinterface = new List<GameObject>();
        //User interface allows to render certain game objects to render last to look part of a cohesive UI-AP
        // Puts a GameObject into the scene.
        public void AddGameObject(GameObject gameObject)
        {
            // This adds the game object onto the back (the end) of the list of game objects.-AP
            _gameObjects.Add(gameObject);
        }
        public void AddGameObject(int position, GameObject gameObject)
        {
            //This adds the game object at a location into the list of game objects. -AP
            _gameObjects.Insert(position, gameObject);
        }

        public void AddTile(GameObject tile)
        {//Adds gameobjects that need to be in between the background and the main gameobjects-AP
            _tiles.Add(tile);
        }

        public void AddBackground(GameObject background)
        {//allows to add background game objects to render first-AP
            _background.Add(background);
        }

        public void AddUserInterface(GameObject UI)
        {//adds game objects to the back of a ui to render them last-AP
            _userinterface.Add(UI);
        }

        public void AddCloud(GameObject cloud)
        {//adds game objects to teh back of the cloud list to render in between the UI and main gameobjects -AP
            _cloud.Add(cloud);
        }
        public int GameObjectAmount()
        {//allows game objects to access the amount of game objects if needed -AP
            return _gameObjects.Count;
        }

        // Called by the Game instance once per frame.
        public void Update(Time time)
        {
            // Clear the window.
            Game.RenderWindow.Clear();

            // Go through our normal sequence of game loop stuff.

            // Handle any keyboard, mouse events, etc. for our game window.
            Game.RenderWindow.DispatchEvents();

            HandleCollisions();
            UpdateGameObjects(time);
            RemoveDeadGameObjects();
            DrawBackground();
            DrawTiles();
            DrawGameObjects();//draw background, then tiles, objects (top-bottom rendering), then the clouds ending with the ui. -AP
            DrawClouds();
            DrawUserInterface();
            // Draw the window as updated by the game objects.
            Game.RenderWindow.Display();
        }

        // This method lets game objects respond to collisions.
        private void HandleCollisions()//handle collisions is only for _gameObjects and _cloud-AP
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                var gameObject = _gameObjects[i];

                // Only check objects that ask to be checked.
                if (!gameObject.IsCollisionCheckEnabled()) continue;

                FloatRect collisionRect = gameObject.GetCollisionRect();

                // Don't bother checking if this game object has a collision rectangle with no area.
                if (collisionRect.Height == 0 || collisionRect.Width == 0) continue;

                // See if this game object is colliding with any other game object.
                for (int j = 0; j < _cloud.Count; j++)
                {
                    var otherGameObject = _cloud[j];
                    //Spell for this game is part of the clouds as it needs to be below UI and above game objects -AP
                    //Only checks for collisions between _gameObjects and _cloud Lists - AP

                    // Don't check an object colliding with itself.
                    if (gameObject == otherGameObject) continue;

                    if (gameObject.IsDead()) return;

                    // When we find a collision, invoke the collision handler for both objects.
                    if (collisionRect.Intersects(otherGameObject.GetCollisionRect()))
                    {
                        gameObject.HandleCollision(otherGameObject);
                        otherGameObject.HandleCollision(gameObject);
                    }
                }
            }
        }

        // This function calls update on each of our game objects for most lists that need to be updated (currently all of them) -AP
        private void UpdateGameObjects(Time time)
        {
            for (int i = 0; i < _gameObjects.Count; i++) { _gameObjects[i].Update(time); }
            for (int i = 0; i < _cloud.Count; i++) {_cloud[i].Update(time); }
            for (int i = 0; i <_userinterface.Count; i++) {_userinterface[i].Update(time); }
            for (int i = 0; i < _background.Count; i++) _background[i].Update(time);
            for (int i = 0; i < _tiles.Count; i++) _tiles[i].Update(time);
        }

        // This function calls draw on each of our game objects goes through each y value and goes from the top to the bottom  rendering objects as they apear. -AP
        private void DrawGameObjects()
        {
            for (int y = 0; y<Game.RenderWindow.Size.Y; y++)
            {
                foreach (var GameObject in _gameObjects)
                {
                    if (GameObject.GetPosition().Y==y)
                    {
                        GameObject.Draw();
                    }
                }
            }
        }
        //Draws all of the other game object lists from the beggining to the end. -AP
        private void DrawBackground()
        { 
            foreach (var gameobject in _background) gameobject.Draw();
        }
        private void DrawUserInterface()
        {
            foreach (var gameobject in _userinterface) gameobject.Draw();
        }
        private void DrawClouds()
        {
            foreach (var gameobject in _cloud) gameobject.Draw();
        }
        private void DrawTiles()
        {
            foreach (var gameobject in _tiles) gameobject.Draw();
        }

        // This function removes objects that indicate they are dead from the scene.
        private void RemoveDeadGameObjects()
        {
            // This is a "lambda", which is a fancy name for an anonymous function.
            // It's "anonymous" because it doesn't have a name. We've declared a variable
            // named "isDead", and that variable can be used to call the function, but the
            // function itself is nameless.
            Predicate<GameObject> isDead = gameObject => gameObject.IsDead();

            // Here we use the lambda declared above by passing it to the standard RemoveAll
            // method on List<T>, which calls our lambda once for each element in
            // gameObjects. If our lambda returns true, that game object ends up being
            // removed from our list.
            _gameObjects.RemoveAll(isDead);//Removes dead game objects from most lists that get made dead during the game. -AP
            _cloud.RemoveAll(isDead);
            _tiles.RemoveAll(isDead);
            _userinterface.RemoveAll(isDead);
        }
    }
}