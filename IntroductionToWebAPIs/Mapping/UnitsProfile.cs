using AutoMapper;
using IntroductionToWebAPIs.DTO.UnitsDTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Mapping
{
    public class UnitsProfile : Profile
    {
        public UnitsProfile()
        {
            // Создание: UnitsCreateDto → Units
            CreateMap<UnitsCreateDTO, Units>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Чтение: Units → UnitsGetDto
            CreateMap<Units, UnitsGetDTO>();
        }
    }
}
