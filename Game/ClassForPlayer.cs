﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
namespace Game
{
    internal class ClassForPlayer
    {

        int canvasLeft { get; set; }
        int canvasTop { get; set; }

        public Image gunRight = new Image();
        public Image gunLeft = new Image();

        public void InitializeGame(Image hero, Grid GameCanvas, int heroHealth, int heroArmor, string gunName )
        {

            HeroRightSide(hero);
            hero.Height = 20;
            hero.Width = 20;
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

        //Hero Move
        public void HeroMove(KeyEventArgs e, Grid GameCanvas, Grid Game, Image hero, string gunName )
        {
            
            int step = 3;

            if (e.Key.ToString() == "W" && GameCanvas.Margin.Top > -GameCanvas.ActualHeight / 2 + hero.Height / 2)
            {
                GameCanvas.Margin = new Thickness(canvasLeft, canvasTop -= step, 0, 0);
            }
            else if (e.Key.ToString() == "S" && GameCanvas.Margin.Top + GameCanvas.Height + 20 < Game.ActualHeight + GameCanvas.ActualHeight / 2 + hero.Height / 2)
            {
                GameCanvas.Margin = new Thickness(canvasLeft, canvasTop += step, 0, 0);

            }
            else if (e.Key.ToString() == "A" && GameCanvas.Margin.Left > -GameCanvas.ActualWidth / 2 + hero.Width / 2)
            {
                GameCanvas.Margin = new Thickness(canvasLeft -= step, canvasTop, 0, 0);
                HeroLeftSide(hero);
                gunRight.Visibility = Visibility.Hidden;
                gunLeft.Visibility = Visibility.Visible;

            }
            else if (e.Key.ToString() == "D" && GameCanvas.Margin.Left + GameCanvas.ActualWidth < Game.ActualWidth + GameCanvas.ActualWidth / 2 - hero.Width / 2)
            {
                GameCanvas.Margin = new Thickness(canvasLeft += step, canvasTop, 0, 0);
                HeroRightSide(hero);
                gunLeft.Visibility = Visibility.Hidden;
                gunRight.Visibility = Visibility.Visible;
            }
        }
    }
}
