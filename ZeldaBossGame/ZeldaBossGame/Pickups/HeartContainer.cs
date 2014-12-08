using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class HeartContainer : Pickup
    {

        public HeartContainer(Sprite sprite, Vector2 position)
            : base(sprite, position)
        {
            SetBoundingShapes(new BoundingShapes(position, new BoundingBox(new Vector3(0, 0, 0), new Vector3(36, 32, 0))));
            Initialize();
        }

        public void Initialize()
        {
            sprite.layerDepth = 1;
        }

        public override void DoPickupAction(Character character)
        {
            Character link = Game1.GetPlayerCharacter();
            if (character.Equals(link))
            {
                wasPickedUp = true;
                link.maxHealth += 2;
                link.health = link.maxHealth;
                Game1.soundManager.PlayCue(SoundManager.HEART_CONTAINER_CUE_NAME);
                Game1.pickupManager.ScheduleRemovePickup(this);
            }
        }
    }
}
