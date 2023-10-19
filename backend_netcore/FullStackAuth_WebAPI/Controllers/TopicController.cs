using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/topic")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TopicController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllTopicsShortData()
        {
            try
            {
                var topics = _context.Topics
                    .Include(u => u.AuthorOfTopic)
                    .ToList();

                var topicsDto = topics.Select(t => new TopicForDisplayDto
                {
                    TopicId = t.TopicId,
                    Title = t.Title,
                    TimePosted = t.TimePosted.ToString("yyyy-MM-dd"),
                    IsEdited = t.IsEdited,
                    EditedDate = t.EditedDate,
                    Likes = t.Likes,

                    AuthorOfTopic = new UserForDisplayDto
                    {
                        Id = t.AuthorOfTopic.Id,
                        UserName = t.AuthorOfTopic.UserName,
                        ProfilePictureB64Base = t.AuthorOfTopic.ImageData
                    }
                    
                }).ToList();
                return Ok(topicsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTopicById(int id)
        {
            try
            {
                var topic = _context.Topics.Include(u => u.AuthorOfTopic)
                                           .Include(c => c.Comments)
                                           .ThenInclude(c => c.CommentOfUser)
                                           .FirstOrDefault(topic => topic.TopicId == id);

                if (topic is null)
                    return NotFound();

                var topicDto = new TopicForDisplayDto
                {
                    TopicId = topic.TopicId,
                    Title = topic.Title,
                    Text = topic.Text,
                    TimePosted = topic.TimePosted.ToString("yyyy-MM-dd"),
                    Likes = topic.Likes,
                    IsEdited = topic.IsEdited,
                    EditedDate = topic.EditedDate,
                    AuthorOfTopic = new UserForDisplayDto
                    {
                        Id = topic.AuthorOfTopic.Id,
                        UserName = topic.AuthorOfTopic.UserName,
                        ProfilePictureB64Base = topic.AuthorOfTopic.ImageData
                    },
                    Comments = topic.Comments.Select(c => new CommentForDisplayDto
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
                            ProfilePictureB64Base = c.CommentOfUser.ImageData,
                        }
                    }).ToList()
                };

                return Ok(topicDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Authorize]
        public IActionResult PostTopic([FromBody] Topic topic)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                topic.AuthorOfTopicId = userId;
                topic.TimePosted = DateTime.Now;

                _context.Topics.Add(topic);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.SaveChanges();

                return StatusCode(201, topic);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteTopic(int id)
        {
            try
            {
                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var topic = _context.Topics.FirstOrDefault(topic => topic.TopicId == id);
                if (topic is null)
                    return NotFound();

                if (topic.AuthorOfTopicId != userId)
                    return Unauthorized();

                _context.Topics.Remove(topic);
                _context.SaveChanges();

                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult UpdateTopic(int id, [FromBody] Topic topic)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var existTopic = _context.Topics.FirstOrDefault(topic => topic.TopicId == id);

                if (existTopic is null)
                    return NotFound();

                if (existTopic.AuthorOfTopicId != userId)
                    return Unauthorized();

                existTopic.Title = topic.Title;
                existTopic.Text = topic.Text;
                existTopic.IsEdited = true;
                existTopic.EditedDate = DateTime.Now;

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

        [HttpPut("liketopic/{id}"), Authorize]
        public IActionResult LikeForTopic(int id)
        {
            try
            {
                var topic = _context.Topics.FirstOrDefault(t => t.TopicId == id);
                if (topic is null)
                    return NotFound();

                topic.Likes++;

                _context.SaveChanges();

                return Ok(topic.Likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
