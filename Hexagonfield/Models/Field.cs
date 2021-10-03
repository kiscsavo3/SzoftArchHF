using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Hexagonfield.Models
{
    public class Field
    {
        private int coordX;
        private int coordY;
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        private Polygon polygon;
        public Polygon Polygon { get; set; } = new Polygon();
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                Polygon.Fill = new SolidColorBrush(value);
            }
        }
    }
}
