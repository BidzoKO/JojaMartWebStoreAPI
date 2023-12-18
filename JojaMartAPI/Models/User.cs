using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("users")]
[Index("Email", Name = "UQ__users__AB6E6164BAE3FE3F", IsUnique = true)]
[Index("Username", Name = "UQ__users__F3DBC5722275F697", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [Column("dob", TypeName = "date")]
    public DateTime Dob { get; set; }

    [Column("gender")]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("address")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Address { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [Column("calling_code")]
    [StringLength(15)]
    [Unicode(false)]
    public string? CallingCode { get; set; }

    [Column("registration_date", TypeName = "date")]
    public DateTime RegistrationDate { get; set; }

    [Column("last_login_date", TypeName = "datetime")]
    public DateTime LastLoginDate { get; set; }

    [Column("account_status")]
    [StringLength(1)]
    [Unicode(false)]
    public string AccountStatus { get; set; } = null!;

    [Column("profile_picture_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ProfilePictureUrl { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual UserRefreshToken? UserRefreshToken { get; set; }
}
