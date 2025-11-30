namespace FuerzaGServicial.Models.Technicians
{
    public class TechnicianModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstLastName { get; set; } = string.Empty;
        public string? SecondLastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? DocumentExtension { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ModifiedByUserId { get; set; }

        public string FullName => $"{FirstLastName} {(SecondLastName ?? string.Empty)} {Name}";
        public string FullDocumentNumber => string.IsNullOrWhiteSpace(DocumentExtension) ? DocumentNumber : $"{DocumentNumber}-{DocumentExtension}";
    }
}
