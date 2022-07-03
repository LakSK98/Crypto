#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string ImageLink { get; set; }
        public bool? IsApproved { get; set; }
        public int? PostedBy { get; set; }

        public virtual Login PostedByNavigation { get; set; }
    }
}