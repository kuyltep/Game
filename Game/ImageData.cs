using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Game
{
    internal class ImageData
    {
        public ImageData()
        {
            ImageBrush grayImageBrush = new ImageBrush();
            grayImageBrush.ImageSource = new BitmapImage(new Uri("/.png", UriKind.RelativeOrAbsolute));
        }
    }
}
