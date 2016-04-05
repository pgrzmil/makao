using Makao.Game.Services;
using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Game.Models
{
    public class OpponentModel
    {
        public Player Player { get; set; }
        public HubProxyService Proxy { get; set; }
    }
}