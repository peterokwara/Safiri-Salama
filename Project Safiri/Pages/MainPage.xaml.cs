using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Project_Safiri.Resources;
using System.Windows.Threading;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using NExtra.Geo;

namespace Project_Safiri
{
    public partial class MainPage : PhoneApplicationPage
    {

        private GeoCoordinateWatcher _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
        private MapPolyline _line;
        private DispatcherTimer _timer = new DispatcherTimer();
        private long _startTime;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Code for building a localized ApplicationBar
            BuildLocalizedApplicationBar();

            // create a line which illustrates the run
            _line = new MapPolyline();
            _line.StrokeColor = Colors.Red;
            _line.StrokeThickness = 5;
            Map.MapElements.Add(_line);

            _watcher.PositionChanged += Watcher_PositionChanged;

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var coord = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);

            TimeSpan time = TimeSpan.FromTicks(System.Environment.TickCount - _previousPositionChangeTick);
            double secondsFromTs = time.TotalSeconds;

            if (_line.Path.Count > 0)
            {
                var previousPoint = _line.Path.Last();
                var distance = coord.GetDistanceTo(previousPoint);

                _kilometres += distance / 1000.0;
                double metersPerSecond = (distance / (secondsFromTs * 1000));
                double kilometersPerHours = (metersPerSecond * 3.6);

                speedTextBlock.Text = string.Format("{0:f2} Km/H", kilometersPerHours.ToString("0.0"));
                distanceTextBlock.Text = string.Format("{0:f2} km", _kilometres);

                PositionHandler handler = new PositionHandler();
                var heading = handler.CalculateBearing(new Position(previousPoint), new Position(coord));
                Map.SetView(coord, Map.ZoomLevel, heading, MapAnimationKind.Parabolic);

            }

            else
            {
                Map.Center = coord;
            }

            _line.Path.Add(coord);
            _previousPositionChangeTick = System.Environment.TickCount;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = TimeSpan.FromMilliseconds(System.Environment.TickCount - _startTime);
            timeTextBlock.Text = runTime.ToString(@"hh\:mm\:ss");
        }

        private double _kilometres;
        private long _previousPositionChangeTick;

         //Code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            //Set the propeties of the ApplicationBar
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton help = new ApplicationBarIconButton(new Uri("/Assets/AppBar/questionmark.png", UriKind.Relative));
            help.Text = AppResources.HelpText;
            help.Click += new EventHandler(help_click);
            ApplicationBar.Buttons.Add(help);
            
            
            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton play = new ApplicationBarIconButton(new Uri("/Assets/AppBar/play.png", UriKind.Relative));
            play.Text = AppResources.PlayText;
            play.Click += new EventHandler(play_click);
            ApplicationBar.Buttons.Add(play);

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton share = new ApplicationBarIconButton(new Uri("/Assets/AppBar/share.png", UriKind.Relative));
            share.Text = AppResources.ShareText;
            share.Click += new EventHandler(share_click);
            ApplicationBar.Buttons.Add(share);

            
            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem about = new ApplicationBarMenuItem(AppResources.AboutText);
            about.Click += new EventHandler(about_click);
            ApplicationBar.MenuItems.Add(about);

            //Create a new menu item with the localized string form AppResources
            ApplicationBarMenuItem settings = new ApplicationBarMenuItem(AppResources.SettingsText);
            settings.Click += new EventHandler(settings_click);
            ApplicationBar.MenuItems.Add(settings);
        }

        private void help_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Help.xaml", UriKind.Relative));
        }

        //Event handler for the settings click event from the ApplicationBar
        private async void settings_click(object sender, EventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        //Event handler for the play click event from the ApplicationBar
        private void play_click(object sender, EventArgs e)
        {
            ApplicationBarIconButton play = (ApplicationBarIconButton)ApplicationBar.Buttons[1];

            if (play.Text == "start")
            {
                play.Text = "pause";
                play.IconUri = new Uri("/Assets/AppBar/pause.png", UriKind.Relative);

            }
            else if (play.Text == "pause")
            {
                play.Text = "start";
                play.IconUri = new Uri("/Assets/AppBar/play.png", UriKind.Relative);

            }

            if (_timer.IsEnabled)
            {
                _watcher.Stop();
                _timer.Stop();

            }
            else
            {
                _watcher.Start();
                _timer.Start();
                _startTime = System.Environment.TickCount;
            }
        }

        //Event handler for the share click event from the ApplicationBar
        private void share_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/share.xaml", UriKind.Relative));
        }

        //Event handler for the about click event from the ApplicationBar
        private void about_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/about.xaml", UriKind.Relative));
        }
    }
}