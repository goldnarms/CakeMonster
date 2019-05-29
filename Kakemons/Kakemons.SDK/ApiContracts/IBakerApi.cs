using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Refit;

namespace Kakemons.SDK.ApiContracts
{
    public interface IBakerApi
    {
        [Get("/Baker")]
        Task<BakerDto> Get(string id);
    }
}
