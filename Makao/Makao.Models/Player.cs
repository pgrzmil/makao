using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public class Player
    {
        public String Name { get; set; }
        protected List<Card> hand;

        public Player(string name)
        {
            Name = name;
            hand = new List<Card>();
        }
    }
}
