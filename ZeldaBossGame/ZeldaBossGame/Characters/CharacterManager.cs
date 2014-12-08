using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class CharacterManager : DrawableGameComponent
    {
        private List<Character> charsToRemove;
        private List<Character> charsToAdd;

        public SpriteBatch spriteBatch;
        private List<Character> charList;

        public AnimatedCharacter grandma;

        public Agahnim agahnim;
        public Vector2 agahnimSpawnPosition;

        public Vitreous vitreous;
        public Vector2 vitreousSpawnPosition;

        public Volvagia volvagia;
        public Vector2 volvagiaSpawnPosition;

        public TwinRova twinRova;
        public Vector2 twinRovaSpawnPosition;

        bool needRemoveAll, needRemove, needAdd;

        public CharacterManager(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {
            base.Initialize();

            charList = new List<Character>();
            charsToAdd = new List<Character>();
            charsToRemove = new List<Character>();
            needRemoveAll = false;
            needRemove = false;
            needAdd = false;

            Character link = new Link(new Sprite(Game.Content.Load<Texture2D>("sprites/linkspritesheet"), new Vector2(0, 0), new Vector2(128,128)), new Vector2(1000,1000));
            link.controller = new PlayerController(link);

            SpriteAnimation grandmaStand = new SpriteAnimation(new Point(0, 0), new Point(128, 128), 3, 1, true, "grandmastand");
            grandma = new AnimatedCharacter(new Sprite(Game.Content.Load<Texture2D>("sprites/grandmaspritesheet"), new Vector2(0,0), new Vector2(128,128), grandmaStand),
                new Vector2(1000, 960));

            agahnimSpawnPosition = new Vector2(340, 124);
            agahnim = new Agahnim(new Sprite(Game.Content.Load<Texture2D>("sprites/agahnimspritesheet"), new Vector2(0, 0), new Vector2(128, 128)), agahnimSpawnPosition);
            agahnim.controller = new AgahnimController(agahnim);

            vitreousSpawnPosition = new Vector2(410,300);
            vitreous = new Vitreous(new Sprite(Game.Content.Load<Texture2D>("sprites/vitreousspritesheet"), new Vector2(0, 0), new Vector2(128, 128)), vitreousSpawnPosition);
            vitreous.controller = new VitreousController(vitreous);

            volvagiaSpawnPosition = new Vector2(900, -256);
            volvagia = new Volvagia(new Sprite(Game.Content.Load<Texture2D>("sprites/volvagiaspritesheet"),
                new Vector2(0, 0), new Vector2(128, 256)), volvagiaSpawnPosition, 1.0f);
            volvagia.controller = new VolvagiaController(volvagia);

            twinRovaSpawnPosition = new Vector2(353, 190);
            twinRova = new TwinRova(new Sprite(Game.Content.Load<Texture2D>("sprites/twinrovaspritesheet"),
                new Vector2(0, 0), new Vector2(128, 128)), twinRovaSpawnPosition);
            twinRova.controller = new TwinRovaController(twinRova);

            charList.Add(link);
            charList.Add(grandma);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (needRemoveAll)
            {
                RemoveAllButMainCharacter();
                needRemoveAll = false;
            }

            if (needRemove)
            {
                foreach (Character character in charsToRemove)
                    charList.Remove(character);

                needRemove = false;
                charsToRemove.RemoveRange(0, charsToRemove.Count);
            }

            if (needAdd)
            {
                foreach (Character character in charsToAdd)
                    charList.Add(character);

                needAdd = false;
                charsToAdd.RemoveRange(0, charsToAdd.Count);
            }

            for (int i = 0; i < charList.Count; i++ )
            {
                Character character = charList[i];
                character.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (Character character in charList)
            {
                character.Draw(spriteBatch);
            }
        }

        //Schedule removal of all but main character at beginning of next frame
        //This avoids modification of charList while it is being iterated over
        public void ScheduleRemoveAllButMainCharacter()
        {
            needRemoveAll = true;
        }

        private void RemoveAllButMainCharacter()
        {
            charList.RemoveRange(1, charList.Count - 1);
        }

        //Schedule removal of character at beginning of next frame
        //This avoids modification of charList while it is being iterated over
        public void ScheduleRemoveCharacter(Character character)
        {
            charsToRemove.Add(character);
            needRemove = true;
        }

        //Schedule add of character at beginning of next frame
        //This avoids modification of charList while it is being iterated over
        public void ScheduleAddCharacter(Character character)
        {
            charsToAdd.Add(character);
            needAdd = true;
        }

        public void GameOverActions()
        {
            foreach (Character character in charList)
            {
                character.GameOver();
            }
        }

        public Character CheckHitBoxCollision(Character attackOwner, BoundingShapes hitBox)
        {
            foreach (Character character in charList)
            {
                if (!character.Equals(attackOwner) && hitBox.CheckCollision(character.boundingShapes))
                {
                    return character;
                }
            }

            return null;
        }

        public List<Character> GetCharacterList()
        {
            return charList;
        }

        public void SpawnLeever(Vector2 pos)
        {
            Leever leever = new Leever(new Sprite(Game.Content.Load<Texture2D>("sprites/leeverspritesheet"), new Vector2(0, 0), new Vector2(40, 60)), pos);

            ScheduleAddCharacter(leever);
        }

        public bool CloseToGrandmaAndWin()
        {
            Character player = Game1.GetPlayerCharacter();

            int xDiff = (int) Math.Abs(player.pos.X - grandma.pos.X);
            int yDiff = (int) Math.Abs(player.pos.Y - grandma.pos.Y);

            if (xDiff < 50 && yDiff < 50 && GameState.AGAHNIM_DEFEATED && GameState.TWIN_ROVA_DEFEATED
                && GameState.VITREOUS_DEFEATED && GameState.VOLVAGIA_DEFEATED)
                return true;
            else
                return false;
        }
    }
}
