using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class Agahnim : BossCharacter
    {
        public static int AGAHNIM_SPRITE_WIDTH = 128;
        public static int AGAHNIM_SPRITE_HEIGHT = 128;

        public static string LIGHTNING_SPHERE_ANIM_NAME = "lightningsphere";
        public static string ICE_BURST_ANIM_NAME = "iceburst";
        public static string LIGHTNING_FORK_ANIM_NAME = "lightningfork";
        public static string LEFT_BURST_ANIM_NAME = "leftburst";
        public static string UPLEFT_BURST_ANIM_NAME = "upleftburst";
        public static string UP_BURST_ANIM_NAME = "upburst";
        public static string UPRIGHT_BURST_ANIM_NAME = "uprightburst";
        public static string RIGHT_BURST_ANIM_NAME = "rightburst";
        public static string DOWNRIGHT_BURST_ANIM_NAME = "downrightburst";
        public static string DOWN_BURST_ANIM_NAME = "downburst";
        public static string DOWN_LEFT_BURST_ANIM_NAME = "downleftburst";
        public static string LIGHTNING_FORK_CAST_ANIM_NAME = "lightningforkcast";

        public static Attack LIGHNING_SPHERE;
        public static Attack ICE_BURST;
        public static ProjectileAttack LIGHTNING_FORK;
        public bool lightningForkActive;

        public List<ProjectileAttack> bursts;
        public bool projectilesActive;

        public Agahnim(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
        }

        public override void Initialize()
        {
            heartPieceSpawnPosition = Game1.backgroundManager.backgrounds[BackgroundManager.SWAMP_DUNGEON_INDEX].size / 2;

            sprite.layerDepth = 0.5f;

            health = 16;
            maxHealth = 16;

            bursts = new List<ProjectileAttack>();
            projectilesActive = false;
            lightningForkActive = false;

            InitAnims();
            InitAttacks();

            moveSpeed = 4;

            SetBoundingShapes(new BoundingShapes(pos, new BoundingBox(new Vector3(50, 40, 0), new Vector3(78, 81, 0))));

            FaceDown();
        }

        public override void InitAnims()
        {
            Point spriteSize = new Point(AGAHNIM_SPRITE_WIDTH, AGAHNIM_SPRITE_HEIGHT);

            SpriteAnimation walkDownAnimation = new SpriteAnimation(new Point(512, 128), spriteSize, 2, 5, true, WALK_DOWN_ANIM_NAME);
            SpriteAnimation walkUpAnimation = new SpriteAnimation(new Point(0, 128), spriteSize, 2, 5, true, WALK_UP_ANIM_NAME);
            SpriteAnimation walkSideAnimation = new SpriteAnimation(new Point(256, 128), spriteSize, 2, 5, true, WALK_SIDE_ANIM_NAME);
            SpriteAnimation standStillDownAnimation = new SpriteAnimation(new Point(0, 0), spriteSize, 1, 0, false, STAND_STILL_DOWN_ANIM_NAME);
            SpriteAnimation standStillUpAnimation = new SpriteAnimation(new Point(0, 128), spriteSize, 1, 0, false, STAND_STILL_UP_ANIM_NAME);
            SpriteAnimation standStillSideAnimation = new SpriteAnimation(new Point(256, 128), spriteSize, 1, 0, false, STAND_STILL_SIDE_ANIM_NAME);
            SpriteAnimation lightningSphereAnimation = new SpriteAnimation(new Point(0, 0), spriteSize, 8, 20, false, LIGHTNING_SPHERE_ANIM_NAME);
            SpriteAnimation deathAnimation = new SpriteAnimation(new Point(256, 384), spriteSize, 6, 10, false, DEATH_ANIM_NAME);
            SpriteAnimation iceBurstAnimation = new SpriteAnimation(new Point(384, 0), spriteSize, 1, 0, false, ICE_BURST_ANIM_NAME);
            SpriteAnimation lightningForkCastAnimation = new SpriteAnimation(new Point(256, 0), spriteSize, 2, 40, true, LIGHTNING_FORK_CAST_ANIM_NAME);

            SpriteAnimation leftBurstFireball = new SpriteAnimation(new Point(0, 256),
                spriteSize, 1, 0, false, LEFT_BURST_ANIM_NAME);
            SpriteAnimation upLeftBurstFireball = new SpriteAnimation(new Point(128, 256),
                spriteSize, 1, 0, false, UPLEFT_BURST_ANIM_NAME);
            SpriteAnimation upBurstFireball = new SpriteAnimation(new Point(256, 256),
                spriteSize, 1, 0, false, UP_BURST_ANIM_NAME);
            SpriteAnimation upRightBurstFireball = new SpriteAnimation(new Point(384, 256),
                spriteSize, 1, 0, false, UPRIGHT_BURST_ANIM_NAME);
            SpriteAnimation rightBurstFireball = new SpriteAnimation(new Point(512, 256),
                spriteSize, 1, 0, false, RIGHT_BURST_ANIM_NAME);
            SpriteAnimation downRightBurstFireball = new SpriteAnimation(new Point(640, 256),
                spriteSize, 1, 0, false, DOWNRIGHT_BURST_ANIM_NAME);
            SpriteAnimation downBurstFireball = new SpriteAnimation(new Point(768, 256),
                spriteSize, 1, 0, false, DOWN_BURST_ANIM_NAME);
            SpriteAnimation downLeftBurstFireball = new SpriteAnimation(new Point(896, 256),
                spriteSize, 1, 0, false, DOWN_LEFT_BURST_ANIM_NAME);
            SpriteAnimation lightningForkAnimation = new SpriteAnimation(new Point(0, 512), new Point(192, 320), 5, 20, false, LIGHTNING_FORK_ANIM_NAME);

            AddAnimation(walkDownAnimation);
            AddAnimation(walkUpAnimation);
            AddAnimation(walkSideAnimation);
            AddAnimation(standStillDownAnimation);
            AddAnimation(standStillUpAnimation);
            AddAnimation(standStillSideAnimation);
            AddAnimation(lightningSphereAnimation);
            AddAnimation(deathAnimation);
            AddAnimation(iceBurstAnimation);
            AddAnimation(leftBurstFireball);
            AddAnimation(upLeftBurstFireball);
            AddAnimation(upBurstFireball);
            AddAnimation(upRightBurstFireball);
            AddAnimation(rightBurstFireball);
            AddAnimation(downRightBurstFireball);
            AddAnimation(downBurstFireball);
            AddAnimation(downLeftBurstFireball);
            AddAnimation(lightningForkAnimation);
            AddAnimation(lightningForkCastAnimation);
        }

        public override void InitAttacks()
        {
            BoundingShapes lightningSphereBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(19, 64, 0), new Vector3(104, 160, 0)));
            LIGHNING_SPHERE = new Attack(this, lightningSphereBox, 2, 2, 6, 6,
                animations.GetAnimation(LIGHTNING_SPHERE_ANIM_NAME).millisecondsPerFrame);

            BoundingShapes iceBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
            ICE_BURST = new Attack(this, iceBurstBox, 0, 0, 0, 0,
                animations.GetAnimation(ICE_BURST_ANIM_NAME).millisecondsPerFrame);

            BoundingShapes lightningForkBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(0, 0, 0), new Vector3(192, 320, 0)));
            //ADD INNER BOXES
            lightningForkBox.AddInnerBoundingBox(new BoundingBox(new Vector3(61, 18, 0), new Vector3(138, 130, 0)));
            lightningForkBox.AddInnerBoundingBox(new BoundingBox(new Vector3(43, 133, 0), new Vector3(183, 209, 0)));
            lightningForkBox.AddInnerBoundingBox(new BoundingBox(new Vector3(17, 215, 0), new Vector3(178, 318, 0)));
            Sprite lfSprite = new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(192, 320), animations.GetAnimation(LIGHTNING_FORK_ANIM_NAME));
            lfSprite.layerDepth = 1;
            LIGHTNING_FORK = new ProjectileAttack(this, lightningForkBox, 2, 0, 5, 5,
                animations.GetAnimation(LIGHTNING_FORK_ANIM_NAME).millisecondsPerFrame,
                lfSprite, pos, new Vector2(0, 0));

            BoundingShapes leftBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(44, 54, 0), new Vector3(94, 78, 0)));
            bursts.Add(new ProjectileAttack(this, leftBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(LEFT_BURST_ANIM_NAME)),
                pos, new Vector2(-8, 0)));

            BoundingShapes upLeftBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(48, 48, 0), new Vector3(88, 78, 0)));
            upLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(56, 58, 0), 7));
            upLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(69, 67, 0), 5));
            upLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(82, 76, 0), 4));
            bursts.Add(new ProjectileAttack(this, upLeftBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(UPLEFT_BURST_ANIM_NAME)),
                pos, new Vector2(-4, -4)));

            BoundingShapes upBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(52, 40, 0), new Vector3(71, 88, 0)));
            bursts.Add(new ProjectileAttack(this, upBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(UP_BURST_ANIM_NAME)),
                pos, new Vector2(0, -8)));

            BoundingShapes upRightBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(44, 42, 0), new Vector3(80, 88, 0)));
            upRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(72, 55, 0), 7));
            upRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(63, 68, 0), 5));
            upRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(54, 80, 0), 4));
            bursts.Add(new ProjectileAttack(this, upRightBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(UPRIGHT_BURST_ANIM_NAME)),
                pos, new Vector2(4, -4)));

            BoundingShapes rightBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(43, 56, 0), new Vector3(87, 72, 0)));
            bursts.Add(new ProjectileAttack(this, rightBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(RIGHT_BURST_ANIM_NAME)),
                pos, new Vector2(8, 0)));

            BoundingShapes downRightBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(41, 41, 0), new Vector3(84, 83, 0)));
            downRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(73, 71, 0), 7));
            downRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(60, 61, 0), 5));
            downRightBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(47, 52, 0), 4));
            bursts.Add(new ProjectileAttack(this, downRightBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(DOWNRIGHT_BURST_ANIM_NAME)),
                pos, new Vector2(4, 4)));

            BoundingShapes downBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(56, 41, 0), new Vector3(75, 86, 0)));
            bursts.Add(new ProjectileAttack(this, downBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(DOWN_BURST_ANIM_NAME)),
                pos, new Vector2(0, 8)));

            BoundingShapes downLeftBurstBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(52, 36, 0), new Vector3(80, 80, 0)));
            downLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(61, 68, 0), 7));
            downLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(70, 55, 0), 5));
            downLeftBurstBox.AddInnerBoundingSphere(new BoundingSphere(new Vector3(80, 42, 0), 4));
            bursts.Add(new ProjectileAttack(this, downLeftBurstBox, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, animations.GetAnimation(DOWN_LEFT_BURST_ANIM_NAME)),
                pos, new Vector2(-4, 4)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (projectilesActive)
            {
                foreach (ProjectileAttack proj in bursts)
                {
                    proj.Update(gameTime, pos);
                }
            }

            if (lightningForkActive)
            {
                LIGHTNING_FORK.Update(gameTime, pos);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (projectilesActive)
            {
                foreach (ProjectileAttack proj in bursts)
                {
                    proj.Draw(spriteBatch);
                }
            }

            if (lightningForkActive)
            {
                LIGHTNING_FORK.Draw(spriteBatch);
            }
        }

        public override void StandStill()
        {
            PlayAnimation(STAND_STILL_DOWN_ANIM_NAME);
        }

        public override void Attack()
        {
            attacking = true;
            Action callback = delegate()
            {
                attacking = false;
                StandStill();
            };
            PlayAnimation(LIGHTNING_SPHERE_ANIM_NAME, callback);
            DoAttack(LIGHNING_SPHERE);
            Game1.soundManager.PlayCue(SoundManager.ELECTRICITY_SHOCK);
        }

        public override void Attack(Attack attack)
        {
            if (attack.Equals(ICE_BURST))
            {
                PlayAnimation(ICE_BURST_ANIM_NAME);
                StartIceBurst();
                Game1.soundManager.PlayCue(SoundManager.ICE_CHARGE);
            }
            else if (attack.Equals(LIGHTNING_FORK))
            {
                Action callback = delegate()
                {
                    attacking = false;
                    lightningForkActive = false;
                    StandStill();
                };
                LIGHTNING_FORK.sprite.currAnim.SetCallback(callback);
                DoProjectileAttack(LIGHTNING_FORK);
                lightningForkActive = true;
                Game1.soundManager.PlayCue(SoundManager.ELECTRICITY_SHOCK);

            }
        }

        private void StartIceBurst()
        {
            ResetBurstPositions();
            projectilesActive = true;
        }

        private void ResetBurstPositions()
        {
            foreach (ProjectileAttack proj in bursts)
            {
                proj.UpdatePosition(pos);
            }
        }

        public override void HandleDeath()
        {
            base.HandleDeath();
            GameState.AGAHNIM_DEFEATED = true;
        }
    }
}
