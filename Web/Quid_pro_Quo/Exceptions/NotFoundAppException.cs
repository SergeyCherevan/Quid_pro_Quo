using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.Exceptions
{
    public class NotFoundAppException : ApplicationException
    {
        public NotFoundAppException(string message = null, Exception exception = null) : base(message, exception) { }
    }
}
