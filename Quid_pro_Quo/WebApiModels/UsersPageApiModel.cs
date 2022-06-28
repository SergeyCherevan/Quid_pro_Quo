using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class UsersPageApiModel
    {
        public IEnumerable<UserApiModel> Users { get; set; }
        public int UsersCount { get; set; }
    }
}
