using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public abstract class BossCharacter : AnimatedCharacter
    {

        public Vector2 heartPieceSpawnPosition;

        public BossCharacter(Sprite sprite, Vector2 worldPos)
            : base(sprite, worldPos)
        {
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if (invinciblityFrames < 1)
                Game1.soundManager.PlayCue(SoundManager.BOSS_DAMAGE);
            base.TakeDamage(attack, damage);
        }

        public override void HandleCollision(Character characterCollided, bool atFault, Vector2 prevPosition)
        {
            //base.HandleCollision(atFault, prevPosition);
        }

        public override void HandleDeath()
        {
            base.HandleDeath();
            Game1.soundManager.PlayMusicNoLoop(Game1.soundManager.defeatBoss);
            Game1.SpawnHeartContainer(heartPieceSpawnPosition);
        }
    }
}
