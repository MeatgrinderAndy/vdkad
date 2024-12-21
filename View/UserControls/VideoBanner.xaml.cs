using CourseProjectOOP.Classes;
using CourseProjectOOP.View.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace CourseProjectOOP.View.UserControls
{
    public partial class UserControl1 : UserControl
    {

        public UserControl1(string name, string path, BitmapImage pic, Video vid)
        {
            InitializeComponent();
            this.DataContext = this; 

            this.videoName = name;
            this.videoPath = path;
            this.img = pic;
            this.Video = vid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           MainWindow main = (MainWindow)Window.GetWindow(this);
           main.callEdit(this);
          
        }

        private void OpenVideo(object sender, RoutedEventArgs e)
        {
            MainWindow main = ( MainWindow)Window.GetWindow(this);
            if(!main.toChoose)
            {
                try
                {
                    VideoPlay window = new VideoPlay();

                    window.SetVideoPath(videoPath.ToString());
                    window.Show();
                }
                catch
                {
                    
                }
            }
            else
            {
                if (!main.playlistToAdd.videoList.Contains(this.Video)) {
                    main.relations.Add(new PlaylistVideoRel(this.Video.id, main.playlistToAdd.Id));
                    main.playlistToAdd.videoList.Add(this.Video);
                    main.toChoose = false;
                    main.vidList.Children.Clear();
                    main.playlistToAdd.videoAmount++;
                    main.PlaylistShow();
                }
                else
                {
                    main.toChoose = false;
                    main.vidList.Children.Clear();
                    main.PlaylistShow();
                    MessageBox.Show($"Видео уже есть в коллекции '{main.playlistToAdd.playlistName}'!", "Дубликат видео!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        public string videoName { get; set; }
        public BitmapImage img {  get; set; }
        public string videoPath { get; set; }
        public Button btn { get; set; }
        public Video Video { get; set; }

        
    }
}
