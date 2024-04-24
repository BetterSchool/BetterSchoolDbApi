namespace BetterAdminDbAPI.Repository
{
    using global::BetterAdminDbAPI.Model;
    using global::BetterAdminDbAPI.Model.@enum;
    using MySqlConnector;

    namespace BetterAdminDbAPI.Repository
    {
        public class ConcertRepository
        {
            private readonly string _connectionString;
            private List<Instrument> _instruments = new();

            public ConcertRepository(string connectionString)
            {
                _connectionString = connectionString;
                _instruments = GetAll();
            }

            public List<Instrument> GetAll()
            {
                _instruments = new List<Instrument>();
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT instrument_id, price, type FROM instrument", con);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            InstrumentType type;
                            Enum.TryParse<InstrumentType>(dr["type"].ToString(), out type);
                            Instrument instrument = new Instrument()
                            {
                                InstrumentId = Convert.ToInt32(dr["instrument_id"]),
                                Price = Convert.ToDecimal(dr["price"]),
                                Type = type

                            };
                            _instruments.Add(instrument);
                        }
                    }
                }
                return _instruments;
            }

            public Instrument Get(int id)
            {
                Instrument? instrumentToReturn = null;
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT instrument_id, price, type FROM instrument WHERE instrument_id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        InstrumentType type;
                        Enum.TryParse<InstrumentType>(dr["type"].ToString(), out type);
                        Instrument instrument = new Instrument()
                        {
                            InstrumentId = Convert.ToInt32(dr["instrument_id"]),
                            Price = Convert.ToDecimal(dr["price"]),
                            Type = type

                        };
                        instrumentToReturn = instrument;
                    }
                }
                return instrumentToReturn;
            }

            public Instrument Create(Instrument instrumentToCreate)
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO concert (price, type)" +
                        "VALUES(@price, @type)", con);

                    cmd.Parameters.AddWithValue("@price", instrumentToCreate.Price);
                    cmd.Parameters.AddWithValue("@type", instrumentToCreate.Type.ToString());

                    cmd.ExecuteNonQuery();

                    if (cmd.LastInsertedId != null)
                        cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                    _instruments.Add(instrumentToCreate);
                    _instruments[_instruments.Count - 1].InstrumentId = Convert.ToInt32(cmd.Parameters["@newId"].Value);

                }
                return _instruments[_instruments.Count - 1];
            }

            public Instrument Update(Instrument instrumentToUpdate)
            {
                Instrument? obj = _instruments.FirstOrDefault(x => x.InstrumentId == instrumentToUpdate.InstrumentId);
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE concert SET price = @price, type = @type WHERE instrument_id = @id", con);

                    cmd.Parameters.AddWithValue("@id", instrumentToUpdate.InstrumentId);
                    cmd.Parameters.AddWithValue("@price", instrumentToUpdate.Price);
                    cmd.Parameters.AddWithValue("@type", instrumentToUpdate.Type.ToString());

                    cmd.ExecuteNonQuery();

                    if (obj != null)
                    {
                        obj.Price = instrumentToUpdate.Price;
                        obj.Type = instrumentToUpdate.Type;
                    }
                }
                return obj;
            }

            public bool Delete(Instrument instrumentToDelete)
            {
                int rowsAffected = 0;
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM instrument WHERE instrument_id = @id", con);

                    cmd.Parameters.AddWithValue("@id", instrumentToDelete.InstrumentId);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
                return rowsAffected != 0;
            }
        }
    }
}
