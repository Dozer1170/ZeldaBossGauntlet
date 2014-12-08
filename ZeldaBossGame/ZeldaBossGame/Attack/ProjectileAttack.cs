using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class ProjectileAttack : Attack
    {
        public Sprite sprite;
        public Vector2 currPos;
        public Vector2 velocity;

        public ProjectileAttack(Character attackOwner, BoundingShapes shapes,
            int damage, int startFrame, int endFrame, int totalFrames,
            int millisecondsPerFrame, Sprite sprite, Vector2 currPos, Vector2 velocity) 
            : base(attackOwner, shapes, damage, startFrame, endFrame, totalFrames, millisecondsPerFrame)
        {
            this.sprite = sprite;
            this.currPos = currPos;
            this.velocity = velocity;

            sprite.layerDepth = 0.99f;
        }

        public override void Update(GameTime gameTime, Vector2 pos)
        {
            currPos += velocity;
            UpdatePosition(currPos);
            sprite.currAnim.Update(gameTime);
            base.Update(gameTime, currPos);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public virtual void UpdatePosition(Vector2 nPos)
        {
            currPos = nPos;
            sprite.pos = currPos - Game1.GetActiveBackground().topLeft;
        }

        public bool BeyondBackgroundRange()
        {
            Vector2 size = Game1.GetActiveBackground().size;
            if (currPos.X > size.X || currPos.Y > size.Y  || currPos.X < -sprite.size.X || currPos.Y < -sprite.size.Y)
                return true;
            else
                return false;
        }

        public override void BeginAttack()
        {
            base.BeginAttack();
            sprite.currAnim.Restart();
        }
    }
}
