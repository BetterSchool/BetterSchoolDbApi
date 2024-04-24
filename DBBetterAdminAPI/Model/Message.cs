namespace BetterAdminDbAPI.Model
{
    public class Message
    {
        public int? MessageId { get; set; }
        public string Content { get; set; }
        public byte[]? Attachment { get; set; }
        public DateTime TimeSent { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
