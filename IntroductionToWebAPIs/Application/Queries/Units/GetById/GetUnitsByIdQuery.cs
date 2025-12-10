using IntroductionToWebAPIs.DTO.UnitsDTO;
using MediatR;

namespace IntroductionToWebAPIs.Application.Queries.Units.GetById
{
    public class GetUnitsByIdQuery : IRequest<UnitsGetDTO?>
    {
        public Guid Id { get; set; }

        public GetUnitsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
