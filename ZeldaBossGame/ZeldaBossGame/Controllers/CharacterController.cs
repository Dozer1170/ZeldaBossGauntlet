using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public abstract class CharacterController
    {
        protected Character character;

        public CharacterController(Character controlledCharacter)
        {
            character = controlledCharacter;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Attack();

        public abstract void TookHit();
    }
}
