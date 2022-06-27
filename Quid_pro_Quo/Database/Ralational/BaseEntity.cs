namespace Quid_pro_Quo.Database.Ralational
{
    public abstract class BaseEntity<IdType>
    {
        public IdType Id { get; set; }
    }
}
