using System;

namespace Quid_pro_Quo.WebApiModels
{
    public class MessageApiModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int AuthorName { get; set; }

        public DateTime PostedAt { get; set; }

        public string ImageFileName { get; set; }

        public string FileName { get; set; }

        public bool Viewed { get; set; }
    }
}
