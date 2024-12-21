using CourseProjectOOP.Classes;
using CourseProjectOOP.View.UserControls;
using CourseProjectOOP.View.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;




namespace CourseProjectOOP.Commands
{
    public class Commands
    {
        static Commands()
        {
            Add = new RoutedCommand("Add", typeof(MainWindow));
            Find = new RoutedCommand("Find", typeof(MainWindow));
            SortByName = new RoutedCommand("SortByName", typeof(MainWindow));
            SortBySize = new RoutedCommand("SortBySize", typeof(MainWindow));
            SortByDate = new RoutedCommand("SortByDate", typeof(MainWindow));
            CutOpen = new RoutedCommand("CutOpen", typeof(MainWindow));
            Close = new RoutedCommand("Close", typeof (MainWindow));
            ChangeTheme = new RoutedCommand("ChangeTheme", typeof(MainWindow));
            Language = new RoutedCommand("Language", typeof( MainWindow));
            WindowSize = new RoutedCommand("WindowSize", typeof(MainWindow));
            SaveTables = new RoutedCommand("Save", typeof(MainWindow));
        }
        public static RoutedCommand SaveTables { get; set; }
        public static RoutedCommand Add { get; set; }
        public static RoutedCommand Find { get; set; }
        public static RoutedCommand SortByName { get; set; }
        public static RoutedCommand SortBySize { get; set; }
        public static RoutedCommand SortByDate { get; set; }
        public static RoutedCommand CutOpen { get; set; }
        public static RoutedCommand Close { get; set; }
        public static RoutedCommand ChangeTheme { get; }
        public static RoutedCommand Language { get; }
        public static RoutedCommand WindowSize { get; }
        


        public static bool FridayNight = true;
        public static bool rusLang = true;

        public static Video CreateVideo(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            DateTime date = fileInfo.CreationTime;
            Video video = new Video();
            video.setName(path);
            video.setPath(path);
            video.setSize(fileInfo.Length / 1024);
            video.setDate(date);
            video.setPreview(GetBitmapImage(video));
            video.setMore(string.Empty);

            return video;
        }

        public static bool IsHere(List<Video> videos, Video video)
        {
            foreach (Video vid in videos)
            {
                if (vid.getPath() == video.getPath())
                {
                    return true;
                }
            }
            return false;
        }

        public static void videoAddToList(List<Video> videos, WrapPanel vidList)
        {
            try
            {
                MainWindow main = (MainWindow)Application.Current.MainWindow;
                main.VideoParms.Visibility = Visibility.Visible;
                main.SortingParms.Visibility = Visibility.Visible;
                Panel.SetZIndex(main.playlistEditPanel, -5);
                main.curVideos = videos;
                main.onPlaylists = false;
                if (videos == main.videos)
                    main.curPlaylist = null;
                List<Video> videosBuf = new List<Video>(videos);
                foreach (Video video in videosBuf)
                {
                    if (System.IO.File.Exists(video.path))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        if (video.preview == null)
                        {
                            //Get Preview
                            bitmapImage = GetBitmapImage(video);
                            video.setPreview(bitmapImage);
                        }
                        else
                        {
                            bitmapImage = video.getPreview();
                        }
                        //Add Video 
                        UserControl1 vidBanner = new UserControl1(video.getName(), video.getPath(), bitmapImage, video);
                        vidList.Children.Add(vidBanner);
                    }
                    else
                    {
                        videos.Remove(video);
                    }
                }
            } catch 
            { 
                    
            }
        }

