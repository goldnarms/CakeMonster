using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Common.Models;
using Kakemons.Common.Parameters;
using Kakemons.Common.Responses;
using Kakemons.SDK.ApiContracts;

namespace Kakemons.SDK.FakeApiServices
{
    public class FakeUserApiService : IUserApiService
    {
        private UserDto _testUser = new UserDto("Cookie", "Monster") { Id = "1", IsDeleted = false, Position = new DbPosition(), AvatarUrl = "https://images.pexels.com/photos/1123401/pexels-photo-1123401.jpeg?cs=srgb&dl=adolescent-adult-attractive-1123401.jpg&fm=jpg" };

        private readonly IEnumerable<CakeDto> _testCakes = new List<CakeDto>
        {
            new CakeDto
            {
                Baker = new BakerDto { FirstName = "Baker", LastName = "Hansen", Id = "2", IsDeleted = false },
                Id = 1,
                Availability = CakeAvailability.Now,
                Name = "Sjokoladekake",
                Description = "Nam nam",
                BakerId = "2"
            }
        };

        public Task<IEnumerable<CakeDto>> GetFavourites(string userId)
        {
            return Task.FromResult(_testCakes);
        }

        public Task<UserDto> GetUser()
        {
            return Task.FromResult(_testUser);
        }

        public Task<RegisterResult> RegisterUser(RegisterUserParameter registerUserParameter)
        {
            var result = new RegisterResult()
            {
                IsSuccessful = true,
                User = _testUser
            };
            return Task.FromResult(result);
        }
    }
}
