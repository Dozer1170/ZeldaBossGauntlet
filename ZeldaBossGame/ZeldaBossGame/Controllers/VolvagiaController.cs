using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class VolvagiaController : BossController
    {
        int pointYInc;
        Vector2 currDestPoint;
        public bool spawnedRocks;
        public bool fireBreathStarted;

        public VolvagiaController(Character character)
            : base(character)
        {
            currentPhase = 1;
            pointYInc = 50;
            currDestPoint = PickDestPointDownScreen();
            fireBreathStarted = false;
            spawnedRocks = false;
        }

        public override void Update(GameTime gameTime)
        {
            Volvagia vol = ((Volvagia)character);

            MoveDownScreen();

            if (currentPhase == 1)
            {
                if (vol.rightArm.alive && !vol.rightArm.attacking && PlayerWithinRightArmReach())
                    vol.DoArmAttack(Direction.Right);

                if (vol.leftArm.alive && !vol.leftArm.attacking && PlayerWithinLeftArmReach())
                    vol.DoArmAttack(Direction.Left);

                if (!vol.leftArm.alive && !vol.rightArm.alive)
                    currentPhase = 2;
            }
            else
            {
                if (!fireBreathStarted)
                {
                    fireBreathStarted = true;
                    vol.Attack();
                }
            }

            MoveSegments();
        }

        private void MoveDownScreen()
        {
            if (character.pos.Y - Game1.GetActiveBackground().size.Y > 100)
            {
                ((SegmentedBossCharacter)character).MoveBossAndSegmentsToPoint(Game1.characterManager.volvagiaSpawnPosition);
                CalculateNewDest();
                spawnedRocks = false;
                ((Volvagia)character).StopBoulderLoopSound();
            }
            else
            {
                if (ForceMoveToward(currDestPoint))
                    CalculateNewDest();

                if (!spawnedRocks && character.pos.Y + character.sprite.size.Y > Game1.GetActiveBackground().size.Y - 200)
                {
                    ((Volvagia)character).SpawnRocks();
                    ((Volvagia)character).StartBoulderLoopSound();
                    spawnedRocks = true;
                }

            }
        }

        private void CalculateNewDest()
        {
            currDestPoint = PickDestPointDownScreen();
            ForceMoveToward(currDestPoint);
        }

        //Pick next point downscreen modelled after sin wave
        private Vector2 PickDestPointDownScreen()
        {
            float x = (float) (200 * Math.Sin((character.pos.Y/250f) * Math.PI));
            return new Vector2(character.pos.X + x + character.sprite.size.X/2, character.pos.Y + pointYInc + character.sprite.size.Y/2);
        }

        public bool PlayerWithinRightArmReach()
        {
            Character player = Game1.GetPlayerCharacter();
            Volvagia vol = ((Volvagia) character);
            float playerMidX = player.pos.X + player.sprite.size.X / 2;
            float playerMidY = player.pos.Y + player.sprite.size.Y / 2;

            //If within X bounds check y bounds
            if (playerMidX - vol.rightArm.pos.X > 0 &&
                playerMidX < vol.rightArm.pos.X + vol.rightArm.sprite.size.X)
            {
                if (playerMidY - vol.rightArm.pos.Y > 0 &&
                    playerMidY < vol.rightArm.pos.Y + vol.rightArm.sprite.size.Y)
                    return true;
            }

            return false;
        }

        public bool PlayerWithinLeftArmReach()
        {
            Character player = Game1.GetPlayerCharacter();
            Volvagia vol = ((Volvagia)character);
            float playerMidX = player.pos.X + player.sprite.size.X / 2;
            float playerMidY = player.pos.Y + player.sprite.size.Y / 2;

            //If within X bounds check y bounds
            if (playerMidX - vol.leftArm.pos.X > 0 &&
                playerMidX < vol.leftArm.pos.X + vol.leftArm.sprite.size.X)
            {
                if (playerMidY - vol.leftArm.pos.Y > 0 &&
                    playerMidY < vol.leftArm.pos.Y + vol.leftArm.sprite.size.Y)
                    return true;
            }

            return false;
        }

        private void MoveSegments()
        {
            Volvagia vol = (Volvagia)character;

            //Only need to make segments after the first one follow
            //the first one is anchored to the boss
            for (int i = 1; i < vol.segments.Count; i++)
            {
                ForceMoveToward(vol.segments[i], vol.segments[i - 1], 4);
            }
        }


        public override void DetermineFacingDirection(Vector2 direction)
        {
            if (!fireBreathStarted)
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
