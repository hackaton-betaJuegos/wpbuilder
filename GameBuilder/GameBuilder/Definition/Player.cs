using GameBuilder.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBuilder.Definition
{
    public class Player : GameObject
    {
        public Texture2D Texture { get; set; }
       
        public Player(GraphicsDevice device, int blocksize)
        {
            Texture = ColorTextureCreator.Create(device, blocksize, blocksize, Color.DarkGoldenrod);
        }
    }
}
