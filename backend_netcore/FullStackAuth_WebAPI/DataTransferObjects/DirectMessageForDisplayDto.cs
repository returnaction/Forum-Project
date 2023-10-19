using FullStackAuth_WebAPI.Models;

namespace FullStackAuth_WebAPI.DataTransferObjects
{
    public class DirectMessageForDisplayDto
    {
        public int DirectMessageId { get; set; }
        public string Text { get; set; }
        public DateTime MessageTime { get; set; }


        public string ToUserId { get; set; }
        public UserForDisplayDto ToUser { get; set; }
        public string FromUserId { get; set; }
        public UserForDisplayDto FromUser { get; set; }
    }
}
