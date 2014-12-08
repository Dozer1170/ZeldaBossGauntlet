using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class VolvagiaArm : AnimatedCharacter
    {
        public static string ARM_ATTACK_ANIM_NAME = "attack";

        public Attack swipe;

        public VolvagiaArm(Sprite sprite, Vector2 worldPos) : base(sprite, worldPos)
        {
        }

        public override void Initialize()
        {
            sprite.layerDepth = 1;

            maxHealth = 6;
            health = 6;

            InitAnims();
            InitAttacks();
        }

        public override void InitAnims()
        {
            Point spriteSize = new Point(128, 256);

            SpriteAnimation still = new SpriteAnimation(new Point(768, 0), spriteSize, 1, 0, false, STAND_STILL_DOWN_ANIM_NAME);
            SpriteAnimation attack = new SpriteAnimation(new Point(0, 256), spriteSize, 4, 10, false, ARM_ATTACK_ANIM_NAME);
            SpriteAnimation deathExplosion = new SpriteAnimation(new Point(512, 256), new Point(128, 128), 4, 10, false, DEATH_ANIM_NAME);

            AddAnimation(still);
            AddAnimation(attack);
            AddAnimation(deathExplosion);

            PlayAnimation(STAND_STILL_DOWN_ANIM_NAME);
        }

        public override void InitAttacks()
        {
            BoundingShapes swipeShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(42,0,0), new Vector3(102,174,0)));
            swipe = new Attack(this, swipeShapes, 2, 3, 4, 4, animations.GetAnimation(ARM_ATTACK_ANIM_NAME).millisecondsPerFrame);
        }

        public override void Attack()
        {
            DoAttack(swipe);
            Game1.soundManager.PlayCue(SoundManager.VOLVAGIA_SWIPE);
            PlayAnimation(ARM_ATTACK_ANIM_NAME, delegate() { PlayAnimation(STAND_STILL_DOWN_ANIM_NAME); });
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if(invinciblityFrames < 1)
                Game1.soundManager.PlayCue(SoundManager.VOLVAGIA_HIT);
            base.TakeDamage(attack, damage);
        }
    }
}
