using System;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Models;
using Kakemons.Common.Responses;

namespace Kakemons.Core.Contracts
{
    public interface ISettingsService
    {
        string BaseApiUrl { get; }
        Task<AuthTokens> GetToken();
        Task SetNewToken();
        Task Logout();
        Task Login(LoginUserResponse accessToken);
    }
}