using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Game
{
    internal class Generation
    {
        private const int MapWidth = ShareData.MapWidth;// Ширина карты в тайлах
        private const int MapHeight = ShareData.MapHeight;// Высота карты в тайлах
        private const int TileSize = ShareData.TileSize;// Размер тайла
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

                for (int x = roomX; x < roomX + roomWidth; x++)// Рисуем стены
                {
                    DrawTile(x, roomY, new SolidColorBrush(Colors.LightGray));// Граница верхняя
                    DrawTile(x, roomY + roomHeight - 1, new SolidColorBrush(Colors.LightGray));// Граница нижняя
                    DrawTile(x, roomY + roomHeight, new SolidColorBrush(Colors.Gray));// Дополнительный слой тайлов под нижней границей
                }

                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    DrawTile(roomX, y, new SolidColorBrush(Colors.LightGray));// Граница левая
                    DrawTile(roomX + roomWidth - 1, y, new SolidColorBrush(Colors.LightGray));// Граница правая
                }

                DrawTile(roomX, roomY, new SolidColorBrush(Colors.Gray));// Верхний левый угол
                DrawTile(roomX + roomWidth - 1, roomY, new SolidColorBrush(Colors.Gray));// Верхний правый угол
                DrawTile(roomX, roomY + roomHeight - 1, new SolidColorBrush(Colors.Gray));// Нижний левый угол
                DrawTile(roomX + roomWidth - 1, roomY + roomHeight - 1, new SolidColorBrush(Colors.Gray));// Нижний правый угол

                                                                       // Заполнить внутренности комнаты
                for (int x = roomX + 1; x < roomX + roomWidth - 1; x++)// Цикл заполнения
                {
                    for (int y = roomY + 1; y < roomY + roomHeight - 1; y++)
                    {
                        if (y == roomY + 1)// Проверка верхней строчки
                        {
                            // Цвет
                            DrawTile(x, y, new SolidColorBrush(Colors.Gray));
                        }
                        else
                        {
                            DrawTile(x, y, Brushes.Beige);// Пол
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
            }
        }
    }
}
