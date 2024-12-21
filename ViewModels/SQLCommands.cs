using CourseProjectOOP.Classes;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CourseProjectOOP.Commands
{
    public static class SQLCommands
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        
        public static bool isEmpty()
        {
            int rowCount;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM VideosT";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    rowCount = (int)command.ExecuteScalar();
                }
            }
            if (rowCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static List<Video> ReadVideos()
        {

            List<Video> list = new List<Video>();
            string selectQuery = "Select Name, Source, Preview, Size, More, Date, Id from VideosT ";

            using (SqlConnection connection = new SqlConnection(connectionString))

                
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание нового объекта видео на основе данных из текущей строки
                            Video obj = new Video();
                            obj.setName(reader.GetString(0));
                            obj.setPath(reader.GetString(1));
                            obj.setPreviewBin((byte[])reader["Preview"]);
                            obj.setSize(reader.GetInt64(3));
                            obj.setMore(reader.GetString(4));
                            obj.setDate(reader.GetDateTime(5));
                            obj.id = reader.GetGuid(6);
                            // Добавление видео в список
                            list.Add(obj);
                        }


                        return list;
                    }
                }
            }
        }
        public static void WriteVideos(List<Video> videos)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string clearQuery = "Delete from VideosT";
                SqlCommand clear = new SqlCommand(clearQuery, connection);
                clear.ExecuteNonQuery();
                string insertQuery = "INSERT INTO VideosT (Id, Name, Source, Preview, Size, More, Date) VALUES (@Id, @Name, @Source, @Preview, @Size, @More, @Date)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    
                    foreach (Video video in videos)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", video.id);
                        command.Parameters.AddWithValue("@Name", video.name);
                        command.Parameters.AddWithValue("@Source", video.path);
                        command.Parameters.AddWithValue("@Preview", video.preview);
                        command.Parameters.AddWithValue("@Size", video.size);
                        command.Parameters.AddWithValue("@More", video.more);
                        command.Parameters.AddWithValue("@Date", video.date);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static bool playlistsIsEmpty()
        {
            int rowCount;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM PlaylistsT";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    rowCount = (int)command.ExecuteScalar();
                }
            }
            if (rowCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<PlayList> ReadPlaylists()
        {

            List<PlayList> list = new List<PlayList>();
            string selectQuery = "Select PlaylistName, VideoAmount, PlaylistPreview, Id from PlaylistsT ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание нового объекта на основе данных из текущей строки
                            PlayList obj = new PlayList();
                            obj.Id = reader.GetGuid(3);
                            obj.playlistName = reader.GetString(0);
                            obj.videoAmount = reader.GetInt16(1);
                            obj.setPreview((byte[])reader["PlaylistPreview"]);
                            
                            // Добавление объекта в список
                            list.Add(obj);
                        }
                        return list;
                    }
                }
            }
        }

        public static void WritePlaylists(List<PlayList> playlists)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string clearQuery = "Delete from PlaylistsT";
                SqlCommand clear = new SqlCommand(clearQuery, connection);
                clear.ExecuteNonQuery();
                string insertQuery = "INSERT INTO PlaylistsT (Id, PlaylistName, VideoAmount, PlaylistPreview) VALUES (@Id, @Name, @VideoAmount, @Preview)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    foreach (PlayList playlist in playlists)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", playlist.Id);
                        command.Parameters.AddWithValue("@Name", playlist.playlistName);
                        command.Parameters.AddWithValue("@VideoAmount", playlist.videoAmount);
                        command.Parameters.AddWithValue("@Preview", playlist.getPreview());

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void WriteRels(List<PlaylistVideoRel> relations)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string clearQuery = "Delete from RelationsT";
                SqlCommand clear = new SqlCommand(clearQuery, connection);
                clear.ExecuteNonQuery();
                // Создание команды для выполнения операции INSERT
                string insertQuery = "INSERT INTO RelationsT (IdOfRel, PlaylistId, VideoId) VALUES (@Id, @Playlist, @Video)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    foreach (PlaylistVideoRel rel in relations)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Id", rel.IdOfRel);
                        command.Parameters.AddWithValue("@Playlist", rel.PlaylistId);
                        command.Parameters.AddWithValue("@Video", rel.VideoId);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<PlaylistVideoRel> ReadRels()
        {

            List<PlaylistVideoRel> list = new List<PlaylistVideoRel>();
            string selectQuery = "Select IdOfRel, PlaylistId, VideoId from RelationsT ";

            using (SqlConnection connection = new SqlConnection(connectionString))


            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание нового объекта на основе данных из текущей строки
                            PlaylistVideoRel obj = new PlaylistVideoRel();
                            obj.IdOfRel = reader.GetGuid(0);
                            obj.PlaylistId = reader.GetGuid(1);
                            obj.VideoId = reader.GetGuid(2);
                            // Добавление объекта в список
                            list.Add(obj);
                        }


                        return list;
                    }
                }
            }
        }


    }
}
