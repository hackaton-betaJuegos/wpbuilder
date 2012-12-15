using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBuilder.Definition
{
    public class Block : GameObject
    {
        public BlockType Type { get; set; }

        public override bool Collides(GameObject obj)
        {
            
        }
    }

    public enum BlockType
    {
        Good, Bad
    }
}
