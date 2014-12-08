using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class AnimatedCharacter : Character
    {
        public AnimationList animations;
        public bool despawnOnDeath;

        public static string WALK_DOWN_ANIM_NAME = "WALK_DOWN";
        public static string WALK_UP_ANIM_NAME = "WALK_UP";
        public static string WALK_SIDE_ANIM_NAME = "WALK_SIDE";
        public static string STAND_STILL_DOWN_ANIM_NAME = "STAND_DOWN";
        public static string STAND_STILL_UP_ANIM_NAME = "STAND_UP";
        public static string STAND_STILL_SIDE_ANIM_NAME = "STAND_SIDE";
        public static string DEATH_ANIM_NAME = "DEATH";

        public AnimatedCharacter(Sprite sprite, Vector2 position)
            : base(sprite, position)
        {
            despawnOnDeath = true;
            animations = new AnimationList();
            Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sprite.Update(gameTime);
        }

        public void AddAnimation(SpriteAnimation anim)
        {
            animations.AddAnimation(anim);
        }

        public void PlayAnimation(string name)
        {
            sprite.currAnim = animations.GetAnimation(name);
            sprite.currAnim.Restart();
        }

        public void PlayAnimation(string name, Action callback)
        {
            PlayAnimation(name);
            sprite.currAnim.SetCallback(callback);
        }

        public override void FaceDown()
        {
            if ((direction != Direction.Down || !sprite.currAnim.name.Equals(WALK_DOWN_ANIM_NAME)) && !attacking)
            {
                base.FaceDown();
                PlayAnimation(WALK_DOWN_ANIM_NAME);
            }
        }

        public override void FaceUp()
        {
            if ((direction != Direction.Up || !sprite.currAnim.name.Equals(WALK_UP_ANIM_NAME)) && !attacking)
            {
                base.FaceUp();
                PlayAnimation(WALK_UP_ANIM_NAME);
            }
        }

        public override void FaceLeft()
        {
            if ((direction != Direction.Left || !sprite.currAnim.name.Equals(WALK_SIDE_ANIM_NAME)) && !attacking)
            {
                base.FaceLeft();
                PlayAnimation(WALK_SIDE_ANIM_NAME);
            }
        }

        public override void FaceRight()
        {
            if ((direction != Direction.Right || !sprite.currAnim.name.Equals(WALK_SIDE_ANIM_NAME)) && !attacking)
            {
                base.FaceRight();
                PlayAnimation(WALK_SIDE_ANIM_NAME);
            }
        }

        public override void StandStill()
        {
            if (!attacking)
            {
                if (direction == Direction.Down)
                    PlayAnimation(STAND_STILL_DOWN_ANIM_NAME);
                else if (direction == Direction.Up)
                    PlayAnimation(STAND_STILL_UP_ANIM_NAME);
                else if (direction == Direction.Left || direction == Direction.Right)
                    PlayAnimation(STAND_STILL_SIDE_ANIM_NAME);
            }
        }

        public override void HandleDeath()
        {
            alive = false;
            PlayAnimation(DEATH_ANIM_NAME, delegate()
            {
                if(despawnOnDeath)
                    Game1.characterManager.ScheduleRemoveCharacter(this);
            });
        }
    }
}
