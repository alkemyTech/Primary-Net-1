using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column ("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Column("last_name")]
        [StringLength(20)]
        public string LastName { get; set;} = null!;

        [Required]
        [StringLength(50)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [Column("password")]
        public string Password { get; set; } = null!;

        [Required]
        [Column("points")]
        public double Points { get; set; }
        
        [ForeignKey(name: "RolId")]
        [NotMapped]
        public Role Role { get; set; }

        [Column("rol_Id")]
        public int RolId { get; set; }

    }
}
