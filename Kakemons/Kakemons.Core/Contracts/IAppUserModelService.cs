using System;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Contracts
{
    public interface IAppUserModelService
    {
        string UserId { get; }
        UserDto User { get; }
        bool IsLoggedIn { get; }
        SourceCache<CakeDto, int> FavouriteCakes { get; }
        Task LogInUser(UserDto user, string accessToken);
        Task LogOutUser();
        IObservable<UserDto> UserObservable { get; }
        Task<Tuple<string, string>> GetToken();
        bool IsNewUser { get; }
        void LogInUser(UserDto user);
        void AddToFavorites(int cakeId, CakeDto cake);
        void RemoveFromFavorites(int cakeId);
    }
}
