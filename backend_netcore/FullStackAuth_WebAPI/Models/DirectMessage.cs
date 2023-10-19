using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FullStackAuth_WebAPI.Models
{
    public class DirectMessage
    {
        [Key]
        public int DirectMessageId { get; set; }

        [MaxLength(200, ErrorMessage = "200 symbols max")]
        public string Text { get; set; }
        public DateTime MessageTime { get; set; }

        //nav
        public string ToUserId { get; set; }
        [ForeignKey("ToUserId")]
        public User ToUser { get; set; }

        public string FromUserId { get; set; }
        [ForeignKey("FromUserId")]
        public User FromUser { get; set; }

        public class DirectMessageDateComparer : IComparer<DirectMessage>
        {
            public int Compare(DirectMessage message1, DirectMessage message2)
            {
                if (message1.MessageTime > message2.MessageTime)
                    return 1;
                else if (message1.MessageTime < message2.MessageTime)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
