namespace FuerzaGServicial.Models.Services
{
    public class ServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public decimal? Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public int? ModifiedByUserId { get; set; }

        public int? CreatedByUserId { get; set; }
        public decimal AccumulatedRevenue { get; set; } = 0.00m;
    }
}
