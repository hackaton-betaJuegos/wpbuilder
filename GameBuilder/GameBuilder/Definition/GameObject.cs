using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBuilder.Definition
{
    public abstract class GameObject
    {
        public long X { get; set; }
        public long Y { get; set; }

        public bool Collides(GameObject obj)
        {
            return obj != null && obj.X == X && obj.Y == Y;
        }
    }
}
