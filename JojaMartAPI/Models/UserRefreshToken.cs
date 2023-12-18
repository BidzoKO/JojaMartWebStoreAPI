using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("user_refresh_tokens")]
public partial class UserRefreshToken
{
    [Key]
    public int Id { get; set; }

    [Column("user_Id")]
    public int UserId { get; set; }

    [Column("refresh_token")]
    [StringLength(1000)]
    [Unicode(false)]
    public string RefreshToken { get; set; } = null!;

    [ForeignKey("Id")]
    [InverseProperty("UserRefreshToken")]
    public virtual User IdNavigation { get; set; } = null!;
}
