using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FullStackAuth_WebAPI.Models
{
    public class Comment
    {

        [Key]
        public int CommentId { get; set; }

        [MaxLength(200)]
        public string Text { get; set; }
        public DateTime TimePosted { get; set; }
        public int Likes { get; set; } = 0;

        public DateTime EditedDate { get; set; }
        public bool IsEdited { get; set; } = false;

        //Nav props

        [ForeignKey("User")]
        public string CommentOfUserId { get; set; }
        public User CommentOfUser { get; set; }


        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
