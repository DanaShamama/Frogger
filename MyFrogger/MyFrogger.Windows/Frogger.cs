using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MyFrogger
{
    class Frogger : Entity
    {
        //data members
        public double StepUpDown { get; set; }

        //c'tor
        public Frogger()
        {
            Image = new Image();
            Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/frog.png "));
            Image.Width = 58;
            Image.Height = 58;
            Image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;

            Location = new Point(1115, 1120);
            Canvas.SetLeft(Image, 1115);
            Canvas.SetTop(Image, 1120);
            Canvas.SetZIndex(Image, 1);
            StepLeftRight = 85;
        }
    }
}