using Microsoft.AspNetCore.Http;

namespace Quid_pro_Quo.WebApiModels
{
    public class SendMessageApiModel
    {
        public string Text { get; set; }

        public IFormFile ImageFile { get; set; }

        public IFormFile File { get; set; }

        public string DestinationName { get; set; }
    }
}
