using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Database.NoSql
{
    public class MessagingEntity : BaseEntity<ObjectId>
    {
        [BsonElement("user1Id")]
        public int User1Id { get; set; }

        [BsonElement("user2Id")]
        public int User2Id { get; set; }

        [BsonElement("messagesList")]
        public IList<MessageEntity> MessagesList { get; set; }
    }
}
