using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class FallingRockController : AIController
    {
        int fallSpeed;
        Vector2 target, minPos, maxPos;
        Random rand;

        public FallingRockController(Character character, Random rand)
            : base(character)
        {
            this.rand = rand;
            fallSpeed = 5;
            character.moveSpeed = fallSpeed;
            minPos = new Vector2(282,128);
            maxPos = new Vector2(1360,750);
            PickTargetPoint();
        }

        private void PickTargetPoint()
        {
            int xDiff = rand.Next(0, (int) (maxPos.X - minPos.X));
            int yDiff = rand.Next(0, (int) (maxPos.Y - minPos.Y));
            target = new Vector2(minPos.X + xDiff, minPos.Y + yDiff);
            //Move character to x pos of target so it falls straight down
            character.UpdatePosition(new Vector2(target.X - character.sprite.size.X/2, 0));
        }

        public override void Update(GameTime gameTime)
        {
            //If within 2 moves of target, do attack
            if (ForceMoveToward(target))
            {
                if(!character.attacking)
                    character.Attack();
            }
        }

        //Stop force move from trying to make rock face a direction
        public override void DetermineFacingDirection(Vector2 direction)
        {
            
        }
    }
}
