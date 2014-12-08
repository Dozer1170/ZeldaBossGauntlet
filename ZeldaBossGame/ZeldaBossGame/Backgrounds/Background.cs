using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class Background
    {
        public Color[,] hitmap;
        public List<Sprite> decorations;
        public Sprite sprite;
        public Vector2 topLeft;
        public Vector2 size;

        public Background(Sprite sprite) : this(sprite,null)
        {
        }

        public Background(Sprite sprite, Color[,] hitmap)
        {
            this.sprite = sprite;
            this.hitmap = hitmap;
            topLeft = new Vector2(sprite.currAnim.currFrame.Left, sprite.currAnim.currFrame.Top);
            size = sprite.size;
            sprite.layerDepth = 0;
            decorations = new List<Sprite>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite decSprite in decorations)
            {
                decSprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);

            foreach (Sprite decSprite in decorations)
                decSprite.Draw(spriteBatch);
        }

        public void MoveTo(Vector2 position, int windowWidth, int windowHeight)
        {
            if (position.X < 0)
                position.X = 0;
            else if (position.X + windowWidth > size.X)
                position.X = size.X - windowWidth;

            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y + windowHeight > size.Y)
                position.Y = size.Y - windowHeight;

            topLeft = position;
            sprite.MoveCurrentFrameTo(new Point((int) topLeft.X, (int) topLeft.Y));
        }
    }
}
