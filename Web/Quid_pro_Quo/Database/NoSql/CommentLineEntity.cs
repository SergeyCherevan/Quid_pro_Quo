using System.Collections.Generic;

using MongoDB.Bson.Serialization.Attributes;

using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Database.NoSql
{
    public class CommentLineEntity
    {
        [BsonElement("сommentedContentId")]
        public int CommentedContentId { get; set; }

        [BsonElement("сommentType")]
        public CommentEnum CommentType { get; set; }

        [BsonElement("сommentsList")]
        public IList<MessageEntity> CommentsList { get; set; }
    }

    public enum CommentEnum
    {
        PostComment,
        UserPageComment,
    }
}
