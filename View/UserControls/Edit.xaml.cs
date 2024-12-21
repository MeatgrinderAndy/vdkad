using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using CourseProjectOOP.View.Windows;
using CourseProjectOOP.Commands;

namespace CourseProjectOOP.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : UserControl
    {
        public string curEdit;
        public UserControl1 vidEdit;
       
        public void getVid(UserControl1 vid)
        {
            vidEdit = vid;
            curEdit = vid.vidName.Text;
            nameEnter.Text = vid.videoName.Remove(vid.videoName.LastIndexOf('.'));
        }

        public void changeName(UserControl1 video, string newName, string format)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            try
            {
                if (video.vidName.Text != newName + format)
                {
                    int index = main.videos.IndexOf(video.Video);
                    FileSystem.RenameFile(video.videoPath, newName + format);
                    video.vidName.Text = newName + format;
                    video.videoPath = video.videoPath.Replace(video.Video.getName(), newName + format);
                    video.Video.setName(video.videoPath);
                    video.Video.setPath(video.videoPath);
                    main.videos[index] = video.Video;
                    SQLCommands.WriteVideos(main.videos);
                }
            }
            catch
            {
                MessageBox.Show("В новом имени присутствует недопустимый символ!", "Ошибка нового имени!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Edit()
        {
            
            InitializeComponent();
            this.DataContext = this;
        }

        public string vidNameDef { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            if (vidEdit.vidName.Text.Contains(".mp4"))
            {
                changeName(vidEdit, nameEnter.Text, ".mp4");
            }
            else if (vidEdit.vidName.Text.Contains(".mkv"))
            {
                changeName(vidEdit, nameEnter.Text, ".mkv");
            }
            else if (vidEdit.vidName.Text.Contains(".mov"))
            {
                changeName(vidEdit, nameEnter.Text, ".mov");
            }
            else if (vidEdit.vidName.Text.Contains(".wmv"))
            {
                changeName(vidEdit, nameEnter.Text, ".wmv");
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)(Window.GetWindow(this));
            main.vidList.Children.Clear();
            if (main.curVideos != main.videos)
            {
                main.curPlaylist.videoList.Remove(vidEdit.Video);
                main.relations.RemoveAll(obj => obj.VideoId == vidEdit.Video.id && obj.PlaylistId == main.curPlaylist.Id);
                CourseProjectOOP.Commands.Commands.videoAddToList(main.curVideos, main.vidList);
            }
            else
            {
                main.videos.Remove(vidEdit.Video);
                CourseProjectOOP.Commands.Commands.videoAddToList(main.videos, main.vidList);
            }
           
            
            SQLCommands.WriteVideos(main.videos);
            
            CancelButton_Click(sender, e);
        }

        private void More_Button_Click(object sender, RoutedEventArgs e)
        {
            More window_more = new More(ref vidEdit);
            window_more.Show();
        }

        private void Cut_Button_Click(object sender, RoutedEventArgs e)
        {
            CutWindow cutWindow = new CutWindow(ref vidEdit);
            cutWindow.Show();
        }
    }
}
