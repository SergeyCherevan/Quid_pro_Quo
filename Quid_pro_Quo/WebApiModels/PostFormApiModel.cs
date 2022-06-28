using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class PostFormApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public IEnumerable<IFormFile> ImageFiles { get; set; }

        public IEnumerable<DateTime> PerformServiceOnDatesList { get; set; }

        // In Format: 51°30'26.0"N+0°07'39.0"W
        public string PerformServiceInPlace { get; set; }
    }
}
