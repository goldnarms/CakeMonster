using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;
using Serilog;

namespace Kakemons.SDK.ApiServices
{
    public class BakerApiService :BaseApiService<IBakerApi>, IBakerApiService
    {
        public BakerApiService(ILogger logger, HttpClient client) : base(logger, client)
        {

        }

        public async Task<BakerDto> GetBaker(string id)
        {
            try
            {
                return await Api.Get(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, nameof(GetBaker));
                return null;
            }
        }
    }
}
