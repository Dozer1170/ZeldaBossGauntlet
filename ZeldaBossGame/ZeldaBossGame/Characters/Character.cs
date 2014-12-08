using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZeldaBossGame
{
    /*
     * Basic character class
     * 
     * Holds movement, sprite information and bounding shapes
     */ 
    public class Character
    {
        public Attack activeAttack;
        public BoundingBox relativeOuterBox;
        public CharacterController controller;
        public Sprite sprite;
        public Vector2 pos;
        public Direction direction;
        public BoundingShapes boundingShapes;
        public float moveSpeed;
        public bool attacking;
        public int health;
        public int maxHealth;
        public int invinciblityFrames;
        public int invinciblityFramesAfterHit;
        public bool alive;

        public Character()
        {
            direction = Direction.Right;
            health = 2;
            maxHealth = 2;
            alive = true;
        }

        public Character(Sprite sprite, Vector2 position) : this()
        {
            this.sprite = sprite;
            UpdatePosition(position);
            invinciblityFramesAfterHit = 80;
            invinciblityFrames = 0;
        }

        public virtual void Initialize()
        {

        }

        public virtual void InitAnims()
        {

        }

        public virtual void InitAttacks()
        {

        }

        public virtual void Update(GameTime gameTime) {

            UpdatePosition(pos);

            if (alive)
            {
                if (controller != null)
                    controller.Update(gameTime);

                if (activeAttack != null)
                    activeAttack.Update(gameTime, pos);

                if (invinciblityFrames > 0)
                {
                    if (invinciblityFrames == 1)
                    {
                        sprite.tintColor = Color.White;
                    }
                    else
                    {
                        if (invinciblityFrames % 10 > 4)
                            sprite.tintColor = Color.Tomato;
                        else if (invinciblityFrames % 10 < 5)
                            sprite.tintColor = Color.DarkRed;
                    }

                    invinciblityFrames--;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        //Move the character by velocity returns color hit from hitmap
        public virtual Color Move(Vector2 velocity)
        {
            Vector2 backgroundSize = Game1.GetActiveBackground().size;
            Vector2 oldPos = pos;
            Vector2 nPos = pos + velocity;

            boundingShapes.UpdatePosition(nPos);
            UpdatePosition(nPos);

            Color hitMapColor = CheckHitMapCollision();

            if (hitMapColor.Equals(Color.Black))
            {
                nPos = oldPos;
                boundingShapes.UpdatePosition(nPos);
                UpdatePosition(nPos);
            }
                

            List<Character> charList = Game1.characterManager.GetCharacterList();
            for (int i = 0; i < charList.Count; i++)
            {
                Character character = charList[i];
                if (!this.Equals(character))
                {
                    if (CheckCollision(character))
                    {
                        HandleCollision(character, true, oldPos);
                        character.HandleCollision(character, false, character.pos);
                    }
                }
            }

            return hitMapColor;
        }

        public void ForceMove(Vector2 velocity)
        {
            Vector2 backgroundSize = Game1.GetActiveBackground().size;
            Vector2 oldPos = pos;
            Vector2 nPos = pos + velocity;

            if(boundingShapes != null)
                boundingShapes.UpdatePosition(nPos);
            UpdatePosition(nPos);
        }

        public virtual void Attack() 
        {
            
        }

        public virtual void Attack(Attack attack)
        {

        }

        public virtual void DoAttack(Attack attack)
        {
            attacking = true;
            activeAttack = attack;
            activeAttack.BeginAttack();
        }

        public virtual void DoProjectileAttack(ProjectileAttack attack)
        {
            attack.BeginAttack();
            attack.UpdatePosition(new Vector2(pos.X + sprite.size.X/2 - attack.sprite.size.X/2, pos.Y + sprite.size.Y/2));
        }

        public virtual void ClearActiveAttack()
        {
            activeAttack = null;
            attacking = false;
        }

        public virtual void TakeDamage(Attack attack, int damage)
        {
            if(controller != null)
                controller.TookHit();
            if(invinciblityFrames < 1) {
                health -= damage;
                invinciblityFrames = invinciblityFramesAfterHit;
                if (health < 1)
                    HandleDeath();
            }
        }

        public virtual void HandleDeath()
        {
            alive = false;
            Game1.characterManager.ScheduleRemoveCharacter(this);
        }

        public virtual void UpdatePosition(Vector2 nPos)
        {
            pos = nPos;
            sprite.pos = pos - Game1.GetActiveBackground().topLeft;
        }

        public virtual void FaceUp()
        {
            direction = Direction.Up;
        }

        public virtual void FaceDown()
        {
            direction = Direction.Down;
        }

        public virtual void FaceLeft()
        {
            direction = Direction.Left;
            sprite.flipped = true;
        }

        public virtual void FaceRight()
        {
            direction = Direction.Right;
            sprite.flipped = false;
        }

        public bool CheckCollision(Character otherCharacter)
        {
            return boundingShapes.CheckCollision(otherCharacter.boundingShapes);
        }

        public bool PointInBoundingShapes(Vector3 point)
        {
            return boundingShapes.PointInShapes(point);
        }

        public Color CheckHitMapCollision()
        {
            return boundingShapes.CheckHitMapCollision(Game1.GetActiveBackground());
        }

        public virtual void HandleCollision(Character characterCollided, bool atFault, Vector2 prevPosition)
        {

        }

        //For Idle actions
        public virtual void StandStill()
        {

        }

        public void SetBoundingShapes(BoundingShapes shapes)
        {
            this.boundingShapes = shapes;
            Vector3 min = new Vector3(shapes.outerBoundingBox.Min.X - shapes.pos.X, shapes.outerBoundingBox.Min.Y - shapes.pos.Y, 0);
            Vector3 max = new Vector3(shapes.outerBoundingBox.Max.X - shapes.pos.X, shapes.outerBoundingBox.Max.Y - shapes.pos.Y, 0);
            this.relativeOuterBox = new BoundingBox(min, max);
        }

        //Game Over logic disable looping sounds/other logic
        public virtual void GameOver()
        {

        }
    }
}
