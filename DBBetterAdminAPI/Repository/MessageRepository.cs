using BetterAdminDbAPI.Model;
using MySqlConnector;
using System.Data;

namespace BetterAdminDbAPI.Repository
{
    public class MessageRepository
    {
        private readonly string _connectionString;
        private List<Message> _messages = new();

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
            _messages = GetAll();
        }

        public List<Message> GetAll()
        {
            _messages.Clear();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message", con);
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
                        _messages.Add(message);
                    }
                }
            }
            return _messages;
        }

        public Message GetById(int id)
        {
            Message messageToReturn = null;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message WHERE message_id = @id", con);
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

        public List<Message> GetBySenderEmail(string senderEmail)
        {
            List<Message> returnList = new();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message WHERE sender_email = @sender_email", con);
                cmd.Parameters.AddWithValue("@sender_email", senderEmail);
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
                        returnList.Add(message);
                    }
                }
            }
            return returnList;
        }

        public List<Message> GetByReceiverEmail(string receiverEmail)
        {
            List<Message> returnList = new();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT message_id, content, attachment, time_sent, sender_email, receiver_email FROM message WHERE receiver_email = @receiver_email", con);
                cmd.Parameters.AddWithValue("@receiver_email", receiverEmail);
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
                        returnList.Add(message);
                    }
                }
            }
            return returnList;
        }

        public Message Create(Message messageToCreate)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO message (content, attachment, time_sent, sender_email, receiver_email)" +
                    "VALUES(@content, @attachment, @time_sent, @sender_email, @receiver_email)", con);

                cmd.Parameters.AddWithValue("@content", messageToCreate.Content);
                cmd.Parameters.AddWithValue("@attachment", messageToCreate.Attachment);
                cmd.Parameters.AddWithValue("@time_sent", messageToCreate.TimeSent);
                cmd.Parameters.AddWithValue("@sender_email", messageToCreate.SenderEmail);
                cmd.Parameters.AddWithValue("@receiver_email", messageToCreate.ReceiverEmail);

                cmd.ExecuteScalar();

                if (cmd.LastInsertedId != null)
                    cmd.Parameters.Add(new MySqlParameter("newId", cmd.LastInsertedId));

                _messages.Add(messageToCreate);
                _messages[_messages.Count - 1].MessageId = Convert.ToInt32(cmd.Parameters["@newId"].Value);
            }
            return _messages[_messages.Count - 1];
        }

        public Message Update(Message messageToUpdate)
        {
            var obj = _messages.FirstOrDefault(x => x.MessageId == messageToUpdate.MessageId);
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE message SET content = @content, attachment = @attachment, time_sent = @time_sent, sender_email = sender_email, receiver_email = @receiver_email WHERE message_id = @id", con);

                cmd.Parameters.AddWithValue("@id", messageToUpdate.MessageId);
                cmd.Parameters.AddWithValue("@content", messageToUpdate.Content);
                cmd.Parameters.AddWithValue("@attachment", messageToUpdate.Attachment);
                cmd.Parameters.AddWithValue("@time_sent", messageToUpdate.TimeSent);
                cmd.Parameters.AddWithValue("@sender_email", messageToUpdate.SenderEmail);
                cmd.Parameters.AddWithValue("@receiver_email", messageToUpdate.ReceiverEmail);

                cmd.ExecuteNonQuery();

                if (obj != null)
                {
                    obj.Content = messageToUpdate.Content;
                    obj.Attachment = messageToUpdate.Attachment;
                    obj.TimeSent = messageToUpdate.TimeSent;
                    obj.SenderEmail = messageToUpdate.SenderEmail;
                    obj.ReceiverEmail = messageToUpdate.ReceiverEmail;
                }
            }
            return obj;
        }

        public bool Delete(Message messageToDelete)
        {
            int rowsAffected = 0;
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM message WHERE message_id = @id", con);

                cmd.Parameters.AddWithValue("@id", messageToDelete.MessageId);

                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected != 0;
        }
    }
}
