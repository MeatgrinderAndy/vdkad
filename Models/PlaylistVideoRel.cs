using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectOOP.Classes
{
    public class PlaylistVideoRel
    {
        public Guid IdOfRel { get; set; }
        public Guid VideoId { get; set; }
        public Guid PlaylistId { get; set; }
        public PlaylistVideoRel()
        {

        }

        public PlaylistVideoRel(Guid video, Guid playlist)
        {
            this.IdOfRel = Guid.NewGuid();
            VideoId = video;
            PlaylistId = playlist;
        }
    }
}
