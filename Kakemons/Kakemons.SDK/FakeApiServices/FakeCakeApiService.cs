using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Common.Models;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;

namespace Kakemons.SDK.FakeApiServices
{
    public class FakeCakeApiService : ICakeApiService
    {
        private readonly IBakerApiService _bakerApiService;

        public FakeCakeApiService(IBakerApiService bakerApiService)
        {
            _bakerApiService = bakerApiService;
        }

        private readonly IEnumerable<CakeDto> _testCakes = new List<CakeDto>
        {
            new CakeDto
            {
                Baker = new BakerDto { FirstName = "Baker", LastName = "Hansen", Id = "1", IsDeleted = false, AvatarUrl = "https://www.pexels.com/photo/man-in-crew-neck-shirt-555790/"},
                Id = 3,
                Availability = CakeAvailability.Now,
                Name = "Sjokoladekake",
                Description = "Nam nam",
                BakerId = "1",
                Price = 200,
                Images = new List<ImageUrlDto>{ new ImageUrlDto { Url = "https://images.pexels.com/photos/697571/pexels-photo-697571.jpeg?auto=compress&cs=tinysrgb&h=350" } },
                Allergens = new List<AllergenDto> { new AllergenDto { Id = 1, Name = "Nøtter", Color = "#163965" }, new AllergenDto { Id = 2, Name = "Gluten", Color = "#551925" } },
                Tags = new List<TagDto> { new TagDto() { Id = 2, Name = "Søt"} },
                CakeType = CakeType.ChocolateCake
            },
            new CakeDto
            {
                Baker = new BakerDto { FirstName = "Test", LastName = "Testesen", Id = "2", IsDeleted = false, AvatarUrl = "https://images.pexels.com/photos/1462980/pexels-photo-1462980.jpeg?auto=compress&cs=tinysrgb&h=750&w=1260"},
                Id = 4,
                Availability = CakeAvailability.Now,
                Name = "Ostekake",
                Description = "Aw me gad so delish",
                BakerId = "2",
                Price = 300,
                Images = new List<ImageUrlDto>{ new ImageUrlDto { Url = "https://images.pexels.com/photos/140831/pexels-photo-140831.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=350" } },
                Allergens = new List<AllergenDto> { new AllergenDto { Id = 3, Name = "Melk", Color = "#447722"} },
                Tags = new List<TagDto> { new TagDto() { Id = 3, Name = "Syrlig"} },
                CakeType = CakeType.CheeseCake
            },
            new CakeDto
            {
                Baker = new BakerDto { FirstName = "Testine", LastName = "Bakersen", Id = "3", IsDeleted = false, AvatarUrl = "https://images.pexels.com/photos/835912/pexels-photo-835912.jpeg?auto=compress&cs=tinysrgb&h=750&w=1260"},
                Id = 5,
                Availability = CakeAvailability.ForOrder,
                Name = "Smashkake",
                Description = "Aw me gad so delish",
                BakerId = "3",
                Price = 500,
                Images = new List<ImageUrlDto>{ new ImageUrlDto { Url = "https://images.pexels.com/photos/140831/pexels-photo-140831.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=350" } },
                Allergens = new List<AllergenDto> { new AllergenDto { Id = 3, Name = "Melk", Color = "#447722"} },
                Tags = new List<TagDto> { new TagDto() { Id = 2, Name = "Søt"} },
                CakeType = CakeType.ChocolateCake
            }
        };

        public Task<IEnumerable<CakeDto>> GetNearbyCakes(DbPosition argPosition)
        {
            return Task.FromResult(_testCakes);
        }
    }
}
