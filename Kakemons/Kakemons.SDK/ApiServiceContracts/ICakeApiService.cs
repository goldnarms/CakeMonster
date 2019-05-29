using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Models;

namespace Kakemons.SDK.ApiContracts
{
    public interface ICakeApiService
    {
        Task<IEnumerable<CakeDto>> GetNearbyCakes(DbPosition argPosition);
    }
}
