using System.Threading.Tasks;

using Quartz;
using Quid_pro_Quo.Services;
using Quid_pro_Quo.Services.Interfaces;

namespace Quid_pro_Quo.AttachingRequests
{
    public class AttachingRequestDeleterJob : IJob
    {
        protected IIoTService _iotService { get; set; }
        public AttachingRequestDeleterJob()
        {
            _iotService = new IoTService(null, new AttachingRequestsQueue(), null);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _iotService.DeleteRequestToAttach((int)context.MergedJobDataMap["IoTCode"]);
        }
    }
}

//using System.Threading.Tasks;

//using Microsoft.Extensions.DependencyInjection;

//using Quartz;

//using Quid_pro_Quo.Services.Interfaces;

//namespace Quid_pro_Quo.AttachingRequests
//{
//    public class AttachingRequestDeleterJob : IJob
//    {
//        protected IServiceScopeFactory _serviceScopeFactory { get; set; }
//        public AttachingRequestDeleterJob(IServiceScopeFactory serviceScopeFactory)
//        {
//            serviceScopeFactory = _serviceScopeFactory;
//        }

//        public async Task Execute(IJobExecutionContext context)
//        {
//            using (var scope = _serviceScopeFactory.CreateScope())
//            {
//                IIoTService _iotService = scope.ServiceProvider.GetService<IIoTService>();

//                await _iotService.DeleteRequestToAttach((int)context.MergedJobDataMap["IoTCode"]);
//            }
//        }
//    }
//}
