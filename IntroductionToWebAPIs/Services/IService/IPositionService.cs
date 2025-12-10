using IntroductionToWebAPIs.DTO.PositionsDTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IPositionService : IBaseService<Position>
    {
        Task<IEnumerable<PositionTreeDTO>> GetPositionTreeAsync();

        Task<Position> CreatePositionAsync(PositionCreateDTO dto);
    }
}
