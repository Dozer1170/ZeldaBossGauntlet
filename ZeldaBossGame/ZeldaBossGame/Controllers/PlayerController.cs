using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZeldaBossGame
{
    public class PlayerController : CharacterController
    {
        bool wasAttackPressed;

        public PlayerController(Character character) : base(character)
        {
            wasAttackPressed = false;
        }

        public override void Update(GameTime gameTime)
        {
            CheckKeyboard();
        }

        public override void Attack()
        {
            if(!character.attacking)
                character.Attack();
        }

        private void CheckKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            CheckMovement(state);
            CheckAttack(state);
        }

        private void CheckMovement(KeyboardState state)
        {
            bool changedFacing = false;
            bool wDown = state.IsKeyDown(Keys.W);
            bool sDown = state.IsKeyDown(Keys.S);
            bool aDown = state.IsKeyDown(Keys.A);
            bool dDown = state.IsKeyDown(Keys.D);

            if (wDown)
            {
                character.Move(new Vector2(0, -character.moveSpeed));
                if (!changedFacing)
                {
                    character.FaceUp();
                    changedFacing = true;
                }
            }
            if (sDown)
            {
                character.Move(new Vector2(0, character.moveSpeed));
                if (!changedFacing)
                {
                    character.FaceDown();
                    changedFacing = true;
                }
            }
            if (aDown)
            {
                character.Move(new Vector2(-character.moveSpeed, 0));
                if (!changedFacing)
                {
                    character.FaceLeft();
                    changedFacing = true;
                }
            }
            if (dDown)
            {
                character.Move(new Vector2(character.moveSpeed, 0));
                if (!changedFacing)
                {
                    character.FaceRight();
                    changedFacing = true;
                }
            }

            if (!wDown && !sDown && !aDown && !dDown)
                character.StandStill();
        }

        private void CheckAttack(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.J) && !wasAttackPressed)
            {
                Attack();
            }

            wasAttackPressed = state.IsKeyDown(Keys.J);
        }

        public override void TookHit()
        {
            
        }
    }
}
