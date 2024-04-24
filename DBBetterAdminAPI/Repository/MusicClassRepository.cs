using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class MusicClassRepository
    {
        private readonly string _connectionString;
        private List<MusicClass> _musicClasses = new();

        public MusicClassRepository(string connectionString)
        {
            _connectionString = connectionString;
            _musicClasses = GetAll();
        }

        public List<MusicClass> GetAll()
        {
            _musicClasses = new List<MusicClass>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT class_id, start_time, end_time, cancelled, course_id, pupil_id FROM class", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        MusicClass musicClass = new MusicClass()
                        {
                            MusicClassId = Convert.ToInt32(dr["class_id"]),
                            StartTime = Convert.ToDateTime(dr["start_time"]),
                            EndTime = Convert.ToDateTime(dr["end_time"]),
                            Cancelled = Convert.ToBoolean(dr["cancelled"]),
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])

                        };
                        _musicClasses.Add(musicClass);
                    }
                }
            }
            return _musicClasses;
        }

        public MusicClass Get(int id)
        {
            MusicClass? musicClassToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT class_id, start_time, end_time, cancelled, course_id, pupil_id FROM class WHERE class_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    MusicClass musicClass = new MusicClass()
                    {
                        MusicClassId = Convert.ToInt32(dr["class_id"]),
                        StartTime = Convert.ToDateTime(dr["start_time"]),
                        EndTime = Convert.ToDateTime(dr["end_time"]),
                        Cancelled = Convert.ToBoolean(dr["cancelled"]),
                        CourseId = Convert.ToInt32(dr["course_id"]),
                        PupilId = Convert.ToInt32(dr["pupil_id"])

                    };
                    musicClassToReturn = musicClass;
                }
            }
            return musicClassToReturn;
        }

        public MusicClass Create(MusicClass musicClassToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO class (start_time, end_time, cancelled, course_id, pupil_id)" +
                    "VALUES(@start_time, @end_time, @cancelled, @course_id, @pupil_id)", con);

                cmd.Parameters.AddWithValue("@start_time", musicClassToCreate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", musicClassToCreate.EndTime);
                cmd.Parameters.AddWithValue("@cancelled", musicClassToCreate.Cancelled);
                cmd.Parameters.AddWithValue("@course_id", musicClassToCreate.CourseId);
                cmd.Parameters.AddWithValue("@pupil_id", musicClassToCreate.PupilId);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _musicClasses.Add(musicClassToCreate);
                _musicClasses[_musicClasses.Count - 1].MusicClassId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _musicClasses[_musicClasses.Count - 1];
        }

        public MusicClass Update(MusicClass musicClassToUpdate)
        {
            MusicClass? obj = _musicClasses.FirstOrDefault(x => x.MusicClassId == musicClassToUpdate.MusicClassId);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE class SET start_time = @start_time, end_time = @end_time, cancelled = @cancelled, course_id = @course_id, pupil_id = @pupil_id WHERE class_id = @id", con);

                cmd.Parameters.AddWithValue("@id", musicClassToUpdate.MusicClassId);
                cmd.Parameters.AddWithValue("@start_time", musicClassToUpdate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", musicClassToUpdate.EndTime);
                cmd.Parameters.AddWithValue("@cancelled", musicClassToUpdate.Cancelled);
                cmd.Parameters.AddWithValue("@course_id", musicClassToUpdate.CourseId);
                cmd.Parameters.AddWithValue("@pupil_id", musicClassToUpdate.PupilId);

                cmd.ExecuteNonQuery();

                if (obj != null)
                {
                    obj.StartTime = musicClassToUpdate.StartTime;
                    obj.EndTime = musicClassToUpdate.EndTime;
                    obj.Cancelled = musicClassToUpdate.Cancelled;
                    obj.CourseId = musicClassToUpdate.CourseId;
                    obj.PupilId = musicClassToUpdate.PupilId;
                }
            }
            return obj;
        }

        public bool Delete(MusicClass musicClassToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM class WHERE class_id = @id", con);

                cmd.Parameters.AddWithValue("@id", musicClassToDelete.MusicClassId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}

