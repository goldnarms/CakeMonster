using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Helpers
{
    public class AppSettings : IAppSettings
    {
        readonly IBlobCache _blobCache;

        public AppSettings(IBlobCache blobCache)
        {
            _blobCache = blobCache;
        }

        public async Task<UserDto> GetUser()
        {
            try
            {
                return await _blobCache.GetObject<UserDto>($"current_user").ToTask();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SetUser(UserDto user)
        {
            try
            {
                await _blobCache.InsertObject($"current_user", user).ToTask();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
