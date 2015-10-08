using Makao.Models;
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
        public Guid PlayerId { get; set; }

        public Player(string name)
        {
            Name = name;
            PlayerId = Guid.NewGuid();
        }
    }
}
