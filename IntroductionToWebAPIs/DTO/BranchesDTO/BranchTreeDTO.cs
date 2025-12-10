using IntroductionToWebAPIs.Entity.Components;

namespace IntroductionToWebAPIs.DTO.BranchesDTO
{
    public class BranchTreeDTO : IBranchComponent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public List<BranchTreeDTO> Children { get; set; } = new();

        public void AddSub(IBranchComponent branch)
        {
            Children.Add((BranchTreeDTO)branch);
        }

        public void RemoveSub(IBranchComponent branch)
        {
            Children.Remove((BranchTreeDTO)branch);
        }

        public IEnumerable<IBranchComponent> GetChildren() => Children;
    }
}
