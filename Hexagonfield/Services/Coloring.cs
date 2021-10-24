using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Hexagonfield.Services
{
    class Coloring
    {
        public static RadialGradientBrush CreateRadialGradientBrush(Color[] Colors)
        {
            RadialGradientBrush radialGradient = new RadialGradientBrush();

            radialGradient.GradientOrigin = new Point(0.5, 0.5);

            radialGradient.Center = new Point(0.5, 0.5);

            radialGradient.RadiusX = 0.5;
            radialGradient.RadiusY = 0.5;

            for(int i = 0; i < Colors.Length; i++)
            {
                radialGradient.GradientStops.Add(new GradientStop() { Color = Colors[i], Offset = ((double) i )/Colors.Length });
            }

            return radialGradient;
        } 
    }
}
