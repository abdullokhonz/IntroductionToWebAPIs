namespace IntroductionToWebAPIs.Entity.Components
{
    public interface IPositionComponent
    {
        string Name { get; }
        string? Description { get; }
        void AddSub(IPositionComponent position);
        void RemoveSub(IPositionComponent position);
        IEnumerable<IPositionComponent> GetChildren();
    }
}
