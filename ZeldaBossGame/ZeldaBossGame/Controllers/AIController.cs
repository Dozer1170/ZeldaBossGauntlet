using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class AIController : CharacterController
    {

        public bool chasing;

        public AIController(Character character)
            : base(character)
        {
            chasing = false;
        }

        public override void Update(GameTime gameTime)
        {
            character.UpdatePosition(character.pos);
            if (!character.attacking)
            {
                if (MoveToward(character, Game1.GetPlayerCharacter()))
                    Attack();
            }
        }

        public bool MoveToward(Character characterOne, Character otherCharacter)
        {
            return MoveToward(characterOne, otherCharacter, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherCharacter"></param>
        /// <returns>true if very close to otherCharacter</returns>
        public virtual bool MoveToward(Character characterOne, Character otherCharacter, int spriteSizeDivisorDist)
        {
            Vector2 direction = (otherCharacter.pos + otherCharacter.sprite.size/2) -
                (characterOne.pos + characterOne.sprite.size / 2);
            //If more than the half size of a sprite away, move towards it
            if (Math.Abs(direction.X) > characterOne.sprite.size.X / 2 || Math.Abs(direction.Y) > characterOne.sprite.size.Y / 2)
            {
                direction = Vector2.Normalize(direction);
                characterOne.Move(direction * characterOne.moveSpeed);
                DetermineFacingDirection(direction);
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns>true if close to destination point</returns>
        public virtual bool MoveToward(Vector2 point)
        {
            Vector2 direction = point -
                (character.pos + character.sprite.size / 2);
            //If more than 2 moves away keep going
            if (Math.Abs(direction.X) > character.moveSpeed * 2 || 
                Math.Abs(direction.Y) > character.moveSpeed * 2)
            {
                direction = Vector2.Normalize(direction);
                character.Move(direction * character.moveSpeed);
                DetermineFacingDirection(direction);
            }
            else
            {
                return true;
            }

            return false;
        }

        public virtual bool ForceMoveToward(Character characterOne, Character otherCharacter)
        {
            return ForceMoveToward(characterOne, otherCharacter, 2);
        }

        public virtual bool ForceMoveToward(Character characterOne, Character otherCharacter, int spriteSizeDivisorDist)
        {
            Vector2 direction = (otherCharacter.pos + otherCharacter.sprite.size / 2) -
                (characterOne.pos + characterOne.sprite.size / 2);
            //If more than the half size of a sprite away, move towards it
            if (Math.Abs(direction.X) > characterOne.sprite.size.X / spriteSizeDivisorDist ||
                Math.Abs(direction.Y) > characterOne.sprite.size.Y / spriteSizeDivisorDist)
            {
                direction = Vector2.Normalize(direction);
                characterOne.ForceMove(direction * characterOne.moveSpeed);
                DetermineFacingDirection(direction);
            }
            else
            {
                return true;
            }

            return false;
        }

        public virtual bool ForceMoveToward(Vector2 point)
        {
            Vector2 direction = point -
                (character.pos + character.sprite.size / 2);
            //If more than the half size of a sprite away, move towards it
            if (Math.Abs(direction.X) > character.moveSpeed * 2 ||
                Math.Abs(direction.Y) > character.moveSpeed * 2)
            {
                direction = Vector2.Normalize(direction);
                character.ForceMove(direction * character.moveSpeed);
                DetermineFacingDirection(direction);
            }
            else
            {
                return true;
            }

            return false;
        }

        public virtual void DetermineFacingDirection(Vector2 direction)
        {
            //If going up and going more in Y direction than X, face up
            if (direction.Y < 0 && Math.Abs(direction.Y) > Math.Abs(direction.X))
            {
                character.FaceUp();
            } //going down and more in Y than X
            else if(direction.Y > 0 && Math.Abs(direction.Y) > Math.Abs(direction.X))
            {
                character.FaceDown();
            } //going left and more in X than Y
            else if (direction.X < 0 && Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                character.FaceLeft();
            } //going right and more in X than Y
            else if (direction.X > 0 && Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                character.FaceRight();
            }
 
        }

        public override void Attack()
        {
            if(!character.attacking)
                character.Attack();
        }

        public override void TookHit()
        {
            
        }
    }
}
