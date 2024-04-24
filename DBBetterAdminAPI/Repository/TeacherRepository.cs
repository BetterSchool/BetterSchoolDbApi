using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class TeacherRepository
    {
        private readonly string _connectionString;
        private List<Teacher> _teachers = new();

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
            _teachers = GetAll();
        }

        public List<Teacher> GetAll()
        {
            _teachers.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("email, hashed_salted_password, salt, first_name, last_name, phone_no, work_hours, teacher_id FROM teacher_credentials", con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher teacher = new Teacher()
                        {
                            TeacherId = Convert.ToInt32(reader["teacher_id"]),
                            Email = reader["email"].ToString()!,
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                            Salt = reader["salt"].ToString()!,
                            FirstName = reader["first_name"].ToString()!,
                            LastName = reader["last_name"].ToString()!,
                            PhoneNo = reader["phone_no"].ToString()!,
                            WorkHours = Convert.ToDouble(reader["work_hours"])
                        };
                        _teachers.Add(teacher);
                    }
                }
            }
            return _teachers;
        }

        public Teacher Get(string email)
        {
            Teacher? teacherToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_teacher_get";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Teacher teacher = new Teacher()
                    {
                        TeacherId = Convert.ToInt32(reader["teacher_id"]),
                        Email = reader["email"].ToString()!,
                        HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                        Salt = reader["salt"].ToString()!,
                        FirstName = reader["first_name"].ToString()!,
                        LastName = reader["last_name"].ToString()!,
                        PhoneNo = reader["phone_no"].ToString()!,
                        WorkHours = Convert.ToDouble(reader["work_hours"])
                    };
                    teacherToReturn = teacher;
                }
            }
            return teacherToReturn;
        }

        public Teacher Create(Teacher teacherToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO credentials (email, hashed_salted_password, salt)" +
                    "VALUES(@email, @hashed_salted_password, @salt);" +
                    "INSERT INTO teacher (email, first_name, last_name, phone_no, work_hours)" +
                    "VALUES(@email, @first_name, @last_name, @phone_no, @work_hours)", con);

                cmd.Parameters.AddWithValue("@email", teacherToCreate.Email);
                cmd.Parameters.AddWithValue("@hashed_salted_password", teacherToCreate.HashedSaltedPassword);
                cmd.Parameters.AddWithValue("@salt", teacherToCreate.Salt);
                cmd.Parameters.AddWithValue("@first_name", teacherToCreate.FirstName);
                cmd.Parameters.AddWithValue("@last_name", teacherToCreate.LastName);
                cmd.Parameters.AddWithValue("@phone_no", teacherToCreate.PhoneNo);
                cmd.Parameters.AddWithValue("@work_hours", teacherToCreate.WorkHours);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _teachers.Add(teacherToCreate);
                _teachers[_teachers.Count - 1].TeacherId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _teachers[_teachers.Count - 1];
        }

        public Teacher Update(Teacher teacherToUpdate)
        {
            Teacher? obj = _teachers.FirstOrDefault(x => x.Email == teacherToUpdate.Email);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_teacher_update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", teacherToUpdate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", teacherToUpdate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", teacherToUpdate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pfirst_name", teacherToUpdate.FirstName);
                cmd.Parameters["pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("plast_name", teacherToUpdate.LastName);
                cmd.Parameters["plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphone_no", teacherToUpdate.PhoneNo);
                cmd.Parameters["pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pwork_hours", teacherToUpdate.WorkHours);
                cmd.Parameters["pcity"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQuery();
            }
            if (obj != null)
            {
                obj.HashedSaltedPassword = teacherToUpdate.HashedSaltedPassword;
                obj.Salt = teacherToUpdate.Salt;
                obj.FirstName = teacherToUpdate.FirstName;
                obj.LastName = teacherToUpdate.LastName;
                obj.PhoneNo = teacherToUpdate.PhoneNo;
                obj.WorkHours = teacherToUpdate.WorkHours;
            }
            return obj;
        }

        public bool Delete(Teacher teacherToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_teacher_delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", teacherToDelete.Email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
