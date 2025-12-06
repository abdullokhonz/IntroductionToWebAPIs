using IntroductionToWebAPIs.DTO.TariffDTO;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Tariff.GetAll
{
    public class GetTariffQueryHandler
        : IRequestHandler<GetTariffQuery, IEnumerable<TariffGetDTO>>
    {
    }
}
