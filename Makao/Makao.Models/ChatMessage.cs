using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public Player Sender { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
