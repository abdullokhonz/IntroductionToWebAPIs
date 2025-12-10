using IntroductionToWebAPIs.DTO.UnitsDTO;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Units.GetAll
{
    public class GetAllUnitsQuery : IRequest<IEnumerable<UnitsGetDTO>>
    {
    }
}
