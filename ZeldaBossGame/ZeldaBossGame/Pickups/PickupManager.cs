using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    public class PickupManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private List<Pickup> pickups;

        private List<Pickup> pickupsToRemove;
        private List<Pickup> pickupsToAdd;

        private Pickup heartContainer;

        bool needRemoveAll, needRemove, needAdd;

        public PickupManager(Game1 game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {
            base.Initialize();

            pickups = new List<Pickup>();
            pickupsToAdd = new List<Pickup>();
            pickupsToRemove = new List<Pickup>();

            heartContainer = new HeartContainer(new Sprite(Game.Content.Load<Texture2D>("sprites/bossheartcontainer"), new Vector2(0, 0), new Vector2(36, 32)), new Vector2(0, 0));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (needRemoveAll)
            {
                RemoveAll();
                needRemoveAll = false;
            }

            if (needRemove)
            {
                foreach (Pickup pickup in pickupsToRemove)
                    pickups.Remove(pickup);

                needRemove = false;
                pickupsToRemove.RemoveRange(0, pickupsToRemove.Count);
            }

            if (needAdd)
            {
                foreach (Pickup pickup in pickupsToAdd)
                    pickups.Add(pickup);

                needRemove = false;
                pickupsToAdd.RemoveRange(0, pickupsToAdd.Count);
            }

            for (int i = 0; i < pickups.Count; i++)
            {
                Pickup pickup = pickups[i];
                pickup.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (Pickup pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
        }

        public void ScheduleRemoveAll()
        {
            needRemoveAll = true;
        }

        private void RemoveAll()
        {
            pickups.RemoveRange(0, pickups.Count);
        }

        public void ScheduleRemovePickup(Pickup pickup)
        {
            pickupsToRemove.Add(pickup);
            needRemove = true;
        }

        public void ScheduleAddPickup(Pickup pickup)
        {
            pickupsToAdd.Add(pickup);
            needAdd = true;
        }

        public void SpawnHeartContainer(Vector2 position)
        {
            heartContainer.UpdatePosition(position - heartContainer.sprite.size / 2);
            heartContainer.boundingShapes.UpdatePosition(heartContainer.pos);
            heartContainer.wasPickedUp = false;
            ScheduleAddPickup(heartContainer);
        }
    }
}
