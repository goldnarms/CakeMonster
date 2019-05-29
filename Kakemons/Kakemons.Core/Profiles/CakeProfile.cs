using AutoMapper;
using Kakemons.Common.Dtos;
using Kakemons.Core.ListView;

namespace Kakemons.Core.Profiles
{
    public class CakeProfile : Profile
    {
        public CakeProfile()
        {
            CreateMap<CakeDto, CakeListItemViewModel>()
                ;
        }
    }
}
