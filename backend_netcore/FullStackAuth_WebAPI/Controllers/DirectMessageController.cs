using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FullStackAuth_WebAPI.Models.DirectMessage;

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/directmessage")]
    [ApiController]
    public class DirectMessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DirectMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public IActionResult GetAllChats()
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                
                var usersWithMessages = _context.DirectMessages.Where(m => m.ToUserId == userId || m.FromUserId == userId).ToList();
                //usersWithMessages.Remove(userId);
                //.Select(u => u.FromUserId).Distinct()
                var usersIdList = usersWithMessages.Select(u => u.FromUserId).Distinct().ToList();
                usersIdList.AddRange(usersWithMessages.Select(u => u.ToUserId).Distinct().ToList());
                usersIdList.RemoveAll(u=> u == userId);

                var userList = _context.Users.Where(u => usersIdList.Contains(u.Id)).ToList();
                var userswithMessgesDTO = userList.Select(u => new UserForDisplayDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    
                }).ToList();

                //return Ok(userswithMessgesDTO);
                return Ok(userswithMessgesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("messages/{userFromId}"), Authorize]
        public IActionResult GetMessagesOfUser(string userFromId)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();


                var myMessages = _context.DirectMessages.Include(m=>m.FromUser).Where(ufrom => ufrom.FromUserId == userId && ufrom.ToUserId == userFromId).ToList();
                var userMessages = _context.DirectMessages.Include(m => m.FromUser).Where(ufrom => ufrom.ToUserId == userId && ufrom.FromUserId == userFromId).ToList();

                myMessages.AddRange(userMessages);

                myMessages.Sort(new DirectMessageDateComparer());

                var directMessagesDto = myMessages.Select(m => new DirectMessageForDisplayDto
                {
                    DirectMessageId = m.DirectMessageId,
                    Text = m.Text,
                    MessageTime = m.MessageTime,
                    FromUserId = m.FromUserId,
                    ToUser = new UserForDisplayDto
                    {
                        //UserName = _context.Users.FirstOrDefault(u => u.Id == m.FromUserId).UserName,
                        
                        UserName = m.ToUser != null ? m.ToUser.UserName : string.Empty,
                        ProfilePictureB64Base = m.ToUser != null ? m.ToUser.ImageData : string.Empty,
                    },
                   
                    ToUserId = m.ToUserId,
                    FromUser = new UserForDisplayDto
                    {
                        //UserName = _context.Users.FirstOrDefault(u => u.Id == m.ToUserId).UserName
                        UserName = m.FromUser != null ? m.FromUser.UserName : string.Empty,
                        ProfilePictureB64Base = m.FromUser != null ? m.FromUser.ImageData : string.Empty,
                    }
                }).ToList();

                return Ok(directMessagesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Authorize]
        public IActionResult SendMessage([FromBody] DirectMessage directMessage)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                directMessage.FromUserId = userId;
                directMessage.MessageTime = DateTime.Now;

                _context.DirectMessages.Add(directMessage);
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
    }
}
