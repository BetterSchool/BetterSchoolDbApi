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
        private readonly string _connectionString;
        private List<Pupil> _pupils = new();

        public PupilRepository(string connectionString)
        {
            _connectionString = connectionString;
            _pupils = GetAll();
        }

        public List<Pupil> GetAll()
        {
            _pupils.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT email, hashed_salted_password, salt, first_name, last_name, phone_no, gender, enrollment_date, note, photo_permission, school, grade, city, road, postal_code, pupil_id, guardian_email FROM pupil_credentials", con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GenderEnum gender;
                        Enum.TryParse<GenderEnum>(reader["gender"].ToString(), out gender);

                        Pupil pupil = new Pupil()
                        {
                            PupilId = Convert.ToInt32(reader["pupil_id"]),
                            Email = reader["email"].ToString()!,
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                            Salt = reader["salt"].ToString()!,
                            FirstName = reader["first_name"].ToString()!,
                            LastName = reader["last_name"].ToString()!,
                            PhoneNo = reader["phone_no"].ToString()!,
                            Gender = gender,
                            EnrollmentDate = DateTime.Parse(reader["enrollment_date"].ToString()!),
                            Note = reader["guardian_email"].ToString() == "" ? null : reader["guardian_email"].ToString(),
                            PhotoPermission = bool.Parse(reader["photo_permission"].ToString()!),
                            School = reader["school"].ToString()!,
                            Grade = Convert.ToInt32(reader["grade"]),
                            City = reader["city"].ToString()!,
                            Road = reader["road"].ToString()!,
                            PostalCode = reader["postal_code"].ToString()!,
                            GuardianEmail = reader["guardian_email"].ToString() == "" ? null : reader["guardian_email"].ToString()
                        };
                        _pupils.Add(pupil);
                    }
                }
            }
            return _pupils;
        }

        public Pupil? Get(string email)
        {
            Pupil? pupilToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

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
                            HashedSaltedPassword = reader["hashed_salted_password"].ToString()!,
                            Salt = reader["salt"].ToString()!,
                            FirstName = reader["first_name"].ToString()!,
                            LastName = reader["last_name"].ToString()!,
                            PhoneNo = reader["phone_no"].ToString()!,
                            Gender = gender,
                            EnrollmentDate = DateTime.Parse(reader["enrollment_date"].ToString()!),
                            Note = reader["guardian_email"].ToString() == "" ? null : reader["guardian_email"].ToString(),
                            PhotoPermission = bool.Parse(reader["photo_permission"].ToString()!),
                            School = reader["school"].ToString()!,
                            Grade = Convert.ToInt32(reader["grade"]),
                            City = reader["city"].ToString()!,
                            Road = reader["road"].ToString()!,
                            PostalCode = reader["postal_code"].ToString()!,
                            GuardianEmail = reader["guardian_email"].ToString() == "" ? null : reader["guardian_email"].ToString()
                        };
                        pupilToReturn = pupil;
                    }
                }
            }
            return pupilToReturn;
        }

        public Pupil Create(Pupil pupilToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO credentials (email, hashed_salted_password, salt)" +
                    "VALUES(@email, @hashed_salted_password, @salt);" +
                    "INSERT INTO pupil (first_name, last_name, phone_no, gender, enrollment_date, note, photo_permission, school, grade, city, road, postal_code, email)" +
                    "VALUES(@first_name, @last_name, @phone_no, @gender, @enrollment_date, @note, @photo_permission, @school, @grade, @city, @road, @postal_code, @email)", con);

                cmd.Parameters.AddWithValue("@email", pupilToCreate.Email);
                cmd.Parameters.AddWithValue("@hashed_salted_password", pupilToCreate.HashedSaltedPassword);
                cmd.Parameters.AddWithValue("@salt", pupilToCreate.Salt);
                cmd.Parameters.AddWithValue("@first_name", pupilToCreate.FirstName);
                cmd.Parameters.AddWithValue("@last_name", pupilToCreate.LastName);
                cmd.Parameters.AddWithValue("@phone_no", pupilToCreate.PhoneNo);
                cmd.Parameters.AddWithValue("@gender", pupilToCreate.Gender.ToString());
                cmd.Parameters.AddWithValue("@enrollment_date", pupilToCreate.EnrollmentDate);
                cmd.Parameters.AddWithValue("@note", pupilToCreate.Note);
                cmd.Parameters.AddWithValue("@photo_permission", pupilToCreate.PhotoPermission);
                cmd.Parameters.AddWithValue("@school", pupilToCreate.School);
                cmd.Parameters.AddWithValue("@grade", pupilToCreate.Grade);
                cmd.Parameters.AddWithValue("@city", pupilToCreate.City);
                cmd.Parameters.AddWithValue("@road", pupilToCreate.Road);
                cmd.Parameters.AddWithValue("@postal_code", pupilToCreate.PostalCode);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _pupils.Add(pupilToCreate);
                _pupils[_pupils.Count - 1].PupilId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _pupils[_pupils.Count - 1];
        }

        public Pupil? Update(Pupil pupilToUpdate)
        {
            Pupil? obj = _pupils.FirstOrDefault(x => x.Email == pupilToUpdate.Email);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

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

                cmd.Parameters.AddWithValue("pgender", pupilToUpdate.Gender.ToString());
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

                cmd.Parameters.AddWithValue("pguardian_email", pupilToUpdate.GuardianEmail);
                cmd.Parameters["pguardian_email"].Direction = ParameterDirection.Input;

                cmd.ExecuteNonQuery();
            }
            if (obj != null)
            {
                obj.HashedSaltedPassword = pupilToUpdate.HashedSaltedPassword;
                obj.Salt = pupilToUpdate.Salt;
                obj.FirstName = pupilToUpdate.FirstName;
                obj.LastName = pupilToUpdate.LastName;
                obj.PhoneNo = pupilToUpdate.PhoneNo;
                obj.Gender = pupilToUpdate.Gender;
                obj.EnrollmentDate = pupilToUpdate.EnrollmentDate;
                obj.Note = pupilToUpdate.Note;
                obj.PhotoPermission = pupilToUpdate.PhotoPermission;
                obj.School = pupilToUpdate.School;
                obj.Grade = pupilToUpdate.Grade;
                obj.City = pupilToUpdate.City;
                obj.Road = pupilToUpdate.Road;
                obj.PostalCode = pupilToUpdate.PostalCode;
                obj.GuardianEmail = pupilToUpdate.GuardianEmail;
            }
            return obj;
        }

        public bool Delete(Pupil pupilToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "usp_pupil_delete";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", pupilToDelete.Email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
