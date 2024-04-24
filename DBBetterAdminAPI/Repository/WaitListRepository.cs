using BetterAdminDbAPI.Model;
using MySqlConnector;
using Newtonsoft.Json.Linq;

namespace BetterAdminDbAPI.Repository
{
    public class WaitListRepository
    {
        private readonly string _connectionString;
        private List<WaitList> _waitLists = new();

        public WaitListRepository(string connectionString)
        {
            _connectionString = connectionString;
            _waitLists = GetAll();
        }

        public List<WaitList> GetAll()
        {
            _waitLists = new List<WaitList>();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT course_id, pupil_id, time_entered_queue FROM waitlist", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        WaitList waitList = new WaitList()
                        {
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"]),
                            TimeEnteredQueue = Convert.ToDateTime(dr["time_entered_queue"])
                        };
                        _waitLists.Add(waitList);
                    }
                }
            }

            return _waitLists;
        }

        public WaitList GetByCourseId(int courseId)
        {
            WaitList? waitListToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pupil_id, course_id, time_entered_queue FROM waitlist WHERE course_id = @course_id", con);
                cmd.Parameters.AddWithValue("@course_id", courseId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    WaitList waitList = new WaitList()
                    {
                        CourseId = Convert.ToInt32(dr["course_id"]),
                        PupilId = Convert.ToInt32(dr["pupil_id"]),
                        TimeEnteredQueue = Convert.ToDateTime(dr["time_entered_queue"])

                    };
                    waitListToReturn = waitList;
                }
            }
            return waitListToReturn;
        }

        public WaitList GetByPupilId(int pupilId)
        {
            WaitList? waitListToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pupil_id, course_id, time_entered_queue FROM waitlist WHERE pupil_id = @pupil_id", con);
                cmd.Parameters.AddWithValue("@pupil_id", pupilId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    WaitList waitList = new WaitList()
                    {
                        CourseId = Convert.ToInt32(dr["course_id"]),
                        PupilId = Convert.ToInt32(dr["pupil_id"]),
                        TimeEnteredQueue = Convert.ToDateTime(dr["time_entered_queue"])

                    };
                    waitListToReturn = waitList;
                }
            }
            return waitListToReturn;
        }

        public WaitList Create(WaitList waitListToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO waitlist (pupil_id, course_id, time_entered_queue)" +
                    "VALUES(@pupil_id, @course_id, @time_entered_queue)", con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToCreate.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToCreate.CourseId);
                cmd.Parameters.AddWithValue("@time_entered_queue", waitListToCreate.TimeEnteredQueue);

                cmd.ExecuteNonQuery();

                _waitLists.Add(waitListToCreate);
                
            }
            return _waitLists[_waitLists.Count - 1];
        }

        public WaitList Update(WaitList waitListToUpdate)
        {
            var obj = _waitLists.FirstOrDefault(x => x.CourseId == waitListToUpdate.CourseId && x.PupilId == waitListToUpdate.PupilId);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE waitlist SET time_entered_queue = @time_entered_queue WHERE pupil_id = @pupil_id AND course_id = @course_id", con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToUpdate.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToUpdate.CourseId);
                cmd.Parameters.AddWithValue("@start_time", waitListToUpdate.TimeEnteredQueue);

                cmd.ExecuteNonQuery();

                if (obj != null) obj.TimeEnteredQueue = waitListToUpdate.TimeEnteredQueue;
            }
            return obj;
        }

        public bool Delete(WaitList waitListToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM waitlist WHERE pupil_id = @pupil_id AND course_id = @course_id", con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToDelete.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToDelete.CourseId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
