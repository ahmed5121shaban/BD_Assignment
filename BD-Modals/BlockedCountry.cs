using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Modals
{
    public class BlockedCountry
    {
        public string Ip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
        public string Currency { get; set; }
        public string Org { get; set; }

        public BlockedCountry(string ip, string city, string region, string country, string currency, string org, string countryCode)
        {
            Ip = ip;
            City = city;
            Region = region;
            Country = country;
            Currency = currency;
            Org = org;
            CountryCode = countryCode;
        }
    }
    
}
