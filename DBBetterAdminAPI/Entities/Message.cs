using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime TimeSent { get; set; }
        public byte[]? Attachments { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public virtual Person Receiver { get; set; } = null!;
        public virtual Person Sender { get; set; } = null!;
    }
}
