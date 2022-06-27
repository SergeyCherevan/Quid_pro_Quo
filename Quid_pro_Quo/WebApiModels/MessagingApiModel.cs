using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class MessagingApiModel
    {
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public IList<MessageApiModel> MessagesList { get; set; }
    }
}
