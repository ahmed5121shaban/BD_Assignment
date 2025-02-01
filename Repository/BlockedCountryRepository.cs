using BD_Modals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BlockedCountryRepository:IBlockedCountryRepository
    {
        public bool IsBlocked(string ipAddress) => BlockedCountries.ContainsKey(ipAddress);

        public ConcurrentDictionary<string, BlockedCountry> BlockedCountries { get; } = new();
        public List<BlockedAttempts> BlockedAttempts { get; } = new();
        public ConcurrentDictionary<string, TemporalBlockedCountry> temporalBlockedCountries { get; } = new();
        public bool AddBlockedCountry(BlockedCountry country)
        {
            return BlockedCountries.TryAdd(country.Ip, country);
        }

        public bool RemoveBlockedCountry(string countryCode)
        {
            var res = string.Empty;
            foreach (KeyValuePair<string, BlockedCountry> item in BlockedCountries)
            {
                if(item.Value.CountryCode == countryCode)
                    res = item.Key;
            }
            return BlockedCountries.TryRemove(res, out _);
        }

        public List<BlockedCountry> GetAllBlockedCountries()
        {
            return BlockedCountries.Values.ToList();
        }

        public void AddBlockedAttempt(BlockedAttempts attempt)
        {
            BlockedAttempts.Add(attempt);
        }

        public List<BlockedAttempts> GetAllBlockedAttempts()
        {
            return BlockedAttempts.ToList();
        }
        public int AddTemporalBlock(string countryCode, int durationMinutes)
        {
            if (temporalBlockedCountries.ContainsKey(countryCode))
                return 0;

            var expirationTime = DateTime.UtcNow.AddMinutes(durationMinutes);
            var temporalBlock = new TemporalBlockedCountry
            {
                CountryCode = countryCode,
                DurationMinutes = durationMinutes,
            };

            return temporalBlockedCountries.TryAdd(countryCode, temporalBlock) ? 2 : 1;
        }
    }
}
