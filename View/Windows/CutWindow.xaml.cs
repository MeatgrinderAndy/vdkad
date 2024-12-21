using CourseProjectOOP.View.UserControls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using MediaInfoDotNet;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;


namespace CourseProjectOOP.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для CutWindow.xaml
    /// </summary>
    public partial class CutWindow : Window
    {
        string videoToCut;
        bool isPaused = false;
        DispatcherTimer _timer = new DispatcherTimer();
        public double posClicked;
        TimeSpan ts;
        public CutWindow(ref UserControl1 video)
        {
            InitializeComponent();

            VideoShow.Source = new Uri(video.Video.getPath(), UriKind.Absolute);
            VideoShow.Play();
            VideoShow.Volume = 0.15;
            videoToCut = video.Video.getPath();

            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
        }


        private void VideoPosition_GotMouseCapture(object sender, MouseEventArgs e)
        {
            _timer.Stop();
            posClicked = VideoPosition.Value;
            VideoPosition.Value = posClicked;
        }

        private void slider_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Захватываем последнее значение слайдера
            double lastSliderValue = VideoPosition.Value;

            // Перемотка видео в соответствии с последним значением слайдера
            VideoShow.Position = TimeSpan.FromSeconds(lastSliderValue);
        }

        void ticktock(object sender, EventArgs e)
        {
            VideoPosition.Value = VideoShow.Position.TotalSeconds;
        }

        private async void CutButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                TimeSpan startTime = TimeSpan.FromSeconds((int)Double.Parse(NewBeginning.Text));
                TimeSpan endTime = TimeSpan.FromSeconds((int)Double.Parse(NewEnding.Text));
                int sst = (int)Math.Round(Double.Parse(NewBeginning.Text));
                int eet = (int)Math.Round(Double.Parse(NewEnding.Text));
                string cutPath = videoToCut.Remove(videoToCut.LastIndexOf('.')) + "_cut.mp4" /*+ videoToCut.Substring(videoToCut.LastIndexOf('.'))*/;
                short num = 1;
                while (true)
                {
                    if (System.IO.File.Exists(cutPath))
                    {
                        cutPath = videoToCut.Remove(videoToCut.LastIndexOf('.')) + $"_cut ({num}).mp4";
                        num++;
                    }
                    else
                    {
                        break;
                    }
                }
                //if(!TimeSpan.TryParse(NewBeginning.Text, out startTime) || !TimeSpan.TryParse(NewEnding.Text, out endTime))
                //{
                  //  throw new Exception("Not right time format!");
                //}
                if(startTime.Seconds > endTime.Seconds)
                {
                    throw new Exception("Левая граница интервала не может начинаться позже чем правая!");
                }

                var task = new Task(() =>
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "D:\\Курсовой проект ООП Кантарович\\CourseProjectOOP\\packages\\ffmpeg\\bin\\ffmpeg.exe",
                        Arguments = $"-ss {sst} -to {eet} -i \"{videoToCut}\" -c copy \"{cutPath}\"", //-i \"{videoToCut}\" -ss {startTime} -to {endTime} -c:v copy -c:a copy \"{cutPath}\"",
                        WorkingDirectory = "D:\\Курсовой проект ООП Кантарович\\CourseProjectOOP\\packages\\ffmpeg\\bin\\",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        process.Start();
                        process.WaitForExit();
                    }
                });
                task.Start();
                while (!task.IsCompleted)
                {
                    await Task.Delay(1000);
                }

                await task;
            }
            catch (Exception ex)
            {
                if (ex != null) 
                {
                    MessageBox.Show(ex.Message, "Ошибка обрезки!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Произошла ошибка во время обрезки!", "Ошибка обрезки!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
    }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                VideoShow.Play();
                isPaused = false;
            }
            else
            {
                VideoShow.Pause();
                isPaused = true;
            }
        }

        private void VideoPosition_LostMouseCapture(object sender, MouseEventArgs e)
        {
            double lastSliderValue = VideoPosition.Value;

            // Перемотка видео в соответствии с последним значением слайдера
            VideoShow.Position = TimeSpan.FromSeconds(lastSliderValue);
            _timer.Start();
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            ts = VideoShow.NaturalDuration.TimeSpan;
            VideoPosition.Minimum = 0;
            VideoPosition.Maximum = ts.TotalSeconds;

        }

        private void PointButton_Click(object sender, RoutedEventArgs e)
        {
            if (bChoosed)
            {
                NewBeginning.Text = VideoPosition.Value.ToString();
                bChoosed = false;
            }
            else if (eChoosed)
            {
                NewEnding.Text = VideoPosition.Value.ToString();
                eChoosed = false;
            }
        }

        bool bChoosed = false;
        bool eChoosed = false;

        private void NewBeginning_GotFocus(object sender, RoutedEventArgs e)
        {
            bChoosed = true;
            eChoosed = false;
        }

        private void NewEnding_GotFocus(object sender, RoutedEventArgs e)
        {
            eChoosed = true;
            bChoosed = false;
        }

        private void ProcessStopButton_Click(object sender, RoutedEventArgs e)
        {
            string ffmpeg = "D:\\Курсовой проект ООП Кантарович\\CourseProjectOOP\\packages\\ffmpeg\\bin\\ffmpeg.exe";
            Process[] processes = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(ffmpeg));
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}

