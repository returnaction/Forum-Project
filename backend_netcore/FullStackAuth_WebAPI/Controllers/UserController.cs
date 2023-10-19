using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _context.Users.ToList();

                var userDto = users.Select(u => new UserForDisplayDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    RegistrationDate = u.RegistrationDate.ToString("yyyy-MM-dd"),

                }).ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserData(string id)
        {
            try
            {
                var user = _context.Users
                    .Include(t => t.Topics)
                    .Include(c => c.Comments)
                    .FirstOrDefault(u => u.Id == id);

                if (user is null)
                    return NotFound();

                var topics = _context.Topics.Include(u => u.AuthorOfTopic).Where(user => user.AuthorOfTopicId == id).ToList();
                var comments = _context.Comments.Where(user => user.CommentOfUserId == id).ToList();

                var userDto = new UserForDisplayDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate.ToString("yyyy-MM-dd"),
                    ProfilePictureB64Base = user.ImageData,
                    Topics = topics.Select(t => new TopicForDisplayDto
                    {
                        TopicId = t.TopicId,
                        Title = t.Title,
                        TimePosted = t.TimePosted.ToString("yyyy-MM-dd"),
                        Likes = t.Likes,
                        Text = t.Text,
                        IsEdited = t.IsEdited,
                        EditedDate = t.EditedDate,
                        AuthorOfTopic = new UserForDisplayDto
                        {
                            UserName = t.AuthorOfTopic.UserName
                        }
                    }).ToList(),
                    Comments = comments.Select(c => new CommentForDisplayDto
                    {
                        CommentId = c.CommentId,
                        Text = c.Text,
                        TimePosted = c.TimePosted.ToString("yyyy-MM-dd"),
                        Likes = c.Likes,
                        IsEdited = c.IsEdited,
                        EditedDate = c.EditedDate,
                        CommentOfUser = new UserForDisplayDto
                        {
                            Id = c.CommentOfUser.Id,
                            UserName = c.CommentOfUser.UserName,
                            ProfilePictureB64Base = c.CommentOfUser.ImageData
                            
                        },
                    }).ToList()
                };


                return Ok(userDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut, Authorize]
        public IActionResult UpdateUser([FromForm] User user)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var existUser = _context.Users.FirstOrDefault(user => user.Id == userId);
                if (existUser is null)
                    return NotFound();

                existUser.LastName = user.LastName;
                existUser.FirstName = user.FirstName;
                existUser.Email = user.Email;

                if (user.File != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        user.File.CopyTo(memoryStream);
                        using (var image = Image.FromStream(memoryStream, true))
                        {
                            existUser.ImageData = ImageBase64Encode(image);
                        }
                    }
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.SaveChanges();
                return StatusCode(201);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private string ImageBase64Encode(Image img)
        {
            using (Image image = img)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

    }
}
