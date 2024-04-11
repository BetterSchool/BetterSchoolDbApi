using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class MessageRepository
    {
        private readonly MySqlConnection _con;

        public MessageRepository(MySqlConnection connection)
        {
            _con = connection;
        }

        public List<Message> GetAll()
        {
            List<Message> messages = new List<Message>();
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message", _con);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Message message = new Message()
                        {
                            MessageId = Convert.ToInt32(dr["message_id"]),
                            Content = dr["content"].ToString(),
                            Attachment = (byte[])dr["attachment"],
                            TimeSent = Convert.ToDateTime(dr["time_sent"]),
                            SenderEmail = dr["sender_email"].ToString(),
                            ReceiverEmail = dr["receiver_email"].ToString()

                        };
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }

        public Message Get(int id)
        {
            Message messageToReturn = null;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message WHERE message_id = @id", _con);
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    Message message = new Message()
                    {
                        MessageId = Convert.ToInt32(dr["message_id"]),
                        Content = dr["content"].ToString(),
                        Attachment = (byte[])dr["attachment"],
                        TimeSent = Convert.ToDateTime(dr["time_sent"]),
                        SenderEmail = dr["sender_email"].ToString(),
                        ReceiverEmail = dr["receiver_email"].ToString()

                    };
                    messageToReturn = message;
                }
            }
            return messageToReturn;
        }

        public int Create(Message messageToCreate)
        {
            int messageId;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO message (content, attachment, time_sent, sender_email, receiver_email)" +
                    "VALUES(@content, @attachment, @time_sent, @sender_email, @receiver_email)", _con);

                cmd.Parameters.AddWithValue("@content", messageToCreate.Content);
                cmd.Parameters.AddWithValue("@attachment", messageToCreate.Attachment);
                cmd.Parameters.AddWithValue("@time_sent", messageToCreate.TimeSent);
                cmd.Parameters.AddWithValue("@sender_email", messageToCreate.SenderEmail);
                cmd.Parameters.AddWithValue("@receiver_email", messageToCreate.ReceiverEmail);

                messageId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return messageId;
        }

        public bool Update(Message messageToUpdate)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE message SET content = @content, attachment = @attachment, time_sent = @time_sent, sender_email = sender_email, receiver_email = @receiver_email WHERE message_id = @id", _con);

                cmd.Parameters.AddWithValue("@id", messageToUpdate.MessageId);
                cmd.Parameters.AddWithValue("@content", messageToUpdate.Content);
                cmd.Parameters.AddWithValue("@attachment", messageToUpdate.Attachment);
                cmd.Parameters.AddWithValue("@time_sent", messageToUpdate.TimeSent);
                cmd.Parameters.AddWithValue("@sender_email", messageToUpdate.SenderEmail);
                cmd.Parameters.AddWithValue("@receiver_email", messageToUpdate.ReceiverEmail);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }

        public bool Delete(Message messageToDelete)
        {
            int rowsAffected = 0;
            using (_con)
            {
                _con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM message WHERE message_id = @id", _con);

                cmd.Parameters.AddWithValue("@id", messageToDelete.MessageId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
