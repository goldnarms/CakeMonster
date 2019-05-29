using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Parameters;
using Kakemons.Common.Responses;

namespace Kakemons.SDK.ApiContracts
{
    public interface IUserApiService
    {
        Task<IEnumerable<CakeDto>> GetFavourites(string userId);
        Task<UserDto> GetUser();
        Task<RegisterResult> RegisterUser(RegisterUserParameter registerUserParameter);
    }
}
