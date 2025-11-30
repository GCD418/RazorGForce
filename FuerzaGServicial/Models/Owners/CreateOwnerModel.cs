namespace FuerzaGServicial.Models.Owners;

public class CreateOwnerModel
{
    public string Name { get; set; } = string.Empty;
    public string FirstLastname { get; set; } = string.Empty;
    public string? SecondLastname { get; set; }
    public int PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string? DocumentExtension { get; set; }
    public string Address { get; set; } = string.Empty;

    public int UserId { get; set; }
}
