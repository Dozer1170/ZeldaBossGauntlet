using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class Attack
    {
        Character attackOwner;
        BoundingShapes hitBoxes;
        int damage;
        int currFrame, startFrame, endFrame, totalFrames;
        int millisecondsPerFrame, timeSinceLastFrame;

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
        }

        public void Update(GameTime gameTime, Vector2 pos)
        {
            if (currFrame < totalFrames)
            {
                hitBoxes.UpdatePosition(pos);
                //If in active frames check for hits
                if (currFrame >= startFrame && currFrame <= endFrame)
                {
                    Character character = Game1.characterManager.CheckHitBoxCollision(attackOwner, hitBoxes);
                    if (character != null)
                    {
                        //Had a hit
                        character.TakeDamage(damage);
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

        public void BeginAttack()
        {
            currFrame = 0;
        }
    }
}
