using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class WaitListRepository
    {
        private readonly MySqlConnection _con;

        public WaitListRepository(MySqlConnection connection)
        {
            _con = connection;
        }

        public List<WaitList> GetAll()
        {
            List<WaitList> waitLists = new List<WaitList>();
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT course_id, pupil_id FROM waitlist", _con);
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
                        waitLists.Add(waitList);
                    }
                }
            }
            return waitLists;
        }

        public WaitList Get(int pupilId)
        {
            WaitList waitListToReturn = null;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pupil_id, course_id, time_entered_queue FROM waitlist WHERE pupil_id = @pupil_id", _con);
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

        public bool Create(WaitList waitListToCreate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO waitlist (pupil_id, course_id, time_entered_queue)" +
                    "VALUES(@pupil_id, @course_id, @time_entered_queue)", _con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToCreate.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToCreate.CourseId);
                cmd.Parameters.AddWithValue("@time_entered_queue", waitListToCreate.TimeEnteredQueue);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Update(WaitList waitListToUpdate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE waitlist SET time_entered_queue = @time_entered_queue WHERE pupil_id = @pupil_id AND course_id = @course_id", _con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToUpdate.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToUpdate.CourseId);
                cmd.Parameters.AddWithValue("@start_time", waitListToUpdate.TimeEnteredQueue);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Delete(WaitList waitListToDelete)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM waitlist WHERE pupil_id = @pupil_id AND course_id = @course_id", _con);

                cmd.Parameters.AddWithValue("@pupil_id", waitListToDelete.PupilId);
                cmd.Parameters.AddWithValue("@course_id", waitListToDelete.CourseId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
