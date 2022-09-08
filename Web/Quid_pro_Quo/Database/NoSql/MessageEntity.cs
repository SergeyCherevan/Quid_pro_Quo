using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Database.NoSql
{
    public class MessageEntity : BaseEntity<int>
    {
        // 0 or 1
        [BsonElement("authorId")]
        public int AuthorId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("imageFileName")]
        public string ImageFileName { get; set; }

        [BsonElement("fileName")]
        public string FileName { get; set; }

        [BsonElement("postedAt")]
        public DateTime PostedAt { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("notViewed")]
        public bool? NotViewed { get; set; }
    }
}
