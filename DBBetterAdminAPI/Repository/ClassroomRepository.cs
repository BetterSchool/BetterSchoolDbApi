namespace BetterAdminDbAPI.Repository
{
    using global::BetterAdminDbAPI.Model;
    using MySqlConnector;

    namespace BetterAdminDbAPI.Repository
    {
        public class ClassroomRepository
        {
            private readonly string _connectionString;
            private List<Classroom> _classrooms = new();

            public ClassroomRepository(string connectionString)
            {
                _connectionString = connectionString;
                _classrooms = GetAll();
            }

            public List<Classroom> GetAll()
            {
                _classrooms = new List<Classroom>();
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT classroom_name FROM classroom", con);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Classroom classroom = new Classroom()
                            {
                                ClassroomName = dr["classroom_name"].ToString()!

                            };
                            _classrooms.Add(classroom);
                        }
                    }
                }
                return _classrooms;
            }

            public Classroom Get(int id)
            {
                Classroom? classroomToReturn = null;
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT classroom_name FROM classroom WHERE classroom_name = @classroom_name", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        Classroom classroom = new Classroom()
                        {
                            ClassroomName = dr["classroom_name"].ToString()!,

                        };
                        classroomToReturn = classroom;
                    }
                }
                return classroomToReturn;
            }

            public Classroom Create(Classroom classroomToCreate)
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO classroom (classroom_name)" +
                        "VALUES(@classroom_name)", con);

                    cmd.Parameters.AddWithValue("@classroom_name", classroomToCreate.ClassroomName);

                    cmd.ExecuteNonQuery();

                    _classrooms.Add(classroomToCreate);

                }
                return _classrooms[_classrooms.Count - 1];
            }

            public bool Delete(Classroom classroomToDelete)
            {
                int rowsAffected = 0;
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM classroom WHERE classroom_name = @classrom_name", con);

                    cmd.Parameters.AddWithValue("@classroom_name", classroomToDelete.ClassroomName);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
                return rowsAffected != 0;
            }
        }
    }
}
