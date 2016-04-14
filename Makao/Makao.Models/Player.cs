﻿using Makao.Models;
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
        public List<Card> Hand { get; set; }
        public string SessionId { get; set; }
        public string ConnectionId { get; set; }
        public bool IsReady { get; set; }
        public bool IsTurn { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            IsReady = false;
            IsTurn = false;
        }
    }
}