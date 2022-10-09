using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.AttachingRequests
{
    public interface IAttachingRequestsQueue
    {
        void Add((string OwnerName, int IoTCode) request);

        (string OwnerName, int IoTCode) GetByCode(int IoTCode);

        (string OwnerName, int IoTCode) GetByOwner(string OwnerName);

        void DeleteByCode(int IoTCode);
    }
}
