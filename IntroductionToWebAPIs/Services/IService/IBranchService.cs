using IntroductionToWebAPIs.DTO.BranchesDTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IBranchService : IBaseService<Branch>
    {
        Task<IEnumerable<BranchTreeDTO>> GetBranchTreeAsync();
        Task<Branch> CreateBranchAsync(BranchCreateDTO dto);
    }
}
