using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Entidades;

[Table("Catalogues")]
public class Catalogue
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