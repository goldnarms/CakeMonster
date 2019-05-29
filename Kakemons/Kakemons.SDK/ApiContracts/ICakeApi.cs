using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Models;
using Refit;

namespace Kakemons.SDK.ApiContracts
{
    public interface ICakeApi
    {
        [Get("/Cake/GetNearbyCakes")]
        Task<IEnumerable<CakeDto>> GetNearbyCakes(DbPosition argPosition);
    }
}
