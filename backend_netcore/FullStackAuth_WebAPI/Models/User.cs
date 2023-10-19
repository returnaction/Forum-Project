using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAuth_WebAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        //public IFormFile ProilePicture { get; set; }
        public string? ImageData { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }



        //Nav props

        public List<Topic> Topics { get; set; }
        public List<Comment> Comments { get; set; }
        public List<DirectMessage> DirectMessagesFrom { get; set; }
        public List<DirectMessage> DirectMessagesTo { get; set; }
    }
}
