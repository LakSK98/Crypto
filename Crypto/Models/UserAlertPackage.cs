#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class UserAlertPackage
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int? RemainingAlerts { get; set; }
        public int? AlertPackageId { get; set; }
        public int? BitUserId { get; set; }

        public virtual AlertPackage AlertPackage { get; set; }
        public virtual Login BitUser { get; set; }
    }
}