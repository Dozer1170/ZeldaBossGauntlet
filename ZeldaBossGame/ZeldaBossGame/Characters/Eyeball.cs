using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class Eyeball : Character
    {
        Attack basicAttack;

        public Eyeball(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
            Initialize();
        }

        public override void Initialize()
        {
            sprite.layerDepth = 0.99f;

            moveSpeed = 1;

            health = 4;
            maxHealth = 4;

            invinciblityFramesAfterHit = 40;

            controller = new ChaseOnlyAIController(this);
            InitAttacks();

            SetBoundingShapes(basicAttack.hitBoxes);
        }

        public override void InitAnims()
        {
            //boss inits them as it is in its sprite sheet
        }

        public override void InitAttacks()
        {
            BoundingShapes shapes = new BoundingShapes(pos, new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0)));
            shapes.AddInnerBoundingSphere(new BoundingSphere(new Vector3(32, 32, 0), 14));
            basicAttack = new Attack(this, shapes, 1, 0, 0, 0, 0);
            DoAttack(basicAttack);
        }

        //Dont worry about hitmap collision if they are flying
        public override Color Move(Vector2 velocity)
        {
            ForceMove(velocity);

            return Color.Transparent;
        }

        // Dont take damage from another eyeball
        public override void TakeDamage(Attack attack, int damage)
        {
            if (!(attack.attackOwner is Eyeball))
            {
                if (invinciblityFrames < 1)
                    Game1.soundManager.PlayCue(SoundManager.EYEBALL_HIT);

                base.TakeDamage(attack, damage);
            }
        }

        public override void HandleDeath()
        {
            base.HandleDeath();
            Game1.soundManager.PlayCue(SoundManager.EYEBALL_DIE);
        }
    }
}
