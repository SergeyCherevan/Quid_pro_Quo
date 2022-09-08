using System.ComponentModel.DataAnnotations.Schema;

namespace Quid_pro_Quo.Database.Ralational
{
    [Table("Complaints")]
    public class ComplaintEntity : BaseEntity<int>
    {
        public ComplaintEnum ComplaintType { get; set; }
        public int ComplaintedContentId { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual UserEntity Author { get; set; }
    }

    public enum ComplaintEnum
    {
        UserPage,
        Post,
        PostComment,
        UserPageComment,
        Message,
    }
}
