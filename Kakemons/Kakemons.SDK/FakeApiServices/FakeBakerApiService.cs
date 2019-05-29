using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.SDK.ApiContracts;
using Kakemons.SDK.ApiServiceContracts;

namespace Kakemons.SDK.FakeApiServices
{
    public class FakeBakerApiService : IBakerApiService
    {
        public FakeBakerApiService()
        {

        }

        public async Task<BakerDto> GetBaker(string id)
        {
            return await Task.FromResult(_testBakers.SingleOrDefault(b => b.Id == id));
        }

        private readonly IEnumerable<BakerDto> _testBakers = new List<BakerDto>
        {
            new BakerDto { FirstName = "Baker", LastName = "Hansen", Id = "1", IsDeleted = false, AvatarUrl = "https://images.pexels.com/photos/555790/pexels-photo-555790.png?auto=compress&cs=tinysrgb&h=750&w=1260", Address = new AddressDto {Street = "Østre Moholt-tun 7", City = "Trondheim"}},
            new BakerDto { FirstName = "Test", LastName = "Testesen", Id = "2", IsDeleted = false, AvatarUrl = "https://images.pexels.com/photos/1462980/pexels-photo-1462980.jpeg?auto=compress&cs=tinysrgb&h=750&w=1260", Address = new AddressDto {Street = "Brøsetvegen 75b", City = "Trondheim"}},
            new BakerDto { FirstName = "Testine", LastName = "Bakersen", Id = "3", IsDeleted = false, AvatarUrl = "https://images.pexels.com/photos/835912/pexels-photo-835912.jpeg?auto=compress&cs=tinysrgb&h=750&w=1260", Address = new AddressDto {Street = "Testheim 7", City = "Trondheim"}},
        };

    }
}
