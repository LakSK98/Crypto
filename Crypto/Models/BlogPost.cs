#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class BlogPost
    {
        public BlogPost()
        {
            BlogPostComments = new HashSet<BlogPostComment>();
            BlogPostLikes = new HashSet<BlogPostLike>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; }
        public virtual ICollection<BlogPostLike> BlogPostLikes { get; set; }
    }
}