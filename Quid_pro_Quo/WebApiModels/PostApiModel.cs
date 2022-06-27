using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quid_pro_Quo.WebApiModels
{
    public class PostApiModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImagesFileName { get; set; }
        public int AuthorName { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsActual { get; set; }

        public IList<DateTime> PerformServiceOnDatesList { get; set; }

        // In Format: 51°30'26.0"N+0°07'39.0"W
        public string PerformServiceInPlace { get; set; }
    }
}
