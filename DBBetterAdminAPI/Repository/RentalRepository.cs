using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class RentalRepository
    {
        private readonly string _connectionString;
        private List<Rental> _rentals = new();

        public RentalRepository(string connectionString)
        {
            _connectionString = connectionString;
            _rentals = GetAll();
        }

        public List<Rental> GetAll()
        {
            _rentals.Clear();

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT rental_id, start_date, end_date, instrument_id, pupil_id FROM rental", con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rental rental = new Rental()
                        {
                            RentalId = Convert.ToInt32(dr["rental_id"]),
                            StartDate = Convert.ToDateTime(dr["start_date"]),
                            EndDate = Convert.ToDateTime(dr["end_date"]),
                            InstrumentId = Convert.ToInt32(dr["instrument_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        _rentals.Add(rental);
                    }
                }
            }

            return _rentals;
        }

        public List<Rental> GetByInstrumentId(int instrumentId)
        {
            List<Rental> resultList = new List<Rental>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT rental_id, start_date, end_date, instrument_id, pupil_id FROM rental WHERE instrument_id = @instrument_id", con);
                cmd.Parameters.AddWithValue("@instrument_id", instrumentId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rental rental = new Rental()
                        {
                            RentalId = Convert.ToInt32(dr["rental_id"]),
                            StartDate = Convert.ToDateTime(dr["start_date"]),
                            EndDate = Convert.ToDateTime(dr["end_date"]),
                            InstrumentId = Convert.ToInt32(dr["instrument_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        resultList.Add(rental);
                    }
                }
            }
            return resultList;
        }

        public List<Rental> GetByPupilId(int pupilId)
        {
            List<Rental> resultList = new List<Rental>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT rental_id, start_date, end_date, instrument_id, pupil_id FROM rental WHERE pupil_id = @pupil_id", con);
                cmd.Parameters.AddWithValue("@pupil_id", pupilId);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rental rental = new Rental()
                        {
                            RentalId = Convert.ToInt32(dr["rental_id"]),
                            StartDate = Convert.ToDateTime(dr["start_date"]),
                            EndDate = Convert.ToDateTime(dr["end_date"]),
                            InstrumentId = Convert.ToInt32(dr["instrument_id"]),
                            PupilId = Convert.ToInt32(dr["pupil_id"])
                        };
                        resultList.Add(rental);
                    }
                }
            }
            return resultList;
        }

        public Rental Create(Rental rentalToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO rental (start_date, end_date, instrument_id, pupil_id)" +
                    "VALUES(@start_date, @end_date, @instrument_id, @pupil_id)", con);

                cmd.Parameters.AddWithValue("@start_date", rentalToCreate.StartDate);
                cmd.Parameters.AddWithValue("@end_date", rentalToCreate.EndDate);
                cmd.Parameters.AddWithValue("@instrument_id", rentalToCreate.InstrumentId);
                cmd.Parameters.AddWithValue("@pupil_id", rentalToCreate.PupilId);

                cmd.ExecuteNonQuery();

                _rentals.Add(rentalToCreate);

            }
            return _rentals[_rentals.Count - 1];
        }

        public Rental Update(Rental rentalToUpdate)
        {
            Rental? obj = _rentals.FirstOrDefault(x => x.RentalId == rentalToUpdate.RentalId);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE rental SET start_date = @start_date, end_date = @end_date, instrument_id = @instrument_id, pupil_id = @pupil_id WHERE rental_id = @id", con);

                cmd.Parameters.AddWithValue("@id", rentalToUpdate.RentalId);
                cmd.Parameters.AddWithValue("@start_date", rentalToUpdate.StartDate);
                cmd.Parameters.AddWithValue("@end_date", rentalToUpdate.EndDate);
                cmd.Parameters.AddWithValue("@instrument_id", rentalToUpdate.InstrumentId);
                cmd.Parameters.AddWithValue("@pupil_id", rentalToUpdate.PupilId);

                cmd.ExecuteNonQuery();

                if (obj != null)
                {
                    obj.StartDate = rentalToUpdate.StartDate;
                    obj.EndDate = rentalToUpdate.EndDate;
                    obj.InstrumentId = rentalToUpdate.InstrumentId;
                    obj.PupilId = rentalToUpdate.PupilId;
                }
            }
            return obj;
        }

        public bool Delete(Rental rentalToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM rental WHERE rental_id = @rental_id", con);

                cmd.Parameters.AddWithValue("@rental_id", rentalToDelete.RentalId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
