using System;
using System.Collections.Generic;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using GameEngine;
using SFML.Audio;
using SFML.Window;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace GameEngine
{
    // The Scene manages all the GameObjects currently in the game.
    class Scene
    {
        // These lists hold various game objects 
        private readonly List<GameObject> _gameObjects = new List<GameObject>();

        // Puts a GameObject into the scene.
        public void AddGameObject(GameObject gameObject)
        {
            // This adds the game object onto the back (the end) of the list of game objects.
            _gameObjects.Add(gameObject);
        }
        public void AddGameObject(int position, GameObject gameObject)
        {
            //This adds the game object at a location into the list of game objects. 
            _gameObjects.Insert(position, gameObject);
        }

        // Called by the Game instance once per frame.
        public void Update(Time time)
        {
            // Clear the window.
            Game.RenderWindow.Clear();
            // Go through our normal sequence of game loop stuff.
            //doesnt't draw game objects as the mesh's draw themselves when updated.
            // Handle any keyboard, mouse events, etc. for our game window.
            Game.RenderWindow.DispatchEvents();
            HandleCollisions();
            UpdateGameObjects(time);
            RemoveDeadGameObjects();
            // Draw the window as updated by the game objects.
            Game.RenderWindow.Display();
        }
        // This method lets game objects respond to collisions.
        private void HandleCollisions()
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
                for (int j = 0; j < _gameObjects.Count; j++)
                {
                    var otherGameObject = _gameObjects[j];

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

        private void UpdateGameObjects(Time time)
        {//updates each game object in _gameobjects
            for (int i = 0; i < _gameObjects.Count; i++) { _gameObjects[i].Update(time); }
        }

        // This has a parameter for 3 points, drawing a line between 1 & 2, 2 & 3, 3 & 1, to draw a triangle between the three points
        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3) 
        {
            //Set Up the 3 points
            Vertex vertex1 = new Vertex();
            //creates first vertex

            vertex1.Position = new Vector2f(x1, y1);
            //first vertex position is at point 1

            vertex1.Color = Color.Red;
            //the color of the vertex is red
            
            Vertex vertex2 = new Vertex();
            //second vertex
            
            vertex2.Position = new Vector2f(x2, y2);
            //vertex is set to the second point
            
            vertex2.Color = Color.Red;
            //vertex's color is red
            
            Vertex vertex3 = new Vertex();
            //3rd vertex
            
            vertex3.Position = new Vector2f(x3, y3);
            //vertex position is set to the 3rd point
            
            vertex3.Color = Color.Red;
            //vertex color is red
            
            //draw line between x1 y1 x2 y2
            VertexArray lines1 = new VertexArray((PrimitiveType)2, 2);//creates a vertex array with two vertices, type line.
            lines1[0] = vertex1;
            lines1[1] = vertex2;
            Game.RenderWindow.Draw(lines1);

            //draw line between x2 y2 x3 y3
            VertexArray lines2 = new VertexArray((PrimitiveType)2, 2);
            lines2[0] = vertex2;
            lines2[1] = vertex3;
            Game.RenderWindow.Draw(lines2);

            //draw line between x3 y3 x1 y1
            VertexArray lines3 = new VertexArray((PrimitiveType)2, 2);
            lines3[0] = vertex3;
            lines3[1] = vertex1;
            Game.RenderWindow.Draw(lines3);
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
            _gameObjects.RemoveAll(isDead);
        }
    }
}