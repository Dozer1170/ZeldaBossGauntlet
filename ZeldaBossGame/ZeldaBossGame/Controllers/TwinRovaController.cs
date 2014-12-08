using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class TwinRovaController : BossController
    {
        int lineTimer, lineWaitTime;
        int lineDurationTimer, lineDuration;
        bool throwDagger;
        int numDaggersToThrow, daggersThrown;
        int daggerTimer, daggerCooldown;
        Vector2 centerScreen;

        public TwinRovaController(Character character)
            : base(character)
        {
            throwDagger = true;
            numDaggersToThrow = 4;
            daggersThrown = 0;
            daggerTimer = 0;
            daggerCooldown = 1000;
            lineTimer = 0;
            lineWaitTime = 1000;
            lineDurationTimer = 0;
            lineDuration = 5000;
            centerScreen = new Vector2(416, 286);
        }

        public override void Update(GameTime gameTime)
        {
            if (throwDagger)
            {
                DoThrowDaggerLogic(gameTime);
            }
            else
            {
                DoBigSpellLogic(gameTime);
            }
        }

        public void DoThrowDaggerLogic(GameTime gameTime)
        {
            Character player = Game1.GetPlayerCharacter();

            //Move towards players x and top of screen
            if (ForceMoveToward(new Vector2(player.pos.X + player.sprite.size.X / 2, 50)))
            {

                if (daggerTimer < daggerCooldown)
                {
                    daggerTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    daggerTimer = gameTime.ElapsedGameTime.Milliseconds; 
                    character.Attack(((TwinRova)character).iceDagger);
                    daggersThrown++;
                    if (daggersThrown > numDaggersToThrow - 1)
                    {
                        daggersThrown = 0;
                        throwDagger = false;
                    }
                }
            }
        }

        public void DoBigSpellLogic(GameTime gameTime)
        {
            if (ForceMoveToward(centerScreen))
            {
                if (lineDurationTimer < 1)
                {
                    if (lineTimer < lineWaitTime)
                    {
                        lineTimer += gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        lineDurationTimer = gameTime.ElapsedGameTime.Milliseconds;
                        character.Attack();
                    }
                }
                else
                {
                    if (lineDurationTimer < lineDuration)
                    {
                        lineDurationTimer += gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        ((TwinRova)character).ClearProjectiles();
                        lineDurationTimer = 0;
                        lineTimer = 0;
                        throwDagger = true;
                    }
                }
            }
        }
    }
}
