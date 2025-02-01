using BD_Dtos;
using BD_Modals;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Services
{
    public class BlockedCountriesService
    {
        private readonly IBlockedCountryRepository _repository;
        private readonly IIpApiService _ipApiService;

        public BlockedCountriesService(IBlockedCountryRepository repository, IIpApiService ipApiService)
        {
            _repository = repository;
            _ipApiService = ipApiService;
        }

        public bool AddBlockedCountry(string ip)
        {
            try
            {
                var country = _ipApiService.GetCountryByIpAsync(ip).Result;
                var blockedCountry = new BlockedCountry(ip, country.City, country.Region, country.Country,  country.Currency.Name, country.Org, country.CountryCode);
                return _repository.AddBlockedCountry(blockedCountry);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error adding blocked country: {ex.Message}");
                return false;
            }
        }

        public bool RemoveBlockedCountry(string countryCode)
        {
            return _repository.RemoveBlockedCountry(countryCode);
        }

        public List<BlockedCountry> GetAllBlockedCountries(int page, int pageSize, string filter = null)
        {
            var countries = _repository.GetAllBlockedCountries();
            if (!string.IsNullOrEmpty(filter))
            {
                countries = countries.Where(c => c.Country.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return countries.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<IpApiDto> FindMyCountry(string ipAddress)
        {
            var country = await _ipApiService.GetCountryByIpAsync(ipAddress);
            return country;
        }

        public bool VerifyBlockedCountry(string ipAddress)
        {
            try
            {
                var country = _ipApiService.GetCountryByIpAsync(ipAddress).Result;
                return _repository.IsBlocked(country.Ip);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error verifying blocked country: {ex.Message}");
                return false;
            }
        }

        public void LogBlockedAttempt(string ipAddress, string countryCode, bool isBlocked, string userAgent)
        {
            var attempt = new BlockedAttempts(ipAddress, countryCode, isBlocked, userAgent);
            _repository.AddBlockedAttempt(attempt);
        }

        public List<BlockedAttempts> GetAllBlockedAttempts(int page, int pageSize, string filter = null)
        {
            var logs = _repository.GetAllBlockedAttempts();
            if (!string.IsNullOrEmpty(filter))
            {
                logs = logs.Where(l => l.CountryCode.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public int AddTemporalBlock(string countryCode, int durationMinutes)
        {
            return _repository.AddTemporalBlock(countryCode,durationMinutes);
        }
    }
}
