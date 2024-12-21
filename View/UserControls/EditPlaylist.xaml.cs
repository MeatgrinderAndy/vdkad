using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProjectOOP.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EditPlaylist.xaml
    /// </summary>
    public partial class EditPlaylist : UserControl
    {
        public PlaylistBanner playlistEdit;
        public EditPlaylist()
        {
            InitializeComponent();
        }

        public void getPlaylist(PlaylistBanner pll)
        {
            try
            {
                playlistEdit = pll;
                nameEnter.Text = pll.playlistName;
            }
            catch 
            {
                MessageBox.Show("Ошибка при получении плейлиста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nameEnter.Text != string.Empty)
                {
                    playlistEdit.sourcePlaylist.playlistName = nameEnter.Text;
                    Panel.SetZIndex(this, -10);
                    show();
                }
                else
                {
                    MessageBox.Show("Пустое поле имени!", "Пустое поле!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при переименовании плейлиста", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
        }

        private void show()
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            main.PlaylistShow();
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void PreviewImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Фото файлы (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
                Nullable<bool> result = ofd.ShowDialog();
                if (result == true)
                {
                    BitmapImage newprev = new BitmapImage(new Uri(ofd.FileName));
                    playlistEdit.sourcePlaylist.playlistPreview = newprev;
                }
                show();
            }
            catch
            {
                MessageBox.Show("Ошибка при смене обложки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            MessageBoxResult res = MessageBox.Show($"Вы уверены что хотите удалить плейлист '{this.playlistEdit.Name}'?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    main.playLists.Remove(playlistEdit.sourcePlaylist);
                    show();
                    Panel.SetZIndex(this, -10);
                    break;
                case MessageBoxResult.No:
                    break;

            }
            
        }

        private void AddVideoButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Window.GetWindow(this);
            main.vidList.Children.Clear();
            main.toChoose = true;
            main.playlistToAdd = playlistEdit.sourcePlaylist;
            CourseProjectOOP.Commands.Commands.videoAddToList(main.videos, main.vidList);
        }
    }
}
