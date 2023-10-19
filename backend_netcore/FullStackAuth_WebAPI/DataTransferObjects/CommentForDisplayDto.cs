namespace FullStackAuth_WebAPI.DataTransferObjects
{
    public class CommentForDisplayDto
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public string TimePosted { get; set; }
        public int Likes { get; set; }

        public DateTime EditedDate { get; set; }
        public bool IsEdited { get; set; } 

        public UserForDisplayDto CommentOfUser { get; set; }
    }
}
