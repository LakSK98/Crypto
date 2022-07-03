using Crypto.Data;
using Crypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly crypto_web_app_dbContext db;

        public ForumController(crypto_web_app_dbContext context)
        {
            db = context;
        }

        // GET: Forum
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forum>>> Index()
        {
            return await db.Forums.ToListAsync();
        }

       
        [HttpPost("feedForum")]
        public async Task<ActionResult> FeedForum(commentData comData)
        {
            bool flag = false;            
            try
            {
                Login user = db.Logins.Find(comData.userId);
                ForumFeed feed = new ForumFeed() {
                    BitUserId = user.Id,
                    BitUserName= user.FirstName,
                    FeedText= comData.comment,
                    ForumId = comData.forumID,
                    CreatedAt = DateTime.Now,                    
                };

                db.ForumFeeds.Add(feed);
                await db.SaveChangesAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return Ok(new { success = flag });
        }

       
        [HttpPost("newForum")]
        public async Task<ActionResult> CreateNewForum(newForumData newForum)
        {
            bool flag = false;
            try
            {
                Login user = db.Logins.Find(newForum.userId);
                Forum forum = new Forum()
                {
                    BitUserId = user.Id,
                    BitUserName = user.FirstName,
                    Image = "https://thumbs.dreamstime.com/b/bitcoin-chat-bitcoin-forum-bitcoin-news-cryptocurrency-live-chat-fully-editable-vector-icons-bitcoin-chat-bitcoin-forum-bitcoin-208002129.jpg",
                    CreatedAt = DateTime.Now,
                    Title = newForum.title,
                    Description = newForum.description,                    
                };

                db.Forums.Add(forum);
                await db.SaveChangesAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return Ok(new { success = flag });
        }

    }

    public class commentData {
        public int forumID { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
    } 
    
    public class newForumData {
        public int userId { get; set; }
        public string title { get; set; }     
        public string description { get; set; }
    }
}