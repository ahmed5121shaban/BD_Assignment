using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Services
{
    public class BackGroundTaskService
    {
        private readonly IBlockedCountryRepository _repository;

        public BackGroundTaskService(IBlockedCountryRepository repository)
        {
            _repository = repository;
        }
        public void RemoveExpiredTemporalBlocks()
        {
            var now = DateTime.UtcNow;
            var expiredBlocks = _repository.temporalBlockedCountries
                .Where(t => t.Value.ExpirationTime <= now)
                .ToList();

            foreach (var block in expiredBlocks)
            {
                _repository.temporalBlockedCountries.TryRemove(block.Key, out _);
            }
        }
    }
}
