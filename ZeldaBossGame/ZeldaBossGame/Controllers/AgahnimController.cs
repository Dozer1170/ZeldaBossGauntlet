using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class AgahnimController : BossController
    {
        int chasingMaxTime;
        bool startedIceBurst;
        bool startedLightningFork;
        bool startupLightningFork;
        int iceBurstWaitTime;
        int lightningForkStartup;
        int lightningForkWaitTime;
        int timer;
        bool justEnteredPhase;
        Vector2 phase2Start;
        int topYRow;
        

        public AgahnimController(Character otherCharacter)
            : base(otherCharacter)
        {
            startedIceBurst = false;
            startedLightningFork = false;
            startupLightningFork = false;
            iceBurstWaitTime = 1000;
            chasingMaxTime = 3500;
            lightningForkStartup = 700;
            lightningForkWaitTime = 1200;
            phase2Start = new Vector2(328, 41);
            justEnteredPhase = false;
            topYRow = 30;
        }

        public override void Update(GameTime gameTime)
        {
            //Update sprite pos on screen
            character.UpdatePosition(character.pos);
            //Logic for phases
            if (currentPhase == 1)
            {
                PhaseOneLogic(gameTime);
                if (character.health < character.maxHealth/2)
                {
                    currentPhase = 2;
                    justEnteredPhase = true;
                }
            }
            else
            {
                PhaseTwoLogic(gameTime);
            }
        }

        public override void TookHit()
        {
            base.TookHit();
            if (chasing)
                chasing = false;
        }

        public void IceBallAttack(GameTime gameTime)
        {
            if (!startedIceBurst)
            {
                if (!character.attacking)
                {
                    character.Attack(Agahnim.ICE_BURST);
                }
                timer = gameTime.ElapsedGameTime.Milliseconds;
                startedIceBurst = true;
            }
            else
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > iceBurstWaitTime)
                {
                    ((Agahnim)character).projectilesActive = false;
                    startedIceBurst = false;
                    chasing = true;
                    timer = gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void LightningForkAttack(GameTime gameTime)
        {
            if (!startupLightningFork)
            {
                startupLightningFork = true;
                timer = gameTime.ElapsedGameTime.Milliseconds;
                ((AnimatedCharacter)character).PlayAnimation(Agahnim.LIGHTNING_FORK_CAST_ANIM_NAME);
            }
            else
            {
                if (timer < lightningForkStartup)
                {
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (!startedLightningFork)
                    {
                        timer = gameTime.ElapsedGameTime.Milliseconds;
                        startedLightningFork = true;
                        character.Attack(Agahnim.LIGHTNING_FORK);
                    }

                    timer += gameTime.ElapsedGameTime.Milliseconds;

                    if (timer > lightningForkWaitTime)
                    {
                        startedLightningFork = false;
                        startupLightningFork = false;
                    }
                }
            }
        }

        //Move toward point above character to electrocute
        public void PhaseOneLogic(GameTime gameTime)
        {
            if (chasing)
            {
                if (timer < chasingMaxTime)
                {
                    Character player = Game1.GetPlayerCharacter();
                    Vector2 goalPos = new Vector2(player.pos.X + player.sprite.size.X / 2,
                        player.pos.Y + 10);
                    if (MoveToward(goalPos))
                    {
                        Attack();
                    }
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    chasing = false;
                }
            }
            else
            {
                if (MoveToward(Game1.GetActiveBackground().size / 2))
                {
                    IceBallAttack(gameTime);
                }
            }
        }

        public void PhaseTwoLogic(GameTime gameTime)
        {
            Character player = Game1.GetPlayerCharacter();
            //If player is in top area where Agahnim will be moving move player down
            if (player.pos.Y < topYRow)
                player.UpdatePosition(new Vector2(player.pos.X, topYRow));

            if (justEnteredPhase)
            {
                if(MoveToward(phase2Start)) 
                {
                    justEnteredPhase = false;
                }
            }
            else
            {
                if (!startupLightningFork)
                {
                    if (MoveToward(new Vector2(Game1.GetPlayerCharacter().pos.X + 
                        Game1.GetPlayerCharacter().sprite.size.X/2, phase2Start.Y)))
                        LightningForkAttack(gameTime);
                }
                else
                {
                    LightningForkAttack(gameTime);
                }
            }
        }
    }
}
