using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MyFrogger
{
    class Turtles : RiverElements
    {
        // c'tor
        public Turtles()
        {
            Image = new Image();
            Image.Source= new BitmapImage(new Uri(@"ms-appx:///Assets/turtles_triple.png "));
            Image.Width = 200;
            Image.Height = 50;
            Image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
            StepLeftRight = 1;
        }
    }
}
