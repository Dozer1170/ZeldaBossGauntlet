using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class VitreousController : BossController
    {
        Vector2 floatPosOne, floatPosTwo;
        bool floatingUp;
        int timer;
        int eyeballTimer;
        int fireballRechargeTime;
        int eyeballSpawnRecharge;

        public VitreousController(Character character)
            : base(character)
        {
            floatingUp = true;
            floatPosOne = new Vector2(character.pos.X + character.sprite.size.X/2,
                character.pos.Y + character.sprite.size.Y / 2 - 5);
            floatPosTwo = new Vector2(character.pos.X + character.sprite.size.X / 2, 
                character.pos.Y + character.sprite.size.Y / 2 + 5);

            timer = 0;
            eyeballTimer = 15000;
            fireballRechargeTime = 2000;
            eyeballSpawnRecharge = 15000;
        }

        public override void Update(GameTime gameTime)
        {
            if (floatingUp)
            {
                if (MoveToward(floatPosOne))
                    floatingUp = false;
            }
            else
            {
                if (MoveToward(floatPosTwo))
                    floatingUp = true;
            }

            if (!((Vitreous)character).fireballActive)
            {
                if (timer > fireballRechargeTime)
                {
                    timer = gameTime.ElapsedGameTime.Milliseconds;
                    DoFireball();
                }
                else
                {
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            if (eyeballTimer > eyeballSpawnRecharge)
            {
                eyeballTimer = gameTime.ElapsedGameTime.Milliseconds;
                ((Vitreous)character).SpawnEyeballs();
            }
            else
            {
                eyeballTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        private void DoFireball()
        {
            character.Attack(Vitreous.FIREBALL);
            character.Attack();
        }

        public override void DetermineFacingDirection(Vector2 direction)
        {
            if (!character.attacking)
            {
                direction = Game1.GetPlayerCharacter().pos - character.pos;
                direction = Vector2.Normalize(direction);
                //If going up and going more in Y direction than X, face up
                if (direction.Y < 0)
                {
                    character.FaceUp();
                } //going down and more in Y than X
                else if (direction.Y > 0)
                {
                    character.FaceDown();
                }

                if (direction.X > 0.5f)
                    character.FaceRight();
                else if (direction.X < -0.5f)
                    character.FaceLeft();
            }
        }

    }
}
