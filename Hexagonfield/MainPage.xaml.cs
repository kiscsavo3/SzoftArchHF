using Hexagonfield.Models;
using Hexagonfield.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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
        public int Size { get; set; } = 7;
        public List<Color> ColorList { get; set; } = new List<Color>() { Colors.Red, Colors.Blue, Colors.Green, Colors.Orange, Colors.Purple };
        public List<Field> Fields { get; set; } = new List<Field>();
        public List<Field> VisitedFields { get; set; } = new List<Field>();
        public List<Player> Players { get; set; } = new List<Player>();
        Player p = new Player() { StartCoordX = 0, StartCoordY = 6, Color = Colors.AliceBlue };
        RobotPlayer robot = new RobotPlayer() { StartCoordX = 0, StartCoordY = -6 , Color = Colors.Black};
        Dictionary<Color, HashSet<Field>> OneColorNeighbourFields = new Dictionary<Color, HashSet<Field>>();
        public MainPage()
        {
            this.InitializeComponent();
            
            //ApplicationView.PreferredLaunchViewSize = new Size(1.5*Size*Width + 100, (2*Size-1)*Height + 100 );
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            CreateColorButtons();
            CreateHexagons();

            Players.Add(new Player() { StartCoordX = 0, StartCoordY = 6, Color = Colors.AliceBlue });
            Players.Add(new RobotPlayer() { StartCoordX = 0, StartCoordY = -6, Color = Colors.Black });

            Players.ForEach(p => FindFieldsOfPlayer(p).ForEach(f => f.Mark(p.Color)));
            
        }
        public void CreateHexagons()
        {
            Point start = new Point(50 + 0.75*(Size-1)*Width, (2 * Size - 1)/2 * Height + 50);
            double[] x_delta = new double[] { 0, Width / 4, 3.0 / 4 * Width, Width, 3.0 / 4 * Width, Width / 4 };
            double[] y_delta = new double[] { 0, -1.0 / 2 * Height, -1.0 / 2 * Height, 0, 1.0 / 2 * Height, 1.0 / 2 * Height };
            for (int i = -Size + 1; i < Size; i++)
            {
                for (int j = -Size + 1; j < Size; j++)
                {
                    if (i * j > 0 && Math.Abs(i) + Math.Abs(j) >= Size) continue;
                    Point LeftCorner = new Point(start.X + Width * 0.75 * i, start.Y + Height * (0.5 * i + 1 * j));
                    var NewField = new Field();
                    NewField.CoordX = i;
                    NewField.CoordY = j;
                    NewField.Color = PickRandomColor();
                    NewField.Polygon.Stroke = new SolidColorBrush(Colors.Black);
                    NewField.Polygon.StrokeThickness = 3;
                    for (int k = 0; k < x_delta.Length; k++)
                    {
                        NewField.Polygon.Points.Add(new Point(LeftCorner.X + x_delta[k], LeftCorner.Y + y_delta[k]));
                    }
                    DrawCanvas.Children.Add(NewField.Polygon);
                    Fields.Add(NewField);
                }
            }
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

        

        public Color PickRandomColor(int NumberOfColors = 5)
        {
            Random rnd = new Random();
            var idx = rnd.Next(0, Math.Min(ColorList.Count, NumberOfColors));
            return ColorList[idx];
        }       

        public void ChangeColor(Player p, Color NewColor)
        {
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f => f.Color = NewColor);
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f => f.Mark(p.Color));
        }
        public List<Field> FindOneColorArea(int StartCoordX, int StartCoordY)
        {
            List<Field> Acc = new List<Field>();
            Field StartField = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            FindOneColorArea(StartCoordX, StartCoordY, StartField.Color, Acc);
            return Acc;
        }
        private void FindOneColorArea(int StartCoordX, int StartCoordY, Color FieldColor, List<Field> Accumulator)
        {
            Field StartField = Fields.Find(f => f.CoordX == StartCoordX && f.CoordY == StartCoordY);
            Accumulator.Add(StartField);
            List<Field> NeighbourFields = FindNeighboursOfField(StartField);
            var ColorableNeighbours = Fields.FindAll(f => NeighbourFields.Contains(f) && f.Color == FieldColor && !Accumulator.Contains(f));
            if (ColorableNeighbours.Count == 0) return;
            ColorableNeighbours.ForEach(f =>
            {
                Accumulator.Add(f);
                FindOneColorArea(f.CoordX, f.CoordY, FieldColor, Accumulator);
            });
        }
        public List<Field> FindFieldsOfPlayer(Player player)
        {
            player.Fields.Clear();
            Field StartField = Fields.Find(f => f.CoordX == player.StartCoordX && f.CoordY == player.StartCoordY);
            FindOneColorArea(player.StartCoordX, player.StartCoordY, StartField.Color, player.Fields);
            return player.Fields;
        } 
        
        List<Field> FindNeighboursOfField(Field field)
        {
            return Fields.FindAll(f => Math.Abs(f.CoordX + f.CoordY - field.CoordX - field.CoordY) <= 1 && Math.Abs(f.CoordX - field.CoordX) <= 1 && Math.Abs(f.CoordY - field.CoordY) <= 1);
        }
        public Dictionary<Color, HashSet<Field>> CountNegihbourColors(Player p)
        {
            OneColorNeighbourFields = new Dictionary<Color, HashSet<Field>>();
            var PossibleColors = new HashSet<Color>();
            HashSet<Field> neighbours = new HashSet<Field>();
            FindFieldsOfPlayer(p);
            p.Fields.ForEach(f =>
            {
                var fieldNeighbours = FindNeighboursOfField(f);
                fieldNeighbours.RemoveAll(field => p.Fields.Contains(field));
                fieldNeighbours.ForEach(fn => neighbours.Add(fn));
            });
            foreach (var neighbour in neighbours)
            {
                PossibleColors.Add(neighbour.Color);
            }
            foreach (var neighbour in neighbours)
            {
                if (OneColorNeighbourFields.ContainsKey(neighbour.Color))
                {
                    FindOneColorArea(neighbour.CoordX, neighbour.CoordY).ForEach(neigh => OneColorNeighbourFields[neighbour.Color].Add(neigh));                    
                }
                else
                {
                    OneColorNeighbourFields.Add(neighbour.Color, new HashSet<Field>(FindOneColorArea(neighbour.CoordX, neighbour.CoordY)));
                }
            }
            return OneColorNeighbourFields;
        }
        public Color RobotTurn(RobotPlayer robot)
        {
            int MaxCount = 0;
            Color MaxColor;
            Dictionary<Color, HashSet<Field>> ColorNeighbours = CountNegihbourColors(robot);
            foreach(var entry in ColorNeighbours)
            {
                if(entry.Value.Count > MaxCount)
                {
                    MaxCount = entry.Value.Count;
                    MaxColor = entry.Key;
                }
            }
            return MaxColor;
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rectangle = (Rectangle)sender;
            ChangeColor(p, (rectangle.Fill as SolidColorBrush).Color);            
            ChangeColor(robot, RobotTurn(robot));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            Fields.Clear();
            CreateHexagons();
            Players.ForEach(p => FindFieldsOfPlayer(p).ForEach(f => f.Mark(p.Color)));
        }
    }
}
