using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace CourseProjectOOP.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для VideoPlay.xaml
    /// </summary>
    /// 


    public partial class VideoPlay : Window
    {

        DispatcherTimer _timer = new DispatcherTimer();
        DispatcherTimer idle;
        private string videoPath;
        bool isPaused;
        short curSpeed = 1;
        TimeSpan ts;
        TimeSpan currentTime;
        TimeSpan videoLength;

        public void SetVideoPath(string videoPath)
        {
            this.videoPath = videoPath;
            this.VideoPlayer.Source = new Uri(videoPath, UriKind.Absolute);
        }
        public string GetVideoPath()
        {
            return this.videoPath;
        }

        public VideoPlay()
        {
            InitializeComponent();
            
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();

            idle = new DispatcherTimer();
            idle.Interval = TimeSpan.FromSeconds(3);
            idle.Tick += IdleTimer_Tick;

            VideoPlayer.Play();
            PPButton.Content = "❚❚";
            isPaused = false;
            SpeedOfVideo.Content = "x1";
            RestartButton.Opacity = 0;

            

            Canvas.SetZIndex(RestartButton, -10);
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            VideoControls.Opacity = 1;
            idle.Stop();
            idle.Start();
        }

        void ticktock(object sender, EventArgs e)
        {
            VideoPosition.Value = VideoPlayer.Position.TotalSeconds;
            currentTime = TimeSpan.FromSeconds(VideoPlayer.Position.TotalSeconds);
            if (currentTime <= videoLength)
                curTime.Content = currentTime.ToString(@"hh\:mm\:ss");
            else
                curTime.Content = videoLength.ToString(@"hh\:mm\:ss");
            if(VideoPosition.Value != VideoPosition.Maximum)
            {
                RestartButton.Opacity = 0;
                Canvas.SetZIndex(RestartButton, -10);
            }
        }

        void IdleTimer_Tick(object sender, EventArgs e)
        {
            VideoControls.Opacity = 0;
        }

        private void PausePlay(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                PPButton.Content = "❚❚";
                VideoPlayer.Play();
                isPaused = false;
            }
            else
            {
                PPButton.Content = "▶";
                VideoPlayer.Pause();
                isPaused = true;
            }
        }
        public bool mouseup = false;


        private void slider_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double lastSliderValue = VideoPosition.Value;

            VideoPlayer.Position = TimeSpan.FromSeconds(lastSliderValue);
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            ts = VideoPlayer.NaturalDuration.TimeSpan;
            VideoPosition.Minimum = 0;
            VideoPosition.Maximum = ts.TotalSeconds;
            videoLength = TimeSpan.FromSeconds((int)VideoPosition.Maximum);
            wholeTime.Content = videoLength;
        }

       

        private void VolumeSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            VideoPlayer.Volume = VolumeSlide.Value;
        }

        private void VideoPosition_LostMouseCapture(object sender, MouseEventArgs e)
        {
            double lastSliderValue = VideoPosition.Value;

            // Перемотка видео в соответствии с последним значением слайдера
            VideoPlayer.Position = TimeSpan.FromSeconds(lastSliderValue);
            _timer.Start();
        }

        private void SpeedOfVideo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            curSpeed++;
            switch(curSpeed % 4) {
                case 0:
                    SpeedOfVideo.Content = "x0.5";
                    VideoPlayer.SpeedRatio = 0.5;
                    break;
                case 1:
                    SpeedOfVideo.Content = "x1";
                    VideoPlayer.SpeedRatio = 1;
                    break;
                case 2:
                    SpeedOfVideo.Content = "x1.5";
                    VideoPlayer.SpeedRatio = 1.5;
                    break;
                case 3:
                    SpeedOfVideo.Content = "x2";
                    VideoPlayer.SpeedRatio = 2;
                    break;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Stop();
            VideoPlayer.Play();
            if (isPaused)
            {
                PPButton.Content = "❚❚";
                VideoPlayer.Play();
                isPaused = false;
            }
            RestartButton.Opacity = 0;
            Canvas.SetZIndex(RestartButton, -10);
            
        }

        public double posClicked;

      

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            RestartButton.Opacity = 1;
            Canvas.SetZIndex(RestartButton, 10);
        }

        private void VideoPosition_GotMouseCapture(object sender, MouseEventArgs e)
        {
            _timer.Stop();
            posClicked = VideoPosition.Value;
            VideoPosition.Value = posClicked;
            
        }
    }
}
