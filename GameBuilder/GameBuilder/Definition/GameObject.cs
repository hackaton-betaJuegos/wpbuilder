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

        public abstract bool Collides(GameObject obj);
    }
}
