using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CourseProjectOOP
{
    public class PlayList
    {
        public Guid Id { get; set; }
        public string playlistName { get; set; }
        public short videoAmount { get; set; }
        public List<Video> videoList { get; set; }
        public BitmapImage playlistPreview { get; set; }
        
        public PlayList(string name, BitmapImage img) 
        { 
            this.Id = Guid.NewGuid();
            this.playlistName = name;
            this.playlistPreview = img;
            this.videoList = new List<Video>();
            this.videoAmount = (short)videoList.Count;
        }
        public void setPreview(byte[] preview)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(preview))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = memoryStream;
                img.EndInit();
            }
            this.playlistPreview = img;
        }
        public byte[] getPreview()
        {
            byte[] preview;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder(); 
                encoder.Frames.Add(BitmapFrame.Create(this.playlistPreview));
                encoder.Save(memoryStream);
                preview = memoryStream.ToArray();
            }                
            return preview;
        }
        public PlayList() { }

    }
}
