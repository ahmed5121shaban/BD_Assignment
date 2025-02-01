using BD_Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD_Dtos;
namespace BD_Services
{
    public interface IIpApiService
    {
        Task<IpApiDto> GetCountryByIpAsync(string ip);
    }

}
