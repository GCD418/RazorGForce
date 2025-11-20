namespace FuerzaGServicial.ModelsD.Owners
{
    public class OwnerModelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstLastname { get; set; } = string.Empty;
        public string? SecondLastname { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? DocumentExtension { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string FullName =>
            $"{FirstLastname} {SecondLastname} {Name}".Trim();

        public string FullDocumentNumber =>
            string.IsNullOrWhiteSpace(DocumentExtension)
                ? DocumentNumber
                : $"{DocumentNumber}-{DocumentExtension}";
    }
}
