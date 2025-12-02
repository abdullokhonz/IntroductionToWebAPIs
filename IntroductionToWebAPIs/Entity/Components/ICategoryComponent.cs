namespace IntroductionToWebAPIs.Entity.Components
{
    public interface ICategoryComponent
    {
        string Name { get; }
        void AddSub(ICategoryComponent category);
        void RemoveSub(ICategoryComponent category);
        IEnumerable<ICategoryComponent> GetChildren();
    }
}
