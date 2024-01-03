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
using System.Windows.Threading;

namespace Game
{
    public partial class MainWindow : Window
    {
        public bool isSettingsMenuOpen = false;
        public bool isGameInitialize = false;
        public int heroHealth = 3;
        public int heroArmor = 3;
        public int canvasLeft = 0;
        public int canvasTop = 0;
        public int heroLeft = 0;
        public int heroTop = 0;
        public bool isMusicOn = false;
        private MediaPlayer _mpBgr;
        private MediaPlayer _mpCurSound;
        public Image hero = new Image();
        public string gunName = "GreenGun";
        public List<Rectangle> enemies = new List<Rectangle>();
        List<(Image, string)> itemsImages = new List<(Image, string)>();
        string[] items = { "book", "drink", "eye", "hearth", "stew" };
        private DispatcherTimer enemyTimer = new DispatcherTimer();
        Random random = new Random();
        Random randomItem = new Random();
        int itemsCount = 0;

        public Image gunRight = new Image();
        public Image gunLeft = new Image();

        private List<(Rectangle bullet, int direction)> bullets = new List<(Rectangle, int)>();
        private DispatcherTimer bulletTimer = new DispatcherTimer();
        private List<int> bulletDistance = new List<int>();
        public int direction = 1;

        private const int MapWidth = 300;    // Ширина карты в тайлах
        private const int MapHeight = 100;  // Высота карты в тайлах
        private const int TileSize = 24;    // Размер тайла
        public MainWindow()
        {
            InitializeComponent();
            GenerateMap(gameCanvas);

        }

        //*****************************************************************************************************************************************************
        //Initializes menu methods
        //При загрузке приложения вызываем методы для инициализации главного меню
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMediaPlayer();
            InitializeImages();
            InitializeMainButtons();
        }

        //Метод для инициализации плеера при запуске игры 
        public void InitializeMediaPlayer()
        {
            //Initialize music player
            _mpBgr = new MediaPlayer();
            _mpCurSound = new MediaPlayer();
            _mpBgr.Open(new Uri(@"sounds\music.mp3", UriKind.Relative));
            _mpBgr.Play();
            isMusicOn = true;
        }

