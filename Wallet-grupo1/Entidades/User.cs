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
        public string first_name { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string last_name { get; set;} = null!;

        [Required]
        [StringLength(50)]
        public string email { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string password { get; set; } = null!;

        [Required]
        public double points { get; set; }

        [Required]
        [ForeignKey(name: "Role")]
        public int rol_Id { get; set; }

    }
}
