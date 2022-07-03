using Crypto.Data;
using Crypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly crypto_web_app_dbContext db;

        public BlogController(crypto_web_app_dbContext db)
        {
            this.db = db;
        }

        // GET: api/BlogPost
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            return await db.BlogPosts.ToListAsync();
        }

        [HttpPost("getBlogPost")]
        public async Task<ActionResult> GetBlogPost(ThisPost thisPost) {
            bool flag = false;
            BlogPost post = db.BlogPosts.Find(thisPost.postId);

            if (post != null)
            {
                BlogPost blogPost = new BlogPost()
                {
                    Id = post.Id,
                    BitUserId = post.BitUserId,
                    Title = post.Title,
                    Description = post.Description,
                    Status = post.Status,
                    CreatedAt = post.CreatedAt,
                    Image = post.Image
                };
                flag = true;
                return Ok(new { success = flag, data= blogPost });
            }
            else {
                return Ok(new { success = flag });
            }
        }

        [HttpPost("deleteBlogPost")]
        public async Task<ActionResult> DeleteBlogPost(ThisPost thisPost)
        {
            bool flag = false;
            BlogPost post = db.BlogPosts.Find(thisPost.postId);

            if (post != null)
            {
                db.BlogPosts.Remove(post);
                db.SaveChanges();
                flag = true;
            }
          
            return Ok(new { success = flag });
            
        }

        [HttpPost("createBlogPost")]
        public async Task<ActionResult> CreateNewBlogPost(newForumData newBlog)
        {
            bool flag = false;
            try
            {
                Login user = db.Logins.Find(newBlog.userId);
                BlogPost blog = new BlogPost()
                {
                   BitUserId = newBlog.userId,
                   Title =newBlog.title,
                   Description= newBlog.description,
                   Status = "active",
                   CreatedAt = DateTime.Now,
                   Image ="-"
                };

                db.BlogPosts.Add(blog);
                db.SaveChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return Ok(new { success = flag });
        }

        [HttpPost("updateBlogPost")]
        public async Task<ActionResult> UpdateBlogPost(EditBlogData editBlog)
        {
            bool flag = false;
            try
            {
                BlogPost post = db.BlogPosts.Find(editBlog.blogId);
                if (post != null) {
                    post.Title = editBlog.title;
                    post.Description= editBlog.description;

                    db.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return Ok(new { success = flag });
        }


        [HttpGet("{currentUserId}")]
        public String GetBlogPostsList(int? currentUserId)
        {
            List<BlogPost> list = db.BlogPosts.ToList();
            
            List<BlogPostView> postist = new List<BlogPostView>();
            foreach (var item in list)
            {
                Login login = db.Logins.Find(item.BitUserId);

                BlogPostData post = new BlogPostData()
                {
                    ID = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Image = item.Image,
                    CreatedAt = item.CreatedAt.ToString(),
                    Status = item.Status,
                    BitUserID = (int)item.BitUserId,
                    BitUserName = login.FirstName.ToString(),
                    postAge = "1 min",
                    isThisUserLiked = false                    
                };

                int commentCount = 0;
                // Make Comment list
                List<BlogPostComment> comt_list = db.BlogPostComments.Where(l => l.BlogPostId == item.Id).ToList();
                List<CommentView> comments = new List<CommentView>();
                foreach (var cmt in comt_list)
                {
                       commentCount++;
                       CommentView comment_ = new CommentView() {
                        ID = cmt.Id,
                        CommentText= cmt.CommentText,
                        BitUserName = cmt.BitUserName,
                        CommentedAt= cmt.CommentedAt.ToString(),
                        BitUserID = (int) cmt.BitUserId,                        
                    };
                    comments.Add(comment_);
                };

                int likeCount= 0;
                int thisUserLikeCount= 0;
                // Make Like list
                List<BlogPostLike> like_list = db.BlogPostLikes.Where(l => l.BlogPostId == item.Id).ToList();
                List<LikeView> likes = new List<LikeView>();
                foreach (var like in like_list)
                {
                    likeCount++;
                    if (like.BitUserId == currentUserId)
                    {
                         thisUserLikeCount++;                       
                    }
                    //thisUserLikeCount = (int)currentUserId;
                    LikeView likeView_ = new LikeView()
                    {
                      ID = like.Id,
                      BitUserName= like.BitUserName,
                      BitUserID= (int) like.BitUserId,
                      LikedAt= like.LikedAt.ToString(),
                    };
                    likes.Add(likeView_);
                };

                post.likeCount = likeCount;
                post.commentCount = commentCount;
                post.isThisUserLiked = thisUserLikeCount > 0 ? true : false;
                Console.WriteLine("================");
                BlogPostView blogPostView = new BlogPostView() {
                    blogPost = post,
                    comments = comments,
                    likes = likes,
                };
                 postist.Add(blogPostView);
            }      
           
            return Newtonsoft.Json.JsonConvert.SerializeObject(postist);
           //  return id.ToString();
        }


        [HttpPost("commentBlogPost")]
        public async Task<ActionResult> CommentBlogPost(blogCommentData comData)
        {
            bool flag = false;
            try
            {
                Login user = db.Logins.Find(comData.userId);
                BlogPostComment comment = new BlogPostComment()
                {
                   BitUserId = comData.userId,
                   BitUserName = user.FirstName,
                   BlogPostId = comData.blogPostID,
                   CommentText = comData.comment,
                   CommentedAt = DateTime.Now
                };

                db.BlogPostComments.Add(comment);
                await db.SaveChangesAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return Ok(new { success = flag });
        }

        [HttpPost("doLike")]
        public async Task<ActionResult> DoLike(doLikeRequest doLikeData)
        {
            bool flag = false;
            //Find user exists
            if (db.Logins.Any(u => u.Id == doLikeData.thisUserId)){
                if (doLikeData.isLiked) // user has liked
                {
                    //Check Already liked
                    if (!db.BlogPostLikes.Any(u => u.BitUserId == doLikeData.thisUserId && u.BlogPostId==doLikeData.postId))
                    {
                        Login login = db.Logins.Find(doLikeData.thisUserId);
                        //If Not liked =>
                        BlogPostLike like = new BlogPostLike() { 
                            BitUserId = doLikeData.thisUserId,
                            BitUserName = login.FirstName,
                            BlogPostId = doLikeData.postId,
                            LikedAt= DateTime.Now
                        };
                        db.BlogPostLikes.Add(like);
                        await db.SaveChangesAsync();
                        flag = true;
                    }
                }
                else // user has un-liked
                {
                    //delete like
                    try
                    {
                        BlogPostLike like = db.BlogPostLikes.Single(u => u.BitUserId == doLikeData.thisUserId && u.BlogPostId == doLikeData.postId);
                        if (like != null)
                        {
                            db.BlogPostLikes.Remove(like);
                            await db.SaveChangesAsync();
                            flag = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Ok(new { success = flag, data= doLikeData });
        }
    }

    public class BlogPostView
    {
        public BlogPostData blogPost { get; set; }
        public List<CommentView> comments { get; set; }
        public List<LikeView> likes { get; set; }        
    }

    public class BlogPostData
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
        public int BitUserID { get; set; }
        public string BitUserName { get; set; }
        public string postAge { get; set; }
        public int likeCount { get; set; }
        public int commentCount { get; set; }
        public bool isThisUserLiked { get; set; }
    }

    public class CommentView
    {
        public int ID { get; set; }
        public string CommentText { get; set; }
        public string BitUserName { get; set; }
        public int BitUserID { get; set; }
        public string CommentedAt { get; set; }
    }

    public class LikeView
    {
        public int ID { get; set; }
        public string LikedAt { get; set; }
        public string BitUserName { get; set; }
        public int BitUserID { get; set; }
    }


    public class blogCommentData
    {
        public int blogPostID { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
    }

    public class ThisUser
    {
        public int thisUserId { get; set; }
    }

    public class ThisPost
    {
        public int postId { get; set; }
    }

    public class doLikeRequest
    {
        public int thisUserId { get; set; }
        public int postId { get; set; }
        public bool isLiked { get; set; }
    }

    public class EditBlogData
    {
        public int blogId { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}