        //Метод для инициализации изображений на грид
        public void InitializeImages()
        {
            //Initialize first image
            Image first_image = new Image();
            BitmapImage first = new BitmapImage();
            first.BeginInit();
            first.UriSource = new Uri("images/first-punk.png", UriKind.Relative);
            first.EndInit();
            first_image.Source = first;
            first_image.Width = 180;
            first_image.Height = 180;
            first_image.VerticalAlignment = VerticalAlignment.Bottom;
            first_image.HorizontalAlignment = HorizontalAlignment.Left;
            MainMenu.Children.Add(first_image);

            //Initialize second image
            Image second_image = new Image();
            BitmapImage second = new BitmapImage();
            second.BeginInit();
            second.UriSource = new Uri(@"images/second-punk.jpg", UriKind.Relative);
            second.EndInit();
            second_image.Source = second;
            second_image.Width = 180;
            second_image.Height = 180;
            second_image.VerticalAlignment = VerticalAlignment.Top;
            second_image.HorizontalAlignment = HorizontalAlignment.Right;
            MainMenu.Children.Add(second_image);

        }
        //Метод для инициализации кнопок главного меню на грид
        public void InitializeMainButtons()
        {
            //Initialize play button
            Button continueBtn = new Button();
            continueBtn.FontFamily = new FontFamily("Wide Latin");
            continueBtn.FontSize = 24;
            continueBtn.Width = 330;
            continueBtn.Height = 40;
            continueBtn.Background = Brushes.LightGray;
            continueBtn.Content = "Играть";
            continueBtn.Click += play_Click;
            continueBtn.HorizontalAlignment = HorizontalAlignment.Center;
            continueBtn.VerticalAlignment = VerticalAlignment.Center;
            continueBtn.Margin = new Thickness(0, 0, 0, 150);
            MainMenu.Children.Add(continueBtn);
            //Initialize settings button
            Button settings = new Button();
            settings.FontFamily = new FontFamily("Wide Latin");
            settings.FontSize = 24;
            settings.Width = 330;
            settings.Height = 40;
            settings.Background = Brushes.LightGray;
            settings.Content = "Настройки";
            settings.Click += settings_Click;
            settings.HorizontalAlignment = HorizontalAlignment.Center;
            settings.VerticalAlignment = VerticalAlignment.Center;
            settings.Margin = new Thickness(0, 0, 0, 0);
            MainMenu.Children.Add(settings);
            //Initialize exit button
            Button exitToMainMenu = new Button();
            exitToMainMenu.FontFamily = new FontFamily("Wide Latin");
            exitToMainMenu.FontSize = 24;
            exitToMainMenu.Width = 330;
            exitToMainMenu.Height = 40;
            exitToMainMenu.Background = Brushes.LightGray;
            exitToMainMenu.Content = "Выйти";
            exitToMainMenu.Click += exit_Click;
            exitToMainMenu.HorizontalAlignment = HorizontalAlignment.Center;
            exitToMainMenu.VerticalAlignment = VerticalAlignment.Center;
            exitToMainMenu.Margin = new Thickness(0, 150, 0, 0);
            MainMenu.Children.Add(exitToMainMenu);
        }
        //Обработчик для кнопки начать игру в главном меню
        private void play_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            if (!isGameInitialize)
            { 
                //Добавить метод для инициализации игры!!!!!!!!!!!!!!!!!!!
                InitializeGame(hero, GameCanvas, heroHealth, heroArmor, gunName);
                isGameInitialize = true;
            }
        }
        //Обработчик для кнопки настройки в главном меню
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Visible;
            if (!isSettingsMenuOpen)
            {
                InitializeSettingsButtons();
                isSettingsMenuOpen = true;
            }
        }
        //Обработчик для кнопки выход в главном меню
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Метод для инициализации кнопок меню паузы в процессе игры
        public void InitializePauseMenuButtons()
        {
            //Initialize play button
            Button play = new Button();
            play.FontFamily = new FontFamily("Wide Latin");
            play.FontSize = 24;
            play.Width = 330;
            play.Height = 40;
            play.Background = Brushes.LightGray;
            play.Content = "Продолжить";
            play.Click += playPauseMenu_Click;
            play.HorizontalAlignment = HorizontalAlignment.Center;
            play.VerticalAlignment = VerticalAlignment.Center;
            play.Margin = new Thickness(0, 0, 0, 150);
            PauseMenu.Children.Add(play);
            //Initialize settings button
            Button settingsBtnOnPauseMenu = new Button();
            settingsBtnOnPauseMenu.FontFamily = new FontFamily("Wide Latin");
            settingsBtnOnPauseMenu.FontSize = 24;
            settingsBtnOnPauseMenu.Width = 330;
            settingsBtnOnPauseMenu.Height = 40;
            settingsBtnOnPauseMenu.Background = Brushes.LightGray;
            settingsBtnOnPauseMenu.Content = "Настройки";
            settingsBtnOnPauseMenu.Click += settingsPauseMenu_Click;
            settingsBtnOnPauseMenu.HorizontalAlignment = HorizontalAlignment.Center;
            settingsBtnOnPauseMenu.VerticalAlignment = VerticalAlignment.Center;
            settingsBtnOnPauseMenu.Margin = new Thickness(0, 0, 0, 0);
            PauseMenu.Children.Add(settingsBtnOnPauseMenu);
            //Initialize exit button
            Button exit = new Button();
            exit.FontFamily = new FontFamily("Wide Latin");
            exit.FontSize = 24;
            exit.Width = 330;
            exit.Height = 40;
            exit.Background = Brushes.LightGray;
            exit.Content = "В главное меню";
            exit.Click += exitPauseMenu_Click;
            exit.HorizontalAlignment = HorizontalAlignment.Center;
            exit.VerticalAlignment = VerticalAlignment.Center;
            exit.Margin = new Thickness(0, 150, 0, 0);
            PauseMenu.Children.Add(exit);
        }

        //Метод для инициализации кнопок в меню настроек на главном гриде
        public void InitializeSettingsButtons()
        {
            //Initialize musicOff button
            Button musicOff = new Button();
            musicOff.FontFamily = new FontFamily("Wide Latin");
            musicOff.FontSize = 24;
            musicOff.Width = 330;
            musicOff.Height = 40;
            musicOff.Background = Brushes.LightGray;
            musicOff.Content = "Выключить музыку";
            musicOff.Click += musicOff_Click;
            musicOff.HorizontalAlignment = HorizontalAlignment.Center;
            musicOff.VerticalAlignment = VerticalAlignment.Center;
            musicOff.Margin = new Thickness(0, 0, 0, 150);
            Settings.Children.Add(musicOff);
            //Initialize musicOn button
            Button musicOn = new Button();
            musicOn.FontFamily = new FontFamily("Wide Latin");
            musicOn.FontSize = 24;
            musicOn.Width = 330;
            musicOn.Height = 40;
            musicOn.Background = Brushes.LightGray;
            musicOn.Content = "Включить музыку";
            musicOn.Click += musicOn_Click;
            musicOn.HorizontalAlignment = HorizontalAlignment.Center;
            musicOn.VerticalAlignment = VerticalAlignment.Center;
            musicOn.Margin = new Thickness(0, 0, 0, 0);
            Settings.Children.Add(musicOn);
            //Initialize backToMainWindow button
            Button backToMainWindow = new Button();
            backToMainWindow.FontFamily = new FontFamily("Wide Latin");
            backToMainWindow.FontSize = 24;
            backToMainWindow.Width = 330;
            backToMainWindow.Height = 40;
            backToMainWindow.Background = Brushes.LightGray;
            backToMainWindow.Content = "Назад";
            backToMainWindow.Click += backToMainWindow_Click;
            backToMainWindow.HorizontalAlignment = HorizontalAlignment.Center;
            backToMainWindow.VerticalAlignment = VerticalAlignment.Center;
            backToMainWindow.Margin = new Thickness(0, 150, 0, 0);
            Settings.Children.Add(backToMainWindow);
        }


        //Метод для инициализации кнопок настройки в меню паузы
        public void InitializePauseSettingsButtons()
        {
            //Initialize musicOff button
            Button musicOff = new Button();
            musicOff.FontFamily = new FontFamily("Wide Latin");
            musicOff.FontSize = 24;
            musicOff.Width = 330;
            musicOff.Height = 40;
            musicOff.Background = Brushes.LightGray;
            musicOff.Content = "Выключить музыку";
            musicOff.Click += musicOff_Click;
            musicOff.HorizontalAlignment = HorizontalAlignment.Center;
            musicOff.VerticalAlignment = VerticalAlignment.Center;
            musicOff.Margin = new Thickness(0, 0, 0, 150);
            PauseSettings.Children.Add(musicOff);
            //Initialize musicOn button
            Button musicOn = new Button();
            musicOn.FontFamily = new FontFamily("Wide Latin");
            musicOn.FontSize = 24;
            musicOn.Width = 330;
            musicOn.Height = 40;
            musicOn.Background = Brushes.LightGray;
            musicOn.Content = "Включить музыку";
            musicOn.Click += musicOn_Click;
            musicOn.HorizontalAlignment = HorizontalAlignment.Center;
            musicOn.VerticalAlignment = VerticalAlignment.Center;
            musicOn.Margin = new Thickness(0, 0, 0, 0);
            PauseSettings.Children.Add(musicOn);
            //Initialize backToMainWindow button
            Button backToPauseMenu = new Button();
            backToPauseMenu.FontFamily = new FontFamily("Wide Latin");
            backToPauseMenu.FontSize = 24;
            backToPauseMenu.Width = 330;
            backToPauseMenu.Height = 40;
            backToPauseMenu.Background = Brushes.LightGray;
            backToPauseMenu.Content = "Назад";
            backToPauseMenu.Click += backToPauseMenu_Click;
            backToPauseMenu.HorizontalAlignment = HorizontalAlignment.Center;
            backToPauseMenu.VerticalAlignment = VerticalAlignment.Center;
            backToPauseMenu.Margin = new Thickness(0, 150, 0, 0);
            PauseSettings.Children.Add(backToPauseMenu);
        }

        //Обработчик для кнопки включить музыку в меню настроек
        private void musicOn_Click(object sender, RoutedEventArgs e)
        {
            if (!isMusicOn)
            {
                _mpBgr.Play();
                isMusicOn = !isMusicOn;
            }
        }
        //Обработчик для кнопки выключить музыку в меню настроек
        private void musicOff_Click(object sender, RoutedEventArgs e)
        {
            if (isMusicOn)
            {
                _mpBgr.Pause();
                isMusicOn = !isMusicOn;
            }
        }
        //Обработчик для кнопки вернуться в главное меню
        private void backToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Settings.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;

        }

        //Меню паузы в процессе игры
        //Обработчик для кнопки продолжить игру в меню паузы
        private void playPauseMenu_Click(object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Hidden;

        }

        //Обработчик для кнопки настроек в меню паузы 
        private void settingsPauseMenu_Click(Object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Hidden;
            PauseSettings.Visibility = Visibility.Visible;
            InitializePauseSettingsButtons();
        }
        //Обработчик для кнопки в главное меню в меню паузы 
        private void exitPauseMenu_Click(Object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Hidden;
            Game.Children.Clear();
            MainMenu.Visibility = Visibility.Visible;
            isGameInitialize = false;

        }
        //Обработчик для кнопки вернуться в меню паузы в меню настроек паузы
        private void backToPauseMenu_Click(Object obj, RoutedEventArgs e)
        {
            PauseSettings.Visibility = Visibility.Hidden;
            PauseMenu.Visibility = Visibility.Visible;
        }

        //Обработчик для нажатия клавиши для отображения меню 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Game.Visibility == Visibility.Visible && Convert.ToString(e.Key) == "Escape")
            {
                if (PauseSettings.Visibility == Visibility.Visible || PauseMenu.Visibility == Visibility.Visible)
                {
                    PauseSettings.Visibility = Visibility.Hidden;
                    PauseMenu.Visibility = Visibility.Hidden;
                }
                else
                {
                    InitializePauseMenuButtons();
                    PauseMenu.Visibility = Visibility.Visible;
                }
            }
            if (Game.Visibility == Visibility.Visible)
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
                        direction = -1;
                        MoveCharacter(-1, 0);
                        HeroLeftSide(hero);
                        gunRight.Visibility = Visibility.Hidden;
                        gunLeft.Visibility = Visibility.Visible;
                        break;
                    case Key.Right:
                        direction = 1;
                        MoveCharacter(1, 0);
                        HeroRightSide(hero);
                        gunLeft.Visibility = Visibility.Hidden;
                        gunRight.Visibility = Visibility.Visible;

                        break;
                    default:
                        break;
                }
                /* Добавить свет для персонажа !!!!!!!!!!!!
                ClassForPlayer.HeroMove(e, GameCanvas, Game, hero, gunName);
                if (circleGeometry != null)
                {
                    Point heromove = new Point(GameCanvas.Margin.Left + GameCanvas.Width / 2, GameCanvas.Margin.Top + GameCanvas.Height / 2);
                    circleGeometry.Center = heromove;
                    lightCanvas.InvalidateVisual();
                }
                */
            }

        }
        //**********************************************************************************************************************************************************


        //********************************************************************************************************************************************************
        //Инициализация игры и всей логики игры (персонаж, стрельба, враги)
        public void InitializeEnemyTimer(Canvas gameCanvas)
        {
            enemyTimer.Interval = TimeSpan.FromSeconds(5); // Интервал появления новых врагов
            enemyTimer.Tick += (sender, e) => EnemyTimer_Tick(sender, e, gameCanvas);
            enemyTimer.Start();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e, Canvas gameCanvas)
        {
            CreateEnemy(gameCanvas);
        }

        public void CreateEnemy(Canvas gameCanvas)
        {

            for (int i = 0; i < 6; i++)
            {
                Rectangle enemy = new Rectangle
                {
                    Width = 15,
                    Height = 15,
                    Fill = Brushes.Blue
                };
                gameCanvas.Children.Add(enemy);

                double randomX = random.Next((int)(gameCanvas.ActualWidth - enemy.Width));
                double randomY = random.Next((int)(gameCanvas.ActualHeight - enemy.Height));

                Canvas.SetLeft(enemy, randomX);
                Canvas.SetTop(enemy, randomY);
                enemies.Add(enemy);
            }
        }

        public void InitializeBulletTimer(Canvas gameCanvas, Grid GameCanvas, List<Rectangle> enemies)
        {
            bulletTimer.Interval = TimeSpan.FromMilliseconds(10);
            bulletTimer.Tick += (sender, e) => BulletTimer_Tick(sender, e, gameCanvas, GameCanvas, enemies);
        }


        public void BulletTimer_Tick(object sender, EventArgs e, Canvas gameCanvas, Grid GameCanvas, List<Rectangle> enemies)
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var (bullet, direction) = bullets[i];
                double bulletLeft = Canvas.GetLeft(bullet);
                double bulletRight = bulletLeft + bullet.Width;
                double bulletTop = Canvas.GetTop(bullet);
                double bulletBottom = bulletTop + bullet.Height;
                Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + 5 * direction); // Скорость полета пули, умноженная на направление
                bulletDistance[i] += 5;

                // Удаление пули, если она преодолела расстояние 200 пикселей
                if (Math.Abs(Canvas.GetLeft(bullet) - (GameCanvas.Margin.Left + GameCanvas.Width / 2)) >= 200 || GameCanvas.Margin.Left - Canvas.GetRight(bullet) >= 200 || Canvas.GetLeft(bullet) < 0 || Canvas.GetLeft(bullet) > gameCanvas.ActualWidth || bulletDistance[i] >= 200)
                {
                    gameCanvas.Children.Remove(bullet);
                    bullets.RemoveAt(i);
                    bulletDistance.RemoveAt(i);
                }
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    var enemy = enemies[j];

                    // Получаем границы врага
                    double enemyLeft = Canvas.GetLeft(enemy);
                    double enemyRight = enemyLeft + enemy.Width;
                    double enemyTop = Canvas.GetTop(enemy);
                    double enemyBottom = enemyTop + enemy.Height;

                    // Проверяем пересечение пули и врага
                    bool isIntersect = false;//CheckRectangleIntersection(enemy, bullet);
                    if (isIntersect)
                    {
                        // Столкновение произошло - удаляем пулю и врага
                        gameCanvas.Children.Remove(bullet);
                        gameCanvas.Children.Remove(enemy);

                        bullets.RemoveAt(i);
                        enemies.RemoveAt(j);
                        break; // Прерываем проверку этой пули с оставшимися врагами
                    }
                }
            }


        }


        public void Window_MouseDown(object sender, MouseEventArgs e)
        {
            int distance = 0;
            var bullet = new Rectangle
            {
                Width = 6,
                Height = 5,
                Fill = Brushes.OrangeRed
            };
            gameCanvas.Children.Add(bullet);
            if (direction == 1)
            {
                Canvas.SetLeft(bullet, GameCanvas.Margin.Left + GameCanvas.Width / 2 + hero.Width); // Начальная позиция пули по оси X
                Canvas.SetTop(bullet, GameCanvas.Margin.Top + GameCanvas.Height / 2); // Начальная позиция пули по оси Y
            }
            else
            {
                Canvas.SetLeft(bullet, GameCanvas.Margin.Left + GameCanvas.Width / 2 - hero.Width); // Начальная позиция пули по оси X
                Canvas.SetTop(bullet, GameCanvas.Margin.Top + GameCanvas.Height / 2); // Начальная позиция пули по оси Y
            }
            bullets.Add((bullet, direction));
            bulletDistance.Add(distance);

            if (!bulletTimer.IsEnabled)
            {
                bulletTimer.Start();
            }
        }


        private void RandomGenerationItems(int itemsCount)
        {
            for(int  i = 0; i <= itemsCount; i++)
            {
                //Рандомная генерация предметов на карте, но не работает то, что они должны добавляться в центр комнаты

                int index = randomItem.Next(0, items.Length);
                Image itemImage = new Image();
                BitmapImage item = new BitmapImage();
                item.BeginInit();
                item.UriSource = new Uri($"images/items/{items[index]}.png", UriKind.Relative);
                item.EndInit();
                itemImage.Source = item;
                itemImage.Width = 20;
                itemImage.Height = 20;
                itemImage.Margin = new Thickness(randomItem.Next(0, MapWidth * TileSize / 2), randomItem.Next(0, MapHeight * TileSize / 2), 0, 0);
                itemsImages.Add((itemImage, items[index]));
                gameCanvas.Children.Add(itemImage);
            }
        }
        public void InitializeGame(Image hero, Grid GameCanvas, int heroHealth, int heroArmor, string gunName)
        {
            RandomGenerationItems(11);
            HeroRightSide(hero);
            hero.Height = 25;
            hero.Width = 25;
            hero.VerticalAlignment = VerticalAlignment.Center;
            hero.HorizontalAlignment = HorizontalAlignment.Center;
            hero.Margin = new Thickness(0, 0, 0, 0);
            GameCanvas.Children.Add(hero);
            setRightGun(gunName, GameCanvas);
            setLeftGun(gunName, GameCanvas);
            gunLeft.Visibility = Visibility.Hidden;
            HeroHealthAndArmor(heroHealth, heroArmor, GameCanvas);

        }

        public Image setRightGun(string gunName, Grid GameCanvas)
        {
            BitmapImage gunRightImage = new BitmapImage();
            gunRightImage.BeginInit();
            gunRightImage.UriSource = new Uri($"images/guns/{gunName}Right.png", UriKind.Relative);
            gunRightImage.EndInit();
            gunRight.Source = gunRightImage;
            gunRight.Height = 12;
            gunRight.Width = 12;
            gunRight.VerticalAlignment = VerticalAlignment.Center;
            gunRight.HorizontalAlignment = HorizontalAlignment.Center;
            gunRight.Margin = new Thickness(25, 5, 0, 0);
            gunRight.Visibility = Visibility.Visible;
            GameCanvas.Children.Add(gunRight);
            return gunRight;
        }

        public Image setLeftGun(string gunName, Grid GameCanvas)
        {

            BitmapImage gunLeftImage = new BitmapImage();
            gunLeftImage.BeginInit();
            gunLeftImage.UriSource = new Uri($"images/guns/{gunName}Left.png", UriKind.Relative);
            gunLeftImage.EndInit();
            gunLeft.Source = gunLeftImage;
            gunLeft.Height = 12;
            gunLeft.Width = 12;
            gunLeft.VerticalAlignment = VerticalAlignment.Center;
            gunLeft.HorizontalAlignment = HorizontalAlignment.Center;
            gunLeft.Margin = new Thickness(0, 5, 25, 0);
            gunLeft.Visibility = Visibility.Visible;
            GameCanvas.Children.Add(gunLeft);
            return gunLeft;

        }

        //Hero health and Hero armor
        public void HeroHealthAndArmor(int heroHealth, int heroArmor, Grid GameCanvas)
        {
            Image health = new Image();
            BitmapImage healthImage = new BitmapImage();
            healthImage.BeginInit();
            healthImage.UriSource = new Uri($"images/hp/{heroHealth}hp.png", UriKind.Relative);
            healthImage.EndInit();
            health.Source = healthImage;
            health.Height = 10;
            health.VerticalAlignment = VerticalAlignment.Bottom;
            health.HorizontalAlignment = HorizontalAlignment.Right;
            health.Margin = new Thickness(0, 0, 0, -10);
            GameCanvas.Children.Add(health);

            Image armor = new Image();
            BitmapImage armorImage = new BitmapImage();
            armorImage.BeginInit();
            armorImage.UriSource = new Uri($"images/armor/{heroArmor}arm.png", UriKind.Relative);
            armorImage.EndInit();
            armor.Source = armorImage;
            armor.Height = 10;
            armor.VerticalAlignment = VerticalAlignment.Bottom;
            armor.HorizontalAlignment = HorizontalAlignment.Right;
            armor.Margin = new Thickness(0, 0, 0, -20);
            GameCanvas.Children.Add(armor);
        }

        //Hero right side initialize
        public void HeroRightSide(Image hero)
        {
            BitmapImage heroRightMove = new BitmapImage();
            heroRightMove.BeginInit();
            heroRightMove.UriSource = new Uri("images/heroRightSide.png", UriKind.Relative);
            heroRightMove.EndInit();
            hero.Source = heroRightMove;
        }
        //Hero left side initialize
        public void HeroLeftSide(Image hero)
        {
            BitmapImage heroLeftMove = new BitmapImage();
            heroLeftMove.BeginInit();
            heroLeftMove.UriSource = new Uri("images/heroLeftSide.png", UriKind.Relative);
            heroLeftMove.EndInit();
            hero.Source = heroLeftMove;
        }


        //***********************************************************************************************************************************************************



        //**********************************************************************************************************************************************************
        private List<Rectangle> tilesList = new List<Rectangle>();// Список тайлов, штоб кализия была


        public void GenerateMap(Canvas gameCanvas)          // Стартуем
        {                                                   // Генерация рандомных комнат
            Random rand = new Random();
            int numRooms = rand.Next(40, 50);               // Сколько комнат добавить?
            List<Point> roomCenters = new List<Point>();    // Создает список точек для хранения центральных точек комнаты

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    DrawTileV(i, j, new ImageBrush(new BitmapImage(new Uri("Image/qwe.png", UriKind.Relative))));
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
                    DrawTile(i, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_1.png", UriKind.Relative))));                          // Граница верхняя
                    DrawTile(i, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_6.png", UriKind.Relative))));             // Граница нижняя
                    DrawTile(i, y + height, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1.png", UriKind.Relative))));                   // Дополнительный слой тайлов под нижней границей
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_7.png", UriKind.Relative))));                          // Граница левая
                    DrawTile(x + width - 1, j, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_8.png", UriKind.Relative))));              // Граница правая
                }

                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_3.png", UriKind.Relative))));                              // Верхний левый угол
                DrawTile(x + width - 1, y, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_5.png", UriKind.Relative))));                  // Верхний правый угол
                DrawTile(x, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_9.png", UriKind.Relative))));                 // Нижний левый угол
                DrawTile(x + width - 1, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1_10.png", UriKind.Relative))));    // Нижний правый угол

                int floor = rand.Next(2); // Выбор случайного варианта пола
                ImageBrush floorBrush = null;

                switch (floor)
                {
                    case 0:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwer.png", UriKind.Relative)));
                        break;
                    case 1:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwert.png", UriKind.Relative)));
                        break;
                    default:
                        break;
                }

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTileV(i, j, floorBrush); // Рисование плитки пола
                    }
                }
                for (int i = x + 1; i < x + width - 1; i++)
                {
                    DrawTile(i, y + 1, new ImageBrush(new BitmapImage(new Uri("Image/Room1/Wall1.png", UriKind.Relative)))); // Например, добавление строки стены чуть ниже верхней границы
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
                    DrawTile(i, y, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_1.png", UriKind.Relative))));
                    DrawTile(i, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_3.png", UriKind.Relative))));
                    DrawTile(i, y + height, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2.png", UriKind.Relative))));
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_4.png", UriKind.Relative))));
                    DrawTile(x + width - 1, j, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_5.png", UriKind.Relative))));
                }

                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_6.png", UriKind.Relative))));
                DrawTile(x + width - 1, y, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_7.png", UriKind.Relative))));
                DrawTile(x, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_9.png", UriKind.Relative))));
                DrawTile(x + width - 1, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2_8.png", UriKind.Relative))));

                int floor = rand.Next(2); // Выбор случайного варианта пола
                ImageBrush floorBrush = null;

                switch (floor)
                {
                    case 0:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwer.png", UriKind.Relative)));
                        break;
                    case 1:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwert.png", UriKind.Relative)));
                        break;
                    default:
                        break;
                }


                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTileV(i, j, floorBrush); // Рисование плитки пола
                    }
                }
                for (int i = x + 1; i < x + width - 1; i++)
                {
                    DrawTile(i, y + 1, new ImageBrush(new BitmapImage(new Uri("Image/Room2/Wall2.png", UriKind.Relative)))); // Например, добавление строки стены чуть ниже верхней границы
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
                    DrawTile(i, y, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_1.png", UriKind.Relative))));
                    DrawTile(i, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_2.png", UriKind.Relative))));
                    DrawTile(i, y + height, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3.png", UriKind.Relative))));
                }

                for (int j = y; j < y + height; j++)
                {
                    DrawTile(x, j, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_3.png", UriKind.Relative))));
                    DrawTile(x + width - 1, j, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_4.png", UriKind.Relative))));
                }

                DrawTile(x, y, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_8.png", UriKind.Relative))));
                DrawTile(x + width - 1, y, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_5.png", UriKind.Relative))));
                DrawTile(x, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_7.png", UriKind.Relative))));
                DrawTile(x + width - 1, y + height - 1, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3_6.png", UriKind.Relative))));

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTile(i, j, (j == y + 1) ? new SolidColorBrush(Colors.Green) : Brushes.Beige);
                    }
                }

                int floor = rand.Next(2); // Выбор случайного варианта пола
                ImageBrush floorBrush = null;

                switch (floor)
                {
                    case 0:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwer.png", UriKind.Relative)));
                        break;
                    case 1:
                        floorBrush = new ImageBrush(new BitmapImage(new Uri("Image/floor/qwert.png", UriKind.Relative)));
                        break;
                    default:
                        break;
                }

                for (int i = x + 1; i < x + width - 1; i++) // Заполнить внутренности комнаты
                {
                    for (int j = y + 1; j < y + height - 1; j++)
                    {
                        DrawTileV(i, j, floorBrush); // Рисование плитки пола
                    }
                }
                for (int i = x + 1; i < x + width - 1; i++)
                {
                    DrawTile(i, y + 1, new ImageBrush(new BitmapImage(new Uri("Image/Room3/Wall3.png", UriKind.Relative)))); // Например, добавление строки стены чуть ниже верхней границы
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

        private int chaacterx = 0;
        private int chaactery = 0;
        private void MoveCharacter(int X, int Y)
        {
            int CharacterX1 = chaacterx + X;
            int CharacterY1 = chaactery + Y;

            if (!Collision(CharacterX1, CharacterY1))
            {
                chaacterx = CharacterX1;
                chaactery = CharacterY1;


                GameCanvas.Margin = new Thickness(chaacterx * TileSize, chaactery * TileSize, 0, 0);
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


    }
}
