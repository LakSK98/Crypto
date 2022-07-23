#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class Login
    {
        public Login()
        {
            AlertHistories = new HashSet<AlertHistory>();
            BlogPostComments = new HashSet<BlogPostComment>();
            BlogPostLikes = new HashSet<BlogPostLike>();
            BlogPosts = new HashSet<BlogPost>();
            Events = new HashSet<Event>();
            ForumFeeds = new HashSet<ForumFeed>();
            Forums = new HashSet<Forum>();
	    Questions = new HashSet<Question>();
            PriceTracks = new HashSet<PriceTrack>();
            UserAlertPackages = new HashSet<UserAlertPackage>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string AboutMe { get; set; }
        public string Role { get; set; }

        public virtual ICollection<AlertHistory> AlertHistories { get; set; }
        public virtual ICollection<BlogPostComment> BlogPostComments { get; set; }
        public virtual ICollection<BlogPostLike> BlogPostLikes { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ForumFeed> ForumFeeds { get; set; }
        public virtual ICollection<Forum> Forums { get; set; }
	public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<PriceTrack> PriceTracks { get; set; }
        public virtual ICollection<UserAlertPackage> UserAlertPackages { get; set; }
    }
}