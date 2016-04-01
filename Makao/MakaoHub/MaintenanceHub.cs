using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub
{
    public class MaintenanceHub : BaseHub
    {
        public void ResetData()
        {
            SharedData.ResetData();
        }
    }
}