using System;

namespace Quid_pro_Quo.WebApiModels
{
    public class MessageApiModel
    {
        public int Id { get; set; }

        // 0 or 1
        public bool AuthorNumber { get; set; }

        public string Text { get; set; }

        public string ImageFileName { get; set; }

        public string FileName { get; set; }

        public DateTime PostedAt { get; set; }
    }
}
