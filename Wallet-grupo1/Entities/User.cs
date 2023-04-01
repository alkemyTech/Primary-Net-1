using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;
using Wallet_grupo1.Helpers;

namespace Wallet_grupo1.Entities;

public class User
{
    [Column("user_id")] public int Id { get; set; }

    [Required]
    [Column("user_firstName", TypeName = "VARCHAR(100)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column("user_lastName", TypeName = "VARCHAR(100)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column("user_email", TypeName = "VARCHAR(100)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("user_password", TypeName = "VARCHAR(64)")]
    public string Password { get; set; } = null!;

    [Column("user_points")] public int Points { get; set; }

    [Required] [Column("role_id")] 
    public int RoleId { get; set; }
    public Role? Role { get; set; }

    [NotMapped] public Account? Account { get; set; }


    public User(RegisterDto dto)
    {
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Password = PasswordEncryptHelper.EncryptPassword(dto.Password);
        Email = dto.Email;
        RoleId = 1;
        Points = 0;
    }

    public User(){}
}