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
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllComment()
        {
            try
            {
                var comments = _context.Comments.ToList();

                var commentsDTo = comments.Select(c => new CommentForDisplayDto
                {
                    CommentId = c.CommentId,
                    TimePosted = c.TimePosted.ToString("yyyy-MM-dd"),
                    
                    
                }).ToList();

                return Ok(commentsDTo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Authorize]
        public IActionResult PostComment([FromBody] Comment comment)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                comment.CommentOfUserId = userId;
                comment.TimePosted = DateTime.Now;

                _context.Comments.Add(comment);
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

        [HttpDelete("{id}"), Authorize]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var comment = _context.Comments.FirstOrDefault(c => c.CommentId == id);
                if (comment is null)
                    return NotFound();

                if (comment.CommentOfUserId != userId)
                    return Unauthorized();

                _context.Comments.Remove(comment);
                _context.SaveChanges();

                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{commentid}"), Authorize]
        public IActionResult UpdateComment(int commentId, [FromBody] Comment comment)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();


                var existComment = _context.Comments.FirstOrDefault(f => f.CommentId == commentId);
                if (existComment is null)
                    return NotFound();

                if (existComment.CommentOfUserId != userId)
                    return Unauthorized();

                existComment.Text = comment.Text;
                existComment.IsEdited = true;
                existComment.EditedDate = DateTime.Now;

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

        [HttpPut("likecomment/{id}"), Authorize]
        public IActionResult LikeComment(int id)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var comment = _context.Comments.FirstOrDefault(c => c.CommentId == id);

                if (comment is null)
                    return NotFound();

                comment.Likes++;

                _context.SaveChanges();

                return Ok(comment.Likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
