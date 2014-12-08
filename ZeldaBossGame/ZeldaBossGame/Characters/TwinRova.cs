using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZeldaBossGame
{
    public class TwinRova : BossCharacter
    {
        public static string FIRE_PATCH = "firepatch";
        public static string ICE_FORM = "iceform";
        public static string ICE_DAGGER = "icedagger";
        public static string BAT_FLY = "batfly";
        public static string CAST_ANIM_NAME = "cast";
        public static string FACE_LEFT_ANIM_NAME = "left";
        public static string FACE_RIGHT_ANIM_NAME = "right";

        public List<ProjectileAttack> currProjectiles;
        public HitmapProjectileAttack iceDagger;
        int numProjectilesInLine;
        Vector2 blProjStart, brProjStart, tlProjStart, trProjStart;
        bool lastLineWasFire;
        public bool iceDaggerActive;

        public TwinRova(Sprite sprite, Vector2 worldPos) : base(sprite, worldPos)
        {
        }

        public override void Initialize()
        {
            heartPieceSpawnPosition = new Vector2(416, 286);

            moveSpeed = 4;

            currProjectiles = new List<ProjectileAttack>();

            lastLineWasFire = false;

            blProjStart = new Vector2(320, 250);
            brProjStart = new Vector2(384, 250);
            tlProjStart = new Vector2(320, 200);
            trProjStart = new Vector2(384, 200);

            health = 16;
            maxHealth = 16;

            numProjectilesInLine = 7;

            sprite.layerDepth = 0.99f;

            SetBoundingShapes(new BoundingShapes(pos, new BoundingBox(new Vector3(39,41,0), new Vector3(90,99,0))));

            InitAnims();
            InitAttacks();
        }

        public override void InitAnims()
        {
            Point spriteSize = new Point(128,128);

            SpriteAnimation flyDown = new SpriteAnimation(new Point(0, 0), spriteSize, 2, 5, true, WALK_DOWN_ANIM_NAME);
            SpriteAnimation flyUp = new SpriteAnimation(new Point(256, 0), spriteSize, 2, 5, true, WALK_UP_ANIM_NAME);
            SpriteAnimation flyLeft = new SpriteAnimation(new Point(0, 256), spriteSize, 1, 0, false, FACE_LEFT_ANIM_NAME);
            SpriteAnimation flyRight = new SpriteAnimation(new Point(128, 256), spriteSize, 1, 0, false, FACE_RIGHT_ANIM_NAME);
            SpriteAnimation still = new SpriteAnimation(new Point(0, 0), spriteSize, 2, 5, true, STAND_STILL_DOWN_ANIM_NAME);
            SpriteAnimation cast = new SpriteAnimation(new Point(0, 128), spriteSize, 1, 0, false, CAST_ANIM_NAME);
            SpriteAnimation death = new SpriteAnimation(new Point(256, 256), spriteSize, 4, 4, false, DEATH_ANIM_NAME);
            SpriteAnimation iceDagger = new SpriteAnimation(new Point(768, 256), spriteSize, 1, 0, false, ICE_DAGGER);

            AddAnimation(flyDown);
            AddAnimation(flyUp);
            AddAnimation(flyLeft);
            AddAnimation(flyRight);
            AddAnimation(still);
            AddAnimation(cast);
            AddAnimation(death);
            AddAnimation(iceDagger);
        }

        public override void InitAttacks()
        {
            BoundingShapes iceShapes = new BoundingShapes(pos, new BoundingBox(new Vector3(59, 44, 0), new Vector3(74, 82, 0)));
            iceDagger = new HitmapProjectileAttack(this, iceShapes, 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), new Vector2(128, 128), animations.GetAnimation(ICE_DAGGER)), pos, new Vector2(0, 10),
                delegate() { iceDaggerActive = false; });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (ProjectileAttack proj in currProjectiles)
                proj.Update(gameTime, pos);

            if (iceDaggerActive)
                iceDagger.Update(gameTime, pos);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (ProjectileAttack proj in currProjectiles)
                proj.Draw(spriteBatch);

            if (iceDaggerActive)
                iceDagger.Draw(spriteBatch);
        }

        public void SpawnProjectiles(string animName)
        {
            SpawnFireProjectilesLine(animName, blProjStart, -32, 32);
            SpawnFireProjectilesLine(animName, brProjStart, 32, 32);
            SpawnFireProjectilesLine(animName, tlProjStart, -32, -32);
            SpawnFireProjectilesLine(animName, trProjStart, 32, -32);

            //Add extra protection during fire
            if (animName.Equals(FIRE_PATCH))
            {
                SpawnFirePatch(new Point(640, 128), new Point(128, 128),
                        new Vector2(320, 224), 8, animName);
                SpawnFirePatch(new Point(640, 128), new Point(128, 128),
                        new Vector2(382, 224), 8, animName);
                SpawnFirePatch(new Point(640, 128), new Point(128, 128),
                        new Vector2(352, 192), 8, animName);
                SpawnFirePatch(new Point(640, 128), new Point(128, 128),
                        new Vector2(352, 256), 8, animName);
            }
        }

        public void SpawnFireProjectilesLine(string animName, Vector2 startPoint, int xDiff, int yDiff)
        {
            Point animStartLoc = new Point(640, 128);
            Point spriteSize = new Point(128, 128);
            int animSpeed = 8;
            if (animName.Equals(FIRE_PATCH))
            {
                for (int i = 0; i < numProjectilesInLine; i++)
                {
                    SpawnFirePatch(animStartLoc, spriteSize, 
                        new Vector2(startPoint.X + i * xDiff, startPoint.Y + i * yDiff), animSpeed, animName);
                }
            }
            else
            {
                Vector2 vec = new Vector2(xDiff, yDiff);
                vec.Normalize();
                for (int i = 0; i < numProjectilesInLine; i++)
                {
                    IceChunk chunck = new IceChunk(new Sprite(sprite.texture, new Vector2(0, 0), sprite.size),
                        new Vector2(startPoint.X + i * xDiff, startPoint.Y + i * yDiff), new Vector2(vec.X, 0));
                    chunck.sprite.layerDepth = 0.5f;
                    Game1.characterManager.ScheduleAddCharacter(chunck);
                }
            }
        }

        private void SpawnFirePatch(Point animStartLoc, Point spriteSize, Vector2 worldPos, int animSpeed, string animName)
        {
            SpriteAnimation anim = new SpriteAnimation(animStartLoc, spriteSize, 2, animSpeed, true, animName);
            ProjectileAttack proj = new ProjectileAttack(this, new BoundingShapes(pos,
                new BoundingBox(new Vector3(48, 48, 0), new Vector3(62, 62, 0))), 1, 0, 0, 0, 0,
                new Sprite(sprite.texture, new Vector2(0, 0), sprite.size, anim),
                worldPos, new Vector2(0, 0));
            proj.sprite.layerDepth = 0.5f;
            currProjectiles.Add(proj);
        }

        public override void FaceLeft()
        {
            direction = Direction.Left;
            sprite.flipped = true;
            PlayAnimation(FACE_LEFT_ANIM_NAME);
        }

        public override void FaceRight()
        {
            direction = Direction.Right;
            sprite.flipped = false;
            PlayAnimation(FACE_RIGHT_ANIM_NAME);
        }

        public override void Attack()
        {
            attacking = true;
            PlayAnimation(CAST_ANIM_NAME);
            if (!lastLineWasFire)
            {
                SpawnProjectiles(FIRE_PATCH);
                lastLineWasFire = true;
                Game1.soundManager.PlayCue(SoundManager.TWINROVA_FIRE_GROUND);
            }
            else
            {
                SpawnProjectiles(ICE_FORM);
                lastLineWasFire = false;
                Game1.soundManager.PlayCue(SoundManager.TWINROVA_ICE_GROUND);
            }
        }

        public override void Attack(Attack attack)
        {
            attacking = true;
            if (attack.Equals(iceDagger))
            {
                iceDaggerActive = true;
                DoProjectileAttack(iceDagger);
                Game1.soundManager.PlayCue(SoundManager.FIREBALL_REFLECT);
            }
        }

        public void ClearProjectiles()
        {
            attacking = false;
            currProjectiles.RemoveRange(0, currProjectiles.Count);
        }

        public override void TakeDamage(Attack attack, int damage)
        {
            if (!(attack.attackOwner is IceChunk))
            {
                if (invinciblityFrames < 1)
                    Game1.soundManager.PlayCue(SoundManager.TWINROVA_HIT);
                base.TakeDamage(attack, damage);
            }
        }

        public override void HandleDeath()
        {
            base.HandleDeath();
            Game1.GetCharacterManager().ScheduleRemoveAllButMainCharacter();
            GameState.TWIN_ROVA_DEFEATED = true;
        }
    }
}
