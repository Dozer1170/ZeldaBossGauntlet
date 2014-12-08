using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    class ChaseOnlyAIController : AIController
    {
        public ChaseOnlyAIController(Character character)
            : base(character)
        {

        }

        public override void Update(GameTime gameTime)
        {
            character.UpdatePosition(character.pos);
            MoveToward(character, Game1.GetPlayerCharacter());
        }
    }
}
