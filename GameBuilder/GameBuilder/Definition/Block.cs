using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBuilder.Definition
{
    public class BlockJson
    {
        public long X { get; set; }
        public long Y { get; set; }
        public BlockType Type { get; set; }
    }

    public class Block : GameObject
    {
        public BlockType Type { get; set; }

        public Block(BlockJson origin)
        {
            this.X = origin.X;
            this.Y = origin.Y;
            this.Type = origin.Type;
        }

        public Block() { }
    }

    public enum BlockType
    {
        Good = 0, Bad = 1
    }
}
