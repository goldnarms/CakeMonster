using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Common.Models;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;
using Serilog;

namespace Kakemons.SDK.ApiServices
{
    public class CakeApiService : BaseApiService<ICakeApi>, ICakeApiService
    {
        public CakeApiService(ILogger logger, HttpClient client):base(logger, client)
        {
        }

        public async Task<IEnumerable<CakeDto>> GetNearbyCakes(DbPosition position)
        {
            try
            {
                return await Api.GetNearbyCakes(position);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, nameof(GetNearbyCakes));
                return new List<CakeDto>();
            }
        }
    }
}
