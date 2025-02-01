using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Modals
{
    public class TemporalBlockedCountry
    {
       public string CountryCode { get; set; }
       public int DurationMinutes { get; set; }
       public DateTime ExpirationTime { get; set; }
    }
}
