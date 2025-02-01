using BD_Modals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBlockedCountryRepository
    {
        bool IsBlocked(string ipAddress);
        bool AddBlockedCountry(BlockedCountry country);
        bool RemoveBlockedCountry(string countryCode);
        List<BlockedCountry> GetAllBlockedCountries();
        void AddBlockedAttempt(BlockedAttempts attempt);
        List<BlockedAttempts> GetAllBlockedAttempts();
        int AddTemporalBlock(string countryCode, int durationMinutes);
        ConcurrentDictionary<string, TemporalBlockedCountry> temporalBlockedCountries { get; }
        ConcurrentDictionary<string, BlockedCountry> BlockedCountries { get; }
    }


}
