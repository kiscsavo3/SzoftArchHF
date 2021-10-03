using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hexagonfield
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int Width { get; set; } = 40;
        public int Height { get; set; } = 36;
        public int Size { get; set; } = 5;
        public List<Color> ColorList { get; set; } = new List<Color>() { Colors.Red, Colors.Blue , Colors.Green };// , Colors.Yellow, Colors.Orange, Colors.White };
        public List<Models.Field> Fields { get; set; } = new List<Models.Field>();
        public List<Models.Field> VisitedFields { get; set; } = new List<Models.Field>();
        public MainPage()
        {
            this.InitializeComponent();
            CreateHexagons();
        }
        public void CreateHexagons()
        {
            Point start = new Point(210, 200);
            double[] x_delta = new double[] { 0, Width / 4, 3.0 / 4 * Width, Width, 3.0 / 4 * Width, Width / 4 };
            double[] y_delta = new double[] { 0, -1.0 / 2 * Height, -1.0 / 2 * Height, 0, 1.0 / 2 * Height, 1.0 / 2 * Height };
            for (int i = -Size + 1; i < Size; i++)
            {
                for (int j = -Size + 1; j < Size; j++)
                {
                    if (i * j > 0 && Math.Abs(i) + Math.Abs(j) >= Size) continue;
                    Point LeftCorner = new Point(start.X + Width * 0.75 * i, start.Y + Height * (0.5 * i + j));
                    var NewField = new Models.Field();
                    NewField.CoordX = i;
                    NewField.CoordY = j;
                    NewField.Color = PickRandomColor();
                    NewField.Polygon.Stroke = new SolidColorBrush(Colors.Black);
                    for (int k = 0; k < x_delta.Length; k++)
                    {
                        NewField.Polygon.Points.Add(new Point(LeftCorner.X + x_delta[k], LeftCorner.Y + y_delta[k]));
                    }
                    DrawCanvas.Children.Add(NewField.Polygon);
                    Fields.Add(NewField);
                }
            }
        }
        public Color PickRandomColor(int NumberOfColors = 5)
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, Math.Min(ColorList.Count, NumberOfColors));
            return ColorList[idx];
        }
        public void ChangeColor(int StartCoordX, int StartCoordY, Color NewColor, Object OldColor = null)
        {
            Models.Field StartField = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            if(OldColor == null) OldColor = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY).Color;
            StartField.Color = NewColor;
            VisitedFields.Add(StartField);
            var ColorableNeighbours = Fields.FindAll(f => Math.Abs(f.CoordX + f.CoordY - StartCoordX - StartCoordY) <= 1 && Math.Abs(f.CoordX - StartCoordX) <= 1 && Math.Abs(f.CoordY - StartCoordY) <= 1  && f.Color == (Color) OldColor && !VisitedFields.Contains(f));
            if (ColorableNeighbours.Count == 0) return;            
            ColorableNeighbours.ForEach(f =>
            {
                VisitedFields.Add(f);
                f.Color = NewColor;
                ChangeColor(f.CoordX, f.CoordY, NewColor, OldColor);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(-2, 2, Colors.Red);
            VisitedFields = new List<Models.Field>();           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            Fields.Clear();
            CreateHexagons();

        }
    }
}
