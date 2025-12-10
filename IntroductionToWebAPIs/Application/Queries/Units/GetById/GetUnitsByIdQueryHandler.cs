using AutoMapper;
using IntroductionToWebAPIs.DTO.UnitsDTO;
using IntroductionToWebAPIs.Services.IService;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Units.GetById
{
    public class GetUnitsByIdQueryHandler : IRequestHandler<GetUnitsByIdQuery, UnitsGetDTO?>
    {
        private readonly IUnitsService _unitsService;
        private readonly IMapper _mapper;

        public GetUnitsByIdQueryHandler(IUnitsService unitsService, IMapper mapper)
        {
            _unitsService = unitsService;
            _mapper = mapper;
        }

        public async Task<UnitsGetDTO?> Handle(GetUnitsByIdQuery request, CancellationToken ct)
        {
            var response = await _unitsService.GetByIdAsync(request.Id, ct); // ServiceResponse<Units?>

            if (response == null || !response.Success || response.Data == null)
                return null;

            var dto = _mapper.Map<UnitsGetDTO>(response.Data);
            return dto;
        }
    }
}
