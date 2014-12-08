using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public abstract class Pickup
    {
        public Sprite sprite;
        public Vector2 pos;
        public BoundingShapes boundingShapes;
        public bool wasPickedUp;

        public Pickup(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.pos = position;
            wasPickedUp = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdatePosition(pos);
            foreach (Character character in Game1.characterManager.GetCharacterList())
            {
                if (!wasPickedUp && character.boundingShapes.CheckCollision(boundingShapes))
                {
                    DoPickupAction(character);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public abstract void DoPickupAction(Character character);

        public virtual void UpdatePosition(Vector2 nPos)
        {
            pos = nPos;
            sprite.pos = pos - Game1.GetActiveBackground().topLeft;
        }

        public void SetBoundingShapes(BoundingShapes shapes)
        {
            this.boundingShapes = shapes;
        }
    }
}
