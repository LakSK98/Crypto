#nullable disable
using System;
using System.Collections.Generic;

namespace Crypto.Models
{
    public partial class AlertPackage
    {
        public AlertPackage()
        {
            UserAlertPackages = new HashSet<UserAlertPackage>();
        }

        public int Id { get; set; }
        public string PackageName { get; set; }
        public int? NoOfAlerts { get; set; }
        public string Description { get; set; }
        public int? DurationInDays { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<UserAlertPackage> UserAlertPackages { get; set; }
    }
}