        public static void Add_Execute(WrapPanel vidlist, MainWindow main)
        {
            if (!main.onPlaylists)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Видео файлы (*.mp4;*.mkv;*.mov;*.wmv)|*.mp4;*.mkv;*.mov;*.wmv";
                Nullable<bool> result = ofd.ShowDialog();
                if (result == true && (ofd.FileName.Contains(".mp4") || ofd.FileName.Contains(".mkv") || ofd.FileName.Contains(".mov") || ofd.FileName.Contains(".wmv")))
                {
                    Video video = new Video();
                    video = CreateVideo(ofd.FileName);
                    if (!IsHere(main.videos, video))
                    {
                        main.videos.Add(video);
                    }
                    SQLCommands.WriteVideos(main.videos);
                    vidlist.Children.Clear();
                    videoAddToList(main.videos, vidlist);
                }
                else if (result == true)
                {
                    MessageBox.Show("Формат не поддерживается!", "Ошибка формата", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                CreateEditPlaylisst createPlaylisst = new CreateEditPlaylisst();
                createPlaylisst.Show();
            }
        }

        public static void AddPlaylist_Execute()
        {

        }
        public static BitmapImage GetBitmapImage(Video video)
        {
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            var st = ShellFile.FromFilePath(video.getPath());
            var thumbn = st.Thumbnail.ExtraLargeBitmap;
            thumbn.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static void Find_Execute(WrapPanel vidList, TextBox findBar, List<Video> videos, MainWindow mw)
        {
            vidList.Children.Clear();
            if (findBar.Text.Length == 0)
            {
                videoAddToList(videos, vidList);
            }
            else
            {
                foreach (Video video in videos)
                {
                    if (video.getName().Contains(findBar.Text))
                    {
                        
                        BitmapImage bitmapImage = new BitmapImage();
                        if (video.preview == null)
                        {
                            bitmapImage = GetBitmapImage(video);
                            video.setPreview(bitmapImage);
                        }
                        else
                        {
                            bitmapImage = video.getPreview();
                        }

                        UserControl1 vidBanner = new UserControl1(video.getName(), video.getPath(), bitmapImage, video);
                        vidList.Children.Add(vidBanner);
                    }
                }
            }

        }

        public static void SortName_Execute(List<Video> videos, WrapPanel vidList)
        {
            videos.Sort(new Comparison<Video>((x, y) => string.Compare(x.getName(), y.getName())));
            vidList.Children.Clear();
            videoAddToList(videos, vidList);
        }

        public static void SortSize_Execute(List<Video> videos, WrapPanel vidList)
        {
            videos.Sort((x, y) => x.getSize().CompareTo(y.getSize()));
            vidList.Children.Clear();
            videoAddToList(videos, vidList);
        }

        public static void SortDate_Execute(List<Video> videos, WrapPanel vidList)
        {
            videos.Sort((x, y) => x.getDate().CompareTo(y.getDate()));
            vidList.Children.Clear();
            videoAddToList(videos, vidList);
        }

        public void callEdit(Edit editPanel, UserControl1 vid)
        {
            Panel.SetZIndex(editPanel, 10);
            editPanel.vidNameDef = vid.vidName.Text;
            editPanel.getVid(vid);
        }

        public static void CUTButton_Execute()
        {
            
        }
        
        public static void videoSearch(ref List<Video> videos)
        {   
            try
            {
                string[] formats = { "*.mp4", "*.mkv", "*.mov", "*.wmv" };
                if (SQLCommands.isEmpty())
                {
                    videos.Clear();
                    string[] entries = Directory.GetFileSystemEntries("C:\\Users\\ASUS\\Videos", "*", SearchOption.AllDirectories);
                    List<string> files = new List<string>();
                    foreach(string format in formats) { files.AddRange(Directory.GetFiles("C:\\Users\\ASUS\\Videos", format)); }
                    
                    foreach (string entry in entries)
                    {
                        if (System.IO.File.GetAttributes(entry).HasFlag(FileAttributes.Directory))
                        {
                            foreach (string format in formats){ files.AddRange(System.IO.Directory.GetFiles(entry, format)); }
                        }
                    }
                    foreach (string file in files)
                    {
                        Video video = new Video();
                        video = CreateVideo(file);
                        videos.Add(video);
                    }
                    SQLCommands.WriteVideos(videos);
                }
                else
                {
                    videos = SQLCommands.ReadVideos();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при поиске видео на устройстве!", "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void playlistRead(ref List<PlayList> playlists)
        {
            if (!SQLCommands.playlistsIsEmpty())
            {
                playlists.Clear();
                playlists = SQLCommands.ReadPlaylists();     
            }
        }

        public static void relationRead(ref List<PlaylistVideoRel> relations, MainWindow main)
        {
            if (!SQLCommands.playlistsIsEmpty())
            {
                relations.Clear();
                relations = SQLCommands.ReadRels();
                foreach (PlayList playlist in main.playLists)
                {
                    List<Guid> videoIds = relations
                        .Where(pv => pv.PlaylistId == playlist.Id)
                        .Select(pv => pv.VideoId)
                        .ToList();

                    List<Video> playlistVideos = main.videos
                        .Where(video => videoIds.Contains(video.id))
                        .ToList();

                    playlist.videoList = playlistVideos;
                }
            }
        }

        public static byte[] ConvertBitmapImageToBytes(BitmapImage bitmapImage)
        {
            byte[] imageBytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder(); // Используйте другой формат, если нужно (например, PngBitmapEncoder)
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            return imageBytes;
        }

        public static BitmapImage ConvertBytesToBitmapImage(byte[] imageBytes)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        public static void Close_Execute(MainWindow mw)
        {
           Save_Execute(mw.videos, mw.playLists, mw.relations);
            mw.Close();
        }

        public static void Save_Execute(List<Video> videos, List<PlayList> playlists, List<PlaylistVideoRel> relations)
        {
            try { 
            SQLCommands.WriteVideos(videos);
            SQLCommands.WritePlaylists(playlists);
            SQLCommands.WriteRels(relations);
            }
            catch
            {
                MessageBox.Show("Ошибка в сохранении!", "Ошибка сохранения!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ChangeTheme_Execute(MainWindow mw)
        {
            if (FridayNight)
            {
                mw.Resources.MergedDictionaries.Remove(mw.night);
                mw.Resources.MergedDictionaries.Add(mw.day);
                FridayNight = false;
            }
            else
            {
                mw.Resources.MergedDictionaries.Remove(mw.day);
                mw.Resources.MergedDictionaries.Add(mw.night);
                FridayNight = true;
            }
        }

        public static void Language_Execute(MainWindow mw)
        {
            if (rusLang)
            {
                mw.Resources.MergedDictionaries.Remove(mw.dictRus);
                mw.Resources.MergedDictionaries.Add(mw.dictEng);
                rusLang = false;
            }
            else
            {
                mw.Resources.MergedDictionaries.Remove(mw.dictEng);
                mw.Resources.MergedDictionaries.Add(mw.dictRus);
                rusLang = true;
            }

        }

        public static void FullWindow_Execute(MainWindow mw)
        {

            if (mw.WindowState == WindowState.Maximized)
            {
                mw.WindowState = mw.prevsize;
            }
            else
            {
                mw.prevsize = mw.WindowState;
                mw.WindowState = WindowState.Maximized;
            }
        }



    }
}
