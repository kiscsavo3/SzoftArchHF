using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Hexagonfield.ViewModels
{
    public class GameViewModel : MvxViewModel
    {

        public int Width { get; set; } = 40;
        public int Height { get; set; } = 36;
        public int Size { get; set; } = 5;
        public List<Color> ColorList { get; set; } = new List<Color>() { Colors.Red, Colors.Blue, Colors.Green, Colors.Orange, Colors.Purple };
        public List<Models.Field> Fields { get; set; } = new List<Models.Field>();
        public List<Models.Field> VisitedFields { get; set; } = new List<Models.Field>();
        public GameViewModel()
        {
            CreateColorButtons();
            CreateHexagons();

            MarkStartField(0, 0);
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
        public void MarkStartField(int CoordX, int CoordY)
        {
            Fields.Find(f => f.CoordX == CoordX && f.CoordY == CoordY).Mark();
        }
        public void UnMarkStartField(int CoordX, int CoordY)
        {
            Fields.Find(f => f.CoordX == CoordX && f.CoordY == CoordY).UnMark();
        }
        public void CreateColorButtons()
        {
            foreach (var color in ColorList)
            {
                Rectangle rectangle = new Rectangle() { Width = 25, Height = 25, Fill = new SolidColorBrush(color) };
                rectangle.Tapped += Rectangle_Tapped;
                ColorPanel.Children.Add(rectangle);
            }
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            ChangeColor(0, 0, (rectangle.Fill as SolidColorBrush).Color);
            VisitedFields = new List<Models.Field>();
        }

        public Color PickRandomColor(int NumberOfColors = 5)
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, Math.Min(ColorList.Count, NumberOfColors));
            return ColorList[idx];
        }
        public void ChangeColor(int StartCoordX, int StartCoordY, Color NewColor)
        {
            Color OldColor = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY).Color;
            ChangeColor(StartCoordX, StartCoordY, NewColor, OldColor);
        }
        private void ChangeColor(int StartCoordX, int StartCoordY, Color NewColor, Color OldColor)
        {
            Models.Field StartField = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            StartField.Color = NewColor;
            VisitedFields.Add(StartField);
            var ColorableNeighbours = Fields.FindAll(f => Math.Abs(f.CoordX + f.CoordY - StartCoordX - StartCoordY) <= 1 && Math.Abs(f.CoordX - StartCoordX) <= 1 && Math.Abs(f.CoordY - StartCoordY) <= 1 && f.Color == (Color)OldColor && !VisitedFields.Contains(f));
            if (ColorableNeighbours.Count == 0) return;
            ColorableNeighbours.ForEach(f =>
            {
                VisitedFields.Add(f);
                f.Color = NewColor;
                ChangeColor(f.CoordX, f.CoordY, NewColor, OldColor);
            });
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            Fields.Clear();
            CreateHexagons();

        }
    }
}
