using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    class HeartDisplay : DrawableGameComponent
    {
        public SpriteBatch spriteBatch;
        public Sprite fullHeart;
        public Sprite halfHeart;
        public Sprite emptyHeart;

        public Vector2 startingDrawPosition;

        public HeartDisplay(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            startingDrawPosition = new Vector2(15, 15);
        }

        protected override void  LoadContent()
        {
            base.LoadContent();

            Texture2D heartTexture = Game.Content.Load<Texture2D>("sprites/HeartContainer");
            fullHeart = new Sprite(heartTexture, new Vector2(15, 15), new Vector2(16, 16));
            //2nd frame is half heart
            halfHeart = new Sprite(heartTexture, new Vector2(15, 15), new Vector2(16, 16));
            halfHeart.currAnim.numFrames = 3;
            halfHeart.currAnim.NextFrame();
            //3rd frame is empty heart
            emptyHeart = new Sprite(heartTexture, new Vector2(15, 15), new Vector2(16, 16));
            emptyHeart.currAnim.numFrames = 3;
            emptyHeart.currAnim.NextFrame();
            emptyHeart.currAnim.NextFrame();

            fullHeart.layerDepth = 1;
            halfHeart.layerDepth = 1;
            emptyHeart.layerDepth = 1;
        }

        public override void  Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int totalHealth = Game1.GetPlayerCharacter().maxHealth;
            int currHealth = Game1.GetPlayerCharacter().health;
            int hearts = totalHealth / 2;
            int filledHeartsToDraw = currHealth / 2;
            int halfHeartsToDraw = currHealth % 2;
            int emptyHeartsToDraw = (totalHealth - currHealth) / 2;
            Vector2 currDrawPos = startingDrawPosition;
            for (int i = 0; i < hearts; i++)
            {
                if (filledHeartsToDraw > 0)
                {
                    fullHeart.pos = currDrawPos;
                    fullHeart.Draw(spriteBatch);
                    filledHeartsToDraw--;
                }
                else if (halfHeartsToDraw > 0)
                {
                    halfHeart.pos = currDrawPos;
                    halfHeart.Draw(spriteBatch);
                    halfHeartsToDraw--;
                }
                else if (emptyHeartsToDraw > 0)
                {
                    emptyHeart.pos = currDrawPos;
                    emptyHeart.Draw(spriteBatch);
                    emptyHeartsToDraw--;
                }

                currDrawPos = new Vector2(currDrawPos.X + fullHeart.size.X + 2, currDrawPos.Y);
            }
        }
    }
}
