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

        public MainWindow()
        {
            InitializeComponent();
            GenerateMap(gameCanvas);

        }
        private List<Rectangle> tilesList = new List<Rectangle>();// Список тайлов, штоб кализия была

        private const int MapWidth = 300;    // Ширина карты в тайлах
        private const int MapHeight = 100;  // Высота карты в тайлах
        private const int TileSize = 24;    // Размер тайла
        public void GenerateMap(Canvas gameCanvas)          // Стартуем
        {                                                   // Генерация рандомных комнат
            Random rand = new Random();
            int numRooms = rand.Next(40, 50);               // Сколько комнат добавить?
            List<Point> roomCenters = new List<Point>();    // Создает список точек для хранения центральных точек комнаты
            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    DrawTileV(i, j, new SolidColorBrush(Colors.Red)); // Рисует тайл в позиции (i, j) красного цвета
                }
            }

            for (int i = 0; i < numRooms; i++)// Комната
            {
                int roomWidth = rand.Next(6, 10);// Рандомная Ширина
                int roomHeight = rand.Next(6, 10);// Рандомная Высота
                int roomX = rand.Next(0, MapWidth - roomWidth);// X
                int roomY = rand.Next(0, MapHeight - roomHeight);// Y

                bool overlap = roomCenters.Any(Room1 =>
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
            for (int i = 0; i < numRooms; i++)// Рисуем комнаты и их границы
            {
                int roomWidth = rand.Next(6, 10);// Рандомная Ширина
                int roomHeight = rand.Next(6, 10);// Рандомная высота
                int roomX = rand.Next(0, MapWidth - roomWidth);// X
                int roomY = rand.Next(0, MapHeight - roomHeight);// Y

                switch (rand.Next(1, 4))
                {
                    case 1:
                        DrawRectangularRoom1(roomX, roomY, roomWidth, roomHeight);
                        break;
                    case 2:
                        DrawRectangularRoom2(roomX, roomY, roomWidth, roomHeight);
                        break;
                    case 3:
                        DrawRectangularRoom3(roomX, roomY, roomWidth, roomHeight);
                        break;
                }
            }

            void DrawRectangularRoom1(int x, int y, int width, int height)
            {
                for (int i = x; i < x + width; i++)
                {
                    DrawTile(i, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_1.png", UriKind.Relative))));// Граница верхняя
                    DrawTile(i, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_6.png", UriKind.Relative))));// Граница нижняя
                    DrawTile(i, y + height, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1.png", UriKind.Relative))));// Дополнительный слой тайлов под нижней границей
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_7.png", UriKind.Relative))));// Граница левая
                    DrawTile(x + width - 1, j, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_8.png", UriKind.Relative))));// Граница правая
                }

                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_3.png", UriKind.Relative))));// Верхний левый угол
                DrawTile(x + width - 1, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_5.png", UriKind.Relative))));// Верхний правый угол
                DrawTile(x, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_9.png", UriKind.Relative))));// Нижний левый угол
                DrawTile(x + width - 1, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_10.png", UriKind.Relative))));// Нижний правый угол

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTileV(i, j, (j == y + 1) ? new SolidColorBrush(Colors.Gray) : Brushes.Beige);
                    }
                }

                int randomWall = rand.Next(4);

                switch (randomWall)
                {
                    case 0: // Верхняя стенка
                        int doorTop = rand.Next(x + 1, x + width - 2);
                        DrawTileV(doorTop, y, Brushes.Red);
                        DrawTileV(doorTop, y + 1, Brushes.Red);
                        break;
                    case 1: // Нижняя стенка
                        int doorBottom = rand.Next(x + 1, x + width - 2);
                        DrawTileV(doorBottom, y + height - 1, Brushes.Red);
                        DrawTileV(doorBottom, y + height, Brushes.Red);
                        break;
                    case 2: // Левая стенка
                        int doorLeft = rand.Next(y + 1, y + height - 2);
                        DrawTileV(x, doorLeft, Brushes.Red);
                        break;
                    case 3: // Правая стенка
                        int doorRight = rand.Next(y + 1, y + height - 2);
                        DrawTileV(x + width - 1, doorRight, Brushes.Red);
                        break;
                    default:
                        break;
                }
            }
            void DrawRectangularRoom2(int x, int y, int width, int height)
            {
                for (int i = x; i < x + width; i++)
                {
                    DrawTile(i, y, new SolidColorBrush(Colors.LightBlue));
                    DrawTile(i, y + height - 1, new SolidColorBrush(Colors.LightBlue));
                    DrawTile(i, y + height, new SolidColorBrush(Colors.Blue));
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new SolidColorBrush(Colors.LightBlue));
                    DrawTile(x + width - 1, j, new SolidColorBrush(Colors.LightBlue));
                }

                DrawTile(x, y, new SolidColorBrush(Colors.Blue));
                DrawTile(x + width - 1, y, new SolidColorBrush(Colors.Blue));
                DrawTile(x, y + height - 1, new SolidColorBrush(Colors.Blue));
                DrawTile(x + width - 1, y + height - 1, new SolidColorBrush(Colors.Blue));

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTile(i, j, (j == y + 1) ? new SolidColorBrush(Colors.Blue) : Brushes.Beige);
                    }
                }

                int randomWall = rand.Next(4);

                switch (randomWall)
                {
                    case 0: // Верхняя стенка
                        int doorXTop = rand.Next(x + 1, x + width - 2);
                        DrawTile(doorXTop, y, Brushes.Red);
                        DrawTile(doorXTop, y + 1, Brushes.Red);
                        break;
                    case 1: // Нижняя стенка
                        int doorXBottom = rand.Next(x + 1, x + width - 2);
                        DrawTile(doorXBottom, y + height - 1, Brushes.Red);
                        DrawTile(doorXBottom, y + height, Brushes.Red);
                        break;
                    case 2: // Левая стенка
                        int doorYLeft = rand.Next(y + 1, y + height - 2);
                        DrawTile(x, doorYLeft, Brushes.Red);
                        break;
                    case 3: // Правая стенка
                        int doorYRight = rand.Next(y + 1, y + height - 2);
                        DrawTile(x + width - 1, doorYRight, Brushes.Red);
                        break;
                    default:
                        break;
                }
            }
            void DrawRectangularRoom3(int x, int y, int width, int height)
            {
                for (int i = x; i < x + width; i++)
                {
                    DrawTile(i, y, new SolidColorBrush(Colors.LightGreen));
                    DrawTile(i, y + height - 1, new SolidColorBrush(Colors.LightGreen));
                    DrawTile(i, y + height, new SolidColorBrush(Colors.Green));
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new SolidColorBrush(Colors.LightGreen));
                    DrawTile(x + width - 1, j, new SolidColorBrush(Colors.LightGreen));
                }

                DrawTile(x, y, new SolidColorBrush(Colors.Green));
                DrawTile(x + width - 1, y, new SolidColorBrush(Colors.Green));
                DrawTile(x, y + height - 1, new SolidColorBrush(Colors.Green));
                DrawTile(x + width - 1, y + height - 1, new SolidColorBrush(Colors.Green));

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTile(i, j, (j == y + 1) ? new SolidColorBrush(Colors.Green) : Brushes.Beige);
                    }
                }

                int randomWall = rand.Next(4);

                switch (randomWall)
                {
                    case 0: // Верхняя стенка
                        int doorXTop = rand.Next(x + 1, x + width - 2);
                        DrawTile(doorXTop, y, Brushes.Red);
                        DrawTile(doorXTop, y + 1, Brushes.Red);
                        break;
                    case 1: // Нижняя стенка
                        int doorXBottom = rand.Next(x + 1, x + width - 2);
                        DrawTile(doorXBottom, y + height - 1, Brushes.Red);
                        DrawTile(doorXBottom, y + height, Brushes.Red);
                        break;
                    case 2: // Левая стенка
                        int doorYLeft = rand.Next(y + 1, y + height - 2);
                        DrawTile(x, doorYLeft, Brushes.Red);
                        break;
                    case 3: // Правая стенка
                        int doorYRight = rand.Next(y + 1, y + height - 2);
                        DrawTile(x + width - 1, doorYRight, Brushes.Red);
                        break;
                    default:
                        break;
                }
            }
            void ConnectRooms(Point start, Point end)// После создания всех комнат соединяем их дорогами
            {
                int x = (int)start.X;
                int y = (int)start.Y;

                while (x != end.X)
                {
                    DrawTileV(x, y, new SolidColorBrush(Colors.DarkGray));// Горизонтальная дорожка
                    x += (int)Math.Sign(end.X - start.X);
                }

                while (y != end.Y)
                {
                    DrawTileV(x, y, new SolidColorBrush(Colors.DarkGray));// Вертикальная дорожка
                    y += (int)Math.Sign(end.Y - start.Y);
                }
            }
            void DrawTile(int x, int y, Brush brush)// Рисуем тайлы
            {
                Rectangle tile = new Rectangle// Тайл = квадрат
                {
                    Width = TileSize,// Ширина тайла
                    Height = TileSize,// Высота тайла
                    Fill = brush// Заливка
                };

                Canvas.SetLeft(tile, x * TileSize);// Позиция по x
                Canvas.SetTop(tile, y * TileSize);// Позиция по y ( куда ставить тайл?)

                gameCanvas.Children.Add(tile);// =)
                tilesList.Add(tile);
            }
            void DrawTileV(int x, int y, Brush brush)// Рисуем тайлы
            {
                Rectangle tile = new Rectangle// Тайл = квадрат
                {
                    Width = TileSize,// Ширина тайла
                    Height = TileSize,// Высота тайла
                    Fill = brush// Заливка
                };

                Canvas.SetLeft(tile, x * TileSize);// Позиция по x
                Canvas.SetTop(tile, y * TileSize);// Позиция по y ( куда ставить тайл?)

                gameCanvas.Children.Add(tile);// =)
            }
        }

        private int characterX = 0;
        private int characterY = 0;
        private void MoveCharacter(int X, int Y)
        {
            int CharacterX1 = characterX + X;
            int CharacterY1 = characterY + Y;

            if (!Collision(CharacterX1, CharacterY1))
            {
                characterX = CharacterX1;
                characterY = CharacterY1;

                Canvas.SetLeft(CharacterRectangle, characterX * TileSize);
                Canvas.SetTop(CharacterRectangle, characterY * TileSize);

                Canvas.SetZIndex(CharacterRectangle, int.MaxValue); // Вывод поверх остальных элементов
            }
        }


        private bool Collision(int x, int y)
        {
            foreach (Rectangle tile in tilesList)
            {
                int tileX = (int)Canvas.GetLeft(tile) / TileSize;
                int tileY = (int)Canvas.GetTop(tile) / TileSize;

                if (tileX == x && tileY == y)
                {
                    return true;
                }
            }
            return false;
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    MoveCharacter(0, -1);
                    break;
                case Key.Down:
                    MoveCharacter(0, 1);
                    break;
                case Key.Left:
                    MoveCharacter(-1, 0);
                    break;
                case Key.Right:
                    MoveCharacter(1, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
