using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class AdminRepository
    {
        private readonly string _connectionString;
        private List<Admin> _admins = new();

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
            _admins = GetAll();
        }

        public List<Admin> GetAll()
        {
            _admins.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("email, hashed_salted_password, salt, admin_id FROM admin_credentials", con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Admin admin = new Admin()
                        {
                            AdminId = Convert.ToInt32(reader["admin_id"]),
                            Email = reader["email"].ToString()!,
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                            Salt = reader["salt"].ToString()!
                        };
                        _admins.Add(admin);
                    }
                }
            }
            return _admins;
        }

        public Admin Get(string email)
        {
            Admin? adminToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_admin_get";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Admin admin = new Admin()
                    {
                        AdminId = Convert.ToInt32(reader["admin_id"]),
                        Email = reader["email"].ToString()!,
                        HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                        Salt = reader["salt"].ToString()!
                    };
                    adminToReturn = admin;
                }
            }
            return adminToReturn;
        }

        public Admin Create(Admin adminToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO credentials (email, hashed_salted_password, salt)" +
                    "VALUES(@email, @hashed_salted_password, @salt);" +
                    "INSERT INTO admin (email)" +
                    "VALUES(@email)", con);

                cmd.Parameters.AddWithValue("@email", adminToCreate.Email);
                cmd.Parameters.AddWithValue("@hashed_salted_password", adminToCreate.HashedSaltedPassword);
                cmd.Parameters.AddWithValue("@salt", adminToCreate.Salt);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _admins.Add(adminToCreate);
                _admins[_admins.Count - 1].AdminId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _admins[_admins.Count - 1];
        }

        public Admin Update(Admin adminToUpdate)
        {
            Admin? obj = _admins.FirstOrDefault(x => x.Email == adminToUpdate.Email);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_admin_update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", adminToUpdate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", adminToUpdate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", adminToUpdate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQuery();
            }
            if (obj != null)
            {
                obj.HashedSaltedPassword = adminToUpdate.HashedSaltedPassword;
                obj.Salt = adminToUpdate.Salt;
            }
            return obj;
        }

        public bool Delete(Admin adminToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_admin_delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", adminToDelete.Email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
