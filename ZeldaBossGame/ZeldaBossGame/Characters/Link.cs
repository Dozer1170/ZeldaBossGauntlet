using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class Link : AnimatedCharacter
    {
        public static int LINK_SPRITE_WIDTH = 128;
        public static int LINK_SPRITE_HEIGHT = 128;

        public static string SWING_DOWN_ANIM_NAME = "SWING_DOWN";
        public static string SWING_UP_ANIM_NAME = "SWING_UP";
        public static string SWING_SIDE_ANIM_NAME = "SWING_SIDE";

        public static Attack SWING_DOWN;
        public static Attack SWING_LEFT;
        public static Attack SWING_RIGHT;
        public static Attack SWING_UP;

        public Link(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
        }

        public override void Initialize()
        {
            sprite.layerDepth = 0.9f;

            health = 6;
            maxHealth = 6;

            InitAnims();
            InitAttacks();

            moveSpeed = 3;

            SetBoundingShapes(new BoundingShapes(pos, new BoundingBox(new Vector3(47, 38, 0), new Vector3(82, 89, 0))));

            FaceUp();
        }

        public override void InitAnims()
        {
            Point spriteSize = new Point(LINK_SPRITE_WIDTH, LINK_SPRITE_HEIGHT);

            SpriteAnimation walkDownAnimation = new SpriteAnimation(new Point(0, 0), spriteSize, 7, 20, true, WALK_DOWN_ANIM_NAME);
            SpriteAnimation walkUpAnimation = new SpriteAnimation(new Point(0, 128), spriteSize, 7, 20, true, WALK_UP_ANIM_NAME);
            SpriteAnimation walkSideAnimation = new SpriteAnimation(new Point(0, 256), spriteSize, 7, 20, true, WALK_SIDE_ANIM_NAME);
            SpriteAnimation swingDownAnimation = new SpriteAnimation(new Point(0, 384), spriteSize, 6, 30, false, SWING_DOWN_ANIM_NAME);
            SpriteAnimation swingUpAnimation = new SpriteAnimation(new Point(0, 512), spriteSize, 7, 30, false, SWING_UP_ANIM_NAME);
            SpriteAnimation swingSideAnimation = new SpriteAnimation(new Point(0, 640), spriteSize, 6, 30, false, SWING_SIDE_ANIM_NAME);
            SpriteAnimation standStillDownAnimation = new SpriteAnimation(new Point(256, 0), spriteSize, 1, 0, false, STAND_STILL_DOWN_ANIM_NAME);
            SpriteAnimation standStillUpAnimation = new SpriteAnimation(new Point(384, 128), spriteSize, 1, 0, false, STAND_STILL_UP_ANIM_NAME);
            SpriteAnimation standStillSideAnimation = new SpriteAnimation(new Point(384, 256), spriteSize, 1, 0, false, STAND_STILL_SIDE_ANIM_NAME);
            SpriteAnimation deathAnimation = new SpriteAnimation(new Point(0, 768), spriteSize, 3, 5, false, DEATH_ANIM_NAME);

            AddAnimation(walkDownAnimation);
            AddAnimation(walkUpAnimation);
            AddAnimation(walkSideAnimation);
            AddAnimation(swingDownAnimation);
            AddAnimation(swingUpAnimation);
            AddAnimation(swingSideAnimation);
            AddAnimation(standStillDownAnimation);
            AddAnimation(standStillUpAnimation);
            AddAnimation(standStillSideAnimation);
            AddAnimation(deathAnimation);
        }

        public override void InitAttacks()
        {
            BoundingShapes swingDownBox = new BoundingShapes(pos, 
                new BoundingBox(new Vector3(32,75,0), new Vector3(85,120, 0)));
            SWING_DOWN = new Attack(this, swingDownBox, 2, 2, 6, 6,
                animations.GetAnimation(SWING_DOWN_ANIM_NAME).millisecondsPerFrame);

            BoundingShapes swingUpBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(44, 13, 0), new Vector3(85, 57, 0)));
            SWING_UP = new Attack(this, swingUpBox, 2, 3, 7, 7,
                animations.GetAnimation(SWING_UP_ANIM_NAME).millisecondsPerFrame);

            BoundingShapes swingRightBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(56, 27, 0), new Vector3(110, 90, 0)));
            SWING_RIGHT = new Attack(this, swingRightBox, 2, 3, 7, 7,
                animations.GetAnimation(SWING_SIDE_ANIM_NAME).millisecondsPerFrame);

            BoundingShapes swingLeftBox = new BoundingShapes(pos,
                new BoundingBox(new Vector3(24, 12, 0), new Vector3(54, 100, 0)));
            SWING_LEFT = new Attack(this, swingLeftBox, 2, 3, 7, 7,
                animations.GetAnimation(SWING_SIDE_ANIM_NAME).millisecondsPerFrame);
        }

        public override Color Move(Vector2 velocity)
        {
            Color hitMapColor =  base.Move(velocity);

            if (hitMapColor.Equals(Color.White)) {
                //Hit exit to overworld
                Game1.backgroundManager.TransitionToMap(BackgroundManager.OVERWORLD_INDEX);
            }
            else if (hitMapColor.Equals(Color.Cyan))
            {
                //Hit entrance to swamp dungeon
                Game1.backgroundManager.TransitionToMap(BackgroundManager.SWAMP_DUNGEON_INDEX);
            }
            else if (hitMapColor.Equals(Color.Red))
            {
                //Hit entrance to death mountain dungeon
                Game1.backgroundManager.TransitionToMap(BackgroundManager.DEATH_MOUNTAIN_DUNGEON_INDEX);
            }
            else if (hitMapColor.Equals(Color.Lime))
            {
                //Hit entrance to desert dungeon
                Game1.backgroundManager.TransitionToMap(BackgroundManager.DESERT_DUNGEON_INDEX);
            }
            else if (hitMapColor.Equals(Color.Blue))
            {
                //Hit entrance to witch's hut dungeon
                Game1.backgroundManager.TransitionToMap(BackgroundManager.WITCH_HUT_DUNGEON_INDEX);
            }

            return hitMapColor;
        }

        public override void UpdatePosition(Vector2 nPos)
        {
            pos = nPos;
            Game1.backgroundManager.CenterOnPos(pos + sprite.size/2);
            sprite.pos = pos - Game1.GetActiveBackground().topLeft;
        }

        public override void Attack()
        {
            attacking = true;
            Action callback = delegate() 
            {
                attacking = false;
                StandStill(); 
            };
            if (direction == Direction.Left)
            {
                PlayAnimation(SWING_SIDE_ANIM_NAME, callback);
                DoAttack(SWING_LEFT);
            }
            else if (direction == Direction.Right)
            {
                PlayAnimation(SWING_SIDE_ANIM_NAME, callback);
                DoAttack(SWING_RIGHT);
            }
            else if (direction == Direction.Up)
            {
                PlayAnimation(SWING_UP_ANIM_NAME, callback);
                DoAttack(SWING_UP);
            }
            else if (direction == Direction.Down)
            {
                PlayAnimation(SWING_DOWN_ANIM_NAME, callback);
                DoAttack(SWING_DOWN);
            }

            PlayLinkAttackSound();
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if (invinciblityFrames < 1)
                PlayLinkHurtSound();
            base.TakeDamage(attack, damage);
        }

        private void PlayLinkAttackSound() 
        {
            Random rand = new Random();
            int num = rand.Next() % 4;
            if (num == 0)
                Game1.soundManager.PlayCue(SoundManager.LINK_ATTACK1);
            else if (num == 1)
                Game1.soundManager.PlayCue(SoundManager.LINK_ATTACK2);
            else if (num == 2)
                Game1.soundManager.PlayCue(SoundManager.LINK_ATTACK3);
            else if (num == 3)
                Game1.soundManager.PlayCue(SoundManager.LINK_ATTACK4);

            Game1.soundManager.PlayCue(SoundManager.SWORD1);
        }

        private void PlayLinkHurtSound()
        {
            Random rand = new Random();
            int num = rand.Next() % 3;
            if (num == 0)
                Game1.soundManager.PlayCue(SoundManager.LINK_HURT1);
            else if (num == 1)
                Game1.soundManager.PlayCue(SoundManager.LINK_HURT2);
            else if (num == 2)
                Game1.soundManager.PlayCue(SoundManager.LINK_HURT3);
        }

        public override void Attack(Attack attack)
        {
            throw new NotImplementedException();
        }

        public override void HandleCollision(Character characterCollision, bool atFault, Vector2 prevPosition)
        {
            //Do nothing
        }

        public override void HandleDeath()
        {
            alive = false;
            PlayAnimation(DEATH_ANIM_NAME);
            Game1.soundManager.PlayMusicNoLoop(Game1.soundManager.gameOver);
            GameState.GAME_OVER = true;
        }
    }
}
