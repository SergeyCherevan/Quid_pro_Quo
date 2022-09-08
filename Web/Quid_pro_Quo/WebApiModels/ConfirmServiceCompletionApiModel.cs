using System;

namespace Quid_pro_Quo.WebApiModels
{
    public class ConfirmServiceCompletionApiModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
