using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Quid_pro_Quo.WebApiModels;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.Services;

namespace Quid_pro_Quo.SignalRHubs
{
    [Authorize]
    public class IoTHub : Hub
    {
        public IoTHub() { }
    }
}
