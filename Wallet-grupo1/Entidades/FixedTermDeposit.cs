using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wallet_grupo1.Entidades
{
    [Table("FixedTermDeposits")]
    public class FixedTermDeposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("product_description")]
        [StringLength(250)]
        public string ProductDescription { get; set; } = null!;

        [Required]
        [Column("image")]
        [StringLength(250)]
        public string Image { get; set; } = null!;

        [Required]
        [Column("points")]
        public double Points { get; set; }
    }
}