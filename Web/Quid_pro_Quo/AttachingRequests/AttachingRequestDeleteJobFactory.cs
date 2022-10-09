using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Spi;

namespace Quid_pro_Quo.AttachingRequests
{
    public class AttachingRequestDeleteJobFactory : IJobFactory
    {
        protected IServiceScopeFactory _serviceScopeFactory { get; set; }


        public AttachingRequestDeleteJobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                return job;
            }

        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}
