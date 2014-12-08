using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class IceChunk : AnimatedCharacter
    {
        public static string SPIN = "spin";

        int bounces;
        Vector2 velocity;
        Attack attack;

        public IceChunk(Sprite sprite, Vector2 worldPos, Vector2 dir)
            : base(sprite, worldPos)
        {
            moveSpeed = 5;

            this.velocity = new Vector2(dir.X * moveSpeed, dir.Y * moveSpeed);

            bounces = 4;

            SpriteAnimation anim = new SpriteAnimation(new Point(768,0), new Point(128,128), 2, 16, true, SPIN);
            SpriteAnimation death = new SpriteAnimation(new Point(512, 0), new Point(128, 128), 2, 5, false, DEATH_ANIM_NAME);
            AddAnimation(anim);
            AddAnimation(death);

            SetBoundingShapes(new BoundingShapes(pos,
                        new BoundingBox(new Vector3(52, 52, 0), new Vector3(76, 76, 0))));

            attack = new Attack(this, boundingShapes, 1, 0, 0, 0, 0);
            DoAttack(attack);

            PlayAnimation(SPIN);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(velocity);
        }

        public override Color Move(Vector2 velocity)
        {
            Color color = base.Move(velocity);

            if (color == Color.Black)
            {
                DecrementBounces();
            }

            return color;
        }

        public override void HandleCollision(Character characterCollided, bool atFault, Vector2 prevPosition)
        {
            if (!(characterCollided is TwinRova))
            {
                UpdatePosition(prevPosition);
                if(alive)
                    characterCollided.TakeDamage(attack, attack.damage);
                DecrementBounces();
            }
        }

        public void DecrementBounces()
        {
            bounces--;
            if (bounces < 1)
            {
                if(alive)
                    HandleDeath();
            }

            velocity *= -1;
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            //base.TakeDamage(attack, damage);
        }
    }
}
