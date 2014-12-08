using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    class DecorativeSprite : Sprite
    {

        Vector2 worldPos;
        Background parent;

        public DecorativeSprite(Texture2D t, Vector2 worldPos, Vector2 s, SpriteAnimation f, Background parent)
            : base(t, new Vector2(0, 0), s, f)
        {
            this.parent = parent;
            UpdatePosition(worldPos);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            UpdatePosition(worldPos);
            base.Draw(batch);
        }

        public void UpdatePosition(Vector2 nPos)
        {
            worldPos = nPos;
            pos = worldPos - parent.topLeft;
        }
    }
}
