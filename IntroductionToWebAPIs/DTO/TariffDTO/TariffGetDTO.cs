namespace IntroductionToWebAPIs.DTO.TariffDTO
{
    public class TariffGetDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }
}
