using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    /*
     * Class for keeping track of frames and frame transitions
     * Works best with sprite sheets, but can also work for texture arrays
     */ 
    public class SpriteAnimation
    {
        Action callbackFunction;
        public string name;
        private bool paused;
        private Point startingLocation;
        private Point spriteSize;
        public int numFrames;
        public int frameIndex;
        private int timeSinceLastFrame;
        public int millisecondsPerFrame;
        private bool loops;
        public Rectangle currFrame;

        public SpriteAnimation(Point startingLocation, Point spriteSize, int numFrames, int framesPerSecond, bool loops, string name)
        {
            this.startingLocation = startingLocation;
            this.spriteSize = spriteSize;
            this.numFrames = numFrames;
            this.frameIndex = 0;
            this.loops = loops;
            this.name = name;
            this.timeSinceLastFrame = 0;
            if (framesPerSecond != 0)
            {
                this.millisecondsPerFrame = 1000 / framesPerSecond;
            }
            else
            {
                this.millisecondsPerFrame = 1000;
                paused = true;
            }
            Restart();
        }

        //If enough time has elapsed move to the next frame
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                if(!paused)
                    NextFrame();
            }
        }

        //Move to the next frame
        public void NextFrame() 
        {
            //Increase curr frame and if less than our total number of frames
            if (++frameIndex < numFrames)
            {
                //move the curr frame right on the sprite sheet
                currFrame = new Rectangle(currFrame.Left + spriteSize.X, currFrame.Top, spriteSize.X, spriteSize.Y);
            }
            else //we are past the total # of frames
            {
                //If set to loop
                if (loops)
                {
                    Restart();
                }
                else //stay on the last frame!!
                {
                    Stop();
                    frameIndex = numFrames - 1;
                }
            }
        }

        //Restart animation
        public void Restart()
        {
            paused = false;
            frameIndex = 0;
            currFrame = new Rectangle(startingLocation.X, startingLocation.Y, spriteSize.X, spriteSize.Y);
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public void SetCallback(Action callback)
        {
            this.callbackFunction = callback;
        }

        public void Stop()
        {
            if (!paused)
            {
                if(callbackFunction != null)
                    callbackFunction();
            }
            paused = true;
        }

        public void MoveCurrentFrameTo(Point position)
        {
            currFrame = new Rectangle(position.X, position.Y, spriteSize.X, spriteSize.Y);
        }
    }
}
