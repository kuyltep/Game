﻿using System;
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
        public bool isMusicOn = false;
        private MediaPlayer _mpBgr;
        private MediaPlayer _mpCurSound;
        public MainWindow()
        {
            InitializeComponent();


        }
        //Класс для инициализации плеера для музыки
        public void InitializeMediaPlayer()
        {
            //Initialize music player
            _mpBgr = new MediaPlayer();
            _mpCurSound = new MediaPlayer();
            _mpBgr.Open(new Uri(@"sounds\music.mp3", UriKind.Relative));
            _mpBgr.Play();
            isMusicOn = true;
            //_mpBgr.Position = TimeSpan.FromMinutes(11.45);
        }
        //Класс для инициализации изображений на грид
        public void InitializeImages()
        {
            //Initialize first image
            Image first_image = new Image();
            BitmapImage first = new BitmapImage();
            first.BeginInit();
            first.UriSource = new Uri("first-punk.png", UriKind.Relative);
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
            second.UriSource = new Uri("second-punk.jpg", UriKind.Relative);
            second.EndInit();
            second_image.Source = second;
            second_image.Width = 180;
            second_image.Height = 180;
            second_image.VerticalAlignment = VerticalAlignment.Top;
            second_image.HorizontalAlignment = HorizontalAlignment.Right;
            MainMenu.Children.Add(second_image);

        }
        //Класс для инициализации кнопок главного меню на грид
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
        //Класс для инициализации кнопок в меню настроек на главном гриде
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
        //Обработчик для кнопки начать игру в главном меню
        private void play_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
        }
        //Обработчик для кнопки настройки в главном меню
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Visible;
            InitializeSettingsButtons();
        }
        //Обработчик для кнопки выход в главном меню
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //При загрузке приложения вызываем классы для инициализации главного меню
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMediaPlayer();
            InitializeImages();
            InitializeMainButtons();
        }

        //Класс для инициализации кнопок меню паузы в процессе игры
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
            play.Click += playPauseMenu_Click; //Добавить другой обработчик событий, чтобы игра возобнавлялась
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
            settingsBtnOnPauseMenu.Click += settingsPauseMenu_Click; //Добавить обработчик событий для настроек
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
            exit.Click += exitPauseMenu_Click; //Добавить обработчик событий для выхода в главное меню
            exit.HorizontalAlignment = HorizontalAlignment.Center;
            exit.VerticalAlignment = VerticalAlignment.Center;
            exit.Margin = new Thickness(0, 150, 0, 0);
            PauseMenu.Children.Add(exit);
        }
        //Класс для инициализации кнопок настройки в меню паузы
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
            PauseMenu.Visibility=Visibility.Hidden;
            PauseSettings.Visibility = Visibility.Visible;
            InitializePauseSettingsButtons();
        }
        //Обработчик для кнопки в главное меню в меню паузы 
        private void exitPauseMenu_Click(Object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Hidden;
            MainMenu.Visibility=Visibility.Visible;

        }
        //Обработчик для кнопки вернуться в меню паузы в меню настроек паузы
        private void backToPauseMenu_Click(Object obj, RoutedEventArgs e)
        {
            PauseSettings.Visibility = Visibility.Hidden;
            PauseMenu.Visibility= Visibility.Visible;
        }
        //Обработчик для нажатия клавиши для отображения меню 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Game.Visibility == Visibility.Visible && Convert.ToString(e.Key) == "Escape" ) 
            {
                if(PauseSettings.Visibility == Visibility.Visible || PauseMenu.Visibility == Visibility.Visible)
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
        }
    }
    
  
        

}
