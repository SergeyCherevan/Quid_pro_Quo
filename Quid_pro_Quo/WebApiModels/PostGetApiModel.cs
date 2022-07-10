using System;
using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class PostGetApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageFileNames { get; set; }
        public string AuthorName { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsActual { get; set; }

        public IEnumerable<DateTime> PerformServiceOnDatesList { get; set; }

        // In Format: @39.6192195,-71.7422487,6.99z
        public string PerformServiceInPlace { get; set; }
    }
}
