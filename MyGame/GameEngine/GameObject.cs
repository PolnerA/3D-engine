using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace GameEngine
{
    // This class represents every object in your game, such as the player, enemies, and so on.
    abstract class GameObject
    {
        private bool _isCollisionCheckEnabled;

        private bool _isDead;
        //position to store the game objects various positions for rendering -AP
        private Vector2f Position;

        // Using a set prevents duplicates.
        private readonly HashSet<string> _tags = new HashSet<string>();

        // Tags let you annotate your objects so you can identify them later
        // (such as "player").
        public void AssignTag(string tag)
        {
            _tags.Add(tag);
        }
        // SetPosition allows for the GameObject to see the sprite positions (useful for rendering and other things) - AP
        public void SetPosition(Vector2f position) { Position = position; }
        public Vector2f GetPosition()//allows the scene to get the game object positions for the render window -AP
        {
            return Position;
        }
        public bool HasTag(string tag)
        {
            return _tags.Contains(tag);
        }

        // "Dead" game objects will be removed from the scene.
        public bool IsDead()
        {
            return _isDead;
        }

        public void MakeDead()
        {
            _isDead = true;
        }

        // Update is called every frame. Use this to prepare to draw (move, perform AI, etc.).
        public abstract void Update(Time elapsed);

        // Draw is called once per frame. Use this to draw your object to the screen.
        public virtual void Draw()
        {
        }

        // This flag indicates whether this game object should be checked for collisions.
        // The more game objects in the scene that need to be checked, the longer it takes.
        public bool IsCollisionCheckEnabled()
        {
            return _isCollisionCheckEnabled;
        }

        public void SetCollisionCheckEnabled(bool isCollisionCheckEnabled)
        {
            _isCollisionCheckEnabled = isCollisionCheckEnabled;
        }

        // This function lets you specify a rectangle for collision checks.
        public virtual FloatRect GetCollisionRect()
        {
            return new FloatRect();
        }

        // Use this to specify what happens when this object collides with another object.
        public virtual void HandleCollision(GameObject otherGameObject)
        {
        }
    }
}