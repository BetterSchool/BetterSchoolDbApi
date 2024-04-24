using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class CourseRepository
    {
        private readonly string _connectionString;
        private List<Course> _courses = new();

        public CourseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _courses = GetAll();
        }

        public List<Course> GetAll()
        {
            _courses.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT course_id, course_name, max_enrolled, price, start_date, end_date, teacher_id, classroom_name FROM course", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Course course = new Course()
                        {
                            CourseId = Convert.ToInt32(dr["course_id"]),
                            CourseName = dr["course_name"].ToString()!,
                            StartDate = Convert.ToDateTime(dr["start_date"]),
                            EndDate = Convert.ToDateTime(dr["end_date"]),
                            MaxEnrolled = Convert.ToInt32(dr["concert_location"]),
                            Price = Convert.ToDecimal(dr["price"]),
                            TeacherId = Convert.ToInt32(dr["teacher_id"]),
                            ClassroomName = dr["classroom_name"].ToString()!

                        };
                        _courses.Add(course);
                    }
                }
            }
            return _courses;
        }

        public Course Get(int id)
        {
            Course? courseToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT course_id, course_name, max_enrolled, price, start_date, end_date, teacher_id, classroom_name FROM course WHERE course_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    Course course = new Course()
                    {
                        CourseId = Convert.ToInt32(dr["course_id"]),
                        CourseName = dr["course_name"].ToString()!,
                        StartDate = Convert.ToDateTime(dr["start_date"]),
                        EndDate = Convert.ToDateTime(dr["end_date"]),
                        MaxEnrolled = Convert.ToInt32(dr["concert_location"]),
                        Price = Convert.ToDecimal(dr["price"]),
                        TeacherId = Convert.ToInt32(dr["teacher_id"]),
                        ClassroomName = dr["classroom_name"].ToString()!

                    };
                    courseToReturn = course;
                }
            }
            return courseToReturn;
        }

        public Course Create(Course courseToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO concert (course_name, max_enrolled, price, start_date, end_date, teacher_id, classroom_name)" +
                    "VALUES(@course_name, @max_enrolled, @price, @start_date, @end_date, @teacher_id, @classroom_name)", con);

                cmd.Parameters.AddWithValue("@course_name", courseToCreate.CourseName);
                cmd.Parameters.AddWithValue("@max_enrolled", courseToCreate.MaxEnrolled);
                cmd.Parameters.AddWithValue("@price", courseToCreate.Price);
                cmd.Parameters.AddWithValue("@start_date", courseToCreate.StartDate);
                cmd.Parameters.AddWithValue("@end_date", courseToCreate.EndDate);
                cmd.Parameters.AddWithValue("@teacher_id", courseToCreate.TeacherId);
                cmd.Parameters.AddWithValue("@classroom_name", courseToCreate.ClassroomName);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _courses.Add(courseToCreate);
                _courses[_courses.Count - 1].CourseId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _courses[_courses.Count - 1];
        }

        public Course Update(Course courseToUpdate)
        {
            Course? obj = _courses.FirstOrDefault(x => x.CourseId == courseToUpdate.CourseId);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE course SET course_name = @course_name, max_enrolled = @max_enrolled, price = @price, start_date = @start_date, end_date = @end_date, teacher_id = @teacher_id, classroom_name = @classroom_name WHERE course_id = @id", con);

                cmd.Parameters.AddWithValue("@id", courseToUpdate.CourseId);
                cmd.Parameters.AddWithValue("@course_name", courseToUpdate.CourseName);
                cmd.Parameters.AddWithValue("@max_enrolled", courseToUpdate.MaxEnrolled);
                cmd.Parameters.AddWithValue("@price", courseToUpdate.Price);
                cmd.Parameters.AddWithValue("@start_date", courseToUpdate.StartDate);
                cmd.Parameters.AddWithValue("@end_date", courseToUpdate.EndDate);
                cmd.Parameters.AddWithValue("@teacher_id", courseToUpdate.TeacherId);
                cmd.Parameters.AddWithValue("@classroom_name", courseToUpdate.ClassroomName);

                cmd.ExecuteNonQuery();

                if (obj != null)
                {
                    obj.CourseName = courseToUpdate.CourseName;
                    obj.MaxEnrolled = courseToUpdate.MaxEnrolled;
                    obj.Price = courseToUpdate.Price;
                    obj.StartDate = courseToUpdate.StartDate;
                    obj.EndDate = courseToUpdate.EndDate;
                    obj.TeacherId = courseToUpdate.TeacherId;
                    obj.ClassroomName = courseToUpdate.ClassroomName;
                }
            }
            return obj;
        }

        public bool Delete(Course courseToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM course WHERE course_id = @id", con);

                cmd.Parameters.AddWithValue("@id", courseToDelete.CourseId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
