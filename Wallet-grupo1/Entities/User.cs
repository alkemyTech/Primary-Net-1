using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entities
{
    public class User
    {
        [Column("user_id")]
        public int Id { get; set; }

        [Required]
        [Column("user_firstName", TypeName = "VARCHAR(100)")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Column("user_lastName", TypeName = "VARCHAR(100)")]
        public string LastName { get; set;} = null!;  
        
        [Required]
        [Column("user_email", TypeName = "VARCHAR(100)")]
        public string Email { get; set; } = null!;  

        [Required]
        [Column("user_password", TypeName = "VARCHAR(30)")]
        public string Password { get; set; } = null!;
        
        [Column("user_points")]
        public int Points { get; set; }
        
        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        
        [NotMapped]
        public Account Account { get; set; }

    }
}
