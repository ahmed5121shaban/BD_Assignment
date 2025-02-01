using BD_Dtos;
using BD_Modals;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BD_Services
{
    public class IpApiService:IIpApiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private List<BlockedCountry> ipApis;
        private List<BlockedAttempts> blockedAttempts;
        public IpApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<IpApiDto> GetCountryByIpAsync(string ip)
        {
            var apiUrl = _configuration["IpGeolocation:Url"];
            var apiKey = _configuration["IpGeolocation:ApiKey"];

            var url = $"{apiUrl}?apiKey={apiKey}&ip={ip}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var ipApiResponse = JsonSerializer.Deserialize<IpApiDto>(responseContent);

            return ipApiResponse;
        }

    }
}
