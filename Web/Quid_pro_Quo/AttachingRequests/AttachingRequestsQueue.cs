using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.AttachingRequests
{
    public class AttachingRequestsQueue : IAttachingRequestsQueue
    {
        static // comment this line, when you will add dependency injection to Quartz.NET
        protected LinkedList<(string OwnerName, int IoTCode)> _requests { get; set; }
             = new LinkedList<(string OwnerName, int IoTCode)>();
        public AttachingRequestsQueue() { }

        public void Add((string OwnerName, int IoTCode) request)
            => _requests.AddLast(request);

        public (string OwnerName, int IoTCode) GetByCode(int IoTCode)
            => _requests.FirstOrDefault(e => e.IoTCode == IoTCode);

        public (string OwnerName, int IoTCode) GetByOwner(string OwnerName)
            => _requests.FirstOrDefault(e => e.OwnerName == OwnerName);

        public void DeleteByCode(int IoTCode)
            => _requests.Remove(GetByCode(IoTCode));
    }
}
