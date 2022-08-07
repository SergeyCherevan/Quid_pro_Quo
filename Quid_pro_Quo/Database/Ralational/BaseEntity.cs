using MongoDB.Bson.Serialization.Attributes;

namespace Quid_pro_Quo.Database.Ralational
{
    public abstract class BaseEntity<IdType>
    {
        [BsonId]
        public IdType Id { get; set; }
    }
}
