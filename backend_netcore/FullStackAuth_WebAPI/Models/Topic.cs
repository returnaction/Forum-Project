using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAuth_WebAPI.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(10000)]
        public string Text { get; set; }

        public DateTime TimePosted { get; set; }
        public int Likes { get; set; } = 0;

        public DateTime EditedDate { get; set; }
        public bool IsEdited { get; set; } = false;

        // Nav props

        [ForeignKey("User")]
        public string AuthorOfTopicId { get; set; }
        public User AuthorOfTopic { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
