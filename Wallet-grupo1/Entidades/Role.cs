using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(20)]
        public string Name { get; set; } = null!;
        
        [Column("description")]
        [StringLength(250)]
        public string Description { get; set; } = null!;
    }

    public enum RoleType
    {
        Admin,
        Regular
    }
}
