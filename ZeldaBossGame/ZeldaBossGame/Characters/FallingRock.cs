using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class FallingRock : AnimatedCharacter
    {
        public static string ROCK_CRUSH = "rockcrush";

        public Attack crushAttack;

        public FallingRock(Sprite sprite, Vector2 worldPos, Random rand)
            : base(sprite, worldPos)
        {
            controller = new FallingRockController(this, rand);
        }

        public override void Initialize()
        {
            sprite.layerDepth = 1.0f;

            InitAnims();
            InitAttacks();
        }

        public override void InitAnims()
        {
            SpriteAnimation fallingFrame = new SpriteAnimation(new Point(384, 512), new Point(128, 128), 1, 0, false, STAND_STILL_DOWN_ANIM_NAME);
            SpriteAnimation rockCrush = new SpriteAnimation(new Point(384, 512), new Point(128, 128), 5, 10, false, ROCK_CRUSH);
            SpriteAnimation death = new SpriteAnimation(new Point(896, 512), new Point(128, 128), 1, 0, false, DEATH_ANIM_NAME);

            AddAnimation(fallingFrame);
            AddAnimation(rockCrush);
            AddAnimation(death);

            PlayAnimation(STAND_STILL_DOWN_ANIM_NAME);
        }

        public override void InitAttacks()
        {
            BoundingShapes crushShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(15, 15, 0), new Vector3(113, 113, 0)));
            crushAttack = new Attack(this, crushShapes, 1, 0, 5, 5, animations.GetAnimation(ROCK_CRUSH).millisecondsPerFrame);
        }

        public override void Attack()
        {
            DoAttack(crushAttack);
            PlayAnimation(ROCK_CRUSH, delegate() { HandleDeath(); });
            Game1.soundManager.PlayCue(SoundManager.BOULDER_CRASH);
        }
    }
}
