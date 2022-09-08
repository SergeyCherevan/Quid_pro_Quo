using Microsoft.AspNetCore.Http;

namespace Quid_pro_Quo.DTOs
{
    public class AccountFormDTO
    {
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
        public IFormFile AvatarFile { get; set; }
        public string Biographi { get; set; }
    }
}
