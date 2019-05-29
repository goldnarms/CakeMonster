using System.Collections.Generic;

namespace Kakemons.Common.Dtos
{
    public class CustomerDto: UserDto
    {
        public IEnumerable<CakeDto> Favourites { get; set; }
    }
}
