using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class ConcertRepository
    {
        private readonly MySqlConnection _con;

        public ConcertRepository(MySqlConnection connection)
        {
            _con = connection;
        }

        public List<Concert> GetAll()
        {
            List<Concert> concerts = new List<Concert>();
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT concert_id, concert_name, start_time, end_time, concert_location FROM concert", _con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Concert concert = new Concert()
                        {
                            ConcertId = Convert.ToInt32(dr["concert_id"]),
                            ConcertName = dr["concert_name"].ToString(),
                            StartTime = Convert.ToDateTime(dr["start_time"]),
                            EndTime = Convert.ToDateTime(dr["end_time"]),
                            ConcertLocation = dr["concert_location"].ToString()

                        };
                        concerts.Add(concert);
                    }
                }
            }
            return concerts;
        }

        public Concert Get(int id)
        {
            Concert concertToReturn = null;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT concert_id, concert_name, start_time, end_time, concert_location FROM concert WHERE concert_id = @id", _con);
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    Concert concert = new Concert()
                    {
                        ConcertId = Convert.ToInt32(dr["concert_id"]),
                        ConcertName = dr["concert_name"].ToString(),
                        StartTime = Convert.ToDateTime(dr["start_time"]),
                        EndTime = Convert.ToDateTime(dr["end_time"]),
                        ConcertLocation = dr["concert_location"].ToString()

                    };
                    concertToReturn = concert;
                }
            }
            return concertToReturn;
        }

        public int Create(Concert concertToCreate)
        {
            int concertId;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO concert (concert_name, start_time, end_time, concert_location)" +
                    "VALUES(@concert_name, @start_time, @end_time, @concert_location)", _con);

                cmd.Parameters.AddWithValue("@concert_name", concertToCreate.ConcertName);
                cmd.Parameters.AddWithValue("@start_time", concertToCreate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", concertToCreate.EndTime);
                cmd.Parameters.AddWithValue("@concert_location", concertToCreate.ConcertLocation);

                concertId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return concertId;
        }

        public bool Update(Concert concertToUpdate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE concert SET concert_name = @concert_name, start_time = @start_time, end_time = @end_time, concert_location = @concert_location WHERE concert_id = @id", _con);

                cmd.Parameters.AddWithValue("@id", concertToUpdate.ConcertId);
                cmd.Parameters.AddWithValue("@concert_name", concertToUpdate.ConcertName);
                cmd.Parameters.AddWithValue("@start_time", concertToUpdate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", concertToUpdate.EndTime);
                cmd.Parameters.AddWithValue("@concert_location", concertToUpdate.ConcertLocation);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Delete(Concert concertToDelete)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM concert WHERE concert_id = @id", _con);

                cmd.Parameters.AddWithValue("@id", concertToDelete.ConcertId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public static implicit operator ConcertRepository(WaitListRepository v)
        {
            throw new NotImplementedException();
        }
    }
}
