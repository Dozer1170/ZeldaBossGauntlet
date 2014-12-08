using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    /*
     * Class for sprite display
     */ 
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 pos;
        public Vector2 size;
        public Vector2 origin;
        public Color tintColor;
        public SpriteAnimation currAnim;
        public bool flipped;
        public float rotation;
        public float layerDepth;

        public Sprite(Texture2D t, Vector2 p, Vector2 s)
        {
            this.texture = t;
            this.pos = p;
            this.size = s;
            this.flipped = false;
            origin = new Vector2(0, 0);
            rotation = 0f;
            tintColor = Color.White;
            currAnim = new SpriteAnimation(new Point(0, 0), 
                new Point((int)size.X, (int)size.Y), 1, 0, false, "default");
            layerDepth = 0.5f;
        }

        public Sprite(Texture2D t, Vector2 p, Vector2 s, SpriteAnimation f)
            : this(t, p, s)
        {
            this.currAnim = f;
        }

        /*
         * Updates currAnim
         */ 
        public virtual void Update(GameTime gameTime)
        {
            currAnim.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, pos, currAnim.currFrame, tintColor,
                rotation, origin, 1, (flipped == true) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
        }

        public void MoveCurrentFrameTo(Point position)
        {
            currAnim.MoveCurrentFrameTo(position);
        }
    }
}
