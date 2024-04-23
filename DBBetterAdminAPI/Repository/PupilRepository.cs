using BetterAdminDbAPI.Model;
using BetterAdminDbAPI.Model.@enum;
using MySqlConnector;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace BetterAdminDbAPI.Repository
{
    public class PupilRepository
    {
        private readonly MySqlConnection _con;
        private readonly GuardianRepository guardianRepo;

        public PupilRepository(MySqlConnection connection)
        {
            _con = connection;
            //_con = new MySqlConnection("server=104.199.62.75;uid=bauser;pwd=blowfish21seahorse;database=better_admin");
            guardianRepo = new GuardianRepository(connection);
        }

        public List<Pupil> GetAll()
        {
            List<Pupil> pupilsToReturn = new List<Pupil>();
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_pupil_getall";
                cmd.CommandType = CommandType.StoredProcedure;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GenderEnum gender;
                        Enum.TryParse<GenderEnum>(reader["gender"].ToString(), out gender);

                        Pupil pupil = new Pupil()
                        {
                            PupilId = Convert.ToInt32(reader["pupil_id"]),
                            Email = reader["email"].ToString(),
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString(),
                            Salt = reader["salt"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            PhoneNo = reader["phone_no"].ToString(),
                            Gender = gender,
                            EnrollmentDate = DateTime.Parse(reader["enrollment_date"].ToString()),
                            Note = reader["note"].ToString(),
                            PhotoPermission = bool.Parse(reader["photo_permission"].ToString()),
                            School = reader["school"].ToString(),
                            Grade = Convert.ToInt32(reader["grade"]),
                            City = reader["city"].ToString(),
                            Road = reader["road"].ToString(),
                            PostalCode = reader["postal_code"].ToString()
                        };
                        // Add guardian if GuardianEmail != null
                        string? guardianEmail = Convert.ToString(reader["guardian_email"]);
                        if (guardianEmail != "")
                        {
                            pupil.Guardian = guardianRepo.Get(guardianEmail);
                        }
                        pupilsToReturn.Add(pupil);
                    }
                }
            }
            return pupilsToReturn;
        }

        public Pupil Get(string email)
        {
            Pupil pupilToReturn = null;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_pupil_get";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GenderEnum gender;
                        Enum.TryParse<GenderEnum>(reader["gender"].ToString(), out gender);
                        Pupil pupil = new Pupil()
                        {
                            PupilId = Convert.ToInt32(reader["pupil_id"]),
                            Email = email,
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString(),
                            Salt = reader["salt"].ToString(),
                            FirstName = reader["first_name"].ToString(),
                            LastName = reader["last_name"].ToString(),
                            PhoneNo = reader["phone_no"].ToString(),
                            Gender = gender,
                            EnrollmentDate = DateTime.Parse(reader["enrollment_date"].ToString()),
                            Note = reader["note"].ToString(),
                            PhotoPermission = bool.Parse(reader["photo_permission"].ToString()),
                            School = reader["school"].ToString(),
                            Grade = Convert.ToInt32(reader["grade"]),
                            City = reader["city"].ToString(),
                            Road = reader["road"].ToString(),
                            PostalCode = reader["postal_code"].ToString()
                        };
                        // Add guardian if GuardianEmail != null
                        string? guardianEmail = Convert.ToString(reader["guardian_email"]);
                        if (guardianEmail != "")
                        {
                            pupil.Guardian = guardianRepo.Get(guardianEmail);
                        }
                        pupilToReturn = pupil;
                    }
                }
            }
            return pupilToReturn;
        }

        public Pupil Create(Pupil pupilToCreate)
        {
            Pupil pupilToReturn = new Pupil();
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_guardian_insert";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", pupilToCreate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", pupilToCreate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", pupilToCreate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pfirst_name", pupilToCreate.FirstName);
                cmd.Parameters["pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("plast_name", pupilToCreate.LastName);
                cmd.Parameters["plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphone_no", pupilToCreate.PhoneNo);
                cmd.Parameters["pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pgender", pupilToCreate.Gender);
                cmd.Parameters["pgender"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("penrollment_date", pupilToCreate.EnrollmentDate);
                cmd.Parameters["penrollment_date"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pnote", pupilToCreate.Note);
                cmd.Parameters["pnote"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphoto_permission", pupilToCreate.PhotoPermission);
                cmd.Parameters["pphoto_permission"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pschool", pupilToCreate.School);
                cmd.Parameters["pschool"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pgrade", pupilToCreate.Grade);
                cmd.Parameters["pgrade"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pcity", pupilToCreate.City);
                cmd.Parameters["pcity"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("proad", pupilToCreate.Road);
                cmd.Parameters["proad"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("ppostal_code", pupilToCreate.PostalCode);
                cmd.Parameters["ppostal_code"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pguardian_email", pupilToCreate.Guardian?.Email);
                cmd.Parameters["pguardian_email"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQueryAsync();
            }
            //pupilToReturn = Get(pupilToCreate.Email);
            return pupilToReturn;
        }

        public bool Update(Pupil pupilToUpdate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_pupil_update";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("pemail", pupilToUpdate.Email);
                cmd.Parameters["pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("phashed_salted_password", pupilToUpdate.HashedSaltedPassword);
                cmd.Parameters["phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("psalt", pupilToUpdate.Salt);
                cmd.Parameters["psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pfirst_name", pupilToUpdate.FirstName);
                cmd.Parameters["pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("plast_name", pupilToUpdate.LastName);
                cmd.Parameters["plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphone_no", pupilToUpdate.PhoneNo);
                cmd.Parameters["pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pgender", pupilToUpdate.Gender);
                cmd.Parameters["pgender"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("penrollment_date", pupilToUpdate.EnrollmentDate);
                cmd.Parameters["penrollment_date"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pnote", pupilToUpdate.Note);
                cmd.Parameters["pnote"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pphoto_permission", pupilToUpdate.PhotoPermission);
                cmd.Parameters["pphoto_permission"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pschool", pupilToUpdate.School);
                cmd.Parameters["pschool"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pgrade", pupilToUpdate.Grade);
                cmd.Parameters["pgrade"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pcity", pupilToUpdate.City);
                cmd.Parameters["pcity"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("road", pupilToUpdate.Road);
                cmd.Parameters["road"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("ppostal_code", pupilToUpdate.PostalCode);
                cmd.Parameters["ppostal_code"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("pguardian_email", pupilToUpdate.Guardian?.Email);
                cmd.Parameters["pguardian_email"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Delete(string email)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_pupil_delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
