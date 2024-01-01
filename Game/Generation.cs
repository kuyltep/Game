using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Windows.Threading;


namespace Game
{
    internal class Generation
    {
        List<Rectangle> enemies = new List<Rectangle>();
        private const int MapWidth = ShareData.MapWidth;// Ширина карты в тайлах
        private const int MapHeight = ShareData.MapHeight;// Высота карты в тайлах
        private const int TileSize = ShareData.TileSize;// Размер тайла
        private DispatcherTimer enemyTimer = new DispatcherTimer();
        Random random = new Random();

        public List<Rectangle> tilesList = new List<Rectangle>();// Список тайлов, штоб кализия была
        public void GenerateMap(Canvas gameCanvas)// Стартуем
        {                                         // Генерация рандомных комнат
            Random rand = new Random();
            int numRooms = rand.Next(10, 20);// Сколько комнат добавить?
            List<Point> roomCenters = new List<Point>();// Создает список точек для хранения центральных точек комнаты


            for (int x = 0; x < MapWidth; x++)// Заполнить тайлами всю карту
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    DrawTile(x, y, new SolidColorBrush(Colors.Beige));
                }
            }
            for (int i = 0; i < numRooms; i++)// Комната
            {
                int roomWidth = rand.Next(6, 10);// Рандомная Ширина
                int roomHeight = rand.Next(6, 10);// Рандомная Высота
                int roomX = rand.Next(0, MapWidth - roomWidth);// X
                int roomY = rand.Next(0, MapHeight - roomHeight);// Y

                bool overlap = roomCenters.Any(Room1 =>// Пытался сделать, чтобы не пересекались комнаты, потом доработаю
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

            for (int i = 0; i < numRooms; i++)// Теперь рисуем комнаты и их границы
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
                    DrawTile(i, y, new SolidColorBrush(Colors.LightGray));// Граница верхняя
                    DrawTile(i, y + height - 1, new SolidColorBrush(Colors.LightGray));// Граница нижняя
                    DrawTile(i, y + height, new SolidColorBrush(Colors.Gray));// Дополнительный слой тайлов под нижней границей
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new SolidColorBrush(Colors.LightGray));// Граница левая
                    DrawTile(x + width - 1, j, new SolidColorBrush(Colors.LightGray));// Граница правая
                }

                DrawTile(x, y, new SolidColorBrush(Colors.Gray));// Верхний левый угол
                DrawTile(x + width - 1, y, new SolidColorBrush(Colors.Gray));// Верхний правый угол
                DrawTile(x, y + height - 1, new SolidColorBrush(Colors.Gray));// Нижний левый угол
                DrawTile(x + width - 1, y + height - 1, new SolidColorBrush(Colors.Gray));// Нижний правый угол

                for (int i = x + 1; i < x + width - 1; i++)// Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        if (j == y + 1)// Проверка верхней строчки
                        {
                            DrawTile(i, j, new SolidColorBrush(Colors.Gray));
                        }
                        else
                        {
                            DrawTile(i, j, Brushes.Beige);
                        }
                    }
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

                for (int i = x + 1; i < x + width - 1; i++)
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        if (j == y + 1)
                        {
                            DrawTile(i, j, new SolidColorBrush(Colors.Blue));
                        }
                        else
                        {
                            DrawTile(i, j, Brushes.Beige);
                        }
                    }
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

                for (int i = x + 1; i < x + width - 1; i++)
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        if (j == y + 1)
                        {
                            DrawTile(i, j, new SolidColorBrush(Colors.Green));
                        }
                        else
                        {
                            DrawTile(i, j, Brushes.Beige);
                        }
                    }
                }
            }
            void ConnectRooms(Point start, Point end)// После создания всех комнат соединяем их дорогами
            {
                int x = (int)start.X;
                int y = (int)start.Y;

                while (x != end.X)
                {
                    DrawTile(x, y, new SolidColorBrush(Colors.DarkGray));// Горизонтальная дорожка
                    x += (int)Math.Sign(end.X - start.X);
                }

                while (y != end.Y)
                {
                    DrawTile(x, y, new SolidColorBrush(Colors.DarkGray));// Вертикальная дорожка
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

        }

        public void MoveCharacter(int X, int Y, Image hero, Grid GameCanvas, int characterX, int characterY, int canvasLeft, int canvasTop)
        {
            int CharacterX1 = characterX * X;
            int CharacterY1 = characterY * Y;

            if (!Collision(CharacterX1, CharacterY1))
            {
                GameCanvas.Margin = new Thickness(canvasLeft += CharacterX1, canvasTop += CharacterY1, 0, 0);
                hero.Margin = new Thickness(characterX += CharacterX1, characterY += CharacterY1, 0, 0);
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

        public void InitializeEnemyTimer(Canvas gameCanvas)
        {
            enemyTimer.Interval = TimeSpan.FromSeconds(3); // Интервал появления новых врагов
            enemyTimer.Tick += (sender, e) => EnemyTimer_Tick(sender, e, gameCanvas);
            enemyTimer.Start();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e, Canvas gameCanvas)
        {
            CreateEnemy(gameCanvas);
        }

        private void CreateEnemy(Canvas gameCanvas)
        {
            Rectangle enemy = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Red
            };
            gameCanvas.Children.Add(enemy);

            double randomX = random.Next((int)(gameCanvas.ActualWidth - enemy.Width));
            double randomY = random.Next((int)(gameCanvas.ActualHeight - enemy.Height));

            Canvas.SetLeft(enemy, randomX);
            Canvas.SetTop(enemy, randomY);

            enemies.Add(enemy);
        }

    }
}
