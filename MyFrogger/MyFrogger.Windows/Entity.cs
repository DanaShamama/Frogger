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
    class Entity
    {
        //data members
        public Image Image { get; set; }
        public Point Location { get; set; }
        public double StepLeftRight { get; set; }

        //c'tor
        public Entity()
        {
            StepLeftRight = 1;
        }
    }
}