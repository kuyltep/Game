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
        private const int MapWidth = 100;                   // Ширина карты в тайлах
        private const int MapHeight = 50;                   // Высота карты в тайлах
        private const int TileSize = 10;                    // Размер тайла
        private EllipseGeometry circleGeometry;             // Круг для света нужна как глобальная, к ней будут обращения от факелов






        public MainWindow()
        {
            InitializeComponent();                          // Основной залетает
            GenerateMap();                                  // Генерация карты
            Light();                                        // Освещение
        }










        private void GenerateMap()                          // Стартуем
        {                                                   // Генерация рандомных комнат
            Random rand = new Random();
            int numRooms = rand.Next(10, 20);               // Сколько комнат добавить?
            List<Point> roomCenters = new List<Point>();    // Создает список точек для хранения центральных точек комнаты


            for (int x = 0; x < MapWidth; x++)              // Заполнить тайлами всю карту
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/10.png"))));
                }
            }
            for (int i = 0; i < numRooms; i++)              // Комната
            {
                int roomWidth = rand.Next(6, 10);           // Рандомная Ширина
                int roomHeight = rand.Next(6, 10);          // Рандомная высота
                int roomX = rand.Next(0, MapWidth - roomWidth);     // X
                int roomY = rand.Next(0, MapHeight - roomHeight);   // Y

                bool overlap = roomCenters.Any(Room1 =>                                              // Пытался сделать, чтобы не пересекались комнаты, потом доработаю
                Math.Abs(Room1.X - (roomX + roomWidth / 2)) < (roomWidth + TileSize) / 2 &&
                Math.Abs(Room1.Y - (roomY + roomHeight / 2)) < (roomHeight + TileSize) / 2);
                if (!overlap)
                {
                    roomCenters.Add(new Point(roomX + roomWidth / 2, roomY + roomHeight / 2));
                }
                else
                {
                    i--;
                }

            }

            for (int i = 0; i < roomCenters.Count - 1; i++)
            {
                ConnectRooms(roomCenters[i], roomCenters[i + 1]);
            }

            for (int i = 0; i < numRooms; i++)                              // Теперь рисуем комнаты и их границы
            {
                int roomWidth = rand.Next(6, 10);                           // Рандомная Ширина
                int roomHeight = rand.Next(6, 10);                          // Рандомная высота
                int roomX = rand.Next(0, MapWidth - roomWidth);             // X
                int roomY = rand.Next(0, MapHeight - roomHeight);           // Y


                for (int x = roomX; x < roomX + roomWidth; x++)             // Рисуем стены
                {
                    DrawTile(x, roomY, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/5.png"))));                                   // Граница верхняя
                    DrawTile(x, roomY + roomHeight - 1, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/8.png"))));                  // Граница нижняя
                    DrawTile(x, roomY + roomHeight, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/9.png"))));                      // Дополнительный слой тайлов под нижней границей
                }

                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    DrawTile(roomX, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/6.png"))));                                   // Граница левая
                    DrawTile(roomX + roomWidth - 1, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/7.png"))));                   // Граница правая
                }

                DrawTile(roomX, roomY, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/1.png"))));                                   // Верхний левый угол
                DrawTile(roomX + roomWidth - 1, roomY, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/2.png"))));                   // Верхний правый угол
                DrawTile(roomX, roomY + roomHeight - 1, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/4.png"))));                  // Нижний левый угол
                DrawTile(roomX + roomWidth - 1, roomY + roomHeight - 1, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/3.png"))));  // Нижний правый угол

                // Заполнить внутренности комнаты
                for (int x = roomX + 1; x < roomX + roomWidth - 1; x++)         // Цикл заполнения
                {
                    for (int y = roomY + 1; y < roomY + roomHeight - 1; y++)
                    {
                        if (y == roomY + 1)                                     // Проверка верхней строчки
                        {
                            // Цвет
                            DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/9.png"))));
                        }
                        else
                        {
                            DrawTile(x, y, Brushes.Beige);                      // Пол
                        }
                    }
                }
            }
        }

        private void ConnectRooms(Point start, Point end)                       // После создания всех комнат соединяем их дорогами
        {
            int x = (int)start.X;
            int y = (int)start.Y;

            while (x != end.X)
            {
                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/15.png")))); // Горизонтальная дорожка
                x += (int)Math.Sign(end.X - start.X);
            }

            while (y != end.Y)
            {
                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("D:/WpfApp6/Image/Walls/15.png")))); // Вертикальная дорожка
                y += (int)Math.Sign(end.Y - start.Y);
            }
        }

        private void DrawTile(int x, int y, Brush brush)                                                   // Рисуем тайлы
        {
            Rectangle tile = new Rectangle              // Тайл = квадрат
            {
                Width = TileSize,       // Ширина тайла
                Height = TileSize,      // Высота тайла
                Fill = brush            // Заливка
            };

            Canvas.SetLeft(tile, x * TileSize);     // Позиция по x
            Canvas.SetTop(tile, y * TileSize);      // Позиция по y ( куда ставить тайл?)

            gameCanvas.Children.Add(tile);          // =)
        }
















        private void Light()                                                                                                                    //Свет
        {
            RectangleGeometry squareGeometry = new RectangleGeometry(new Rect(-50, -50, MapWidth * TileSize + 100, MapHeight * TileSize + 100));    // Это полупрозрачный темный слой aka ночь

            circleGeometry = new EllipseGeometry(new Point(50, 50), 40, 40);                                                                    // Геометрия для круга

            GeometryGroup combination = new GeometryGroup();                                                                                    // Это нужно для объединения двух геометрических фигур
            combination.Children.Add(squareGeometry);                                                                                           // Эти 2 строчки нужны , чтобы в группу добавить объекты
            combination.Children.Add(circleGeometry);

            combination.FillRule = FillRule.EvenOdd;                                                                                            //https://learn.microsoft.com/ru-ru/dotnet/maui/user-interface/controls/shapes/fillrules?view=net-maui-8.0

            Path path = new Path();
            path.Data = combination;                                                                                                            // Тут уже можно обращаться к фигурам как к единому елементу


            path.Effect = new BlurEffect { Radius = 50 };                                                                                       // Размытие или мягкое освещение

            path.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));                                                                      // Прозрачность

            gameCanvas.Children.Add(path);                                                                                                      // Добавляем
        }

        private void gameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (circleGeometry != null)                                                                                                         // Проверка переменной, чтобы не была равна нулю
            {
                Point mousePosition = e.GetPosition(gameCanvas);                                                                                // Позиция мыши
                circleGeometry.Center = mousePosition;                                                                                          // Следование за мышью, центр круга = мышке

                gameCanvas.InvalidateVisual();                                                                                                  // Перерисовываем холст, чтобы обновить свет
            }
        }
    }
}