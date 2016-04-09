using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public class ChatMessage
    {
        public Player sender { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
