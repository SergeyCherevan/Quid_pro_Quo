using System;
using System.Threading.Tasks;

using Quartz;
using Quid_pro_Quo.Services;
using Quid_pro_Quo.Services.Interfaces;

namespace Quid_pro_Quo.AttachingRequests
{
    public class AttachingRequestDeleterJob : IJob
    {
        protected IIoTService _iotService { get; set; }
        public AttachingRequestDeleterJob(IIoTService iotService)
        {
            _iotService = iotService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _iotService.DeleteRequestToAttach((int)context.MergedJobDataMap["IoTCode"]);
        }
    }
}