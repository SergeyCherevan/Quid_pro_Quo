using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.WebApiModels
{
    public class UsersPageApiModel
    {
        public IEnumerable<UserApiModel> Users { get; set; }
        public int UsersCount { get; set; }
    }
}
