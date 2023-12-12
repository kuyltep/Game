using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    public partial class MainWindow : Window
    {
        private const int MapWidth = ShareData.MapWidth;
        private const int MapHeight = ShareData.MapHeight;
        private const int TileSize = ShareData.TileSize;
        private EllipseGeometry circleGeometry;
        private RectangleGeometry lightRectangleGeometry;
        Generation generation = new Generation();

        public MainWindow()
        {
            InitializeComponent();
            generation.GenerateMap(gameCanvas);
            Light();
        }

        private void Light()
        {
            RectangleGeometry squareGeometry = new RectangleGeometry(new Rect(-50, -50, MapWidth * TileSize + 100, MapHeight * TileSize + 100));

            circleGeometry = new EllipseGeometry(new Point(50, 50), 40, 40);
            lightRectangleGeometry = new RectangleGeometry(new Rect(0, 0, 100, 100)); // Прямоугольник для факела

            GeometryGroup combination = new GeometryGroup();
            combination.Children.Add(squareGeometry);
            combination.Children.Add(circleGeometry);
            combination.Children.Add(lightRectangleGeometry); // Добавляем прямоугольник в группу

            combination.FillRule = FillRule.EvenOdd;

            Path path = new Path();
            path.Data = combination;

            path.Effect = new BlurEffect
            {
                Radius = 50
            };

            path.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));

            lightCanvas.Children.Add(path);
        }

        private void lightCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (circleGeometry != null && lightRectangleGeometry != null)
            {
                Point mousePosition = e.GetPosition(lightCanvas);
                circleGeometry.Center = mousePosition;
                lightRectangleGeometry.Rect = new Rect(mousePosition.X - 50, mousePosition.Y - 50, 100, 100); // Обновляем положение прямоугольника

                lightCanvas.InvalidateVisual();
            }
        }
    }
}