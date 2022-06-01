using AutoMapper;
using Diia.Challenge.Lib;

namespace Diia.Challenge.Maps
{
    public class AdressProfile : Profile
    {
        public AdressProfile()
        {
            CreateMap<Address, AddressForSql>();
            CreateMap<Address, Application>()
                .ForMember(p => p.Street, p => p.MapFrom(src => src.StreetId))
                .ForMember(p => p.City, p => p.MapFrom(src => src.CityId))
                .ForMember(p => p.District, p => p.MapFrom(src => src.CityDistrictId));
        }
    }
}
