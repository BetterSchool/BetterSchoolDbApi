using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;
using System.Security.Cryptography;

namespace BetterAdminDbAPI.Repository
{
    public class PupilRepository
    {
        private List<Pupil> _pupils = new List<Pupil>();
        private readonly MySqlConnection _con;

        public PupilRepository(MySqlConnection connection)
        {
            _con = connection;
        }

        public Pupil GetPupil(string email)
        {
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
                    Pupil pupil = new Pupil()
                    {
                        Email = email,
                        HashedSaltedPassword,

                    }
                }

            }
        }

        public bool Create(Pupil pupilToCreate)
        {
            int rowsAffected = -1;
            using (_con)
            {
                _con.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _con;

                cmd.CommandText = "usp_pupil_insert";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pemail", pupilToCreate.Email);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@phashed_salted_password", pupilToCreate.HashedSaltedPassword);
                cmd.Parameters["@phashed_salted_password"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@psalt", pupilToCreate.Salt);
                cmd.Parameters["@psalt"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pfirst_name", pupilToCreate.FirstName);
                cmd.Parameters["@pfirst_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@plast_name", pupilToCreate.LastName);
                cmd.Parameters["@plast_name"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pphone_no", pupilToCreate.PhoneNo);
                cmd.Parameters["@pphone_no"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pgender", pupilToCreate.Gender);
                cmd.Parameters["@pgender"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@penrollment_date", pupilToCreate.EnrollmentDate);
                cmd.Parameters["@penrollment_date"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pnote", pupilToCreate.Note);
                cmd.Parameters["@pnote"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pphoto_permission", pupilToCreate.PhotoPermission);
                cmd.Parameters["@pphoto_permission"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pschool", pupilToCreate.School);
                cmd.Parameters["@pschool"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pgrade", pupilToCreate.Grade);
                cmd.Parameters["@pgrade"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pcity", pupilToCreate.City);
                cmd.Parameters["@pcity"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@road", pupilToCreate.Road);
                cmd.Parameters["@road"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@ppostal_code", pupilToCreate.PostalCode);
                cmd.Parameters["@ppostal_code"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@pguardian_email", pupilToCreate.GuardianEmail);
                cmd.Parameters["@pguardian_email"].Direction = ParameterDirection.Input;

                rowsAffected = cmd.ExecuteNonQuery();
            }

            return rowsAffected == 0;
        }
    }
}
