using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class HitmapProjectileAttack : ProjectileAttack
    {
        Action hitmapCallback;

        public HitmapProjectileAttack(Character attackOwner, BoundingShapes shapes,
            int damage, int startFrame, int endFrame, int totalFrames,
            int millisecondsPerFrame, Sprite sprite, Vector2 currPos, Vector2 velocity, Action callbackOnHitmapCollision)
            : base(attackOwner, shapes, damage, startFrame, endFrame, totalFrames, millisecondsPerFrame, sprite, currPos, velocity)
        {
            this.hitmapCallback = callbackOnHitmapCollision;
        }

        public override void Update(GameTime gameTime, Vector2 pos)
        {
            base.Update(gameTime, pos);
            Color hitmapColor = hitBoxes.CheckHitMapCollision(Game1.GetActiveBackground());
            if (hitmapColor == Color.Black)
                hitmapCallback();
        }
    }
}
