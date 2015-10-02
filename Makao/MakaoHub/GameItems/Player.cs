using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makao.Models;

namespace Makao.GameItems
{
    class Player: Models.Player        
    {
        public Player(string name) : base(name)
        {
        }

        public void AddCards(IEnumerable<Card> cards)
        {
            this.hand.AddRange(cards);
        }

    }
}
