#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class BlogPostComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime? CommentedAt { get; set; }
        public string BitUserName { get; set; }
        public int? BlogPostId { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
        public virtual BlogPost BlogPost { get; set; }
    }
}