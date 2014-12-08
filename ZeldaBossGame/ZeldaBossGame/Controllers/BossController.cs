using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaBossGame
{
    class BossController : AIController
    {
        public int currentPhase;

        public BossController(Character character)
            : base(character)
        {
            currentPhase = 1;
        }
    }
}
