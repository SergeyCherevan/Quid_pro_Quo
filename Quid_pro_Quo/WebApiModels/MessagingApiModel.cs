using System.Collections.Generic;

namespace Quid_pro_Quo.WebApiModels
{
    public class MessagingApiModel
    {
        public string User1Name { get; set; }
        public string User2Name { get; set; }
        public IEnumerable<MessageApiModel> MessagesList { get; set; }
    }
}
