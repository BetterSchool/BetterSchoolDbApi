using BetterAdminDbAPI.Model.@enum;
using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class GuardianRepository
    {
        private readonly string _connectionString;
        private List<Guardian> _guardians = new();

        public GuardianRepository(string connectionString)
        {
            _connectionString = connectionString;
            _guardians = GetAll();
        }

        public List<Guardian> GetAll()
        {
            _guardians.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT email, hashed_salted_password, salt, first_name, last_name, phone_no, city, road, postal_code, guardian_id FROM guardian_credentials", con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guardian guardian = new Guardian()
                        {
                            GuardianId = Convert.ToInt32(reader["guardian_id"]),
                            Email = reader["email"].ToString(),
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString(),
                            Salt = reader["salt"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            PhoneNo = reader["phone_no"].ToString(),
                            City = reader["city"].ToString(),
                            Road = reader["road"].ToString(),
                            PostalCode = reader["postal_code"].ToString(),
                        };
                        _guardians.Add(guardian);
                    }
                }
            }
            return _guardians;
        }

        public Guardian Get(string email)
        {
            Guardian guardianToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_guardian_get";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Guardian guardian = new Guardian()
                    {
                        GuardianId = Convert.ToInt32(reader["guardian_id"]),
                        Email = reader["email"].ToString(),
                        HashedSaltedPassword = reader["hashed_salted_password"].ToString(),
                        Salt = reader["salt"].ToString(),
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        PhoneNo = reader["phone_no"].ToString(),
                        City = reader["city"].ToString(),
                        Road = reader["road"].ToString()
                    };
                    guardianToReturn = guardian;
                }
            }
            return guardianToReturn;
        }

        public Guardian Create(Guardian guardianToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO credentials (email, hashed_salted_password, salt)" +
                    "VALUES(@email, @hashed_salted_password, @salt);" +
                    "INSERT INTO pupil (first_name, last_name, phone_no, city, road, postal_code, email)" +
                    "VALUES(@first_name, @last_name, @phone_no, @city, @road, @postal_code, @email)", con);

                cmd.Parameters.AddWithValue("@email", guardianToCreate.Email);
                cmd.Parameters.AddWithValue("@hashed_salted_password", guardianToCreate.HashedSaltedPassword);
                cmd.Parameters.AddWithValue("@salt", guardianToCreate.Salt);
                cmd.Parameters.AddWithValue("@first_name", guardianToCreate.FirstName);
                cmd.Parameters.AddWithValue("@last_name", guardianToCreate.LastName);
                cmd.Parameters.AddWithValue("@phone_no", guardianToCreate.PhoneNo);
                cmd.Parameters.AddWithValue("@city", guardianToCreate.City);
                cmd.Parameters.AddWithValue("@road", guardianToCreate.Road);
                cmd.Parameters.AddWithValue("@postal_code", guardianToCreate.PostalCode);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _guardians.Add(guardianToCreate);
                _guardians[_guardians.Count - 1].GuardianId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _guardians[_guardians.Count - 1];
        }

        public Guardian Update(Guardian guardianToUpdate)
        {
            var obj = _guardians.FirstOrDefault(x => x.Email ==  guardianToUpdate.Email);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_guardian_update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", guardianToUpdate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", guardianToUpdate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", guardianToUpdate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pfirst_name", guardianToUpdate.FirstName);
                cmd.Parameters["pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("plast_name", guardianToUpdate.LastName);
                cmd.Parameters["plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphone_no", guardianToUpdate.PhoneNo);
                cmd.Parameters["pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pcity", guardianToUpdate.City);
                cmd.Parameters["pcity"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("road", guardianToUpdate.Road);
                cmd.Parameters["road"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("ppostal_code", guardianToUpdate.PostalCode);
                cmd.Parameters["ppostal_code"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQuery();
            }
            if (obj != null)
            {
                obj.HashedSaltedPassword = guardianToUpdate.HashedSaltedPassword;
                obj.Salt = guardianToUpdate.Salt;
                obj.FirstName = guardianToUpdate.FirstName;
                obj.LastName = guardianToUpdate.LastName;
                obj.PhoneNo = guardianToUpdate.PhoneNo;
                obj.City = guardianToUpdate.City;
                obj.Road = guardianToUpdate.Road;
                obj.PostalCode = guardianToUpdate.PostalCode;
            }
            return obj;
        }

        public bool Delete(Guardian guardianToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_guardian_delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", guardianToDelete.Email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
