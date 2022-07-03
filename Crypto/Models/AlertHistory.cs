#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class AlertHistory
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public string PackageName { get; set; }
        public string SentAt { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
    }
}