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

        // Propiedades auxiliares opcionales para el front
        public string DisplayPrice => Price.HasValue ? $"{Price:C}" : "No definido";
        public string Status => IsActive ? "Activo" : "Inactivo";
    }
}
