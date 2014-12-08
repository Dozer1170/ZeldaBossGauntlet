using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZeldaBossGame
{
    public class Volvagia : SegmentedBossCharacter
    {
        public static string LARGEST_SEGMENT_ANIM_NAME = "largest";
        public static string LM_SEGMENT_ANIM_NAME = "lm";
        public static string MM_SEGMENT_ANIM_NAME = "mm";
        public static string MS_SEGMENT_ANIM_NAME = "ms";
        public static string SMALL_SEGMENT_ANIM_NAME = "small";
        public static string SMALLEST_SEGMENT_ANIM_NAME = "smallest";
        public static string FLAME_STARTUP_ANIM_NAME = "flame";
        public static string FLOAT_STRAIGHT = "straight";
        public static string FLOAT_LEFT = "left";
        public static string FLOAT_RIGHT = "right";
        public static string FIRE_BREATH_ANIM_NAME = "firebreath";

        public Cue boulderLoopCue;

        public VolvagiaArm leftArm, rightArm;
        Vector2 leftArmOffset, rightArmOffset, firstSegmentOffset, fireBreathOffset;
        int numRocksToSpawn;

        public ProjectileAttack fireBreath;

        public Volvagia(Sprite sprite, Vector2 worldPos, float layerDepth) :
            base(sprite, worldPos, layerDepth)
        {
        }

        public override void Initialize()
        {
            heartPieceSpawnPosition = new Vector2(782, 762);

            numRocksToSpawn = 10;

            moveSpeed = 5;

            leftArmOffset = new Vector2(sprite.size.X/3 + 10, sprite.size.Y/3);
            rightArmOffset = new Vector2(-(sprite.size.X/3 + 10), sprite.size.Y/3);
            firstSegmentOffset = new Vector2(0 , sprite.size.Y/6);
            fireBreathOffset = new Vector2(62 - 128, 200);

            maxHealth = 16;
            health = 16;

            SetBoundingShapes(new BoundingShapes(pos, new BoundingBox(new Vector3(27, 72, 0), new Vector3(98, 207, 0))));
            InitAnims();
            InitAttacks();
        }

        public void InitArms()
        {
            if (leftArm == null && rightArm == null)
            {
                leftArm = new VolvagiaArm(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 256)), leftArmOffset);
                rightArm = new VolvagiaArm(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 256)), rightArmOffset);
                rightArm.sprite.flipped = true;

                BoundingShapes leftArmShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(32, 15, 0), new Vector3(104, 125, 0)));
                leftArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(44, 20, 0), new Vector3(80, 66, 0)));
                leftArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(79, 38, 0), new Vector3(102, 88, 0)));
                leftArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(34, 75, 0), new Vector3(72, 127, 0)));
                BoundingShapes rightArmShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(16, 17, 0), new Vector3(98, 127, 0)));
                rightArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(47, 17, 0), new Vector3(80, 58, 0)));
                rightArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(24, 47, 0), new Vector3(55, 90, 0)));
                rightArmShapes.AddInnerBoundingBox(new BoundingBox(new Vector3(45, 73, 0), new Vector3(98, 127, 0)));
                leftArm.SetBoundingShapes(leftArmShapes);
                rightArm.SetBoundingShapes(rightArmShapes);

                
            }

            Game1.characterManager.ScheduleAddCharacter(leftArm);
            Game1.characterManager.ScheduleAddCharacter(rightArm);
        }

        public override void InitAnims()
        {
            SpriteAnimation largestSegment = new SpriteAnimation(new Point(384, 128), new Point(128, 128), 1, 0, false, LARGEST_SEGMENT_ANIM_NAME);
            SpriteAnimation lmSegment = new SpriteAnimation(new Point(512, 128), new Point(128, 128), 1, 0, false, LM_SEGMENT_ANIM_NAME);
            SpriteAnimation mmSegment = new SpriteAnimation(new Point(640, 128), new Point(128, 128), 1, 0, false, MM_SEGMENT_ANIM_NAME);
            SpriteAnimation msSegment = new SpriteAnimation(new Point(384, 0), new Point(128, 128), 1, 0, false, MS_SEGMENT_ANIM_NAME);
            SpriteAnimation smallSegment = new SpriteAnimation(new Point(512, 0), new Point(128, 128), 1, 0, false, SMALL_SEGMENT_ANIM_NAME);
            SpriteAnimation smallestSegment = new SpriteAnimation(new Point(640, 0), new Point(128, 128), 1, 0, false, SMALLEST_SEGMENT_ANIM_NAME);

            SpriteAnimation flameStartup = new SpriteAnimation(new Point(0, 0), new Point(128, 256), 3, 3, false, FLAME_STARTUP_ANIM_NAME);
            SpriteAnimation fireBreath = new SpriteAnimation(new Point(0, 768), new Point(256, 256), 4, 6, false, FIRE_BREATH_ANIM_NAME);
            SpriteAnimation floatLeft = new SpriteAnimation(new Point(0, 512), new Point(128, 256), 1, 0, false, FLOAT_LEFT);
            SpriteAnimation floatStraight = new SpriteAnimation(new Point(128, 512), new Point(128, 256), 1, 0, false, FLOAT_STRAIGHT);
            SpriteAnimation floatRight = new SpriteAnimation(new Point(256, 512), new Point(128, 256), 1, 0, false, FLOAT_RIGHT);
            SpriteAnimation deathExplosion = new SpriteAnimation(new Point(512, 256), new Point(128, 128), 4, 1, false, DEATH_ANIM_NAME);

            
            AddAnimation(largestSegment);
            AddAnimation(lmSegment);
            AddAnimation(mmSegment);
            AddAnimation(msSegment);
            AddAnimation(smallSegment);
            AddAnimation(smallestSegment);

            AddAnimation(flameStartup);
            AddAnimation(fireBreath);
            AddAnimation(floatLeft);
            AddAnimation(floatStraight);
            AddAnimation(floatRight);
            AddAnimation(deathExplosion);
        }

        public override void InitAttacks()
        {
            BoundingShapes fireBreathShapes = new BoundingShapes(pos, 
                new BoundingBox(new Vector3(55, 0, 0), new Vector3(207, 255, 0)));
            fireBreath = new ProjectileAttack(this, fireBreathShapes, 2, 0, 4, 4,
                animations.GetAnimation(FIRE_BREATH_ANIM_NAME).millisecondsPerFrame,
                new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(256, 256),
                    animations.GetAnimation(FIRE_BREATH_ANIM_NAME)),
                pos, new Vector2(0, 0));
        }

        public override void InitSegments()
        {
            AddLargeSegment();
            AddLargeSegment();
            AddLargeMediumSegment();
            AddLargeMediumSegment();
            AddMediumSegment();
            AddMediumSegment();
            AddMediumSmallSegment();
            AddMediumSmallSegment();
            AddSmallSegment();
            AddSmallSegment();
            AddSmallestSegment();
            AddSmallestSegment();
        }

        public void AddLargeSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128), 
                animations.GetAnimation(LARGEST_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public void AddLargeMediumSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128),
                animations.GetAnimation(LM_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public void AddMediumSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128),
                animations.GetAnimation(MM_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public void AddMediumSmallSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128),
                animations.GetAnimation(MS_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public void AddSmallSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128),
                animations.GetAnimation(SMALL_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public void AddSmallestSegment()
        {
            AnimatedCharacter segment = new AnimatedCharacter(new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128),
                animations.GetAnimation(SMALLEST_SEGMENT_ANIM_NAME)), pos);

            segment.SetBoundingShapes(new BoundingShapes(new Vector2(0, 0), new BoundingBox(new Vector3(0, 0, 0), new Vector3(128, 128, 0))));
            segment.animations.AddAnimation(animations.GetAnimation(DEATH_ANIM_NAME));

            AddSegment(segment);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            leftArm.UpdatePosition(pos + leftArmOffset);
            leftArm.boundingShapes.UpdatePosition(leftArm.pos);
            rightArm.UpdatePosition(pos + rightArmOffset);
            rightArm.boundingShapes.UpdatePosition(rightArm.pos);
            segments[0].UpdatePosition(pos + firstSegmentOffset);

            //If doing flame breath
            if (attacking)
            {
                fireBreath.UpdatePosition(pos + fireBreathOffset);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (attacking)
                fireBreath.Draw(spriteBatch);
        }

        public override Color Move(Vector2 velocity)
        {
            ForceMove(velocity);

            return Color.Transparent;
        }

        public override void FaceDown()
        {
            PlayAnimation(FLOAT_STRAIGHT);
        }

        public override void FaceLeft()
        {
            PlayAnimation(FLOAT_LEFT);
        }

        public override void FaceRight()
        {
            PlayAnimation(FLOAT_RIGHT);
        }

        public override void FaceUp()
        {
            PlayAnimation(FLOAT_STRAIGHT);
        }

        public override void Attack()
        {
            PlayAnimation(FLAME_STARTUP_ANIM_NAME,
                delegate()
                {
                    DoAttack(fireBreath);
                    Game1.soundManager.PlayCue(SoundManager.VOLVAGIA_FLAME);
                });
        }

        public override void ClearActiveAttack()
        {
            base.ClearActiveAttack();
            ((VolvagiaController)controller).fireBreathStarted = false;
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if (!leftArm.alive && !rightArm.alive)
            {
                if(invinciblityFrames < 1)
                    Game1.soundManager.PlayCue(SoundManager.VOLVAGIA_HIT);
                base.TakeDamage(attack, damage);
            }
        }

        public override void HandleDeath()
        {
            //Move down because death anim is smaller frame size than head
            UpdatePosition(new Vector2(pos.X, pos.Y + sprite.size.Y/2));
            base.HandleDeath();
            GameState.VOLVAGIA_DEFEATED = true;
        }

        public void DoArmAttack(Direction side)
        {
            if (side == Direction.Left)
                leftArm.Attack();
            else
                rightArm.Attack();
        }

        public void SpawnRocks()
        {
            //Create rand for falling rocks
            Random rand = new Random(DateTime.Now.Millisecond);
            for(int i = 0; i < numRocksToSpawn; i++) 
            {
                Character rock = new FallingRock(new Sprite(sprite.texture, pos, new Vector2(128, 128)),
                    new Vector2(0, 0), rand);
                Game1.characterManager.ScheduleAddCharacter(rock);
            }
        }

        public void StartBoulderLoopSound()
        {
            StopBoulderLoopSound();
            boulderLoopCue = Game1.soundManager.soundBank.GetCue(SoundManager.BOULDER_LOOP);
            boulderLoopCue.Play();
        }

        public void StopBoulderLoopSound()
        {
            if (boulderLoopCue != null)
            {
                boulderLoopCue.Stop(AudioStopOptions.Immediate);
            }
        }

        public override void GameOver()
        {
            StopBoulderLoopSound();
        }
    }
}
