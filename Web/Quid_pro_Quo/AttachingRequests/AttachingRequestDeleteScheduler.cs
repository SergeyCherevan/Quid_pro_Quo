using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;

namespace Quid_pro_Quo.AttachingRequests
{
    public class AttachingRequestDeleteScheduler
    {
        public static async Task Start(IServiceProvider serviceProvider, int iotCode)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetRequiredService<AttachingRequestDeleteJobFactory>();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<AttachingRequestDeleterJob>()
                .WithIdentity("AttachingRequestDeleter_Job")
                .UsingJobData("IoTCode", iotCode)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("OneTimeAfter5Minutes_Trigger", "AttachingRequest_Group")     // идентифицируем триггер с именем и группой
                .StartAt(DateTime.Now + new TimeSpan(0, 5, 0))                            // запуск через 5 минут начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInSeconds(0))          // 
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
