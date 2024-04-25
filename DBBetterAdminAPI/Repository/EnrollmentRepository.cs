using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class EnrollmentRepository
    {
        private readonly string _connectionString;
        private List<Enrollment> _enrollments = new();

        public EnrollmentRepository(string connectionString)
        {
            _connectionString = connectionString;
            _enrollments = GetAll();
        }

        public List<Enrollment> GetAll()
        {
            _enrollments.Clear();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT course_id, pupil_id FROM enrollment", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Enrollment enrollment = new Enrollment()
                        {
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        _enrollments.Add(enrollment);
                    }
                }
            }

            return _enrollments;
        }

        public List<Enrollment> GetByCourseId(int courseId)
        {
            List<Enrollment> resultList = new List<Enrollment>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pupil_id, course_id FROM enrollment WHERE course_id = @course_id", con);
                cmd.Parameters.AddWithValue("@course_id", courseId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Enrollment enrollment = new Enrollment()
                        {
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        resultList.Add(enrollment);
                    }
                }
            }
            return resultList;
        }

        public List<Enrollment> GetByPupilId(int pupilId)
        {
            List<Enrollment> resultList = new List<Enrollment>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT pupil_id, course_id FROM enrollment WHERE pupil_id = @pupil_id", con);
                cmd.Parameters.AddWithValue("@pupil_id", pupilId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Enrollment enrollment = new Enrollment()
                        {
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        resultList.Add(enrollment);
                    }
                }
            }
            return resultList;
        }

        public Enrollment Create(Enrollment enrollmentToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO enrollment (pupil_id, course_id)" +
                    "VALUES(@pupil_id, @course_id)", con);

                cmd.Parameters.AddWithValue("@pupil_id", enrollmentToCreate.PupilId);
                cmd.Parameters.AddWithValue("@course_id", enrollmentToCreate.CourseId);

                cmd.ExecuteNonQuery();

                _enrollments.Add(enrollmentToCreate);

            }
            return _enrollments[_enrollments.Count - 1];
        }

        public bool Delete(Enrollment enrollmentToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM enrollment WHERE pupil_id = @pupil_id AND course_id = @course_id", con);

                cmd.Parameters.AddWithValue("@pupil_id", enrollmentToDelete.PupilId);
                cmd.Parameters.AddWithValue("@course_id", enrollmentToDelete.CourseId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
