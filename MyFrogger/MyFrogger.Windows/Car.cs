using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MyFrogger
{
    class Car : Entity
    {
        //c'tor
        public Car()
        {
            Image = new Image();
            Image.Width = 58;
            Image.Height = 58;
            Image.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
            StepLeftRight = 1;
        }
    }
}
