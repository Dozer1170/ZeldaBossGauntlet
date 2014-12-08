using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class Attack
    {
        public Character attackOwner;
        public BoundingShapes hitBoxes;
        public int damage;
        public int currFrame, startFrame, endFrame, totalFrames;
        int millisecondsPerFrame, timeSinceLastFrame;
        bool alwaysActive;
        public Action callbackOnHit;

        public Attack(Character attackOwner, BoundingShapes shapes, 
            int damage, int startFrame, int endFrame, int totalFrames,
            int millisecondsPerFrame)
        {
            this.attackOwner = attackOwner;
            this.hitBoxes = shapes;
            this.damage = damage;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.totalFrames = totalFrames;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.timeSinceLastFrame = 0;

            if (totalFrames == 0 || totalFrames == -1)
                alwaysActive = true;
            else
                alwaysActive = false;
        }

        public virtual void Update(GameTime gameTime, Vector2 pos)
        {
            if (currFrame < totalFrames || alwaysActive)
            {
                hitBoxes.UpdatePosition(pos);
                //If in active frames check for hits
                if (IsInActiveFrames())
                {
                    Character character = Game1.characterManager.CheckHitBoxCollision(attackOwner, hitBoxes);
                    if (character != null)
                    {
                        //Had a hit
                        HandleHit(character);
                    }
                }
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    currFrame++;
                }
            }
            else
            {
                attackOwner.ClearActiveAttack();
            }
        }

        public bool IsInActiveFrames()
        {
            return alwaysActive || (currFrame >= startFrame && currFrame <= endFrame);
        }

        public virtual void HandleHit(Character characterHit)
        {
            characterHit.TakeDamage(this, damage);

            if (callbackOnHit != null)
                callbackOnHit();
        }

        public virtual void BeginAttack()
        {
            currFrame = 0;
        }
    }
}
