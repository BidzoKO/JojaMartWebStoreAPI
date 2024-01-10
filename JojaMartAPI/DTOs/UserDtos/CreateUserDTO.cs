using System.ComponentModel.DataAnnotations;


public class CreateUserDTO
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; }

    [Required]
    public DateTime Dob { get; set; }

    [Required]
    [StringLength(1)]
    public string Gender { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(15)]
    public string? CallingCode { get; set; }
}


