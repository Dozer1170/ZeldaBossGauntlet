using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZeldaBossGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static string GAME_START = "\"Hi Link, four monsters \n have stolen my favorite\n places to relax! \n Please defeat them for your \n grandma!\"";
        public static string WON_TEXT = "\"Thank you Link! That \n was very nice of you. \n You have saved Hyrule again!";
        public static string GAME_OVER = "Game Over";
        public static string CONTINUE_TEXT = "Press Attack To Continue";
        public SpriteFont font;

        GraphicsDeviceManager graphics;
        HeartDisplay heartDisplay;
        bool gameOverStarted;
        int gameOverTimeTillStop;
        int timer;

        bool showedWinText;

        public static SpriteBatch spriteBatch;
        public static GameState gameState;
        public static CharacterManager characterManager;
        public static PickupManager pickupManager;
        public static BackgroundManager backgroundManager;
        public static SoundManager soundManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            showedWinText = false;
            gameOverTimeTillStop = 2000;

            font = Content.Load<SpriteFont>("font/SpriteFont1");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundManager = new BackgroundManager(this, spriteBatch, graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
            characterManager = new CharacterManager(this, spriteBatch);
            pickupManager = new PickupManager(this, spriteBatch);
            soundManager = new SoundManager(this);
            heartDisplay = new HeartDisplay(this, spriteBatch);
            gameOverStarted = false;

            //Add background manager first so it is drawn first
            Components.Add(backgroundManager);
            Components.Add(characterManager);
            Components.Add(pickupManager);
            Components.Add(heartDisplay);
            Components.Add(soundManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            backgroundManager.TransitionToMap(BackgroundManager.OVERWORLD_INDEX);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameState.STARTING)
            {
                GameStartLogic();
            }
            else  if (GameState.WON && !showedWinText)
            {
                WonTextLogic();
            }
            else
            {
                if (!GameState.GAME_OVER)
                {
                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();

                    backgroundManager.SetScreenDimensions(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

                    base.Update(gameTime);

                    if (!GameState.WON)
                    {
                        CheckCloseToGrandmaAndWin();
                    }

                }
                else
                {
                    GameOverLogic(gameTime);
                }
            }
        }

        public void CheckCloseToGrandmaAndWin()
        {
            if (!GameState.WON && backgroundManager.currentBackgroundIndex == BackgroundManager.OVERWORLD_INDEX 
                && characterManager.CloseToGrandmaAndWin())
            {
                GameState.WON = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            base.Draw(gameTime);
            if (GameState.STARTING)
            {
                DrawGameStartStrings();
            }
            if (gameOverStarted)
            {
                DrawGameOverStrings();
            }
            if (GameState.WON && !showedWinText)
            {
                DrawWonStrings();
            }
            spriteBatch.End();
        }

        public void DrawGameStartStrings()
        {
            spriteBatch.DrawString(font, GAME_START,
                   new Vector2((graphics.PreferredBackBufferWidth / 2) - 150, (graphics.PreferredBackBufferHeight / 2) + 100), Color.White,
                   0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, CONTINUE_TEXT,
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - 130, (graphics.PreferredBackBufferHeight / 2)), Color.White,
                    0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
        }

        public void DrawWonStrings()
        {
            spriteBatch.DrawString(font, WON_TEXT,
                   new Vector2((graphics.PreferredBackBufferWidth / 2) - 150, (graphics.PreferredBackBufferHeight / 2) + 100), Color.White,
                   0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, CONTINUE_TEXT,
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - 130, (graphics.PreferredBackBufferHeight / 2)), Color.White,
                    0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
        }

        public void DrawGameOverStrings()
        {
            spriteBatch.DrawString(font, GAME_OVER,
                   new Vector2((graphics.PreferredBackBufferWidth / 2) - 75, (graphics.PreferredBackBufferHeight / 2) - 50), Color.White,
                   0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            if(timer > gameOverTimeTillStop)
                spriteBatch.DrawString(font, CONTINUE_TEXT,
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - 195, (graphics.PreferredBackBufferHeight / 2)), Color.White,
                    0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
        }

        public void GameStartLogic()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.J))
            {
                GameState.STARTING = false;
            }
        }

        public void WonTextLogic()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.J))
            {
                showedWinText = true;
            }
        }

        public void GameOverLogic(GameTime gameTime)
        {
            if (!gameOverStarted)
            {
                gameOverStarted = true;
                timer = gameTime.ElapsedGameTime.Milliseconds;
                characterManager.GameOverActions();
            }
            else
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer < gameOverTimeTillStop)
                {
                    base.Update(gameTime);
                }
                else
                {
                    KeyboardState state = Keyboard.GetState();

                    if (state.IsKeyDown(Keys.J))
                    {
                        ContinueGame();
                    }
                }
            }
        }

        public void ContinueGame()
        {
            int maxHealth = GetPlayerCharacter().maxHealth;

            gameOverStarted = false;
            GameState.GAME_OVER = false;
            characterManager.Initialize();
            GetPlayerCharacter().maxHealth = maxHealth;
            GetPlayerCharacter().health = maxHealth;
            backgroundManager.TransitionToMap(BackgroundManager.OVERWORLD_INDEX);
        }

        public static CharacterManager GetCharacterManager()
        {
            return characterManager;
        }

        public static Character GetPlayerCharacter()
        {
            if (characterManager.GetCharacterList().Count > 0)
                return characterManager.GetCharacterList()[0];
            else
                return null;
        }

        public static Background GetActiveBackground()
        {
            return backgroundManager.activeBackground;
        }

        public static void SpawnHeartContainer(Vector2 position)
        {
            pickupManager.SpawnHeartContainer(position);
        }
    }
}
