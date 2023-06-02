using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace GameEngine
{
    // A sprite that can have multiple animations and play them with different AnimationModes.
    class AnimatedSprite : GameObject
    {
        // Specifies how to play an animation.
        public enum AnimationMode
        {
            LoopForwards,
            LoopBackwards,
            OnceForwards,
            OnceBackwards,
            FirstFrameOnly
        }

        // Specifies where the "orgin" of the sprite is. This determines where on the screen
        // we'll draw the sprite relative to its position. For example, if the origin is
        // TopLeft, all the sprite's pixels will be placed to the right of and below its
        // current position:
        //
        //  | Sprite's position
        // \./
        //  --------
        //  |sprite|  <-- With TopLeft, the sprite is drawn below and to the right of its position
        //  --------
        //
        // The default value used by AnimatedSprite is Center.
        public enum OriginMode
        {
            TopLeft,
            TopMiddle,
            TopRight,
            MiddleLeft,
            Center,
            MiddleRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        }

        // This controls how fast animations are by default.
        // It specifies how long each frame is displayed on the screen.
        private const int DefaultMsPerFrame = 30;

        // The sprite we draw to the screen.
        private readonly Sprite _sprite = new Sprite();

        // This map associates the name of the animation (a string) with
        // all the frames for that animation (a vector of IntRect).
        // The vector is sort of like an array that can resize itself,
        // and the IntRect is simply a rectangle that specifies where
        // in the texture that frame of animation is located.
        private readonly Dictionary<string, List<IntRect>> _animations = new Dictionary<string, List<IntRect>>();

        // The name of the animation being played.
        private string _currentAnimation;

        // The number of the current frame in the sequence of frames for
        // the current animation.
        private int _currentFrameNum;

        // The mode the current animation is being played in.
        private AnimationMode _currentMode;

        // True if we are playing the animation (changing frames over time).
        private bool _isPlaying;

        // How many milliseconds each frame is shown for.
        private readonly int _msPerFrame;

        // How many milliseconds the current frame has been displayed.
        private int _msSinceLastFrame;

        // How many frames are in the animation.
        private int _numFrames;

        // The origin mode, which determines how we draw the sprite on the screen.
        private OriginMode _originMode = OriginMode.Center;

        // Constructs the sprite with a specified location and frame rate.
        // If you don't provide a frame rate, the default frame rate is used.
        public AnimatedSprite(Vector2f position, int msPerFrame = DefaultMsPerFrame)
        {
            _msPerFrame = msPerFrame;
            Position = position;
        }

        // Sets the spritesheet texture we will use for animation frames.
        public Texture Texture
        {
            get { return _sprite.Texture;  }
            set { _sprite.Texture = value; }
        }

        // Sets the position of the sprite.
        public Vector2f Position
        {
            get { return _sprite.Position; }
            set { _sprite.Position = value; }
        }

        // Set the origin mode, which specifies how we draw the sprite relative to its position.
        public void SetOriginMode(OriginMode originMode)
        {
            _originMode = originMode;
        }

        // Adds an animation, which is a sequence of frames associated with a name.
        public void AddAnimation(string name, List<IntRect> frames)
        {
            _animations[name] = frames;
        }

        // Plays an animation by name, with a given AnimationMode.
        public void PlayAnimation(string name, AnimationMode mode)
        {
            _currentAnimation = name;
            _currentMode = mode;
            _isPlaying = true;

            // Reset the "timer" so we display the frame the right amount of time.
            _msSinceLastFrame = 0;

            DetermineNumFrames();
            DetermineFirstFrameNum();
            SetTextureRectForFrame();
        }

        // Returns true if the sprite is currently playing.
        public bool IsPlaying()
        {
            return _isPlaying;
        }

        // Functions overridden from GameObject:
        public override void Draw()
        {
            // Don't draw if we're not drawable.
            if (!IsDrawable()) return;

            // Draw it.
            Game.RenderWindow.Draw(_sprite);
        }

        public override void Update(Time elapsed)
        {
            // We don't need to update if we're not drawable.
            if (!IsDrawable()) return;

            // Update our "timer", and change the frame if we've waited long enough.
            _msSinceLastFrame += elapsed.AsMilliseconds();

            if (_msSinceLastFrame >= _msPerFrame)
            {
                DetermineNextFrameNum();
                SetTextureRectForFrame();
            }
        }

        public override FloatRect GetCollisionRect()
        {
            // Just use the boundaries of the sprite.
            return _sprite.GetGlobalBounds();
        }

        // Functions to determine which frame to display.
        private void DetermineNumFrames()
        {
            _numFrames = _animations[_currentAnimation].Count;
        }

        private void DetermineFirstFrameNum()
        {
            switch (_currentMode)
            {
                case AnimationMode.LoopForwards:
                case AnimationMode.OnceForwards:
                case AnimationMode.FirstFrameOnly:
                    // Forwards starts from frame 0.
                    _currentFrameNum = 0;
                    break;
                case AnimationMode.LoopBackwards:
                case AnimationMode.OnceBackwards:
                    // Backwards starts from the last frame.
                    _currentFrameNum = _numFrames - 1;
                    break;
            }
        }

        private void DetermineNextFrameNum()
        {
            // We don't need to determine the next frame if the animation is not playing.
            if (!_isPlaying) return;

            // We don't need to adjust the frame for FirstFrameOnly mode.
            if (_currentMode == AnimationMode.FirstFrameOnly) return;

            // Divide how much time has elapsed by the number of milliseconds per frame.
            // This tells us how many frames we should advance.
            int elapsedFrames = _msSinceLastFrame / _msPerFrame;
            _msSinceLastFrame = 0;

            // Add or subtract the elapsed frames based on whether we are animating
            // forwards or backwards.
            switch (_currentMode)
            {
                case AnimationMode.LoopForwards:
                case AnimationMode.OnceForwards:
                    _currentFrameNum += elapsedFrames;
                    break;
                case AnimationMode.LoopBackwards:
                case AnimationMode.OnceBackwards:
                    _currentFrameNum -= elapsedFrames;
                    break;
            }

            // If we've gone past the end of the animation, determine where we should be.
            if (_currentFrameNum >= _numFrames || _currentFrameNum < 0)
                switch (_currentMode)
                {
                    case AnimationMode.LoopForwards:
                    case AnimationMode.LoopBackwards:
                        // Use modulo to make sure we wrap to the correct frame. For example,
                        // if numFrames is 6, but currentFrameNum is 8, we should wrap around to frame 2, or
                        // if numFrames is 6, but currentFrameNum is -2, we should wrap around to frame 4.
                        _currentFrameNum %= _numFrames;

                        // This compensates for the fact that in C++ -1 % 15 may equal -1,
                        // whereas in math it would be 14.
                        if (_currentFrameNum < 0) _currentFrameNum = _numFrames + _currentFrameNum;
                        break;
                    case AnimationMode.OnceForwards:
                        // We never go past the last frame
                        _currentFrameNum = _numFrames - 1;
                        _isPlaying = false;
                        break;
                    case AnimationMode.OnceBackwards:
                        // We never go past the first frame.
                        _currentFrameNum = 0;
                        _isPlaying = false;
                        break;
                }
        }

        // Sets the rectangle in our texture that contains pixels for the
        // current frame of the current animation.
        private void SetTextureRectForFrame()
        {
            // Gets the rectangle information from the animation's list of rects.
            IntRect rect = _animations[_currentAnimation][_currentFrameNum];

            // Tell the sprite to use the new frame of pixel data.
            _sprite.TextureRect = rect;

            // set the origin based on the Origin mode
            float originX = 0.0f;
            float originY = 0.0f;

            switch (_originMode)
            {
                case OriginMode.TopMiddle:
                case OriginMode.Center:
                case OriginMode.BottomMiddle:
                    originX = rect.Width / 2.0f;
                    break;
                case OriginMode.TopRight:
                case OriginMode.MiddleRight:
                case OriginMode.BottomRight:
                    originX = rect.Width;
                    break;
            }

            switch (_originMode)
            {
                case OriginMode.MiddleLeft:
                case OriginMode.Center:
                case OriginMode.MiddleRight:
                    originY = rect.Width / 2.0f;
                    break;
                case OriginMode.BottomLeft:
                case OriginMode.BottomMiddle:
                case OriginMode.BottomRight:
                    originY = rect.Width;
                    break;
            }

            _sprite.Origin = new Vector2f(originX, originY);
        }

        // True if we should draw the sprite to the screen. This will be
        // false if we don't have animation frame information.
        private bool IsDrawable()
        {
            // If no animations have been set, or if the current animation contains
            // no frames, then the sprite is not drawable.
            return _animations.Count > 0 && _currentAnimation != null;
        }
    }
}