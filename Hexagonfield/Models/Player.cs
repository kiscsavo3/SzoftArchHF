using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hexagonfield.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; } = new List<Field>();
        private int startCoordX;
        private int startCoordY;
        public int StartCoordX { get; set; }
        public int StartCoordY { get; set; }
        public Color Color { get; set; }
    }
}
