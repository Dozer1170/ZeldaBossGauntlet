using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class HeartContainer : Character
    {

        public HeartContainer(Sprite sprite, Vector2 position)
            : base(sprite, position)
        {
            SetBoundingShapes(new BoundingShapes(position, new BoundingBox(new Vector3(0, 0, 0), new Vector3(36, 32, 0))));
            sprite.layerDepth = 1;
        }

        public override void HandleCollision(bool atFault, Vector2 prevPosition)
        {
            Character link = Game1.GetPlayerCharacter();
            link.maxHealth += 2;
            link.health = link.maxHealth;
            Game1.soundManager.PlayCue(SoundManager.HEART_CONTAINER_CUE_NAME);
            Game1.characterManager.ScheduleRemoveCharacter(this);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void StandStill()
        {
            throw new NotImplementedException();
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void Attack(Attack attack)
        {
            throw new NotImplementedException();
        }
    }
}
