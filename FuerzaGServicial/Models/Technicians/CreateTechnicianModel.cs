namespace FuerzaGServicial.Models.Technicians
{
    public class CreateTechnicianModel
    {
        public string Name { get; set; } = string.Empty;
        public string FirstLastName { get; set; } = string.Empty;
        public string? SecondLastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? DocumentExtension { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal BaseSalary { get; set; }

        public int UserId { get; set; }
    }
}
