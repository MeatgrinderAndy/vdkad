using CourseProjectOOP;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CourseProjectOOP.View.UserControls;
using System.Data.SqlClient;
using CourseProjectOOP.Commands;
using CourseProjectOOP.Classes;

namespace CourseProjectOOP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.Add_Execute(vidList, this);
        }

        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.Find_Execute(vidList, findBar, curVideos, this);
        }

        private void SortName_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.SortName_Execute(curVideos, vidList);
        }
        private void SortSize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.SortSize_Execute(curVideos, vidList);
        }
        private void SortDate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.SortDate_Execute(curVideos, vidList);
        }


        private void CUTButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.CUTButton_Execute();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.Close_Execute(this);
        }

        private void ChangeTheme_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.ChangeTheme_Execute(this);
        }

        private void ChangeLanguage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.Language_Execute(this);
        }

        private void ChangeSize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.FullWindow_Execute(this);
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CourseProjectOOP.Commands.Commands.Save_Execute(videos, playLists, relations);
        }

        public ResourceDictionary dictEng;
        public ResourceDictionary dictRus;

        public ResourceDictionary night;
        public ResourceDictionary day;
        
        public WindowState prevsize;

        public bool onPlaylists = false;

        public bool toChoose = false;
        public PlayList playlistToAdd;   

        public List<Video> videos = new List<Video>();
        public List<PlayList> playLists = new List<PlayList>();
        public List<PlaylistVideoRel> relations = new List<PlaylistVideoRel>();
        public PlayList curPlaylist;
        public List<Video> curVideos;
        
        static string connectionString = "Data Source=DESKTOP-BJ2TUNN\\SQLEXPRESS;Initial Catalog=VIDEO_TABLE;User ID=sa;Password=1111;";
        public SqlConnection connection = new SqlConnection(connectionString);

        public void callEdit(UserControl1 vid)
        {
            editPanel.Visibility = Visibility.Visible;
            playlistEditPanel.Visibility = Visibility.Hidden;
            editPanel.vidNameDef = vid.vidName.Text;
            editPanel.getVid(vid);
        }

        public void callPlaylistEdit(PlaylistBanner pll)
        {
            
            editPanel.Visibility = Visibility.Hidden;
            playlistEditPanel.Visibility = Visibility.Visible;
            Panel.SetZIndex(playlistEditPanel, 10);
            
            playlistEditPanel.getPlaylist(pll);
        }

        

        public static RoutedCommand openDBWindow { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CourseProjectOOP.Commands.Commands.videoSearch(ref videos);
            CourseProjectOOP.Commands.Commands.playlistRead(ref playLists);
            CourseProjectOOP.Commands.Commands.relationRead(ref relations, this);

            CourseProjectOOP.Commands.Commands.videoAddToList(videos, vidList);
            dictEng = new ResourceDictionary() { Source = new Uri("Languages\\EngLang.xaml", UriKind.Relative) };
            dictRus = new ResourceDictionary() { Source = new Uri("Languages\\RusLang.xaml", UriKind.Relative) };
            night = new ResourceDictionary() { Source = new Uri("Themes\\night.xaml", UriKind.Relative) };
            day = new ResourceDictionary() { Source = new Uri("Themes\\day.xaml", UriKind.Relative) };
            Resources.MergedDictionaries.Add(dictRus);
            Resources.MergedDictionaries.Add(night);
            connection.Open();

        }


        ///////////////////////////////


        private void HideWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        

        private void CUTButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VideosButton_Click(object sender, RoutedEventArgs e)
        {
            vidList.Children.Clear();
            CourseProjectOOP.Commands.Commands.videoAddToList(videos, vidList);
            onPlaylists = false;
        }

        private void PlayListsButton_Click(object sender, RoutedEventArgs e)
        {
            PlaylistShow();
            onPlaylists = true;
        }

        public void PlaylistShow() {
            SortingParms.Visibility = Visibility.Hidden;
            vidList.Children.Clear();
            VideoParms.Visibility = Visibility.Visible;
            try
            {
                List<PlayList> playlistsBuf = new List<PlayList>(playLists);
                foreach (PlayList playlist in playlistsBuf)
                {

                    BitmapImage bitmapImage = new BitmapImage();
                    if (playlist.playlistPreview == null)
                    {
                        //Get Preview
                        bitmapImage = null;
                    }
                    else
                    {
                        bitmapImage = playlist.playlistPreview;
                    }
                    //Add playlist
                    PlaylistBanner PlaylistBanner = new PlaylistBanner(playlist.playlistName, playlist.playlistPreview, playlist);
                    vidList.Children.Add(PlaylistBanner);
                }
            }
            catch
            {

            }
            
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialog = MessageBox.Show("Очистить весь список видео?", "Очистка!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (dialog)
            {
                case MessageBoxResult.Yes:
                    videos.Clear();
                    vidList.Children.Clear();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            

        }
    }
}
