using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace CourseProjectOOP.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PlaylistBanner.xaml
    /// </summary>
    public partial class PlaylistBanner : UserControl
    {

        public PlaylistBanner(string name, BitmapImage img, PlayList playList)
        {
            InitializeComponent();

            this.playlistName = name;
            this.img = img;
            this.sourcePlaylist = playList;
            this.videoNumb = (short)playList.videoList.Count();

            PlaylistName.Text = this.playlistName;
            PlaylistPreview.Source = this.img;
            VideoCount.Text = playList.videoList.Count.ToString() + " видео";

        }
        public string playlistName { get; set; }
        public BitmapImage img { get; set; }
        public PlayList sourcePlaylist;
        public short videoNumb;

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            main.vidList.Children.Clear();
            main.curPlaylist = this.sourcePlaylist;
            main.VideoParms.Visibility = Visibility.Hidden;
            CourseProjectOOP.Commands.Commands.videoAddToList(sourcePlaylist.videoList, main.vidList);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            main.callPlaylistEdit(this);
        }
    }

    

}
