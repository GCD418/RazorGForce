namespace FuerzaGServicial.Models.Services
{
    public class CreateServiceModel
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
