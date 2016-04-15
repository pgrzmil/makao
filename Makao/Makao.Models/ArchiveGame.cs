using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public class ArchiveGame
    {
        public int ArchiveGameID { get; set; }
        public DateTime Date { get; set; }
        public int Points { get; set; }

        public Player WinnerID { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
