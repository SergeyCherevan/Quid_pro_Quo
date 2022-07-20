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

        public double PerformServiceInPlaceLat { get; set; }
        public double PerformServiceInPlaceLng { get; set; }
        public double PerformServiceInPlaceZoom { get; set; }
    }
}
