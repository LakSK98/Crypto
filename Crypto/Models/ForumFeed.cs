#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class ForumFeed
    {
        public int Id { get; set; }
        public string FeedText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string BitUserName { get; set; }
        public int? ForumId { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
        public virtual Forum Forum { get; set; }
    }
}