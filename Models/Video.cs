using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;

namespace CourseProjectOOP
{
    public class Video
    {
        public Guid id;
        public string name;
        public string path;
        public byte[] preview;
        public long size;
        public string more;
        public DateTime date;

        public void setSize(long size)  { this.size = size; }
        public long getSize() { return this.size; }

        public void setName(string file) { this.name = file.Substring(file.LastIndexOf('\\') + 1, file.Length - file.LastIndexOf('\\') - 1); }
        public string getName() { return this.name; }

        public void setPath(string path) { this.path = path; }
        public string getPath() { return this.path; }

        public BitmapImage getPreview()
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(this.preview))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        public void setPreview(BitmapImage preview)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder(); // Используйте соответствующий кодировщик в зависимости от формата изображения
                encoder.Frames.Add(BitmapFrame.Create(preview));
                encoder.Save(memoryStream);
                this.preview = memoryStream.ToArray();
            }
        }

        public void setPreviewBin(byte[] bytes) { this.preview = bytes; }
        public void findPreview(Video video)
        {
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            var st = ShellFile.FromFilePath(video.getPath());
            var thumbn = st.Thumbnail.ExtraLargeBitmap;
            thumbn.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            setPreview(bitmapImage);
        }

        public void setMore(string more) { this.more = more; }

        public string getMore() { return this.more; }

        public void setDate(DateTime date) { this.date = date; }
        public DateTime getDate() { return this.date; }
        public Video()
        {
            this.id = Guid.NewGuid();
        }

    }


}
