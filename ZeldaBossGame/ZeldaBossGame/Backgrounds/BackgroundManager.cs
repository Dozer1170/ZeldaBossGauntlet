using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class BackgroundManager : DrawableGameComponent
    {
        public static int OVERWORLD_INDEX = 0;
        public static int SWAMP_DUNGEON_INDEX = 1;
        public static int DESERT_DUNGEON_INDEX = 2;
        public static int WITCH_HUT_DUNGEON_INDEX = 3;
        public static int DEATH_MOUNTAIN_DUNGEON_INDEX = 4;

        public int currentBackgroundIndex;
        public Vector2 defaultOverworldPosition;

        public Vector2 swampDungeonEntrancePosition;
        public Vector2 swampExitPosition;

        public Vector2 desertDungeonEntrancePosition;
        public Vector2 desertExitPosition;

        public Vector2 witchHutDungeonEntrancePosition;
        public Vector2 witchHutExitPosition;

        public Vector2 deathMountainDungeonEntrancePosition;
        public Vector2 deathMountainExitPosition;

        SpriteBatch spriteBatch;
        public List<Background> backgrounds;
        public Background activeBackground;
        public int screenWidth, screenHeight;

        public BackgroundManager(Game game, SpriteBatch spriteBatch,
            int screenWidth, int screenHeight) : base(game)
        {
            this.backgrounds = new List<Background>();
            this.spriteBatch = spriteBatch;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            currentBackgroundIndex = 0;
            defaultOverworldPosition = new Vector2(1000, 1000);

            swampDungeonEntrancePosition = new Vector2(345, 450);
            swampExitPosition = new Vector2(1725, 993);

            desertDungeonEntrancePosition = new Vector2(408, 580);
            desertExitPosition = new Vector2(130, 735);

            witchHutDungeonEntrancePosition = new Vector2(350, 450);
            witchHutExitPosition = new Vector2(130, 280);

            deathMountainDungeonEntrancePosition = new Vector2(719, 1351);
            deathMountainExitPosition = new Vector2(1746, 284);

            Sprite overworldSprite = new Sprite(Game.Content.Load<Texture2D>("backgrounds/overworld"),
                new Vector2(0, 0), new Vector2(2048, 2048));
            Color[] overworldHitmap = new Color[2048 * 2048];
            Game.Content.Load<Texture2D>("backgrounds/overworldhitmap").GetData(overworldHitmap);
            Color[,] overworldHitmap2D = new Color[2048, 2048];
            for (int x = 0; x < 2048; x++)
                for (int y = 0; y < 2048; y++)
                    overworldHitmap2D[x, y] = overworldHitmap[x + y * 2048];

            Background overworld = new Background(overworldSprite, overworldHitmap2D);

            Sprite swampSprite = new Sprite(Game.Content.Load<Texture2D>("backgrounds/swampdungeon"), new Vector2(0,0), new Vector2(800,600));
            Color[] swampHitmap = new Color[800 * 600];
            Game.Content.Load<Texture2D>("backgrounds/swampdungeonhitmap").GetData(swampHitmap);
            Color[,] swampHitmap2D = new Color[800, 600];
            for (int x = 0; x < 800; x++)
                for (int y = 0; y < 600; y++)
                    swampHitmap2D[x, y] = swampHitmap[x + y * 800];

            Background swampBackground = new Background(swampSprite, swampHitmap2D);

            Sprite desertSprite = new Sprite(Game.Content.Load<Texture2D>("backgrounds/desertdungeon"), new Vector2(0, 0), new Vector2(942, 822));
            Color[] desertHitmap = new Color[942 * 822];
            Game.Content.Load<Texture2D>("backgrounds/desertdungeonhitmap").GetData(desertHitmap);
            Color[,] desertHitmap2D = new Color[942, 822];
            for (int x = 0; x < 942; x++)
                for (int y = 0; y < 822; y++)
                    desertHitmap2D[x, y] = desertHitmap[x + y * 942];

            Background desertBackground = new Background(desertSprite, desertHitmap2D);

            Sprite witchHutSprite = new Sprite(Game.Content.Load<Texture2D>("backgrounds/witchshut"), new Vector2(0, 0), new Vector2(800, 600));
            Color[] witchHutHitmap = new Color[800 * 600];
            Game.Content.Load<Texture2D>("backgrounds/witchshuthitmap").GetData(witchHutHitmap);
            Color[,] witchHutHitmap2D = new Color[800, 600];
            for (int x = 0; x < 800; x++)
                for (int y = 0; y < 600; y++)
                    witchHutHitmap2D[x, y] = witchHutHitmap[x + y * 800];

            Background witchHutBackground = new Background(witchHutSprite, witchHutHitmap2D);

            //Brazier fires for witch's hut
            Texture2D fireTexture = Game.Content.Load<Texture2D>("sprites/firesprite");
            SpriteAnimation fire = new SpriteAnimation(new Point(0,0), new Point(24, 36), 2, 2, true, "fire");
            witchHutBackground.decorations.Add(new DecorativeSprite(fireTexture, new Vector2(195, 146), new Vector2(24, 36), fire, witchHutBackground));
            witchHutBackground.decorations.Add(new DecorativeSprite(fireTexture, new Vector2(195, 370), new Vector2(24, 36), fire, witchHutBackground));
            witchHutBackground.decorations.Add(new DecorativeSprite(fireTexture, new Vector2(580, 370), new Vector2(24, 36), fire, witchHutBackground));
            witchHutBackground.decorations.Add(new DecorativeSprite(fireTexture, new Vector2(580, 146), new Vector2(24, 36), fire, witchHutBackground));

            Sprite deathMountainSprite = new Sprite(Game.Content.Load<Texture2D>("backgrounds/deathmountaindungeon"), new Vector2(0, 0), new Vector2(1536, 1536));

            Color[] deathMountainHitmap = new Color[1536 * 1536];
            Game.Content.Load<Texture2D>("backgrounds/deathmountaindungeonhitmap").GetData(deathMountainHitmap);
            Color[,] deathMountainHitmap2D = new Color[1536, 1536];
            for (int x = 0; x < 1536; x++)
                for (int y = 0; y < 1536; y++)
                    deathMountainHitmap2D[x, y] = deathMountainHitmap[x + y * 1536];

            Background deathMountainBackground = new Background(deathMountainSprite, deathMountainHitmap2D);

            backgrounds.Add(overworld);
            backgrounds.Add(swampBackground);
            backgrounds.Add(desertBackground);
            backgrounds.Add(witchHutBackground);
            backgrounds.Add(deathMountainBackground);
            SetActiveBackground(overworld);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            activeBackground.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            activeBackground.Draw(spriteBatch);
        }

        public void TransitionToMap(int index)
        {
            int oldBackgroundIndex = currentBackgroundIndex;
            currentBackgroundIndex = index;
            SetActiveBackground(backgrounds[index]);
            Game1.characterManager.ScheduleRemoveAllButMainCharacter();
            if (index == OVERWORLD_INDEX)
            {
                if (oldBackgroundIndex == 0)
                    Game1.GetPlayerCharacter().UpdatePosition(defaultOverworldPosition);
                else if (oldBackgroundIndex == SWAMP_DUNGEON_INDEX)
                    Game1.GetPlayerCharacter().UpdatePosition(swampExitPosition);
                else if (oldBackgroundIndex == DESERT_DUNGEON_INDEX)
                    Game1.GetPlayerCharacter().UpdatePosition(desertExitPosition);
                else if (oldBackgroundIndex == WITCH_HUT_DUNGEON_INDEX)
                    Game1.GetPlayerCharacter().UpdatePosition(witchHutExitPosition);
                else if (oldBackgroundIndex == DEATH_MOUNTAIN_DUNGEON_INDEX)
                    Game1.GetPlayerCharacter().UpdatePosition(deathMountainExitPosition);

                Game1.characterManager.SpawnLeever(new Vector2(459, 990));
                Game1.characterManager.SpawnLeever(new Vector2(588, 1671));
                Game1.characterManager.SpawnLeever(new Vector2(81, 1632));
                Game1.characterManager.SpawnLeever(new Vector2(645, 1887));
                Game1.characterManager.SpawnLeever(new Vector2(549, 1946));
                Game1.characterManager.ScheduleAddCharacter(Game1.characterManager.grandma);
                Game1.soundManager.PlayMusic(Game1.soundManager.overworldSong);
            }
            else if (index == SWAMP_DUNGEON_INDEX)
            {
                Game1.GetPlayerCharacter().UpdatePosition(swampDungeonEntrancePosition);
                if (!GameState.AGAHNIM_DEFEATED)
                {
                    Game1.soundManager.PlayMusic(Game1.soundManager.dungeonSongOne);
                    Game1.characterManager.agahnim.UpdatePosition(Game1.characterManager.agahnimSpawnPosition);
                    Game1.characterManager.ScheduleAddCharacter(Game1.characterManager.agahnim);
                }
                else
                {
                    Game1.soundManager.StopMusic();
                }
            }
            else if (index == DEATH_MOUNTAIN_DUNGEON_INDEX)
            {
               Game1.GetPlayerCharacter().UpdatePosition(deathMountainDungeonEntrancePosition);
               if (!GameState.VOLVAGIA_DEFEATED)
               {
                   Game1.soundManager.PlayMusic(Game1.soundManager.dungeonSongTwo);
                   Game1.characterManager.volvagia.InitArms();
                   Game1.characterManager.volvagia.UpdatePosition(Game1.characterManager.volvagiaSpawnPosition);
                   Game1.characterManager.ScheduleAddCharacter(Game1.characterManager.volvagia);
               }
               else
               {
                   Game1.soundManager.StopMusic();
               }
            }
            else if (index == DESERT_DUNGEON_INDEX)
            {
                Game1.GetPlayerCharacter().UpdatePosition(desertDungeonEntrancePosition);
                if (!GameState.VITREOUS_DEFEATED)
                {
                    Game1.soundManager.PlayMusic(Game1.soundManager.dungeonSongTwo);
                    Game1.characterManager.agahnim.UpdatePosition(Game1.characterManager.vitreousSpawnPosition);
                    Game1.characterManager.ScheduleAddCharacter(Game1.characterManager.vitreous);
                }
                else
                {
                    Game1.soundManager.StopMusic();
                }
            }
            else if (index == WITCH_HUT_DUNGEON_INDEX)
            {
               Game1.GetPlayerCharacter().UpdatePosition(witchHutDungeonEntrancePosition);
               if (!GameState.TWIN_ROVA_DEFEATED)
               {
                   Game1.soundManager.PlayMusic(Game1.soundManager.dungeonSongOne);
                   Game1.characterManager.twinRova.UpdatePosition(Game1.characterManager.twinRovaSpawnPosition);
                   Game1.characterManager.ScheduleAddCharacter(Game1.characterManager.twinRova);
               }
               else
               {
                   Game1.soundManager.StopMusic();
               }
            }

            Game1.GetPlayerCharacter().boundingShapes.UpdatePosition(Game1.GetPlayerCharacter().pos);
        }

        public void CenterOnPos(Vector2 pos)
        {
            activeBackground.MoveTo(new Vector2(pos.X - screenWidth / 2, pos.Y - screenHeight / 2),
                screenWidth, screenHeight);
        }

        public void SetActiveBackground(Background background)
        {
            this.activeBackground = background;
        }

        public void SetScreenDimensions(int w, int h)
        {
            screenWidth = w;
            screenHeight = h;
        }
    }
}
