using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    class Leever : AnimatedCharacter
    {
        public static string DEFAULT_ANIM_NAME = "default";
        Attack basicAttack;

        public Leever(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
            Initialize();
        }

        public override void Initialize()
        {
            moveSpeed = 2;

            health = 4;
            maxHealth = 4;

            invinciblityFramesAfterHit = 40;

            controller = new ChaseOnlyAIController(this);
            InitAnims();
            InitAttacks();

            SetBoundingShapes(basicAttack.hitBoxes);
        }

        public override void InitAnims()
        {
            SpriteAnimation defaultAnim = new SpriteAnimation(new Point(0, 0), new Point(40, 60), 2, 5, true, DEFAULT_ANIM_NAME);
            SpriteAnimation death = new SpriteAnimation(new Point(0, 0), new Point(40, 60), 2, 5, false, DEATH_ANIM_NAME);

            AddAnimation(defaultAnim);
            AddAnimation(death);

            PlayAnimation(DEFAULT_ANIM_NAME);
        }

        public override void InitAttacks()
        {
            BoundingShapes shapes = new BoundingShapes(pos, new BoundingBox(new Vector3(4, 12, 0), new Vector3(36, 46, 0)));
            basicAttack = new Attack(this, shapes, 1, 0, 0, 0, 0);
            DoAttack(basicAttack);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 distToPlayer = Game1.GetPlayerCharacter().pos - pos;
            if (Math.Abs(distToPlayer.X) < 500 && Math.Abs(distToPlayer.Y) < 500)
            {
                base.Update(gameTime);
            }
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            //Dont take damage from another leever
            if (!(attack.attackOwner is Leever))
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
