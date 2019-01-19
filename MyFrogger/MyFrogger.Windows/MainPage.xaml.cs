using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyFrogger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //data members
        private GameManager _manager;
        private bool _pausePressedOnce;
        private bool _startEnterdTwice;

        public MainPage()
        {
            _pausePressedOnce = false;
            _startEnterdTwice = false;
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            buttonMeeting.Visibility = Visibility.Collapsed;
            imageMeeting.Visibility = Visibility.Collapsed;
            _manager = new GameManager(MainCanvas);
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (!_manager.FlagDead)
            {
                if (_manager.IsKing)
                {
                    buttonMeeting.Visibility = Visibility.Visible;
                }

                _manager.MoveFrogy(args);
            }
        }

        private void startButtun_Click(object sender, RoutedEventArgs e)
        {
            if (_startEnterdTwice)
            {
                _manager.RemoveElements();
            }
            imageMeeting.Visibility = Visibility.Collapsed;
            startButton.Visibility = Visibility.Collapsed;
            mediaElement.Play();
            _manager.StartNewGame();
            _startEnterdTwice = true;
        }

        private void endGame_Click(object sender, RoutedEventArgs e)
        {
            _manager.RemoveElements();
            mediaElement.Stop();
            startButton.Visibility = Visibility.Visible;
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_pausePressedOnce)
            {
                _manager.PauseGame();
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
                pauseButton.Content = "RESUME";
                _pausePressedOnce = true;
            }
            else
            {
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
                _manager.ResumeGame();
                pauseButton.Content = "PAUSE";
                _pausePressedOnce = false;
            }
        }

        private void buttonMeeting_Click(object sender, RoutedEventArgs e)
        {
            buttonMeeting.Visibility = Visibility.Collapsed;
            imageMeeting.Visibility = Visibility.Visible;
            startButton.Visibility = Visibility.Visible;
        }

        private void startButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imageMeeting.Visibility = Visibility.Collapsed;
        }

        private void loadButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            imageMeeting.Visibility = Visibility.Collapsed;
        }

        private void imageMeeting_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            buttonMeeting.Visibility = Visibility.Collapsed;
        }

        private void MainCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_manager.IsKing)
            {
                buttonMeeting.Visibility = Visibility.Visible;
            }

            if (_manager.FlagDead)
            {
                //manager.RemoveElements();
                mediaElement.Stop();
                startButton.Visibility = Visibility.Visible;
            }
        }

        private void PlayMedia()
        {
            mediaElement.Play();
        }
    }
}
