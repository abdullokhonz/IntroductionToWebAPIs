namespace IntroductionToWebAPIs.Entity.Components
{
    public interface IBranchComponent
    {
        string Name { get; }
        string? Description { get; }
        string? Location { get; }
        void AddSub(IBranchComponent branch);
        void RemoveSub(IBranchComponent branch);
        IEnumerable<IBranchComponent> GetChildren();
    }
}
