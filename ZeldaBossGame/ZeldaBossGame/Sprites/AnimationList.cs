using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeldaBossGame
{
    public class AnimationList
    {
        public List<SpriteAnimation> animations;

        public AnimationList()
        {
            animations = new List<SpriteAnimation>();
        }

        public void AddAnimation(SpriteAnimation anim) 
        {
            animations.Add(anim);
        }

        public SpriteAnimation GetAnimation(string name)
        {
            foreach (SpriteAnimation anim in animations)
            {
                if (anim.name.Equals(name))
                    return anim;
            }

            return null;
        }
    }
}
