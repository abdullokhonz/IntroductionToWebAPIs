using AutoMapper;
using IntroductionToWebAPIs.DTO.TariffDTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Mapping
{
    public class TariffProfile : Profile
    {
        public TariffProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Tariff, TariffGetDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
