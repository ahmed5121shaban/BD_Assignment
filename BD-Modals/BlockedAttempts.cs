using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Modals
{
    public class BlockedAttempts
    {
        public string IpAddress {  get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; }
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; }

        public BlockedAttempts(string ipAddress, string countryCode, bool isBlocked, string userAgent)
        {
            IpAddress = ipAddress;
            Timestamp = DateTime.UtcNow;
            CountryCode = countryCode;
            IsBlocked = isBlocked;
            UserAgent = userAgent;
        }
    }
}
