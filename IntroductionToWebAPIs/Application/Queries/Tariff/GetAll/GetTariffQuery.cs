using IntroductionToWebAPIs.DTO.TariffDTO;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Tariff.GetAll
{
    public class GetTariffQuery : IRequest<IEnumerable<TariffGetDTO>>
    {
    }
}
