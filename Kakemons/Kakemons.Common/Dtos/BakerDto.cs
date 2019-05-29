using System.Collections.Generic;

namespace Kakemons.Common.Dtos
{
    public class BakerDto: UserDto
    {        
        public IEnumerable<CakeDto> Cakes { get; set; }
        public AddressDto Address { get; set; }
        public string AvatarUrl { get; set; }
        public string Description { get; set; }
    }
}
