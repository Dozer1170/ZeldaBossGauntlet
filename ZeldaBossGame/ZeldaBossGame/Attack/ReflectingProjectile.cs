using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class ReflectingProjectile : ProjectileAttack
    {
        bool hasHitCharacter;
        Vector2 reflectingSpeedMult;

        public ReflectingProjectile(Character attackOwner, BoundingShapes shapes,
            int damage, int startFrame, int endFrame, int totalFrames,
            int millisecondsPerFrame, Sprite sprite, Vector2 currPos, Vector2 velocity, Vector2 reflectingSpeedMult) 
            : base(attackOwner, shapes, damage, startFrame, endFrame, totalFrames, millisecondsPerFrame,
              sprite, currPos, velocity)
        {
            this.reflectingSpeedMult = reflectingSpeedMult;
            hasHitCharacter = false;
        }

        public override void Update(GameTime gameTime, Vector2 pos)
        {
            if (!hasHitCharacter)
            {
                base.Update(gameTime, pos);
                Attack reflectingAttack = GetAttackInActiveHitBox();
                if (reflectingAttack != null)
                {
                    Reflect(reflectingAttack);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!hasHitCharacter)
                base.Draw(spriteBatch);
        }

        //Checks if any active attacks are in this attack's hitbox and returns if there is
        public Attack GetAttackInActiveHitBox()
        {
            foreach (Character character in Game1.characterManager.GetCharacterList())
            {
                Attack attack = character.activeAttack;
                if (attack != null && !this.Equals(attack) && !attack.attackOwner.Equals(attackOwner))
                {
                    //If attack is in active frames and if its hitboxes collide with this's hitboxes
                    if (IsInActiveFrames() && attack.hitBoxes.CheckCollision(hitBoxes))
                    {
                        return attack;
                    }
                }
            }

            return null;
        }

        public void Reflect(Attack reflectingAttack)
        {
            Game1.soundManager.PlayCue(SoundManager.FIREBALL_REFLECT);
            attackOwner = reflectingAttack.attackOwner;
            velocity = velocity * -reflectingSpeedMult;
        }

        public override void HandleHit(Character characterHit)
        {
            base.HandleHit(characterHit);
            Game1.soundManager.PlayCue(SoundManager.FIREBALL_HIT);
            hasHitCharacter = true;
        }

        public override void BeginAttack()
        {
            base.BeginAttack();
            hasHitCharacter = false;
        }
    }
}
