using System;
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



        public void InitializeGame(Image hero, Grid GameCanvas, int heroHealth, int heroArmor)
        {

            HeroRightSide(hero);
            hero.Height = 20;
            hero.Width = 20;
            hero.VerticalAlignment = VerticalAlignment.Center;
            hero.HorizontalAlignment = HorizontalAlignment.Center;
            hero.Margin = new Thickness(0, 0, 0, 0);
            GameCanvas.Children.Add(hero);
            HeroHealthAndArmor(heroHealth, heroArmor, GameCanvas);
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
        public void HeroMove(KeyEventArgs e, Grid GameCanvas, Grid Game, Image hero )
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

            }
            else if (e.Key.ToString() == "D" && GameCanvas.Margin.Left + GameCanvas.ActualWidth < Game.ActualWidth + GameCanvas.ActualWidth / 2 - hero.Width / 2)
            {
                GameCanvas.Margin = new Thickness(canvasLeft += step, canvasTop, 0, 0);
                HeroRightSide(hero);
            }
        }
    }
}
