using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Requests;
using Kakemons.Common.Responses;
using Refit;

namespace Kakemons.SDK.ApiContracts
{
    public interface IAccountApi
    {
        [Get("/Account/Login")]
        Task<LoginUserResponse> Login([Body] LoginRequest model);
    }
}
