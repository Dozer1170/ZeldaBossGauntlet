using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZeldaBossGame
{
    public class Vitreous : BossCharacter
    {
        public static string FLOAT_LOOK_LEFT = "lookleft";
        public static string FLOAT_LOOK_STRAIGHT = "lookstraight";
        public static string FLOAT_LOOK_RIGHT = "lookright";
        public static string SPIN_ATTACK_ANIM_NAME = "spin";
        public static string LIGHTNING_ANIM_NAME = "lightning";
        public static string FIREBALL_ANIM_NAME = "fireball";

        public static string GREEN_EYEBALL_FRAME = "green";
        public static string PURPLE_EYEBALL_FRAME = "purple";
        public static string RED_EYEBALL_FRAME = "red";
        public static string BLUE_EYEBALL_FRAME = "blue";

        private Cue fireballFlyingCue;

        public static ProjectileAttack SPIN_ATTACK;
        public static ReflectingProjectile FIREBALL;
        public float fireballMoveSpeed;
        public bool fireballActive;

        public Vitreous(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
        }

        public override void Initialize()
        {
            heartPieceSpawnPosition = new Vector2(470, 520);

            sprite.layerDepth = 0.95f;

            despawnOnDeath = true;

            maxHealth = 16;
            health = 16;

            moveSpeed = 0.10f;
            fireballMoveSpeed = 5;

            InitAnims();
            InitAttacks();

            SetBoundingShapes(new BoundingShapes(pos, new BoundingBox(new Vector3(30, 20, 0), new Vector3(92, 85, 0))));
            boundingShapes.AddInnerBoundingSphere(new BoundingSphere(new Vector3(64, 55, 0), 30));
        }

        public override void InitAnims()
        {
            Point spriteSize = new Point((int) sprite.size.X, (int) sprite.size.Y);

            SpriteAnimation floatLookLeft = new SpriteAnimation(new Point(0,0) , spriteSize, 1, 0, false, FLOAT_LOOK_LEFT);
            SpriteAnimation floatLookStraight = new SpriteAnimation(new Point(128, 0), spriteSize, 1, 0, false, FLOAT_LOOK_STRAIGHT);
            SpriteAnimation floatLookRight = new SpriteAnimation(new Point(256, 0), spriteSize, 1, 0, false, FLOAT_LOOK_RIGHT);
            SpriteAnimation spinAttack = new SpriteAnimation(new Point(0, 256), spriteSize, 4, 30, true, SPIN_ATTACK_ANIM_NAME);
            SpriteAnimation lightningAnimation = new SpriteAnimation(new Point(0, 384), spriteSize, 4, 30, true, LIGHTNING_ANIM_NAME);
            SpriteAnimation deathAnimation = new SpriteAnimation(new Point(0, 256), spriteSize, 4, 10, false, DEATH_ANIM_NAME);
            SpriteAnimation fireballAnimation = new SpriteAnimation(new Point(256, 128), spriteSize, 2, 40, true, FIREBALL_ANIM_NAME);

            Point eyeballSize = new Point(64,64);

            SpriteAnimation greenEyeball = new SpriteAnimation(new Point(0, 128), eyeballSize, 1, 0, false, GREEN_EYEBALL_FRAME);
            SpriteAnimation purpleEyeball = new SpriteAnimation(new Point(64, 128), eyeballSize, 1, 0, false, PURPLE_EYEBALL_FRAME);
            SpriteAnimation redEyeball = new SpriteAnimation(new Point(128, 128), eyeballSize, 1, 0, false, RED_EYEBALL_FRAME);
            SpriteAnimation blueEyeball = new SpriteAnimation(new Point(192, 128), eyeballSize, 1, 0, false, BLUE_EYEBALL_FRAME);

            AddAnimation(floatLookLeft);
            AddAnimation(floatLookStraight);
            AddAnimation(floatLookRight);
            AddAnimation(spinAttack);
            AddAnimation(lightningAnimation);
            AddAnimation(deathAnimation);
            AddAnimation(fireballAnimation);
            AddAnimation(greenEyeball);
            AddAnimation(purpleEyeball);
            AddAnimation(redEyeball);
            AddAnimation(blueEyeball);
        }

        public override void InitAttacks()
        {
            BoundingShapes spinAttackShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(25, 15, 0), new Vector3(87, 80, 0)));
            SPIN_ATTACK = new ProjectileAttack(this, spinAttackShapes, 1, 0, 0, 0, 0, 
                new Sprite(sprite.texture, new Vector2(0,0), sprite.size, animations.GetAnimation(LIGHTNING_ANIM_NAME)), pos, new Vector2(0,0));

            BoundingShapes fireballShapes = new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(19, 11, 0), new Vector3(99, 98, 0)));
            fireballShapes.AddInnerBoundingSphere(new BoundingSphere(new Vector3(64, 64, 0), 31));
            FIREBALL = new ReflectingProjectile(this, fireballShapes, 2, 0, 0, 0, 0, 
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(FIREBALL_ANIM_NAME)), pos,
                new Vector2(0, 0), new Vector2(1.3f, 1.3f));
            FIREBALL.callbackOnHit = delegate()
            {
                fireballActive = false;
                ClearActiveAttack();
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (fireballActive)
            {
                FIREBALL.Update(gameTime, pos);
                if (FIREBALL.BeyondBackgroundRange())
                {
                    fireballActive = false;
                    ClearActiveAttack();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (fireballActive)
                FIREBALL.Draw(spriteBatch);

            if (attacking)
            {
                SPIN_ATTACK.Draw(spriteBatch);
            }
        }

        public override Color Move(Vector2 velocity)
        {
            ForceMove(velocity);

            return Color.Transparent; //Since floating pretend like it didnt hit anything
        }

        public override void Attack()
        {
            DoAttack(SPIN_ATTACK);
            PlayAnimation(SPIN_ATTACK_ANIM_NAME);
        }

        public override void Attack(Attack attack)
        {
            if (attack.Equals(FIREBALL))
            {
                DoFireball();
            }
        }

        public override void ClearActiveAttack()
        {
            base.ClearActiveAttack();
            fireballFlyingCue.Stop(AudioStopOptions.Immediate);
            fireballFlyingCue = null;
        }

        public void DoFireball()
        {
            Vector2 diff = Game1.GetPlayerCharacter().pos - pos;
            FIREBALL.UpdatePosition(pos);
            FIREBALL.attackOwner = this;
            diff.Normalize();
            FIREBALL.velocity = diff * fireballMoveSpeed;
            FIREBALL.BeginAttack();
            fireballActive = true;
            if (fireballFlyingCue == null)
            {
                fireballFlyingCue = Game1.soundManager.soundBank.GetCue(SoundManager.FIREBALL_FLYING);
                fireballFlyingCue.Play();
            }
        }

        public override void FaceDown()
        {
            PlayAnimation(FLOAT_LOOK_STRAIGHT);
        }

        public override void FaceLeft()
        {
            PlayAnimation(FLOAT_LOOK_LEFT);
        }

        public override void FaceRight()
        {
            PlayAnimation(FLOAT_LOOK_RIGHT);
        }

        public override void FaceUp()
        {
            PlayAnimation(FLOAT_LOOK_STRAIGHT);
        }

        public void SpawnEyeballs() 
        {
            Vector2 eyeballSize = new Vector2(64, 64);

            Eyeball green = new Eyeball(new Sprite(sprite.texture, new Vector2(0, 0),
                eyeballSize, animations.GetAnimation(GREEN_EYEBALL_FRAME)), new Vector2(225, 255));
            Eyeball blue = new Eyeball(new Sprite(sprite.texture, new Vector2(0, 0),
                eyeballSize, animations.GetAnimation(BLUE_EYEBALL_FRAME)), new Vector2(690, 255));
            Eyeball purple = new Eyeball(new Sprite(sprite.texture, new Vector2(0, 0),
                eyeballSize, animations.GetAnimation(PURPLE_EYEBALL_FRAME)), new Vector2(225, 537));
            Eyeball red = new Eyeball(new Sprite(sprite.texture, new Vector2(0, 0),
                eyeballSize, animations.GetAnimation(RED_EYEBALL_FRAME)), new Vector2(690, 537));

            Game1.characterManager.ScheduleAddCharacter(green);
            Game1.characterManager.ScheduleAddCharacter(blue);
            Game1.characterManager.ScheduleAddCharacter(purple);
            Game1.characterManager.ScheduleAddCharacter(red);
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if(attack is ProjectileAttack)
                base.TakeDamage(attack, damage);
        }

        public override void HandleDeath()
        {
            base.HandleDeath();
            GameState.VITREOUS_DEFEATED = true;
            foreach (Character character in Game1.characterManager.GetCharacterList())
            {
                if (character is Eyeball)
                    Game1.characterManager.ScheduleRemoveCharacter(character);
            }

            if (fireballFlyingCue != null)
                fireballFlyingCue.Stop(AudioStopOptions.Immediate);

        }

        public override void GameOver()
        {
            if (fireballFlyingCue != null)
                fireballFlyingCue.Stop(AudioStopOptions.Immediate);
        }
    }
}
