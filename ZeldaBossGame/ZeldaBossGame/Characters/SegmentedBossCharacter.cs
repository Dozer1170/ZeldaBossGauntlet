using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class SegmentedBossCharacter : BossCharacter
    {
        public List<AnimatedCharacter> segments;
        float segmentLayerDepth;

        public SegmentedBossCharacter(Sprite sprite, Vector2 worldPos, float layerDepth)
            : base(sprite, worldPos)
        {
            segments = new List<AnimatedCharacter>();
            segmentLayerDepth = 0.98f;
            this.sprite.layerDepth = layerDepth;
            InitSegments();
        }

        public virtual void InitSegments()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < segments.Count; i++)
            {
                Character segment = segments[i];
                segment.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < segments.Count; i++)
                segments[i].Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public virtual void MoveBossAndSegmentsToPoint(Vector2 point)
        {
            UpdatePosition(point);
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i].UpdatePosition(new Vector2(point.X, point.Y - (i * segments[i].sprite.size.Y / 3)));
            }
        }

        //Add segment behind last segment
        public void AddSegment(AnimatedCharacter segment)
        {
            segment.sprite.layerDepth = segmentLayerDepth;
            segment.moveSpeed = moveSpeed;
            segmentLayerDepth -= 0.001f;
            if (segments.Count > 0)
            {
                segment.UpdatePosition(new Vector2(segments[segments.Count - 1].pos.X,
                    segments[segments.Count - 1].pos.Y - segment.sprite.size.Y/3));
            }
            else
            {
                segment.UpdatePosition(new Vector2(pos.X,pos.Y + sprite.size.Y/3));
            }
            segments.Add(segment);
        }

        public void RemoveSegment(AnimatedCharacter segment) {
            segments.Remove(segment);
        }

        public override void HandleDeath()
        {
            foreach (AnimatedCharacter segment in segments)
                segment.HandleDeath();
            base.HandleDeath();
        }
    }
}
