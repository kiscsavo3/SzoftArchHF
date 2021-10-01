using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public List<PointCollection> MyPoints { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            this.MyPoints = new List<PointCollection>();

            this.DataContext = this;

            Point start = new Point(210, 200);
            List<Point> startPoints = new List<Point>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        startPoints.Add(new Point(start.X + 0.75 * 40 * i, start.Y + 0.5 * 40 * (j + k)));
                    }
                }
            }
            var unique_startPoints = startPoints.Distinct().ToList();
            for (int i = 0; i < unique_startPoints.Count; i++)
            {
                var polygone = new PointCollection();
                int[] x_delta = new int[] {0, 10, 30, 40, 30, 10 };
                int[] y_delta = new int[] {0, -18, -18, 0, 18, 18 };
                for (int j = 0; j < x_delta.Length; j++)
                {
                    polygone.Add(new Point(unique_startPoints[i].X + x_delta[j], unique_startPoints[i].Y + y_delta[j]));
                }
                MyPoints.Add(polygone);
            }
        }
    }
}
