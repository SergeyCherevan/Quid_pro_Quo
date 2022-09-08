using Microsoft.AspNetCore.Http;

namespace Quid_pro_Quo.WebApiModels
{
    public class AccountFormApiModel
    {
        public string UserName { get; set; }
        public IFormFile AvatarFile { get; set; }
        public string Biographi { get; set; }
    }
}
