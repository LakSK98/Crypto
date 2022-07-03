using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Models
{
    public class Payment
    {
        public long Amount { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}
