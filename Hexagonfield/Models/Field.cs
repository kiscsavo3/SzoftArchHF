using Hexagonfield.Services;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Hexagonfield.Models
{
    public class Field
    {
        private bool isMarked = false;
        public bool IsMarked 
        {
            get
            {
                return isMarked;
            }
            set
            {
                isMarked = value;
            }
        }
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
                Polygon.Fill = Coloring.CreateRadialGradientBrush(new Color[] { value });
            }
        }
        public void Mark(Color PlayerColor)
        {
            IsMarked = true;
            Polygon.Fill = Coloring.CreateRadialGradientBrush(new Color[] { PlayerColor, Color });

        }
        public void UnMark()
        {
            IsMarked = false;
        }        
    }
}
