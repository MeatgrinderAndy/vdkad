using CourseProjectOOP.View.UserControls;
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
    /// Логика взаимодействия для More.xaml
    /// </summary>

    public partial class More : Window
    {

        UserControl1 thisVid;

        public More(ref UserControl1 video)
        {
            InitializeComponent();
            previewVid.Source = video.img;
            thisVid = video;
            MoreField.Text = thisVid.Video.more;
            VideoName.Content = "Название: " + thisVid.Video.name;
            VideoPath.Content = "Расположение: " + thisVid.Video.path;
            VideoDate.Content = "Добавлено: " + thisVid.Video.date;
            VideoSize.Content = "Размер: " + Math.Round((double)thisVid.Video.size/1024.0, 2) + " MБ";
        }

        private void Choose_Thumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Фото файлы (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
            Nullable<bool> result = ofd.ShowDialog();
            if(result == true ) 
            {
                BitmapImage newprev = new BitmapImage(new Uri(ofd.FileName));    
                previewVid.Source = newprev;
            }
        }

        private void save_changes_Click(object sender, RoutedEventArgs e)
        {
            thisVid.img = (BitmapImage)previewVid.Source;
            thisVid.Video.setPreview(thisVid.img);
            thisVid.Video.more = MoreField.Text;
            this.Close();
        }
    }
}
