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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{

    public partial class MainWindow : Window
    {
        private int[,] map;
        private Rectangle player;
        private int playerX, playerY;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();
            InitializePlayer();
            DrawMap();
        }

        //Что такое карта?
        private void InitializeMap()
        {
            map = new int[,]
            {
                {1, 0, 1, 2},
                {1, 1, 1, 0},
                {0, 0, 1, 1},
                {2, 1, 0, 1}
            };
        }

        //Кто есть игрок?
        private void InitializePlayer()
        {
            player = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.Blue
            };

            playerX = 0;
            playerY = 0;
        }

        //Генерируем карту + в конце добавляем игрока
        private void DrawMap()
        {
            Canvas1.Children.Clear();

            //Рисуем падики
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Rectangle tile = new Rectangle
                    {
                        Width = 30,
                        Height = 30
                    };

                    switch (map[x, y])
                    {
                        case 0:
                            tile.Fill = Brushes.Gray;
                            break;
                        case 1:
                            tile.Fill = Brushes.Green;
                            break;
                        case 2:
                            tile.Fill = Brushes.Brown;
                            break;
                    }

                    Canvas.SetLeft(tile, x * 30);
                    Canvas.SetTop(tile, y * 30);

                    Canvas1.Children.Add(tile);
                }
            }
            //Игрок входит в хату
            Canvas.SetLeft(player, playerX * 30);
            Canvas.SetTop(player, playerY * 30);
            Canvas1.Children.Add(player);
        }

        //Движение
        private void MovePlayer(int deltaX, int deltaY)
        {
            int newX = playerX + deltaX;
            int newY = playerY + deltaY;

            if (newX >= 0 && newX < map.GetLength(0) && newY >= 0 && newY < map.GetLength(1))
            {
                if (map[newX, newY] != 0)
                {
                    Canvas1.Children.Remove(player);

                    playerX = newX;
                    playerY = newY;

                    Canvas.SetLeft(player, playerX * 30);
                    Canvas.SetTop(player, playerY * 30);
                    Canvas1.Children.Add(player);
                }
            }
        }

        //Штоб клацатб
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    MovePlayer(0, -1);
                    break;
                case System.Windows.Input.Key.Down:
                    MovePlayer(0, 1);
                    break;
                case System.Windows.Input.Key.Left:
                    MovePlayer(-1, 0);
                    break;
                case System.Windows.Input.Key.Right:
                    MovePlayer(1, 0);
                    break;
            }
        }

    }
}
