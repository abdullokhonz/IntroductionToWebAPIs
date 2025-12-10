using AutoMapper;
using IntroductionToWebAPIs.DTO.UnitsDTO;
using IntroductionToWebAPIs.Services.IService;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Units.GetAll
{
    public class GetAllUnitsQueryHandler : IRequestHandler<GetAllUnitsQuery, IEnumerable<UnitsGetDTO>>
    {
        private readonly IUnitsService _unitsService;
        private readonly IMapper _mapper;

        public GetAllUnitsQueryHandler(IUnitsService unitsService, IMapper mapper)
        {
            _unitsService = unitsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitsGetDTO>> Handle(GetAllUnitsQuery request, CancellationToken ct)
        {
            var response = await _unitsService.GetAllAsync(ct); // ServiceResponse<IEnumerable<Units>>

            // Проверяем ответ сервиса
            if (response == null || !response.Success || response.Data == null)
            {
                // Вариант поведения: вернуть пустой список
                // Можно также бросать исключение или логировать ошибку — по твоему выбору.
                return Enumerable.Empty<UnitsGetDTO>();
            }

            // Мапим именно данные (IEnumerable<Units>) -> IEnumerable<UnitGetDTO>
            var dto = _mapper.Map<IEnumerable<UnitsGetDTO>>(response.Data);
            return dto;
        }
    }
}



/*
using AutoMapper;
using IntroductionToWebAPIs.DTO.UnitsDTO;
using IntroductionToWebAPIs.Services.IService;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Units.GetAll
{
    public class GetAllUnitQueryHandler : IRequestHandler<GetAllUnitQuery, IEnumerable<UnitGetDTO>>
    {
        private readonly IUnitService _unitsService;
        private readonly IMapper _mapper;

        public GetAllUnitQueryHandler(IUnitService unitsService, IMapper mapper)
        {
            _unitsService = unitsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitGetDTO>> Handle(GetAllUnitQuery request, CancellationToken ct)
        {
            var response = await _unitsService.GetAllAsync(ct); // ServiceResponse<IEnumerable<Units>>

            // Проверяем ответ сервиса
            if (response == null || !response.Success || response.Data == null)
            {
                // Вариант поведения: вернуть пустой список
                // Можно также бросать исключение или логировать ошибку — по твоему выбору.
                return Enumerable.Empty<UnitGetDTO>();
            }

            // Мапим именно данные (IEnumerable<Units>) -> IEnumerable<UnitGetDTO>
            var dto = _mapper.Map<IEnumerable<UnitGetDTO>>(response.Data);
            return dto;
        }
    }
}
*/