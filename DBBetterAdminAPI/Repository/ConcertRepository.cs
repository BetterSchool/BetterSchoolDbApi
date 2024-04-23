using BetterAdminDbAPI.Model;
using MySqlConnector;

namespace BetterAdminDbAPI.Repository
{
    public class ConcertRepository
    {
        private readonly MySqlConnection _con;
        private List<Concert> _concerts = new();

        public ConcertRepository(MySqlConnection connection)
        {
            _con = connection;
            _concerts = GetAll();
        }

        public List<Concert> GetAll()
        {
            _concerts = new List<Concert>();
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
                        _concerts.Add(concert);
                    }
                }
            }
            return _concerts;
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

        public Concert Create(Concert concertToCreate)
        {
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO concert (concert_name, start_time, end_time, concert_location)" +
                    "VALUES(@concert_name, @start_time, @end_time, @concert_location)", _con);

                cmd.Parameters.AddWithValue("@concert_name", concertToCreate.ConcertName);
                cmd.Parameters.AddWithValue("@start_time", concertToCreate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", concertToCreate.EndTime);
                cmd.Parameters.AddWithValue("@concert_location", concertToCreate.ConcertLocation);

                cmd.ExecuteNonQuery();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _concerts.Add(concertToCreate);
                _concerts[_concerts.Count - 1].ConcertId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

            }
            return _concerts[_concerts.Count - 1];
        }

        public Concert Update(Concert concertToUpdate)
        {
            var obj = _concerts.FirstOrDefault(x => x.ConcertId == concertToUpdate.ConcertId);
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE concert SET concert_name = @concert_name, start_time = @start_time, end_time = @end_time, concert_location = @concert_location WHERE concert_id = @id", _con);

                cmd.Parameters.AddWithValue("@id", concertToUpdate.ConcertId);
                cmd.Parameters.AddWithValue("@concert_name", concertToUpdate.ConcertName);
                cmd.Parameters.AddWithValue("@start_time", concertToUpdate.StartTime);
                cmd.Parameters.AddWithValue("@end_time", concertToUpdate.EndTime);
                cmd.Parameters.AddWithValue("@concert_location", concertToUpdate.ConcertLocation);

                cmd.ExecuteNonQuery();

                if (obj != null)
                {
                    obj.ConcertName = concertToUpdate.ConcertName;
                    obj.ConcertLocation = concertToUpdate.ConcertLocation;
                    obj.EndTime = concertToUpdate.EndTime;
                    obj.StartTime = concertToUpdate.StartTime;
                }
            }
            return obj;
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
    }
}
