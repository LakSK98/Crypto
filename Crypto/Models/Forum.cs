#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class Forum
    {
        public Forum()
        {
            ForumFeeds = new HashSet<ForumFeed>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string BitUserName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
        public virtual ICollection<ForumFeed> ForumFeeds { get; set; }
    }
}