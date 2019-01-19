using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;


namespace MyFrogger
{
    class GameManager
    {
        //data members
        const int NUM_OF_CARS = 6;
        const int NUM_OF_TURTLES = 4;
        const int NUM_OF_WOODS = 5;
        const int NUM_OF_BIG_CARS = 3;
        const int NUM_OF_ROAD_AND_RIVER_ELEMENTS = 9;

        private Canvas _myCanvas;
        DispatcherTimer managerTimer;

        private Frogger _frogi;
        private Entity[][] RoadAndRiverElements;
        Frogger[] _bigFrogger;
        private int _bigFrogCounter;
        private bool[] _bigFroggerIsOccupied;

        public bool _flagDead;
        public bool FlagDead { get { return _flagDead; }  }
        private bool _flagFrogiOnRiverElement;
        private bool _flagWin;
        private bool _isKing;
        public bool IsKing { get { return _isKing; } }

        //c'tor
        public GameManager(Canvas cnv)
        {
            
            _myCanvas = cnv;
            
            RoadAndRiverElements = new Entity[NUM_OF_ROAD_AND_RIVER_ELEMENTS][];

            managerTimer = new DispatcherTimer();
            managerTimer.Interval = TimeSpan.FromTicks(1);

            _bigFrogger = new Frogger[5];
            _bigFroggerIsOccupied = new bool[5];
            for (int i = 0; i < _bigFrogger.Length; i++)
            {
                _bigFrogger[i] = new Frogger();

                _bigFrogger[i].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/froghead.png "));
                _bigFrogger[i].Image.Height = 100;
                _bigFrogger[i].Image.Width = 100;

                _bigFrogger[i].Location = new Point(118 + i * 500 ,40);
                Canvas.SetLeft(_bigFrogger[i].Image, _bigFrogger[i].Location.X);
                Canvas.SetTop(_bigFrogger[i].Image, _bigFrogger[i].Location.Y);

                _bigFroggerIsOccupied[i] = false;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            FrogiOverTheRiver();

            if (FrogiOverTheRiver())
            {
                _frogi.Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/froghead.png "));
            }
            Entity elementofFrogyOnRiver = new Entity();
            for (int i = 0; i < RoadAndRiverElements.Length; i++)
            {

                for (int j = 0; j < RoadAndRiverElements[i].Length; j++)
                {
                    if (i % 2 == 0)
                    {
                        RoadAndRiverElements[i][j].StepLeftRight = -2; //wood
                    }
                    else
                    {
                        RoadAndRiverElements[i][j].StepLeftRight = 2; //turtle 

                    }

                    RoadAndRiverElements[i][j].Location = new Point(RoadAndRiverElements[i][j].Location.X + RoadAndRiverElements[i][j].StepLeftRight, RoadAndRiverElements[i][j].Location.Y); // Todo move methode to set the canvas 
                    Canvas.SetLeft(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.X);
                    Canvas.SetTop(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.Y);
                    bool isColliding = false;

                    for (int l = 0; l < RoadAndRiverElements.Length; l++)
                    {
                        for (int m = 0; m < RoadAndRiverElements[l].Length; m++)
                        {
                            isColliding = !Intersect(RoadAndRiverElements[l][m]).IsEmpty;
                            if (isColliding)
                            {
                                elementofFrogyOnRiver = RoadAndRiverElements[l][m];
                                break;
                            }
                        }
                        if (isColliding)
                            break;
                    }


                    if (isColliding && _frogi.Location.Y < 500)
                    {
                        _flagFrogiOnRiverElement = true;
                        BitmapImage a = (BitmapImage)_frogi.Image.Source;
                        var s = a.UriSource.AbsolutePath;
                    }
                    else
                    {
                        _flagFrogiOnRiverElement = false;
                    }
                    if (_frogi.Location.Y < 500 && !_flagFrogiOnRiverElement || isColliding && _frogi.Location.Y > 600) // TODO less than king place!!!
                    {
                        _frogi.Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/green_splat.png "));
                        _flagDead = true;
                    }
                    if (_flagFrogiOnRiverElement && !_flagDead && RoadAndRiverElements[i][j].Location == elementofFrogyOnRiver.Location) // TODO: !_flagDead
                    {
                        _frogi.Location = new Point(_frogi.Location.X + elementofFrogyOnRiver.StepLeftRight, _frogi.Location.Y); // Todo move methode to set the canvas //* (0.1175)
                        Canvas.SetLeft(_frogi.Image, _frogi.Location.X);
                        Canvas.SetTop(_frogi.Image, _frogi.Location.Y);
                    }

                    for (int k = 0; k < RoadAndRiverElements[i].Length; k++)
                    {
                        int start;

                        if (i % 2 == 0)
                        {
                            start = 2200;
                        }
                        else
                        {
                            start = 0;
                        }


                        if (i % 2 == 0 && RoadAndRiverElements[i][j].Location.X == 0 || i % 2 != 0 && RoadAndRiverElements[i][j].Location.X == 2000)
                        {
                            RoadAndRiverElements[i][j].Location = new Point(start, RoadAndRiverElements[i][j].Location.Y); // Todo move methode to set the canvas 
                            Canvas.SetLeft(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.X);
                            Canvas.SetTop(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.Y);
                        }
                    }
                }
            }
        }
        //fonctions

        public void StartNewGame()
        {
            _frogi = new Frogger();
            _myCanvas.Children.Add(_frogi.Image);
            _bigFrogCounter = 0;
            _flagWin = false;
            _flagDead = false;
            _flagFrogiOnRiverElement = false;
            _isKing = false;
            CreateElements();

            managerTimer.Tick += Timer_Tick;
            managerTimer.Start();
        }

        public void MoveFrogy(Windows.UI.Core.KeyEventArgs args)
        {
            if (!_flagDead)
            {
                switch (args.VirtualKey)
                {
                    case Windows.System.VirtualKey.Left:
                        MoveLeft();
                        break;
                    case Windows.System.VirtualKey.Up:
                        MoveUp();
                        break;
                    case Windows.System.VirtualKey.Right:
                        MoveRight();
                        break;
                    case Windows.System.VirtualKey.Down:
                        MoveDown();
                        break;
                }
            }
        }

        public void MoveUp()
        {
            if (_frogi.Location.Y >= 105)
            {
                if (_frogi.Location.Y <= 720)
                {
                    _frogi.StepUpDown = 105;                  
                }
                else
                {
                    _frogi.StepUpDown = 85;
                }
                _frogi.Location = new Point(_frogi.Location.X, _frogi.Location.Y - _frogi.StepUpDown);
                Canvas.SetTop(_frogi.Image, _frogi.Location.Y);
            }
        }

        public void MoveDown()
        {
            if (_frogi.Location.Y <= 1105)
            {
                if (_frogi.Location.Y <= 615)
                {
                    _frogi.StepUpDown = 105;
                }
                else
                {
                    _frogi.StepUpDown = 85;
                }

                _frogi.Location = new Point(_frogi.Location.X, _frogi.Location.Y + _frogi.StepUpDown);
                Canvas.SetTop(_frogi.Image, _frogi.Location.Y);
            }
        }

        public void MoveLeft()
        {
            if (_frogi.Location.X >= 85)
            {
                _frogi.Location = new Point(_frogi.Location.X - _frogi.StepLeftRight, _frogi.Location.Y);
                Canvas.SetLeft(_frogi.Image, _frogi.Location.X);
            }
        }

        public void MoveRight()
        {

            if (_frogi.Location.X <= 2140)
            {
                _frogi.Location = new Point(_frogi.Location.X + _frogi.StepLeftRight, _frogi.Location.Y);
                Canvas.SetLeft(_frogi.Image, _frogi.Location.X);
            }
        }

        public void EndGame()
        {
            RemoveElements();
            managerTimer.Tick -= Timer_Tick;
            _flagDead = true;           
        }

        public void PauseGame()
        {
            managerTimer.Stop();
        }

        public void ResumeGame()
        {
            managerTimer.Start();
        }

        private Rect Intersect(Entity element)
        {
            Rect myRectangle = new Rect();
            myRectangle.X = _frogi.Location.X;
            myRectangle.Y = _frogi.Location.Y;
            myRectangle.Width = _frogi.Image.Width;
            myRectangle.Height = _frogi.Image.Height;

            Rect myRectangle2 = new Rect();
            myRectangle2.X = element.Location.X;
            myRectangle2.Y = element.Location.Y;
            myRectangle2.Width = element.Image.Width;
            myRectangle2.Height = element.Image.Height;

            myRectangle.Intersect(myRectangle2);

            return myRectangle;
        }

        public void CreateElements()
        {
            for (int i = 0; i < _bigFrogger.Length; i++)
            {
                _bigFrogger[i].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/froghead.png "));
            }

            for (int i = 0; i < RoadAndRiverElements.Length; i++)
            {
                if (i <= 4)
                {
                    if (i == 4)
                    {
                        RoadAndRiverElements[i] = new Car[NUM_OF_BIG_CARS];
                    }
                    else
                    {
                        RoadAndRiverElements[i] = new Car[NUM_OF_CARS];
                    }


                    for (int j = 0; j < RoadAndRiverElements[i].Length; j++)
                    {
                        int gap = 250;

                        if (i == 4)
                        {
                            gap = 700;
                        }

                        RoadAndRiverElements[i][j] = new Car();

                        switch (i)
                        {
                            case 0:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/hotrod2.png "));
                                RoadAndRiverElements[i][j].Location = new Point(2200 - i * 40 - j * gap, 1030 - i * 88);
                                break;
                            case 1:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/tractor.inettools.net.rotate.image.png "));
                                RoadAndRiverElements[i][j].Image.Width = 80;
                                RoadAndRiverElements[i][j].Image.Height = 80;
                                RoadAndRiverElements[i][j].Location = new Point(0 + i * 40 + j * gap, 1030 - i * 88);
                                break;
                            case 2:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/hotrod.png "));
                                RoadAndRiverElements[i][j].Location = new Point(2200 - i * 40 - j * gap, 1030 - i * 88);
                                break;
                            case 3:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/car.inettools.net.rotate.image.png "));
                                RoadAndRiverElements[i][j].Image.Width = 80;
                                RoadAndRiverElements[i][j].Image.Height = 80;
                                RoadAndRiverElements[i][j].Location = new Point(0 + i * 40 + j * gap, 1030 - i * 88);
                                break;
                            case 4:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/bigrig.png "));
                                RoadAndRiverElements[i][j].Image.Width = 200;
                                RoadAndRiverElements[i][j].Image.Height = 80;
                                RoadAndRiverElements[i][j].Location = new Point(2200 - i * 40 - j * gap, 1030 - i * 88);
                                break;
                            default:
                                RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/hotrod2.png "));
                                break;
                        }

                        Canvas.SetLeft(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.X);
                        Canvas.SetTop(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.Y);
                        _myCanvas.Children.Add(RoadAndRiverElements[i][j].Image);
                    }
                }
                else if (i % 2 == 0)
                {
                    RoadAndRiverElements[i] = new Wood[NUM_OF_WOODS];
                    int heightOfRoadLocation = 0;

                    if (i == 6)
                    {
                        heightOfRoadLocation = 385;
                    }
                    else
                    {
                        heightOfRoadLocation = 180;
                    }

                    for (int j = 0; j < NUM_OF_WOODS; j++)
                    {
                        RoadAndRiverElements[i][j] = new Wood(); // ToDo ctor that gets things, like image.. etc
                        RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/log_medium.png "));
                        RoadAndRiverElements[i][j].Location = new Point(2200 - i * 40 - j * 400, heightOfRoadLocation);
                        Canvas.SetLeft(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.X);
                        Canvas.SetTop(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.Y);
                        _myCanvas.Children.Add(RoadAndRiverElements[i][j].Image);
                    }
                }
                else
                {
                    RoadAndRiverElements[i] = new Turtles[NUM_OF_TURTLES];
                    int heightOfRoadLocation = 0;

                    if (i == 5)
                    {
                        heightOfRoadLocation = 490;
                    }
                    else
                    {
                        heightOfRoadLocation = 280;
                    }

                    for (int j = 0; j < NUM_OF_TURTLES; j++)
                    {

                        RoadAndRiverElements[i][j] = new Turtles(); // ToDo ctor that gets things, like image.. etc
                        RoadAndRiverElements[i][j].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/turtles_triple.png "));
                        RoadAndRiverElements[i][j].Location = new Point(2200 - i * 40 - j * 500, heightOfRoadLocation);
                        Canvas.SetLeft(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.X);
                        Canvas.SetTop(RoadAndRiverElements[i][j].Image, RoadAndRiverElements[i][j].Location.Y);
                        _myCanvas.Children.Add(RoadAndRiverElements[i][j].Image);
                    }
                }
            }
        }

        public bool FrogiOverTheRiver()
        {
            int ind = 0;
            bool isInRange = IsInRange(ref ind);

            if (_frogi.Location.Y <= 100 && isInRange && !_bigFroggerIsOccupied[ind] && !_flagWin)
            {                
                _myCanvas.Children.Add(_bigFrogger[ind].Image);
                _bigFroggerIsOccupied[ind] = true;
                _bigFrogCounter++;
                _flagWin = true;

                if (_bigFrogCounter == 5)
                {
                    _myCanvas.Children.Remove(_frogi.Image);

                    for (int i = 0; i < _bigFrogger.Length; i++)
                    {
                        _bigFrogger[i].Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/kingFrog2.png"));
                    }

                    _isKing = true;
                    return true;
                }

                SetPositionToTheStart();
                return true;
            }
            else if (_frogi.Location.Y <= 100 && !isInRange || _frogi.Location.Y <= 100 && isInRange && _bigFroggerIsOccupied[ind])
            {
                _frogi.Image.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/green_splat.png "));
                _flagDead = true;
                RemoveElements();
                return false;
            }

            return false;
        }

        public void SetPositionToTheStart()
        {
            _frogi.Location = new Point(1115, 1120);
            Canvas.SetLeft(_frogi.Image, 1115);
            Canvas.SetTop(_frogi.Image, 1120);
            _flagWin = false;
        }

        public bool IsInRange(ref int ind)
        {
            double x = _frogi.Location.X;

            if (x >= 60 && x <= 180)
            {
                ind = 0;
                return true;
            }
            else if (x >= 580 && x <= 705)
            {
                ind = 1;
                return true;
            }
            else if (x >= 1015 && x <= 1215)
            {
                ind = 2;
                return true;
            }
            else if (x >= 1540 && x <= 1745)
            {
                ind = 3;
                return true;
            }
            else if (x >= 2060 && x <= 2165)
            {
                ind = 4;
                return true;
            }

            return false;
        }



        public void RemoveElements()
        {
            managerTimer.Tick -= Timer_Tick;
            _myCanvas.Children.Remove(_frogi.Image);

            for (int i = 0; i < RoadAndRiverElements.Length; i++)
            {
                for (int j = 0; j < RoadAndRiverElements[i].Length; j++)
                {
                    _myCanvas.Children.Remove(RoadAndRiverElements[i][j].Image);
                }
            }

            for (int i = 0; i < _bigFrogger.Length; i++)
            {
                _myCanvas.Children.Remove(_bigFrogger[i].Image);
            }
        }
    }
}

