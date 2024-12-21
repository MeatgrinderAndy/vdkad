using Microsoft.Win32;
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

namespace CourseProjectOOP.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateEditPlaylisst.xaml
    /// </summary>
    public partial class CreateEditPlaylisst : Window
    {
        public CreateEditPlaylisst()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = PlaylistNameField.Text;
                BitmapImage preview = (BitmapImage)PlaylistImage.Source;
                if (preview!= null || name != string.Empty)
                {
                    MainWindow main = (MainWindow)Application.Current.MainWindow;
                    PlayList playList = new PlayList(name, preview);
                    main.playLists.Add(playList);
                    Close();
                    main.PlaylistShow();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch 
            { 
                MessageBox.Show("Пустое поле недопустимо!", "Ошибка поля!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Фото файлы (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                BitmapImage newprev = new BitmapImage(new Uri(ofd.FileName));
                PlaylistImage.Source = newprev;
            }
        }
    }
}
