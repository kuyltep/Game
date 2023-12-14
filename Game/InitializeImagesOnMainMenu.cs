using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Game
{
    internal class InitializeImagesOnMainMenu
    {
        Grid MainMenu { get; set; }
        //Класс для инициализации изображений на грид
        public void InitializeImages(Grid MainMenu)
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
            second.UriSource = new Uri("images/second-punk.jpg", UriKind.Relative);
            second.EndInit();
            second_image.Source = second;
            second_image.Width = 180;
            second_image.Height = 180;
            second_image.VerticalAlignment = VerticalAlignment.Top;
            second_image.HorizontalAlignment = HorizontalAlignment.Right;
            MainMenu.Children.Add(second_image);

        }
    }
}
