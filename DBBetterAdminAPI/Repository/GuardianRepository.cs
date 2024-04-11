using BetterAdminDbAPI.Model.@enum;
using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class GuardianRepository
    {
        private readonly MySqlConnection _con;

        public GuardianRepository(MySqlConnection connection)
        {
            _con = connection;
        }

        public List<Guardian> GetAll()
        {
            List<Guardian> guardiansToReturn = new List<Guardian>();
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_guardian_getall";
                cmd.CommandType = CommandType.StoredProcedure;

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
                            Road = reader["road"].ToString()
                        };
                        guardiansToReturn.Add(guardian);
                    }
                }
            }
            return guardiansToReturn;
        }

        public Guardian Get(string email)
        {
            Guardian guardianToReturn = null;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

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
            Guardian guardianToReturn = new Guardian();
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_guardian_insert";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", guardianToCreate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", guardianToCreate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", guardianToCreate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pfirst_name", guardianToCreate.FirstName);
                cmd.Parameters["pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("plast_name", guardianToCreate.LastName);
                cmd.Parameters["plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphone_no", guardianToCreate.PhoneNo);
                cmd.Parameters["pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pcity", guardianToCreate.City);
                cmd.Parameters["pcity"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("road", guardianToCreate.Road);
                cmd.Parameters["road"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("ppostal_code", guardianToCreate.PostalCode);
                cmd.Parameters["ppostal_code"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQuery();

                guardianToReturn = Get(guardianToCreate.Email);
            }

            return guardianToReturn;
        }

        public bool Update(Guardian guardianToUpdate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

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

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Delete(Guardian guardianToDelete)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

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
