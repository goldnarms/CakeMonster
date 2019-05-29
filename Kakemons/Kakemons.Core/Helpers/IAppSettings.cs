using System.Threading.Tasks;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Helpers
{
    public interface IAppSettings
    {
        Task<UserDto> GetUser();
        Task<bool> SetUser(UserDto user);
    }
}
