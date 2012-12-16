using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameBuilder.Definition
{
    public class GameDefinition
    {
        public List<BlockJson> Blocks { get; set; }
        public double Gravity { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public long InitialX { get; set; }
        public long InitialY { get; set; }
        public long FinalX { get; set; }
    }
}
