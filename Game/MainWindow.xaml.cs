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
        private EllipseGeometry circleGeometry;// Круг для света нужна как глобальная, к ней будут обращения от факелов
        Generation generation = new Generation();
        public MainWindow()
        {
            InitializeComponent();// Основной залетает
            generation.GenerateMap(gameCanvas);// Генерация карты
            Light();// Освещение
        }

        private void Light()//Свет
        {
            RectangleGeometry squareGeometry = new RectangleGeometry(new Rect(-50, -50, MapWidth * TileSize + 100, MapHeight * TileSize + 100));// Это полупрозрачный темный слой aka ночь

            circleGeometry = new EllipseGeometry(new Point(50, 50), 40, 40);// Геометрия для круга

            GeometryGroup combination = new GeometryGroup();// Это нужно для объединения двух геометрических фигур
            combination.Children.Add(squareGeometry);// Эти 2 строчки нужны , чтобы в группу добавить объекты
            combination.Children.Add(circleGeometry);

            combination.FillRule = FillRule.EvenOdd;//https://learn.microsoft.com/ru-ru/dotnet/maui/user-interface/controls/shapes/fillrules?view=net-maui-8.0

            Path path = new Path();
            path.Data = combination;// Тут уже можно обращаться к фигурам как к единому елементу


            path.Effect = new BlurEffect { Radius = 50 };// Размытие или мягкое освещение

            path.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));// Прозрачность

            gameCanvas.Children.Add(path);// Добавляем
        }

        private void gameCanvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (circleGeometry != null)// Проверка переменной, чтобы не была равна нулю
            {
                Point mousePosition = e.GetPosition(gameCanvas);// Позиция мыши
                circleGeometry.Center = mousePosition;// Следование за мышью, центр круга = мышке

                gameCanvas.InvalidateVisual();// Перерисовываем холст, чтобы обновить свет
            }
        }
    }
}