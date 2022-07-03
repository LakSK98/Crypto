#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class PriceTrack
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public int? BitUserId { get; set; }

        public virtual Login BitUser { get; set; }
    }
}